using CarCompanies.Domain;
using CarCompanies.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CarCompanies.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly ILogger<EventController> _logger;
    private readonly IEventService _eventService;

    public EventController(ILogger<EventController> logger, IEventService eventService)
    {
        _logger = logger;
        _eventService = eventService;
    }

    [HttpPost]
    [Route("CreateEvent")]
    public async Task<IActionResult> CreateEvent([FromBody] Event vehicleEvent)
    {
        try
        {
            if (vehicleEvent == null)
            {
                return BadRequest("Invalid event data");
            }

            var result = await _eventService.CreateEventAsync(vehicleEvent);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet]
    [Route("GetEvent/{eventPlateCar}")]
    public async Task<IActionResult> GetEvent(string eventPlateCar)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(eventPlateCar))
            {
                return BadRequest("Invalid event plate car");
            }

            var result = await _eventService.GetEventAsync(eventPlateCar);

            return Ok(result);
        }
        catch (ArgumentNullException)
        {
            return NotFound("Event not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting event");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    [Route("UpdateEvent/{licensePlate}")]
    public async Task<IActionResult> UpdateEvent(string licensePlate)
    {
        try
        {
            var success = await _eventService.UpdateEventAsync(licensePlate);

            if (success)
            {
                return Ok("Event updated successfully");
            }
            else
            {
                return NotFound("Event not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event");
            return StatusCode(500, "Internal server error");
        }
    }
}