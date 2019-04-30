﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Web.Models.DataAccessPostgreSqlProvider
{
    public class AstronomicDirectoryDbContext : DbContext
    {
        public AstronomicDirectoryDbContext()
        {

            Database.EnsureCreated();
        }

        public AstronomicDirectoryDbContext(DbContextOptions<AstronomicDirectoryDbContext> options) : base(options)
        {
        }

        public DbSet<DBStar> Stars { get; set; }
        //public DbSet<DbFlight> Flights { get; set; }
        public static string ConnectionString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
            //UseNpgsql
            base.OnConfiguring(optionsBuilder);
        }
    }
}