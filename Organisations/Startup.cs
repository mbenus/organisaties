using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema;
using NSwag.AspNetCore;
using Organisations.Middleware;
using Organisations.Repositories;
using Organisations.Services;
using Serilog;

namespace Organisations
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			var configurationServiceDatabase = new ConfigurationBuilder();
			configurationServiceDatabase.SetBasePath(Directory.GetCurrentDirectory());
			configurationServiceDatabase.AddJsonFile("databasesettings.json");
			var configurationDatabase = configurationServiceDatabase.Build();

			var configurationService = new ConfigurationService();

			services.AddDbContext<OrganisationContext>(options =>
				options.UseNpgsql(configurationDatabase["ConnectionStrings:User"]));

			services.AddScoped<IOrganisationContext, OrganisationContext>();
			services.AddScoped<IOrganisationService, OrganisationService>();
			services.AddSingleton<IConfigurationService, ConfigurationService>();


			services.AddMvc()
				// support also application/xml besides the default: json
				 .AddXmlSerializerFormatters();
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfigurationService conf)
		{
			// Inititialize the logging behaviour
			InitLogger(app, conf);


			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();

			// Enable features set in the appsettings.json
			if (conf.FeatureEnabled("Swagger"))
			{
				// Register the Swagger generator
				app.UseSwagger(typeof(Startup).Assembly, settings =>
				{
					settings.PostProcess = document =>
					{
						document.Info.Version = "v1";
						document.Info.Title = "Organisaties API";
						document.Info.Description = "Opvragen van organisatie data";
						document.Info.TermsOfService = "Commercial";
						document.Info.Contact = new NSwag.SwaggerContact
						{
							Name = "Roxit",
							Email = "mark.benus@roxit.nl",
							Url = "https://roxit.nl/"
						};
						//document.Info.License = new NSwag.SwaggerLicense
						//{
						//	Name = "Use under LICX",
						//	Url = "https://example.com/license"
						//};
					};
				});
				
				// Enable Swagger UI
				if (conf.FeatureEnabled("SwaggerUi"))
					app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
					{
						settings.GeneratorSettings.DefaultPropertyNameHandling =
							PropertyNameHandling.CamelCase;
					});
			}

			if (conf.FeatureEnabled("CorrelationId"))
				app.UseCorrelationId();
		}

		/// <summary>
		/// Inititialize logging behaviour
		/// </summary>
		/// <param name="app"></param>
		/// <param name="conf"></param>
		private void InitLogger(IApplicationBuilder app, IConfigurationService conf)
		{
			var logger = new LoggerConfiguration();

			switch (conf.LogLevel)
			{
				case "info":
					logger.MinimumLevel.Information();
					break;
				case "warning":
					logger.MinimumLevel.Warning();
					break;
				case "error":
					logger.MinimumLevel.Error();
					break;
				case "critical":
					logger.MinimumLevel.Fatal();
					break;
				case "debug":
					logger.MinimumLevel.Debug();
					break;
				default:
					logger.MinimumLevel.Verbose();
					break;
			}

			if (conf.LogEnabled("ToConsole"))
				logger.WriteTo.Console();

			if (conf.LogEnabled("ToFile"))
				logger.WriteTo.File(conf.Logfile, rollingInterval: RollingInterval.Day);

			Log.Logger = logger.CreateLogger();
			Log.Information("Starting organisation api");

			if (conf.LogIncomingRequests)
			{
				app.Use(async (context, next) =>
				{
					// TODO: check if the code is not also hit by outgoing requests
					const string prefix = "Incoming request: ";

					// log request url
					Log.Debug($"{prefix}{context.Request.Path.Value}{context.Request.QueryString.Value}");

					// log headers
					foreach (var h in context.Request.Headers)
						Log.Debug($"{prefix}Header={h.Key}:{h.Value}");
					
					await next();
				});
			}
		}
    }
}
