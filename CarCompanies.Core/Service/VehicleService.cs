using System.ComponentModel;
using System.Text.RegularExpressions;
using CarCompanies.Domain;
using CarCompanies.Domain.Validation;
using CarCompanies.Repository;
using CarCompanies.Repository.Interface;
using CarCompanies.Service.Interface;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CarCompanies.Service;

public class VehicleService : RepositoryBase, IVehicle 
{
    private readonly ILogger<VehicleService> _logger;
    private readonly IRepositoryBase _repositoryBase;
    private readonly string _collectionName = "carcompanie";
    private readonly IEventService _eventService;
    private readonly IBsonFilter<Vehicle> _bsonFilter;
    public VehicleService(IRepositoryBase repositoryBase, ILogger<VehicleService> logger, IEventService eventService,IBsonFilter<Vehicle> bsonFilter) : base(logger)
    {
        _repositoryBase = repositoryBase;
        _logger = logger;
        _eventService = eventService;
        _bsonFilter = bsonFilter;
    }
    public async Task<List<Vehicle>> GetVehicleForModel(string model)
    {
        try
        {
            string plateField = "VehicleModel";
            var filter = _bsonFilter.FilterDefinition<Vehicle>(plateField, model);
            var result = await _repositoryBase.GetDocument<Vehicle>(filter,_collectionName);
            return result;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the vehicle.");

            throw;
        }
    }
    public async Task<bool> UpdateVehicleAsync(string vehicleStatus, string licensePlate)
    {
        try
        {
            var filterField = "LicensePlate";
            var updateField = "VehicleStatus";
            var filter = _bsonFilter.FilterDefinitionUpdate(filterField, licensePlate,updateField,vehicleStatus, out UpdateDefinition<Vehicle> update );
            var result = await _repositoryBase.UpdateDocument(_collectionName, filter, update);

            var updateEventCar = _eventService.UpdateEventAsync(licensePlate);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while update the vehicle.");
            throw;
        }
    }
    public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
    {
        try
        {
            if (vehicle == null) throw new NullReferenceException("You send object null");

            var result = await _repositoryBase.CreateDocumentAsync(_collectionName, vehicle);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while create the vehicle.");
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while delete the vehicle.");
            throw;
        }
    }
    public async Task<Vehicle> GetVehicleForPlate(string plate)
    {
        try
        {
            string plateField = "LicensePlate";
            var filter = _bsonFilter.FilterDefinition<Vehicle>(plateField, plate);
            var result = await _repositoryBase.GetDocument(filter, _collectionName);
            return result.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting vehicle for plate.");
            throw;
        }
    }
    public async Task<List<Vehicle>> GetVehicleForStatus(string status)
    {
        try
        {
            string plateField = "VehicleStatus";
            var filter = _bsonFilter.FilterDefinition<Vehicle>(plateField, status);
            var result = await _repositoryBase.GetDocument<Vehicle>(filter,_collectionName);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting vehicle for status.");
            throw;
        }
    }
    public bool IsValidMercosulLicensePlate(string placa)
    {
        string pattern = @"^[A-Z]{3}\d{1}[A-Z]\d{2}$";

        return Regex.IsMatch(placa, pattern);
    }
}