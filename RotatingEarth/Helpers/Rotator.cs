using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RotatingEarth
{
    public static class Rotator
    {
        public enum RotationBy { X, Y, Z };

        private static Func<double, double>[,] xRotationMatrix;
        private static Func<double, double>[,] yRotationMatrix;
        private static Func<double, double>[,] zRotationMatrix;

        static Rotator()
        {
            xRotationMatrix = new Func<double, double>[,]{ 
                { (angle) => { return 1; }, (angle) => { return 0; },               (angle) => { return 0; } },
                { (angle) => { return 0; }, (angle) => { return Math.Cos(angle); }, (angle) => { return (-1)*Math.Sin(angle); } },
                { (angle) => { return 0; }, (angle) => { return Math.Sin(angle); }, (angle) => { return Math.Cos(angle); } }
            };

            yRotationMatrix = new Func<double, double>[,]{
                { (angle) => { return Math.Cos(angle); },       (angle) => { return 0; }, (angle) => { return Math.Sin(angle); } },
                { (angle) => { return 0; },                     (angle) => { return 1; }, (angle) => { return 0; } },
                { (angle) => { return (-1)*Math.Sin(angle); },  (angle) => { return 0; }, (angle) => { return Math.Cos(angle); } }
            };

            zRotationMatrix = new Func<double, double>[,]{
                { (angle) => { return Math.Cos(angle); },   (angle) => { return (-1)*Math.Sin(angle); },    (angle) => { return 0; } },
                { (angle) => { return Math.Sin(angle); },   (angle) => { return Math.Cos(angle); },         (angle) => { return 0; } },
                { (angle) => { return 0; },                 (angle) => { return 0; },                       (angle) => { return 1; } }
            };
        }


        /// <param name="angle">Degrees</param>
        public static double[] RotateBy(double angle, double[] vector, RotationBy rotationOption)
        {
            Func<double, double>[,] selectedRotationMatrix = new Func<double, double>[0,0];
            angle = Math.PI * angle / 180.0;

            switch (rotationOption)
            {
                case RotationBy.X:
                    selectedRotationMatrix = xRotationMatrix;
                    break;
                case RotationBy.Y:
                    selectedRotationMatrix = yRotationMatrix;
                    break;
                case RotationBy.Z:
                    selectedRotationMatrix = zRotationMatrix;
                    break;
                default:
                    break;
            }

            double[,] rotationMatrix = new double[,]
            {
                { selectedRotationMatrix[0,0](angle), selectedRotationMatrix[0,1](angle), selectedRotationMatrix[0,2](angle) },
                { selectedRotationMatrix[1,0](angle), selectedRotationMatrix[1,1](angle), selectedRotationMatrix[1,2](angle) },
                { selectedRotationMatrix[2,0](angle), selectedRotationMatrix[2,1](angle), selectedRotationMatrix[2,2](angle) }
            };

            return MultiplyMatrixByVector(rotationMatrix, vector);
        }

        private static double[] MultiplyMatrixByVector(double[,] matrix, double[] vector)
        {
            double[] res = new double[3];

            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    res[i] += (matrix[i, j] * vector[j]);
                }
            }

            return res;
        }
    }
}
