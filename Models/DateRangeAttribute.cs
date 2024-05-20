using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class DateRangeAttribute : RangeAttribute
{
    public DateRangeAttribute(int minimumYear)
    : base(typeof(DateOnly),
           new DateOnly(minimumYear, 1, 1).ToString("yyyy-MM-dd"),
           DateTime.Now.Date.ToString())

    {
    }
}
