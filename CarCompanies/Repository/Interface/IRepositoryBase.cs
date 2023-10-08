using CarCompanies.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CarCompanies.Repository.Interface;

public interface IRepositoryBase
{
    Task<T> CreateDocumentAsync<T>(string collectionName, T Document);
    Task<List<T>> GetVehicleForStatus<T>(string collectionName, string status);
    Task<List<T>> GetVehicleForModel<T>(string collectionName, string model);
    Task<T> GetDocument<T>(string collectionName, string id );
    Task<bool> DeleteDocument<T>(string collectionName, string id);
    Task<bool> UpdateDocument<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update);
    Task<List<T>> GetVehicleForPlate<T>(string collectionName, string plate);
}