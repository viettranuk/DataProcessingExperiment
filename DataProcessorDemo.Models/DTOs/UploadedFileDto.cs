using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessorDemo.Models.DTOs
{
    public class UploadedFileDto
    {
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Size { get; set; }
        public DateTime ImportedOn { get; set; }
        public int ProcessedLineCount { get; set; }
    }
}
