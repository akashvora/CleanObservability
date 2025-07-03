using CleanObservability.Core.Extensions;
using CleanObservability.Core.Results;
using CleanObservability.Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanObservability.Demo.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
	private readonly ILogger<HelloController> _logger;

	public HelloController(ILogger<HelloController> logger)
	{
		_logger = logger;
	}

	[HttpPost]
	public IActionResult Greet(HelloRequest request)
	{
		var response = new HelloResponse
		{
			Message = $"Hello, {request.Name}!"
		};
		_logger.LogInformation("📨 Received request with name='{Name}' and TraceId={TraceId}", request.Name, HttpContext.TraceIdentifier);
		return Result<HelloResponse>.Success(response).ToActionResult(this);
	}
	[HttpPost("SayHello")]
	public IActionResult SayHello(GreetRequest request)
	{
		var response = new HelloResponse
		{
			Message = $"Hello, {request.Name}! Say Hello Model."
		};
		_logger.LogInformation("📨 Received request with name='{Name}' and TraceId={TraceId}", request.Name, HttpContext.TraceIdentifier);
		return Result<HelloResponse>.Success(response).ToActionResult(this);
	}
	[HttpGet("logs/latest")]
	public IActionResult GetLatestLogs()
	{
		var path = $"Logs/{DateTime.UtcNow:yyyy}/log-{DateTime.UtcNow:yyyy-MM-dd}.txt";

		//if (!System.IO.File.Exists(path))
		//{
		//	return NotFound("No logs found for today.");
		//}

		//var content = System.IO.File.ReadAllText(path);// Let it throw if missing   // this will throws shared resource kind error "files is already in use you cant access etc"
		using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		using var reader = new StreamReader(stream);
		var content = reader.ReadToEnd();
		//return File(content, "text/plain");
		return File(Encoding.UTF8.GetBytes(content), "text/plain");
	}
	public class GreetRequest
	{
		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; }
	}

}