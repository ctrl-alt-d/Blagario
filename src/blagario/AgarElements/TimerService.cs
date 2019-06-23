using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace blagario.elements
{

    internal class TimedHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private World World;

        public TimedHostedService(Universe universe)
        {
            World = universe.World;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromMilliseconds(17));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            await World.Tic();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}