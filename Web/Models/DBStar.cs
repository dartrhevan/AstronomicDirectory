using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AstronomicDirectory;
using Web.Models.DataAccessPostgreSqlProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Web.Models
{
    namespace DataAccessPostgreSqlProvider
    {
        public class DBStar : ISpaceObject
        {
            public DBStar(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius,
                    uint temperature, IEnumerable<DBPlanet> planets, string galaxy)
                //base(inventingDate, photo, name, middleDistance, radius, temperature)
            {
                Planets = new HashSet<DBPlanet>(planets); //planets;
                Galaxy = galaxy;
                Photo = photo;

                Name = name;
                MiddleDistance = middleDistance;
                Radius = radius;
                Temperature = temperature;
                //Galaxy = galaxy;
                InventingDate = inventingDate;

            }

            public DBStar(Star st) : this(st.InventingDate, st.Photo, st.Name, st.MiddleDistance, st.Radius, st.Temperature.HasValue ? st.Temperature.Value : 0, st.Galaxy)
            {

            }


            public DBStar(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius,
                    uint temperature, string galaxy)
                //base(inventingDate, photo, name, middleDistance, radius, temperature)
            {
                Planets = new HashSet<DBPlanet>(); //<Planet>();
                Galaxy = galaxy;
                Photo = photo;

                Name = name;
                MiddleDistance = middleDistance;
                Radius = radius;
                Temperature = temperature;
                //Galaxy = galaxy;
                InventingDate = inventingDate;

            }


            public DBStar()
            {
                Planets = new HashSet<DBPlanet>(); //<Planet>();
            }

            /// <summary>
            /// Планеты, вращающиеся вокруг звезды
            /// </summary>
            public readonly HashSet<DBPlanet> Planets;

            public override bool Equals(object obj)
            {
                var star = obj as DBStar;
                return star != null && EqualityComparer<string>.Default.Equals(Name, star.Name);
            }

            public override int GetHashCode()
            {
                return -1696190688 + EqualityComparer<string>.Default.GetHashCode(Name);
            }


            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            /// <summary>
            /// Название галактики, в которой расположен объект
            /// </summary>
            public string Galaxy { get; set; }

            public byte[] Photo { get; set; }
            public string Name { get; set; }

            public Distance MiddleDistance { get; set; }
            public uint Radius { get; set; }
            public uint? Temperature { get; set; }
            public DateTime InventingDate { get; set; }
        }
    }
}
