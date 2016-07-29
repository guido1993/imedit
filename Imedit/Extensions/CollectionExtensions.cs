using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace Imedit.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Restituisce la view di una lista per essere usata per il data binding
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A view for the list</returns>
        public static ICollectionView GetView(this IEnumerable source)
        {
            return GetView(source, false);
        }

        /// <summary>
        /// Restituisce la view di una lista per essere usata per il data binding
        /// </summary>
        /// <param name="source"></param>
        /// <param name="isSourceGrouped">Se la sorgente è raggruppata</param>
        /// <returns></returns>
        public static ICollectionView GetView(this IEnumerable source, bool isSourceGrouped)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            CollectionViewSource cs = new CollectionViewSource();
            cs.Source = source;
            cs.IsSourceGrouped = isSourceGrouped;

            return cs.View;
        }

        /// <summary>
        /// Adds items to the list
        /// </summary>
        /// <typeparam name="T">The type of the element</typeparam>
        /// <param name="source">The source where put new items</param>
        /// <param name="range">The range to add.</param>
        public static void AddRange<T>(this IList<T> source, IEnumerable<T> range)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (range == null)
                throw new ArgumentNullException("range");

            foreach (T item in range)
                source.Add(item);
        }

        /// <summary>
        /// Executes an action for each items in the source
        /// </summary>
        /// <typeparam name="T">The type of the element</typeparam>
        /// <param name="source">The source to iterate.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            foreach (T item in source)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Return a flat list iterating the source and getting the childrens recursively
        /// </summary>
        /// <typeparam name="T">The type of the element</typeparam>
        /// <param name="source">The source to iterate</param>
        /// <param name="getChild">The get child function.</param>
        /// <returns></returns>
        public static IEnumerable<T> ToFlat<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getChild)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (getChild == null)
                throw new ArgumentNullException("getChild");

            foreach (T item in source)
            {
                yield return item;

                foreach (T item2 in getChild(item).ToFlat(getChild))
                    yield return item2;
            }
        }

        /// <summary>
        /// Creates a new ObservableCollection<T> from an IEnumrable<T>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            ObservableCollection<T> items = new ObservableCollection<T>();
            items.AddRange(source);
            return items;
        }
    }
}