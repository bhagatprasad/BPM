using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/rabbitmq")]
public class RabbitMqController : ControllerBase
{
    private readonly RabbitMqService _rabbitMqService;

    public RabbitMqController(RabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        await using var connection =
            await _rabbitMqService.CreateConnectionAsync();

        return Ok("RabbitMQ connected successfully!");
    }
}