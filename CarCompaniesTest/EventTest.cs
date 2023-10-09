using Xunit;
using Moq;
using CarCompanies.Domain;
using CarCompanies.Domain.Validation;
using CarCompanies.Repository.Interface;
using CarCompanies.Service;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;


namespace EventServiceTest;

public class EventServiceTests
{
    [Fact]
    public async Task CreateEventAsync_ValidEvent_CreatesEvent()
    {
        // Arrange
        var mockRepositoryBase = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<EventService>>();
        var mockBsonFilter = new Mock<IEventBsonFilter>(); 

        var eventService = new EventService(mockRepositoryBase.Object, mockLogger.Object, mockBsonFilter.Object);
        var validEvent = new Event
        {
            LicensePlate = "ABC123",
            ListEventCompanie = new List<EventCar>
            {
                new EventCar
                {
                    DateTime = DateTime.Now,
                    Description = "Test Description"
                }
            }
        };

        mockRepositoryBase.Setup(repo => repo.CreateDocumentAsync("eventcompanie", validEvent))
                         .ReturnsAsync(validEvent);

        // Act
        var createdEvent = await eventService.CreateEventAsync(validEvent);

        // Assert
        Assert.NotNull(createdEvent);
        Assert.Equal(validEvent.LicensePlate, createdEvent.LicensePlate);
    }
    [Fact]
    public async Task UpdateEventAsync_ValidLicensePlate_UpdatesEvent()
    {
        // Arrange
        var mockRepositoryBase = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<EventService>>();
        var mockBsonFilter = new Mock<IEventBsonFilter>(); 

        var eventService = new EventService(mockRepositoryBase.Object, mockLogger.Object, mockBsonFilter.Object);
        var validLicensePlate = "ABC123";

        mockRepositoryBase.Setup(repo => repo.UpdateDocument("eventcompanie", It.IsAny<FilterDefinition<Event>>(), It.IsAny<UpdateDefinition<Event>>()))
                         .ReturnsAsync(true);

        // Act
        var result = await eventService.UpdateEventAsync(validLicensePlate);

        // Assert
        Assert.True(result);
    }
}