using System.Runtime.Caching;

namespace FibonacciTest
{
    internal class FibonacciMemoryCache:ICache
    {
        private const string Prefix = "Fibonacci";
        ObjectCache cache = MemoryCache.Default;

        public int Get(int index)
        {
            var obj = cache.Get(Prefix + index);
            if (obj is int numb)
            {
                return numb;
            }
            else
            {
                return -1;
            }
        }

        public void Set(int index, int value)
        {
            cache.Set(Prefix + index, value, ObjectCache.InfiniteAbsoluteExpiration);
        }
    }
}
