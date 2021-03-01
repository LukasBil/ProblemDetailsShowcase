using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ProblemDetailsShowcase;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProblemDetails.Showcase.Tests
{
    [TestFixture]
    public class ServiceControllerTests
    {
        private CustomWebApplicationFactory _webApplicationFactory;
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void SetUp()
        {
            _webApplicationFactory = new CustomWebApplicationFactory();
            _httpClient = _webApplicationFactory.CreateClient();
        }

        [Test]
        public async Task GetService_ServiceDoesntExist_ThrowsNotFound()
        {
            var guid = new Guid();
            var response = await _httpClient.GetAsync($"api/service/{guid}");
            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task AddService_ServiceWithSameNameAlreadyExists_ReturnsValidationProblem()
        {
            var service = new Service { Id = new Guid(), Name = "TestName" };
            var content = new StringContent(JsonSerializer.Serialize(service), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"api/service", content);
            var response = await _httpClient.PostAsync($"api/service", content);
            var responseContent = JsonSerializer.Deserialize<ValidationProblemDetails>(await response.Content.ReadAsStringAsync()) ;
            
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
            Assert.IsTrue(responseContent.Errors.ContainsKey(nameof(service.Name)));
            Assert.AreEqual("Service with name TestName already exists", responseContent.Errors[nameof(service.Name)][0]);
            Assert.AreEqual("application/problem+json", response.Content.Headers.ContentType.ToString());
        }
    }
}
