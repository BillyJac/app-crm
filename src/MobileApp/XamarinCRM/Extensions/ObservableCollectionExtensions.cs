﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using XamarinCRM.Models;
using System.Reflection;
using System.Linq;

namespace XamarinCRM.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var i in items)
            {
                collection.Add(i);
            }
        }

        /// <summary>
        /// Adds a range of IEnumerable<T> to an ObservableCollection<Grouping<K,T>, grouped by a propertyName of type K.
        /// </summary>
        /// <param name="collection">An ObservableCollection<Grouping<K,T>>.</param>
        /// <param name="items">IEnumerable<T></param>
        /// <param name="propertyName">The property name to group by. MUST be a valid property name on type T and MUST be of type K. Throws an argument exception if not.</param>
        /// <typeparam name="K">The type of the Grouping key.</typeparam>
        /// <typeparam name="T">The type of items in the items collection of the Grouping.</typeparam>
        public static void AddRange<K,T>(this ObservableCollection<Grouping<K,T>> collection, IEnumerable<T> items, string propertyName)
        {
            if (typeof(T).GetRuntimeProperties().All(propertyInfo => propertyInfo.Name != propertyName))
            {
                throw new ArgumentException(String.Format("Type '{0}' does not have a property named '{1}'", typeof(T).Name, propertyName));
            }

            var groupings = items.GroupBy(t => t.GetType().GetRuntimeProperties().Single(propertyInfo => propertyInfo.Name == propertyName).GetValue(t, null));

            collection.AddRange(groupings.Select(grouping => new Grouping<K,T>((K)grouping.Key, grouping)));
        }
    }
}

