﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AstronomicDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;


namespace Web.Models.DataAccessMySqlProvider
{
    public class DBPlanet : IDbSpaceObject
    {
        /*public Planet(Bitmap photo, string name, Distance middleDistance, uint radius, uint temperature) : base(photo, name, middleDistance, radius, temperature)
            {
            }*/

        public DBPlanet(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius, bool hasAtmosphere, PlanetType type, DBStar star, uint temperature = 0, HashSet<Moon> moons = null)
            //base(inventingDate, photo, name, middleDistance, radius, temperature)
        {

            //Database.EnsureCreated();
            HasAtmosphere = hasAtmosphere;
            Type = type;
            Star = star.Name;

            Moons = new Collection<DBMoon>();
            if (moons != null)
                foreach (var moon in moons)
                    Moons.Add(new DBMoon(moon, Id));

            Name = name;
            MiddleDistanceValue = middleDistance.Value;
            MiddleDistanceUnit = middleDistance.Unit;

            Radius = radius;
            Temperature = temperature;
            //Galaxy = galaxy;
            InventingDate = inventingDate;

        }

        public DBPlanet(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius, bool hasAtmosphere, PlanetType type, string star, string galaxy, uint temperature = 0, HashSet<Moon> moons = null)
            //base(inventingDate, photo, name, middleDistance, radius, temperature)
        {
            HasAtmosphere = hasAtmosphere;
            Type = type;
            Moons = new Collection<DBMoon>();
            if(moons != null)
                foreach (var moon in moons)
                    Moons.Add(new DBMoon(moon, Id));
            //else Moons = new HashSet<DBMoon>();

            Photo = photo;

            Name = name;
            MiddleDistanceValue = middleDistance.Value;
            MiddleDistanceUnit = middleDistance.Unit;

            Radius = radius;
            Temperature = temperature;
            //Galaxy = galaxy;
            InventingDate = inventingDate;

            //Database.EnsureCreated();
            Type = type;
            Star = star;
            Galaxy = galaxy;
        }

        public Planet ToPlanet()
        {
            var planet = new Planet(InventingDate, Photo, Name, new Distance(MiddleDistanceValue, MiddleDistanceUnit), Radius, HasAtmosphere, Type, Star, Galaxy, Temperature, Moons.Select(m => m.ToMoon()));
            return planet;
        }

        public override bool Equals(object obj)
        {
            var pl = obj as DBPlanet;
            return pl != null && pl.Name != null && pl.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name}, Звезда: {Star}, Радиус: {Radius}";
        }

        //public DBPlanet(DBStar owner)
        //{
        //    Star = owner.Name;
        //    Galaxy = owner.Galaxy;
        //    Moons = new HashSet<DBMoon>();
        //}

        public DBPlanet(string owner, string galaxy)
        {
            //global::
            Star = owner;
            Galaxy = galaxy;//owner.Galaxy;
            Moons = new Collection<DBMoon>();
        }

        public DBPlanet()
        {
            Moons = new Collection<DBMoon>();

        }

        public bool HasAtmosphere { get; set; }

        public PlanetType Type { get; set; }
        /// <summary>
        /// Звезда, вокруг которой вращается планета
        /// </summary>
        public string Star { get; set; }

        public virtual Collection<DBMoon> Moons { get; set; }

        /// <summary>
        /// Название галактики, в которой расположен объект
        /// </summary>
        public string Galaxy { get; set; }//=> Star.Galaxy;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //public DBPlanet(bool hasAtmosphere, PlanetType type, string star, HashSet<DBMoon> moons, string galaxy, int id, int starShipId, DBStar starOwner, byte[] photo, string name, uint middleDistanceValue, UnitType middleDistanceUnit, uint radius, uint? temperature, DateTime inventingDate)
        //{
        //    HasAtmosphere = hasAtmosphere;
        //    Type = type;
        //    Star = star;
        //    Moons = moons;
        //    Galaxy = galaxy;
        //    Id = id;
        //    StarShipId = starShipId;
        //    StarOwner = starOwner;
        //    Photo = photo;
        //    Name = name;
        //    MiddleDistanceValue = middleDistanceValue;
        //    MiddleDistanceUnit = middleDistanceUnit;
        //    Radius = radius;
        //    Temperature = temperature;
        //    InventingDate = inventingDate;
        //}

        public int StarId { get; set; }
        [ForeignKey("StarId")]
        public virtual DBStar StarOwner { get; set; }

        public byte[] Photo { get; set; }
        public string Name { get; set; }
        public uint MiddleDistanceValue { get; set; }
        public UnitType MiddleDistanceUnit { get; set; }

        //public Distance MiddleDistance { get; set; }
        public uint Radius { get; set; }
        public uint Temperature { get; set; }
        public DateTime InventingDate { get; set; }

        DBPlanet(Planet planet) : this(planet.InventingDate, planet.Photo, planet.Name, planet.MiddleDistance, planet.Radius, planet.HasAtmosphere, planet.Type, planet.Star, planet.Galaxy, planet.Temperature, planet.Moons)
        {
        }

        public DBPlanet(Planet planet, int starId) : this(planet)//this(planet.InventingDate, planet.Photo, planet.Name, planet.MiddleDistance, planet.Radius, planet.HasAtmosphere, planet.Type, planet.Star, planet.Galaxy, planet.Temperature)
        {
            this.StarId = starId;
        }
        //public class AstronomicDictionaryDb
        //{

        //}
    }
}