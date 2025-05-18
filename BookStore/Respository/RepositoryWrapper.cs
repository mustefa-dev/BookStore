using AutoMapper;
using BookStore.DATA;
using BookStore.Interface;
using BookStore.Repository;

namespace BookStore.Respository;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;


    // here to add
private ICartRepository _Cart;

public ICartRepository Cart {
    get {
        if(_Cart == null) {
            _Cart = new CartRepository(_context, _mapper);
        }
        return _Cart;
    }
}
private ICartProductRepository _CartProduct;

public ICartProductRepository CartProduct {
    get {
        if(_CartProduct == null) {
            _CartProduct = new CartProductRepository(_context, _mapper);
        }
        return _CartProduct;
    }
}

    
    private IUserRepository _user;


    public RepositoryWrapper(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    

    public IUserRepository User
    {
        get
        {
            if (_user == null) _user = new UserRepository(_context, _mapper);
            return _user;
        }
    }

    private ICategoryRepository _Category;

    public ICategoryRepository Category {
        get {
            if(_Category == null) {
                _Category = new CategoryRepository(_context, _mapper);
            }
            return _Category;
        }
    }
    private IBookRepository _Book;

    public IBookRepository Book {
        get {
            if(_Book == null) {
                _Book = new BookRepository(_context, _mapper);
            }
            return _Book;
        }
    }
}
