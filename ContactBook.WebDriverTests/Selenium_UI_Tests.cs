using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace ContactBook.WebDriverTests
{
    public class Selenium_UI_Tests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            this.driver.Navigate().GoToUrl("https://contactbook.nakov.repl.co/");
            this.driver.Manage().Window.Maximize();
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void ShutDown()
        {
            driver.Close();
        }

        //We have two ways to visualize all contacts - from View Contacts button OR from Contacts button
        //TC01 AND TC02 are verifying if this is working as expected.

        [Test]
        public void TC01_ListAllContacts_ViewContactsButton_IsFirstContact_SteveJobs()
        {
            var viewContactsButton = driver.FindElement(By.CssSelector("body > main > div > a:nth-child(1) > span.icon"));
            viewContactsButton.Click();

            var allContacts = driver.FindElements(By.ClassName("contacts-grid"));
            var firstContact = allContacts[0];

            var firstContactName = firstContact.FindElement(By.CssSelector("#contact1 > tbody > tr.fname > td"));
            var firstContactFirstName = firstContactName.Text;

            var firstContactLName = firstContact.FindElement(By.CssSelector("#contact1 > tbody > tr.lname > td"));
            var firstContactLastName = firstContactLName.Text;

            Assert.AreEqual("Steve", firstContactFirstName);
            Assert.AreEqual("Jobs", firstContactLastName);
        }

        [Test]
        public void TC02_ListAllContacts_ContactsButton_IsFirstContact_SteveJobs()
        {
            var contactsButton = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(2) > a"));
            contactsButton.Click();

            var allContacts = driver.FindElements(By.ClassName("contacts-grid"));
            var firstContact = allContacts[0];

            var firstContactName = firstContact.FindElement(By.CssSelector("#contact1 > tbody > tr.fname > td"));
            var firstContactFirstName = firstContactName.Text;

            var firstContactLName = firstContact.FindElement(By.CssSelector("#contact1 > tbody > tr.lname > td"));
            var firstContactLastName = firstContactLName.Text;

            Assert.AreEqual("Steve", firstContactFirstName);
            Assert.AreEqual("Jobs", firstContactLastName);
        }

        [Test]
        public void TC03_SearchForAlbert_IsFirstContact_AlbertEinstein()
        {
            var searchContactButton = driver.FindElement(By.CssSelector("body > main > div > a:nth-child(3) > span:nth-child(2)"));
            searchContactButton.Click();

            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.Clear();
            searchField.Click();
            searchField.SendKeys("albert");

            var searchButton = driver.FindElement(By.Id("search"));
            searchButton.Click();

            var allContacts = driver.FindElements(By.ClassName("contacts-grid"));
            var firstContact = allContacts[0];

            var firstContactName = firstContact.FindElement(By.CssSelector("#contact3 > tbody > tr.fname > td"));
            var firstContactFirstName = firstContactName.Text;

            var firstContactLName = firstContact.FindElement(By.CssSelector("#contact3 > tbody > tr.lname > td"));
            var firstContactLastName = firstContactLName.Text;

            Assert.AreEqual("Albert", firstContactFirstName);
            Assert.AreEqual("Einstein", firstContactLastName);
        }

        [Test]
        public void TC04_SearchForInvalid2635_AreZeroContactsFound()
        {
            var searchContactButton = driver.FindElement(By.CssSelector("body > main > div > a:nth-child(3) > span:nth-child(2)"));
            searchContactButton.Click();

            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.Clear();
            searchField.Click();
            searchField.SendKeys("invalid2635");

            var searchButton = driver.FindElement(By.Id("search"));
            searchButton.Click();

            var resultsText = driver.FindElement(By.Id("searchResult")).Text;
            
            Assert.AreEqual("No contacts found.", resultsText);
        }

        [Test]
        public void TC05_CreateInvalidUser_IsErrorMessageReturned()
        {
            var createContactButton = driver.FindElement(By.CssSelector("body > main > div > a:nth-child(2)"));
            createContactButton.Click();

            var fnameField = driver.FindElement(By.Id("firstName"));
            fnameField.Clear();
            fnameField.Click();
            fnameField.SendKeys("test");


            Create();
            Assert.IsTrue(driver.FindElement(By.ClassName("err")).Displayed);
            Assert.AreEqual("Error: Last name cannot be empty!", driver.FindElement(By.ClassName("err")).Text);

            var lnameField = driver.FindElement(By.Id("lastName"));
            lnameField.Clear();
            lnameField.Click();
            lnameField.SendKeys("testov");

            Create();
            Assert.IsTrue(driver.FindElement(By.ClassName("err")).Displayed);
            Assert.AreEqual("Error: Invalid email!", driver.FindElement(By.ClassName("err")).Text);

            var emailield = driver.FindElement(By.Id("email"));
            emailield.Clear();
            emailield.Click();
            emailield.SendKeys("testche");
            Create();
            Assert.IsTrue(driver.FindElement(By.ClassName("err")).Displayed);
            Assert.AreEqual("Error: Invalid email!", driver.FindElement(By.ClassName("err")).Text);

           
        }

        public void Create()
        {
            var createButton = driver.FindElement(By.Id("create"));

            var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(5));
            var clickableElement = wait.Until(d => d.FindElement(By.Id("create"))).Displayed;

            createButton.Click();
        }


        [Test]
        public void TC_06_CreateUserWithValidData()
        {
            var createContactButton = driver.FindElement(By.CssSelector("body > main > div > a:nth-child(2)"));
            createContactButton.Click();

            var fnameField = driver.FindElement(By.Id("firstName"));
            fnameField.Clear();
            fnameField.Click();
            fnameField.SendKeys("Buy");

          

            var lnameField = driver.FindElement(By.Id("lastName"));
            lnameField.Clear();
            lnameField.Click();
            var dateTimeUTC = DateTime.UtcNow;
            lnameField.SendKeys(dateTimeUTC + "Bitcoin");
            

            var emailield = driver.FindElement(By.Id("email"));
            emailield.Clear();
            emailield.Click();
            emailield.SendKeys("Bitcoin@abv.bg");

            var phoneField = driver.FindElement(By.Id("phone"));
            phoneField.Clear();
            phoneField.Click();
            phoneField.SendKeys("+35980010000");

            var commentField = driver.FindElement(By.Id("comments"));
            commentField.Clear();
            commentField.Click();
            commentField.SendKeys("BTC to 10M EURO");

            Create();

            var allContacts = driver.FindElements(By.CssSelector("table.contact-entry"));
            var lastContact = allContacts.Last();

            var firstNameLabel = lastContact.FindElement(By.CssSelector("tr.fname > td")).Text;
            var lastNameLabel = lastContact.FindElement(By.CssSelector("tr.lname > td")).Text;

            Assert.AreEqual("Buy", firstNameLabel);
            Assert.AreEqual(dateTimeUTC + "Bitcoin", lastNameLabel);

        }
    }
}