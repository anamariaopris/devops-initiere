using Microsoft.AspNetCore.Mvc;

namespace mock_car_source_api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "OK",
            service = "mock-car-source-api",
            timestampUtc = DateTime.UtcNow
        });
    }
}