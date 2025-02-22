using System.ComponentModel.DataAnnotations;

namespace SAW.DTO.User
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Username is mandatory")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        [StringLength(40, ErrorMessage = "Password cannot be longer than 40 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress(ErrorMessage = "Email should be valid")]
        public string Email { get; set; }
    }
}