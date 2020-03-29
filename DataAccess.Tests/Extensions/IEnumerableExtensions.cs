using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Tests.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> CloneAndReplaceAt<T>(this IEnumerable<T> enumerable, int index, T value)
        {
            for (int i = 0; i < enumerable.Count(); i++)
            {
                if(i == index)
                {
                    yield return value;
                }
                else
                {
                    yield return enumerable.ElementAt(i);
                }
            }
        }
    }
}
