using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AvioKompanija.Models
{
    public class Airport
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<Flight> FromFlights { get; set; }
        public virtual ICollection<Flight> ToFlights { get; set; }
    }
}