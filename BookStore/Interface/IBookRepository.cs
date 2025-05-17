using BookStore.Entities;

namespace BookStore.Interface
{
    public interface IBookRepository : IGenericRepository<Book , Guid>
    {
         
    }
}
