using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SupRestoWow.DataStore
{
    public class RestaurentContext : DbContext
    {
        public RestaurentContext()
            : base("OnlyConnection")
        {

        }
    }
}