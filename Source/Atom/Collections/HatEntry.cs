// <copyright file="HatEntry.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Collections.HatEntry{T} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Collections
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a single entry in a <see cref="Hat{T}"/>.
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="T"> 
    /// The type of the data stored in the HatEntry.
    /// </typeparam>
    public sealed class HatEntry<T>
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the data stores in the <see cref="HatEntry{T}"/>.
        /// </summary>
        /// <value>
        /// The data this <see cref="HatEntry{T}"/> contains.
        /// </value>
        public T Data
        {
            get
            {
                return this.data;
            }
        }

        /// <summary>
        /// Gets the id associated with this <see cref="HatEntry{T}"/> .
        /// </summary>
        /// <remarks>
        /// Id's don't have to be unique. Non-unique id's can
        /// be used to set the weightModifier of all entries that
        /// share the same id to the same value.
        /// </remarks>
        /// <value>The id of the <see cref="HatEntry{T}"/>.</value>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Gets the original weight of the <see cref="HatEntry{T}"/> ,
        /// before any modifiers.
        /// </summary>
        /// <value>The original weight of the <see cref="HatEntry{T}"/>.</value>
        public float OriginalWeight
        {
            get
            {
                return this.origWeight;
            }
        }

        /// <summary>
        /// Gets the final weight of the <see cref="HatEntry{T}"/>,
        /// after taking into account the current <see cref="WeightModifier"/>.
        /// </summary>
        /// <value>The final weight of the <see cref="HatEntry{T}"/>.</value>
        public float Weight
        {
            get
            {
                return this.weight;
            }
        }

        /// <summary>
        /// Gets or sets the weight modifier value of the <see cref="HatEntry{T}"/> .
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Set: If the given value is negative.
        /// </exception>
        /// <value>The weight modifier of the <see cref="HatEntry{T}"/>.</value>
        public float WeightModifier
        {
            get
            {
                return this.weightModifier;
            }

            set
            {
                Contract.Requires<InvalidOperationException>( this.Owner != null );
                Contract.Requires<ArgumentException>( value >= 0.0f );

                this.weightModifier = value;

                this.Owner.TotalWeight -= this.weight;
                this.weight = this.origWeight * this.weightModifier;
                this.Owner.TotalWeight += this.weight;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="WeightModifier"/> is
        /// individual to all others. 
        /// </summary>
        /// <value>
        /// If true the <see cref="WeightModifier"/> of the <see cref="HatEntry{T}"/> will not be shared 
        /// by entries that have the same <see cref="Id"/>. 
        /// </value>
        public bool HasIndividualModifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="Hat{T}"/> that owns the <see cref="HatEntry{T}"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Hat{T}"/> that owns the <see cref="HatEntry{T}"/>.
        /// </value>
        public Hat<T> Owner
        {
            get
            {
                return this.owner;
            }

            internal set
            {
                this.owner = value;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="HatEntry{T}"/> class.
        /// </summary>
        /// <param name="owner">
        /// The Hat that owns the new HatEntry.
        /// </param>
        /// <param name="data">
        /// The data stored within the new HatEntry.
        /// </param>
        /// <param name="id">
        /// The id of the new HatEntry.
        /// </param>
        /// <param name="originalWeight">
        /// The weight of the new HatEntry, before applying any modifiers.
        /// </param>
        /// <param name="weightModifier">
        /// The modifier applied to the HatEntry's originalWeight.
        /// </param>
        /// <param name="hasIndividualModifier">
        /// States whether the <paramref name="weightModifier"/> of the new HatEntry is individual;
        /// even if the <paramref name="id"/> dictates that it is not.
        /// </param>
        internal HatEntry(
            Hat<T> owner,
            T data,
            int id,
            float originalWeight,
            float weightModifier,
            bool hasIndividualModifier )
        {
            this.Owner                 = owner;
            this.data                  = data;
            this.id                    = id;
            this.origWeight            = originalWeight;
            this.weightModifier        = weightModifier;
            this.weight                = origWeight * weightModifier;

            this.HasIndividualModifier = hasIndividualModifier;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The final weight of the <see cref="HatEntry{T}"/> .
        /// </summary>
        private float weight;

        /// <summary>
        /// The weight modifier value of the <see cref="HatEntry{T}"/> .
        /// </summary>
        private float weightModifier;

        /// <summary>
        /// The <see cref="Hat{T}"/> that owns the <see cref="HatEntry{T}"/> .
        /// </summary>
        private Hat<T> owner;

        /// <summary>
        /// The id associated to the <see cref="HatEntry{T}"/> .
        /// </summary>
        private readonly int id;

        /// <summary>
        /// The data in the <see cref="HatEntry{T}"/> .
        /// </summary>
        private readonly T data;

        /// <summary>
        /// The original weight of the <see cref="HatEntry{T}"/> ,
        /// before any modifiers.
        /// </summary>
        private readonly float origWeight;

        #endregion
    }
}
