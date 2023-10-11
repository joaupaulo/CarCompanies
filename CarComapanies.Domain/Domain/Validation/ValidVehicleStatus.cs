using System.ComponentModel.DataAnnotations;
using CarCompanies.Domain.Enum;

namespace CarCompanies.Domain.Validation;

public class ValidVehicleStatus : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string status)
        {
            if (System.Enum.GetNames(typeof(VehicleStatus)).Contains(status))
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult("Status de veículo inválido.");
    }
}