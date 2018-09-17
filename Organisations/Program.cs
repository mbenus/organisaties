using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Organisations.Repositories;

namespace Organisations
{
    public class Program
    {
        public static void Main(string[] args)
        {
			//var host = BuildWebHost(args);

			// use this to allow command line parameters in the config
			var configuration = new ConfigurationBuilder()
				.AddCommandLine(args)
				.Build();

			var hostUrl = configuration["hosturl"];
			if (string.IsNullOrEmpty(hostUrl))
				hostUrl = "http://0.0.0.0:6000";


			var host = new WebHostBuilder()
				.UseKestrel()
				.UseUrls(hostUrl)   // <!-- this 
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				.UseConfiguration(configuration)
				.Build();

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var context = services.GetRequiredService<OrganisationContext>();
					DbInitializer.Initialize(context);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
				}
			}


			host.Run();
		}

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
