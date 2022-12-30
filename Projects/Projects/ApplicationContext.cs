using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Projects.Models;

namespace MySQLApp
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Tasks> tasks { get; set; }
        public DbSet<Project> projects { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseMySql(
                "server=localhost;user=root;database=projectstasks;",
                new MySqlServerVersion(new Version(8, 0, 11))
            );
        }
    }
}