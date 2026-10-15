using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using mock_car_source_api.Errors;
using mock_car_source_api.Models;
using mock_car_source_api.Services;

namespace mock_car_source_api.Controllers;

[ApiController]
[Route("api/external/cars")]
public class ExternalCarsController(ExternalCarCatalogService catalog) : ControllerBase
{
    [HttpGet]
    public IReadOnlyList<ExternalCarDto> GetAllCars() => catalog.GetAll();

    [HttpGet("{externalId}")]
    public ExternalCarDto GetCarById(string externalId) =>
        catalog.GetById(externalId) ?? throw new NotFoundException($"Car {externalId} not found.");

    [HttpPost("regenerate")]
    public IActionResult RegenerateCars([FromQuery, Range(1, 1000)] int count = 50)
    {
        catalog.Regenerate(count);
        return Ok(new { message = $"Generated {count} cars." });
    }
}
