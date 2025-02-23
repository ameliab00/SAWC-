using System;
using SAW.Models;

namespace SAW.DTO.Ticket
{
    public class TicketProjections
    {
        public long Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid Barcode { get; set; }
        public Models.Event EventEntity { get; set; }
        // public Models.User UserEntity { get; set; }
    }
}