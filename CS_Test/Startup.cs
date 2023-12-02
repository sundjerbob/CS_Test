using CS_Test.Repositories;
using CS_Test.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CS_Test
{
    public class Startup
    {
        // Configure services for dependency injection
        public void ConfigureServices(IServiceCollection services)
        {
            // Provide ShiftCheckInRegistry(repository api) object for injection inside EmployeeService class
            services.AddSingleton<IShiftCheckInRegistry, ShiftCheckInRegistry>(
                provider =>
                new ShiftCheckInRegistry(
                    "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ=="
                    )
                );

            // Provide EmployeeService object for injection inside EmployeeController class
            services.AddSingleton<Services.IEmployeeService, EmployeeService>(
                provider => new EmployeeService(
                    provider.GetRequiredService<IShiftCheckInRegistry>()
                )
            );

            
            // Provide ChartService object for injection inside ChartController class
            services.AddSingleton<IChartService, ChartService>(provider => new ChartService());


            // Add controllers and views
            services.AddControllersWithViews();
        }


        // Configure the request processing pipeline
        public void Configure(IApplicationBuilder app)
        {
            if (app.ApplicationServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
            {
                // Enable developer exception page in development environment
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // Configure routing for controllers
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "chart",
                    pattern: "chart",
                    defaults: new { controller = "Chart", action = "GetPieChart" }
                );
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Employee}/{action=Index}/{id?}"
                );


            });
        }
    }
}
