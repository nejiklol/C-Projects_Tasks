using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Projects.Models;

// This file implements the connection to the database
// Also DBSet variables are created
// Which are responsible for the connection with the tables of the database

namespace MySQLApp
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Tasks> tasks { get; set; }
        public DbSet<Project> projects { get; set; }

        // The ApplicationContext function creates a database
        // migration and allows you to exchange information with it
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        // Configuration of the database connection, filling the
        // database address of the user name and the user name
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseMySql(
                "server=localhost;user=root;database=projectstasks;",
                new MySqlServerVersion(new Version(8, 0, 11))
            );
        }
    }
}