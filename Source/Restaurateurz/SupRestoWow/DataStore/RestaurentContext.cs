using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SupRestoWow.Models;

namespace SupRestoWow.DataStore
{
    public class RestaurentContext : DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Compte>().ToTable("comptes");
            base.OnModelCreating(modelBuilder);
        }
        public RestaurentContext()
            : base("OnlyConnection")
        {

        }
    }
}