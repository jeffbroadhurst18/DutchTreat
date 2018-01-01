using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog.Web;

namespace DutchTreat
{
	public class Program
    {
        public static void Main(string[] args)
        {
			var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
			try
			{
				logger.Debug("init main");
				BuildWebHost(args).Run();
			}
			catch (Exception e)
			{
				//NLog: catch setup errors
				logger.Error(e, "Stopped program because of exception");
				throw;
			}
		}

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration(SetupConfiguration)
				.UseStartup<Startup>()
			.UseNLog()
				.Build();

		private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
		{
			//remove the default configuration options
			builder.Sources.Clear();

			builder.AddJsonFile("config.json", false, true);
				
		}
	}
}
