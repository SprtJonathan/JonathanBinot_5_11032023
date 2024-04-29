using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class YearRangeAttribute : RangeAttribute
{
    public YearRangeAttribute(int minimum) : base(minimum, DateTime.Now.Year)
    {
    }
}