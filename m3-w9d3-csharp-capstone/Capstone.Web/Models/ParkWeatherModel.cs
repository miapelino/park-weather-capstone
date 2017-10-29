using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace Capstone.Web.Models
{
    public class ParkWeatherModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public int Acreage { get; set; }
        public int Elevation { get; set; }
        public double MilesOfTrail { get; set; }
        public int NumCampsites { get; set; }
        public string Climate { get; set; }
        public int YearFounded { get; set; }
        public int AnnualVisitor { get; set; }
        public string Quote { get; set; }
        public string QuoteSource { get; set; }
        public string Description { get; set; }
        public int EntryFee { get; set; }
        public int NumSpecies { get; set; }

        public string FiveDayForecastValue { get; set; }
        public int Low { get; set; }
        public int High { get; set; }
        public string Forecast { get; set; }

        public bool IsCelsius { get; set; }

        public int ConvertToCelsius(int temp)
        {
            int newTemp = Convert.ToInt32((temp - 32) / 1.8);
            return newTemp;
        }

        public List<ParkWeatherModel> DetailParkList(string name, List<ParkWeatherModel> parkList)
        {
            List<ParkWeatherModel> parkDetails = new List<ParkWeatherModel>();
            foreach (ParkWeatherModel p in parkList)
            {
                if (name == p.Name)
                {
                    parkDetails.Add(p);
                }
            }
            return parkDetails;
        }

        public List<string> ForecastRecommendations(ParkWeatherModel parkWeather)
        {
            List<string> recommendations = new List<string>();
            ParkWeatherModel tempParkWeather = parkWeather;
            if (tempParkWeather.Forecast == "snow")
            {
                recommendations.Add("Pack snowshoes");
            }
            else if (tempParkWeather.Forecast == "rain" || tempParkWeather.Forecast == "thunderstorms")
            {
                recommendations.Add("Pack rain gear and wear waterproof shoes");
            }
            if (tempParkWeather.Forecast == "thunderstorms")
            {
                recommendations.Add("Seek shelter and avoid hiking on exposed ridges");
            }
            else if (tempParkWeather.Forecast == "sunny")
            {
                recommendations.Add("Pack sunblock");
            }
            if (tempParkWeather.High > 75)
            {
                recommendations.Add("Bring an extra gallon of water");
            }
            else if (tempParkWeather.Low < 20)
            {
                recommendations.Add("Warning: exposure to frigid temperatures is dangerous");
            }
            if ((tempParkWeather.High - tempParkWeather.Low) > 20)
            {
                recommendations.Add("Wear breathable layers");
            }

            return recommendations;
        }

        public string GetShortDescription()
        {
            string result = "";
            string[] description = Description.Split('.');
            if (description[0].Contains("Mt"))
            {
                result += description[0] + "." + description[1] + ".";
            }
            else result = description[0] + ".";
            return result;
        }

        public string GetForecastImageName(string forecast)
        {
            string forecastImgName = Forecast;
            if (forecastImgName == "partly cloudy")
            {
                forecastImgName = "partlyCloudy";
            }
            return forecastImgName;
        }

        public string GetDayOfForecast()
        {
            string weekDay = "";

            if (FiveDayForecastValue == "1")
            {
                weekDay = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToString("MM/dd");
            }
            else if (FiveDayForecastValue == "2")
            {
                weekDay = DateTime.Now.AddDays(1).DayOfWeek + ", " + DateTime.Now.AddDays(1).ToString("MM/dd");
            }
            else if (FiveDayForecastValue == "3")
            {
                weekDay = DateTime.Now.AddDays(2).DayOfWeek + ", " + DateTime.Now.AddDays(2).ToString("MM/dd");
            }
            else if (FiveDayForecastValue == "4")
            {
                weekDay = DateTime.Now.AddDays(3).DayOfWeek + ", " + DateTime.Now.AddDays(3).ToString("MM/dd");
            }
            else if (FiveDayForecastValue == "5")
            {
                weekDay = DateTime.Now.AddDays(4).DayOfWeek + ", " + DateTime.Now.AddDays(4).ToString("MM/dd");
            }
            return weekDay;
        }

        public string ToTitleCase(string text)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(text);
        }
    }
}