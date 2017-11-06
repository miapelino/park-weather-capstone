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
    public class UnitTests
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

                cmd = new SqlCommand("INSERT INTO park(parkCode, parkName, state, acreage, elevationInFeet, milesOfTrail, numberOfCampsites, climate, yearFounded, annualVisitorCount, inspirationalQuote, inspirationalQuoteSource, parkDescription, entryFee, numberOfAnimalSpecies) VALUES ('TST1','Awesome National Park', 'TheMoon', 500, 250, 250, 25, 'cold', 1980, 25, 'Bigger step than you think', 'Mia Pelino', 'cold and dry', 10, 10)", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TST1', 1, 10, 50, 'sunny')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TST1', 2, 15, 60, 'dark')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TST1', 3, 25, 70, 'dry')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TST1', 4, 5, 40, 'wet')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TST1', 5, 35, 80, 'cloudy')", conn);
                cmd.ExecuteNonQuery();

                //
                //cmd = new SqlCommand("INSERT INTO survey_result VALUES ('TST1', 'test@test.com', 'Texas', 'sedentary')", conn);
                //cmd.ExecuteNonQuery();
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
        public void DetailParkListTest()
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
            Assert.AreEqual("1", testList[0].FiveDayForecastValue);
            Assert.AreEqual("2", testList[1].FiveDayForecastValue);
            Assert.AreEqual("3", testList[2].FiveDayForecastValue);
            Assert.AreEqual("4", testList[3].FiveDayForecastValue);
            Assert.AreEqual("5", testList[4].FiveDayForecastValue);
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
            ParkWeatherModel testModel = testList[0];
            List<string> result = testModel.ForecastRecommendations(testModel);

            List<string> expected = new List<string> {"Pack sunblock", "Warning: exposure to frigid temperatures is dangerous", "Wear breathable layers"};

            //assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetDayOfForecastTest()
        {
            //arrange
            ParkWeatherSqlDal applicationDal = new ParkWeatherSqlDal(connectionString);
            List<ParkWeatherModel> tempList = applicationDal.GetParks();
            ParkWeatherModel tempPark = new ParkWeatherModel();

            //act
            List<ParkWeatherModel> testList = tempPark.DetailParkList("Awesome National Park", tempList);

            ParkWeatherModel testModel1 = testList[0];
            ParkWeatherModel testModel2 = testList[1];
            ParkWeatherModel testModel3 = testList[2];
            ParkWeatherModel testModel4 = testList[3];
            ParkWeatherModel testModel5 = testList[4];

            string result1 = testModel1.GetDayOfForecast();
            string expected1 = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MM/dd");

            string result2 = testModel2.GetDayOfForecast();
            string expected2 = DateTime.Now.AddDays(1).DayOfWeek + ", " + DateTime.Now.AddDays(1).ToString("MM/dd");

            string result3 = testModel3.GetDayOfForecast();
            string expected3 = DateTime.Now.AddDays(2).DayOfWeek + ", " + DateTime.Now.AddDays(2).ToString("MM/dd");

            string result4 = testModel4.GetDayOfForecast();
            string expected4 = DateTime.Now.AddDays(3).DayOfWeek + ", " + DateTime.Now.AddDays(3).ToString("MM/dd");

            string result5 = testModel5.GetDayOfForecast();
            string expected5 = DateTime.Now.AddDays(4).DayOfWeek + ", " + DateTime.Now.AddDays(4).ToString("MM/dd");


            //assert
            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(expected2, result2);
            Assert.AreEqual(expected3, result3);
            Assert.AreEqual(expected4, result4);
            Assert.AreEqual(expected5, result5);
        }
    }
}
