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

    [HttpGet("GetVehicle/Model/{modelVehicle}")]
    public async Task<IActionResult> GetVehicleForModel(string modelVehicle)
    {
        try
        {
            if (!System.Enum.GetNames(typeof(VehicleModel)).Contains(modelVehicle))
            {
                throw new BusinessException("Select another model vehicle, this dont exist!");
            }
            
            var vehicle = await _vehicleService.GetVehicleForModel(modelVehicle);
            if (vehicle == null) return NotFound();
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the vehicle.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("GetVehicle/Plate/{plateVehicule}")]
    public async Task<IActionResult> GetVehicleForPlate(string plateVehicule)
    {
        try
        {
            if (!_vehicleService.IsValidMercosulLicensePlate(plateVehicule))
            {
                throw new BusinessException("Write a place with model Mercosul");
            }

            var vehicle = await _vehicleService.GetVehicleForPlate(plateVehicule);
            if (vehicle == null) return NotFound();
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the vehicle.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpGet("GetVehicle/Status/{statusVehicule}")]
    public async Task<IActionResult> GetVehicleForStatus(string statusVehicule)
    {
        try
        {
            if (!System.Enum.GetNames(typeof(VehicleStatus)).Contains(statusVehicule))
            {
                throw new BusinessException("Select another status of Vehicle");
            }
            var vehicle = await _vehicleService.GetVehicleForStatus(statusVehicule);
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
            if (vehicle == null) return BadRequest("You are send request null");
            
            var result = await _vehicleService.CreateVehicleAsync(vehicle);
            
            return Ok(result);
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
              throw new ArgumentNullException("Fill in all fields");
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

    [HttpDelete("{plateVehicule}")]
    public async Task<IActionResult> DeleteVehicle(string plateVehicule)
    {
        try
        {
            var existingVehicle = await _vehicleService.GetVehicleForPlate(plateVehicule);
           
            if (existingVehicle == null) return NotFound();
            var vehicle = existingVehicle;
           
            if (vehicle.VehicleStatus == VehicleStatus.Rented.ToString())
            {
                throw new BusinessException("You are selecting a rental vehicle");

            }

            if ((DateTime.Now - vehicle.RegistrationDate).TotalDays > 15)
            {
                throw new BusinessException("You are selecting a vehicle that was created more than 15 days ago");

            }
            
            await _vehicleService.DeleteVehicleAsync(plateVehicule);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the vehicle.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}