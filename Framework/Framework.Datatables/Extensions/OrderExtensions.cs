using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Datatables.Extensions
{
    public static class OrderExtension
    {
        private const string SortAsending = "asc";

        public static IEnumerable<T> Order<T>(this IEnumerable<T> entities, JQueryDatatablesParam searchParams)
        {
            var propertyName = searchParams.SortColumnName();

            var enumerable = entities as IList<T> ?? entities.ToList();
            if (!enumerable.Any() || string.IsNullOrEmpty(propertyName))
                return enumerable;

            var propertyInfo = enumerable.First().GetType().GetTypeInfo().GetProperty(propertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            return searchParams.sSortDir_0 == SortAsending
                ? enumerable.OrderBy(e => propertyInfo.GetValue(e, null))
                : enumerable.OrderByDescending(e => propertyInfo.GetValue(e, null));
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> entities, string propertyName)
        {
            var enumerable = entities as IList<T> ?? entities.ToList();
            if (!enumerable.Any() || string.IsNullOrEmpty(propertyName))
                return enumerable;

            var propertyInfo = enumerable.First().GetType().GetTypeInfo().GetProperty(propertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            return enumerable.OrderBy(e => propertyInfo.GetValue(e, null));
        }

        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> entities, string propertyName)
        {
            var enumerable = entities as IList<T> ?? entities.ToList();
            if (!enumerable.Any() || string.IsNullOrEmpty(propertyName))
                return enumerable;

            var propertyInfo = enumerable.First().GetType().GetTypeInfo().GetProperty(propertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            return enumerable.OrderByDescending(e => propertyInfo.GetValue(e, null));
        }
    }
}