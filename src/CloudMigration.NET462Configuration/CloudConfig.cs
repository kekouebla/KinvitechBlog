using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Extensions.Configuration.ConfigServer;
using System;
using System.IO;

namespace CloudMigration.NET462Configuration
{
    public class CloudConfig
    {
        /// <summary>
        /// 8 - Sets Spring Cloud Config Server active profiles
        /// </summary>
        private static readonly string _hostingEnvironment = "SPRING_PROFILES_ACTIVE";

        /// <summary>
        /// 1 - Sets the path to the appsettings.json file if running locally
        /// </summary>
        private static readonly string APP_CONTEXT_BASE_DIRECTORY = @"C:\Kinvitech\GitHub\Repos\Kinvitech.Blog\src\CloudMigration.NET462Configuration";

        /// <summary>
        /// 2 - Returns the appsettings.json full path
        /// </summary>
        /// <returns></returns>
        private static string GetContentRoot()
        {
            var basePath = Path.GetFullPath(APP_CONTEXT_BASE_DIRECTORY) ??
               AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(basePath);
        }

        /// <summary>
        /// 3 - Defines a Generic Host builder (required Microsoft.Extensions.Hosting, version 2.2.0 NuGet package)
        /// </summary>
        private static IHost builder;

        /// <summary>
        /// 4 - Gets service of type T from the container (required Microsoft.Extensions.DependencyInjection, version 2.2.0 NuGet package)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {            
            return builder.Services.GetService<T>();
        }

        /// <summary>
        /// Registers configurations
        /// </summary>
        public static void RegisterConfigurations()
        {
            builder = new HostBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Add configuration providers as needed:
                    // 6 - Sets file provider base path
                    config.SetBasePath(GetContentRoot());
                    // 7 - Adds JSON file provider (Microsoft.Extensions.Configuration.Json, version 2.2.0)
                    config.AddJsonFile(@"appsettings.json", optional: true, reloadOnChange: true);
                    // 9 - Gets Spring Cloud Config Server active profiles
                    context.HostingEnvironment.EnvironmentName = String.IsNullOrEmpty(Environment.GetEnvironmentVariable(_hostingEnvironment)) ? "dev" : Environment.GetEnvironmentVariable(_hostingEnvironment);
                    // 10 - Adds Config Server provider with active profile (Steeltoe.Extensions.Configuration.ConfigServerBase, version 2.2.0)
                    config.AddConfigServer(context.HostingEnvironment.EnvironmentName);

                }).Build();
        }
    }
}
