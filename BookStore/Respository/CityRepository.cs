using AutoMapper;
using BookStore.DATA;
using BookStore.Entities;
using BookStore.Interface;
using BookStore.Repository;

namespace BookStore.Respository
{

    public class CityRepository : GenericRepository<City , Guid> , ICityRepository
    {
        public CityRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
