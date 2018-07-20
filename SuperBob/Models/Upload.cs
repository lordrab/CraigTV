using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace SuperBob.Models
{
    public class Upload
    {
        public string contentType { get; set; }
        public string fileData { get; set; }
        public string fileName { get; set; }
        public bool EndOfFile { get; set; }
    }
}