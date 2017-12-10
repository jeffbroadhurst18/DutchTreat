using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DutchTreat
{
	public class Startup
    {
		private readonly IConfiguration _config;
		private readonly IHostingEnvironment _env;

		public Startup(IConfiguration config, IHostingEnvironment env) {
			_config = config;
			_env = env;
		}


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddIdentity<StoreUser, IdentityRole>(
				cfg => { cfg.User.RequireUniqueEmail = true; }).
				AddEntityFrameworkStores<DutchContext>();

			services.AddAuthentication().AddCookie().AddJwtBearer(cfg =>
			 {
				 cfg.TokenValidationParameters = new TokenValidationParameters()
				 {
					 ValidIssuer = _config["Tokens:Issuer"],
					 ValidAudience = _config["Tokens:Audience"],
					 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
				 };
			 }
			);

			services.AddDbContext<DutchContext>(cfg => {
				cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
			});

			services.AddAutoMapper();

			services.AddTransient<IMailService, NullMailService>();//inject the service
			services.AddTransient<DutchSeeder>();

			services.AddScoped<IDutchRepository, DutchRepository>(); // interface then actual implementation

			//Dependency injection mandatory;
			services.AddMvc(opt => {
				if (_env.IsProduction())
				{
					opt.Filters.Add(new RequireHttpsAttribute());//adds https on whole site
				}
			}).AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
			//=> bit stopd infinite loop when reading order orderitem order etc
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

			app.UseAuthentication(); //Needs to go before MVC as MVC needs identity.

			app.UseMvc(cfg =>
				cfg.MapRoute("Default", "{controller}/{action}/{id?}",
				new { controller = "App", Action = "Index" })); //Default page
			
			if (env.IsDevelopment())
			{
				using (var scope = app.ApplicationServices.CreateScope())
				{
					var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
					seeder.Seed().Wait(); ; //seeder is async .Wait waits for it to finish without making 
					// the whole method async.
				}
			}
        }
    }
}
