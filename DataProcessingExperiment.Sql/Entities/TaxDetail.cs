using DataProcessingExperiment.Models.DTOs;
using DataProcessingExperiment.Sql.Enums;

namespace DataProcessingExperiment.Sql
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
