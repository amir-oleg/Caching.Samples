using System.Collections.Generic;
using NorthwindLibrary;

namespace CachingSolutionsSamples.CustomerCache
{
    internal interface ICustomerCache
    {
        IEnumerable<Customer> Get(string forUser);
        void Set(string forUser, IEnumerable<Customer> categories);
    }
}