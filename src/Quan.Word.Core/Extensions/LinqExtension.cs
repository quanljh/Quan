﻿using System;
using System.Collections.Generic;

namespace Quan.Word.Core
{
    public static class LinqExtension
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }
    }
}
