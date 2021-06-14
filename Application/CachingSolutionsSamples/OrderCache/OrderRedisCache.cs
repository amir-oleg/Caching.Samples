using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using StackExchange.Redis;
using Order = NorthwindLibrary.Order;

namespace CachingSolutionsSamples.OrderCache
{
    internal class OrderRedisCache: IOrderCache
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private const string Prefix = "Orders";
        private readonly DataContractSerializer _serializer = new DataContractSerializer(typeof(IEnumerable<Order>));

        public OrderRedisCache(string hostName)
        {
            _redisConnection = ConnectionMultiplexer.Connect(hostName + ",allowAdmin=true");
            _redisConnection.GetServer(hostName).FlushDatabase();
        }

        public IEnumerable<Order> Get(string forUser)
        {
            var db = _redisConnection.GetDatabase();
            byte[] s = db.StringGet(Prefix + forUser);
            if (s == null)
            {
                return null;
            }

            return (IEnumerable<Order>)_serializer.ReadObject(new MemoryStream(s));
        }

        public void Set(string forUser, IEnumerable<Order> orders)
        {
            var db = _redisConnection.GetDatabase();
            var key = Prefix + forUser;

            if (orders == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                _serializer.WriteObject(stream, orders);
                db.StringSet(key, stream.ToArray(), TimeSpan.FromSeconds(2));
            }
        }
    }
}
