namespace LoginUpLevel.DTOs
{
    public class StatisticsDTO
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public float? TotalPrice { get; set; }
        public float? TotalPriceToday { get; set; }
        public int? TotalOrders { get; set; }
        public int? TotalOrdersToday { get; set; }
        public int? TotalCustomers { get; set; }
        public int? TotalProducts { get; set; }
        public int? TotalEmployee { get; set; }
    }
}
