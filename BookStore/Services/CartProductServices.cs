using AutoMapper;
using BookStore.Repository;
using BookStore.Services;
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.CartProduct;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Services;

public interface ICartProductServices
{
    Task<(CartProduct? cartproduct, string? error)> Create(CartProductForm cartproductForm);
    Task<(List<CartProductDto> cartproducts, int? totalCount, string? error)> GetAll(CartProductFilter filter);
    Task<(CartProduct? cartproduct, string? error)> Update(Guid id, CartProductUpdate cartproductUpdate);
    Task<(CartProduct? cartproduct, string? error)> Delete(Guid id);
}

public class CartProductServices : ICartProductServices
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CartProductServices(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper
    )
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }


    public async Task<(CartProduct? cartproduct, string? error)> Create(CartProductForm cartproductForm)
    {
        throw new NotImplementedException();
    }

    public async Task<(List<CartProductDto> cartproducts, int? totalCount, string? error)> GetAll(
        CartProductFilter filter)
    {
        throw new NotImplementedException();
    }

    public async Task<(CartProduct? cartproduct, string? error)> Update(Guid id, CartProductUpdate cartproductUpdate)
    {
        throw new NotImplementedException();
    }

    public async Task<(CartProduct? cartproduct, string? error)> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}