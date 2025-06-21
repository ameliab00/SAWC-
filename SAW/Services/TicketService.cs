using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAW.DTO.Ticket;
using SAW.Models;
using SAW.Repositories;
using SAW.Exceptions;

namespace SAW.Services
{
    public class TicketService
    {
        private readonly TicketRepository _ticketRepository;
        private readonly EventRepository _eventRepository;

        public TicketService(TicketRepository ticketRepository, EventRepository eventRepository)
        {
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
        }

        
        public async Task<List<Ticket>> GetTicketsForEventAsync(long eventId)
        {
            return await _ticketRepository.GetTicketsForEventAsync(eventId);
        }

        
        public async Task<TicketProjections?> GetTicketByIdAsync(long ticketId)
        {
            return await _ticketRepository.FindTicketByIdAsync(ticketId);
        }

        
        public async Task<Ticket> CreateTicketAsync(long eventId)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(eventId);
            if (eventEntity == null)
                throw new EntityNotFoundException($"Wydarzenie o ID {eventId} nie zostało znalezione.");

            if (eventEntity.SeatingCapacity <= 0)
                throw new NoAvailableSeatsException($"Brak dostępnych miejsc na wydarzenie: {eventEntity.Title}");

            eventEntity.SeatingCapacity--;
            await _eventRepository.SaveChangesAsync();

            var ticket = new Ticket
            {
                EventEntity = eventEntity,
                PurchaseDate = DateTime.Now
            };

            return await _ticketRepository.AddAsync(ticket);
        }

        
        public async Task DeleteTicketAsync(long ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
                throw new EntityNotFoundException($"Bilet o ID {ticketId} nie został znaleziony.");

            await _ticketRepository.DeleteAsync(ticket);
        }
    }
}

