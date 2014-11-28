using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using idei.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace idei.DAL
{
    public class DefaultConnection : DbContext
    {
        public DefaultConnection() : base("DefaultConnection") { }
        public DbSet<Record> Records { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Format> Formats { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}