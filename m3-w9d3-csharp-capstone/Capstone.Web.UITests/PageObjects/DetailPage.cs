using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Web.UITests.PageObjects
{
    public class DetailPage : BasePage
    {
        public DetailPage(IWebDriver driver)
            :base(driver, "/Home/Detail/Yosemite%20National%20Park")
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "C")]
        public IWebElement C { get; set; }

        [FindsBy(How = How.Id, Using = "F")]
        public IWebElement F { get; set; }

        [FindsBy(How = How.Name, Using = "IsCelsius")]
        public IWebElement IsCelsius { get; set; }

        [FindsBy(How= How.CssSelector, Using = "button")]
        public IWebElement SubmitButton { get; set; }

        public DetailPage TempConversion(TempTypes temperature)
        {
            string id = temperature.ToString();
            IWebElement radioButton = driver.FindElement(By.Id(id));
            radioButton.Click();

            SubmitButton.Click();

            return new DetailPage(driver);
        }

        public enum TempTypes
        {
            F, C
        }
    }
}
