using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.CategoryCache;
using CachingSolutionsSamples.CustomerCache;
using CachingSolutionsSamples.OrderCache;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
		[TestMethod]
		public void CategoriesMemoryCache()
		{
			var categoryManager = new CategoriesManager(new CategoriesMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetCategories().Count());
				Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void CategoriesRedisCache()
		{
			var categoryManager = new CategoriesManager(new CategoriesRedisCache("127.0.0.1:6379"));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetCategories().Count());
				Thread.Sleep(100);
			}
		}

        [TestMethod]
		public void CustomersMemoryCache()
        {
            var customerManager = new CustomerManager(new CustomerMemoryCache(Environment.UserName));

            for (var i = 0; i < 20; i++)
            {
                Console.WriteLine(customerManager.GetCustomers().Count());
				Thread.Sleep(500);
            }
        }

        [TestMethod]
        public void CustomersRedisCache()
        {
            var customerManager = new CustomerManager(new CustomerRedisCache("127.0.0.1:6379"));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(customerManager.GetCustomers().Count());
                Thread.Sleep(400);
            }
		}

        [TestMethod]
        public void OrdersMemoryCache()
        {
            var customerManager = new OrderManager(new OrderMemoryCache(Environment.UserName));

            for (var i = 0; i < 20; i++)
            {
                Console.WriteLine(customerManager.GetOrders().Count());
                Thread.Sleep(500);
            }
        }

        [TestMethod]
        public void OrdersRedisCache()
        {
            var customerManager = new OrderManager(new OrderRedisCache("127.0.0.1:6379"));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(customerManager.GetOrders().Count());
                Thread.Sleep(400);
            }
        }
    }
}
