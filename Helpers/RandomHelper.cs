using System;

namespace BombVacuum.Helpers
{
    public sealed class RandomHelper
    {
        private static readonly Lazy<RandomHelper> lazy =
            new Lazy<RandomHelper>(() => new RandomHelper());

        public static RandomHelper Instance { get { return lazy.Value; } }

        private RandomHelper()
        {
            _random = new Random();
        }

        private readonly Random _random;
        public Random Random
        {
            get
            {
                return new Random(_random.Next());
            }
        }
    }
}