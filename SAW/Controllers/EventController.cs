using Microsoft.AspNetCore.Mvc;
using SAW.DTO.Event;
using SAW.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAW.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

     
        [HttpGet]
        public async Task<IActionResult> GetEventList()
        {
            var events = await _eventService.GetEventListAsync();
            if (events == null || events.Count == 0)
            {
                return NotFound(new { Message = "Nie znaleziono żadnych wydarzeń." });
            }

            return Ok(new { Message = "Wydarzenia załadowane pomyślnie.", Events = events });
        }

        
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventById(long eventId)
        {
            var eventItem = await _eventService.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                return NotFound(new { Message = $"Wydarzenie o ID {eventId} nie zostało znalezione." });
            }

            return Ok(new { Message = "Wydarzenie pobrane pomyślnie.", Event = eventItem });
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest createEventRequest)
        {
            if (createEventRequest == null || string.IsNullOrWhiteSpace(createEventRequest.Title) || string.IsNullOrWhiteSpace(createEventRequest.Description))
                return BadRequest(new { Message = "Nieprawidłowe dane wydarzenia." });

            var createdEvent = await _eventService.CreateEventAsync(createEventRequest);

            return CreatedAtAction(nameof(GetEventById), new { eventId = createdEvent.Id },
                new { Message = "Wydarzenie zostało pomyślnie dodane!", Event = createdEvent });
        }

        
        [HttpPatch("{eventId}")]
        public async Task<IActionResult> UpdateEvent(long eventId, [FromBody] UpdateEventRequest updateEventRequest)
        {
            if (updateEventRequest == null || string.IsNullOrWhiteSpace(updateEventRequest.Description))
                return BadRequest(new { Message = "Nieprawidłowe dane do aktualizacji." });

            var updatedEvent = await _eventService.UpdateEventAsync(eventId, updateEventRequest);
            if (updatedEvent == null)
                return NotFound(new { Message = $"Wydarzenie o ID {eventId} nie zostało znalezione." });

            return Ok(new { Message = "Wydarzenie zostało pomyślnie zaktualizowane!", Event = updatedEvent });
        }

        
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> DeleteEvent(long eventId)
        {
            var eventToDelete = await _eventService.GetEventByIdAsync(eventId);
            if (eventToDelete == null)
                return NotFound(new { Message = $"Wydarzenie o ID {eventId} nie zostało znalezione." });

            await _eventService.DeleteEventAsync(eventId);
            return Ok(new { Message = "Wydarzenie zostało pomyślnie usunięte." });
        }

        
        [HttpGet("search")]
        public async Task<IActionResult> SearchEvents([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { Message = "Zapytanie nie może być puste." });
            }

            var events = await _eventService.SearchEventsAsync(query);
            if (events == null || events.Count == 0)
            {
                return NotFound(new { Message = "Nie znaleziono wydarzeń odpowiadających zapytaniu." });
            }

            return Ok(new { Message = "Wydarzenia zostały znalezione.", Events = events });
        }
    }
}
