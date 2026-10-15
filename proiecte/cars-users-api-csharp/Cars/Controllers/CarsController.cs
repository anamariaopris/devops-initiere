using Microsoft.AspNetCore.Mvc;
using OneToManyApi.Dtos;
using OneToManyApi.Services;

namespace OneToManyApi.Controllers;

[ApiController]
[Route("api/cars")]
public class CarsController(ICarQueryService carQueryService, ICarCommandService carCommandService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CarResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<CarResponse>> Create([FromBody] CarCreateRequest request, CancellationToken cancellationToken)
    {
        var created = await carCommandService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(CarResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CarResponse>> Update([FromRoute] long id, [FromBody] CarUpdateRequest request, CancellationToken cancellationToken)
    {
        var updated = await carCommandService.UpdateAsync(id, request, cancellationToken);
        return Ok(updated);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(CarResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CarResponse>> Patch([FromRoute] long id, [FromBody] CarPatchRequest request, CancellationToken cancellationToken)
    {
        var updated = await carCommandService.PatchAsync(id, request, cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(CarResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CarResponse>> Delete([FromRoute] long id, CancellationToken cancellationToken)
    {
        var deleted = await carCommandService.DeleteAsync(id, cancellationToken);
        return Ok(deleted);
    }

    [HttpGet]
    [ProducesResponseType(typeof(CarListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CarListResponse>> GetAll(CancellationToken cancellationToken)
    {
        var cars = await carQueryService.GetAllAsync(cancellationToken);
        if (cars.Cars.Count == 0)
        {
            return NoContent();
        }

        return Ok(cars);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(CarResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CarResponse>> GetById([FromRoute] long id, CancellationToken cancellationToken)
    {
        var car = await carQueryService.GetByIdAsync(id, cancellationToken);
        return Ok(car);
    }

    [HttpGet("by-brand/{brand}")]
    [ProducesResponseType(typeof(CarListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CarListResponse>> GetByBrand([FromRoute] string brand, CancellationToken cancellationToken)
    {
        var cars = await carQueryService.GetByBrandAsync(brand, cancellationToken);
        if (cars.Cars.Count == 0)
        {
            return NoContent();
        }

        return Ok(cars);
    }

    [HttpGet("by-year-range")]
    [ProducesResponseType(typeof(CarListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CarListResponse>> GetByYearRange([FromQuery] int minYear, [FromQuery] int maxYear, CancellationToken cancellationToken)
    {
        var cars = await carQueryService.GetByYearRangeAsync(minYear, maxYear, cancellationToken);
        if (cars.Cars.Count == 0)
        {
            return NoContent();
        }

        return Ok(cars);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(CarListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CarListResponse>> Search([FromQuery] CarSearchRequest request, CancellationToken cancellationToken)
    {
        var cars = await carQueryService.SearchAsync(request, cancellationToken);
        if (cars.Cars.Count == 0)
        {
            return NoContent();
        }

        return Ok(cars);
    }
}
