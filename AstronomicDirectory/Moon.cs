﻿using System.Drawing;

namespace AstronomicDirectory
{
    public class Moon : Planet
    {
        /// <summary>
        /// Планета, спутником которой является данный объекта
        /// </summary>
        public readonly Planet PlanetOwner;

        public Moon(Bitmap photo, string name, uint radius, bool hasAtmosphere, Planet planetOwner, uint temperature = 0) :
            base(photo, name, planetOwner.MiddleDistance, radius, planetOwner.Galaxy, hasAtmosphere, PlanetType.Moon, planetOwner.Star, temperature)
        {
            PlanetOwner = planetOwner;
        }
    }
}