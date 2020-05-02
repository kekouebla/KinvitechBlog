using AutoMapper;
using Kinvitech.Services;
using Kinvitech.Services.Interfaces;
using Kinvitech.Services.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.Threading;

namespace Kinvitech.FileManager
{
    class Program
    {
        private static IHostedService _scheduledService;
        private static CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Main method of File Manager
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            // Set up configuration sources
            var builder = new ConfigurationBuilder()
                .AddCloudFoundry();

            IConfiguration configuration = builder.Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .ConfigureCloudFoundryOptions(configuration)
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IHostedService, ScheduledService>()
                .AddSingleton<IScheduledTask, FileMonitorCheckInboundFolderTask>()
                .AddSingleton<IScheduledTask, FileMonitorArchiveTask>()
                .AddSingleton<IScheduledTask, FileMonitorSfgTask>()
                .AddAutoMapper(typeof(Program))
                .BuildServiceProvider();

            _cancellationTokenSource = new CancellationTokenSource();
            _scheduledService = serviceProvider.GetService<IHostedService>();

            Console.CancelKeyPress += StopService;

            _scheduledService.StartAsync(_cancellationTokenSource.Token);
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _cancellationTokenSource.Token.WaitHandle.WaitOne(-1);
            }

        }

        /// <summary>
        /// Method to stop the service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void StopService(object sender, ConsoleCancelEventArgs args)
        {
            _scheduledService.StopAsync(_cancellationTokenSource.Token);
            _cancellationTokenSource.Cancel();
        }
    }
}
