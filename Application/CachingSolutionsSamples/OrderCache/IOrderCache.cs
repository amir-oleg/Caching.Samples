using System.Collections.Generic;
using NorthwindLibrary;

namespace CachingSolutionsSamples.OrderCache
{
    internal interface IOrderCache
    {
        IEnumerable<Order> Get(string forUser);
        void Set(string forUser, IEnumerable<Order> orders);
    }
}
