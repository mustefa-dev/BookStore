using BookStore.Entities;

namespace BookStore.Interface
{
    public interface ICategoryRepository : IGenericRepository<Category , Guid>
    {
         
    }
}
