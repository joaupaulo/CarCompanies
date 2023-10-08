using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using CarCompanies.Domain;
using CarCompanies.Domain.Enum;
using CarCompanies.Domain.Excpt;
using CarCompanies.Service;
using CarCompanies.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CarCompanies.Controllers;

[Route("api/vehicles")]
[ApiController]
public class VehicleController : ControllerBase
{
    private readonly ILogger<VehicleController> _logger;
    private readonly IVehicle _vehicleService;

    public VehicleController(IVehicle vehicleService, ILogger<VehicleController> logger)
    {
        _vehicleService = vehicleService;
        _logger = logger;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableVehicles()
    {
        try
        {
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting available vehicles.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVehicle(string id)
    {
        try
        {
            var vehicle = await _vehicleService.GetVehicleAsync(id);
            if (vehicle == null) return NotFound();
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the vehicle.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpGet("GetVehicle/Model/{model}")]
    public async Task<IActionResult> GetVehicleForModel(string model)
    {
        try
        {
            if (!System.Enum.GetNames(typeof(VehicleModel)).Contains(model))
            {
                throw new BusinessException("Choice correct model car");
            }
            
            var vehicle = await _vehicleService.GetVehicleForModel(model);
            if (vehicle == null) return NotFound();
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the vehicle.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("GetVehicle/Plate/{plate}")]
    public async Task<IActionResult> GetVehicleForPlate(string plate)
    {
        try
        {
            if (!IsValidMercosulLicensePlate(plate))
            {
                throw new BusinessException("Plate without mercosul");
            }

            var vehicle = await _vehicleService.GetVehicleForPlate(plate);
            if (vehicle == null) return NotFound();
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the vehicle.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpGet("GetVehicle/Status/{status}")]
    public async Task<IActionResult> GetVehicleForStatus(string status)
    {
        try
        {
            if (!System.Enum.GetNames(typeof(VehicleStatus)).Contains(status))
            {
                throw new BusinessException("Choice correct model status");
            }
            var vehicle = await _vehicleService.GetVehicleForStatus(status);
            if (vehicle == null) return NotFound();
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the vehicle.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateVehicle([FromBody] Vehicle vehicle)
    {
        try
        {
            if (vehicle == null) return BadRequest("Invalid input data.");
            await _vehicleService.CreateVehicleAsync(vehicle);
            return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, vehicle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the vehicle.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPut("{vehicleStatus}")]
    public async Task<IActionResult> UpdateVehicle([Required] string plate, [Required]string vehicleStatus)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plate) && string.IsNullOrWhiteSpace(vehicleStatus))
            { 
              throw new ArgumentNullException("Object null");
            }

            var existingVehicle = await _vehicleService.GetVehicleForPlate(plate);
            
            if (existingVehicle == null) return NotFound();

            await _vehicleService.UpdateVehicleAsync(vehicleStatus,plate);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the vehicle.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicle(string plate)
    {
        try
        {
            var existingVehicle = await _vehicleService.GetVehicleForPlate(plate);
           
            if (existingVehicle == null) return NotFound();
            var vehicle =  existingVehicle.First();
           
            if (vehicle.VehicleStatus == VehicleStatus.Rented.ToString())
            {
                throw new BusinessException("Choice correct model status");

            }

            if ((DateTime.Now - vehicle.RegistrationDate).TotalDays > 15)
            {
                throw new BusinessException("Error date");

            }
            
            await _vehicleService.DeleteVehicleAsync(plate);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the vehicle.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    static bool IsValidMercosulLicensePlate(string placa)
    {
        string pattern = @"^[A-Z]{3}\d{1}[A-Z]\d{2}$";

        return Regex.IsMatch(placa, pattern);
    }
}