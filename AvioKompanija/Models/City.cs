using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AvioKompanija.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "City")]
        [StringLength(32)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "State")]
        public int StateId { get; set; }
        public virtual State State { get; set; }

        public virtual ICollection<Airport> Airports { get; set; }
    }
}