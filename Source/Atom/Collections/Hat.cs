// <copyright file="Hat.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.Hat{T} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Atom.Math;

    /// <summary>
    /// A <see cref="Hat{T}"/> is a collection of weighted items.
    /// </summary>
    /// <typeparam name="T"> 
    /// The type of the items to be stored in the hat.
    /// </typeparam>
    public class Hat<T> : IEnumerable<HatEntry<T>>
    {
        #region [ Constants ]

        /// <summary>
        /// The default Id of items stored in the <see cref="Hat{T}"/>.
        /// </summary>
        private const int DefaultItemId = 0;

        /// <summary>
        /// The default weight modifier of items stored in the <see cref="Hat{T}"/>.
        /// </summary>
        private const float DefaultWeightModifier = 1.0f;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets a value that represents the number of HatEntries 
        /// that have been inserted into this <see cref="Hat{T}"/>.
        /// </summary>
        /// <value>
        /// The number of elements actually contained in the Hat{T}.
        /// </value>
        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        /// <summary>
        /// Gets the total weight of all items in the <see cref="Hat{T}"/>.
        /// </summary>
        /// <value>
        /// A value that represents the total weight contained in the Hat{T}.
        /// </value>
        public float TotalWeight
        {
            get 
            {
                return this.totalWeight;
            }

            internal set
            {
                this.totalWeight = value;
            }
        }

        /// <summary>
        /// Gets a reference to the <see cref="HatEntry{T}"/> at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The index of the <see cref="HatEntry{T}"/> to get.
        /// </param>
        /// <returns>
        /// The HatEntry{T} at the given <paramref name="index"/>.
        /// </returns>
        public HatEntry<T> this[int index]
        {
            get
            {
                return this.items[index];
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Hat{T}"/> class.
        /// </summary>
        /// <param name="rand">
        /// The random number generator the new Hat{T} should use.
        /// </param>
        /// <param name="capacity">
        /// The initial capacity of the <see cref="Hat{T}"/>.
        /// </param>
        public Hat( IRand rand, int capacity )
        {
            Contract.Requires<ArgumentNullException>( rand != null );

            this.rand  = rand;
            this.items = new List<HatEntry<T>>( capacity );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hat{T}"/> class.
        /// </summary>
        /// <param name="rand">
        /// The random number generator the new Hat{T} should use.
        /// </param>
        public Hat( IRand rand )
        {
            Contract.Requires<ArgumentNullException>( rand != null );

            this.rand = rand;
            this.items = new List<HatEntry<T>>();
        }

        #endregion

        #region [ Methods ]

        #region Get

        /// <summary>
        /// Randomly picks out one item from the <see cref="Hat{T}"/>.
        /// Items with greater weight have a higher chance to be picked.
        /// </summary>
        /// <returns>
        /// An item that has been randomly (based on item weight) picked out of the Hat{T}.
        /// </returns>
        public T Get()
        {
            Debug.Assert( totalWeight >= 0.0f, "The Hat is empty; or its items are weightless." );
            float value = rand.RandomRange( 0.0f, this.totalWeight );

            float valueItr = 0.0f;
            for( int i = 0; i < this.items.Count; ++i )
            {
                HatEntry<T> entry = this.items[i];
                valueItr += entry.Weight;

                if( value <= valueItr )
                {
                    return entry.Data;
                }
            }

            return default( T );
        }

        #endregion

        #region SetWeightModifier

        /// <summary>
        /// Sets the weight modifier of all <see cref="HatEntry{T}"/>ies
        /// that share the given <paramref name="id"/>.
        /// Entries that have HasIndividualModifier set to true are excluded. 
        /// </summary>
        /// <param name="id"> The id of the items to set the modifier for. </param>
        /// <param name="modifier"> The new modifier. </param>
        public void SetWeightModifier( int id, float modifier )
        {
            Contract.Requires<ArgumentException>( modifier >= 0.0f );

            for( int i = 0; i < this.items.Count; ++i )
            {
                HatEntry<T> entry = this.items[i];

                if( entry.Id == id && !entry.HasIndividualModifier )
                {
                    entry.WeightModifier = modifier;
                }
            }
        }

        #endregion

        #region > Organization <

        #region - Insert -

        /// <summary>
        /// Inserts a new item into the <see cref="Hat{T}"/>,
        /// which has the <see cref="DefaultItemId"/>, <see cref="DefaultWeightModifier"/> and
        /// has an individual weight modifier.
        /// </summary>
        /// <param name="item">
        /// The actual item. Can be null.
        /// </param>
        /// <param name="weight">
        /// The weight of the entry. May not be a negative value.
        /// </param>
        /// <returns>
        /// The newly inserted HatEntry{T}.
        /// </returns>
        public HatEntry<T> Insert( T item, float weight )
        {
            return this.Insert( item, weight, DefaultItemId, DefaultWeightModifier, true );
        }

        /// <summary>
        /// Inserts a new item into the <see cref="Hat{T}"/>,
        /// which has the <see cref="DefaultWeightModifier"/>.
        /// </summary>
        /// <param name="item">
        /// The actual item. Can be null.
        /// </param>
        /// <param name="weight">
        /// The weight of the entry. May not be a negative value.
        /// </param>
        /// <param name="id">
        /// The Id of the item. 
        /// Ids can be used to create 'item groups' by applying the same Id
        /// to the items that should make up a group.
        /// </param>
        /// <returns>
        /// The newly inserted HatEntry{T}.
        /// </returns>
        public HatEntry<T> Insert( T item, float weight, int id )
        {
            return this.Insert( item, weight, id, DefaultWeightModifier, false );
        }

        /// <summary>
        /// Inserts a new item into the <see cref="Hat{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The actual item. Can be null.
        /// </param>
        /// <param name="weight">
        /// The weight of the entry. May not be a negative value.
        /// </param>
        /// <param name="id">
        /// The Id of the item. 
        /// Ids can be used to create 'item groups' by applying the same Id
        /// to the items that should make up a group.
        /// </param>
        /// <param name="weightModifier">
        /// A modifier value which is applied to the <paramref name="weight"/> of the item
        /// to create the final weight value. May not be a negative value.
        /// </param>
        /// <param name="hasIndividualModifier">
        /// States whether the <paramref name="weightModifier"/> of the item
        /// is individual. If this value is true the <paramref name="weightModifier"/>
        /// won't be changed by operations which affect items that share the same id.
        /// </param>
        /// <returns>
        /// The newly inserted HatEntry{T}.
        /// </returns>
        public HatEntry<T> Insert( T item, float weight, int id, float weightModifier, bool hasIndividualModifier )
        {
            Contract.Requires<ArgumentException>( weight >= 0.0f );
            Contract.Requires<ArgumentException>( weightModifier >= 0.0f );
            Contract.Ensures( Contract.Result<HatEntry<T>>() != null );

            HatEntry<T> entry = new HatEntry<T>( this, item, id, weight, weightModifier, hasIndividualModifier );

            this.totalWeight += entry.Weight;
            this.items.Add( entry );

            return entry;
        }

        #endregion

        #region - Remove -

        /// <summary>
        /// Removes the given <see cref="HatEntry{T}"/> from the <see cref="Hat{T}"/>.
        /// </summary>
        /// <param name="entry">
        /// The <see cref="HatEntry{T}"/> to remove.
        /// </param>
        /// <returns>
        /// Returns true if it has been removed; 
        /// otherwise false.
        /// </returns>
        public bool RemoveEntry( HatEntry<T> entry )
        {
            if( this.items.Remove( entry ) )
            {
                entry.Owner = null; // Invalidate Entry
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the <see cref="HatEntry{T}"/> at the given <paramref name="index"/>
        /// from the <see cref="Hat{T}"/>.
        /// </summary>
        /// <param name="index">
        /// The index of the <see cref="HatEntry{T}"/> to remove.
        /// </param>
        public void RemoveAt( int index )
        {
            HatEntry<T> entry = this.items[index];
            items.RemoveAt( index );

            entry.Owner = null; // Invalidate Entry
        }

        /// <summary>
        /// Removes the first occurence of given <paramref name="item"/>
        /// from the <see cref="Hat{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The item to remove.
        /// </param>
        /// <returns>
        /// Returns true if it has been removed;
        /// otherwise false.
        /// </returns>
        public bool Remove( T item )
        {
            if( item == null )
            {
                for( int i = 0; i < this.items.Count; ++i )
                {
                    HatEntry<T> entry = this.items[i];

                    if( entry.Data == null )
                    {
                        this.items.RemoveAt( i );
                        entry.Owner = null;
                        return true;
                    }
                }
            }
            else
            {
                for( int i = 0; i < this.items.Count; ++i )
                {
                    HatEntry<T> entry = this.items[i];

                    if( item.Equals( entry.Data ) )
                    {
                        this.items.RemoveAt( i );
                        entry.Owner = null;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the first occurence of given <paramref name="item"/> 
        /// that has the given <paramref name="id"/>
        /// from the <see cref="Hat{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The item to remove.
        /// </param>
        /// <param name="id">
        /// The id the item has to have to be removed.
        /// </param>
        /// <returns>
        /// Returns true if the given <paramref name="item"/> has been removed; 
        /// therwise false.
        /// </returns>
        public bool Remove( T item, int id )
        {
            if( item == null )
            {
                for( int i = 0; i < this.items.Count; ++i )
                {
                    HatEntry<T> entry = this.items[i];

                    if( entry.Data == null && entry.Id == id )
                    {
                        this.items.RemoveAt( i );
                        entry.Owner = null;
                        return true;
                    }
                }
            }
            else
            {
                for( int i = 0; i < this.items.Count; ++i )
                {
                    HatEntry<T> entry = this.items[i];

                    if( item.Equals( entry.Data ) && entry.Id == id )
                    {
                        this.items.RemoveAt( i );
                        entry.Owner = null;
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region - Clear -

        /// <summary>
        /// Removes all items from the <see cref="Hat{T}"/>.
        /// </summary>
        public void Clear()
        {
            this.items.Clear();
            this.totalWeight = 0.0f;
        }

        #endregion

        #endregion

        #region > IEnumerable Members <

        /// <summary>
        /// Receives an enumerates that iterates over the entries in this <see cref="Hat{T}"/>.
        /// </summary>
        /// <returns>The new enumerator.</returns>
        public IEnumerator<HatEntry<T>> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        /// <summary>
        /// Receives an enumerates that iterates over the entries in this <see cref="Hat{T}"/>.
        /// </summary>
        /// <returns>The new enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The total weight of the <see cref="Hat{T}"/>.
        /// </summary>
        private float totalWeight;

        /// <summary>
        /// The items within the <see cref="Hat{T}"/>.
        /// </summary>
        private readonly List<HatEntry<T>> items;

        /// <summary>
        /// The random number generator to use.
        /// </summary>
        private readonly IRand rand;

        #endregion
    }
}
