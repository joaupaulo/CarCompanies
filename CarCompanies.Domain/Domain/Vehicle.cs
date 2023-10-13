using System.ComponentModel.DataAnnotations;
using CarCompanies.Domain.Enum;
using CarCompanies.Domain.Validation;
using MongoDB.Bson;

namespace CarCompanies.Domain;

public class Vehicle
{
    [Required]
    public ObjectId Id { get; set; }
    [Required]
    [RegularExpression(@"^[A-Z]{3}\d{1}[A-Z]{1}\d{2}$", ErrorMessage = "Placa must be in the format MERCOSUL.")]
    public string LicensePlate { get; set; }
    [Required]
    [ValidVehicleModel(ErrorMessage = "Modelo de veículo inválido.")]
    public string VehicleModel{ get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime RegistrationDate { get; set; }
    [Required]
    [ValidVehicleStatus(ErrorMessage = "Status do veículo invalido.")]
    public string VehicleStatus{ get; set; }
    
}