// <copyright file="MultiLog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.MultiLog class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a mechanism that allows writing to multiple <see cref="ILog"/> 
    /// instances at the same time. This class can't be inherited.
    /// </summary>
    public sealed class MultiLog : BaseLog, IList<ILog>
    {
        #region [ Properties ]
        
        /// <summary>
        /// Gets the number of <see cref="ILog"/>s that listen to this MultiLog.
        /// </summary>
        public int Count
        {
            get 
            {
                return this.listeners.Count; 
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Writes a new empty line to each ILog that has been attached to this MultiLog.
        /// </summary>
        public override void WriteLine()
        {
            foreach( var log in this.listeners )
            {
                log.WriteLine();
            }
        }

        /// <summary>
        /// Writes the given message to each ILog that has been attached to this MultiLog using
        /// the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        protected override void ActuallyWrite( LogSeverities severity, string message )
        {
            foreach( var log in this.listeners )
            {
                log.Write( severity, message );
            }
        }

        /// <summary>
        /// Writes the given message, followed by a new line,
        /// to each ILog that has been attached to this MultiLog
        /// using the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        protected override void ActuallyWriteLine( LogSeverities severity, string message )
        {
            foreach( var log in this.listeners )
            {
                log.WriteLine( severity, message );
            }
        }
        
        #region > Organization <

        /// <summary>
        /// Adds the given <see cref="ILog"/> to the list of logs
        /// that listen to this MultiLog.
        /// </summary>
        /// <param name="item">
        /// The ILog to add.
        /// </param>
        public void Add( ILog item )
        {
            this.listeners.Add( item );
        }

        /// <summary>
        /// Tries to remove the given <see cref="ILog"/> from the list 
        /// of logs that listen to this MultiLog.
        /// </summary>
        /// <param name="item">
        /// The ILog to remove.
        /// </param>
        /// <returns>
        /// Returns true if item was successfully removed from this MultiLog; 
        /// otherwise, false.
        /// </returns>
        public bool Remove( ILog item )
        {
            return this.listeners.Remove( item );
        }

        /// <summary>
        /// Removes all <see cref="ILog"/>s that listen to this MultiLog.
        /// </summary>
        public void Clear()
        {
            this.listeners.Clear();
        }

        /// <summary> 
        /// Determines whether this MultiLog contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The ILog to locate in this MultiLog. 
        /// </param>
        /// <returns>
        /// Returns true if the given <see cref="ILog"/> listens to this MultiLog;
        /// otherwise false.
        /// </returns>
        public bool Contains( ILog item )
        {
            return this.listeners.Contains( item );
        }

        #region IList<ILog> Members

        /// <summary>
        /// Gets or sets the ILog at the given index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the ILog to get or sets.
        /// </param>
        /// <returns>
        /// The ILog at the given zero-based index.
        /// </returns>
        ILog IList<ILog>.this[int index]
        {
            get
            {
                return this.listeners[index];
            }

            set
            {
                this.listeners[index] = value;
            }
        }

        /// <summary>
        /// Determines the index of a specific item in the underlying System.Collections.Generic.IList{T}.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the underlying System.Collections.Generic.IList{T}.
        /// </param>
        /// <returns>
        /// The index of item if found in the list; otherwise, -1.
        /// </returns>
        int IList<ILog>.IndexOf( ILog item )
        {
            return this.listeners.IndexOf( item );
        }

        /// <summary>
        ///  Inserts an item to the underlying System.Collections.Generic.IList{T} at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the underlying System.Collections.Generic.IList{T}.</param>
        void IList<ILog>.Insert( int index, ILog item )
        {
            this.listeners.Insert( index, item );
        }

        /// <summary>
        /// Removes the System.Collections.Generic.IList{T} item at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        void IList<ILog>.RemoveAt( int index )
        {
            this.listeners.RemoveAt( index );
        }

        #endregion

        #region ICollection<ILog> Members

        /// <summary>
        /// Copies the elements of the underlying System.Collections.Generic.ICollection{T} to an 
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements
        /// copied from System.Collections.Generic.ICollection{T}. The System.Array must
        /// have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in array at which copying begins.
        /// </param>
        void ICollection<ILog>.CopyTo( ILog[] array, int arrayIndex )
        {
            this.listeners.CopyTo( array, arrayIndex );
        }

        /// <summary>
        /// Gets a value indicating whether the MultiLog is read-only.
        /// </summary>
        bool ICollection<ILog>.IsReadOnly
        {
            get
            {
                return this.listeners.IsReadOnly; 
            }
        }

        #endregion

        #region IEnumerable<ILog> Members

        /// <summary>
        /// Returns an enumerator that iterates through the list of <see cref="ILog"/>s that listen to this MultiLog.
        /// </summary>
        /// <returns>
        /// An IEnumerator{ILog} that can be used to iterate through this MultiLog.
        /// </returns>
        public IEnumerator<ILog> GetEnumerator()
        {
            return this.listeners.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list of <see cref="ILog"/>s that listen to this MultiLog.
        /// </summary>
        /// <returns>
        /// An IEnumerator that can be used to iterate through this MultiLog.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.listeners.GetEnumerator();
        }

        #endregion

        #endregion
        
        #endregion

        #region [ Fields ]

        /// <summary>
        /// The list of <see cref="ILog"/>s that listen to this MultiLog.
        /// </summary>
        private readonly IList<ILog> listeners = new List<ILog>();

        #endregion
    }
}
