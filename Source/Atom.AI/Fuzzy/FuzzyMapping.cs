// <copyright file="FuzzyMapping.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.AI.Fuzzy.FuzzyMapping{TMappedValue, TInput} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.AI.Fuzzy
{
    /// <summary>
    /// A <see cref="FuzzyMapping{TMappedValue, TInput}"/> represents
    /// a value of a <see cref="FuzzySet{TMappedValue, TInput}"/>.
    /// </summary>
    /// <typeparam name="TMappedValue">
    /// The type of the value the FuzzyMapping maps onto.
    /// </typeparam>
    /// <typeparam name="TInput">
    /// The type of the input expected by the FuzzyMapping's <see cref="MembershipFunction{TInput}"/>.
    /// </typeparam>
    public class FuzzyMapping<TMappedValue, TInput>
    {
        /// <summary>
        /// Gets or sets the value the <see cref="FuzzyMapping{TMappedValue, TInput}"/> is associated to.
        /// </summary>
        /// <value>The value this FuzzyMapping associated to.</value>
        public TMappedValue Value
        { 
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="MembershipFunction{TInput}"/> 
        /// (also called characteristic function)
        /// of the <see cref="FuzzyMapping{MappingT, TInput}"/>.
        /// </summary>
        /// <value>
        /// The <see cref="MembershipFunction{TInput}"/> that is used to calculate the characteristic
        /// of the <see cref="Value"/>.
        /// </value>
        public MembershipFunction<TInput> Function
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FuzzyMapping{MappingT, TInput}"/> class.
        /// </summary>
        /// <param name="value">
        /// The value the FuzzyMapping maps onto.
        /// </param>
        /// <param name="function">
        /// The <see cref="MembershipFunction{TInput}"/> that is used to
        /// calcualte how much a given input value is part of the new FuzzyMapping.
        /// </param>
        public FuzzyMapping( TMappedValue value, MembershipFunction<TInput> function )
        {
            this.Value    = value;
            this.Function = function;
        }
    }
}
