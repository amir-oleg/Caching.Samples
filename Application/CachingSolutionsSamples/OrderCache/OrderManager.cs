using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NorthwindLibrary;

namespace CachingSolutionsSamples.OrderCache
{
    internal class OrderManager
    {
        private readonly IOrderCache _cache;

        public OrderManager(IOrderCache cache)
        {
            _cache = cache;
        }

        public IEnumerable<Order> GetOrders()
        {
            Console.WriteLine("Get Orders");
            var user = Environment.UserName;
            IEnumerable<Order> orders = null;
            try
            {
                orders = _cache.Get(user);

                if (orders == null)
                {
                    Console.WriteLine($"Query to db for user {user}");

                    using (var dbContext = new Northwind())
                    {
                        dbContext.Configuration.LazyLoadingEnabled = false;
                        dbContext.Configuration.ProxyCreationEnabled = false;
                        orders = dbContext.Orders.ToList();
                        _cache.Set(user, orders);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return orders;
        }
	}
}
