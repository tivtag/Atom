// <copyright file="FuzzySet.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.AI.Fuzzy.FuzzySet{TMappedValue, TInput} class.
// </summary>
// <author>
// Paul Ennemoser (Tick)
// </author>

namespace Atom.AI.Fuzzy
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// A <see cref="FuzzySet{TMappedValue, TInput}"/>
    /// represents a set of <see cref="FuzzyMapping{TMappedValue, TInput}"/>s.
    /// </summary>
    /// <typeparam name="TMappedValue">
    /// The type of the value the FuzzyMappings map onto.
    /// </typeparam>
    /// <typeparam name="TInput">
    /// The type of the input expected by the FuzzyMappings <see cref="MembershipFunction{TInput}"/>s.
    /// </typeparam>
    public class FuzzySet<TMappedValue, TInput>
        : IEnumerable<FuzzyMapping<TMappedValue, TInput>>, IEnumerable<TMappedValue>
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="FuzzySet{TMappedValue, TInput}"/> class.
        /// </summary>
        /// <param name="initialCapacity">
        /// The initial number of elements the new FuzzySet can store without
        /// allocating new memory.
        /// </param>
        public FuzzySet( int initialCapacity )
        {
            this.set = new Dictionary<TMappedValue, FuzzyMapping<TMappedValue, TInput>>( initialCapacity );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Evulates the fuzzy-percentage that tells how much part the given <paramref name="input"/> value
        /// is of the specified <paramref name="mappedValue"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If no FuzzyMapping has been set for to the given <paramref name="mappedValue"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If there exists a FuzzyMapping for to the given <paramref name="mappedValue"/>,
        /// but the MembershipFunction is null.
        /// </exception>
        /// <param name="mappedValue">
        /// The value to identify the percentage for.
        /// </param>
        /// <param name="input">The input value.</param>
        /// <returns>The evulated value.</returns>
        public float Evaluate( TMappedValue mappedValue, TInput input )
        {
            FuzzyMapping<TMappedValue, TInput> mapping;

            if( !this.set.TryGetValue( mappedValue, out mapping ) )
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        AIErrorStrings.ThereIsNoFuzzyMappingX,
                        mappedValue
                    )
                );
            }

            if( mapping.Function == null )
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        AIErrorStrings.ThereIsNoMembershipFunctionSetForFuzzyMappingX,
                        mappedValue.ToString()
                    )
                );
            }

            return mapping.Function( input );
        }

        /// <summary>
        /// Adds the specified <see cref="FuzzyMapping{TMappedValue, TInput}"/>
        /// to this <see cref="FuzzySet{TMappedValue, TInput}"/>.
        /// </summary>
        /// <param name="mapping">
        /// The mapping to add.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the <see cref="FuzzySet{TMappedValue, TInput}"/> already contains a mapping
        /// for the value descriped by the specified <see cref="FuzzyMapping{TMappedValue, TInput}"/>.
        /// </exception>
        public void Add( FuzzyMapping<TMappedValue, TInput> mapping )
        {
            this.set.Add( mapping.Value, mapping );
        }

        /// <summary>
        /// Returns an enumerator that iterates through the
        /// elements of this <see cref="FuzzySet{TMappedValue, TInput}"/>.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<FuzzyMapping<TMappedValue, TInput>> GetEnumerator()
        {
            return this.set.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the
        /// elements of this <see cref="FuzzySet{TMappedValue, TInput}"/>.
        /// </summary>
        /// <returns>An enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the
        /// values this <see cref="FuzzySet{TMappedValue, TInput}"/> onto.
        /// </summary>
        /// <returns>An enumerator.</returns>
        IEnumerator<TMappedValue> IEnumerable<TMappedValue>.GetEnumerator()
        {
            return this.set.Keys.GetEnumerator();
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The dictionary that maps a value to its FuzzyMapping{TMappedValue, TInput}.
        /// </summary>
        private readonly Dictionary<TMappedValue, FuzzyMapping<TMappedValue, TInput>> set;

        #endregion
    }
}
