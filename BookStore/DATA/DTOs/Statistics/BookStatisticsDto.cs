namespace BookStore.DATA.DTOs.Statistics;

public class BookStatisticsDto
{
    public List<BestSellingBookDto> BestSellingBooks { get; set; } = new();
    public List<LowStockBookDto> LowStockBooks { get; set; } = new();
    public Dictionary<string, decimal> RevenueByCategory { get; set; } = new();
}

public class BestSellingBookDto
{
    public Guid BookId { get; set; }
    public string Title { get; set; }
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}

public class LowStockBookDto
{
    public Guid BookId { get; set; }
    public string Title { get; set; }
    public int CurrentStock { get; set; }
}