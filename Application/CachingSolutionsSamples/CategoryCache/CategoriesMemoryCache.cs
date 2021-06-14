using System.Collections.Generic;
using System.Runtime.Caching;
using NorthwindLibrary;

namespace CachingSolutionsSamples.CategoryCache
{
	internal class CategoriesMemoryCache : ICategoriesCache
	{
		ObjectCache cache = MemoryCache.Default;
		string prefix  = "Cache_Categories";

		public IEnumerable<Category> Get(string forUser)
		{
			return (IEnumerable<Category>) cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Category> categories)
		{
			cache.Set(prefix + forUser, categories, ObjectCache.InfiniteAbsoluteExpiration);
		}
	}
}
