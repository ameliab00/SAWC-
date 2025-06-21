using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SAW.Exceptions;
using SAW.Repositories;
using SAW.Models;
using SAW.DTO.Event;
using SAW.Mappers;

namespace SAW.Services
{
    public class EventService
    {
        private readonly EventRepository _eventRepository;
        private readonly UpdateEventMapper _updateEventMapper;
        private readonly TicketRepository _ticketRepository;
        private readonly UserRepository _userRepository;

        public EventService(EventRepository eventRepository, UpdateEventMapper updateEventMapper, TicketRepository ticketRepository, UserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _updateEventMapper = updateEventMapper;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        
        public async Task<List<Event>> GetEventListAsync()
        {
            return await _eventRepository.SortedListOfEventsAsync();
        }

        
        public async Task<Event> CreateEventAsync(CreateEventRequest request)
        {
            if (await _eventRepository.FindByTitleAsync(request.Title) != null)
            {
                throw new DuplicateException($"Wydarzenie o tytule {request.Title} już istnieje.");
            }

            var eventEntity = new Event
            {
                Title = request.Title,
                Location = request.Location,
                Price = request.Price,
                StartingDate = request.StartingDate,
                EndingDate = request.EndingDate,
                SeatingCapacity = request.SeatingCapacity,
                Description = request.Description
            };

            await _eventRepository.AddAsync(eventEntity);
            await _eventRepository.SaveChangesAsync();

            return eventEntity;
        }

        
        public async Task<UpdateEventResponse> UpdateEventAsync(long eventId, UpdateEventRequest updateEventRequest)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(eventId);
            if (eventEntity == null)
            {
                throw new EntityNotFoundException($"Wydarzenie o ID {eventId} nie zostało znalezione.");
            }

            eventEntity = _updateEventMapper.ToEntity(eventEntity, updateEventRequest);
            await _eventRepository.SaveChangesAsync();

            return new UpdateEventResponse
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Location = eventEntity.Location,
                Price = eventEntity.Price,
                StartingDate = eventEntity.StartingDate,
                EndingDate = eventEntity.EndingDate,
                SeatingCapacity = eventEntity.SeatingCapacity,
                Description = eventEntity.Description
            };
        }

        
        public async Task DeleteEventAsync(long eventId)
        {
            var eventToDelete = await _eventRepository.GetByIdAsync(eventId);

            if (eventToDelete == null)
            {
                throw new EntityNotFoundException($"Wydarzenie o ID {eventId} nie zostało znalezione.");
            }
            
 
            if (eventToDelete.TicketEntities != null)
            {
                foreach (var ticket in eventToDelete.TicketEntities)
                {
                    // ticket.UserEntity = null;
                    ticket.EventEntity = null;
                }

                await _ticketRepository.SaveChangesAsync();
                await _ticketRepository.DeleteTicketsAsync(eventToDelete.TicketEntities);
            }

            await _eventRepository.DeleteAsync(eventToDelete);
        }

        
        public async Task<List<Event>> SearchEventsAsync(string query)
        {
            return await _eventRepository.SearchByTitleAsync(query);
        }

        public async Task<Event?> GetEventByIdAsync(long eventId)
        {
            return await _eventRepository.GetByIdAsync(eventId);
        }
    }
}
