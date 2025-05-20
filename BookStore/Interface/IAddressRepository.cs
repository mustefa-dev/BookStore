using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Interface
{
    public interface IAddressRepository : IGenericRepository<Address , Guid>
    {
         
    }
}
