using CarCompanies.Domain;
using CarCompanies.Domain.Enum;
using CarCompanies.Repository.Interface;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CarCompanies.Repository;

public class RepositoryBase : IRepositoryBase
{
    private readonly ILogger<RepositoryBase> _logger;
    protected readonly IMongoDatabase MongoDatabase;
    protected readonly MongoClientSettings MongoClientSettings;
    private string ConnectionString => "mongodb+srv://joaopaulo123:1799jp@cluster0.8rok3.mongodb.net/?authSource=admin";

    public RepositoryBase(ILogger<RepositoryBase> logger,ConnectionStringType connectionStringType = ConnectionStringType.Vehicle)
    {
        MongoClientSettings = MongoClientSettings.FromConnectionString(ConnectionString);
        _logger = logger;
        switch (connectionStringType)
        {
            case ConnectionStringType.Vehicle:
                MongoDatabase = new MongoClient(MongoClientSettings).GetDatabase("UserRegister");
                break;
        }
    }
    
   public async Task<T> CreateDocumentAsync<T>(string collectionName, T document)
    {
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);
            
             await collection.InsertOneAsync(document);

             return document;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while create the vehicle.");

            throw;
        }
    }
   public async Task<List<T>> GetDocument<T>(FilterDefinition<T> filter, string collectionName)
    {
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            var result = await collection.Find(filter).ToListAsync();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the vehicle.");

            throw;
        }
    }
   public async Task<bool> UpdateDocument<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update) 
    {
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            var result = await collection.UpdateOneAsync(filter,update);

            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while update the vehicle.");

            throw;
        }
    }
   
    public async Task<bool> DeleteDocument<T>(string collectionName, string id)
    {
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter.Eq("EventKey", id);

            var result = await collection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while delete the vehicle.");
            throw;
        }
    }
}