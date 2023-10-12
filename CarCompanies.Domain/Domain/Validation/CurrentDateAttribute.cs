using System.ComponentModel.DataAnnotations;

namespace CarCompanies.Domain.Validation;

public class CurrentDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime date = (DateTime)value;
        return date >= DateTime.UtcNow.Date;
    }
}