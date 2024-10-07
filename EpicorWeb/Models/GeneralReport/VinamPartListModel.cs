namespace EpicorWeb.Models
{
    public class VinamPartList
	{
		public string Part_PartNum { get; set; }
		public string Part_PartDescription { get; set; }
		public string Part_TypeCode { get; set; }
		public string Part_UOMClassID { get; set; }
		public string Part_IUM { get; set; }
		public string Part_SalesUM { get; set; }
		public string Part_PUM { get; set; }
		public decimal Part_UnitPrice { get; set; }
		public string Part_ProdCode { get; set; }
		public string Part_ClassID { get; set; }
		public bool Part_UsePartRev { get; set; }
		public bool Part_TrackLots { get; set; }
		public string Part_CostMethod { get; set; }
		public bool Part_NonStock { get; set; }
		public bool Part_QtyBearing { get; set; }
		public string PartPlant_Plant { get; set; }
		public string PartPlant_PrimWhse { get; set; }
		public string PartPlant_CostMethod { get; set; }
		public decimal PartPlant_MfgLotSize { get; set; }
		public string PartPlant_SourceType { get; set; }
		public bool PartPlant_NonStock { get; set; }
		public bool PartPlant_QtyBearing { get; set; }
		public bool PartPlant_GenerateSugg { get; set; }
		public bool Part_InActive { get; set; }
		public string EntityGLC_GLControlType { get; set; }
		public string EntityGLC_GLControlCode { get; set; }
		public string Part_TaxCatID { get; set; }
		public decimal PartPlant_MinimumQty { get; set; }
		public decimal PartPlant_MaximumQty { get; set; }
		public decimal PartPlant_SafetyQty { get; set; }
	}
}
