namespace backend.Models.T357
{
    public class ProductQueryMaster
    {
        public string? ProdID { get; set; }
        public string? ProdName { get; set; }
        public string? Unit { get; set; }
        public decimal? BCurrStock { get; set; }
        public decimal? CCurrStock { get; set; }
        
        // Detailed data sections
        public List<DetailData>? DetailData { get; set; } // 客戶原廠編號
        public List<DetailData1>? DetailData1 { get; set; } // 廠商原廠編號
    }

    public class DetailData
    {
        public string? CustID { get; set; }
        public string? CustProdID { get; set; }
    }

    public class DetailData1
    {
        public string? FactID { get; set; }
        public string? FactProdID { get; set; }
    }
}
