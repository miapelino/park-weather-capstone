using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public int FiveDayForecastValue { get; set; }
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
    }
}