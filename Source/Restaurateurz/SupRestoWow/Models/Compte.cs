using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupRestoWow.Models
{
    public class Compte
    {
        public string NomUtilisateur { get; set; }

        [DataType(DataType.Password)]
        public string MotDePasse { get; set; }
    }
}