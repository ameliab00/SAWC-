using System.ComponentModel.DataAnnotations;
using SAW.Models;

namespace SAW.DTO.User
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Username is mandatory")]
        [StringLength(50, ErrorMessage = "Username length should be less than 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        [StringLength(40, ErrorMessage = "Password cannot be longer than 40 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is mandatory")]
        public string Email { get; set; }
        
        public UserRole UserRole { get; set; }
    }
}