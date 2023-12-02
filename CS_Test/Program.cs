
using CS_Test;
using Microsoft.AspNetCore.Hosting;

namespace CS_TestProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Build and run the web host
            CreateHostBuilder(args).Build().Run();
        }

        // Configure and create the host builder
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Use the Startup class to configure the web host
                    webBuilder.UseStartup<Startup>();
                });
    }
}
