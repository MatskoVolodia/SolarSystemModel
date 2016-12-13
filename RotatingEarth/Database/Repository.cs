using RotatingEarth.Abstract;
using RotatingEarth.images;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatingEarth.Database
{
    public static class Repository
    {
        private static OleDbDataAdapter _adapter;
        private static OleDbConnection _connection;

        static Repository()
        {
            _adapter = new OleDbDataAdapter();

            _connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            _connection.Open();
        }

        public static Dictionary<PlanetNames, IPlanet> GetSolarSystem(string system)
        {
            var res = new Dictionary<PlanetNames, IPlanet>();

            _adapter.SelectCommand = new OleDbCommand("SELECT * FROM " + system, _connection);

            OleDbDataReader reader = _adapter.SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                PlanetNames planetName = (PlanetNames)Enum.Parse(typeof(PlanetNames), reader["PlanetName"].ToString());

                var planetInfo = new Planet()
                {
                    Diameter = double.Parse(reader["Diameter"].ToString()),
                    DistanceToTheSun = double.Parse(reader["DistanceToTheSun"].ToString())
                };
                res[planetName] = planetInfo;
            }

            return res;
        }
    }
}
