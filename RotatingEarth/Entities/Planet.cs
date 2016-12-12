using RotatingEarth.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatingEarth.images
{
    public class Planet : IPlanet
    {
        public double Diameter { get; set; }
        public double DistanceToTheSun { get; set; }
    }
}
