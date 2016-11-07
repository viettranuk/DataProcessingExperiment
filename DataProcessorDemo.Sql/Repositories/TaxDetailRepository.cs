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
    public partial class TaxDetailRepository : ITaxDetailRepository
    {
        private TaxEntities taxDbContext = new TaxEntities();

        private IGenericRepository<TaxDetail> taxDetailRepo;
        public IGenericRepository<TaxDetail> TaxDetailRepo
        {
            get
            {
                if (this.taxDetailRepo == null)
                {
                    this.taxDetailRepo = new GenericRepository<TaxDetail>(taxDbContext);
                }

                return taxDetailRepo;
            }
        }

        private readonly ILogging _logger;

        public TaxDetailRepository(ILogging logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this._logger = logger;
        }

        public async Task AddTaxDetailAsync(string taxDetailValues)
        {
            try
            {
                await taxDbContext.Database.ExecuteSqlCommandAsync(
                    "Insert Into dbo.TaxDetails (FileId, Account, Description, CurrencyCodeId, Amount) Values " + taxDetailValues);

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

        public List<TaxDetailDto> GetProcessedTaxDetailsByFileId(int fileId)
        {
            try
            {
                var allTaxDetails = TaxDetailRepo
                    .Get(f => f.FileId == fileId)
                    .Select(f => f.ToTaxDetailDto())
                    .ToList();

                return allTaxDetails;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return null;
            }
        }
    }

    public partial class TaxDetailRepository : IDisposable
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
