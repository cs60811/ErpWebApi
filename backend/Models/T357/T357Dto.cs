namespace backend.Models.T357
{
    public class T357Settings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string CID { get; set; } = string.Empty;
        public string UID { get; set; } = string.Empty;
        public string UPWD { get; set; } = string.Empty;
        public string LoginType { get; set; } = "Net";
    }

    public class T357Request<T>
    {
        public string CID { get; set; } = string.Empty;
        public string UID { get; set; } = string.Empty;
        public string UPWD { get; set; } = string.Empty;
        public string LoginType { get; set; } = "Net";
        public string Tag { get; set; } = string.Empty;
        public T357Data<T> Data { get; set; } = new();
    }

    public class T357Data<T>
    {
        public List<T> MasterData { get; set; } = new();
    }

    public class T357Response<T>
    {
        public List<T357Result> result { get; set; } = new();
        public List<T> MasterData { get; set; } = new();
    }

    public class T357Result
    {
        public string IfSucceed { get; set; } = "False";
        public string ErrMessage { get; set; } = string.Empty;
    }

    public static class T357Tags
    {
        public const string ProductQuery = "Product_Query";
        public const string CustomerQuery = "Customer_Query";
        public const string OrderCreate = "Order_Create";
    }
}
