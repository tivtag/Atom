// <copyright file="LambdaValueModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.LambdaValueModifier class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Modifiers
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents an <see cref="IValueModifier{TInput, TOutput}"/> that uses a lambda function to modify its value.
    /// </summary>
    /// <typeparam name="TInput">
    /// The type of the input value.
    /// </typeparam>
    /// <typeparam name="TOutput">
    /// The type of the output value.
    /// </typeparam>
    public class LambdaValueModifier<TInput, TOutput> : BaseValueModifier<TInput, TOutput>
    {
        /// <summary>
        /// Gets or sets the lambda function that is applied by this LambdaValueModifier{TInput, TOutput}.
        /// </summary>
        public Func<TInput, TOutput> Lambda 
        {
            get
            {
                return this._lambda;
            }

            set
            {
                this._lambda = value;
                this.OnChanged();
            }
        }

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
        public override TOutput Apply( TInput input )
        {
            if( this._lambda == null )
            {
                throw new InvalidOperationException( "Precondition failed: this.Lambda != null" );
            }

            return this._lambda( input );
        }

        /// <summary>
        /// Represents the storage field of the <see cref="Lambda"/> property.
        /// </summary>
        private Func<TInput, TOutput> _lambda;
    }
}
