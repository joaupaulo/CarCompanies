using System.Linq.Expressions;
using CarCompanies.Domain;

namespace CarCompanies.Service.Interface;

public interface IVehicle
{
    Task<Vehicle> GetVehicleAsync(string id);
    Task<List<Vehicle>> GetVehicleForModel(string model);
    Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
    Task<bool> UpdateVehicleAsync(string vehicleStatus, string id);
    Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
    Task<bool> DeleteVehicleAsync(string id);
    Task<List<Vehicle>> GetVehicleForPlate(string plate);
    Task<List<Vehicle>> GetVehicleForStatus(string status);
}