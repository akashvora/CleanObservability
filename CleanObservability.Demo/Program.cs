using CleanObservability.Core.Setup;
using CleanObservability.Demo.Extensions;
using CleanObservability.Demo.Middlewares;
using CleanObservability.WebAPI.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using Serilog.Sinks;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.PostgreSQL;



var builder = WebApplication.CreateBuilder(args);
var columnWriters = new Dictionary<string, ColumnWriterBase>
{
	{ "Message", new RenderedMessageColumnWriter() },
	{ "MessageTemplate", new MessageTemplateColumnWriter() },
	{ "Level", new LevelColumnWriter() },
	{ "TimeStamp", new TimestampColumnWriter() },
	{ "Exception", new ExceptionColumnWriter() },
	{ "LogEvent", new LogEventSerializedColumnWriter() }
};

var configuration = builder.Configuration;

var now = DateTime.UtcNow;
var logPath = $"Logs/{now:yyyy}/log-{now:yyyy-MM-dd}.txt";

Directory.CreateDirectory(Path.GetDirectoryName(logPath)!); // Ensure folder exists

var connectionString1 = $"Host={configuration["LoggingDb:Host"]};Port={configuration["LoggingDb:Port"]};Database={configuration["LoggingDb:Database"]};Username={configuration["LoggingDb:Username"]};Password={configuration["LoggingDb:Password"]}";

Serilog.Debugging.SelfLog.Enable(msg =>
{
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine($"[Serilog Sink Error] {msg}");
	Console.ResetColor();
});

Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File(
		path: logPath,
		rollingInterval: RollingInterval.Infinite, // 📌 prevent auto-renaming
		retainedFileCountLimit: null,              // ♾ keep logs forever
		shared: true,
		outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
		rollOnFileSizeLimit: true,
		fileSizeLimitBytes: 100_000_000 // (Optional: if rolling by size too)
		)

	.WriteTo.PostgreSQL(
		connectionString: connectionString1, //configuration.GetConnectionString("PostgresLogDb"),
		tableName: "Logs",
		columnOptions: columnWriters,
		needAutoCreateTable: true)
	.WriteTo.Seq("http://localhost:5341")// ✅ Seq sink
	.WriteTo.GrafanaLoki(
		"http://localhost:3100", // Loki endpoint
		labels: new[]
		{
			new LokiLabel { Key = "app", Value = "clean-observability" },
			new LokiLabel { Key = "traceId", Value = "${TraceId}" },
			new LokiLabel { Key = "requestPath", Value = "${RequestPath}" }
		})
	.CreateLogger();

builder.Host.UseSerilog();

builder.Services
	.AddRequestContextEnrichment()
	.AddObservability(options => options.ServiceName = "DemoApi");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseRequestContextEnrichment();
app.UseExceptionToProblemDetails();
app.UseObservabilityMiddlewares();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // mapOpenApi() is the replacement for SwaggerEndpoint in .NET 7+. not needed if you are using SwaggerUI
					  //  swagger is now integrated with OpenAPI, so you can use MapOpenApi to expose the OpenAPI document directly.
					  // This will generate the OpenAPI document at /openapi.json by default.
					  // If you want to use Swagger UI, you can still do so by calling UseSwaggerUI()
					  // but you don't need to call UseSwagger() anymore.
					  //app.UseSwagger(); // Use this if you want to expose the OpenAPI document at /swagger/v1/swagger.json

	/*app.UseSwagger();*/
	app.UseSwagger();
	app.UseSwaggerUI();
//app.UseSwaggerUI(options =>
//	{
//		options.SwaggerEndpoint("/openapi.json", "Demo API v1");
//	});


}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


//namespace CleanObservability.Demo;
public partial class Program { }
