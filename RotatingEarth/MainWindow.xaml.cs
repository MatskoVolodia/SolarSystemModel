using RotatingEarth.Abstract;
using RotatingEarth.Entities;
using RotatingEarth.Helpers;
using RotatingEarth.images;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace RotatingEarth
{
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
            UpdateSolarSystemSizes();
            DrawOrbits();
            Helper.DrawSaturnRingModel(SaturnRing, SolarSystem.Planets[PlanetNames.Saturn].Diameter*2, 
                new Point3D(SolarSystem.Planets[PlanetNames.Saturn].DistanceToTheSun, 0, 0), RingGeometry, translateRing);
            helpLabel.Content = "Arrows - Rotate camera \nSpace - Pause/Resume animation \nScroll - Zoom\nS - Toggle fake/real solar system";
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Camera.Position = Camera.Position + Camera.LookDirection * Math.Sign(e.Delta) * (SolarSystem is RealSolarSystem ? 20 : 1);
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
                case Key.S:
                    if(SolarSystem is RealSolarSystem)
                    {
                        SolarSystem = new FakeSolarSystem();
                    }
                    else
                    {
                        SolarSystem = new RealSolarSystem();
                    }

                    UpdateSolarSystemSizes();
                    DrawOrbits();
                    Helper.DrawSaturnRingModel(SaturnRing, SolarSystem.Planets[PlanetNames.Saturn].Diameter * 2,
                        new Point3D(SolarSystem.Planets[PlanetNames.Saturn].DistanceToTheSun, 0, 0), RingGeometry, translateRing);
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
        
        private void DrawOrbits()
        {
            double thickness = SolarSystem is RealSolarSystem ? 0.4 : 0.04;
            foreach (var item in SolarSystem.Planets)
            {
                Helper.AddCircleModel(thickness, item.Value.DistanceToTheSun, mainViewPort);
            }
        }

        public void UpdateSolarSystemSizes()
        {
            Helper.BindItem(translateMercury, scaleMercury, PlanetNames.Mercury, SolarSystem);
            Helper.BindItem(translateVenus, scaleVenus, PlanetNames.Venus, SolarSystem);
            Helper.BindItem(translateEarth, scaleEarth, PlanetNames.Earth, SolarSystem);
            Helper.BindItem(translateMars, scaleMars, PlanetNames.Mars, SolarSystem);
            Helper.BindItem(translateJupiter, scaleJupiter, PlanetNames.Jupiter, SolarSystem);
            Helper.BindItem(translateSaturn, scaleSaturn, PlanetNames.Saturn, SolarSystem);
            Helper.BindItem(translateUranus, scaleUranus, PlanetNames.Uranus, SolarSystem);
            Helper.BindItem(translateNeptune, scaleNeptune, PlanetNames.Neptune, SolarSystem);

            scaleSun.ScaleX = SolarSystem.SunDiameter;
            scaleSun.ScaleY = SolarSystem.SunDiameter;
            scaleSun.ScaleZ = SolarSystem.SunDiameter;
        }
    }
}
