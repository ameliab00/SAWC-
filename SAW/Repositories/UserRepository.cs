using Microsoft.EntityFrameworkCore;
using SAW.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SAW.Infrastructure;

namespace SAW.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<User?> FindByUserNameIgnoreCaseAsync(string userName)
        {
            return await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
        }
        
        
        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Set<User>().ToListAsync();
        }

        
        public async Task<User?> GetByIdAsync(long userId)
        {
            return await _context.Set<User>().FindAsync(userId);
        }

        
        public async Task<User> AddAsync(User user)
        {
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        
        public async Task DeleteAsync(User user)
        {
            _context.Set<User>().Remove(user);
            await _context.SaveChangesAsync();
        }

        
        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}