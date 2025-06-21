using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAW.Infrastructure;
using SAW.Models;

namespace SAW.Repositories
{
    public class EventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<Event?> FindByTitleAsync(string title)
        {
            return await _context.Set<Event>()
                .FirstOrDefaultAsync(e => e.Title == title);
        }

        
        public async Task<List<Event>> SearchByTitleAsync(string query)
        {
            return await _context.Set<Event>()
                .Where(e => e.Title.Contains(query))
                .ToListAsync();
        }
        
        public async Task<List<Event>> SortedListOfEventsAsync()
        {
            return await _context.Set<Event>()
                .Where(e => e.EndingDate >= DateTime.Now)
                .OrderBy(e => e.StartingDate)
                .ToListAsync();
        }

        
        public async Task AddAsync(Event eventEntity)
        {
            await _context.Set<Event>().AddAsync(eventEntity);
            await _context.SaveChangesAsync();
        }

        
        public async Task<bool> ExistsByTitleAsync(string title)
        {
            return await _context.Set<Event>()
                .AnyAsync(e => e.Title == title);
        }

        
        public async Task<Event?> GetByIdAsync(long eventId)
        {
            return await _context.Set<Event>().FindAsync(eventId);
        }

        
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        
        public async Task DeleteAsync(Event eventEntity)
        {
            _context.Set<Event>().Remove(eventEntity);
            await _context.SaveChangesAsync();
        }

        
        public async Task DeleteRangeAsync(IEnumerable<Event> eventEntities)
        {
            _context.Set<Event>().RemoveRange(eventEntities);
            await _context.SaveChangesAsync();
        }
    }
}