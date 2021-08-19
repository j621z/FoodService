using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FoodService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IServiceProvider serviceProvider = new ServiceProvider(); 
            ICacheManager cacheManager = new CacheManager(serviceProvider);
            cacheManager.TrucksInCache().GetAwaiter().GetResult();
            ILoggerFactory loggerFactory = new LoggerFactory();
            ILogger logger = loggerFactory.CreateLogger<Program>();

            CreateHostBuilder(logger, serviceProvider, cacheManager).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(ILogger logger, IServiceProvider serviceProvider, ICacheManager cacheManager) =>
            Host.CreateDefaultBuilder()
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.AddSingleton<ILogger>(logger);
                    serviceCollection.AddSingleton<IServiceProvider>(serviceProvider); 
                    serviceCollection.AddSingleton<ICacheManager>(cacheManager);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
