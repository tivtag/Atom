// <copyright file="LagrangePolynomial.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.LagrangePolynomial class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// The Lagrange Polynom descripes an arabitary function of N points.
    /// </summary>
    public sealed class LagrangePolynomial
    {
        /// <summary>
        /// Gets the control point at the specified index of this LagrangePolynomial.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the point to get.
        /// </param>
        public Vector2 this[int index]
        {
            get
            {
                return this.points[index];
            }
        }

        /// <summary>
        /// Initializes a new instance of the LagrangePolynomial class.
        /// </summary>
        /// <param name="points">
        /// The control points of the LagrangePolynomial.
        /// </param>
        public LagrangePolynomial( IEnumerable<Vector2> points )
        {
            Contract.Requires<ArgumentNullException>( points != null );

            this.points = points.ToArray();
            this.factors = new float[this.points.Length];
        }

        /// <summary>
        /// Descripes the invariant properties of the LagrangePolynomial.
        /// </summary>
        // [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant( this.points != null );
            Contract.Invariant( this.factors != null );
        }

        /// <summary>
        /// Gets the value of this LagrangePolynomial at the given x-position.
        /// </summary>
        /// <param name="x">
        /// The position on the x-axis.
        /// </param>
        /// <returns>
        /// The position on the y-axis.
        /// </returns>
        public float GetY( float x )
        {
            this.CalculateFactors( x );

            float y = 0.0f;

            for( int i = 0; i < this.points.Length; ++i )
            {
                y = y + this.points[i].Y * this.factors[i];
            }

            return y;
        }

        /// <summary>
        /// Updates the <see cref="factors"/> to descripe the function at the given x-position.
        /// </summary>
        /// <param name="x">
        /// The position on the x-axis.
        /// </param>
        private void CalculateFactors( float x )
        {
            int i;
            int n = this.points.Length;
            float O, U;

            for( int k = 0; k < this.points.Length; ++k )
            {
                O = 1;
                U = 1;

                for( i = 0; i <= k - 1; ++i )
                {
                    O = O * (x - this.points[i].X);
                }

                for( i = k + 1; i < n; ++i )
                {
                    O = O * (x - this.points[i].X);
                }

                for( i = 0; i <= k - 1; ++i )
                {
                    U = U *(this.points[k].X - this.points[i].X);
                }

                for( i = k + 1; i < n; ++i )
                {
                    U = U *(this.points[k].X - this.points[i].X);
                }

                this.factors[k] = O / U;
            }
        }

        /// <summary>
        /// The control points that descripe this LagrangePolynomial.
        /// </summary>
        private readonly Vector2[] points;

        /// <summary>
        /// The factors that have been calculated.
        /// </summary>
        private readonly float[] factors;
    }
}