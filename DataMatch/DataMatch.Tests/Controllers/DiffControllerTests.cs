using DataMatch.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DataMatch.Tests.Controllers
{
    public class DiffControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public DiffControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SetDataLeft_ValidInput_ReturnsCreated()
        {
            // Arrange
            var client = _factory.CreateClient();
            var id = 1;
            var body = new InputDiff { Data = "AAAAAA==" };

            // Act
            var response = await client.PutAsJsonAsync($"/v1/diff/{id}/left", body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task SetDataLeft_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var id = 1;
            var body = new InputDiff { Data = "this should fail" };

            // Act
            var response = await client.PutAsJsonAsync($"/v1/diff/{id}/left", body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SetDataRight_ValidInput_ReturnsCreated()
        {
            // Arrange
            var client = _factory.CreateClient();
            var id = 1;
            var body = new InputDiff { Data = "AAAAAA==" };

            // Act
            var response = await client.PutAsJsonAsync($"/v1/diff/{id}/left", body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task SetDataRight_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var id = 1;
            var body = new InputDiff { Data = "this should fail" };

            // Act
            var response = await client.PutAsJsonAsync($"/v1/diff/{id}/left", body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetData_Equals_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var id = 1;
            var bodyLeft = new InputDiff { Data = "AAAAAA==" };
            var bodyRight = new InputDiff { Data = "AAAAAA==" };

            // Act
            await client.PutAsJsonAsync($"/v1/diff/{id}/left", bodyLeft);
            await client.PutAsJsonAsync($"/v1/diff/{id}/right", bodyRight);

            var response = await client.GetAsync($"/v1/diff/{id}");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonSerializer.Deserialize<DataMatchResponse>(responseContent);

            result.Should().NotBeNull();
            result.DiffResultType.Should().Be("Equals");
        }

        [Fact]
        public async Task GetData_ContentDoNotMatch_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var id = 1;
            var bodyLeft = new InputDiff { Data = "AAAAAA==" };
            var bodyRight = new InputDiff { Data = "AQABAQ==" };

            // Act
            await client.PutAsJsonAsync($"/v1/diff/{id}/left", bodyLeft);
            await client.PutAsJsonAsync($"/v1/diff/{id}/right", bodyRight);

            var response = await client.GetAsync($"/v1/diff/{id}");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonSerializer.Deserialize<DataMatchResponse>(responseContent);

            result.Should().NotBeNull();
            result.DiffResultType.Should().Be("ContentDoNotMatch");
            result.Diffs.Should().NotBeEmpty().And.HaveCountGreaterThan(1);
        }

        [Fact]
        public async Task GetData_SizeDoNotMatch_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var id = 1;
            var bodyLeft = new InputDiff { Data = "AAAAAA==" };
            var bodyRight = new InputDiff { Data = "AAA=" };

            // Act
            await client.PutAsJsonAsync($"/v1/diff/{id}/left", bodyLeft);
            await client.PutAsJsonAsync($"/v1/diff/{id}/right", bodyRight);

            var response = await client.GetAsync($"/v1/diff/{id}");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonSerializer.Deserialize<DataMatchResponse>(responseContent);

            result.Should().NotBeNull();
            result.DiffResultType.Should().Be("SizeDoNotMatch");
        }
    }
}
