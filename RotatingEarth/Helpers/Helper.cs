using RotatingEarth.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace RotatingEarth.Helpers
{
    public static class Helper
    {
        public static void AddCircleModel(double thickness, double radius, Viewport3D mainViewPort)
        {
            int k = 0;

            var materialGroup = new MaterialGroup();
            materialGroup.Children.Add(new DiffuseMaterial(Brushes.LightGray));

            var meshGeometry = new MeshGeometry3D();

            for (float n = 0f; n < 2 * Math.PI; n += 0.01f, k++)
            {
                var p = new Point3D(radius * Math.Cos(n), 0, radius * Math.Sin(n));
                var u = new Point3D(radius * Math.Cos(n), (-1)*thickness, radius * Math.Sin(n));
                meshGeometry.Positions.Add(p);
                meshGeometry.Positions.Add(u);
                if (k > 0)
                {
                    meshGeometry.TriangleIndices.Add(k);
                    meshGeometry.TriangleIndices.Add(k - 1);
                    meshGeometry.TriangleIndices.Add(k + 1);
                    meshGeometry.TriangleIndices.Add(k);
                }
                meshGeometry.TriangleIndices.Add(k);
                meshGeometry.TriangleIndices.Add(k + 1);
                k++;
            }

            var geometryModel = new GeometryModel3D() { Material = materialGroup, BackMaterial = materialGroup, Geometry = meshGeometry };
            var modelgroup = new Model3DGroup();
            modelgroup.Children.Add(geometryModel);
            var model = new ModelVisual3D() { Content = modelgroup };

            model.SetValue(GeometryModel3D.GeometryProperty, meshGeometry);
            mainViewPort.Children.Add(model);
        }

        public static void DrawSaturnRingModel(ModelVisual3D vis, double radius, Point3D center, 
            GeometryModel3D ringGeometry, TranslateTransform3D translateRing)
        {
            var geo = new MeshGeometry3D();
            var resolution = 1000;

            geo.Positions.Add(new Point3D(0, 0, 0));

            double t = 2 * Math.PI / resolution;
            for (int i = 0; i < resolution; i++)
            {
                geo.Positions.Add(new Point3D(radius * Math.Cos(t * i), 0, -radius * Math.Sin(t * i)));
            }

            for (int i = 0; i < resolution; i++)
            {
                geo.TriangleIndices.Add(0);
                geo.TriangleIndices.Add(i + 1);
                geo.TriangleIndices.Add((i < (resolution - 1)) ? i + 2 : 1);
            }

            ringGeometry.Geometry = geo;

            translateRing.OffsetX = center.X;
            translateRing.OffsetY = center.Y;
            translateRing.OffsetZ = center.Z;
        }

        public static void BindItem(TranslateTransform3D translate, ScaleTransform3D model, PlanetNames name, ISolarSystem solarSystem)
        {
            model.ScaleX = solarSystem.Planets[name].Diameter;
            model.ScaleY = solarSystem.Planets[name].Diameter;
            model.ScaleZ = solarSystem.Planets[name].Diameter;

            translate.OffsetX = solarSystem.Planets[name].DistanceToTheSun;
        }
    }
}
