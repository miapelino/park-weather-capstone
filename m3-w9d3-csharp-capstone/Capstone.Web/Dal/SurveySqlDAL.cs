using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Dal
{
    public class SurveySqlDAL
    {
        private readonly string connectionString;
        private const string SQL_InsertSurvey = @"INSERT INTO survey_result VALUES(@parkCode, @emailAddress, @date, @activityLevel);";
        private const string SQL_DisplaySurveyResults = ""
    }
}