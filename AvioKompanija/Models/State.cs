using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AvioKompanija.Models
{
    public class State
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "State")]
        [StringLength(32)]
        public string Name { get; set; }
        
        public virtual ICollection<City> Cities { get; set; }
    }
}