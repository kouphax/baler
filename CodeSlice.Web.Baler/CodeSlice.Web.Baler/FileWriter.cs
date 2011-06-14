using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CodeSlice.Web.Baler
{
    internal class FileWriter : IDisposable
    {
        public string OutputFile { private get; set; }
        public string Contents { private get; set; }
        
        private bool _overwrite { get; set;}

        public FileWriter(bool overwrite)
        {
            _overwrite = overwrite;
        }

        public void Dispose()
        {            
            if (_overwrite || (!_overwrite && !File.Exists(OutputFile)))
            {
                File.WriteAllText(OutputFile, Contents);
            }
        }
    }
}
