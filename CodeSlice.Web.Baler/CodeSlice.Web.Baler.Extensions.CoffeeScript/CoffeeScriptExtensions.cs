using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSlice.Web.Baler.Extensions.CoffeeScript
{
    // Static class that is used to provide the extension method to allow 
    // processing the bale as a [CoffeeScript](http://jashkenas.github.com/coffee-script/)
    // bale
    public static class CoffeeScriptExtensions
    {
        // Apply CoffeeScript processing to the bale after concatenation and 
        // render as JavaScript
        public static string AsCoffeeScript(this IBale bale)
        {
            return bale.After(CoffeeScriptProcessor.Process).AsJs();
        }
    }
}
