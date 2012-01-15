using System;

namespace CodeSlice.Web.Baler
{
    // Describes the public view of returned bales.  Allows for generation 
    // of JS and CSS Bales
    public interface IBale
    {
        // Create bale as a JavaScript bale and return the `<script />` tag
        string AsJs();

        // Create bale as a CSS bale and return the `<link />` tag
        string AsCss();

        // Adds a function to execute on each item of the bale prior to being 
        // merged.  It gets the path and the contents and returns the 
        // transformed content
        IBale Before(Func<string,string,string> processor, int order);
        IBale Before(Func<string, string, string> processor);

        // Adds a function to execute on the concatenated bale content.  
        // It gets the contents and returns the transformed content
        IBale After(Func<string, string> processor, int order);
        IBale After(Func<string, string> processor);

        // Adds a custom attribute to the output tag for this bale e.g.
        //
        //     bale.Attr("media", "screen").AsCss();
        //
        // should produce a link tag like so
        //
        //    <link rel="stylesheet" type="text/css" href="..." media="screen" /> 
        IBale Attr(string name, string value);
        IBale Attr(string name);
    }
}
