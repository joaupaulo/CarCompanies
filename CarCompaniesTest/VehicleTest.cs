using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarCompanies.Domain;
using CarCompanies.Repository.Interface;
using CarCompanies.Service;
using CarCompanies.Service.Interface;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

public class VehicleServiceTests
{
    [Fact]
    public async Task CreateVehicleAsync_ValidVehicle_CreatesVehicleAndRegistersEvent()
    {
        // Arrange
        var mockRepositoryBase = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();

        var vehicleService = new VehicleService(mockRepositoryBase.Object, mockLogger.Object, mockEventService.Object);
        var validVehicle = new Vehicle
        {
            LicensePlate = "ABC123",
            VehicleModel = "Model123",
            RegistrationDate = DateTime.Now,
            VehicleStatus = "Active"
        };

        mockRepositoryBase.Setup(repo => repo.CreateDocumentAsync("carcompanie", validVehicle))
            .ReturnsAsync(validVehicle);

        mockEventService.Setup(eventService => eventService.CreateEventAsync(It.IsAny<Event>()))
            .ReturnsAsync(new Event());

        // Act
        var createdVehicle = await vehicleService.CreateVehicleAsync(validVehicle);

        // Assert
        Assert.NotNull(createdVehicle);
        Assert.Equal(validVehicle.LicensePlate, createdVehicle.LicensePlate);
        mockEventService.Verify(eventService => eventService.CreateEventAsync(It.IsAny<Event>()), Times.Once);
    }

    [Fact]
    public async Task UpdateVehicleAsync_ValidStatusAndLicensePlate_UpdatesVehicleAndRegistersEvent()
    {
        // Arrange
        var mockRepositoryBase = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();

        var vehicleService = new VehicleService(mockRepositoryBase.Object, mockLogger.Object, mockEventService.Object);
        var validLicensePlate = "ABC123";
        var validStatus = "UpdatedStatus";

        mockRepositoryBase.Setup(repo => repo.UpdateDocument("carcompanie", It.IsAny<FilterDefinition<Vehicle>>(),
                It.IsAny<UpdateDefinition<Vehicle>>()))
            .ReturnsAsync(true);

        mockEventService.Setup(eventService => eventService.UpdateEventAsync(validLicensePlate))
            .ReturnsAsync(true);

        // Act
        var result = await vehicleService.UpdateVehicleAsync(validStatus, validLicensePlate);

        // Assert
        Assert.True(result);
        mockEventService.Verify(eventService => eventService.UpdateEventAsync(validLicensePlate), Times.Once);
    }

    [Fact]
    public async Task DeleteVehicleAsync_ValidId_DeletesVehicle()
    {
        // Arrange
        var mockRepositoryBase = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();

        var vehicleService = new VehicleService(mockRepositoryBase.Object, mockLogger.Object, Mock.Of<IEventService>());
        var validId = "123";

        mockRepositoryBase.Setup(repo => repo.DeleteDocument<Vehicle>("carcompanie", validId))
            .ReturnsAsync(true);

        // Act
        var result = await vehicleService.DeleteVehicleAsync(validId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetVehicleForPlate_ValidPlate_ReturnsListOfVehicles()
    {
        // Arrange
        var mockRepositoryBase = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();

        var vehicleService = new VehicleService(mockRepositoryBase.Object, mockLogger.Object, Mock.Of<IEventService>());
        var validPlate = "ABC123";
        var expectedVehicles = new List<Vehicle>
        {
            new Vehicle { LicensePlate = validPlate },
            new Vehicle { LicensePlate = validPlate }
        };

        mockRepositoryBase.Setup(repo => repo.GetVehicleForPlate<Vehicle>("carcompanie", validPlate))
            .ReturnsAsync(expectedVehicles);

        // Act
        var result = await vehicleService.GetVehicleForPlate(validPlate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedVehicles, result);
    }

    [Fact]
    public async Task CreateVehicleAsync_NullVehicle_ThrowsException()
    {
        // Arrange
        var mockRepositoryBase = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();

        var vehicleService = new VehicleService(mockRepositoryBase.Object, mockLogger.Object, mockEventService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => vehicleService.CreateVehicleAsync(null));
    }

    [Fact]
    public async Task UpdateVehicleAsync_InvalidLicensePlate_ReturnsFalse()
    {
        // Arrange
        var mockRepositoryBase = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();

        var vehicleService = new VehicleService(mockRepositoryBase.Object, mockLogger.Object, mockEventService.Object);

        var invalidLicensePlate = "InvalidPlate";
        var validStatus = "UpdatedStatus";

        // Act
        var result = await vehicleService.UpdateVehicleAsync(validStatus, invalidLicensePlate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteVehicleAsync_InvalidId_ReturnsFalse()
    {
        // Arrange
        var mockRepositoryBase = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();

        var vehicleService = new VehicleService(mockRepositoryBase.Object, mockLogger.Object, mockEventService.Object);

        var invalidId = "InvalidId";

        // Act
        var result = await vehicleService.DeleteVehicleAsync(invalidId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetVehicleForPlate_NoMatchingPlate_ReturnsEmptyList()
    {
        // Arrange
        var mockRepositoryBase = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();

        var vehicleService = new VehicleService(mockRepositoryBase.Object, mockLogger.Object, mockEventService.Object);

        var nonExistentPlate = "NonExistentPlate";

        mockRepositoryBase.Setup(repo => repo.GetVehicleForPlate<Vehicle>("carcompanie", nonExistentPlate))
            .ReturnsAsync(new List<Vehicle>());

        // Act
        var result = await vehicleService.GetVehicleForPlate(nonExistentPlate);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetVehicleForStatus_NoMatchingStatus_ReturnsEmptyList()
    {
        // Arrange
        var mockRepositoryBase = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();

        var vehicleService = new VehicleService(mockRepositoryBase.Object, mockLogger.Object, mockEventService.Object);

        var nonExistentStatus = "NonExistentStatus";

        mockRepositoryBase.Setup(repo => repo.GetVehicleForStatus<Vehicle>("carcompanie", nonExistentStatus))
            .ReturnsAsync(new List<Vehicle>());

        // Act
        var result = await vehicleService.GetVehicleForStatus(nonExistentStatus);

        // Assert
        
        Assert.Empty(result);
    }
}