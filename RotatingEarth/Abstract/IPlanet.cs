using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatingEarth.Abstract
{
    public interface IPlanet
    {
        double Diameter { get; set; }
        double DistanceToTheSun { get; set; }
    }
}
