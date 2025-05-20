using AutoMapper;
using BookStore.DATA;
using BookStore.Entities;
using BookStore.Interface;
using BookStore.Repository;

namespace BookStore.Repository
{

    public class AddressRepository : GenericRepository<Address , Guid> , IAddressRepository
    {
        public AddressRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
