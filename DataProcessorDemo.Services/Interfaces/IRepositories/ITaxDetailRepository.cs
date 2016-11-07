﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessorDemo.Models.DTOs;

namespace DataProcessorDemo.Services.Interfaces.IRepositories
{
    public interface ITaxDetailRepository
    {
        Task AddTaxDetailAsync(string insertTaxDetailSql);
        List<TaxDetailDto> GetProcessedTaxDetailsByFileId(int fileId);
    }
}
