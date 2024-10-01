namespace EpicorWeb.Models
{
    public class PurchaseInvoiceListVN
    {
        public int? Stt { get; set; }
        public string? InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? Applydate { get; set; }
        public string? Name { get; set; }
        public string? TaxPayerID { get; set; }
        public decimal? InvoiceAmt { get; set; }
        public decimal? TaxAmt { get; set; }
        public decimal? Percentt { get; set; }
        public string? Descriptionn { get; set; }
        public string? VendorName_c { get; set; }
        public string? TaxID_c { get; set; }
        public string? VendorAddress_c { get; set; }
    }
}
