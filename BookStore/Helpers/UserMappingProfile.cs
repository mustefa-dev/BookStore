using AutoMapper;
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.Book;
using BookStore.DATA.DTOs.Cart;
using BookStore.DATA.DTOs.CartProduct;
using BookStore.DATA.DTOs.Category;
using BookStore.DATA.DTOs.Order;
using BookStore.DATA.DTOs.User;
using BookStore.Entities;
using OneSignalApi.Model;

namespace BookStore.Helpers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<AppUser, UserDto>();
        CreateMap<UpdateUserForm, AppUser>();
        CreateMap<AppUser, TokenDTO>();
        CreateMap<RegisterForm, App>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<AppUser, AppUser>();

        
        // here to add

        CreateMap<Address, AddressDto>().ForMember(dist => dist.GovernorateId
                , opt => opt.MapFrom(src => src.City.District.Governorate!.Id))
            .ForMember(dist => dist.DistrictId
                , opt => opt.MapFrom(src => src.City.District.Id))
            .ForMember(dist => dist.CityId
                , opt => opt.MapFrom(src => src.City.Id))
            .ForMember(dist => dist.GovernorateName
                , opt => opt.MapFrom(src => src.City.District.Governorate!.Name))
            .ForMember(dist => dist.DistrictName
                , opt => opt.MapFrom(src => src.City.District.Name))
            .ForMember(dist => dist.CityName
                , opt => opt.MapFrom(src => src.City.Name));


        CreateMap<AddressForm, Address>();
        CreateMap<AddressUpdate, Address>().ForAllMembers(opts =>
            opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<District, DistrictDto>();
        CreateMap<DistrictForm, District>();
        CreateMap<DistrictUpdate, District>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Governorate, GovernorateDto>();
        CreateMap<GovernorateForm, Governorate>();
        CreateMap<GovernorateUpdate, Governorate>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    
        
        
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderProducts))
            .ForMember(dest => dest.ClientFullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.ClientEmail, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.ClientRole, opt => opt.MapFrom(src => src.User.Role))
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.User.Id));
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
      
        
        
        CreateMap<City, CityDto>();
        CreateMap<CityForm, City>();
        CreateMap<CityUpdate, City>();
        
        
        
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