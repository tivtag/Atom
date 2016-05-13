// <copyright file="MembershipFunction.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.AI.Fuzzy.MembershipFunction{TInput} delegate.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.AI.Fuzzy
{
    /// <summary>
    /// Calculates how much a given <typeparamref name="TInput"/> value
    /// is part of the <see cref="FuzzyMapping{TMappedValue, TInput}"/> specified 
    /// by the <see cref="MembershipFunction{TInput}"/>.
    /// </summary>
    /// <typeparam name="TInput">
    /// The type of input expected by the function.
    /// </typeparam>
    /// <param name="inputValue">
    /// The input value.
    /// </param>
    /// <returns>
    /// A value in the interval [0; 1]
    /// where 0 maps to false (0%),
    ///       1 maps to true  (100%),
    /// and any other value maps to a meaning between true and false (X%).
    /// </returns>
    public delegate float MembershipFunction<TInput>( TInput inputValue );
}
