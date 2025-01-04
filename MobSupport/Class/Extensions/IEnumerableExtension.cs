using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobSupport.Class.Extensions
{
    internal static class IEnumerableExtension
    {
        private static readonly Random _random = new();
        public static T Random<T>(this IEnumerable<T> collection)
        {
            return collection.ElementAt(_random.Next(collection.Count()));
        }
    }
}
