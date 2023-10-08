using CarCompanies.Domain;
using CarCompanies.Domain.Enum;
using CarCompanies.Repository.Interface;
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
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);
            
             await collection.InsertOneAsync(document);

             return document;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}, have find {count}");
        }
    }
    

    public async Task<List<T>> GetAllDocument<T>(string collectionName)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            var filter = new BsonDocument();

            var result = await collection.FindAsync(filter);

            var documents = await result.ToListAsync();

            count = documents.Count;

            return documents;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}, have find {count}");
        }
    }

    public async Task<List<T>> GetVehicleForModel<T>(string collectionName, string model)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter.Eq("VehicleModel", model);

            var result = await collection.Find(filter).ToListAsync();

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}");
        }

    }
   
    public async Task<List<T>> GetVehicleForPlate<T>(string collectionName, string plate)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter.Eq("LicensePlate", plate);

            var result = await collection.Find(filter).ToListAsync();

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}");
        }
    }
    
    public async Task<List<T>> GetVehicleForStatus<T>(string collectionName, string status)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter.Eq("VehicleStatus", status);

            var result = await collection.Find(filter).ToListAsync();

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}");
        }
    }
    
    
    public async Task<T> GetDocument<T>(string collectionName, string id)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter.Eq("LicensePlate", id);

            var result = await collection.Find(filter).FirstOrDefaultAsync();

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}");
        }

    }

    public async Task<T> GetVehiclePlate<T>(string collectionName, Guid Key)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter.Eq("EventKey", Key);

            var result = await collection.Find(filter).FirstOrDefaultAsync();

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}");
        }

    }
    
    public async Task<bool> UpdateDocument<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update) 
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            var result = await collection.UpdateOneAsync(filter,update);

            return result.ModifiedCount > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}");
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            _logger.LogInformation($"Delete done in collection {collectionName}");
        }
    }
}