using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using NorthwindLibrary;

namespace CachingSolutionsSamples.CustomerCache
{
    internal class CustomerManager
    {
        private ICustomerCache _cache;

        public CustomerManager(ICustomerCache cache)
        {
            _cache = cache;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            Console.WriteLine("Get customers");
            var user = Environment.UserName;
            IEnumerable<Customer> customers = null;
            try
            {
                customers = _cache.Get(user);
                if (customers == null)
                {
                    Console.WriteLine($"Query to db for user {user}");
                    using (var dbContext = new Northwind())
                    {
                        dbContext.Configuration.LazyLoadingEnabled = false;
                        dbContext.Configuration.ProxyCreationEnabled = false;
                        customers = dbContext.Customers.ToList();
                        _cache.Set(user, customers);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return customers;
        }
    }
}