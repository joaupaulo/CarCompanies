using System.ComponentModel.DataAnnotations;
using CarCompanies.Domain.Validation;

namespace CarCompanies.Domain;

public class Event
{  
   [Required]
   [RegularExpression(@"^[A-Z]{3}\d{1}[A-Z]{1}\d{2}$", ErrorMessage = "Plate must be in the format MERCOSUL.")]
   public string LicensePlate { get; set; }
   public List<EventCar> ListEventCompanie { get; set; }
}