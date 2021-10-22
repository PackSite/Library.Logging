namespace WebAppExample.Services
{
    using System;

    public class Randomizer : IRandomizer
    {
        private readonly Random _generator = new();

        public int Next(int min, int max)
        {
            return _generator.Next(min, max);
        }
    }
}
