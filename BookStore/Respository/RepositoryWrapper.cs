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

    private ICityRepository _City;

    public ICityRepository City
    {
        get
        {
            if (_City == null)
            {
                _City = new CityRepository(_context, _mapper);
            }

            return _City;
        }
    }

    private IAddressRepository _Address;

    public IAddressRepository Address
    {
        get
        {
            if (_Address == null)
            {
                _Address = new AddressRepository(_context, _mapper);
            }

            return _Address;
        }
    }

    private IDistrictRepository _District;

    public IDistrictRepository District
    {
        get
        {
            if (_District == null)
            {
                _District = new DistrictRepository(_context, _mapper);
            }

            return _District;
        }
    }

    private IGovernorateRepository _Governorate;

    public IGovernorateRepository Governorate
    {
        get
        {
            if (_Governorate == null) _Governorate = new GovernorateRepository(_context, _mapper);
            return _Governorate;
        }
    }

    private IOrderRepository _Order;

    public IOrderRepository Order
    {
        get
        {
            if (_Order == null)
            {
                _Order = new OrderRepository(_context, _mapper);
            }

            return _Order;
        }
    }

    private IOrderItemRepository _OrderItem;

    public IOrderItemRepository OrderItem
    {
        get
        {
            if (_OrderItem == null)
            {
                _OrderItem = new OrderItemRepository(_context, _mapper);
            }

            return _OrderItem;
        }
    }

    private ICartRepository _Cart;

    public ICartRepository Cart
    {
        get
        {
            if (_Cart == null)
            {
                _Cart = new CartRepository(_context, _mapper);
            }

            return _Cart;
        }
    }

    private ICartProductRepository _CartProduct;

    public ICartProductRepository CartProduct
    {
        get
        {
            if (_CartProduct == null)
            {
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

    public ICategoryRepository Category
    {
        get
        {
            if (_Category == null)
            {
                _Category = new CategoryRepository(_context, _mapper);
            }

            return _Category;
        }
    }

    private IBookRepository _Book;

    public IBookRepository Book
    {
        get
        {
            if (_Book == null)
            {
                _Book = new BookRepository(_context, _mapper);
            }

            return _Book;
        }
    }
}