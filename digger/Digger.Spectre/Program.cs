using System;
using System.Threading;
using System.Threading.Tasks;

namespace Digger.Spectre
{
    internal static class Program
    {
        private static async Task Main()
        {
            var con = new ConDigger(null);
            var game = new DiggerClassic.Digger(con);
            game.Init();
            game.Start();

            Console.Title = "Digger Remastered";
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, _) =>
            {
                cts.Cancel();
                Environment.Exit(0);
            };

            con._digger = game;
            await ConRefresher.ShowAll(cts, con);
        }
    }
}