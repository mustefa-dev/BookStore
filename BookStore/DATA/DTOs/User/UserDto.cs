namespace BookStore.DATA.DTOs.User
{
    public class UserDto : BaseDto<Guid>
    {
        public string? FullName { get; set; }
        
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
        

    }
}