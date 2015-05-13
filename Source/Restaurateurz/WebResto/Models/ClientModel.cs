using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebResto.Models
{
    public class ClientModel
    {   
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}