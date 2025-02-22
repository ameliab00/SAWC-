using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SAW.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Username is mandatory")]
        [StringLength(50, ErrorMessage = "Username length should be less than 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is mandatory")]
        // [EmailAddress(ErrorMessage = "Email should be valid")]
        public string Email { get; set; }

        [Required]
        public UserRole UserRole { get; set; }

        // Relacje
        
        public virtual ICollection<Event> EventEntities { get; set; }
        
        
        public virtual ICollection<Ticket> TicketEntities { get; set; } = new HashSet<Ticket>();
    }
}