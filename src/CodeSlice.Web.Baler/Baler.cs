using System.Collections.Generic;

namespace CodeSlice.Web.Baler
{
    // The Baler class represents the entry point for defining *bales* 
    // that can be embedded on a web page
    public static class Baler
    {
        private static readonly Dictionary<string, Bale> _cache = new Dictionary<string, Bale>();

        // ### Constructors
        
        // Responsible for creating the default configuration object including
        // setting
        //
        // * `OutputPath` - the location of the actual generated bales 
        //   relative to the web project root (defaults to `~/Scripts`)
        static Baler()
        {
            Configuration = new BalerConfiguration
            {
                OutputPath = "~/Scripts"
            };
        }

        // ### Public Methods
        
        // The build method allows us to define and return a bale.  It 
        // accepts a list as relative urls, relative to the application root
        // such as `~/Scripts/script.js` or `~/Styles/style.css` and returns
        // the instanstiated bale that can be further configured.
        public static IBale Build(params string[] items)
        {
            // Check cache to see if bale is currently defined and return 
            // cached value otherwise create a new bale and cache it
            string key = Bale.Hash(items);
            if (!_cache.ContainsKey(key))
            {
                _cache[key] = new Bale(items);
            }

            return _cache[key];
        }

        // ### Public Properties
        
        // Provides an entry point into the Baler configuration.
        public static BalerConfiguration Configuration { get; set; }
    }
}
