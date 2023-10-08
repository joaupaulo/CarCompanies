using CarCompanies.Domain;

namespace CarCompanies.Service.Interface;

public interface IEventService
{
    Task<Event> CreateEventAsync(Event vehicleEvent);
    Task<Event> GetEventAsync(string eventPlateCar);
    Task<bool> UpdateEventAsync(string licensePlate);
}