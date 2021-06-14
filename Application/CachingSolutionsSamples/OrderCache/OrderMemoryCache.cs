using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Caching;
using NorthwindLibrary;

namespace CachingSolutionsSamples.OrderCache
{
    internal class OrderMemoryCache: IOrderCache
    {
        private const string Prefix = "Orders";
        private const string ConnectionString = @"Data Source=OL3GRUS;Initial Catalog=Northwind;Integrated Security=True";
        private const string MonitorCommand = @"Select [OrderID],[CustomerID],[EmployeeID],[OrderDate],[RequiredDate],[ShippedDate],[ShipVia],[Freight],[ShipName],[ShipAddress],[ShipCity],[ShipRegion],[ShipPostalCode],[ShipCountry] From [dbo].[Orders]";
        private readonly ObjectCache _cache = MemoryCache.Default;

        public OrderMemoryCache(string forUser)
        {
            var customers = new List<Order>();
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
                        if (!int.TryParse(reader["OrderID"].ToString(), out var orderId)
                            || !int.TryParse(reader["EmployeeID"].ToString(), out var employeeId)
                            || !DateTime.TryParse(reader["OrderDate"].ToString(), out var orderDate)
                            || !DateTime.TryParse(reader["RequiredDate"].ToString(), out var requiredDate)
                            || !DateTime.TryParse(reader["ShippedDate"].ToString(), out var shippedDate)
                            || !int.TryParse(reader["ShipVia"].ToString(), out var shipVia)
                            || !decimal.TryParse(reader["Freight"].ToString(), out var freight))
                        {
                            continue;
                        }
                        customers.Add(new Order
                        {
                            OrderID = orderId,
                            CustomerID = reader["CustomerID"].ToString(),
                            EmployeeID = employeeId,
                            OrderDate = orderDate,
                            RequiredDate = requiredDate,
                            ShippedDate = shippedDate,
                            ShipVia = shipVia,
                            Freight = freight,
                            ShipName = reader["ShipName"].ToString(),
                            ShipAddress = reader["ShipAddress"].ToString(),
                            ShipCity = reader["ShipCity"].ToString(),
                            ShipRegion = reader["ShipRegion"].ToString(),
                            ShipPostalCode = reader["ShipPostalCode"].ToString(),
                            ShipCountry = reader["ShipCountry"].ToString()
                        });
                    }
                    var monitor = new SqlChangeMonitor(dep);
                    policy.ChangeMonitors.Add(monitor);
                }
            }
            _cache.Set(Prefix + forUser, customers, policy);
            Console.WriteLine($"Monitor update record {Prefix + forUser}");
        }

        public IEnumerable<Order> Get(string forUser)
        {
            return _cache.Get(Prefix + forUser) as IEnumerable<Order>;
        }

        public void Set(string forUser, IEnumerable<Order> orders)
        {
            var policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(10) };
            _cache.Set(Prefix + forUser, orders, policy);
        }
    }
}
