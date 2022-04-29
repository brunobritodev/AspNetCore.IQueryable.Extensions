namespace AspNetCore.IQueryable.Extensions.Filter
{
    public enum WhereOperator
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
        Contains,
        StartsWith,
        LessThanOrEqualWhenNullable, 
        GreaterThanOrEqualWhenNullable,
        EqualsWhenNullable
    }
}