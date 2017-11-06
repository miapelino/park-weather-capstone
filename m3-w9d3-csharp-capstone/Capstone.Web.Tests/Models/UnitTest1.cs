using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.Web.Models;
using Capstone.Web.Dal;
using System.Collections.Generic;
using System.Transactions;
using System.Data.SqlClient;

namespace Capstone.Web.Tests.Models
{
    [TestClass]
    public class UnitTest1
    {
        private string connectionString = @"Data Source=localhost\sqlexpress;Initial Catalog=ParkWeather;Integrated Security=True";

        private TransactionScope tran;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                cmd = new SqlCommand("INSERT INTO park(parkCode, parkName, state, acreage, elevationInFeet, milesOfTrail, numberOfCampsites, climate, yearFounded, annualVisitorCount, inspirationalQuote, inspirationalQuoteSource, parkDescription, entryFee, numberOfAnimalSpecies) VALUES ('TEST','Awesome National Park', 'TheMoon', 500, 250, 250, 25, 'cold', 1980, 25, 'Bigger step than you think', 'Mia Pelino', 'cold and dry', 10, 10)", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TEST', 1, 10, 50, 'sunny')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TEST', 2, 15, 60, 'dark')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TEST', 3, 25, 70, 'dry')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TEST', 4, 5, 40, 'wet')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TEST', 5, 35, 80, 'cloudy')", conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void ConvertToCelsiusTest()
        {
            ParkWeatherModel testObject = new ParkWeatherModel()
            {
                Low = 60,
                High = 85
            };

            int result = testObject.ConvertToCelsius(testObject.Low);

            Assert.AreEqual(16, result);

            result = testObject.ConvertToCelsius(testObject.High);

            Assert.AreEqual(29, result);

        }

        [TestMethod]
        public void DetailTest()
        {
            //arrange
            ParkWeatherSqlDal applicationDal = new ParkWeatherSqlDal(connectionString);
            List<ParkWeatherModel> tempList = applicationDal.GetParks();
            ParkWeatherModel tempPark = new ParkWeatherModel();

            //act
            List<ParkWeatherModel> testList = tempPark.DetailParkList("Awesome National Park", tempList);

            //assert
            Assert.AreEqual(5, testList.Count);
            Assert.AreEqual("Awesome National Park", testList[0].Name);
            Assert.AreEqual("Awesome National Park", testList[1].Name);
            Assert.AreEqual("Awesome National Park", testList[2].Name);
            Assert.AreEqual("Awesome National Park", testList[3].Name);
            Assert.AreEqual("Awesome National Park", testList[4].Name);
            Assert.AreEqual(1, testList[0].FiveDayForecastValue);
            Assert.AreEqual(2, testList[1].FiveDayForecastValue);
            Assert.AreEqual(3, testList[2].FiveDayForecastValue);
            Assert.AreEqual(4, testList[3].FiveDayForecastValue);
            Assert.AreEqual(5, testList[4].FiveDayForecastValue);
        }

        [TestMethod]
        public void ForecastRecommendationsTest()
        {
            //arrange
            ParkWeatherSqlDal applicationDal = new ParkWeatherSqlDal(connectionString);
            List<ParkWeatherModel> tempList = applicationDal.GetParks();
            ParkWeatherModel tempPark = new ParkWeatherModel();

            //act
            List<ParkWeatherModel> testList = tempPark.DetailParkList("Awesome National Park", tempList);

            //assert
            Assert.AreEqual("sunny", testList[0].Forecast);
            Assert.AreEqual("dark", testList[1].Forecast);
            Assert.AreEqual("dry", testList[2].Forecast);
            Assert.AreEqual("wet", testList[3].Forecast);
            Assert.AreEqual("cloudy", testList[4].Forecast);

        }
    }
}
