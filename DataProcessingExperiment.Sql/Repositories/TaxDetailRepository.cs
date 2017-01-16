﻿using System;
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
                var allTaxDetails = 
                    GetQueryable<TaxDetail>(f => f.FileId == fileId)
                        .Select(f => f.ToTaxDetailDto())
                        .ToList();

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

                await _context.Database.ExecuteSqlCommandAsync(
                    "Insert Into dbo.TaxDetails (FileId, Account, Description, CurrencyCodeId, Amount) Values " + 
                    taxDetailValues);

                await _context.SaveChangesAsync();

                _context.Dispose();
                //_context = new TaxEntities();
                //_context.Configuration.AutoDetectChangesEnabled = false;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
            }
        }
    }   
}