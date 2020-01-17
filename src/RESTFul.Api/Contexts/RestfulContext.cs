using Microsoft.EntityFrameworkCore;
using RESTFul.Api.Models;

namespace RESTFul.Api.Contexts
{
    public class RestfulContext : DbContext
    {
        public RestfulContext(DbContextOptions<RestfulContext> options)
            : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }
    }
}
