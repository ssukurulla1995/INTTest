namespace INTTest.Models
{
    public class Docdet
    {
        public int? DocDetail_ID { get; set; } // Assuming an identity column for primary key
        public int? HDRAuto_ID { get; set; } // Assuming a foreign key relationship with DOCHDR
        public int? SrNo { get; set; }
        public int? Item_ID { get; set; } // Assuming a foreign key relationship with ITEM_MASTER
        public int? UOM_ID { get; set; } // Assuming a foreign key relationship with UOM
        public decimal? Qty { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Value { get; set; }
    }
}
