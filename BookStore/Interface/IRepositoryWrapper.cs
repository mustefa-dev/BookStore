namespace BookStore.Interface;

public interface IRepositoryWrapper
{
    IUserRepository User { get; }

    // here to add
ICartRepository Cart{get;}
ICartProductRepository CartProduct{get;}
ICategoryRepository Category{get;}
IBookRepository Book{get;}


}
