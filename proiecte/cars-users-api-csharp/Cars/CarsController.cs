using Microsoft.AspNetCore.Mvc;

namespace OneToManyApi.Cars;

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

    [HttpGet("by-brand/{brand}")]
    public Task<CarListResponse> GetByBrand(string brand, CancellationToken cancellationToken) =>
        service.GetByBrandAsync(brand, cancellationToken);

    [HttpGet("by-year-range")]
    public Task<CarListResponse> GetByYearRange(int minYear, int maxYear, CancellationToken cancellationToken) =>
        service.GetByYearRangeAsync(minYear, maxYear, cancellationToken);

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
    public Task<CarResponse> Delete(long id, CancellationToken cancellationToken) =>
        service.DeleteAsync(id, cancellationToken);
}
