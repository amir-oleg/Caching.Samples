using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FibonacciTest
{
    [TestClass]
    public class CacheTests
    {
        private int[] _fib = {0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 17711};

        [TestMethod]
        public void MemoryCacheTest()
        {
            var manager = new FibonacciManager(new FibonacciMemoryCache());
            var rand = new Random();
            var index = rand.Next(5, _fib.Length - 4);
            var res = manager.GetFibonacci(index);
            Assert.AreEqual(_fib[index], res);
            Console.WriteLine(string.Empty.PadLeft(100,'-'));
            res = manager.GetFibonacci(index + 3);
            Assert.AreEqual(_fib[index + 3], res);
        }

        [TestMethod]
        public void RedisCacheTest()
        {
            var manager = new FibonacciManager(new FibonacciRedisCache("127.0.0.1:6379"));
            var rand = new Random();
            var index = rand.Next(5, _fib.Length - 4);
            var res = manager.GetFibonacci(index);
            Assert.AreEqual(_fib[index], res);
            Console.WriteLine(string.Empty.PadLeft(100, '-'));
            res = manager.GetFibonacci(index + 3);
            Assert.AreEqual(_fib[index + 3], res);
        }
    }
}
