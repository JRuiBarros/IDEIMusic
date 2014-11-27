using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using idei.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace idei.DAL
{
    public class IDEIContext : DbContext
    {
        public IDEIContext() : base("IDEIContext") { }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Artist> Artists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}