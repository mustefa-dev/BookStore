using BookStore.Entities;

namespace BookStore.DATA.DTOs.User
{
    public class UserFilter : BaseFilter
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public UserRole? Role { get; set; }
    }
}