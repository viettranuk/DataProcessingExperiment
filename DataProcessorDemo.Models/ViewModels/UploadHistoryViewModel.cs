using System.Collections.Generic;
using DataProcessorDemo.Models.DTOs;

namespace DataProcessorDemo.Models.ViewModels
{
    public class UploadHistoryViewModel
    {
        public List<UploadedFileDto> UploadHistories { get; set; }

        public UploadHistoryViewModel()
        {
            UploadHistories = new List<UploadedFileDto>();
        }
    }
}
