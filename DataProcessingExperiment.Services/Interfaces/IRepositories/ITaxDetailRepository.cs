using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessingExperiment.Models.DTOs;

namespace DataProcessingExperiment.Services.Interfaces.IRepositories
{
    public interface ITaxDetailRepository
    {
        Task AddTaxDetailAsync(string insertTaxDetailSql);
        List<TaxDetailDto> GetProcessedTaxDetailsByFileId(int fileId);
    }
}
