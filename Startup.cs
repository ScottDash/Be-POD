using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProofOfDeliveryAPI.Helpers;
using ProofOfDeliveryAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Hosting;

namespace ProofOfDeliveryAPI
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });

            services.AddControllers();

            // Mapping the configuration
            var connectionSection = Configuration.GetSection("ConnectionStrings");
            services.Configure<ConnectionStrings>(connectionSection);

            // Configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // Configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVehicleChecklistService, VehicleChecklistService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IOrderService, OrderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAllHeaders");
            app.UseRouting();
    
            app.UseAuthentication();
            app.UseAuthorization();
         



            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
