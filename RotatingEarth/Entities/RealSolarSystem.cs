using RotatingEarth.Abstract;
using RotatingEarth.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatingEarth.Entities
{
    public class RealSolarSystem : ISolarSystem
    {
        public Dictionary<PlanetNames, IPlanet> Planets { get; set; }

        public double SunDiameter { get; set; }

        public RealSolarSystem()
        {
            SunDiameter = 69;

            Planets = Repository.GetSolarSystem("RealSolarSystem");
        }
    }
}
