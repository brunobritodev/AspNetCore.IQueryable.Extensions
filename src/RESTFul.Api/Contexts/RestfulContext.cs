using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RESTFul.Api.Models;

namespace RESTFul.Api.Contexts
{
    public class RestfulContext : DbContext
    {

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });


        public RestfulContext(DbContextOptions<RestfulContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .EnableSensitiveDataLogging();
        }
    }
}
