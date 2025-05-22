using AutoMapper;
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.Book;
using BookStore.DATA.DTOs.Cart;
using BookStore.DATA.DTOs.CartProduct;
using BookStore.DATA.DTOs.Category;
using BookStore.DATA.DTOs.Order;
using BookStore.DATA.DTOs.User;
using BookStore.Entities;
using Tweetinvi.Core.Models.Properties;

namespace BookStore.Helpers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<AppUser, UserDto>();
        CreateMap<UpdateUserForm, AppUser>();
        CreateMap<AppUser, TokenDTO>();
        CreateMap<AppUser, AppUser>();


        // here to add

        CreateMap<Address, AddressDto>().ForMember(dist => dist.GovernorateName
            , opt => opt.MapFrom(src => src.Governorate));
            

        CreateMap<AddressForm, Address>();
        CreateMap<AddressUpdate, Address>().ForAllMembers(opts =>
            opts.Condition((src, dest, srcMember) => srcMember != null));
       
        CreateMap<Governorate, GovernorateDto>();
        CreateMap<GovernorateForm, Governorate>();
        CreateMap<GovernorateUpdate, Governorate>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderProducts))
            .ForMember(dest => dest.ClientFullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.ClientEmail, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.ClientRole, opt => opt.MapFrom(src => src.User.Role))
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.AddressName, opt => opt.MapFrom(src => src.Address.Name))
            .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => src.Address.FullAddress))
            .ForMember(dest => dest.Latidute, opt => opt.MapFrom(src => src.Address.Latidute))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Address.Longitude))
            .ForMember(dest => dest.IsMain, opt => opt.MapFrom(src => src.Address.IsMain))
            .ForMember(dest => dest.GovernorateName,
                opt => opt.MapFrom(src => src.Address.Governorate.Name))
            .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.Address.District))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Address.City));
        CreateMap<OrderForm, Order>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.Book.Name))
            .ForMember(dest => dest.BookAuthor, opt => opt.MapFrom(src => src.Book.Author))
            .ForMember(dest => dest.BookDescription, opt => opt.MapFrom(src => src.Book.Description))
            .ForMember(dest => dest.BookImageUrl, opt => opt.MapFrom(src => src.Book.ImageUrl))
            .ForMember(dest => dest.BookPublishedDate, opt => opt.MapFrom(src => src.Book.PublishedDate))
            .ForMember(dest => dest.BookPrice, opt => opt.MapFrom(src => src.Book.Price))
            .ForMember(dest => dest.BookGenre, opt => opt.MapFrom(src => src.Book.Genre))
            .ForMember(dest => dest.BookCategoryId, opt => opt.MapFrom(src => src.Book.CategoryId));
        CreateMap<OrderItemForm, OrderItem>();
        CreateMap<OrderItemUpdate, OrderItem>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        CreateMap<Cart, CartDto>();
        CreateMap<CartForm, Cart>();
        CreateMap<CartUpdate, Cart>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


     
        CreateMap<CartProduct, CartProductDto>();
        CreateMap<CartProductForm, CartProduct>();
        CreateMap<CartProductUpdate, CartProduct>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryForm, Category>();
        CreateMap<CategoryUpdate, Category>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        CreateMap<Book, BookDto>();
        CreateMap<BookForm, Book>();
        CreateMap<BookUpdate, Book>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}