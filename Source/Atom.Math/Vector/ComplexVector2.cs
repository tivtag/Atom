// <copyright file="ComplexVector2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.ComplexVector2 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Defines a two dimensional vector whose elements
    /// are <see cref="Complex"/> numbers.
    /// </summary>
    [Serializable]
    public struct ComplexVector2 : IEquatable<ComplexVector2>
    {
        #region [ Fields ]

        /// <summary>
        /// The first component of this <see cref="ComplexVector2"/>.
        /// </summary>
        public Complex X;

        /// <summary>
        /// The second component of this <see cref="ComplexVector2"/>.
        /// </summary>
        public Complex Y;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the complex magnitude of this ComplexVector2.
        /// </summary>
        /// <value>
        /// The <see cref="Complex"/> magnitude of this ComplexVector2.
        /// </value>
        public Complex Magnitude
        {
            get
            {
                return ((this.X * this.X) + (this.Y * this.Y)).SquareRoot;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexVector2"/> structure.
        /// </summary>
        /// <param name="x">
        /// The first component of the new <see cref="ComplexVector2"/>.
        /// </param>
        /// <param name="y">
        /// The second component of the new <see cref="ComplexVector2"/>.
        /// </param>
        public ComplexVector2( Complex x, Complex y )
        {
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Normalizes this ComplexVector2.
        /// </summary>
        public void Normalize()
        {
            Complex mag = this.Magnitude;

            this.X /= mag;
            this.Y /= mag;
        }

        #region > Overwrites / Impls <

        #region Equals

        /// <summary> 
        /// Gets whether the specified <see cref="Object"/> is equal to this <see cref="ComplexVector2"/>.
        /// </summary>
        /// <param name="obj"> The object to test against. </param>
        /// <returns>true if they are equal, otherwise false. </returns>
        public override bool Equals( object obj )
        {
            return (obj is ComplexVector2) && this.Equals( (ComplexVector2)obj );
        }

        /// <summary> 
        /// Gets whether the specified <see cref="ComplexVector2"/> number is equal 
        /// to this <see cref="ComplexVector2"/>. 
        /// </summary>
        /// <param name="other"> The vector to test against. </param>
        /// <returns>true if they are equal, otherwise false. </returns>
        public bool Equals( ComplexVector2 other )
        {
            return this.X.Equals( other.X ) && this.Y.Equals( other.Y );
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a human-readable string representation of this <see cref="ComplexVector2"/>.
        /// </summary>
        /// <returns>A string representation of this <see cref="ComplexVector2"/>.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable string representation of this <see cref="ComplexVector2"/>.
        /// </summary>
        /// <param name="formatProvider">Provides culture specific formatting information.</param>
        /// <returns>A string representation of this <see cref="ComplexVector2"/>.</returns>
        public string ToString( IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider, 
                "[{0} {1}]", 
                this.X.ToString( formatProvider ), 
                this.Y.ToString( formatProvider ) 
            );
        }

        #endregion

        /// <summary> 
        /// Returns the hash code of this <see cref="Complex"/> number.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();
            
            hashBuilder.AppendStruct( this.X );
            hashBuilder.AppendStruct( this.Y );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #endregion

        #region [ Operators ]

        #region > Logic <

        /// <summary>
        /// Returns whether the given <see cref="ComplexVector2"/> instances are equal.
        /// </summary>
        /// <param name="left">
        /// The ComplexVector2 on the left side.</param>
        /// <param name="right">
        /// The ComplexVector2 on the right side.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==( ComplexVector2 left, ComplexVector2 right )
        {
            return left.X == right.X &&
                   left.Y == right.Y;
        }

        /// <summary>
        /// Returns whether the given <see cref="ComplexVector2"/> instances are inequal.
        /// </summary>
        /// <param name="left">The ComplexVector2 on the left side.</param>
        /// <param name="right">The ComplexVector2 on the right side.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=( ComplexVector2 left, ComplexVector2 right )
        {
            return left.X != right.X ||
                   left.Y != right.Y;
        }

        #endregion

        #endregion
    }
}
