using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LacteosTolima.App.Models
{
    public class CowsDBContext : DbContext
    {
        public DbSet<Production> Productions { get; set; }
        public DbSet<Cow> Cows { get; set; }
        public DbSet<Herd> Herds { get; set; }
        public DbSet<Milker> Milkers { get; set; }
        public DbSet<Consumption> Consumptions { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cow>()
                        .HasOptional(c => c.Mother)
                        .WithMany()
                        .HasForeignKey(c => c.MotherId);
        }
    }
}