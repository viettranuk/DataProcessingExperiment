﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessorDemo.Models.DTOs;

namespace DataProcessorDemo.Services.Interfaces.IRepositories
{
    public interface IUploadedFileRepository
    {
        Task<int> AddFileDetailsAsync(UploadedFileDto uploadedFileDto);
        List<UploadedFileDto> GetAllUploadedFiles();
        Task UpdateProcessedLineCount(int fileId, int processedLineCount);
        long GetProcessedLineCountForFile(int fileId);
        string GetFileNameById(int fileId);
    }
}
