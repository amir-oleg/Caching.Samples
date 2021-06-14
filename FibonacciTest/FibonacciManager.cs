using System;
using System.Text;

namespace FibonacciTest
{
    internal class FibonacciManager
    {
        private readonly ICache _cache;

        public FibonacciManager(ICache cache)
        {
            _cache = cache;
        }

        internal int GetFibonacci(int index)
        {
            var res = _cache.Get(index);
            if (res == -1)
            {
                res = CalcFibonacci(index);
            }
            return res; 
        }

        private void SetFibonacci(int index, int value)
        {
            if (_cache.Get(index) == -1)
            {
                Console.WriteLine($"Saving Fibonacci{index} - {value}");
                _cache.Set(index, value);
            }
        }

        private int CalcFibonacci(int index)
        {
            if (index == 0 || index == 1)
            {
                SetFibonacci(index, index);
                return index;
            }

            var first = _cache.Get(index - 1);
            if (first == -1)
            {
                first = CalcFibonacci(index - 1);
            }

            var second = _cache.Get(index - 2);
            if (second == -1)
            {
                second = CalcFibonacci(index - 2);
            }

            var res = first + second;
            SetFibonacci(index, res);
            return res;
        }
    }
}
