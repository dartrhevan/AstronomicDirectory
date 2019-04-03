﻿using System;
using System.Drawing;

namespace AstronomicDirectory
{
    public class Moon : Planet
    {
        /// <summary>
        /// Планета, спутником которой является данный объекта
        /// </summary>
        public readonly Planet PlanetOwner;

        public Moon(DateTime inventingDate, Image photo, string name, uint radius, bool hasAtmosphere, Planet planetOwner, uint temperature = 0) :
            base(inventingDate, photo, name, planetOwner.MiddleDistance, radius, hasAtmosphere, PlanetType.Moon, planetOwner.Star, temperature)
        {
            PlanetOwner = planetOwner;
        }

        public override string ToString()
        {
            return $"{Name}, Планета: {PlanetOwner.Name}, Радиус: {Radius}";
        }
    }
}