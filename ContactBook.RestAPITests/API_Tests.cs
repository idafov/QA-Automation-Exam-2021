using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace ContactBook.RestAPITests
{
    public class API_Tests
    {
        private RestClient client;    
        private RestRequest request;
        private const string url = "https://contactbook.nakov.repl.co/api";

        [SetUp]
        public void Setup()
        {
            client = new RestClient();
        }

        [Test]
        public void TC_GetAllClient_IsFirstClient_SteveJobs()
        {
            request = new RestRequest(url + "/contacts", Method.Get);
            var response = client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            Assert.That(contacts.Count, Is.GreaterThan(0));
            Assert.AreEqual("Steve", contacts[0].firstName);
            Assert.AreEqual("Jobs", contacts[0].lastName);
        }

        [Test]
        public void TC_SearchForAlbert_IsFirstContact_AlbertEinstein()
        {
            request = new RestRequest(url + "/contacts/search/albert", Method.Get);
            
            var response = client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            Assert.That(contacts.Count, Is.GreaterThan(0));
            Assert.AreEqual("Albert", contacts[0].firstName);
            Assert.AreEqual("Einstein", contacts[0].lastName);
        }

        [Test]
        public void TC_SearchForMissingContact_AssertNoContactsFound()
        {
            request = new RestRequest(url + "/contacts/search/{keyword}", Method.Get);
            request.AddUrlSegment("keyword", "missing12345");

            var response = client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);


            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            Assert.That(contacts.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC_CreateNewContactInvalidData_AssertNoContactCreatedANdErrorIsReturned()
        {
            request = new RestRequest(url + "/contacts", Method.Post);

            var body = new
            {
                firstName = "Bitcoin"
            };

            request.AddJsonBody(body);

            var response = client.Execute(request);
            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsTrue(response.Content.Contains("Last name cannot be empty!"));
        }

        [Test]
        public void TC_CreateNewContactValidData_AssertContactCreatedAndProperlyListed()
        {
            request = new RestRequest(url + "/contacts");

            var body = new
            {
                firstName = "Bitcoin",
                lastName = "BTC",
                email = "btc10meuro@abv.bg",
                phone = "+3590000000"
            };

            request.AddJsonBody(body);

            var response = client.Execute(request, Method.Post);

            var allContacts = client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(allContacts.Content);
            var lastContactAdded = contacts.Last();

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            Assert.AreEqual(body.firstName, lastContactAdded.firstName);
            Assert.AreEqual(body.lastName, lastContactAdded.lastName);
            Assert.AreEqual(body.email, lastContactAdded.email);
            Assert.AreEqual(body.phone, lastContactAdded.phone);
        }
    }
}