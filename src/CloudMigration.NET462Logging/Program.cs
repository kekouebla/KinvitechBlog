using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloudMigration.NET462Logging
{
    class Program
    {
        private static ILogger<Program> _logger;

        static void Main(string[] args)
        {
            CloudConfig.RegisterConfigurations();
            _logger = CloudConfig.GetService<ILogger<Program>>();

            LogTrace();
            LogDebug();
            LogInformation();
            LogWarning();
            LogError();
            LogCritical();

            _logger.LogInformation("Press any key to continue...");
            Console.ReadLine();
        }

        static void LogTrace()
        {
            var sf = new StackFrame(true);
            var sb = new StringBuilder();
            sb.AppendLine("Logs Trace from Microsoft Extensions Logging ...");
            sb.AppendLine("This is information valuable for debugging.  Do not enable in production environment as it may contain sensitive information.");
            sb.AppendLine("In production, send Trace through Information level to a volume data store.");
            sb.AppendLine("Action from: " + MethodBase.GetCurrentMethod().Name);
            sb.AppendLine("Action caused by command: " + MethodBase.GetCurrentMethod().Name);
            sb.AppendLine("Action happened at: " + DateTime.Now);
            sb.AppendLine("Action line number: " + sf.GetFileLineNumber());
            _logger.LogError(sb.ToString());
        }

        static void LogDebug()
        {
            var sf = new StackFrame(true);
            var sb = new StringBuilder();
            sb.AppendLine("Logs Debug from Microsoft Extensions Logging ...");
            sb.AppendLine("This is information useful in debugging.  Enable in development environment.  Enable in production environment only if needed for troubleshooting.");
            sb.AppendLine("Action from: " + MethodBase.GetCurrentMethod().Name);
            sb.AppendLine("Action caused by command: " + MethodBase.GetCurrentMethod().Name);
            sb.AppendLine("Action happened at: " + DateTime.Now);
            sb.AppendLine("Action line number: " + sf.GetFileLineNumber());
            _logger.LogError(sb.ToString());
        }

        static void LogInformation()
        {
            var sf = new StackFrame(true);
            var sb = new StringBuilder();
            sb.AppendLine("Logs Information from Microsoft Extensions Logging ...");
            sb.AppendLine("This is information for long term value to track general flow of the application.");
            sb.AppendLine("Action from: " + MethodBase.GetCurrentMethod().Name);
            sb.AppendLine("Action caused by command: " + MethodBase.GetCurrentMethod().Name);
            sb.AppendLine("Action happened at: " + DateTime.Now);
            sb.AppendLine("Action line number: " + sf.GetFileLineNumber());
            _logger.LogInformation(sb.ToString());
        }

        static void LogWarning()
        {
            var sf = new StackFrame(true);
            var sb = new StringBuilder();
            sb.AppendLine("Logs Warning from Microsoft Extensions Logging ...");
            sb.AppendLine("This is information for unexpected events that do not stop the flow of the application and may include common errors that can be handled.");
            sb.AppendLine("In development, send Warning through Critical level to a console.");
            sb.AppendLine("In production, send Warning through Critical level to a volume data store.");
            _logger.LogError(sb.ToString());
        }        

        static void LogError()
        {
            var sf = new StackFrame(true);
            var sb = new StringBuilder();
            sb.AppendLine("Logs Error from Microsoft Extensions Logging ...");
            sb.AppendLine("This is information for errors and exceptions failure that can be handled and is not an application-wide failure.");
            sb.AppendLine("Action from: " + MethodBase.GetCurrentMethod().Name);
            sb.AppendLine("Action caused by command: " + MethodBase.GetCurrentMethod().Name);
            sb.AppendLine("Action happened at: " + DateTime.Now);
            sb.AppendLine("Action line number: " + sf.GetFileLineNumber());
            _logger.LogError(sb.ToString());
        }

        static void LogCritical()
        {
            var sf = new StackFrame(true);
            var sb = new StringBuilder();
            sb.AppendLine("Logs Critical from Microsoft Extensions Logging ...");
            sb.AppendLine("This is information for failures that require immediate attention.");
            sb.AppendLine("Action from: " + MethodBase.GetCurrentMethod().Name);
            sb.AppendLine("Action caused by command: " + MethodBase.GetCurrentMethod().Name);
            sb.AppendLine("Action happened at: " + DateTime.Now);
            sb.AppendLine("Action line number: " + sf.GetFileLineNumber());
            _logger.LogError(sb.ToString());
        }
    }
}
