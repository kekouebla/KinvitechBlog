using Kinvitech.Services.Helpers;
using Kinvitech.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kinvitech.Services.Tasks
{
    public abstract class ScheduledMonitoringTask : IScheduledTask
    {
        protected IMonitoringService _monitoringService;

        protected abstract int ScheduleTimeoutInSecs { get; }

        protected ScheduledMonitoringTask(IMonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
        }

        protected virtual void Process()
        {
            LoggerHelper.Info($"Firing Monitor event by calling { this.GetType().Name }'s Service");
            _monitoringService.Monitor();

        }

        private void TaskProcess(CancellationToken stoppingToken)
        {
            try
            {
                LoggerHelper.Info($"{this.GetType().Name } Process Starting");

                // Run Process
                Task.Factory.StartNew(() => Process())
                    .ContinueWith((prevTask) =>
                    {
                        if (!stoppingToken.IsCancellationRequested)
                        {

                            var scheduleDelayDisplay = ScheduleTimeoutInSecs > 60
                                        ? $"{ ScheduleTimeoutInSecs / 60 } mins and { ScheduleTimeoutInSecs % 60 } secs"
                                        : $"{ScheduleTimeoutInSecs} secs";

                            LoggerHelper.Info($"{this.GetType().Name } process will run again after {scheduleDelayDisplay}.");
                            Task.Delay(TimeSpan.FromSeconds(ScheduleTimeoutInSecs), stoppingToken).Wait();
                        }
                        else
                        {
                            LoggerHelper.Warn($"{ this.GetType().Name } was stopped");
                        }
                    }).Wait();
            }
            catch (Exception ex)
            {
                // Disable the ending of the task, always catch exception
                LoggerHelper.Error(ex.Message);
            }
        }

        public Task StartTask(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    TaskProcess(stoppingToken);
                }
            });
        }
    }
}
