namespace EpicorWeb.Models
{
    public class SupplierPriceList
    {
        public string? Company { get; set; }
        public string? PartNum { get; set; }
        public int? VendorNum { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal BreakQty { get; set; }
        public decimal PriceModifier { get; set; }
        public Guid SysRowID { get; set; }
    }
}
