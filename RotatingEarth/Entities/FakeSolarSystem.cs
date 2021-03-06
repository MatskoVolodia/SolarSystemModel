﻿using RotatingEarth.Abstract;
using RotatingEarth.Database;
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

            Planets = Repository.GetSolarSystem("FakeSolarSystem");
        }
    }
}
