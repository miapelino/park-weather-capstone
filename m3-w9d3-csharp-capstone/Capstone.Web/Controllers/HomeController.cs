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
            return View(detailParkList);
        }

        [HttpPost]
        public ActionResult Detail(string id, bool isCelsius)
        {
            List<ParkWeatherModel> parkWeather = applicationDal.GetParks();
            ParkWeatherModel tempPark = new ParkWeatherModel();

            List<ParkWeatherModel> detailParkList = Session["IsCelsius"] as List<ParkWeatherModel>;
            if (detailParkList == null)
            {
                detailParkList = new List<ParkWeatherModel>();
            }

            detailParkList = tempPark.DetailParkList(id, parkWeather);
            detailParkList[0].IsCelsius = isCelsius;

            Session["IsCelsius"] = detailParkList;

            return View(detailParkList);
        }
    }
}