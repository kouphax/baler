using System.Collections.Generic;
using System;

namespace CodeSlice.Web.Baler.Extensions.NamedBales
{
    // `NamedBale` extensions provides a mechanism to declare a Bale definition 
    // up front and reuse the same bale throughout the application without
    // having to redefine the contents over and over.  For example we can define 
    // a bale with 3 scripts and name it
    //
    //     Baler.Build(
    //       "script1.js", 
    //       "script2.js", 
    //       "script3.js"
    //     ).NameAs("base")
    //
    // This can returns the Bale itself so can be used inline or somewhere like 
    // `Global.asax`.  Next we can render this bundle using `AsJs` later in the 
    // app like so,
    //     
    //     NamedBale.Called("base").AsJs();
    //
    // Currently this doesn't short circuit Balers internal cache so a second 
    // cache check will be made using the internal hashing mechanism.  This is
    // either a good thing or a bad thing.  But for now it's a good thing!
    public static class NamedBales
    {
        // Internal cache for named bundle keys
        private static readonly Dictionary<string, IBale> _cache = new Dictionary<string, IBale>();

        // `NameAs` adds some sugar to the `IBale` interface allowing us to apply 
        // a friendly name to a bale.  There is currently no check to see if the
        // bale name is already taken.  Existing definitions will be 
        // overwritten.
        public static IBale NameAs(this IBale bale, string name)
        {
            _cache[name] = bale;
            return bale;
        }

        // Allows us to retrieve a bale based on a friendly name defined by the 
        // developer.  Will throw an exception if the bale doesn't exist - otherwise 
        // the bale itself.
        public static IBale Get(string name)
        {
            if(_cache.ContainsKey(name))
            {
                return _cache[name];
            }

            throw new Exception(string.Format("No Bale exists with the name of {0}", name));
        }
    }
}
