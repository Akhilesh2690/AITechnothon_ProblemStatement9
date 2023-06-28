using AITechnothon_ProblemStatement9.Options;

namespace AITechnothon_ProblemStatement9.Extensions
{
    public static class AddAppsettings
    {
        private const string CopyFromAppDetails = "AppDetails";
        private const string CopyFromAWSDetails = "AwsDetails";

        public static IWebHostBuilder AddAppSettings(this IWebHostBuilder builder)
        {
            _ = builder
                .ConfigureAppConfiguration((hostingconContext, configBuilder) =>
                {
                    var config = configBuilder.Build();
                    if (!hostingconContext.HostingEnvironment.EnvironmentName.Equals("Development"))
                    {
                        configBuilder.AddJsonFile($"appsettings.{hostingconContext.HostingEnvironment.EnvironmentName}.json");
                        config = configBuilder.Build();
                    }
                    configBuilder.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<AppDetailsOptions>(context.Configuration.GetSection(CopyFromAppDetails));
                    services.Configure<AWSDetailsOptions>(context.Configuration.GetSection(CopyFromAWSDetails));
                });
            return builder;
        }
    }
}