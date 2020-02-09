namespace AspNetCore.IQueryable.Extensions.Sort
{
    public interface IQuerySort : ICustomQueryable
    {
        string Sort { get; set; }
    }
}