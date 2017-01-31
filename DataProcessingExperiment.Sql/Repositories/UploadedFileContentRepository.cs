using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessingExperiment.Services.Interfaces;
using DataProcessingExperiment.Services.Interfaces.IRepositories;

namespace DataProcessingExperiment.Sql.Repositories
{
    public partial class BaseRepository<TContext> : BaseReadOnlyRepository<TContext>, IBaseRepository
        where TContext : DbContext
    {
        public async Task AddFileDataAsync(int fileId, byte[] inputStream)
        {
            try
            {
                var fileData = new UploadedFilesContent
                {
                    FileId = fileId,
                    Data = inputStream
                };

                Create<UploadedFilesContent>(fileData);

                await SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
            }
        }
    }    
}