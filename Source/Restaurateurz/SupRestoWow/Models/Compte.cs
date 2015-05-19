using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupRestoWow.Models
{
    public class Compte
    {
        public string Nom { get; set; }

        [DataType(DataType.Password)]
        public string MotDePasse { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        public string Adresse { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Courriel { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        public int Id { get; set; }
    }
}