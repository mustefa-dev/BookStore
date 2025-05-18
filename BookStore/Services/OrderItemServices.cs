
using AutoMapper;
using BookStore.Repository;
using BookStore.Services;
using BookStore.DATA.DTOs;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Services;


public interface IOrderItemServices
{
Task<(OrderItem? orderitem, string? error)> Create(OrderItemForm orderitemForm );
Task<(List<OrderItemDto> orderitems, int? totalCount, string? error)> GetAll(OrderItemFilter filter);
Task<(OrderItem? orderitem, string? error)> Update(Guid id , OrderItemUpdate orderitemUpdate);
Task<(OrderItem? orderitem, string? error)> Delete(Guid id);
}

public class OrderItemServices : IOrderItemServices
{
private readonly IMapper _mapper;
private readonly IRepositoryWrapper _repositoryWrapper;

public OrderItemServices(
    IMapper mapper ,
    IRepositoryWrapper repositoryWrapper
    )
{
    _mapper = mapper;
    _repositoryWrapper = repositoryWrapper;
}
   
   
public async Task<(OrderItem? orderitem, string? error)> Create(OrderItemForm orderitemForm )
{
    throw new NotImplementedException();
      
}

public async Task<(List<OrderItemDto> orderitems, int? totalCount, string? error)> GetAll(OrderItemFilter filter)
    {
        throw new NotImplementedException();
    }

public async Task<(OrderItem? orderitem, string? error)> Update(Guid id ,OrderItemUpdate orderitemUpdate)
    {
        throw new NotImplementedException();
      
    }

public async Task<(OrderItem? orderitem, string? error)> Delete(Guid id)
    {
        throw new NotImplementedException();
   
    }

}
