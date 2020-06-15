using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Quan.Word
{

    public static class TypeUtilities
    {
        /// <summary>
        /// Create a list of actual data type at runtime
        /// </summary>
        /// <param name="source">The source item of <see cref="DragInfo"/></param>
        /// <returns></returns>
        public static IEnumerable CreateDynamicallyTypedList(IEnumerable source)
        {
            var enumerable = source as object[] ?? source.Cast<object>().ToArray();
            var type = GetCommonBaseClass(enumerable);
            var listType = typeof(List<>).MakeGenericType(type);
            var addMethod = listType.GetMethod("Add");
            // Use type's default constructor to create list
            var list = listType.GetConstructor(Type.EmptyTypes)?.Invoke(null);

            foreach (var o in enumerable)
            {
                addMethod?.Invoke(list, new[] { o });
            }

            return (IEnumerable)list;
        }

        public static Type GetCommonBaseClass(IEnumerable e)
        {
            var types = e.Cast<object>().Select(o => o.GetType()).ToArray<Type>();
            return GetCommonBaseClass(types);
        }

        public static Type GetCommonBaseClass(Type[] types)
        {
            if (types.Length == 0)
            {
                return typeof(object);
            }

            var ret = types[0];

            for (var i = 1; i < types.Length; ++i)
            {
                if (types[i].IsAssignableFrom(ret))
                {
                    ret = types[i];
                }
                else
                {
                    // This will always terminate when ret == typeof(object)
                    while (ret != null && !ret.IsAssignableFrom(types[i]))
                    {
                        ret = ret.BaseType;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets the enumerable as list.
        /// If enumerable is an ICollectionView then it returns the SourceCollection as list.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>Returns a list.</returns>
        public static IList TryGetList(this IEnumerable enumerable)
        {
            if (enumerable is ICollectionView view)
            {
                return view.SourceCollection as IList;
            }

            var list = enumerable as IList;
            return list ?? enumerable?.OfType<object>().ToList();
        }
    }
}
