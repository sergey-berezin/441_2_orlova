using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using FluentAssertions;
using Newtonsoft.Json;
using System.Text;

namespace WebApplication1
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UnitTest1(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Endpoint_ReturnsSuccess()
        {
            string id = "1 2 3";
            var requestUri = $"/initial/{id}";

            var response = await _client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Post_Endpoint_ReturnsCreated()
        {
            Evolution square = new Evolution(1, 2, 3);
            PopulationData data = new PopulationData();
            data.Population = square.Population;
            data.Iter_num = 0;
            data.Anum = 1;
            data.Bnum = 2;
            data.Cnum = 3;

            var requestUri = "/next";
            var content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json");
            var response = await _client.PostAsync(requestUri, content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}