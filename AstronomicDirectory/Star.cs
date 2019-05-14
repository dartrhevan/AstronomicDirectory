using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AstronomicDirectory
{
    public class Star : ISpaceObject
    {
        public Star(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius, uint temperature, IEnumerable<Planet> planets, string galaxy)
            //base(inventingDate, photo, name, middleDistance, radius, temperature)
        {
            Planets = new HashSet<Planet>(planets);//planets;
            Galaxy = galaxy;
            Photo = photo;

            Name = name;
            MiddleDistance = middleDistance;
            Radius = radius;
            Temperature = temperature;
            //Galaxy = galaxy;
            InventingDate = inventingDate;

        }

        public Star(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius, uint temperature, string galaxy)
            //base(inventingDate, photo, name, middleDistance, radius, temperature)
        {
            Planets = new HashSet<Planet>();//<Planet>();
            Galaxy = galaxy;
            Photo = photo;

            Name = name;
            MiddleDistance = middleDistance;
            Radius = radius;
            Temperature = temperature;
            //Galaxy = galaxy;
            InventingDate = inventingDate;

        }

        public Star()
        {
            Planets = new HashSet<Planet>();//<Planet>();
        }

        /// <summary>
        /// Планеты, вращающиеся вокруг звезды
        /// </summary>
        public readonly HashSet<Planet> Planets;

        public override bool Equals(object obj)
        {
            var star = obj as Star;
            return star != null && EqualityComparer<string>.Default.Equals(Name, star.Name);
        }

        public override int GetHashCode()
        {
            return -1696190688 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
        /// <summary>
        /// Название галактики, в которой расположен объект
        /// </summary>
        public string Galaxy { get; set; }

        public byte[] Photo { get; set; }
        public string Name { get; set; }
        public Distance MiddleDistance { get; set; }
        public uint Radius { get; set; }
        public uint Temperature { get; set; }
        public DateTime InventingDate { get; set; } = DateTime.Now;
    }
}
