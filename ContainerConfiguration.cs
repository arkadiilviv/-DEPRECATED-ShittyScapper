using Services;
using Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ContainerConfiguration
{
    public static class DiConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IOtodomScrapper, OtodomScrapper>();
        }
    }
}