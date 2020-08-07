using AspNetCore.IQueryable.Extensions.Filter;
using System;

namespace AspNetCore.IQueryable.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryOperatorAttribute : Attribute
    {
        public WhereOperator Operator { get; set; } = WhereOperator.Equals;
        public bool UseNot { get; set; } = false;
        public bool CaseSensitive { get; set; } = true;
        public string HasName { get; set; }
        public int Max { get; set; }
        public bool UseOr { get; set; } = false;
    }
}
