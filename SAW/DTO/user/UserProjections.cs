using System;
using System.Collections.Generic;
using SAW.Models;

namespace SAW.DTO.User
{
    public class UserProjections
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserRole UserRole { get; set; }
        public Models.Event EventEntity { get; set; }
        public HashSet<Models.Ticket> TicketEntities { get; set; }
    }
}