using AutoMapper;
using BookStore.DATA;
using BookStore.Entities;
using BookStore.Interface;
using BookStore.Repository;

namespace BookStore.Repository
{

    public class DistrictRepository : GenericRepository<District , Guid> , IDistrictRepository
    {
        public DistrictRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
