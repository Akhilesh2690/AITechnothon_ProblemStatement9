﻿using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace AITechnothon_ProblemStatement9
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API WSVAP (WebSmartView)", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
            });
            services.AddMemoryCache();

            var credentials = new BasicAWSCredentials("AKIA5WAEWOPOYEUDXJGA", "HHun814RVFkNWVe6WXb/Hes9pwoobK4ecvX9PquO");
            var config = new AmazonDynamoDBConfig()
            {
                RegionEndpoint = RegionEndpoint.APSouth1
            };
            var client = new AmazonDynamoDBClient(credentials, config);
            services.AddSingleton<IAmazonDynamoDB>(client);
            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();

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