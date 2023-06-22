namespace AITechnothon_ProblemStatement9.Extensions
{
    public static class AddAppsettings
    {
        private const string CopyFromFromAWSSection = "adhoc:aws";

        public static IWebHostBuilder AddAppSettings(this IWebHostBuilder builder)
        {
            _ = builder
                .ConfigureAppConfiguration((hostingconContext, configBuilder) =>
                {
                    var config = configBuilder.Build();
                    if (config["apm"] == null && hostingconContext.HostingEnvironment.EnvironmentName.Equals("Development"))
                    {
                        configBuilder.AddJsonFile("appsettings.json");
                        config = configBuilder.Build();
                    }

                    var configDict = config
                    .GetSection($"{CopyFromFromAWSSection}")
                    .AsEnumerable(true)
                    .ToDictionary(item => item.Key, item => item.Value);

                    configBuilder.AddInMemoryCollection(configDict);

                    configBuilder.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    //services.Configure<AWSOptions>(context.Configuration.GetSection(CopyFromFromAWSSection));
                });
            return builder;
        }
    }
}