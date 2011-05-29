using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jurassic;

namespace CodeSlice.Web.Baler.Extensions.CoffeeScript
{
    // CoffeeScript Processor can take coffeescript and produce valid JavaScript.
    // It currently utilises [Jurassic](http://jurassic.codeplex.com) and the 
    // standalone [CoffeeScript](http://coffeescript.org) compiler to do the 
    // conversion
    internal class CoffeeScriptProcessor
    {
        // JavaScript function used to execute the coffeescript compiler
        private static readonly string COMPILE_TASK = "CoffeeScript.compile(Source, {bare: true})";

        // References to the shared Jurassic Engine.  `ThreadStatic` because 
        // Jurassic isn't threadsafe
        [ThreadStatic]
        private static ScriptEngine _engine;
        private static ScriptEngine Engine
        {
            get
            {
                // Only intialise the coffeescript compiler if it hasn't be 
                // done on this thread already.  This is the bottleneck and we 
                // need to address this in a more effective and safe way.
                if (_engine == null)
                {
                    _engine = new ScriptEngine();
                    _engine.Execute(Scripts.CoffeeScript);
                }

                return _engine;
            }
        }

        // Perform compilation of the CoffeeScript by passing the current 
        // contents into the engine an invking the `COMPILE_TASK` scriptlet
        public static string Process(string contents)
        {
            Engine.SetGlobalValue("Source", contents);
            return Engine.Evaluate<string>(COMPILE_TASK);   
        }
    }
}