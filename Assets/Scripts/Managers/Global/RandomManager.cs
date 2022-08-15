using System.Collections;
using System.Collections.Generic;
using System;

namespace CustomRandom
{
    public static class RandomManager
    {
        private static Random random;
        public static int Step { get; private set; }

        public static void Initiate(string seed)
        {
            int _seed = seed.GetHashCode();
            random = new Random(_seed);
        }

        public static int GetRandomInteger()
        {
            Step++;
            return random.Next();
        }

        public static int GetRandomInteger(int max)
        {
            Step++;
            return random.Next(max);
        }

        public static int GetRandomInteger(int min, int max)
        {
            Step++;
            return random.Next(min, max);
        }

        public static float GetRandomFloat()
        {
            Step++;
            var number = random.NextDouble();
            var _f = (float)number;
            return _f;
        }

        public static float GetRandomFloat(float min, float max)
        {
            Step++;
            var number = random.NextDouble();
            var _f = (float)number;
            _f = ((max - min) * _f) + min;
            return _f;
        }

        public static void SetState(int steps)
        {
            Step = steps;
            for (int i = 0; i < Step; i++)
            {
                random.Next();
            }
        }
    }
}