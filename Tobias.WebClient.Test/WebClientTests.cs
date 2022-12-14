using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;

namespace Tobias.WebClient.Test
{
    [TestFixture]
    public class WebClientTests
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void SearchCourseTest()
        {
            driver.Navigate().GoToUrl("https://www.du.se/");
            driver.Manage().Window.Size = new System.Drawing.Size(915, 944);
            driver.FindElement(By.LinkText("Utbildning")).Click();
            {
                var element = driver.FindElement(By.LinkText("Utbildning"));
                Actions builder = new Actions(driver);
                builder.MoveToElement(element).Perform();
            }
            //{
            //  var element = driver.FindElement(By.TagName("body"));
            //  Actions builder = new Actions(driver);
            //  builder.MoveToElement(element, 0, 0).Perform();
            //}
            driver.FindElement(By.LinkText("Samverkan")).Click();
            //driver.FindElement(By.LinkText("KLOSS-Net")).Click();
        }

        [Test]
        public void NavigateTobias()
        {
            driver.Navigate().GoToUrl("http://localhost:4200/");
            driver.Manage().Window.Size = new System.Drawing.Size(968, 540);
            driver.FindElement(By.CssSelector(".card:nth-child(1) > span")).Click();
            driver.FindElement(By.CssSelector(".card:nth-child(3) > span")).Click();
            driver.FindElement(By.Name("Edit")).Click();
            js.ExecuteScript("window.scrollTo(0,122)");
            driver.FindElement(By.CssSelector("tr:nth-child(3) textarea")).Click();
            driver.FindElement(By.CssSelector("tr:nth-child(2) textarea")).SendKeys("Kalle");
            driver.FindElement(By.CssSelector("tr:nth-child(3) textarea")).SendKeys("Kula");
            driver.FindElement(By.CssSelector("button")).Click();
            driver.FindElement(By.LinkText("Stem Cell Recipients")).Click();
            driver.FindElement(By.Name("Edit")).Click();
            js.ExecuteScript("window.scrollTo(0,135)");
            driver.FindElement(By.CssSelector("tr:nth-child(2) textarea")).Click();
            driver.FindElement(By.CssSelector("tr:nth-child(2) textarea")).SendKeys("olle");
            driver.FindElement(By.CssSelector("tr:nth-child(3) textarea")).Click();
            driver.FindElement(By.CssSelector("tr:nth-child(3) textarea")).SendKeys("bolle");
            driver.FindElement(By.CssSelector("button")).Click();
            driver.FindElement(By.CssSelector(".card:nth-child(3) > svg:nth-child(1)")).Click();
            driver.FindElement(By.Name("Open")).Click();
            js.ExecuteScript("window.scrollTo(0,72)");
            driver.FindElement(By.CssSelector(".circle-link:nth-child(2) g > g > path:nth-child(1)")).Click();
            driver.FindElement(By.CssSelector(".circle-link:nth-child(1) g > g > path")).Click();
            driver.FindElement(By.CssSelector(".circle-link:nth-child(3) > svg")).Click();
            driver.FindElement(By.CssSelector(".card:nth-child(1) > span")).Click();
        }
    }
}