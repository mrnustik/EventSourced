﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventSourced.Helpers
{
    public abstract class HostedServiceBase : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        protected HostedServiceBase(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();
            await StartAsync(serviceScope, cancellationToken);
        }


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();
            await StopAsync(serviceScope, cancellationToken);
        }

        protected virtual Task StartAsync(IServiceScope serviceScope, CancellationToken cancellationToken) => Task.CompletedTask;
        protected virtual Task StopAsync(IServiceScope serviceScope, CancellationToken cancellationToken) => Task.CompletedTask;

    }
}