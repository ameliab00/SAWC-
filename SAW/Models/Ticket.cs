using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAW.Models
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        public long Id { get; set; }

        [Column("purchase_date")]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        public Guid Barcode { get; set; }

        // Relacje
        
        [ForeignKey("Event")]
        public long eventId { get; set; }
        
        // [ForeignKey("User")]
        // public long userId { get; set; }
        public virtual Event EventEntity { get; set; }
        // public virtual User UserEntity { get; set; }

        public Ticket()
        {
            this.Barcode = Guid.NewGuid(); 
        }
    }
}