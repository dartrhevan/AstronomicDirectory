using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AstronomicDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;


namespace Web.Models.DataAccessPostgreSqlProvider
{
    public class DBPlanet : ISpaceObject
    {
        /*public Planet(Bitmap photo, string name, Distance middleDistance, uint radius, uint temperature) : base(photo, name, middleDistance, radius, temperature)
            {
            }*/

        public DBPlanet(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius, bool hasAtmosphere, PlanetType type, DBStar star, uint temperature = 0, IEnumerable<DBMoon> moons = null)
            //base(inventingDate, photo, name, middleDistance, radius, temperature)
        {

            //Database.EnsureCreated();
            HasAtmosphere = hasAtmosphere;
            Type = type;
            Star = star.Name;
            if (moons != null) Moons = new HashSet<DBMoon>(moons);
            Photo = photo;

            Name = name;
            MiddleDistance = middleDistance;
            Radius = radius;
            Temperature = temperature;
            //Galaxy = galaxy;
            InventingDate = inventingDate;

        }

        public DBPlanet(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius, bool hasAtmosphere, PlanetType type, string star, string galaxy, uint temperature = 0, IEnumerable<DBMoon> moons = null)
            //base(inventingDate, photo, name, middleDistance, radius, temperature)
        {

            //Database.EnsureCreated();
            HasAtmosphere = hasAtmosphere;
            Type = type;
            Star = Name;
            Galaxy = galaxy;

            if (moons != null) Moons = new HashSet<DBMoon>(moons);
        }


        public override bool Equals(object obj)
        {
            var pl = obj as DBPlanet;
            return pl != null && pl.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name}, Звезда: {Star}, Радиус: {Radius}";
        }

        public DBPlanet(DBStar owner)
        {
            Star = owner.Name;
            Galaxy = owner.Galaxy;
            Moons = new HashSet<DBMoon>();
        }

        public DBPlanet(string owner, string galaxy)
        {
            //global::
            Star = owner;
            Galaxy = galaxy;//owner.Galaxy;
            Moons = new HashSet<DBMoon>();
        }

        public DBPlanet()
        {
            Moons = new HashSet<DBMoon>();

        }

        public bool HasAtmosphere { get; set; }

        public PlanetType Type { get; set; }
        /// <summary>
        /// Звезда, вокруг которой вращается планета
        /// </summary>
        public string Star { get; set; }

        public readonly HashSet<DBMoon> Moons;

        /// <summary>
        /// Название галактики, в которой расположен объект
        /// </summary>
        public string Galaxy { get; set; }//=> Star.Galaxy;


        public byte[] Photo { get; set; }
        public string Name { get; set; }

        public Distance MiddleDistance { get; set; }
        public uint Radius { get; set; }
        public uint? Temperature { get; set; }
        public DateTime InventingDate { get; set; }

        //public class AstronomicDictionaryDb
        //{

        //}
    }
}