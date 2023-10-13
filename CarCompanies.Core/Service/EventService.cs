using CarCompanies.Domain;
using CarCompanies.Domain.Validation;
using CarCompanies.Repository;
using CarCompanies.Repository.Interface;
using CarCompanies.Service.Interface;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CarCompanies.Service;

public class EventService : RepositoryBase, IEventService
{
    private readonly ILogger<EventService> _logger;
    private readonly IRepositoryBase _repositoryBase;
    private readonly IEventBsonFilter _eventBsonFilter;
    private readonly string _collectionName = "eventcompanie";
    
    public EventService(IRepositoryBase repositoryBase, ILogger<EventService> logger,IEventBsonFilter eventBsonFilter) : base(logger)
    {
        _repositoryBase = repositoryBase;
        _eventBsonFilter = eventBsonFilter;
        _logger = logger;
    }
    
    public async Task<Event> CreateEventAsync(Event vehicleEvent)
    {
        try
        {
            if (vehicleEvent == null) throw new NullReferenceException("error data");

            var result = await _repositoryBase.CreateDocumentAsync(_collectionName, vehicleEvent);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    } 
    
    public async Task<Event> GetEventAsync(string eventPlateCar)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(eventPlateCar))
            {
                throw new ArgumentNullException();
            }

            var result = await _repositoryBase.GetDocument<Event>(_collectionName, eventPlateCar);
            
            if (result == null)
            {
                throw new ArgumentNullException();
            }
            
            return result.First();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<bool>  UpdateEventAsync(string licensePlate)
    {
        try
        {
            var newEventCar = new EventCar
            {
                DateTime = DateTime.Now,
                Description = $"Veiculo de placa {licensePlate} atualizado"
            };
            
            var filter =  _eventBsonFilter.FilterDefinition(licensePlate, newEventCar, out var update); 
            
            var result = await _repositoryBase.UpdateDocument(_collectionName, filter, update);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}