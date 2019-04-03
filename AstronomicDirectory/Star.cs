using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AstronomicDirectory
{
    public class Star : SpaceObject
    {
        public Star(DateTime inventingDate, Image photo, string name, Distance middleDistance, uint radius, uint temperature, List<Planet> planets, string galaxy) : 
            base(inventingDate, photo, name, middleDistance, radius, temperature)
        {
            Planets = planets;
            Galaxy = galaxy;
        }

        public Star(DateTime inventingDate, Image photo, string name, Distance middleDistance, uint radius, uint temperature, string galaxy) : 
            base(inventingDate, photo, name, middleDistance, radius, temperature)
        {
            Planets = new List<Planet>();
            Galaxy = galaxy;
        }

        public Star()
        {
            Planets = new List<Planet>();
        }

        /// <summary>
        /// Планеты, вращающиеся вокруг звезды
        /// </summary>
        public readonly List<Planet> Planets;

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

    }
}
