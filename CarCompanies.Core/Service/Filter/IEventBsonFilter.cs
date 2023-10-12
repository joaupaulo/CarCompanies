using CarCompanies.Domain;
using CarCompanies.Domain.Validation;
using MongoDB.Driver;

namespace CarCompanies.Service;

public interface IEventBsonFilter
{
    FilterDefinition<Event> FilterDefinition(string licensePlate, EventCar newEventCar,
        out UpdateDefinition<Event> update);
}