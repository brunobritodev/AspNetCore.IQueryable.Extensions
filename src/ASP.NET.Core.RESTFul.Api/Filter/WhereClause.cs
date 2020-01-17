using System.Diagnostics;
using System.Reflection;
using AspNetCore.RESTFul.Extensions.Attributes;

namespace AspNetCore.RESTFul.Extensions.Filter
{
    [DebuggerDisplay("{FieldName}")]
    public class WhereClause
    {
        private bool _customName;

        public WhereOperator Operator { get; set; }
        public bool CaseSensitive { get; set; }
        public bool UseNot { get; set; }
        public PropertyInfo Property { get; set; }
        public string FieldName { get; set; }
        public WhereClause()
        {
            Operator = WhereOperator.Equals;
            UseNot = false;
            CaseSensitive = true;
        }

        public void UpdateAttributeData(RestAttribute data)
        {
            Operator = data.Operator;
            UseNot = data.UseNot;
            CaseSensitive = data.CaseSensitive;
            FieldName = data.HasName;
            if (!string.IsNullOrEmpty(FieldName))
                _customName = true;
        }

        public void UpdateValues(PropertyInfo propertyInfo)
        {
            Property = propertyInfo;
            if (!_customName)
                FieldName = Property.Name;
        }

    }
}