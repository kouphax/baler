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
        IBale Before(Func<string,string,string> processor, int order = 0);

        // Adds a function to execute on the concatenated bale content.  
        // It gets the contents and returns the transformed content
        IBale After(Func<string, string> processor, int order = 0);
    }
}
