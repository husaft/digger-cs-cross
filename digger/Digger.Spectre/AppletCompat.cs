using System;
using System.Threading;

namespace Digger.Spectre
{
    internal abstract class AppletCompat
    {
        public string GetSubmitParameter() => null;

        public int GetSpeedParameter() => 66;

        public void RequestFocus()
        {
            // NOP
        }

        internal void ReadKey(CancellationTokenSource cts)
        {
            while (!cts.IsCancellationRequested)
            {
                var key = Console.ReadKey(intercept: true);
                var num = ConvertToLegacy(key);
                if (num >= 0)
                    KeyDown(num);
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