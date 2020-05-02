using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kinvitech.Services.Interfaces
{
    public interface IScheduledTask
    {
        Task StartTask(CancellationToken stoppingToken);
    }
}
