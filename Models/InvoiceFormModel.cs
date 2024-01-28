namespace INTTest.Models
{
    public class InvoiceFormModel
    {
        public string PartyName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string TotalAmount { get; set; }
        public List<TableRowData> TableData { get; set; }
    }
}


