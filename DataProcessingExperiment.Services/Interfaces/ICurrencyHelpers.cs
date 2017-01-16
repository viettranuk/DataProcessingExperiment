using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingExperiment.Services.Interfaces
{
    public interface ICurrencyHelpers
    {
        bool IsValidCurrencyCode(string currencyCode);
        int ToCurrencyCodeEnumValue(string currencyCode);
    }
}
