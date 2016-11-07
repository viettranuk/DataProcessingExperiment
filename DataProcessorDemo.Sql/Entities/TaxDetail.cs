using DataProcessorDemo.Models.DTOs;
using DataProcessorDemo.Sql.Enums;

namespace DataProcessorDemo.Sql
{
    public partial class TaxDetail
    {
        internal TaxDetailDto ToTaxDetailDto()
        {
            return new TaxDetailDto
            {
                FileId = this.FileId,
                Account = this.Account,
                Description = this.Description,
                CurrencyCode = ((CurrencyCode)this.CurrencyCodeId).ToString(),
                Amount = this.Amount
            };
        }
    }
}
