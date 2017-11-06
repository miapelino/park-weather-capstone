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
    public class DALTests
    {
        private string connectionString = @"Data Source=localhost\sqlexpress;Initial Catalog=ParkWeather;Integrated Security=True";
        private TransactionScope tran;
        private int existingParks = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();
                //count parks to get existing number
                cmd = new SqlCommand("SELECT COUNT(*) FROM park;", conn);
                existingParks = (int)cmd.ExecuteScalar();

                //create parks to test
                cmd = new SqlCommand("INSERT INTO park(parkCode, parkName, state, acreage, elevationInFeet, milesOfTrail, numberOfCampsites, climate, yearFounded, annualVisitorCount, inspirationalQuote, inspirationalQuoteSource, parkDescription, entryFee, numberOfAnimalSpecies) VALUES ('TST2','Awesome National Park', 'TheMoon', 500, 250, 250, 25, 'cold', 1980, 25, 'Bigger step than you think', 'Mia Pelino', 'cold and dry', 10, 10)", conn);
                cmd.ExecuteNonQuery();

                //create weather to test
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TST2', 1, 10, 50, 'sunny')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TST2', 2, 15, 60, 'dark')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TST2', 3, 25, 70, 'dry')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TST2', 4, 5, 40, 'wet')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('TST2', 5, 35, 80, 'cloudy')", conn);
                cmd.ExecuteNonQuery();

                //create survey submission to test
                cmd = new SqlCommand("INSERT INTO survey_result VALUES ('TST2', 'test@test.com', 'Texas', 'sedentary')", conn);
                cmd.ExecuteNonQuery();

            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetParksTest()
        {
            //Arrange
            ParkWeatherSqlDal parkWeatherDal = new ParkWeatherSqlDal(connectionString);
            existingParks *= 5;

            //Act
            List<ParkWeatherModel> parks = parkWeatherDal.GetParks();

            //Assert
            Assert.AreEqual(existingParks + 5, parks.Count);
        }


        [TestMethod]
        public void SubmitSurveyTest()
        {
            //Arrange
            SurveySqlDAL surveyDal = new SurveySqlDAL(connectionString);
            Survey survey = new Survey
            {
                Email = "test@test.com",
                ParkCode = "TST2",
                State = "Ohio",
                ActivityLevel = "active"
            };

            //Act
            bool didWork = surveyDal.SubmitSurvey(survey);

            //Assert
            Assert.AreEqual(true, didWork);
        }

        [TestMethod]
        public void GetResultsTest()
        {
            //Arrange
            SurveySqlDAL surveyDal = new SurveySqlDAL(connectionString);

            //Act
            List<Survey> results = surveyDal.GetResults();

            //Assert
            Assert.IsTrue(results.Count <= 10);

        }
    }
}