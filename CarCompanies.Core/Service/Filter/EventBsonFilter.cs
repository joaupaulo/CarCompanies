using CarCompanies.Domain;
using CarCompanies.Domain.Validation;
using MongoDB.Driver;

namespace CarCompanies.Service;

public class EventBsonFilter : IEventBsonFilter
{
    public FilterDefinition<Event> FilterDefinition(string licensePlate, EventCar newEventCar, out UpdateDefinition<Event> update)
    {
        var filter = Builders<Event>.Filter.Eq("LicensePlate", licensePlate);
        update = Builders<Event>.Update.Push(x => x.ListEventCompanie, newEventCar);
        return filter;
    }
}