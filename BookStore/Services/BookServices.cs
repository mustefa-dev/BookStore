using AutoMapper;
using BookStore.DATA.DTOs.Book;
using BookStore.DATA.DTOs.Statistics;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Services;

public interface IBookServices
{
    Task<(BookDto? book, string? error)> Create(BookForm bookForm, Guid userId);
    Task<(List<BookDto> books, int? totalCount, string? error)> GetAll(BookFilter filter);

    Task<(BookDto? book, string? error)> GetById(Guid id);
    Task<(BookDto? book, string? error)> Update(Guid id, BookUpdate bookUpdate, Guid userId);
    Task<(Book? book, string? error)> Delete(Guid id, Guid userId);
    
    Task<(BookStatisticsDto? statistics, string? error)> GetBookStatistics();
}

public class BookServices : IBookServices
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public BookServices(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper
    )
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }


    public async Task<(BookDto? book, string? error)> Create(BookForm bookForm, Guid userId)
    {
        var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
        if (user == null) return (null, "User not found");
        {
            var category = await _repositoryWrapper.Category.GetById(bookForm.CategoryId);
            if (category == null) return (null, "Category not found");
            var book = _mapper.Map<Book>(bookForm);
            var response = await _repositoryWrapper.Book.Add(book);
            return response == null ? (null, "Error") : (_mapper.Map<BookDto>(response), null);
        }
    }

    public async Task<(List<BookDto> books, int? totalCount, string? error)> GetAll(BookFilter filter)
    {
        var (books, totalCount) = await _repositoryWrapper.Book.GetAll<BookDto>(
            x =>
                (filter.Name == null || x.Name!.Contains(filter.Name)) &&
                (filter.Author == null || x.Author!.Contains(filter.Author)) &&
                (filter.Genre == null || x.Genre!.Contains(filter.Genre)) &&
                (filter.MinPrice == null || x.Price >= filter.MinPrice) &&
                (filter.MaxPrice == null || x.Price <= filter.MaxPrice) &&
                (filter.PublishedDateFrom == null || x.PublishedDate >= filter.PublishedDateFrom) &&
                (filter.PublishedDateTo == null || x.PublishedDate <= filter.PublishedDateTo) &&
                (filter.CategoryId == null || x.CategoryId == filter.CategoryId),
            filter.PageNumber,
            filter.PageSize);

        return (books, totalCount, null);
    }


    public async Task<(BookDto? book, string? error)> GetById(Guid id)
    {
        var book = await _repositoryWrapper.Book.Get<BookDto>(u => u.Id == id && u.Deleted != true);
        if (book == null) return (null, "Book not found");
        var mappedBook = _mapper.Map<BookDto>(book);
        return (mappedBook, null);
    }

    public async Task<(BookDto? book, string? error)> Update(Guid id, BookUpdate bookUpdate, Guid userId)
    {
        var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
        if (user == null) return (null, "User not found");
        var book = await _repositoryWrapper.Book.GetById(id);
        if (book == null) return (null, "Book not found");
        if (!bookUpdate.CategoryId.HasValue) { bookUpdate.CategoryId = book.CategoryId; }
        else
        {
            var category = await _repositoryWrapper.Category.GetById(bookUpdate.CategoryId.Value);
            if (category == null) return (null, $"Category with ID {bookUpdate.CategoryId.Value} does not exist");
        }
        _mapper.Map(bookUpdate, book);
        var response = await _repositoryWrapper.Book.Update(book);
        var bookDto = _mapper.Map<BookDto>(response);
        return response == null ? (null, "Book cannot be updated") : (bookDto, null);
    }

    public async Task<(Book? book, string? error)> Delete(Guid id, Guid userId)
    {
        var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
        if (user == null) return (null, "User not found");
        var book = await _repositoryWrapper.Book.GetById(id);
        if (book == null) return (null, "Book not found");
        var response = await _repositoryWrapper.Book.SoftDelete(id);
        return response == null ? (null, "Error") : (response, null);
    }
    public async Task<(BookStatisticsDto? statistics, string? error)> GetBookStatistics()
{
    var statistics = new BookStatisticsDto();
    
    // Get all orders with items
    var (orders, _) = await _repositoryWrapper.Order.GetAll(
        o => o.OrderStatus != OrderStatus.Canceled);
    
    if (orders == null)
        return (statistics, null);

    // Get all books
    var (books, _) = await _repositoryWrapper.Book.GetAll();
    
    if (books == null)
        return (statistics, null);

    // Get all order items
    var bookSales = new Dictionary<Guid, (int Quantity, decimal Revenue)>();
    foreach (var order in orders)
    {
        var (orderItems, _) = await _repositoryWrapper.OrderItem.GetAll(oi => oi.OrderId == order.Id);
        if (orderItems != null)
        {
            foreach (var item in orderItems)
            {
                var book = books.FirstOrDefault(b => b.Id == item.BookId);
                if (book != null)
                {
                    decimal revenue = book.Price * item.Quantity;
                    if (bookSales.ContainsKey(book.Id))
                    {
                        var existing = bookSales[book.Id];
                        bookSales[book.Id] = (existing.Quantity + item.Quantity, existing.Revenue + revenue);
                    }
                    else
                    {
                        bookSales[book.Id] = (item.Quantity, revenue);
                    }
                }
            }
        }
    }

    // Best selling books
    statistics.BestSellingBooks = bookSales.Select(kv => 
        {
            var book = books.FirstOrDefault(b => b.Id == kv.Key);
            return new BestSellingBookDto
            {
                BookId = kv.Key,
                Title = book?.Name ?? "Unknown",
                QuantitySold = kv.Value.Quantity,
                Revenue = kv.Value.Revenue
            };
        })
        .OrderByDescending(b => b.QuantitySold)
        .Take(10)
        .ToList();

    // Low stock books (less than 5 in stock)
    statistics.LowStockBooks = books
        .Where(b => b.Stock.HasValue && b.Stock.Value < 5)
        .Select(b => new LowStockBookDto
        {
            BookId = b.Id,
            Title = b.Name ?? "Unknown",
            CurrentStock = b.Stock ?? 0
        })
        .OrderBy(b => b.CurrentStock)
        .ToList();

    // Revenue by category
    var revenueByCategory = new Dictionary<string, decimal>();
    foreach (var sale in bookSales)
    {
        var book = books.FirstOrDefault(b => b.Id == sale.Key);
        if (book != null)
        {
            var category = await _repositoryWrapper.Category.Get(c => c.Id == book.CategoryId);
            var categoryName = category?.Name ?? "Unknown";
            if (revenueByCategory.ContainsKey(categoryName))
                revenueByCategory[categoryName] += sale.Value.Revenue;
            else
                revenueByCategory[categoryName] = sale.Value.Revenue;
        }
    }
    statistics.RevenueByCategory = revenueByCategory;

    return (statistics, null);
}
}