using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using AstronomicDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Web.Models.DataAccessMySqlProvider
{
    public class DBMoon : IDbSpaceObject
    {


        /// <summary>
        /// Планета, спутником которой является данный объекта
        /// </summary>
        public string PlanetOwner
        {
            get;
            /*set
            {
                planetOwner = value;
                //PlanetOwner = planetOwner;
                MiddleDistance = planetOwner.MiddleDistance;
                //this.Moons = null;
                Type = PlanetType.Moon;

            }*/
            set;
        }
        
        //DBMoon(DateTime inventingDate, byte[] photo, string name, uint radius, bool hasAtmosphere, string planetOwner, string star, string galaxy, uint temperature = 0) :
        //    base(inventingDate, photo, name, middleDistance, radius, hasAtmosphere, PlanetType.Moon, star, galaxy, temperature)
        //{
        //    PlanetOwner = planetOwner;
        //}

        public Moon ToMoon()
        {
            var moon = new Moon()
            {
                Galaxy = Galaxy, HasAtmosphere = HasAtmosphere, InventingDate = InventingDate,
                MiddleDistance = new Distance(MiddleDistanceValue, MiddleDistanceUnit), Name = Name, Photo = Photo,
                PlanetOwner = PlanetOwner, Radius = Radius, Star = Star, Temperature = Temperature, Moons = { }
            };
            return moon;
        }

        public override string ToString()
        {
            return $"{Name}, Планета: {PlanetOwner}, Радиус: {Radius}";
        }

        //public DBMoon(DBPlanet planetOwner) : this(planetOwner.Star, planetOwner.Galaxy)
        //{
        //    PlanetOwner = planetOwner.Name;
        //    MiddleDistanceValue = planetOwner.MiddleDistanceValue;
        //    MiddleDistanceUnit = planetOwner.MiddleDistanceUnit;
        //    //this.Moons = null;
        //    Type = PlanetType.Moon;

        //}
        public bool HasAtmosphere { get; set; }

        public PlanetType Type { get; set; }
        public string Star { get; set; }

        public DBMoon(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius, bool hasAtmosphere, PlanetType type, string star, string galaxy, uint temperature = 0)
            //base(inventingDate, photo, name, middleDistance, radius, temperature)
        {
            Photo = photo;
            //Database.EnsureCreated();
            HasAtmosphere = hasAtmosphere;
            Type = type;
            Star = star;
            Galaxy = galaxy;
            
            //Moons = new HashSet<DBMoon>();
            //if (moons != null)
            //    foreach (var moon in moons)
            //        Moons.Add(new DBMoon(moon, Id));

            Name = name;
            MiddleDistanceValue = middleDistance.Value;
            MiddleDistanceUnit = middleDistance.Unit;

            Radius = radius;
            Temperature = temperature;
            //Galaxy = galaxy;
            InventingDate = inventingDate;

        }
        public string Galaxy { get; set; }//=> Star.Galaxy;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DBMoon(Moon moon, int plId) : this(moon.InventingDate, moon.Photo, moon.Name, moon.MiddleDistance, moon.Radius, moon.HasAtmosphere, PlanetType.Moon, moon.PlanetOwner, moon.Galaxy)
        {
            PlanetId = plId;
        }

        public DBMoon()
        {
            Type = PlanetType.Moon;
        }

        public int PlanetId { get; set; }
        [ForeignKey("PlanetId")]
        public virtual DBPlanet Planet { get; set; }

        public byte[] Photo { get; set; }
        public string Name { get; set; }
        public uint MiddleDistanceValue { get; set; }
        public UnitType MiddleDistanceUnit { get; set; }
        public uint Radius { get; set; }
        public uint Temperature { get; set; }
        public DateTime InventingDate { get; set; }
    }
}