using AstronomicDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Web.Models.DataAccessPostgreSqlProvider
{
    public class DBMoon : DBPlanet
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

        //public Moon(DateTime inventingDate, Image photo, string name, uint radius, bool hasAtmosphere, Planet planetOwner, uint temperature = 0) :
        //    base(inventingDate, photo, name, planetOwner.MiddleDistance, radius, hasAtmosphere, PlanetType.Moon, planetOwner.Star, planetOwner.Galaxy, temperature)
        //{
        //    PlanetOwner = planetOwner.Name;
        //}

        public override string ToString()
        {
            return $"{Name}, Планета: {PlanetOwner}, Радиус: {Radius}";
        }

        public DBMoon(DBPlanet planetOwner) : base(planetOwner.Star, planetOwner.Galaxy)
        {
            PlanetOwner = planetOwner.Name;
            MiddleDistance = planetOwner.MiddleDistance;
            //this.Moons = null;
            Type = PlanetType.Moon;

        }
        public DBMoon()
        {
            Type = PlanetType.Moon;
        }
    }
}