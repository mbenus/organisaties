using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Organisations.Services
{
    /// <inheritdoc />
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRoot _config;

        public ConfigurationService()
        {
            var configurationService = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _config = configurationService.Build();
        }

        /// <inheritdoc />
        public string Get(string path)
        {
            if (_config.GetSection(path).Exists())
            {
                return _config[path];
            }
            
			Log.Warning($"Missing configuration item at '{path}'");
			return null;
        }

		/// <summary>
		/// Features are expected to reside in {"Features" : {} } appsettings.config
		/// </summary>
		/// <param name="feature"></param>
		/// <returns></returns>
		public bool FeatureEnabled(string featureName)
		{
			var featureSection = _config.GetSection("Features");
			if (featureSection == null)
				return false;

			return KeyValueIsTrue(featureSection, $"{featureName}Enabled");
		}

		/// <summary>
		/// Check if the log should be enabled for a particular entry i.e:
		///  - toFile
		///  - toConsole
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		public bool LogEnabled(string entry)
		{
			string[] validEntries = { "ToFile", "ToConsole" };
			if (!validEntries.Contains(entry))
				return false;

			var loggingSection = _config.GetSection("Logging");
			if (loggingSection == null)
				return false;

			var entrySection = loggingSection.GetSection(entry);
			if (entrySection == null)
				return false;

			return KeyValueIsTrue(entrySection, "Enabled");
		}

		/// <summary>
		/// Log the requests that are send to the api
		/// </summary>
		public bool LogIncomingRequests
		{
			get
			{
				var loggingSection = _config.GetSection("Logging");
				if (loggingSection == null)
					return false;
				
				return KeyValueIsTrue(loggingSection, "LogIncomingRequests");
			}
		}

		public string LogLevel
		{
			get
			{
				var defaultLogLevel = "info";
				var loggingSection = _config.GetSection("Logging");
				if (loggingSection == null)
					return defaultLogLevel;

				var feature = loggingSection.GetChildren().FirstOrDefault(f => f.Key == "LogLevel");
				if (feature == null)
					return defaultLogLevel;

				if (string.IsNullOrEmpty(feature.Value))
					return defaultLogLevel;

				string[] validLogLevels = { "info", "warning", "error", "debug", "critical" };
				if (!validLogLevels.Contains(feature.Value.ToLower()))
					return defaultLogLevel;

				return feature.Value;
			}
		}

		public string Logfile
		{
			get
			{
				var defaultLog = "organisations.log";
				var loggingSection = _config.GetSection("Logging");
				if (loggingSection == null)
					return defaultLog;

				var section = loggingSection.GetSection("ToFile");
				if (section == null)
					return defaultLog;

				var feature = section.GetChildren().FirstOrDefault(f => f.Key == "File");
				if (feature == null)
					return defaultLog;

				return string.IsNullOrEmpty(feature.Value) ? defaultLog : feature.Value;
			}
		}

		/// <summary>
		/// Helper to check if a key value is true
		/// </summary>
		/// <param name="section"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		private bool KeyValueIsTrue(IConfigurationSection section, string key)
		{
			var feature = section.GetChildren().FirstOrDefault(f => f.Key == key);
			if (feature == null)
				return false;

			if (feature.Value == null)
				return false;

			return feature.Value.ToLower() == "true" ? true : false;
		}
    }
}
