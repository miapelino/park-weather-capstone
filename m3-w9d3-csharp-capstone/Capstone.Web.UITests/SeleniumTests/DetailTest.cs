using Capstone.Web.UITests.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Web.UITests.SeleniumTests
{
    [TestClass]
    public class DetailTest
    {
        private static IWebDriver driver;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:55601/");
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            driver.Close();
            driver.Quit();
        }

        [TestMethod]
        public void Selenium_TemperatureCoversion_Radiobutton_RedirectDetailView()
        {
            DetailPage entryPage = new DetailPage(driver);
            entryPage.Navigate();

            DetailPage testdetailPage = entryPage.TempConversion(DetailPage.TempTypes.C);

            string result = entryPage.C.Text;

            Assert.AreEqual("C", result);

            testdetailPage = entryPage.TempConversion(DetailPage.TempTypes.F);

            result = entryPage.F.Text;

            Assert.AreEqual("F", result);

        }
    }
}
