namespace EpicorWeb.Models
{
    public class VinamPartOnHandQuantity
	{
		public string Company { get; set; }
		public string PartNum { get; set; }
		public string PartDescription { get; set; }
		public string PartClass_Description { get; set; }
		public string WarehouseCode { get; set; }
		public string BinNum { get; set; }
		public decimal OnhandQty { get; set; }
		public string LotNum { get; set; }
		public string MfgBatch { get; set; }
		public string MfgLot { get; set; }
		public string HeatNum { get; set; }
		public string FirmWare { get; set; }
		public DateTime? BestBeforeDt { get; set; }
		public DateTime? MfgDt { get; set; }
		public DateTime? CureDt { get; set; }
		public DateTime? ExpireDt { get; set; }
		public DateTime? ExpirationDate { get; set; }
		public string Batch { get; set; }
		public int? ImportNum { get; set; }
		public int? ImportedFrom { get; set; }
		public string ImportedFromDesc { get; set; }
		public string DimCode { get; set; }
		public decimal AllocatedQty { get; set; }
		public decimal SalesAllocatedQty { get; set; }
		public decimal SalesPickingQty { get; set; }
		public decimal SalesPickedQty { get; set; }
		public decimal JobAllocatedQty { get; set; }
		public decimal JobPickingQty { get; set; }
		public decimal JobPickedQty { get; set; }
		public decimal TFOrdAllocatedQty { get; set; }
		public decimal TFOrdPickingQty { get; set; }
		public decimal TFOrdPickedQty { get; set; }
		public decimal ShippingQty { get; set; }
		public decimal PackedQty { get; set; }
		public Guid SysRowID { get; set; }
		public string PCID { get; set; }
		public bool SendToFSA { get; set; }
	}
}
