using Kinvitech.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kinvitech.Services
{
    public class ScheduledService : HostedService
    {
        private readonly IEnumerable<IScheduledTask> _scheduledTasks;

        public ScheduledService(IEnumerable<IScheduledTask> scheduledTasks)
        {
            _scheduledTasks = scheduledTasks;
        }

        protected override Task Process(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var task in _scheduledTasks)
                {
                    task.StartTask(stoppingToken);
                }
            });
        }
    }
}
