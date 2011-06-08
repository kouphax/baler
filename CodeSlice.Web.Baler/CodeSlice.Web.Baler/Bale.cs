using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CodeSlice.Web.Baler
{
    // A bale represents a single definition of a collection of files that 
    // should be baled together.  A bale is responsible for processing its
    // items and outputting the bale file and associated HTML tag.  Currently
    // supports JavaScript and CSS.  No intention for this to change :-)
    public class Bale : IBale
    {
        // ###  Private variable declarations

        // Declare the templates used to output the html tags at the end
        private static readonly string SCRIPT_TAG_TPL = @"<script type='text/javascript' src='{0}' {1}></script>";
        private static readonly string STYLE_TAG_TPL = @"<link href='{0}' rel='stylesheet' type='text/css' {1}/>";

        // Hold the references to the bale source items
        private string[] _items;

        // Before and After lists are sorted.  This allows for us to control 
        // the ordering of the actions.  For example Minification should almost 
        // always be performed at the end (e.g when translating from 
        // coffeescript).  Default ordering will always be 0.
        private SortedList<int, Func<string, string, string>> _before;
        private SortedList<int, Func<string, string>> _after; 

        // Holds all the custom attributes to be appended to the output tag
        private List<string> _attrs;

        // Hold flag and result to ensure that a bale is only generated once
        // Currently this presents an issue.  If we generate a JS bale first 
        // then use the same items to generate a CSS bale this will return 
        // the old result.  If this happens you've done it wrong in the first 
        // place.
        private bool _isGenerated = false;
        private string _generatedTag;

        // ### Contructors

        // A bale can only be built by Baler and by passing the list of 
        // parameters that represent the bale items.
        internal Bale(params string[] items)
        {
            _items = items;
            _before = new SortedList<int, Func<string, string, string>>();
            _after = new SortedList<int, Func<string, string>>();
            _attrs = new List<string>();

            // set the key based on bale contents
            Key = Bale.GenerateKey(items);
        }

        // ###  Public Methods

        // `AsJs()` is a convenience method for generating the bale output 
        // file and returning the script tag pointing to that file
        public string AsJs()
        {
            return GenerateBale("js", SCRIPT_TAG_TPL);
        }

        // `AsCss()` is a convenience method for generating the bale output 
        // file and returning the css link tag pointing to that file
        public string AsCss()
        {
            return GenerateBale("css", STYLE_TAG_TPL);
        }

        // Adds a function to execute on each item of the bale prior to being 
        // merged.  It gets the path and the contents and returns the 
        // transformed content
        public IBale Before(Func<string, string, string> processor, int order = 0)
        {
            _before.Add(order, processor);
            return this;
        }

        // Adds a function to execute on the concatenated bale content.  
        // It gets the contents and returns the transformed content
        public IBale After(Func<string, string> processor, int order = 0)
        {
            _after.Add(order, processor);
            return this;
        }

        // Adds a custom attribute to the output tag for this bale e.g.
        //
        //     bale.Attr("media", "screen").AsCss();
        //
        // should produce a link tag like so
        //
        //    <link rel="stylesheet" type="text/css" href="..." media="screen" /> 
        public IBale Attr(string name, string value)
        {
            _attrs.Add(string.Format("{0}='{1}'", name, value));
            return this;
        }

        // ### Public Properties

        // Holds the key for this bale that can be used to uniquely 
        // identify this bale based in it's initial contents
        public string Key
        {
            get;
            private set;
        }

        // ### Static Methods

        // Calculates a `key` for the current bale by joining all the 
        // required paths of items and creating a hash.  This means we
        // can uniquely identify a set of items as a bale e.g. for caching
        public static string GenerateKey(params string[] items)
        {
            string key = string.Join(string.Empty, items);
            byte[] buffer = Encoding.UTF8.GetBytes(key);
            byte[] hashBytes = new MD5CryptoServiceProvider().ComputeHash(buffer);

            StringBuilder output = new StringBuilder(hashBytes.Length);
            for (int i = 0; i < hashBytes.Length; i++)
            {
                output.Append(hashBytes[i].ToString("X2"));
            }

            return output.ToString();
        }

        // ###  Private Methods
        
        // `GenerateBale` is the primary method of a bale.  It is 
        // responbile for calculating the output file name and directory, 
        // concatenating and processing the input file and piping them out to 
        // the output directory.  A bale extension `js` or `css` need 
        // specififed for completeness and the tag template will be sued to 
        // generate this html tag
        private string GenerateBale(string extension, string template)
        {
            // check if file has been previously generated and don't generate
            // again if this is the case
            if (!_isGenerated)
            {
                // Build output file based on a random name and REAL path to output directory
                string outputFilename = string.Format("{0}.{1}", Path.GetRandomFileName(), extension);
                string outputFile = GetOutputFileWithPath(outputFilename);

                // Calculate relative values for the path so we can output the tag
                string relativeOutputFile = GetRelativeOutputPath(outputFilename);

                // Create the script tag from the path and the supplied template
                // and a join of all custom attributes
                string attrs = string.Join(" ", _attrs);
                string tag = string.Format(template, relativeOutputFile, attrs);

                // Copy all files into a single output file
                ConcatenateAllFiles(outputFile);

                _generatedTag = tag;
                _isGenerated = true;
            }

            return _generatedTag;
        }
        
        // `GetOutputFileWithPath()` takes a filename of an output and resolves 
        // the full physical path using the Baler classes configuration for 
        // the `OutputPath`. 
        private string GetOutputFileWithPath(string filename)
        {
            string outputPath = HttpContext.Current.Server.MapPath(Baler.Configuration.OutputPath);
            string outputFile = Path.Combine(outputPath, filename);

            return outputFile;
        }

        // Similar to `GetOutputFileWithPath` except rather than output the 
        // physical path to a file it determines the path relative to the root
        // of the web application
        private string GetRelativeOutputPath(string filename)
        {
            string outputRelativePath = Baler.Configuration.OutputPath.Replace("~/", HttpRuntime.AppDomainAppVirtualPath);
            string relativeOutputFile = outputRelativePath + "/" + filename;

            return relativeOutputFile;
        }

        // Iterates over the current bales items and processes and 
        // concatenates them into the passed in output file
        private void ConcatenateAllFiles(string outputFile)
        {
            StringBuilder mergedItems = new StringBuilder();

            // Loop over each script reference in the bale collection
            foreach (string script in _items)
            {
                // Derive physical path to the current script source
                string scriptFile = HttpContext.Current.Server.MapPath(script);

                // Read the contents of the input file
                string contents = File.ReadAllText(scriptFile);

                // Perform some pre-processing of items if a before function 
                // has been specified
                foreach (Func<string,string,string> before in _before.Values)
                {
                    contents = before(script, contents);
                }

                // append contents of this item to the ouput file
                mergedItems.AppendLine(contents);
            }

            string outputContent = mergedItems.ToString();

            // Perform some post-processing of bale if an after function has 
            // been specified
            foreach (Func<string,string> after in _after.Values)
            {
                outputContent = after(outputContent);
            }

            File.WriteAllText(outputFile, outputContent);
        }
    }
}
