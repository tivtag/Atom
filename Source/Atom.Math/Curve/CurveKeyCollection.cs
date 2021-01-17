// <copyright file="CurveKeyCollection.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.CurveKeyCollection class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Stores the CurveKeys making up a Curve.
    /// This is a sealed class.
    /// </summary>
    [System.ComponentModel.TypeConverter( typeof( System.ComponentModel.ExpandableObjectConverter ) )]
    [System.Serializable]
    public sealed class CurveKeyCollection : IList<CurveKey>, IList, ICloneable
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the number of elements contained in the CurveKeyCollection.
        /// </summary>
        /// <value>The number of elements in the CurveKeyCollection.</value>
        public int Count
        {
            get
            {
                return this.keys.Count;
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Set: If <paramref name="value"/> is null.
        /// </exception>
        /// <param name="index">The zero-based array index of the element.</param>
        /// <returns>The CurveKey at the specified index.</returns>
        public CurveKey this[int index]
        {
            get
            {
                return this.keys[index];
            }

            set
            {
                if( value == null )
                    throw new ArgumentNullException( "value" );

                if( this.keys[index].Position == value.Position )
                {
                    this.keys[index] = value;
                }
                else
                {
                    this.keys.RemoveAt( index );
                    this.Add( value );
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the CurveKeyCollection is read-only.
        /// </summary>
        /// <value>
        /// Always returns <see langword="false"/>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the inversed time range.
        /// </summary>
        /// <value>The <see cref="TimeRange"/> property; inversed.</value>
        internal float InvTimeRange
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the cache is currently available, 
        /// or needs to be updated.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if the cache is currently available;
        /// or otherwise <see langword="false"/> if the cache needs to be updated.
        /// </value>
        internal bool IsCacheAvailable   
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total time the CurvedKeys have when added together.
        /// </summary>
        /// <value>The total time the Curve takes up.</value>
        internal float TimeRange   
        {
            get;
            private set;
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveKeyCollection"/> class.
        /// </summary>
        public CurveKeyCollection()
        {
            this.keys = new List<CurveKey>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveKeyCollection"/> class.
        /// </summary>
        /// <param name="keys">
        /// The keys that make-up the curve.
        /// </param>
        internal CurveKeyCollection( IEnumerable<CurveKey> keys )
        {
            this.keys = new List<CurveKey>( keys );
        }
        
        #endregion

        #region [ Methods ]

        /// <summary>
        /// Adds a CurveKey to the CurveKeyCollection.
        /// </summary>
        /// <param name="item">The CurveKey to add.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="item"/> is null.
        /// </exception>
        public void Add( CurveKey item )
        {
            if( item == null )
                throw new ArgumentNullException( "item" );

            int index = this.keys.BinarySearch(item);
            if( index >= 0 )
            {
                while( (index < this.keys.Count) && (item.Position == this.keys[index].Position) )
                {
                    ++index;
                }
            }
            else
            {
                index = ~index;
            }

            this.keys.Insert( index, item );
            this.IsCacheAvailable = false;
        }

        /// <summary>
        /// Inserts the given <see cref="CurveKey"/> into the <see cref="CurveKeyCollection"/> at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which the <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The CurveKey to insert.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="item"/> is null.
        /// </exception>
        public void Insert( int index, CurveKey item )
        {
            if( item == null )
                throw new ArgumentNullException( "item" );

            this.keys.Insert( index, item );
        }

        /// <summary>
        /// Removes all CurveKeys from the CurveKeyCollection.
        /// </summary>
        public void Clear()
        {
            this.keys.Clear();

            this.TimeRange        = this.InvTimeRange = 0f;
            this.IsCacheAvailable = false;
        }
        
        /// <summary>
        /// Computes internal cached values.
        /// </summary>
        internal void ComputeCacheValues()
        {
            if( this.keys.Count > 1 )
            {
                this.TimeRange = this.keys[this.keys.Count - 1].Position - this.keys[0].Position;
                if( this.TimeRange > float.Epsilon )
                {
                    this.InvTimeRange = 1f / this.TimeRange;
                }
                else
                {
                    this.InvTimeRange = 0f;
                }
            }
            else
                this.TimeRange = this.InvTimeRange = 0f;

            this.IsCacheAvailable = true;
        }

        /// <summary>
        /// Creates a clone of the CurveKeyCollection.
        /// </summary>
        /// <returns>The cloned CurveKeyCollection.</returns>
        public CurveKeyCollection Clone()
        {
            return new CurveKeyCollection( this.keys ) {
                InvTimeRange     = this.InvTimeRange,
                TimeRange        = this.TimeRange,
                IsCacheAvailable = true
            };
        }

        /// <summary>
        /// Creates a clone of the CurveKeyCollection.
        /// </summary>
        /// <returns>The cloned CurveKeyCollection.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        
        /// <summary>
        /// Determines whether the CurveKeyCollection contains a specific CurveKey.
        /// </summary>
        /// <param name="item">
        /// The CurveKey to locate in the CurveKeyCollection.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the CurveKey is found in the CurveKeyCollection; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Contains( CurveKey item )
        {
            return this.keys.Contains(item);
        }

        /// <summary>
        /// Copies the CurveKeys of the CurveKeyCollection to an array, starting at the array index provided.
        /// </summary>
        /// <param name="array">
        /// The destination of the CurveKeys copied from CurveKeyCollection. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in the array to start copying from.</param>
        public void CopyTo( CurveKey[] array, int arrayIndex )
        {
            this.keys.CopyTo(array, arrayIndex);
            this.IsCacheAvailable = false;
        }

        /// <summary>
        /// Determines the index of a CurveKey in the CurveKeyCollection.
        /// </summary>
        /// <param name="item">The CurveKey to locate in the CurveKeyCollection.</param>
        /// <returns>
        /// The index of the CurveKey if found in the CurveKeyCollection;
        /// −1 otherwise.
        /// </returns>
        public int IndexOf( CurveKey item )
        {
            return this.keys.IndexOf(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific CurveKey from the CurveKeyCollection.
        /// </summary>
        /// <param name="item">The CurveKey to remove from the CurveKeyCollection.</param>
        /// <returns>true if CurveKey is successfully removed; false otherwise.</returns>
        public bool Remove( CurveKey item )
        {
            this.IsCacheAvailable = false;
            return this.keys.Remove(item);
        }

        /// <summary>
        /// Removes the CurveKey at the specified zero-based index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        public void RemoveAt( int index )
        {
            this.keys.RemoveAt(index);
            this.IsCacheAvailable = false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the CurveKeyCollection.
        /// </summary>
        /// <returns>An enumerator for the CurveKeyCollection.</returns>
        public IEnumerator<CurveKey> GetEnumerator()
        {
            return this.keys.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the CurveKeyCollection.
        /// </summary>
        /// <returns>An enumerator for the CurveKeyCollection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.keys.GetEnumerator();
        }

        /// <summary>
        /// Removes the CurveKey at the specified zero-based index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        void IList.RemoveAt( int index )
        {
            this.RemoveAt( index );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The list that is internally used to store the keys.
        /// </summary>
        private readonly List<CurveKey> keys;

        #endregion

        #region [ Private Implemenations ]

        #region IList Members

        /// <summary>
        /// Adds a CurveKey to the CurveKeyCollection.
        /// </summary>
        /// <param name="value">The CurveKey to add.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="value"/> is null.
        /// </exception>
        /// <returns>The position at which the item was added.</returns>
        int IList.Add( object value )
        {
            this.Add( (CurveKey)value );
            return this.Count - 1;
        }

        /// <summary>
        /// Determines whether the CurveKeyCollection contains a specific CurveKey.
        /// </summary>
        /// <param name="value">
        /// The CurveKey to locate in the CurveKeyCollection.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the CurveKey is found in the CurveKeyCollection; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        bool IList.Contains( object value )
        {
            return this.Contains( value as CurveKey );
        }

        /// <summary>
        /// Determines the index of a CurveKey in the CurveKeyCollection.
        /// </summary>
        /// <param name="value">The CurveKey to locate in the CurveKeyCollection.</param>
        /// <returns>
        /// The index of the CurveKey if found in the CurveKeyCollection;
        /// −1 otherwise.
        /// </returns>
        int IList.IndexOf( object value )
        {
            return this.IndexOf( value as CurveKey );
        }

        /// <summary>
        /// Inserts the given <see cref="CurveKey"/> into the <see cref="CurveKeyCollection"/> at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which the <paramref name="value"/> should be inserted.</param>
        /// <param name="value">The CurveKey to insert.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="value"/> is null.
        /// </exception>
        void IList.Insert( int index, object value )
        {
            this.Insert( index, (CurveKey)value );
        }

        /// <summary>
        /// Gets a value indicating whether the CurveKeyCollection is of fixed size.
        /// </summary>
        /// <value>
        /// Always returns <see langword="false"/>.
        /// </value>
        bool IList.IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific CurveKey from the CurveKeyCollection.
        /// </summary>
        /// <param name="value">The CurveKey to remove from the CurveKeyCollection.</param>
        void IList.Remove( object value )
        {
            this.Remove( value as CurveKey );
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Set: If <paramref name="value"/> is null.
        /// </exception>
        /// <param name="index">The zero-based array index of the element.</param>
        /// <returns>The CurveKey at the specified index.</returns>
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                this[index] = (CurveKey)value;
            }
        }

        #endregion

        #region ICollection Members

        /// <summary>
        /// Copies the CurveKeys of the CurveKeyCollection to an array, starting at the array index provided.
        /// </summary>
        /// <param name="array">
        /// The destination of the CurveKeys copied from CurveKeyCollection. The array must have zero-based indexing.
        /// </param>
        /// <param name="index">The zero-based index in the array to start copying from.</param>
        void ICollection.CopyTo( Array array, int index )
        {
            Array.Copy( this.keys.ToArray(), array, index );
            this.IsCacheAvailable = false;
        }

        /// <summary>
        /// Gets a value indicating whether the CurveKeyCollection is synchronized.
        /// </summary>
        /// <value>
        /// Always returns <see langword="false"/>.
        /// </value>
        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the snyc root object.
        /// </summary>
        /// <value>
        /// Always returns <see langword="null"/>.
        /// </value>
        object ICollection.SyncRoot
        {
            get { return null; }
        }

        #endregion

        #endregion
    }
}
