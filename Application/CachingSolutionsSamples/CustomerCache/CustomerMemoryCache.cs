using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Caching;
using NorthwindLibrary;

namespace CachingSolutionsSamples.CustomerCache
{
    internal class CustomerMemoryCache : ICustomerCache
    {
        private const string Prefix = "Customers";
        private const string ConnectionString = @"Data Source=OL3GRUS;Initial Catalog=Northwind;Integrated Security=True";
        private const string MonitorCommand = @"Select [CustomerID],[CompanyName],[ContactName],[ContactTitle],[Address],[City],[Region],[PostalCode],[Country],[Phone],[Fax] From [dbo].[Customers]";

        private readonly ObjectCache _cache = MemoryCache.Default;

        public CustomerMemoryCache(string forUser)
        {
            var customers = new List<Customer>();
            var policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(10) };
            SqlDependency.Start(ConnectionString);
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var command = new SqlCommand(MonitorCommand, conn))
                {
                    command.Notification = null;
                    var dep = new SqlDependency();
                    dep.AddCommandDependency(command);
                    conn.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            CustomerID = reader["CustomerID"].ToString(),
                            CompanyName = reader["CompanyName"].ToString(),
                            ContactName = reader["ContactName"].ToString(),
                            ContactTitle = reader["ContactTitle"].ToString(),
                            Address = reader["Address"].ToString(),
                            City = reader["City"].ToString(),
                            Region = reader["Region"].ToString(),
                            PostalCode = reader["PostalCode"].ToString(),
                            Country = reader["Country"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Fax = reader["Fax"].ToString()
                        });
                    }
                    var monitor = new SqlChangeMonitor(dep);
                    policy.ChangeMonitors.Add(monitor);
                }
            }
            _cache.Set(Prefix + forUser, customers, policy);
            Console.WriteLine($"Monitor update record {Prefix + forUser}");
        }

        public IEnumerable<Customer> Get(string forUser)
        {
            return _cache.Get(Prefix + forUser) as IEnumerable<Customer>;
        }

        public void Set(string forUser, IEnumerable<Customer> customers)
        {
            var policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(10) };
            _cache.Set(Prefix + forUser, customers, policy);
        }
        
    }
}
