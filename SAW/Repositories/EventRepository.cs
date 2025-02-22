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

        // Metoda do wyszukiwania po tytule, ignorując wielkość liter
        public async Task<Event?> FindByTitleIgnoreCaseAsync(string title)
        {
            return await _context.Set<Event>()
                .FirstOrDefaultAsync(e => e.Title.ToLower() == title.ToLower());
        }

        // Wyszukiwanie po tytule, ignorując wielkość liter
        public async Task<List<Event>> SearchByTitleLikeIgnoreCaseAsync(string query)
        {
            return await _context.Set<Event>()
                .Where(e => EF.Functions.Like(e.Title.ToLower(), $"%{query.ToLower()}%"))
                .ToListAsync();
        }

        // Zwracanie listy eventów posortowanych po dacie rozpoczęcia
        public async Task<List<Event>> SortedListOfEventsAsync()
        {
            return await _context.Set<Event>()
                .Where(e => e.EndingDate >= DateTime.Now)
                .OrderBy(e => e.StartingDate)
                .ToListAsync();
        }

        // Dodanie nowego wydarzenia
        public async Task AddAsync(Event eventEntity)
        {
            await _context.Set<Event>().AddAsync(eventEntity);
            await _context.SaveChangesAsync();
        }

        // Sprawdzanie, czy wydarzenie o danym tytule już istnieje
        public async Task<bool> ExistsByTitleAsync(string title)
        {
            return await _context.Set<Event>()
                .AnyAsync(e => e.Title.ToLower() == title.ToLower());
        }

        // Pobieranie wydarzenia po ID
        public async Task<Event?> GetByIdAsync(long eventId)
        {
            return await _context.Set<Event>().FindAsync(eventId);
        }

        // Zapisanie zmian w bazie danych
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Usuwanie wydarzenia
        public async Task DeleteAsync(Event eventEntity)
        {
            _context.Set<Event>().Remove(eventEntity);
            await _context.SaveChangesAsync();
        }

        // Usuwanie wielu obiektów
        public async Task DeleteRangeAsync(IEnumerable<Event> eventEntities)
        {
            _context.Set<Event>().RemoveRange(eventEntities);
            await _context.SaveChangesAsync();
        }
    }
}

