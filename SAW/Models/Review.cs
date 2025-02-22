using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAW.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Title is mandatory")]
        [StringLength(255, ErrorMessage = "Title length should be less than 255 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is mandatory")]
        [StringLength(100, ErrorMessage = "Content length must be less than or equal to 100 characters")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Rating is mandatory")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Ustalamy domyślną wartość przy tworzeniu
        
        // Relacje
        [ForeignKey("User")]
        public long userId { get; set; }
        
        [ForeignKey("Event")]
        public long eventId { get; set; }

        
        public virtual User User { get; set; }
        
        public virtual Event Event { get; set; }
    }
}