using AvioKompanija.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AvioKompanija.ViewModels
{
    public class BookFlightViewModel
    {
        public Flight Flight { get; set; }
        [Range(1, 4, ErrorMessage = "You can book 1 up to 4 tickets for one flight!")]
        public int NumOfTickets { get; set; }
    }
}