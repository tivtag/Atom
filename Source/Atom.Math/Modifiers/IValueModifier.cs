// <copyright file="IValueModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.IValueModifier interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Represents an ecapsulated function that modifies an input value to produce an output value.
    /// </summary>
    /// <typeparam name="TInput">
    /// The type of the input value.
    /// </typeparam>
    /// <typeparam name="TOutput">
    /// The type of the output value.
    /// </typeparam>
    public interface IValueModifier<in TInput, out TOutput>
    {
        /// <summary>
        /// Raised when this IValueModifier{TInput, TOutput} has been modified in a way that might require
        /// previously modified input values to be re-evualated again.
        /// </summary>
        event EventHandler Changed;

        /// <summary>
        /// Applies this IValueModifier{TInput, TOutput} to the specified <paramref name="input"/> value
        /// and returns the resulting value.
        /// </summary>
        /// <param name="input">
        /// The input value.
        /// </param>
        /// <returns>
        /// The output value.
        /// </returns>
        TOutput Apply( TInput input );
    }
}
