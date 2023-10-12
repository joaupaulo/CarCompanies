using CarCompanies.Domain;
using CarCompanies.Domain.Validation;
using MongoDB.Driver;

namespace CarCompanies.Service;

public class BsonFilter<T> : IBsonFilter<T>
{
    public FilterDefinition<T> FilterDefinitionUpdate(string filterDefinitionField, string filterDefinitionParam,
        string filterUpdateDefinitionField, string filterUpdateDefinitionParam, out UpdateDefinition<T> update)
    {
        var filter = Builders<T>.Filter.Eq(filterDefinitionField, filterDefinitionParam);
        update = Builders<T>.Update.Set(filterUpdateDefinitionField, filterUpdateDefinitionParam);
        return filter;
    }

    public FilterDefinition<T> FilterDefinition<T>(string filterDefinitionField, string filterDefinitionParam)
    {
        FilterDefinition<T> filter = Builders<T>.Filter.Eq(filterDefinitionField, filterDefinitionParam);
        return filter;
    }
    
  
    
}

