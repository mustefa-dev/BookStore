using System.ComponentModel.DataAnnotations;
using BookStore.DATA.DTOs.User;
using BookStore.Entities;

namespace BookStore.DATA.DTOs.User
{
    public class RegisterForm
    {
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string? Password { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        [Required]
        [MinLength(2, ErrorMessage = "FullName must be at least 2 characters")]
        public string? FullName { get; set; }
        public UserRole? Role { get; set; }                
        
    }
}

