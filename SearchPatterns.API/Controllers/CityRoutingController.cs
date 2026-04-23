using Microsoft.AspNetCore.Mvc;
using SearchPatterns.Application.CityRouting.DTOs;
using SearchPatterns.Application.CityRouting.Services;

namespace SearchPatterns.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CityRoutingController : ControllerBase
{
    private readonly ICityRoutingService _routingService;

    public CityRoutingController(ICityRoutingService routingService)
    {
        _routingService = routingService;
    }

    [HttpGet("cities")]
    public IActionResult GetCities()
    {
        var cities = _routingService.GetAvailableCities();
        return Ok(cities);
    }

    [HttpPost("calculate")]
    public IActionResult CalculateRoute([FromBody] CityRoutingRequest request)
    {
        try
        {
            var result = _routingService.CalculateRoute(request.StartCity, request.TargetCity, request.Algorithm);
            
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            
            return BadRequest(result.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
