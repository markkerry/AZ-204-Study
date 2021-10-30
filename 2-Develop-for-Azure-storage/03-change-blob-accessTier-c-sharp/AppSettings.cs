using Microsoft.Extensions.Configuration;

namespace changeBlobAccessTier
{
    public class AppSettings
    {
        public string SASConnectionString { get; set; }
        public string AccountName { get; set; }
        public string ContainerName { get; set; }

        public static AppSettings LoadAppSettings()
        {
            IConfigurationRoot configRoot = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json",false)
                .Build();
            AppSettings appSettings = configRoot.Get<AppSettings>();
            return appSettings;
        }
    }
}