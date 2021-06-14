using System.Collections.Generic;
using NorthwindLibrary;

namespace CachingSolutionsSamples.CategoryCache
{
	public interface ICategoriesCache
	{
		IEnumerable<Category> Get(string forUser);
		void Set(string forUser, IEnumerable<Category> categories);
	}
}
