using CarCompanies.Domain;
using MongoDB.Driver;

namespace CarCompanies.Service;

public interface IBsonFilter<T>
{
    FilterDefinition<T> FilterDefinitionUpdate(string filterDefinitionField, string filterDefinitionParam,
        string filterUpdateDefinitionField, string filterUpdateDefinitionParam, out UpdateDefinition<T> update);

    FilterDefinition<T> FilterDefinition<T>(string filterDefinitionField, string filterDefinitionParam);
}