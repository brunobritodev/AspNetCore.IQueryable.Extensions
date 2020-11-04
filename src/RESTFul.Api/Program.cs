using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RESTFul.Api.Contexts;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RESTFul.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            Task.WaitAll(DbMigrationHelpers.EnsureSeedData(serviceScope: host.Services.CreateScope()));

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public static class DbMigrationHelpers
    {

        public static async Task EnsureSeedData(IServiceScope serviceScope)
        {
            Debug.Assert(serviceScope != null, nameof(serviceScope) + " != null");

            var serviceProvider = serviceScope?.ServiceProvider;
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var appContext = scope.ServiceProvider.GetRequiredService<RestfulContext>();

            await appContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
        }
    }
}
