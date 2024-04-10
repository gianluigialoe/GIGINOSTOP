using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GIGINOSTOP.Models
{
    public class RegisterViewModel
    {
        public string nome { get; set; }


        public string cognome { get; set; }


        public string email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "La password e la conferma password non corrispondono.")]
        [Display(Name = "Conferma password")]
        public string confermapassword { get; set; }

        [StringLength(50)]
        public string ruolo { get; set; }

    }
}