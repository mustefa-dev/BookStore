namespace BookStore.DATA.DTOs.User
{
    public class UpdateUserForm
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? GovernmentName { get; set; }
        public bool? IsActive { get; set; }
    }
}