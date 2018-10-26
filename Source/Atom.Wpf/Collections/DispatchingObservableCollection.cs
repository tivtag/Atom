// <copyright file="DispatchingObservableCollection.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Collections.DispatchingObservableCollection class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Acid.Wpf.Collections
{
    using System;
    using System.Collections.ObjectModel;
    using Atom.Diagnostics.Contracts;
    using System.Windows.Threading;

    /// <summary>
    /// Represents a thread-safe dynamic data collection that provides notifications when items
    /// get added, removed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="T">
    /// The type of items stored in the collection.
    /// </typeparam>
    public class DispatchingObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the DispatchingObservableCollection class.
        /// </summary>
        /// <param name="dispatcher">
        /// The dispatcher that is running on the UI thread.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="dispatcher"/> is null.
        /// </exception>
        public DispatchingObservableCollection( Dispatcher dispatcher )
        {
            Contract.Requires<ArgumentNullException>( dispatcher != null );

            this.dispatcher = dispatcher;
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        protected override void SetItem( int index, T item )
        {
            if( this.dispatcher.CheckAccess() )
            {
                base.SetItem( index, item );
            }
            else
            {
                this.dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new Action<int, T>( this.SetItem ),
                    index,
                    new object[] { item }
                );
            }
        }

        /// <summary>
        /// Removes the item at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem( int index )
        {
            if( this.dispatcher.CheckAccess() )
            {
                base.RemoveItem( index );
            }
            else
            {
                this.dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new Action<int>( this.RemoveItem ),
                    index
                );
            }
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        protected override void ClearItems()
        {
            if( this.dispatcher.CheckAccess() )
            {
                base.ClearItems();
            }
            else
            {
                this.dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new Action( this.ClearItems )
                );
            }
        }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index"> The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem( int index, T item )
        {
            if( this.dispatcher.CheckAccess() )
            {
                base.InsertItem( index, item );
            }
            else
            {
                this.dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new Action<int, T>( this.InsertItem ),
                    index,
                    new object[] { item }
                );
            }
        }

        /// <summary>
        ///  Moves the item at the specified index to a new location in the collection.
        /// </summary>
        /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        protected override void MoveItem( int oldIndex, int newIndex )
        {
            if( this.dispatcher.CheckAccess() )
            {
                base.MoveItem( oldIndex, newIndex );
            }
            else
            {
                this.dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new Action<int, int>( this.MoveItem ),
                    oldIndex,
                    new object[] { newIndex }
                );
            }
        }

        /// <summary>
        /// The Dispatcher we're using to make the collection thread-safe.
        /// </summary>
        private readonly Dispatcher dispatcher;
    }
}
