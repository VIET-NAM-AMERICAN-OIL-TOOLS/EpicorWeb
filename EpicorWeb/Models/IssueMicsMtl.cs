namespace EpicorWeb.Models
{
    public class IssueMiscMtl
    {
        public string? Company { get; set; }
        public DateTime? TranDate { get; set; }
        public string? TranType { get; set; }
        public string? PartNum { get; set; }
        public string? PartDescription { get; set;}
        public string? ClassID { get; set;}
        public decimal? GrossWeight { get; set; }
        public string? GrossWeightUOM { get; set; }
        public string? Description { get; set; }
        public string? BinNum { get; set; }
        public string? Name { get; set;}
        public string? LotNum { get;set; }
        public string? Batch { get;set; }
        public string? MfgBatch { get;set; }
        public string? LegalNumber { get; set;}
        public decimal? TranQty { get;set; }
        public string? UM { get;set; }
        public decimal? ExtCost { get; set; }
        public string? EntryPerson { get;set; }
        public int? TranNum { get;set; }
        public string? CalTranNum { get; set; }
        public string? FilterTransaction { get;set; }
        public string? Key1 { get; set; }
        public string? Reference { get; set;}
        public string? Character03 { get; set;}
    }
}
