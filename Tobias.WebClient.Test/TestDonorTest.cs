// Generated by Selenium IDE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;
using System.Reflection;

[TestFixture]
public class TestDonorTest {
  private IWebDriver driver;
  public IDictionary<string, object> vars {get; private set;}
  private IJavaScriptExecutor js;
  [SetUp]
  public void SetUp() {
    driver = new ChromeDriver();
    js = (IJavaScriptExecutor)driver;
    vars = new Dictionary<string, object>();
  }
  [TearDown]
  protected void TearDown() {
    driver.Quit();
  }
  [Test]
  public void testDonor() {
    driver.Navigate().GoToUrl("http://localhost:4200/");
    driver.Manage().Window.Size = new System.Drawing.Size(1294, 1410);
    driver.FindElement(By.LinkText("Donors")).Click();
    new WebDriverWait(driver, TimeSpan.FromMilliseconds(1000))
                        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                        .ElementExists(By.Name("Add")));
    driver.FindElement(By.Name("Add")).Click();
    driver.FindElement(By.CssSelector("tr:nth-child(2) .ng-untouched")).Clear();
    driver.FindElement(By.CssSelector("tr:nth-child(3) .ng-untouched")).Clear();
    driver.FindElement(By.CssSelector("tr:nth-child(4) .ng-untouched")).Clear();
    driver.FindElement(By.CssSelector("tr:nth-child(5) .ng-untouched")).Clear();
    driver.FindElement(By.CssSelector("tr:nth-child(6) .ng-untouched")).Clear();
    driver.FindElement(By.CssSelector("tr:nth-child(2) .ng-valid")).SendKeys("Atichoke");
    driver.FindElement(By.CssSelector("tr:nth-child(3) .ng-valid")).SendKeys("Nantarat");
    driver.FindElement(By.CssSelector("tr:nth-child(4) .ng-valid")).SendKeys("199905277954");
    driver.FindElement(By.CssSelector("tr:nth-child(5) .ng-valid")).SendKeys("Rh+");
    driver.FindElement(By.CssSelector("tr:nth-child(6) .ng-valid")).SendKeys("A");
    new WebDriverWait(driver, TimeSpan.FromMilliseconds(1000))
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                    .ElementExists(By.CssSelector("button")));
    driver.FindElement(By.CssSelector("button")).Click();
    new WebDriverWait(driver, TimeSpan.FromMilliseconds(1000))
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                    .ElementExists(By.LinkText("Donors")));
    driver.FindElement(By.LinkText("Donors")).Click();
    new WebDriverWait(driver, TimeSpan.FromMilliseconds(1000))
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                    .ElementExists(By.Name("Open")));
    driver.FindElement(By.Name("Open")).Click();
    new WebDriverWait(driver, TimeSpan.FromMilliseconds(1000))
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                    .ElementExists(By.CssSelector("button")));
    driver.FindElement(By.CssSelector("button")).Click();

    var element = driver.FindElement(By.CssSelector("tr:nth-child(2) .ng-valid")).GetAttribute("ng-reflect-model");
    Assert.AreEqual("Atichoke", element);

    driver.FindElement(By.CssSelector("tr:nth-child(6) .ng-untouched")).Clear();
    driver.FindElement(By.CssSelector("tr:nth-child(6) .ng-valid")).SendKeys("B");
    driver.FindElement(By.CssSelector("tr:nth-child(3) .ng-untouched")).Clear();
    driver.FindElement(By.CssSelector("tr:nth-child(3) .ng-valid")).SendKeys("Palo");
    new WebDriverWait(driver, TimeSpan.FromMilliseconds(1000))
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                    .ElementExists(By.CssSelector("button")));
    driver.FindElement(By.CssSelector("button")).Click();
    driver.FindElement(By.LinkText("Donors")).Click();
    new WebDriverWait(driver, TimeSpan.FromMilliseconds(1000))
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                    .ElementExists(By.Name("Delete")));
    driver.FindElement(By.Name("Delete")).Click();
  }
}
