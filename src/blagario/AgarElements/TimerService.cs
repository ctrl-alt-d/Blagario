using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace blagario.elements
{
    public static class TaskExtensions
    {
        public static void DoNotAwait(this Task task) { }
    }

    internal class TimedHostedService : IHostedService, IDisposable
    {
        //private Timer _timer;
        private World World;

        public bool IsRunning {get; private set;}
        const int fps = 60;
        const int millisecondsdelay = 1000 / fps;

        public TimedHostedService(Universe universe)
        {
            World = universe.World;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IsRunning = true;
            //_timer = new Timer(DoWork, null, TimeSpan.Zero, 
            //    TimeSpan.FromMilliseconds(20));

            Task.Run( async () => {
                while (IsRunning)
                {
                    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                    stopWatch.Start();
                    await World.Tic();
                    stopWatch.Stop();
                    var d = millisecondsdelay - stopWatch.Elapsed.Milliseconds;
                    if (d<=1) d = 1;
                    await Task.Delay(d);
                }
            }).DoNotAwait();

            await Task.CompletedTask;
        }


        private async void DoWork(object state)
        {
            await World.Tic();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            IsRunning = false;
            //_timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //_timer?.Dispose();
        }
    }
}