using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Dal;
using System.Data.SqlClient;

namespace Capstone.Web.Models
{
    public class Survey
    {
        
        public int Votes { get; set; }
        public string ParkCode { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
        public string ActivityLevel { get; set; }

        public static List<SelectListItem> ActivityLevels { get; } = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "inactive", Value = "inactive"},
            new SelectListItem() { Text = "sedentary", Value = "sedentary"},
            new SelectListItem() { Text = "active", Value = "active"},
            new SelectListItem() { Text = "extremely active", Value = "extremely active"}
        };


        public static List<SelectListItem> ChoosePark { get; } = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "Cuyahoga Valley", Value = "CVNP"},
            new SelectListItem() { Text = "Everglades", Value = "ENP"},
            new SelectListItem() { Text = "Grand Canyon", Value = "GCNP"},
            new SelectListItem() { Text = "Glacier", Value = "GNP"}
        };
    }
}