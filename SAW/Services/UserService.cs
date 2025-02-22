using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAW.DTO.User;
using SAW.Models;
using SAW.Repositories;

namespace SAW.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Pobranie użytkownika po nazwie
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.FindByUserNameIgnoreCaseAsync(username);
        }

        // Pobranie listy użytkowników
        public async Task<List<User>> GetUserListAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        // Tworzenie nowego użytkownika
        public async Task<User> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            var existingUser = await _userRepository.FindByUserNameIgnoreCaseAsync(createUserRequest.UserName);
            if (existingUser != null)
                throw new InvalidOperationException("Użytkownik już istnieje.");
            
            var user = new User
            {
                UserName = createUserRequest.UserName,
                Email = createUserRequest.Email
            };
            
            return await _userRepository.AddAsync(user);
        }

        // Usuwanie użytkownika
        public async Task DeleteUserAsync(long userId)
        {
            var userToDelete = await _userRepository.GetByIdAsync(userId);
            if (userToDelete == null)
                throw new InvalidOperationException("Użytkownik nie został znaleziony.");

            await _userRepository.DeleteAsync(userToDelete);
        }
    }
}