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
    public partial class UnprocessedDetailRepository : IUnprocessedDetailRepository
    {
        private TaxEntities taxDbContext = new TaxEntities();

        private IGenericRepository<UnprocessedDetail> unprocessedDetailRepo;
        public IGenericRepository<UnprocessedDetail> UnprocessedDetailRepo
        {
            get
            {
                if (this.unprocessedDetailRepo == null)
                {
                    this.unprocessedDetailRepo = new GenericRepository<UnprocessedDetail>(taxDbContext);
                }

                return unprocessedDetailRepo;
            }
        }

        private readonly ILogging _logger;

        public UnprocessedDetailRepository(ILogging logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this._logger = logger;
        }

        public async Task AddUnprocessedDetailAsync(string unprocessedValues)
        {
            try
            {
                await taxDbContext.Database.ExecuteSqlCommandAsync(
                    "Insert Into dbo.UnprocessedDetails (FileId, LineData) Values " + unprocessedValues);

                await taxDbContext.SaveChangesAsync();

                taxDbContext.Dispose();
                taxDbContext = new TaxEntities();
                taxDbContext.Configuration.AutoDetectChangesEnabled = false;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }
        
        public List<UnprocessedDetailDto> GetUnprocessedDetailsByFileId(int fileId)
        {
            try
            {
                var allUnprocessedDetails = UnprocessedDetailRepo
                    .Get(f => f.FileId == fileId)
                    .Select(f => f.ToUnprocessedDetailDto())
                    .ToList();

                return allUnprocessedDetails;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return null;
            }
        }
    }

    public partial class UnprocessedDetailRepository : IDisposable
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
