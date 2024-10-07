namespace EpicorWeb.Models
{
    public class VinamPartOnHandConsumables
	{
		public string Company { get; set; }
		public string PartNum { get; set; }
		public string WarehouseCode { get; set; }
		public string BinNum { get; set; }
		public decimal OnhandQty { get; set; }
		public string LotNum { get; set; }
		public int PONum { get; set; }
		public int POLine { get; set; }
		public int PORel_PORelNum { get; set; }
		public string LineDesc { get; set; }
		public decimal OrderQty { get; set; }
		public string IUM { get; set; }
		public decimal UnitCost { get; set; }
		public decimal DocUnitCost { get; set; }

	}
}
