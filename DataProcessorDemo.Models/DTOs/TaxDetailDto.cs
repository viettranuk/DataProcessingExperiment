namespace DataProcessorDemo.Models.DTOs
{
    public class TaxDetailDto
    {
        public int FileId { get; set; }
        public string Account { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
    }
}
