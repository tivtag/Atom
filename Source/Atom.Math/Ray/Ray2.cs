// <copyright file="Ray2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Ray2 structure.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary> 
    /// Represents a ray in 2D space.
    /// A ray is a line that has a start-point but no end-point.
    /// </summary>
    /// <remarks>
    /// Any point on the ray can be represented using the
    /// parametic line equation:
    /// p = Origin + t * Distance; where t >= 0.
    /// </remarks>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.Ray2Converter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Ray2 : IEquatable<Ray2>, ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>
        /// The orgin of this <see cref="Ray2"/>.
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// The direction of this <see cref="Ray2"/>.
        /// </summary>
        public Vector2 Direction;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Ray2"/> struct;
        /// using given origin and direction vectors.
        /// </summary>
        /// <param name="origin">
        /// The new Ray2's origin point.
        /// </param>
        /// <param name="direction">
        /// The new Ray2's direction vector.
        /// </param>
        public Ray2( Vector2 origin, Vector2 direction )
        {
            this.Origin    = origin;
            this.Direction = direction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ray2"/> struct;
        /// using the data from the specified <see cref="Ray2"/>.
        /// </summary>
        /// <param name="ray">The <see cref="Ray2"/> instance to copy values from. </param>
        public Ray2( Ray2 ray )
        {
            this.Origin = ray.Origin;
            this.Direction = ray.Direction;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Calculates the distance between this 
        /// <see cref="Ray3"/> and the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">A <see cref="Vector2"/> instance.</param>
        /// <returns>
        /// The distance between this <see cref="Ray3"/> and the given <paramref name="point"/>.
        /// </returns>
        [Pure]
        public float DistanceTo( Vector2 point )
        {
            return (float)System.Math.Sqrt( SquaredDistanceTo( point ) );
        }

        /// <summary>
        /// Calculates the squared distance between this 
        /// <see cref="Ray3"/> and the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">A <see cref="Vector2"/> instance.</param>
        /// <returns>
        /// The squared distance between this <see cref="Ray3"/> and the given <paramref name="point"/>.
        /// </returns>
        [Pure]
        public float SquaredDistanceTo( Vector2 point )
        {
            Vector2 delta;
            delta.X = point.X - this.Origin.X;
            delta.Y = point.Y - this.Origin.Y;

            float time;
            Vector2.Dot( ref delta, ref this.Direction, out time );

            if( time > 0.0f )
            {
                time  /= this.Direction.SquaredLength;
                delta -= time * Direction;
            }

            return delta.SquaredLength;
        }

        /// <summary> 
        /// Gets a point on this <see cref="Ray3"/>. 
        /// </summary>
        /// <param name="time">
        /// The amount to travel on the ray.
        /// </param>
        /// <returns>
        /// A point along the ray.
        /// </returns>
        [Pure]
        public Vector2 GetPointOnRay( float time )
        {
            return this.Origin + (this.Direction * time);
        }

        #region > Overrides / Impls <

        #region GetHashCode

        /// <summary>
        /// Get the hashcode for this <see cref="Ray2"/> instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.Direction );
            hashBuilder.AppendStruct( this.Origin );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #region Equals

        /// <summary>
        /// Returns a value indicating whether this <see cref="Ray2"/> instance is equal to
        /// the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>
        /// Returns <see langword="true"/> if <paramref name="obj"/> is a <see cref="Ray2"/> and
        /// has the same values as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is Ray2 )
                return this.Equals( (Ray2)obj );

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether this <see cref="Ray2"/> is equal to
        /// the specified <see cref="Ray2"/>.
        /// </summary>
        /// <param name="other">The Ray2 instnace to compare to this instance.</param>
        /// <returns>
        /// Returns <see langword="true"/> if <paramref name="other"/> has the same values as this instance;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals( Ray2 other )
        {
            return Origin == other.Origin && Direction == other.Direction;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overriden to return a human-readable text representation of the Ray2.
        /// </summary>
        /// <returns>A human-readable text representation of the Ray2.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Ray2.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Ray2.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "Ray2[ Origin={0}, Direction={1} ]",
                Origin.ToString(), 
                Direction.ToString()
             );
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        /// <summary>
        /// Determines whether the two specified rays are equal.
        /// </summary>
        /// <param name="left">The first of two rays to compare.</param>
        /// <param name="right">The second of two rays to compare.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the two rays are equal; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==( Ray2 left, Ray2 right )
        {
            return left.Origin == right.Origin && left.Direction == right.Direction;
        }

        /// <summary>
        /// Determines whether the two specified rays are not equal.
        /// </summary>
        /// <param name="left">The first of two rays to compare.</param>
        /// <param name="right">The second of two rays to compare.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the two rays are not equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=( Ray2 left, Ray2 right )
        {
            return left.Origin != right.Origin || left.Direction != right.Direction;
        }

        #endregion
    }
}
