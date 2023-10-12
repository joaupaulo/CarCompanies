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
    public async Task GetVehicleForModel_ValidModel_ReturnsListOfVehicles()
    {
        // Arrange
        var mockRepository = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();
        var mockBsonFilter = new Mock<IBsonFilter<Vehicle>>();

        var service = new VehicleService(mockRepository.Object, mockLogger.Object, mockEventService.Object, mockBsonFilter.Object);

        var model = "TestModel";
        var vehicles = new List<Vehicle> { new Vehicle { VehicleModel = model } };
        var filter = Builders<Vehicle>.Filter.Eq("VehicleModel", model);
        
        mockBsonFilter.Setup(f => f.FilterDefinition<Vehicle>("VehicleModel", model)).Returns(filter);
        
        mockRepository.Setup(r => r.GetDocument<Vehicle>(filter, "carcompanie")).ReturnsAsync(vehicles);

        // Act
        var result = await service.GetVehicleForModel(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vehicles, result);
    }

    [Fact]
    public async Task UpdateVehicleAsync_ValidData_ReturnsTrue()
    {
        // Arrange
        var mockRepository = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();
        var mockBsonFilter = new Mock<IBsonFilter<Vehicle>>();

        var service = new VehicleService(mockRepository.Object, mockLogger.Object, mockEventService.Object, mockBsonFilter.Object);

        var vehicleStatus = "NewStatus";
        var licensePlate = "ABC123";
        var filterField = "LicensePlate";
        var updateField = "VehicleStatus";

        mockRepository
            .Setup(r => r.UpdateDocument("carcompanie", It.IsAny<FilterDefinition<Vehicle>>(), It.IsAny<UpdateDefinition<Vehicle>>()))
            .ReturnsAsync(true);

        mockEventService
            .Setup(e => e.UpdateEventAsync(licensePlate))
            .ReturnsAsync(true);

        // Act
        var result = await service.UpdateVehicleAsync(vehicleStatus, licensePlate);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateVehicleAsync_ValidVehicle_ReturnsCreatedVehicle()
    {
        // Arrange
        var mockRepository = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();
        var mockBsonFilter = new Mock<IBsonFilter<Vehicle>>();

        var service = new VehicleService(mockRepository.Object, mockLogger.Object, mockEventService.Object, mockBsonFilter.Object);

        var vehicle = new Vehicle
        {
            LicensePlate = "ABC123",
            VehicleModel = "TestModel",
            RegistrationDate = DateTime.Now,
            VehicleStatus = "Active"
        };

        mockRepository
            .Setup(r => r.CreateDocumentAsync("carcompanie", vehicle))
            .ReturnsAsync(vehicle);

        // Act
        var result = await service.CreateVehicleAsync(vehicle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vehicle, result);
    }

    [Fact]
    public async Task DeleteVehicleAsync_ValidId_ReturnsTrue()
    {
        // Arrange
        var mockRepository = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();
        var mockBsonFilter = new Mock<IBsonFilter<Vehicle>>();

        var service = new VehicleService(mockRepository.Object, mockLogger.Object, mockEventService.Object, mockBsonFilter.Object);

        var vehicleId = "123";

        mockRepository
            .Setup(r => r.DeleteDocument<Vehicle>("carcompanie", vehicleId))
            .ReturnsAsync(true);

        // Act
        var result = await service.DeleteVehicleAsync(vehicleId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetVehicleForPlate_ValidPlate_ReturnsVehicle()
    {
        // Arrange
        var mockRepository = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();
        var mockBsonFilter = new Mock<IBsonFilter<Vehicle>>();

        var service = new VehicleService(mockRepository.Object, mockLogger.Object, mockEventService.Object, mockBsonFilter.Object);

        var plate = "ABC123";

        var filter = Builders<Vehicle>.Filter.Eq("LicensePlate", plate);
        var vehicle = new Vehicle { LicensePlate = plate };
        
        mockBsonFilter.Setup(f => f.FilterDefinition<Vehicle>("VehicleModel", plate)).Returns(filter);
        mockRepository
            .Setup(r => r.GetDocument(filter, "carcompanie"))
            .ReturnsAsync(new List<Vehicle> { vehicle });

        // Act
        var result = await service.GetVehicleForPlate(plate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vehicle, result);
    }

    [Fact]
    public async Task GetVehicleForStatus_ValidStatus_ReturnsListOfVehicles()
    {
        // Arrange
        var mockRepository = new Mock<IRepositoryBase>();
        var mockLogger = new Mock<ILogger<VehicleService>>();
        var mockEventService = new Mock<IEventService>();
        var mockBsonFilter = new Mock<IBsonFilter<Vehicle>>();

        var service = new VehicleService(mockRepository.Object, mockLogger.Object, mockEventService.Object, mockBsonFilter.Object);

        var status = "Active";
        var filter = Builders<Vehicle>.Filter.Eq("VehicleStatus", status);
        var vehicles = new List<Vehicle> { new Vehicle { VehicleStatus = status } };

        mockRepository
            .Setup(r => r.GetDocument<Vehicle>("carcompanie", status))
            .ReturnsAsync(vehicles);

        // Act
        var result = await service.GetVehicleForStatus(status);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vehicles, result);
    }
}