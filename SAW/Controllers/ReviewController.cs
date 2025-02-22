using Microsoft.AspNetCore.Mvc;
using SAW.Models;
using SAW.DTO.Review;
using SAW.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAW.Controllers
{
    [Route("review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // // Pobiera wszystkie recenzje użytkownika
        // [HttpGet("user/{userId}")]
        // public async Task<IActionResult> GetAllUserReviews(long userId)
        // {
        //     var reviews = await _reviewService.GetUserReviewsAsync(userId);
        //     if (reviews == null || reviews.Count == 0)
        //         return NotFound(new { message = "Brak recenzji dla tego użytkownika." });
        //
        //     return Ok(new { message = "Recenzje użytkownika załadowane pomyślnie.", data = reviews });
        // }

        // Pobiera wszystkie recenzje dla danego wydarzenia
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetReviewsForEvent(long eventId)
        {
            var reviews = await _reviewService.GetReviewsForEventAsync(eventId);
            if (reviews == null || reviews.Count == 0)
                return NotFound(new { message = "Brak recenzji dla tego wydarzenia." });

            return Ok(new { message = "Recenzje wydarzenia załadowane pomyślnie.", data = reviews });
        }

        // Tworzy nową recenzję dla wydarzenia
        [HttpPost("{eventId}")]
        public async Task<IActionResult> CreateReview(long eventId, [FromBody] CreateReviewRequest createReviewRequest)
        {
            if (createReviewRequest == null)
                return BadRequest(new { message = "Żądanie recenzji nie może być puste." });

            try
            {
                var review = await _reviewService.CreateReviewAsync(eventId, createReviewRequest);
                return CreatedAtAction(nameof(GetReviewsForEvent), new { eventId = eventId }, new { message = "Recenzja została pomyślnie dodana!", data = review });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Usuwa istniejącą recenzję
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(long reviewId)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(reviewId);
                return Ok(new { message = "Recenzja została pomyślnie usunięta." });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

