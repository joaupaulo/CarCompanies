using CarCompanies.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CarCompanies.Repository.Interface;

public interface IRepositoryBase
{
    Task<T> CreateDocumentAsync<T>(string collectionName, T Document);
    Task<bool> DeleteDocument<T>(string collectionName, string id);
    Task<bool> UpdateDocument<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update);
    Task<List<T>> GetDocument<T>(FilterDefinition<T> filter, string collectionName);
}