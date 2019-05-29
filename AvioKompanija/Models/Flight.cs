using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AvioKompanija.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal price { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name ="Departure time")]
        public DateTime time { get; set; }

        public int TicketsLeft { get; set; }

        public int FromAirportId { get; set; }
        public int ToAirportId { get; set; }

        [Display(Name = "From")]
        public virtual Airport FromAirport { get; set; }

        [Display(Name = "To")]
        public virtual Airport ToAirport { get; set; }

        public int AvionId { get; set; }
        [Display(Name = "Plane")]
        public virtual Avion Avion { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }

    }
}