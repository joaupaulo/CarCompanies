using System.ComponentModel;
using System.Text.RegularExpressions;
using CarCompanies.Domain;
using CarCompanies.Domain.Validation;
using CarCompanies.Repository;
using CarCompanies.Repository.Interface;
using CarCompanies.Service.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CarCompanies.Service;

public class VehicleService : RepositoryBase, IVehicle 
{
    private readonly ILogger<VehicleService> _logger;
    private readonly IRepositoryBase _repositoryBase;
    private readonly string _collectionName = "carcompanie";
    private readonly IEventService _eventService;

    public VehicleService(IRepositoryBase repositoryBase, ILogger<VehicleService> logger, IEventService eventService) : base(logger)
    {
        _repositoryBase = repositoryBase;
        _logger = logger;
        _eventService = eventService;
    }
    

    public async Task<List<Vehicle>> GetVehicleForModel(string model)
    {
        try
        {
            var result = await _repositoryBase.GetVehicleForModel<Vehicle>(_collectionName, model);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public async Task<bool> UpdateVehicleAsync(string vehicleStatus, string licensePlate)
    {
        try
        {
            var filter = Filter(vehicleStatus, licensePlate, out var update);
            
            var result = await _repositoryBase.UpdateDocument(_collectionName, filter, update);

            var updateEventCar = _eventService.UpdateEventAsync(licensePlate);
            
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
    {
        try
        {
            if (vehicle == null) throw new NullReferenceException("You send object null");

            var result = await _repositoryBase.CreateDocumentAsync(_collectionName, vehicle);

            if (result != null)
            {
                RegisterEventCreate(vehicle);
            }
            
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<bool> DeleteVehicleAsync(string Id)
    {
        try
        {
            var result = await _repositoryBase.DeleteDocument<Vehicle>(_collectionName, Id);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Vehicle>> GetVehicleForPlate(string plate)
    {
        try
        {
            var result = await _repositoryBase.GetVehicleForPlate<Vehicle>(_collectionName, plate);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    

    public async Task<List<Vehicle>> GetVehicleForStatus(string status)
    {
        try
        {
            var result = await _repositoryBase.GetVehicleForStatus<Vehicle>(_collectionName,status);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private static FilterDefinition<Vehicle> Filter(string vehicleStatus, string licensePlate, out UpdateDefinition<Vehicle> update)
    {
        var filter = FilterDefinition(vehicleStatus, licensePlate, out update);
        return filter;
    }
    
    private void RegisterEventCreate(Vehicle vehicle)
    {
        Event events = new Event
        {
            LicensePlate = vehicle.LicensePlate,
            ListEventCompanie = new List<EventCar>
            {
                new EventCar
                {
                    DateTime = DateTime.Now,
                    Description = $"Cadastrando novo veiculo de placa {vehicle.LicensePlate}"
                }
            }
        };
        _eventService.CreateEventAsync(events);
    }
    
    private static FilterDefinition<Vehicle> FilterDefinition(string vehicleStatus, string licensePlate, out UpdateDefinition<Vehicle> update)
    {
        var filter = Builders<Vehicle>.Filter.Eq("LicensePlate", licensePlate);
        update = Builders<Vehicle>.Update.Set("VehicleStatus", vehicleStatus);
        return filter;
    }
    
    public bool IsValidMercosulLicensePlate(string placa)
    {
        string pattern = @"^[A-Z]{3}\d{1}[A-Z]\d{2}$";

        return Regex.IsMatch(placa, pattern);
    }
    
}