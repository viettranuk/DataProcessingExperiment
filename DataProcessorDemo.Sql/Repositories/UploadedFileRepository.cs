using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessorDemo.Models.DTOs;
using DataProcessorDemo.Services.Interfaces;
using DataProcessorDemo.Services.Interfaces.IRepositories;

namespace DataProcessorDemo.Sql.Repositories
{
    public partial class UploadedFileRepository : IUploadedFileRepository
    {
        private TaxEntities taxDbContext = new TaxEntities();

        private IGenericRepository<UploadedFile> uploadedFileRepo;
        public IGenericRepository<UploadedFile> UploadedFileRepo
        {
            get
            {
                if (this.uploadedFileRepo == null)
                {
                    this.uploadedFileRepo = new GenericRepository<UploadedFile>(taxDbContext);
                }

                return uploadedFileRepo;
            }
        }

        private readonly ILogging _logger;

        public UploadedFileRepository(ILogging logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this._logger = logger;
        }

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

                UploadedFileRepo.Insert(file);
                await taxDbContext.SaveChangesAsync();

                return file.FileId;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return -1;
            }
        }

        public async Task UpdateProcessedLineCount(int fileId, int processedLineCount)
        {
            try
            {
                var thisFile = UploadedFileRepo.GetById(fileId);

                thisFile.ProcessedLineCount = processedLineCount;
                UploadedFileRepo.Update(thisFile);
                
                await taxDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }

        public long GetProcessedLineCountForFile(int fileId)
        {
            try
            {
                return UploadedFileRepo.GetById(fileId).ProcessedLineCount;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return 0;
            }
        }

        public string GetFileNameById(int fileId)
        {
            try
            {
                return UploadedFileRepo.GetById(fileId).Name;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return null;
            }
        }

        public List<UploadedFileDto> GetAllUploadedFiles()
        {
            try
            {
                var allUploadedFiles = UploadedFileRepo
                    .Get(null, i => i.OrderByDescending(j => j.FileId))
                    .Select(f => f.ToUploadedFileDto())
                    .ToList();

                return allUploadedFiles;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return null;
            }
        }
    }

    public partial class UploadedFileRepository : IDisposable
    {
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    taxDbContext.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
