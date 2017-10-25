using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Capstone.Web.Dal
{
    public class ParkWeatherSqlDal : IParkWeatherDal
    {
        private readonly string connectionString;
        private const string SQL_GetParks = "SELECT * from park";

        public ParkWeatherSqlDal(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<ParkWeatherModel> GetParks()
        {
            List<ParkWeatherModel> parkList = new List<ParkWeatherModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetParks, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ParkWeatherModel park = new ParkWeatherModel();
                        MoveFields(reader, park);
                        parkList.Add(park);
                    }
                }

            }
            catch (SqlException ex)
            {

                throw;
            }
            return parkList;
        }

        private void MoveFields(SqlDataReader reader, ParkWeatherModel park)
        {
            park.Code = Convert.ToString(reader["parkCode"]);
            park.Name = Convert.ToString(reader["parkName"]);
            park.State = Convert.ToString(reader["FIRSTNAME"]);
            park.Description = Convert.ToString(reader["RES_STREET"]);
        }
    }
}

