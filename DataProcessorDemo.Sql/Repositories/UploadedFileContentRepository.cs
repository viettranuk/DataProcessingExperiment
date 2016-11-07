using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessorDemo.Services.Interfaces;
using DataProcessorDemo.Services.Interfaces.IRepositories;

namespace DataProcessorDemo.Sql.Repositories
{
    public partial class UploadedFileContentRepository : IUploadedFileContentRepository, IDisposable
    {
        private TaxEntities taxDbContext = new TaxEntities();

        private IGenericRepository<UploadedFilesContent> uploadedFileContentRepo;
        public IGenericRepository<UploadedFilesContent> UploadedFileContentRepo
        {
            get
            {
                if (this.uploadedFileContentRepo == null)
                {
                    this.uploadedFileContentRepo = new GenericRepository<UploadedFilesContent>(taxDbContext);
                }

                return uploadedFileContentRepo;
            }
        }

        private readonly ILogging _logger;

        public UploadedFileContentRepository(ILogging logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this._logger = logger;
        }

        public async Task AddFileDataAsync(int fileId, byte[] inputStream)
        {
            try
            {
                var fileData = new UploadedFilesContent
                {
                    FileId = fileId,
                    Data = inputStream
                };

                UploadedFileContentRepo.Insert(fileData);
                await taxDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }
    }

    public partial class UploadedFileContentRepository : IDisposable
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