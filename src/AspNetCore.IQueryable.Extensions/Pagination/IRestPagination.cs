namespace AspNetCore.IQueryable.Extensions.Pagination
{
    public interface IRestPagination
    {
        int Limit { get; set; }
        int Offset { get; set; }
    }
}