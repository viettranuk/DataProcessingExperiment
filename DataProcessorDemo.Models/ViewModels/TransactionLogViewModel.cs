using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessorDemo.Models.DTOs;

namespace DataProcessorDemo.Models.ViewModels
{
    public class TransactionLogViewModel
    {
        public string FileName { get; set; }
        public long ProcessedLineCount { get; set; }
        public List<TaxDetailDto> ProcessedTaxDetails { get; set; }
        public List<UnprocessedDetailDto> UnprocessedDetails { get; set; }

        public TransactionLogViewModel()
        {
            ProcessedTaxDetails = new List<TaxDetailDto>();
            UnprocessedDetails = new List<UnprocessedDetailDto>();
        }
    }
}
