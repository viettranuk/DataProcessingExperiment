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
        public List<TaxDetailDto> GetProcessedTaxDetailsByFileId(int fileId)
        {
            try
            {
                var rawAllTaxDetails = GetQueryable<TaxDetail>(f => f.FileId == fileId).AsEnumerable();
                var allTaxDetails = rawAllTaxDetails.Select(f => f.ToTaxDetailDto()).ToList();

                return allTaxDetails;
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
        public async Task AddTaxDetailAsync(string taxDetailValues)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                _context.Database.ExecuteSqlCommand(
                    "Insert Into dbo.TaxDetails (FileId, Account, Description, CurrencyCodeId, Amount) Values " + 
                    taxDetailValues);

                await SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
            }
        }
    }   
}