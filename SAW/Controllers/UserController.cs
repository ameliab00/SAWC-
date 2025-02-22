using Microsoft.AspNetCore.Mvc;
using SAW.Models;
using SAW.DTO.User;
using SAW.Services;
using System;
using System.Threading.Tasks;

namespace SAW.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // Pobranie listy użytkowników
        [HttpGet]
        public async Task<IActionResult> GetUserList()
        {
            try
            {
                var users = await _userService.GetUserListAsync();
                return Ok(new { Message = "Lista użytkowników pobrana pomyślnie.", Users = users });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Błąd podczas pobierania listy użytkowników: {ex.Message}" });
            }
        }

        // Pobranie użytkownika po nazwie
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            try
            {
                var user = await _userService.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return NotFound(new { Message = "Użytkownik nie został znaleziony." });
                }
                return Ok(new { Message = "Użytkownik znaleziony.", User = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Błąd podczas wyszukiwania użytkownika: {ex.Message}" });
            }
        }

        // Tworzenie nowego użytkownika
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var user = await _userService.CreateUserAsync(createUserRequest);
                return CreatedAtAction(nameof(GetUserByUsername), new { username = user.UserName },
                    new { Message = "Użytkownik został pomyślnie utworzony.", User = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Błąd podczas tworzenia użytkownika: {ex.Message}" });
            }
        }

        // Usuwanie użytkownika
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(long userId)
        {
            try
            {
                await _userService.DeleteUserAsync(userId);
                return Ok(new { Message = "Użytkownik został pomyślnie usunięty." });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = $"Błąd podczas usuwania użytkownika: {ex.Message}" });
            }
        }
    }
}
