using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NorthwindLibrary;

namespace CachingSolutionsSamples.CategoryCache
{
	internal class CategoriesManager
	{
		private ICategoriesCache cache;

		public CategoriesManager(ICategoriesCache cache)
		{
			this.cache = cache;
		}

		public IEnumerable<Category> GetCategories()
		{
			Console.WriteLine("Get Categories");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var categories = cache.Get(user);

			if (categories == null)
			{
				Console.WriteLine("From DB");

				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
					categories = dbContext.Categories.ToList();
					cache.Set(user, categories);
				}
			}

			return categories;
		}
	}
}
