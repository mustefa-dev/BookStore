namespace BookStore.Utils;

public class Respons<T>
{
    public Respons()
    {
    }

    public Respons(bool status, string message, List<T> data, int totalPages, int currentPage, int totalCount)
    {
        Data = data;
        totalPages = totalPages;
        CurrentPage = currentPage;
        TotalCount = totalCount;

    }
    

    public List<T> Data { get; set; }
    public int? totalPages { get; set; }
    public int CurrentPage { get; set; }
    public string Type { get; set; } = typeof(T).Name;
    public int? TotalCount { get; set; }
}