using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatingEarth.Abstract
{
    public enum PlanetNames
    {
        Mercury,
        Venus,
        Earth,
        Mars,
        Jupiter,
        Saturn,
        Uranus,
        Neptune
    }

    public interface ISolarSystem
    {
        double SunDiameter { get; set; }
        Dictionary<PlanetNames, IPlanet> Planets { get; set; }
    }
}
