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

        public Planet(DateTime inventingDate, Bitmap photo, string name, Distance middleDistance, uint radius, string galaxy, bool hasAtmosphere, PlanetType type, Star star, uint temperature = 0, List<Planet> moons = null) :
            base(inventingDate, photo, name, middleDistance, radius, galaxy, temperature)
        {
            HasAtmosphere = hasAtmosphere;
            Type = type;
            Star = star;
            Moons = moons ?? new List<Planet>();
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

        public readonly bool HasAtmosphere;
        
        public readonly PlanetType Type;
        /// <summary>
        /// Звезда, вокруг которой вращается планета
        /// </summary>
        public readonly Star Star;

        public readonly List<Planet> Moons;

    }
}