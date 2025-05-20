using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Interface
{
    public interface ICityRepository : IGenericRepository<City , Guid>
    {
         
    }
}
