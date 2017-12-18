using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketballPlayerSimulator
{
    public static class Util
    {
        private static readonly object syncLock = new object();
        private static readonly Random random = new Random();

        public static double RandomDouble()
        {
            lock (syncLock)
            { // synchronize
                return random.NextDouble() * 100;
            }
        }

        public static int RandomInteger()
        {
            lock(syncLock)
            {
                return random.Next();
            }
        }

        public static int RandomInteger(int min, int max)
        {
            lock (syncLock)
            {
                return random.Next(min, max);
            }
        }
    }
}
