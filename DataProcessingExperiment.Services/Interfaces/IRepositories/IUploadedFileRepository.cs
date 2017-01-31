using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessingExperiment.Models.DTOs;

namespace DataProcessingExperiment.Services.Interfaces.IRepositories
{
    public interface IUploadedFileRepository
    {
        Task<int> AddFileDetailsAsync(UploadedFileDto uploadedFileDto);
        List<UploadedFileDto> GetAllUploadedFiles();
        Task UpdateProcessedLineCountAsync(int fileId, int processedLineCount);
        long GetProcessedLineCountForFile(int fileId);
        string GetFileNameById(int fileId);
    }
}
