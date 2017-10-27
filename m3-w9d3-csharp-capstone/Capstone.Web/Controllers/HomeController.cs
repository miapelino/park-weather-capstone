using Capstone.Web.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Models;

namespace Capstone.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IParkWeatherDal applicationDal;

        public HomeController(IParkWeatherDal applicationDal)
        {
            this.applicationDal = applicationDal;
        }

        public ActionResult Index()
        {
            List<ParkWeatherModel> parkList = applicationDal.GetParks();
            return View(parkList);
        }

        public ActionResult Detail(string id)
        {
            List<ParkWeatherModel> parkWeather = applicationDal.GetParks();
            ParkWeatherModel tempPark = new ParkWeatherModel();
            List<ParkWeatherModel> detailParkList = tempPark.DetailParkList(id, parkWeather);

            bool sessionCelsius;

            if (Session["IsCelsius"] == null)
            {
                sessionCelsius = false;
            }
            else
            {
                sessionCelsius = (bool)Session["IsCelsius"];
            }

            Session["IsCelsius"] = sessionCelsius;

            return View(detailParkList);
        }

        [HttpPost]
        public ActionResult Detail(List<ParkWeatherModel> detailParkList)
        {
            bool objCelsius = (bool)Session["IsCelsius"];
            bool sessionCelsius;

            sessionCelsius = detailParkList[0].IsCelsius;

            string parkName = detailParkList[0].Name;
            Session["IsCelsius"] = sessionCelsius;

            return RedirectToAction("Detail", new { id = parkName });
        }
    }
}