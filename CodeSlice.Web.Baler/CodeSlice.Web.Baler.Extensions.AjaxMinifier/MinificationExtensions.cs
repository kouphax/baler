using Microsoft.Ajax.Utilities;

namespace CodeSlice.Web.Baler.Extensions.AjaxMinifier
{
    // Static class that is used to provide the extension methods to support 
    // minifying the bales using Microsofts [Ajax Minfier](http://ajaxmin.codeplex.com/)
    // [Ajax Minfier](http://ajaxmin.codeplex.com/) supports both JavaScript
    // and Stylesheets/CSS
    public static class MinificationExtensions
    {
        // Keep a static reference to the minifier engine
        private static Minifier _minfier = new Minifier();

        public static IBale MinifyJavaScript(this IBale bale)
        {
            // Add `MinifyJavaScript` action with 999 to ensure that it is 
            // executed as the last action
            return bale.After(_minfier.MinifyJavaScript, 999);
        }

        public static IBale MinifyStyleSheet(this IBale bale)
        {
            // Add `MinifyStyleSheet` action with 999 to ensure that it is 
            // executed as the last action
            return bale.After(_minfier.MinifyStyleSheet, 999);
        }
    }
}
