using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Pagination;

namespace RestFulTests.Models
{
    public class PagingMax : IQueryPaging
    {
        public int? Offset { get; set; }
        [QueryOperator(Max = 5)]
        public int? Limit { get; set; }
    }
    public class SinglePaging : IQueryPaging
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }
}