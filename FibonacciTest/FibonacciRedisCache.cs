using StackExchange.Redis;

namespace FibonacciTest
{
    internal class FibonacciRedisCache:ICache
    {
        private const string Prefix = "Fibonacci";
        private readonly ConnectionMultiplexer _redisConnection;

        public FibonacciRedisCache(string hostName)
        {
            _redisConnection = ConnectionMultiplexer.Connect(hostName + ",allowAdmin=true");
            _redisConnection.GetServer(hostName).FlushDatabase();
        }

        public int Get(int index)
        {
            var db = _redisConnection.GetDatabase();
            var t = db.KeyType(Prefix + index);
            if (int.TryParse(db.StringGet(Prefix + index), out var res))
            {
                return res;
            }
            else
            {
                return -1;
            }
        }

        public void Set(int index, int value)
        {
            var db = _redisConnection.GetDatabase();
            db.StringSet(Prefix + index, value.ToString());
        }
    }
}
