using AutoMapper;
using BookStore.DATA.DTOs.Book;
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
}

public class BookServices : IBookServices
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repository;

    public BookServices(
        IMapper mapper,
        IRepositoryWrapper repository
    )
    {
        _mapper = mapper;
        _repository = repository;
    }


    public async Task<(BookDto? book, string? error)> Create(BookForm bookForm, Guid userId)
    {
        var user = await _repository.User.Get(x => x.Id == userId);
        if (user == null) return (null, "User not found");
        {
            var category = await _repository.Category.GetById(bookForm.CategoryId);
            if (category == null) return (null, "Category not found");
            var book = _mapper.Map<Book>(bookForm);
            var response = await _repository.Book.Add(book);
            return response == null ? (null, "Error") : (_mapper.Map<BookDto>(response), null);
        }
    }

    public async Task<(List<BookDto> books, int? totalCount, string? error)> GetAll(BookFilter filter)
    {
        var (books, totalCount) = await _repository.Book.GetAll<BookDto>(
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
        var book = await _repository.Book.Get<BookDto>(u => u.Id == id && u.Deleted != true);
        if (book == null) return (null, "Book not found");
        var mappedBook = _mapper.Map<BookDto>(book);
        return (mappedBook, null);
    }

    public async Task<(BookDto? book, string? error)> Update(Guid id, BookUpdate bookUpdate, Guid userId)
    {
        var user = await _repository.User.Get(x => x.Id == userId);
        if (user == null) return (null, "User not found");
        var book = await _repository.Book.GetById(id);
        if (book == null) return (null, "Book not found");
        if (!bookUpdate.CategoryId.HasValue) { bookUpdate.CategoryId = book.CategoryId; }
        else
        {
            var category = await _repository.Category.GetById(bookUpdate.CategoryId.Value);
            if (category == null) return (null, $"Category with ID {bookUpdate.CategoryId.Value} does not exist");
        }
        _mapper.Map(bookUpdate, book);
        var response = await _repository.Book.Update(book);
        var bookDto = _mapper.Map<BookDto>(response);
        return response == null ? (null, "Book cannot be updated") : (bookDto, null);
    }

    public async Task<(Book? book, string? error)> Delete(Guid id, Guid userId)
    {
        var user = await _repository.User.Get(x => x.Id == userId);
        if (user == null) return (null, "User not found");
        var book = await _repository.Book.GetById(id);
        if (book == null) return (null, "Book not found");
        var response = await _repository.Book.SoftDelete(id);
        return response == null ? (null, "Error") : (response, null);
    }
}