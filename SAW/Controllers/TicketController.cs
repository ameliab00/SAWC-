using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SAW.Models;
using SAW.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using SAW.Exceptions;

namespace SAW.Controllers
{
    [Route("ticket")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(TicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
        }

        // Pobierz listę biletów dla wydarzenia
        [HttpGet("{eventId}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsForEvent(long eventId)
        {
            _logger.LogInformation("Fetching list of tickets for event with id: {EventId}", eventId);
            var tickets = await _ticketService.GetTicketsForEventAsync(eventId);
            if (tickets == null || tickets.Count == 0)
            {
                return NotFound(new { Message = "Brak biletów na to wydarzenie." });
            }
            return Ok(new { Message = "Lista biletów pobrana pomyślnie.", Tickets = tickets });
        }

        // Pobierz bilet po ID
        [HttpGet("details/{ticketId}")]
        public async Task<ActionResult<Ticket>> GetTicketById(long ticketId)
        {
            _logger.LogInformation("Fetching ticket with id: {TicketId}", ticketId);
            var ticket = await _ticketService.GetTicketByIdAsync(ticketId);
            if (ticket == null)
            {
                return NotFound(new { Message = $"Bilet o ID {ticketId} nie został znaleziony." });
            }
            return Ok(new { Message = "Bilet pobrany pomyślnie.", Ticket = ticket });
        }

        // Tworzenie nowego biletu
        [HttpPost("{eventId}")]
        public async Task<ActionResult<Ticket>> CreateTicket(long eventId)
        {
            _logger.LogInformation("Creating ticket for event with id: {EventId}", eventId);
            try
            {
                var ticket = await _ticketService.CreateTicketAsync(eventId);
                return CreatedAtAction(nameof(GetTicketById), new { ticketId = ticket.Id },
                    new { Message = "Bilet utworzony pomyślnie.", Ticket = ticket });
            }
            catch (NoAvailableSeatsException)
            {
                return BadRequest(new { Message = "Brak dostępnych miejsc na to wydarzenie." });
            }
        }

        // Usuwanie biletu
        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> DeleteTicket(long ticketId)
        {
            _logger.LogInformation("Deleting ticket with id: {TicketId}", ticketId);

            try
            {
                await _ticketService.DeleteTicketAsync(ticketId);
                return Ok(new { Message = "Bilet usunięty pomyślnie." });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}

