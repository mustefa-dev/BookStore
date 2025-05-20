using BookStore.DATA.DTOs.Book;

namespace BookStore.DATA.DTOs.Category
{

    public class CategoryDto : BaseDto<Guid>
    {
        public string Name { get; set; }
        public List<BookDto> books { get; set; }    
    }
}
