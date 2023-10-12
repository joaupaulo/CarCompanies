using System.ComponentModel.DataAnnotations;
using CarCompanies.Domain.Enum;

namespace CarCompanies.Domain.Validation;

public class ValidVehicleModel : ValidationAttribute
{
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string model)
        {
            if (System.Enum.GetNames(typeof(VehicleModel)).Contains(model))
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult("Modelo de veículo inválido.");
    }
}