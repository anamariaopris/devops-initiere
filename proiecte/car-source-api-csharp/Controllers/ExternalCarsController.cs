using Microsoft.AspNetCore.Mvc;

namespace mock_car_source_api.Controllers;

[ApiController]
[Route("api/external/cars")]
public class ExternalCarsController : ControllerBase
{
    private readonly ExternalCarCatalogService _catalogService;

    public ExternalCarsController(ExternalCarCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    public IActionResult GetAllCars()
    {
        var cars = _catalogService.GetAll();
        return Ok(cars);
    }

    [HttpGet("{externalId}")]
    public IActionResult GetCarById(string externalId)
    {
        var car = _catalogService.GetById(externalId);

        if (car is null)
        {
            return NotFound();
        }

        return Ok(car);
    }

    [HttpPost("regenerate")]
    public IActionResult RegenerateCars([FromQuery] int count = 50)
    {
        _catalogService.Regenerate(count);
        return Ok(new { message = $"Generated {count} cars." });
    }
}