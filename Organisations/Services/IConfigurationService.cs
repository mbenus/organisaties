namespace Organisations.Services
{
    /// <summary>
    /// Reads the settings defined within appsettings.json.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Returns a value defined in the appsettings.json configuration.
        /// </summary>
        /// <param name="path">The location in the appsettings.json file to find the requested value</param>
        /// <returns>Value from the appsettings.json file</returns>
        string Get(string path);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="featureName"></param>
		/// <returns></returns>
		bool FeatureEnabled(string featureName);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logEntry"></param>
		/// <returns></returns>
		bool LogEnabled(string logEntry);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		string Logfile { get; }

		/// <summary>
		/// 
		/// </summary>
		bool LogIncomingRequests { get; }

		string LogLevel { get; }
	}
}