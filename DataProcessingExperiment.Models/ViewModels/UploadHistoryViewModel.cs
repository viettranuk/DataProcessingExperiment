using System.Collections.Generic;
using DataProcessingExperiment.Models.DTOs;

namespace DataProcessingExperiment.Models.ViewModels
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
