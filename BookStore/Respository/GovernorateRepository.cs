using AutoMapper;
using BookStore.DATA;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Repository;

public class GovernorateRepository : GenericRepository<Governorate, Guid>, IGovernorateRepository
{
    public GovernorateRepository(DataContext context, IMapper mapper) : base(context, mapper)
    {
    }
}