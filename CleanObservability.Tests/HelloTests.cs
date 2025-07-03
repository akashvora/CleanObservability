using CleanObservability.Demo;
using CleanObservability.Demo.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace CleanObservability.Tests;

public class HelloTests : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public HelloTests(WebApplicationFactory<Program> factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task Greet_Returns_Success_When_Name_Is_Provided()
	{
		var request = new HelloRequest { Name = "Akash" };
		var response = await _client.PostAsJsonAsync("/hello", request);

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var payload = await response.Content.ReadFromJsonAsync<HelloResponse>();
		payload!.Message.Should().Be("Hello, Akash!");
	}

	[Fact]
	public async Task Greet_Returns_400_When_Name_Is_Missing()
	{
		var request = new HelloRequest { Name = "" };
		var response = await _client.PostAsJsonAsync("/hello", request);

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		var content = await response.Content.ReadAsStringAsync();
		//content.Should().Contain("validation_error");
		content.Should().Contain("\"errors\"");
		content.Should().Contain("\"Name\"");
		content.Should().Contain("must not be empty");
		content.Should().Contain("traceId");
	}

	[HttpGet("boom")]
	public IActionResult Boom()
	{
		throw new InvalidOperationException("💣 This is a deliberate crash");
	}

}