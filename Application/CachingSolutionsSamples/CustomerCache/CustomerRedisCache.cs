using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using NorthwindLibrary;
using StackExchange.Redis;

namespace CachingSolutionsSamples.CustomerCache
{
    internal class CustomerRedisCache: ICustomerCache
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private const string Prefix = "Categories";
        private readonly DataContractSerializer _serializer = new DataContractSerializer(typeof(IEnumerable<Customer>));

        public CustomerRedisCache(string hostName)
        {
            _redisConnection = ConnectionMultiplexer.Connect(hostName + ",allowAdmin=true");
            _redisConnection.GetServer(hostName).FlushDatabase();
        }

        public IEnumerable<Customer> Get(string forUser)
        {
            var db = _redisConnection.GetDatabase();
            byte[] s = db.StringGet(Prefix + forUser);
            if (s == null)
            {
                return null;
            }

            return (IEnumerable<Customer>)_serializer.ReadObject(new MemoryStream(s));
        }

        public void Set(string forUser, IEnumerable<Customer> categories)
        {
            var db = _redisConnection.GetDatabase();
            var key = Prefix + forUser;

            if (categories == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                _serializer.WriteObject(stream, categories);
                db.StringSet(key, stream.ToArray(), TimeSpan.FromSeconds(2));
            }
        }
    }
}
