using RotatingEarth.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatingEarth.images
{
    public class FakeSolarSystem : ISolarSystem
    {
        public Dictionary<PlanetNames, IPlanet> Planets { get; set; }

        public double SunDiameter { get; set; }

        public FakeSolarSystem()
        {
            SunDiameter = 3;

            Planets = new Dictionary<PlanetNames, IPlanet>();

            Planets[PlanetNames.Mercury] = new Planet()
            {
                Diameter = 0.3,
                DistanceToTheSun = 5
            };
            Planets[PlanetNames.Venus] = new Planet()
            {
                Diameter = 0.4,
                DistanceToTheSun = 7
            };
            Planets[PlanetNames.Earth] = new Planet()
            {
                Diameter = 0.4,
                DistanceToTheSun = 9
            };
            Planets[PlanetNames.Mars] = new Planet()
            {
                Diameter = 0.25,
                DistanceToTheSun = 11.5
            };
            Planets[PlanetNames.Jupiter] = new Planet()
            {
                Diameter = 1,
                DistanceToTheSun = 18
            };
            Planets[PlanetNames.Saturn] = new Planet()
            {
                Diameter = 1,
                DistanceToTheSun = 28
            };
            Planets[PlanetNames.Uranus] = new Planet()
            {
                Diameter = 0.8,
                DistanceToTheSun = 37
            };
            Planets[PlanetNames.Neptune] = new Planet()
            {
                Diameter = 0.8,
                DistanceToTheSun = 49
            };
        }
    }
}
