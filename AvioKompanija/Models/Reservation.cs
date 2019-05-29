using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AvioKompanija.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Number of tickets")]
        public int NumberOfTickets { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [Display(Name = "Flight")]
        public int FlightId { get; set; }
        public virtual Flight Flight { get; set; }
        

    }
}