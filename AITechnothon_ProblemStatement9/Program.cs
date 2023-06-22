using AITechnothon_ProblemStatement9;
using AITechnothon_ProblemStatement9.Extensions;

public static class Program
{
    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

    private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(ConfigureWebHostDefaults);

    private static void ConfigureWebHostDefaults(IWebHostBuilder webHostBuilder)
    {
        webHostBuilder.AddAppSettings();
        webHostBuilder.UseStartup<Startup>();
    }
}