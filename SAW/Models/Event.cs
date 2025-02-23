using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SAW.Models
{
    [Table("Events")]
    public class Event
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Title is mandatory")]
        [StringLength(255, ErrorMessage = "Title length should be less than 255 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Location is mandatory")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Price is mandatory")]
        public double Price { get; set; }

        [Column("starting_date")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "01/01/2023", "12/31/2100", ErrorMessage = "Starting date should be future or present")]
        public DateTime StartingDate { get; set; }

        [Column("ending_date")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "01/01/2023", "12/31/2100", ErrorMessage = "Ending date should be future")]
        public DateTime EndingDate { get; set; }

        [Column("seating_capacity")]
        [Required(ErrorMessage = "Seating capacity is mandatory")]
        public int SeatingCapacity { get; set; }

        [Required(ErrorMessage = "Description is mandatory")]
        public string Description { get; set; }

        // Relacje
        // public virtual ICollection<User> UserEntities { get; set; }

        public virtual ICollection<Ticket> TicketEntities { get; set; } = new HashSet<Ticket>();
    }
}