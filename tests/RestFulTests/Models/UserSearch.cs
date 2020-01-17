using AspNetCore.RESTFul.Extensions.Attributes;
using AspNetCore.RESTFul.Extensions.Filter;
using AspNetCore.RESTFul.Extensions.Pagination;
using AspNetCore.RESTFul.Extensions.Sort;
using System;

namespace RestFulTests.Models
{
    public class UserSearch : IRestSort, IRestPagination
    {
        public string Username { get; set; }

        [Rest(Operator = WhereOperator.GreaterThan)]
        public DateTime? Birthday { get; set; }

        [Rest(Operator = WhereOperator.Contains, HasName = "Firstname")]
        public string Name { get; set; }

        [Rest(Operator = WhereOperator.Contains, HasName = "SocialNumber.Identification")]
        public string Ssn { get; set; }

        public int Offset { get; set; }
        public int Limit { get; set; } = 10;
        public string Sort { get; set; }
    }
}
