using System.Linq.Expressions;

namespace AspNetCore.IQueryable.Extensions.Filter
{
    internal class ExpressionParser
    {
        public WhereClause Criteria { get; set; }
        public Expression FieldToFilter { get; set; }
        public Expression FilterBy { get; set; }
    }
}