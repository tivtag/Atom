// <copyright file="BaseValueModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.BaseValueModifier class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Represents an abstract base implemention of the <see cref="IValueModifier{TInput, TOutput}"/> interface.
    /// </summary>
    /// <typeparam name="TInput">
    /// The type of the input value.
    /// </typeparam>
    /// <typeparam name="TOutput">
    /// The type of the output value.
    /// </typeparam>
    public abstract class BaseValueModifier<TInput, TOutput> : IValueModifier<TInput, TOutput>
    {
        /// <summary>
        /// Raised when this IValueModifier{TInput, TOutput} has been modified in a way that might require
        /// previously modified input values to be re-evualated again.
        /// </summary>
        public event EventHandler Changed;

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
        public abstract TOutput Apply( TInput input );

        /// <summary>
        /// Raises the <see cref="Changed"/> of this BaseValueModifier{TInput, TOutput}.
        /// </summary>
        protected void OnChanged()
        {
            this.Changed.Raise( this );
        }

        /// <summary>
        /// Raises the <see cref="Changed"/> of this BaseValueModifier{TInput, TOutput}.
        /// </summary>
        /// <param name="e">
        /// The EventArgs to pass to the event.
        /// </param>
        protected void OnChanged( EventArgs e )
        {
            this.Changed.Raise( this, e );
        }
    }
}
