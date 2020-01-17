using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RESTFul.Api.Configuration
{
    public static class AutomapperConfiguration
    {
        public static void ConfigureAutomapper(this IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = new Mapper(mappingConfig);
            services.TryAddSingleton(mapper);
        }
    }
}