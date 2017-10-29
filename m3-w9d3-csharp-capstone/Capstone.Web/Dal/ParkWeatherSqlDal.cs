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
        private const string SQL_GetParks = "SELECT * from park join weather on weather.parkCode = park.parkCode ORDER BY weather.fiveDayForecastValue";

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
            park.State = Convert.ToString(reader["state"]);
            park.Description = Convert.ToString(reader["parkDescription"]);
            park.Acreage = Convert.ToInt32(reader["acreage"]);
            park.Elevation = Convert.ToInt32(reader["elevationInFeet"]);
            park.MilesOfTrail = Convert.ToDouble(reader["milesOfTrail"]);
            park.NumCampsites = Convert.ToInt32(reader["numberOfCampsites"]);
            park.Climate = Convert.ToString(reader["climate"]);
            park.YearFounded = Convert.ToInt32(reader["yearFounded"]);
            park.AnnualVisitor = Convert.ToInt32(reader["annualVisitorCount"]);
            park.Quote = Convert.ToString(reader["inspirationalQuote"]);
            park.QuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]);
            park.EntryFee = Convert.ToInt32(reader["entryFee"]);
            park.NumSpecies = Convert.ToInt32(reader["numberOfAnimalSpecies"]);
            park.FiveDayForecastValue = Convert.ToString(reader["fiveDayForecastValue"]);
            park.Low = Convert.ToInt32(reader["low"]);
            park.High = Convert.ToInt32(reader["high"]);
            park.Forecast = Convert.ToString(reader["forecast"]);
        }
    }
}

