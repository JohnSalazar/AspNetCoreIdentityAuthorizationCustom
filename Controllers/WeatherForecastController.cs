using AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom;
using AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityAuthorizationCustom.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    [HttpGet]
    [Route("/weatherforecastcontroller")]
    [Authorize("BROnly")]
    [Authorize("Over15")]
    [AuthorizeCustom(PolicyType.Admin)]
    //[AuthorizeCustom(PolicyType.AdminFull)]
    public IActionResult Get()
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();

        return Ok(forecast);
    }
}