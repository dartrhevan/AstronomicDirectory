using System;
using System.Collections.Generic;
using System.Drawing;

namespace AstronomicDirectory
{
    public class Planet : SpaceObject
    {
        /*public Planet(Bitmap photo, string name, Distance middleDistance, uint radius, uint temperature) : base(photo, name, middleDistance, radius, temperature)
        {
        }*/

        public Planet(DateTime inventingDate, Image photo, string name, Distance middleDistance, uint radius, bool hasAtmosphere, PlanetType type, Star star, uint temperature = 0, IEnumerable<Moon> moons = null) :
            base(inventingDate, photo, name, middleDistance, radius, temperature)
        {
            HasAtmosphere = hasAtmosphere;
            Type = type;
            Star = star.Name;
            if (moons != null) Moons = new HashSet<Moon>(moons);
        }

        public Planet(DateTime inventingDate, Image photo, string name, Distance middleDistance, uint radius, bool hasAtmosphere, PlanetType type, string star, string galaxy, uint temperature = 0, IEnumerable<Moon> moons = null) :
            base(inventingDate, photo, name, middleDistance, radius, temperature)
        {
            HasAtmosphere = hasAtmosphere;
            Type = type;
            Star = Name;
            Galaxy = galaxy;

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


    }
}