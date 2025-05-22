namespace BookStore.DATA.DTOs.Statistics;

public class OrderStatisticsDto
{
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public decimal AverageOrderValue { get; set; }
    public Dictionary<string, int> OrdersByStatus { get; set; } = new();
    public Dictionary<string, decimal> RevenueByGovernorate { get; set; } = new();
    public List<TopCustomerDto> TopCustomers { get; set; } = new();
    public Dictionary<string, decimal> DailyRevenue { get; set; } = new();
}

public class TopCustomerDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public int OrderCount { get; set; }
    public decimal TotalSpent { get; set; }
}