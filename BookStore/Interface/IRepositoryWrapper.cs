namespace BookStore.Interface;

public interface IRepositoryWrapper
{
    IUserRepository User { get; }

    // here to add
ICategoryRepository Category{get;}
IBookRepository Book{get;}


}
