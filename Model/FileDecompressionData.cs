using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUnrar.Model
{
    public class FileDecompressionData
    {
        public File? fileData { get; set; }
        public string? output_path { get; set; }
        public System.Diagnostics.Process? decompressionProcess { get; set; }
    }
}
