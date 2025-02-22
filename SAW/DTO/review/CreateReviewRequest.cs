using System;
using System.ComponentModel.DataAnnotations;

namespace SAW.DTO.Review
{
    public class CreateReviewRequest
    {
        [Required(ErrorMessage = "Title is mandatory")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is mandatory")]
        [StringLength(100, ErrorMessage = "Content length must be less than or equal to 100 characters")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Rating is mandatory")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
    }
}