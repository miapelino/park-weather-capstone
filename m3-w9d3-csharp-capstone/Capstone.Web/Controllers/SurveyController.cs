using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Models;
using Capstone.Web.Dal;
using System.Configuration;

namespace Capstone.Web.Controllers
{
    public class SurveyController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ParkWeatherDb"].ToString();
        private readonly SurveySqlDAL surveyDal;

        public SurveyController()
        {
            this.surveyDal = new SurveySqlDAL(connectionString);
        }

        public ActionResult Survey()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Survey(Survey survey)
        {
            surveyDal.SubmitSurvey(survey);
            return RedirectToAction("SurveyResults");
        }

        public ActionResult SurveyResults()
        {
            List<Survey> surveyResults = surveyDal.GetResults();
            return View(surveyResults);
        }
    }
}