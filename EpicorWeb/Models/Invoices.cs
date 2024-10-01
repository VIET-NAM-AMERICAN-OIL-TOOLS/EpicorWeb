namespace EpicorWeb.Models
{
    public class Invoices
    {
        public int? Invoice { set; get; }
        public string? Suffix { set; get; }
        public DateTime? Date { set; get; }
        public DateTime? Due_date { set; get; }
        public bool? Open { set; get; }
        public string? Type { set; get; }
        public string? Name { set; get; }
        public string? Part { set; get; }
        public string? Description { set; get; }
        public string? LegalNumber { set; get; }
        public string? CustInvNum { set; get; }
        public string? Total_Inv_Amt { set; get; }
        public string? Inv_Bal { set; get; }
        public string? Currency { set; get; }
        public string? Inv_Line { set; get; }
        public string? PO { set; get; }
        public string? PO_Line { set; get; }
        public string? Sales_Order { set; get; }
        public string? SO_Line { set; get; }
        public string? Line_Amt { set; get; }
        public string? UR_Check { set; get; }
        public string? Entered_By { set; get; }
        public string? Hold_Invoice { set; get; }

    }
}
