using RotatingEarth;
using RotatingEarth.Abstract;
using RotatingEarth.images;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace RotatingEarth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Vector3D CurrentLookDirection
        {
            get
            {
                return new Vector3D()
                {
                    X = Camera.Position.X / DistanceToTheSun * (-1),
                    Y = Camera.Position.Y / DistanceToTheSun * (-1),
                    Z = Camera.Position.Z / DistanceToTheSun * (-1)
                };
            }
        }
        public double DistanceToTheSun
        {
            get
            {
                return Math.Sqrt(Math.Pow(Camera.Position.X, 2) + Math.Pow(Camera.Position.Y, 2) + Math.Pow(Camera.Position.Z, 2));
            }
        }

        public ISolarSystem SolarSystem { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SolarSystem = new FakeSolarSystem();
            SetValues();
            DrawOrbits();
            DrawSaturnRingModel(SaturnRing, SolarSystem.Planets[PlanetNames.Saturn].Diameter*2, new Point3D(SolarSystem.Planets[PlanetNames.Saturn].DistanceToTheSun, 0, 0));
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Camera.Position = Camera.Position + Camera.LookDirection * Math.Sign(e.Delta);
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            double[] currentState = new[] { Camera.Position.X, Camera.Position.Y, Camera.Position.Z };
            double[] rotatedState = new double[3];

            ValidateUpDirection();

            switch (e.Key)
            {
                case Key.Left:
                    rotatedState = Rotator.RotateBy(1, currentState, Rotator.RotationBy.Y);
                    Camera.Position = new Point3D(rotatedState[0], rotatedState[1], rotatedState[2]);
                    break;
                case Key.Up:
                    rotatedState = Rotator.RotateBy(-1, currentState, Rotator.RotationBy.X);
                    Camera.Position = new Point3D(rotatedState[0], rotatedState[1], rotatedState[2]);
                    break;
                case Key.Right:
                    rotatedState = Rotator.RotateBy(-1, currentState, Rotator.RotationBy.Y);
                    Camera.Position = new Point3D(rotatedState[0], rotatedState[1], rotatedState[2]);
                    break;
                case Key.Down:
                    rotatedState = Rotator.RotateBy(1, currentState, Rotator.RotationBy.X);
                    Camera.Position = new Point3D(rotatedState[0], rotatedState[1], rotatedState[2]);
                    break;
                case Key.Space:
                    Storyboard spinStoryboard = Resources["mainAnimation"] as Storyboard;
                    if (spinStoryboard != null)
                    {
                        if (spinStoryboard.GetIsPaused(this))
                        {
                            spinStoryboard.Resume(this);
                        }
                        else
                        {
                            spinStoryboard.Pause(this);
                        }
                    }
                    break;
                default:
                    break;
            }

            Camera.LookDirection = CurrentLookDirection;
        }

        public void ValidateUpDirection()
        {
            if (Math.Abs(Camera.Position.Y) <= DistanceToTheSun / 1000)
            {
                Camera.UpDirection = new Vector3D(Camera.UpDirection.X, Camera.UpDirection.Y * (-1), Camera.UpDirection.Z);
            }
        }

        private void AddCircleModel(double radius)
        {
            int k = 0;

            var materialGroup = new MaterialGroup();
            materialGroup.Children.Add(new DiffuseMaterial(Brushes.LightGray));

            var meshGeometry = new MeshGeometry3D();

            for (float n = 0f; n < 2 * Math.PI; n += 0.01f, k++)
            {
                var p = new Point3D(radius * Math.Cos(n), 0, radius * Math.Sin(n));
                var u = new Point3D(radius * Math.Cos(n), -0.04, radius * Math.Sin(n));
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
        private void DrawOrbits()
        {
            foreach (var item in SolarSystem.Planets)
            {
                AddCircleModel(item.Value.DistanceToTheSun);
            }
        }

        public void SetValues()
        {
            BindItem(translateMercury, scaleMercury, PlanetNames.Mercury);
            BindItem(translateVenus, scaleVenus, PlanetNames.Venus);
            BindItem(translateEarth, scaleEarth, PlanetNames.Earth);
            BindItem(translateMars, scaleMars, PlanetNames.Mars);
            BindItem(translateJupiter, scaleJupiter, PlanetNames.Jupiter);
            BindItem(translateSaturn, scaleSaturn, PlanetNames.Saturn);
            BindItem(translateUranus, scaleUranus, PlanetNames.Uranus);
            BindItem(translateNeptune, scaleNeptune, PlanetNames.Neptune);

            scaleSun.ScaleX = SolarSystem.SunDiameter;
            scaleSun.ScaleY = SolarSystem.SunDiameter;
            scaleSun.ScaleZ = SolarSystem.SunDiameter;
        }
        public void BindItem(TranslateTransform3D translate, ScaleTransform3D model, PlanetNames name)
        {
            model.ScaleX = SolarSystem.Planets[name].Diameter;
            model.ScaleY = SolarSystem.Planets[name].Diameter;
            model.ScaleZ = SolarSystem.Planets[name].Diameter;

            translate.OffsetX = SolarSystem.Planets[name].DistanceToTheSun;
        }

        private void DrawSaturnRingModel(ModelVisual3D vis, double radius, Point3D center)
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

            RingGeometry.Geometry = geo;

            translateRing.OffsetX = center.X;
            translateRing.OffsetY = center.Y;
            translateRing.OffsetZ = center.Z;
        }
    }
}
