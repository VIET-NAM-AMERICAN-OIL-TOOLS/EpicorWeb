namespace EpicorWeb.Models
{
    public class PriceList
    {
        public string? Company { get; set; }
        public string? ListCode { get; set; }
        public string? PartNum {  get; set; }
        public decimal Quantity {  get; set; }
        public decimal BasePrice {  get; set; }
        public decimal UnitPrice {  get; set; }
        public Guid SysRowID { get; set; }
    }
}
