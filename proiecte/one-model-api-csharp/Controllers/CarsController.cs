using Microsoft.AspNetCore.Mvc;
using OneModelApi.Dtos;
using OneModelApi.Services;

namespace OneModelApi.Controllers;

[ApiController]
[Route("api/cars")]
public class CarsController : ControllerBase
{
    private readonly ICarQueryService queryService;
    private readonly ICarCommandService commandService;

    public CarsController(ICarQueryService queryService, ICarCommandService commandService)
    {
        this.queryService = queryService;
        this.commandService = commandService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CarResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<CarResponse>> Create([FromBody] CarCreateRequest request, CancellationToken cancellationToken)
    {
        var created = await commandService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet]
    [ProducesResponseType(typeof(CarListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CarListResponse>> GetAll(CancellationToken cancellationToken)
    {
        var cars = await queryService.GetAllAsync(cancellationToken);
        if (!cars.Cars.Any())
        {
            return NoContent();
        }

        return Ok(cars);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(CarResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CarResponse>> GetById([FromRoute] long id, CancellationToken cancellationToken)
    {
        var car = await queryService.GetByIdAsync(id, cancellationToken);
        return Ok(car);
    }

    [HttpGet("brand/{brand}")]
    [ProducesResponseType(typeof(CarListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CarListResponse>> GetByBrand([FromRoute] string brand, CancellationToken cancellationToken)
    {
        var cars = await queryService.GetByBrandAsync(brand, cancellationToken);
        if (!cars.Cars.Any())
        {
            return NoContent();
        }

        return Ok(cars);
    }

    [HttpGet("color/{color}")]
    [ProducesResponseType(typeof(CarListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CarListResponse>> GetByColor([FromRoute] string color, CancellationToken cancellationToken)
    {
        var cars = await queryService.GetByColorAsync(color, cancellationToken);
        if (!cars.Cars.Any())
        {
            return NoContent();
        }

        return Ok(cars);
    }

    [HttpGet("fuel/{fuelType}")]
    [ProducesResponseType(typeof(CarListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CarListResponse>> GetByFuelType([FromRoute] string fuelType, CancellationToken cancellationToken)
    {
        var cars = await queryService.GetByFuelTypeAsync(fuelType, cancellationToken);
        if (!cars.Cars.Any())
        {
            return NoContent();
        }

        return Ok(cars);
    }

    [HttpGet("transmission/{transmission}")]
    [ProducesResponseType(typeof(CarListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CarListResponse>> GetByTransmission([FromRoute] string transmission, CancellationToken cancellationToken)
    {
        var cars = await queryService.GetByTransmissionAsync(transmission, cancellationToken);
        if (!cars.Cars.Any())
        {
            return NoContent();
        }

        return Ok(cars);
    }

    [HttpGet("available/{available:bool}")]
    [ProducesResponseType(typeof(CarListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CarListResponse>> GetByAvailable([FromRoute] bool available, CancellationToken cancellationToken)
    {
        var cars = await queryService.GetByAvailableAsync(available, cancellationToken);
        if (!cars.Cars.Any())
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
        var cars = await queryService.SearchAsync(request, cancellationToken);
        if (!cars.Cars.Any())
        {
            return NoContent();
        }

        return Ok(cars);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(CarResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CarResponse>> Update([FromRoute] long id, [FromBody] CarPutRequest request, CancellationToken cancellationToken)
    {
        var updated = await commandService.UpdateAsync(id, request, cancellationToken);
        return Ok(updated);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(CarResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CarResponse>> Patch([FromRoute] long id, [FromBody] CarPatchRequest request, CancellationToken cancellationToken)
    {
        var updated = await commandService.PatchAsync(id, request, cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
    {
        await commandService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
