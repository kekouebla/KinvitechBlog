﻿using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kinvitech.Services
{
    public abstract class HostedService : IHostedService, IDisposable
    {
        private Task _executingTask;
        private CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        public void Dispose()
        {
            _stoppingCts.Dispose();
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
            }
        }

        protected virtual async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Process(stoppingToken).ConfigureAwait(false);
        }

        protected abstract Task Process(CancellationToken stoppingToken);
    }
}
