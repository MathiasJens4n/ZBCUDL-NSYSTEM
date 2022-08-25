using LoanSystemApiMini.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LoanSystemApiMini.Data.IRepositories;
using LoanSystemLibraryMini;
using NLog;
using System.IO;
using LoanSystemApiMini.Log;

namespace LoanSystemApiMini
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DatabaseContext>();
            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddControllers();

            services.AddScoped<IRepo<Equipment>, EquipmentRepo>();
            services.AddScoped<IRepo<Loan>, LoanRepo>();
            services.AddScoped<IRepo<LoanRule>, LoanRuleRepo>();
            services.AddScoped<IRepo<Model>, ModelRepo>();
            services.AddScoped<IBaseRepo<Status>, StatusRepo>();
            services.AddScoped<IRepo<Type>, TypeRepo>();
            services.AddScoped<IRepo<User>, UserRepo>();
            services.AddScoped<IBaseRepo<Department>, DepartmentRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}