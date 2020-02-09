using AspNetCore.IQueryable.Extensions.Attributes;
using System.Diagnostics;
using System.Reflection;

namespace AspNetCore.IQueryable.Extensions.Filter
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
        public bool UseOr { get; set; }

        public WhereClause()
        {
            Operator = WhereOperator.Equals;
            UseNot = false;
            CaseSensitive = true;
        }

        public void UpdateAttributeData(QueryOperatorAttribute data)
        {
            Operator = data.Operator;
            UseNot = data.UseNot;
            CaseSensitive = data.CaseSensitive;
            FieldName = data.HasName;
            UseOr = data.UseOr;
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