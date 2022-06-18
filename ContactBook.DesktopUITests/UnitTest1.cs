using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;

namespace ContactBook.DesktopUITests
{
    public class Tests
    {
        public WindowsDriver<WindowsElement> driver;
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";
        private AppiumOptions options;
        private const string AppPath = @"C:\Users\lhmdai1\Downloads\ContactBook-DesktopClient\ContactBook-DesktopClient.exe";


        [SetUp]
        public void Setup()
        {
            options = new AppiumOptions();
            options.AddAdditionalCapability("platformName", "Windows");
            options.AddAdditionalCapability("app", AppPath);

            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void TC_SearchForSteveJobs()
        {
            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            string windowName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowName);

            var searchField = driver.FindElementByAccessibilityId("textBoxSearch");
            searchField.Clear();
            searchField.SendKeys("steve");

            var searchButton = driver.FindElementByAccessibilityId("buttonSearch");
            searchButton.Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            var element = wait.Until(d =>
            {
                var searchLabel = driver.FindElementByAccessibilityId("labelResult").Text;
                return searchLabel.StartsWith("Contacts found");
            });

            var fName = driver.FindElementByXPath("//Edit[@Name=\"FirstName Row 0, Not sorted.\"]").Text;
            var lName = driver.FindElementByXPath("//Edit[@Name=\"LastName Row 0, Not sorted.\"]").Text;

            Assert.AreEqual("Steve", fName);
            Assert.AreEqual("Jobs", lName);
        }
    }
}