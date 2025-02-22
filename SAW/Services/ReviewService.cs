using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAW.Models;
using SAW.Repositories;
using SAW.DTO.Review;
using SAW.Exceptions;

namespace SAW.Services
{
    public class ReviewService
    {
        private readonly ReviewRepository _reviewRepository;
        private readonly EventRepository _eventRepository;

        public ReviewService(ReviewRepository reviewRepository, EventRepository eventRepository)
        {
            _reviewRepository = reviewRepository;
            _eventRepository = eventRepository;
        }

        // Pobranie recenzji dla wydarzenia
        public async Task<List<Review>> GetReviewsForEventAsync(long eventId)
        {
            return await _reviewRepository.SortedListOfReviewForEventAsync(eventId);
        }

        // Tworzenie nowej recenzji
        public async Task<Review> CreateReviewAsync(long eventId, CreateReviewRequest createReviewRequest)
        {
            // Pobieramy wydarzenie z repozytorium
            var eventEntity = await _eventRepository.GetByIdAsync(eventId);
            if (eventEntity == null)
                throw new EntityNotFoundException($"Wydarzenie o ID {eventId} nie zostało znalezione.");

            // Walidacja danych recenzji
            if (createReviewRequest == null)
                throw new ArgumentNullException(nameof(createReviewRequest));

            if (string.IsNullOrEmpty(createReviewRequest.Content))
                throw new ArgumentException("Content cannot be null or empty");

            if (createReviewRequest.Rating < 1 || createReviewRequest.Rating > 5)
                throw new ArgumentOutOfRangeException("Rating must be between 1 and 5");

            // Tworzenie obiektu recenzji
            var review = new Review
            {
                Event = eventEntity,
                Title = createReviewRequest.Title,
                Content = createReviewRequest.Content,
                Rating = createReviewRequest.Rating,
                CreatedAt = DateTime.Now
            };

            // Zapisanie recenzji
            return await _reviewRepository.AddAsync(review);
        }

        // Usuwanie recenzji
        public async Task DeleteReviewAsync(long reviewId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null)
                throw new EntityNotFoundException($"Recenzja o ID {reviewId} nie została znaleziona.");

            await _reviewRepository.DeleteAsync(review);
        }
    }
}
