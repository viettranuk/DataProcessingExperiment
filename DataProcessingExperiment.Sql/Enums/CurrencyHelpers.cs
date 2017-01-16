using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataProcessingExperiment.Services.Interfaces;

namespace DataProcessingExperiment.Sql.Enums
{
    public class CurrencyHelpers : ICurrencyHelpers
    {
        private readonly ILogging _logger;

        public CurrencyHelpers(ILogging logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this._logger = logger;
        }
        
        public bool IsValidCurrencyCode(string currencyCode)
        {
            try
            {
                var camelCaseCode = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(currencyCode.ToLower());
                var validCode = Enum.IsDefined(typeof(CurrencyCode), camelCaseCode);

                return validCode;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
                return false;
            }
        }

        public int ToCurrencyCodeEnumValue(string currencyCode)
        {
            var moneyCode = (CurrencyCode) Enum.Parse(typeof(CurrencyCode), currencyCode, true);
            return (int) moneyCode;
        }
    }
}
