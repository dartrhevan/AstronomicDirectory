using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AstronomicDirectory;
using Web.Models.DataAccessMySqlProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Web.Models
{
    namespace DataAccessMySqlProvider
    {
        public class DBStar : IDbSpaceObject
        {
            public DBStar(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius,
                    uint temperature, IEnumerable<Planet> planets, string galaxy)
            //base(inventingDate, photo, name, middleDistance, radius, temperature)
            {
                Planets = new Collection<DBPlanet>();
                foreach (var pl in planets)
                    Planets.Add(new DBPlanet(pl, Id));
                Galaxy = galaxy;
                Photo = photo;

                Name = name;
                MiddleDistanceValue = middleDistance.Value;
                MiddleDistanceUnit = middleDistance.Unit;
                Radius = radius;
                Temperature = temperature;
                //Galaxy = galaxy;
                InventingDate = inventingDate;

            }

            public DBStar(Star st) : this(st.InventingDate, st.Photo, st.Name, st.MiddleDistance, st.Radius, st.Temperature/*.HasValue ? st.Temperature.Value : 0*/, st.Planets, st.Galaxy)
            {

            }


            public DBStar(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius,
                    uint temperature, string galaxy)
                //base(inventingDate, photo, name, middleDistance, radius, temperature)
            {
                Planets = new Collection<DBPlanet>(); //<Planet>();
                Galaxy = galaxy;
                Photo = photo;

                Name = name;
                MiddleDistanceValue = middleDistance.Value;
                MiddleDistanceUnit = middleDistance.Unit;
                Radius = radius;
                Temperature = temperature;
                //Galaxy = galaxy;
                InventingDate = inventingDate;

            }


            public DBStar()
            {
                Planets = new Collection<DBPlanet>(); //<Planet>();
            }

            /// <summary>
            /// Планеты, вращающиеся вокруг звезды
            /// </summary>
            public virtual Collection<DBPlanet> Planets { get; set; }

            public override bool Equals(object obj)
            {
                var star = obj as DBStar;
                return star != null && EqualityComparer<string>.Default.Equals(Name, star.Name);
            }

            public override int GetHashCode()
            {
                return -1696190688 + EqualityComparer<string>.Default.GetHashCode(Name);
            }

            public Star ToStar()
            {
                var star = new Star(InventingDate, Photo, Name, new Distance(MiddleDistanceValue, MiddleDistanceUnit),
                    Radius, Temperature, Planets.Select(pl => pl.ToPlanet()), Galaxy);
                return star;
            }

            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            /// <summary>
            /// Название галактики, в которой расположен объект
            /// </summary>
            public string Galaxy { get; set; }

            public byte[] Photo { get; set; }
            public string Name { get; set; }
            public uint MiddleDistanceValue { get; set; }
            public UnitType MiddleDistanceUnit { get; set; }

            //public Distance MiddleDistance { get; set; }
            public uint Radius { get; set; }
            public uint Temperature { get; set; }
            public DateTime InventingDate { get; set; }
        }
    }
}
