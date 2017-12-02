using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using DutchTreat.Services;
using DutchTreat.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DutchTreat
{
    public class Startup
    {
		private readonly IConfiguration _config;

		public Startup(IConfiguration config) {
			_config = config;
		}


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<DutchContext>(cfg => {
				cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
			});
			services.AddTransient<IMailService, NullMailService>();//inject the service
			services.AddTransient<DutchSeeder>();

			services.AddScoped<IDutchRepository, DutchRepository>(); // interface then actual implementation

			//Dependency injection mandatory;
			services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/error");
			}
			//app.UseDefaultFiles();//means you don't have to specify index.html - commented as not required for mvc
			app.UseStaticFiles();//Can serve static files
			app.UseMvc(cfg =>
				cfg.MapRoute("Default", "{controller}/{action}/{id?}",
				new { controller = "App", Action = "Index" })); //Default page
			
			if (env.IsDevelopment())
			{
				using (var scope = app.ApplicationServices.CreateScope())
				{
					var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
					seeder.Seed();
				}
			}
        }
    }
}
