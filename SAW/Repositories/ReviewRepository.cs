using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using SAW.Infrastructure;
using SAW.Models;

namespace SAW.Repositories
{
    public class ReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<List<Review>> SortedListOfReviewForEventAsync(long eventId)
        {
            return await _context.Set<Review>()
                .Where(r => r.Event.Id == eventId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        
        public async Task<Review> AddAsync(Review review)
        {
            await _context.Set<Review>().AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        
        public async Task DeleteAsync(Review review)
        {
            _context.Set<Review>().Remove(review);
            await _context.SaveChangesAsync();
        }

        
        public async Task<Review> GetByIdAsync(long reviewId)
        {
            return await _context.Set<Review>().FindAsync(reviewId);
        }
    }
}