using System.Threading.Tasks;
using System.Threading;
using Digger.API;
using Nito.AsyncEx;
using Spectre.Console;

namespace Digger.Spectre
{
    internal sealed class ConRefresher : IRefresher
    {
        private static readonly AsyncManualResetEvent Manual = new();

        public ConRefresher(IColorModel model)
        {
            Model = model;
        }

        public IColorModel Model { get; }

        public void NewPixels()
        {
            Manual.Set();
        }

        public void NewPixels(int x, int y, int width, int height)
        {
            Manual.Set();
        }

        public static async Task ShowAll(CancellationTokenSource cts, ConDigger con)
        {
            var keys = Task.Run(() => con.ReadKey(cts));
            var live = AnsiConsole.Live(Text.Empty)
                .StartAsync(async ctx =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        ctx.UpdateTarget(con.OnDraw());

                        await Manual.WaitAsync(cts.Token);
                        Manual.Reset();
                    }
                });
            await Task.WhenAll(live, keys);
        }
    }
}