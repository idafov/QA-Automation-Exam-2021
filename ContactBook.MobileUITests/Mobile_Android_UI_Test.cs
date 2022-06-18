using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using System;

namespace ContactBook.MobileUITests
{
    public class Mobile_Android_UI_Test
    {
        public AndroidDriver<AndroidElement> driver;
        private const string appiumServer = "http://127.0.0.1:4723/wd/hub";
        private AppiumOptions options;
        private const string AppPath = @"C:\Users\lhmdai1\Downloads\contactbook-androidclient.apk";

        [SetUp]
        public void Setup()
        {
            options = new AppiumOptions();
            options.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            options.AddAdditionalCapability(MobileCapabilityType.App, AppPath);

            driver = new AndroidDriver<AndroidElement>(new Uri(appiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void TC_SearchForSteveJobs()
        {
            var connectButton = driver.FindElement(By.Id("contactbook.androidclient:id/buttonConnect"));
            connectButton.Click();

            var textField = driver.FindElement(By.Id("contactbook.androidclient:id/editTextKeyword"));
            textField.Clear();
            textField.SendKeys("steve");

            var searchButton = driver.FindElement(By.Id("contactbook.androidclient:id/buttonSearch"));
            searchButton.Click();

        }
    }
}