using AutoMapper;
using BookStore.DATA;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Repository
{

    public class BookRepository : GenericRepository<Book , Guid> , IBookRepository
    {
        public BookRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
