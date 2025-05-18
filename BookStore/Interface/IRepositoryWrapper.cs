namespace BookStore.Interface;

public interface IRepositoryWrapper
{
    IUserRepository User { get; }

    // here to add
IOrderRepository Order{get;}
IOrderItemRepository OrderItem{get;}
ICartRepository Cart{get;}
ICartProductRepository CartProduct{get;}
ICategoryRepository Category{get;}
IBookRepository Book{get;}


}
