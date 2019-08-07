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
        public const int fps = 60;
        public const int millisecondsdelay = 1000 / fps;

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
                int fpsTicNum = 0;
                while (IsRunning)
                {
                    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                    stopWatch.Start();
                    await World.Tic(fpsTicNum);
                    stopWatch.Stop();
                    var d = millisecondsdelay - stopWatch.Elapsed.Milliseconds;
                    if (d<=1) d = 1;
                    await Task.Delay(d);
                    fpsTicNum = (fpsTicNum+1) % 60;
                }
            }).DoNotAwait();

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            IsRunning = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //_timer?.Dispose();
        }
    }
}