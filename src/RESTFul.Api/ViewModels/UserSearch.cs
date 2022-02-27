using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using System;

namespace RESTFul.Api.ViewModels
{
    public class UserSearch : IQuerySort, IQueryPaging
    {
        [QueryOperator(Operator = WhereOperator.Contains, UseOr = true)]
        public string Username { get; set; }

        [QueryOperator(Operator = WhereOperator.GreaterThan)]
        public DateTime? Birthday { get; set; }

        [QueryOperator(Operator = WhereOperator.Contains, HasName = "Firstname")]
        public string Name { get; set; }

        [QueryOperator(Operator = WhereOperator.Equals)]
        public Guid CustomId { get; set; }

        public int? Offset { get; set; }
        public int? Limit { get; set; } = 10;
        public string Sort { get; set; }
    }

}
