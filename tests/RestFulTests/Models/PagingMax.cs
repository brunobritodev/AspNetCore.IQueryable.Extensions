using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Pagination;

namespace RestFulTests.Models
{
    public class PagingMax : IRestPagination
    {
        public int Offset { get; set; }
        [Rest(Max = 5)]
        public int Limit { get; set; } = 10;
    }
}