using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace Digger.Spectre
{
    internal abstract class AppletCompat
    {
        private readonly Dictionary<int, long> _keyList;
        private readonly Timer _keyTimer;
        private const int KeyDelay = 120;

        protected AppletCompat()
        {
            _keyList = new Dictionary<int, long>();
            _keyTimer = new Timer(OnKeyTick, null, KeyDelay, KeyDelay);
        }

        public string GetSubmitParameter() => null;

        public int GetSpeedParameter() => 66;

        public void RequestFocus()
        {
            /* NOP */
        }

        private static long CurrentTicks => Environment.TickCount64;

        private void OnKeyTick(object state)
        {
            foreach (var (key, value) in _keyList
                         .OrderBy(k => k.Value)
                         .Take(1)
                         .ToArray())
            {
                if (CurrentTicks <= value + KeyDelay)
                    continue;
                _keyList.Remove(key);
                KeyUp(key);
            }
        }

        internal async Task ReadKey(CancellationTokenSource cts)
        {
            while (!cts.IsCancellationRequested)
            {
                var con = AnsiConsole.Console.Input;
                var key = await con.ReadKeyAsync(intercept: true, cts.Token);
                if (key == null)
                    continue;
                var num = ConvertToLegacy(key.Value);
                if (num >= 0)
                {
                    KeyDown(num);
                    _keyList[num] = CurrentTicks;
                }
            }
        }

        protected abstract bool KeyUp(int key);

        protected abstract bool KeyDown(int key);

        private static int ConvertToLegacy(ConsoleKeyInfo netCode)
        {
            switch (netCode.Key)
            {
                case ConsoleKey.LeftArrow:
                    return 1006;
                case ConsoleKey.RightArrow:
                    return 1007;
                case ConsoleKey.UpArrow:
                    return 1004;
                case ConsoleKey.DownArrow:
                    return 1005;
                case ConsoleKey.F1:
                    return 1008;
                case ConsoleKey.F10:
                    return 1021;
                case ConsoleKey.Add:
                    return 1031;
                case ConsoleKey.Subtract:
                    return 1032;
                default:
                    var ascii = (int)netCode.KeyChar;
                    return ascii;
            }
        }
    }
}