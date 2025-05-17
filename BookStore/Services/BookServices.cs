using AutoMapper;

using BookStore.DATA.DTOs.Book;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Services;

public interface IBookServices
{
    Task<(Book? book, string? error)> Create(BookForm bookForm);
    Task<(List<BookDto> books, int? totalCount, string? error)> GetAll(BookFilter filter);
    
    Task<(Book? book, string? error)> GetById(Guid id);
    Task<(BookDto? book, string? error)> Update(Guid id, BookUpdate bookUpdate);
    Task<(Book? book, string? error)> Delete(Guid id);
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


    public async Task<(Book? book, string? error)> Create(BookForm bookForm)
    {
        var book = _mapper.Map<Book>(bookForm);
        var response = await _repositoryWrapper.Book.Add(book);
        return response == null ? (null, "Error") : (_mapper.Map<Book>(response), null);
        
        
    }

    public async Task<(List<BookDto> books, int? totalCount, string? error)> GetAll(BookFilter filter)
    {
        var (books, totalCount) = await _repositoryWrapper.Book.GetAll<BookDto>(
            x =>
                (filter.Name == null || x.Name!.Contains(filter.Name))
                
            , filter.PageNumber, filter.PageSize);
        
        return (books, totalCount, null);
    }

    public async Task<(Book? book, string? error)> GetById(Guid id)
    {
        var book = await _repositoryWrapper.Book.Get(u => u.Id == id && u.Deleted != true);
        if (book is null) return (null, "Book not found");
        var mappedBook = _mapper.Map<Book>(book);
        return (mappedBook, null);
    }

    public async Task<(BookDto? book, string? error)> Update(Guid id, BookUpdate bookUpdate)
    {
        var book = await _repositoryWrapper.Book.GetById(id);
        var response = await _repositoryWrapper.Book.Update(book);
        var BookDto = _mapper.Map<BookDto>(response);
        return response == null ? (null, "book  cannot be updated") : (BookDto, null);

    }

    public async Task<(Book? book, string? error)> Delete(Guid id)
    {
        var book = await _repositoryWrapper.Book.GetById(id);
        if (book == null) return  (null, "Book not found");
        var response = await _repositoryWrapper.Book.SoftDelete(id);
        return response == null ? (null, "Error") : (response, null);
    }
}