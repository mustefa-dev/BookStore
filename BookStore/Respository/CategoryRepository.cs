using AutoMapper;
using BookStore.DATA;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Repository
{

    public class CategoryRepository : GenericRepository<Category , Guid> , ICategoryRepository
    {
        public CategoryRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
