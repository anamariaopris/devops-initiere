using Microsoft.AspNetCore.Mvc;
using OneModelApi.Dtos;
using OneModelApi.Services;

namespace OneModelApi.Controllers;

[ApiController]
[Route("api/cars")]
public class CarsController(CarService service) : ControllerBase
{
    [HttpGet]
    public Task<CarListResponse> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    [HttpGet("{id:long}")]
    public Task<CarResponse> GetById(long id, CancellationToken cancellationToken) =>
        service.GetByIdAsync(id, cancellationToken);

    [HttpGet("brand/{brand}")]
    public Task<CarListResponse> GetByBrand(string brand, CancellationToken cancellationToken) =>
        service.GetByBrandAsync(brand, cancellationToken);

    [HttpGet("color/{color}")]
    public Task<CarListResponse> GetByColor(string color, CancellationToken cancellationToken) =>
        service.GetByColorAsync(color, cancellationToken);

    [HttpGet("fuel/{fuelType}")]
    public Task<CarListResponse> GetByFuelType(string fuelType, CancellationToken cancellationToken) =>
        service.GetByFuelTypeAsync(fuelType, cancellationToken);

    [HttpGet("transmission/{transmission}")]
    public Task<CarListResponse> GetByTransmission(string transmission, CancellationToken cancellationToken) =>
        service.GetByTransmissionAsync(transmission, cancellationToken);

    [HttpGet("available/{available:bool}")]
    public Task<CarListResponse> GetByAvailable(bool available, CancellationToken cancellationToken) =>
        service.GetByAvailableAsync(available, cancellationToken);

    [HttpGet("search")]
    public Task<CarListResponse> Search([FromQuery] CarSearchRequest request, CancellationToken cancellationToken) =>
        service.SearchAsync(request, cancellationToken);

    [HttpPost]
    public async Task<ActionResult<CarResponse>> Create(CarRequest request, CancellationToken cancellationToken)
    {
        var car = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
    }

    [HttpPut("{id:long}")]
    public Task<CarResponse> Update(long id, CarRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(id, request, cancellationToken);

    [HttpPatch("{id:long}")]
    public Task<CarResponse> Patch(long id, CarPatchRequest request, CancellationToken cancellationToken) =>
        service.PatchAsync(id, request, cancellationToken);

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
