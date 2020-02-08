namespace AspNetCore.IQueryable.Extensions.Pagination
{
    public interface IQueryPaging
    {
        int Limit { get; set; }
        int Offset { get; set; }
    }
}