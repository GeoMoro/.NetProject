using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Presentation.Models
{
    public class FileViewModel
    {
        public IFormFile File { get; set; }
        public long Size { get; set; }
        public string Source { get; set; }
        public string Extension { get; set; }
        public string Type { get; set; }

    }
}
