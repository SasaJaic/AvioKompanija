using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AvioKompanija.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression("^[A-Z][a-z]{1,13}$", ErrorMessage = "Ime mora imati samo slova i maksimalno 14 karaktera!")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[A-Z][a-z]{1,13}$", ErrorMessage = "Prezime mora imati samo slova i maksimalno 14 karaktera!")]
        public string LastName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordRepeat { get; set; }


    }
}