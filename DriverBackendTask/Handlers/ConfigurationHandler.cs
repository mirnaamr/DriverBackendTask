namespace DriverBackendTask.Handlers
{
    public static class ConfigurationHandler
    {
        public static IConfiguration AppSetting { get; }
        static ConfigurationHandler()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }
    }
}
