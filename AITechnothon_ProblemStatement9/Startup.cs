using AITechnothon_ProblemStatement9.Domain;
using AITechnothon_ProblemStatement9.Repository;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace AITechnothon_ProblemStatement9
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        private readonly string _policyName = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            services.AddCors(opt =>
            {
                opt.AddPolicy(name: _policyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API WSVAP (WebSmartView)", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
            });

            services.AddSingleton<IDynamoDBClientConfig, DynamoDBClientConfig>();
            services.AddSingleton<IAmazonDynamoDB>(s => s.GetService<IDynamoDBClientConfig>().GetDynamoDBClient());
            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
            services.AddScoped<IS3ClientRepository, S3ClientRepository>();
            services.AddScoped<IDynamoClientRepository, DynamoClientRepository>();
            services.AddScoped<IFileScanner, FileScanner>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(_policyName);

            app.UseAuthorization();
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        //Logging logic goes here
                        await context.Response.WriteAsync(context.Response.StatusCode + " Internal Server Error Test.");
                    }
                });
            });
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}