using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using NorthwindLibrary;
using StackExchange.Redis;

namespace CachingSolutionsSamples.CategoryCache
{
	class CategoriesRedisCache : ICategoriesCache
	{
		private ConnectionMultiplexer redisConnection;
		string prefix = "Cache_Categories";
		DataContractSerializer serializer = new DataContractSerializer(
			typeof(IEnumerable<Category>));

		public CategoriesRedisCache(string hostName)
		{
			redisConnection = ConnectionMultiplexer.Connect(hostName);
		}

		public IEnumerable<Category> Get(string forUser)
		{
			var db = redisConnection.GetDatabase();
			byte[] s = db.StringGet(prefix + forUser);
			if (s == null)
				return null;

			return (IEnumerable<Category>)serializer
				.ReadObject(new MemoryStream(s));

		}

		public void Set(string forUser, IEnumerable<Category> categories)
		{
			var db = redisConnection.GetDatabase();
			var key = prefix + forUser;

			if (categories == null)
			{
				db.StringSet(key, RedisValue.Null);
			}
			else
			{
				var stream = new MemoryStream();
				serializer.WriteObject(stream, categories);
				db.StringSet(key, stream.ToArray());
			}
		}
	}
}
