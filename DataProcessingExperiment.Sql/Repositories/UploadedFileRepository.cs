using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessingExperiment.Models.DTOs;
using DataProcessingExperiment.Services.Interfaces;
using DataProcessingExperiment.Services.Interfaces.IRepositories;

namespace DataProcessingExperiment.Sql.Repositories
{
    public partial class BaseReadOnlyRepository<TContext> : IBaseReadOnlyRepository
        where TContext : DbContext
    {
        public long GetProcessedLineCountForFile(int fileId)
        {
            try
            {
                return GetById<UploadedFile>(fileId).ProcessedLineCount;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
                return 0;
            }
        }

        public string GetFileNameById(int fileId)
        {
            try
            {
                return GetById<UploadedFile>(fileId).Name;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
                return null;
            }
        }

        public List<UploadedFileDto> GetAllUploadedFiles()
        {
            try
            {
                var allUploadedFiles = GetAll<UploadedFile>(i => i.OrderByDescending(j => j.FileId))
                    .Select(f => f.ToUploadedFileDto())
                    .ToList();

                return allUploadedFiles;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
                return null;
            }
        }
    }

    public partial class BaseRepository<TContext> : BaseReadOnlyRepository<TContext>, IBaseRepository
        where TContext : DbContext
    {
        public async Task<int> AddFileDetailsAsync(UploadedFileDto uploadedFileDto)
        {
            try
            {
                var file = new UploadedFile
                {
                    Name = uploadedFileDto.Name,
                    Description = uploadedFileDto.Description,
                    Size = uploadedFileDto.Size,
                    ImportedOn = DateTime.Now,
                    ProcessedLineCount = 0
                };

                Create<UploadedFile>(file);

                await _context.SaveChangesAsync();

                return file.FileId;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
                return -1;
            }
        }

        public async Task UpdateProcessedLineCount(int fileId, int processedLineCount)
        {
            try
            {
                var thisFile = GetById<UploadedFile>(fileId);

                thisFile.ProcessedLineCount = processedLineCount;

                Update<UploadedFile>(thisFile);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
            }
        }
    }
}
