using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Capstone.Web.Models;
using System.Web.Mvc;

namespace Capstone.Web.Dal
{
    public class SurveySqlDAL
    {
        private readonly string connectionString;
        private const string SQL_SubmitSurvey = @"INSERT INTO survey_result VALUES(@parkCode, @emailAddress, @state, @activityLevel);";
        private const string SQL_DisplaySurveyResults = "SELECT COUNT(*) as votes, s.parkCode, p.parkName from survey_result s join park p on p.parkCode = s.parkCode GROUP BY s.parkCode, p.parkName ORDER BY votes DESC;";
        private const string SQL_GetParkNames = "SELECT parkName, parkCode from park;";

        public SurveySqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Survey> GetResults()
        {
            List<Survey> surveyResults = new List<Survey>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_DisplaySurveyResults, conn);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        Survey s = new Survey();
                        s.Votes = Convert.ToInt32(results["votes"]);
                        s.ParkCode = Convert.ToString(results["parkCode"]);
                        s.Name = Convert.ToString(results["parkName"]);
                        surveyResults.Add(s);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return surveyResults;
        }

        public bool SubmitSurvey(Survey survey)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SubmitSurvey, conn);
                    cmd.Parameters.AddWithValue("@parkCode", survey.ParkCode);
                    cmd.Parameters.AddWithValue("@emailAddress", survey.Email);
                    cmd.Parameters.AddWithValue("@activityLevel", survey.ActivityLevel);
                    cmd.Parameters.AddWithValue("@state", survey.State);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        //to use with List<SelectListItem> ChoosePark
        public SelectList GetParkList()
        {
           Survey survey = new Survey();
           List<Survey> list = new List<Survey>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetParkNames, conn);
                    SqlDataReader results = cmd.ExecuteReader();

                    while (results.Read())
                    {
                        list.Add(new Survey
                        {
                            Text = Convert.ToString(results["parkName"]),
                            Value = Convert.ToString(results["parkCode"])
                        });
                    }
                }
                survey.DropDownList = new SelectList(list, "Value", "Text");
            }
            catch (SqlException ex)
            {
                throw;
            }
            return survey.DropDownList;
        }
    }
}