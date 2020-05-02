using System;
using System.Collections.Generic;
using System.Text;

namespace Kinvitech.Services.Models
{
    public class FileErrorMetadata
    {
        public string FileName { get; set; }
        public string BillerID { get; set; }
        public Exception Error { get; set; }
    }
}
