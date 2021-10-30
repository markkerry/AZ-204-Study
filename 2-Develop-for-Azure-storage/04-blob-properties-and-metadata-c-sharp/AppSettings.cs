using Microsoft.Extensions.Configuration;

namespace blobPropertiesAndMetadata
{
    public class AppSettings
    {
        public string SASToken { get; set; }
        public string AccountName { get; set; }
        public string ContainerName { get; set; }

        public static AppSettings LoadAppSettings()
        {
            iConfigurationRoot configRoot = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json",false)
                .Build();
            AppSettings appSettings = configRoot.Get<AppSettings>();
            return appSettings;
        }
    }
}