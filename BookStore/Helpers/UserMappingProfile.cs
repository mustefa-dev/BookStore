using AutoMapper;
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.Book;
using BookStore.DATA.DTOs.Cart;
using BookStore.DATA.DTOs.CartProduct;
using BookStore.DATA.DTOs.Category;
using BookStore.DATA.DTOs.User;
using BookStore.Entities;
using OneSignalApi.Model;

namespace BookStore.Helpers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        var baseUrl = "http://localhost:5051/";


        CreateMap<AppUser, UserDto>();
        CreateMap<UpdateUserForm, AppUser>();
        CreateMap<AppUser, TokenDTO>();

        CreateMap<RegisterForm, App>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        CreateMap<AppUser, AppUser>();


        // here to add
        CreateMap<Cart, CartDto>();
        CreateMap<CartProduct, CartProductDto>();
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