using System.ComponentModel.DataAnnotations;

public class GreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public GreaterThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        ErrorMessage = ErrorMessageString;
        var currentValue = (DateTime)value;

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
            throw new ArgumentException("Property with this name not found");

        var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

        if (currentValue < comparisonValue)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}

public class YearGreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public YearGreaterThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        ErrorMessage = ErrorMessageString;
        var currentValue = (DateTime)value;

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
            throw new ArgumentException("Property with this name not found");

        var comparisonValue = (int)property.GetValue(validationContext.ObjectInstance);

        if (currentValue.Year < comparisonValue || currentValue.Year > DateTime.Now.Year)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}

