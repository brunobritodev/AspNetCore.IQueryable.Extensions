using System;
using AspNetCore.RESTFul.Extensions.Filter;

namespace AspNetCore.RESTFul.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RestAttribute : Attribute
    {
        public WhereOperator Operator { get; set; } = WhereOperator.Equals;
        public bool UseNot { get; set; } = false;
        public bool CaseSensitive { get; set; } = true;
        public string HasName { get; set; }
    }
}
