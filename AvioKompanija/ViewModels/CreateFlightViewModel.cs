using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AvioKompanija.ViewModels
{
    public class CreateFlightViewModel
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Departure time")]
        public DateTime Date { get; set; }

        [Required]
        [Range(0,24)]
        public int Hours { get; set; }

        [Required]
        [Range(0, 60)]
        public int Minutes { get; set; }

        [Required]
        public int FromAirportId { get; set; }
        [Required]
        public int ToAirportId { get; set; }
        [Required]
        public int AvionId { get; set; }

    }
}