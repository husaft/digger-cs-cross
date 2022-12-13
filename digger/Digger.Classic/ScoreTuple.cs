using System;

namespace DiggerClassic
{
    public sealed class ScoreTuple : Tuple<string, int>
    {
        public ScoreTuple(string key, int value) : base(key, value)
        {
        }
    }
}