using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupRestoWow.Models
{
    public class Compte
    {
        [Required]
        public string Nom { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        [Required]
        public string Adresse { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Courriel { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }


        public int Id { get; set; }
    }
}