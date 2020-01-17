namespace AspNetCore.RESTFul.Extensions.Pagination
{
    public interface IRestPagination
    {
        int Limit { get; set; }
        int Offset { get; set; }
    }
}