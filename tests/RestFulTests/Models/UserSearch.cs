using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using System;
using System.Collections.Generic;

namespace RestFulTests.Models
{
    public class UserSearch : IQueryPaging, IQuerySort
    {
        [QueryOperator(Operator = WhereOperator.Contains, UseOr = true)]
        public string Username { get; set; }

        [QueryOperator(Operator = WhereOperator.GreaterThan)]
        public DateTime? Birthday { get; set; }

        [QueryOperator(Operator = WhereOperator.Contains, HasName = "Firstname")]
        public string Name { get; set; }

        [QueryOperator(Operator = WhereOperator.Contains, HasName = "SocialNumber.Identification")]
        public string Ssn { get; set; }

        [QueryOperator(Operator = WhereOperator.GreaterThanOrEqualTo, HasName = "Age")]
        public int? OlderThan { get; set; }

        [QueryOperator(Operator = WhereOperator.LessThanOrEqualTo, HasName = "Age")]
        public int? YoungerThan { get; set; }

        [QueryOperator(Operator = WhereOperator.Contains)]
        public IEnumerable<int> Id { get; set; }

        [QueryOperator(Operator = WhereOperator.Equals)]
        public Guid? CustomId { get; set; }


        public int? Offset { get; set; }
        public int? Limit { get; set; } = 10;
        public string Sort { get; set; }
    }

    public class WrongOperator : ICustomQueryable
    {
        [QueryOperator(Operator = WhereOperator.Equals)]
        public IEnumerable<int> Id { get; set; }
    }
}
