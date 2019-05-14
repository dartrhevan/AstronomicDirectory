using System;
using System.Collections.Generic;

namespace AstronomicDirectory
{
    public class Planet : ISpaceObject
    {
        /*public Planet(Bitmap photo, string name, Distance middleDistance, uint radius, uint temperature) : base(photo, name, middleDistance, radius, temperature)
        {
        }*/

        public Planet(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius, bool hasAtmosphere, PlanetType type, Star star, uint temperature = 0, IEnumerable<Moon> moons = null) 
            //base(inventingDate, photo, name, middleDistance, radius, temperature)
        {
            HasAtmosphere = hasAtmosphere;
            Type = type;
            Star = star.Name;
            if (moons != null) Moons = new HashSet<Moon>(moons);
            Photo = photo;

            Name = name;
            MiddleDistance = middleDistance;
            Radius = radius;
            Temperature = temperature;
            //Galaxy = galaxy;
            InventingDate = inventingDate;

        }

        public Planet(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius, bool hasAtmosphere, PlanetType type, string star, string galaxy, uint temperature = 0, IEnumerable<Moon> moons = null) 
            //base(inventingDate, photo, name, middleDistance, radius, temperature)
        {
            HasAtmosphere = hasAtmosphere;
            Type = type;
            Star = star;
            Galaxy = galaxy;
            MiddleDistance = middleDistance;
            InventingDate = inventingDate;
            Photo = photo;
            Name = name;
            Radius = radius;
            Temperature = temperature;
            if (moons != null) Moons = new HashSet<Moon>(moons);
        }


        public override bool Equals(object obj)
        {
            var pl = obj as Planet;
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

        public Planet(Star owner)
        {
            Star = owner.Name;
            Galaxy = owner.Galaxy;
            Moons = new HashSet<Moon>();
        }

        public Planet(string owner, string galaxy)
        {
            //global::
            Star = owner;
            Galaxy = galaxy;//owner.Galaxy;
            Moons = new HashSet<Moon>();
        }

        public Planet()
        {
            Moons = new HashSet<Moon>();

        }

        public bool HasAtmosphere { get; set; }
        
        public PlanetType Type { get; set; }
        /// <summary>
        /// Звезда, вокруг которой вращается планета
        /// </summary>
        public string Star { get; set; }

        public readonly HashSet<Moon> Moons;

        /// <summary>
        /// Название галактики, в которой расположен объект
        /// </summary>
        public string Galaxy { get; set; }//=> Star.Galaxy;


        public byte[] Photo { get; set; }
        public string Name { get; set; }
        public Distance MiddleDistance { get; set; }
        public uint Radius { get; set; }
        public uint Temperature { get; set; }
        public DateTime InventingDate { get; set; } = DateTime.Now;
    }
}