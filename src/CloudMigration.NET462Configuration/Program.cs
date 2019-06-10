using Microsoft.Extensions.Configuration;
using System;
using System.Text;

namespace CloudMigration.NET462Configuration
{
    class Program
    {
        private static IConfiguration _configuration;

        static void Main(string[] args)
        {
            CloudConfig.RegisterConfigurations();
            _configuration = CloudConfig.GetService<IConfiguration>();

            GetAppSettingsConfigurations();
            GetConfigServerConfigurations();

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        static void GetAppSettingsConfigurations()
        {
            var sb = new StringBuilder();
            sb.AppendLine("appsettings configurations directly from Microsoft Extensions Configuration ...");
            sb.AppendLine(_configuration["vcap:services:mssql:0:credentials:uri"]);
            sb.AppendLine(_configuration["vcap:services:user-provided:0:credentials:uri"]);
            sb.AppendLine(_configuration["HashiCorpTerraformUrl"]); //Note that the key with IConfiguration is case insensitive!

            Console.WriteLine(sb.ToString());
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
        }

        static void GetConfigServerConfigurations()
        {
            var sb = new StringBuilder();
            sb.AppendLine("hypress/spring-cloud-config-server configurations from https://github.com/kekouebla/KinvitechConfig ...");
            sb.AppendLine(_configuration["configurationdbconnection"]);
            sb.AppendLine(_configuration["hashicorpurl"]);
            sb.AppendLine(_configuration["hashicorpterraformurl"]);

            Console.WriteLine(sb.ToString());
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
        }
    }
}
