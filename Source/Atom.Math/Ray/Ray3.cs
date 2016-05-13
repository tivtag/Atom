// <copyright file="Ray3.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Ray3 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Defines a ray in 3D space.
    /// A ray is a line that has a start-point but no end-point.
    /// </summary>
    /// <remarks>
    /// Any point on the ray can be represented using the
    /// parametic line equation:
    /// p = Origin + t * Distance; where t >= 0.
    /// </remarks>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.Ray3Converter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Ray3 : IEquatable<Ray3>, ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>
        /// Specifies the starting point of this <see cref="Ray3"/>.
        /// </summary>
        public Vector3 Origin;

        /// <summary>
        /// Unit point specifying the direction this <see cref="Ray3"/> is pointing.
        /// </summary>
        public Vector3 Direction;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Ray3"/> struct.
        /// </summary>
        /// <param name="origin">
        /// The starting point of the new Ray3.
        /// </param>
        /// <param name="direction">
        /// Unit point describing the direction of the new Ray3.
        /// </param>
        public Ray3( Vector3 origin, Vector3 direction )
        {
            this.Origin    = origin;
            this.Direction = direction;
        }

        #endregion

        #region [ Methods ]

        #region > Intersection Tests <

        #region Ray3 <-> Plane3

        /// <summary>
        /// Determines whether this Ray3 intersects a specified Plane.
        /// </summary>
        /// <param name="plane">The Plane with which to calculate this Ray3's intersection.</param>
        /// <returns>
        /// The distance at which this Ray3 intersects the specified Plane, or null if there is no intersection.
        /// </returns>
        [Pure]
        public float? Intersects( Plane3 plane )
        {
            float dotDir = (plane.Normal.X * this.Direction.X) + (plane.Normal.Y * this.Direction.Y) + (plane.Normal.Z * this.Direction.Z);
            
            if( System.Math.Abs( dotDir ) < 1E-05f )
                return null;

            float dotPos   = (plane.Normal.X * this.Origin.X) + (plane.Normal.Y * this.Origin.Y) + (plane.Normal.Z * this.Origin.Z);
            float distance = (-plane.Distance - dotPos) / dotDir;
            
            if( distance < 0.0f )
            {
                if( distance < -1E-05f )
                    return null;

                distance = 0f;
            }

            return new float?( distance );
        }

        /// <summary>
        /// Determines whether this Ray3 intersects a specified Plane.
        /// </summary>
        /// <param name="plane">The Plane with which to calculate this Ray3's intersection.</param>
        /// <param name="result">The distance at which this Ray3 intersects the specified Plane, or null if there is no intersection.</param>
        public void Intersects( ref Plane3 plane, out float? result )
        {
            float dotDir = (plane.Normal.X * this.Direction.X) + (plane.Normal.Y * this.Direction.Y) + (plane.Normal.Z * this.Direction.Z);

            if( System.Math.Abs( dotDir ) < 1E-05f )
            {
                result = null;
                return;
            }

            float dotPos   = (plane.Normal.X * this.Origin.X) + (plane.Normal.Y * this.Origin.Y) + (plane.Normal.Z * this.Origin.Z);
            float distance = (-plane.Distance - dotPos) / dotDir;

            if( distance < 0.0f )
            {
                if( distance < -1E-05f )
                {
                    result = null;
                    return;
                }

                distance = 0f;
            }

            result = new float?( distance );
        }

        #endregion

        #region Ray3 <-> Sphere

        /// <summary>
        /// Checks whether this <see cref="Ray3"/> intersects the specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">
        /// The Sphere to check for intersection with the Ray3.
        /// </param>
        /// <returns>
        /// The distance at which the Ray3 intersects the Sphere or null if there is no intersection.
        /// </returns>
        [Pure]
        public float? Intersects( Sphere sphere )
        {
            float deltaX = sphere.Center.X - this.Origin.X;
            float deltaY = sphere.Center.Y - this.Origin.Y;
            float deltaZ = sphere.Center.Z - this.Origin.Z;

            float deltaSquaredLength = (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ);
            float squaredRadius      = sphere.Radius * sphere.Radius;

            if( deltaSquaredLength <= squaredRadius )
                return 0.0f;

            float dot = (deltaX * this.Direction.X) + (deltaY * this.Direction.Y) + (deltaZ * this.Direction.Z);
            if( dot < 0.0f )
                return null;

            float squaredDistance = deltaSquaredLength - (dot * dot);
            if( squaredDistance > squaredRadius )
                return null;

            float deltaDistance = (float)System.Math.Sqrt( squaredRadius - squaredDistance );
            return new float?( dot - deltaDistance );
        }

        /// <summary>
        /// Checks whether this <see cref="Ray3"/> intersects the specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">
        /// The Sphere to check for intersection with the Ray3.
        /// </param>
        /// <param name="result">
        /// Will contain the distance at which the Ray3 intersects the Sphere or null if there is no intersection.
        /// </param>
        public void Intersects( ref Sphere sphere, out float? result )
        {
            float deltaX = sphere.Center.X - this.Origin.X;
            float deltaY = sphere.Center.Y - this.Origin.Y;
            float deltaZ = sphere.Center.Z - this.Origin.Z;

            float deltaSquaredLength = (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ);
            float squaredRadius      = sphere.Radius * sphere.Radius;

            if( deltaSquaredLength <= squaredRadius )
            {
                result = 0.0f;
                return;
            }

            float dot = (deltaX * this.Direction.X) + (deltaY * this.Direction.Y) + (deltaZ * this.Direction.Z);
            if( dot < 0.0f )
            {
                result = null;
                return;
            }

            float squaredDistance = deltaSquaredLength - (dot * dot);
            if( squaredDistance > squaredRadius )
            {
                result = null;
                return;
            }

            float deltaDistance = (float)System.Math.Sqrt( squaredRadius - squaredDistance );
            result = new float?( dot - deltaDistance );
        }

        #endregion

        #region Ray3 <-> Box

        /// <summary>
        /// Checks whether this <see cref="Ray3"/> intersects a specified Box.
        /// </summary>
        /// <param name="box">The Box to check for intersection with the Ray3.</param>
        /// <returns>
        /// The distance at which the Ray3 intersects the Box or null if there is no intersection.
        /// </returns>
        [Pure]
        public float? Intersects( Box box )
        {
            return box.Intersects( this );
        }

        /// <summary>
        /// Checks whether this <see cref="Ray3"/> intersects a Box.
        /// </summary>
        /// <param name="box">
        /// The Box to check for intersection with.
        /// </param>
        /// <param name="result">
        /// Will contain the distance at which the Ray3 intersects the <see cref="Box"/> 
        /// or null if there is no intersection.
        /// </param>
        public void Intersects( ref Box box, out float? result )
        {
            box.Intersects( ref this, out result );
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Determines whether two instances of Ray3 are equal.
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current Ray3.</param>
        /// <returns>true if the specified System.Object is equal to the current Ray3; false otherwise.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is Ray3 )
                return this.Equals( (Ray3)obj );

            return false;
        }

        /// <summary>
        /// Determines whether the specified Ray3 is equal to the current Ray3.
        /// </summary>
        /// <param name="other">The Ray3 to compare with the current Ray3.</param>
        /// <returns>true if the specified Ray3 is equal to the current Ray3; false otherwise.
        /// </returns>
        public bool Equals( Ray3 other )
        {
            return this.Origin == other.Origin && this.Direction == other.Direction;
        }

        #endregion

        #region GetHashCode

        /// <summary>Gets the hash code for this instance.</summary>
        /// <returns>
        /// A hash code for the current Ray3.
        /// </returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.Direction );
            hashBuilder.AppendStruct( this.Origin );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a human-readable text representation of the Ray3.
        /// </summary>
        /// <returns>A human-readable text representation of the Ray3.</returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Ray3.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Ray3.</returns>
        public string ToString( IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider, 
                "[Position:{0} Direction:{1}]",
                this.Origin.ToString( formatProvider ),
                this.Direction.ToString( formatProvider )
            );
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        /// <summary>
        /// Determines whether two instances of Ray3 are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>
        /// Returns <see langword="true"/> if left is equal to right;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==( Ray3 left, Ray3 right )
        {
            return left.Origin == right.Origin && left.Direction == right.Direction;
        }

        /// <summary>
        /// Determines whether two instances of Ray3 are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the inequality operator.</param>
        /// <param name="right">The object to the right of the inequality operator.</param>
        /// <returns>
        /// Returns <see langword="true"/> if left is not equal to right; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=( Ray3 left, Ray3 right )
        {
            return left.Origin != right.Origin || left.Direction != right.Direction;
        }

        #endregion
    }
}

/*
/// <summary>
/// Checks whether the Ray3 intersects a specified BoundingFrustum.
/// </summary>
/// <param name="frustum">The BoundingFrustum to check for intersection with the Ray3.</param>
/// <returns>Distance at which the Ray3 intersects the BoundingFrustum or null if there is no intersection.
/// </returns>
public float? Intersects( BoundingFrustum frustum )
{
    if( frustum == null )
    {
        throw new ArgumentNullException( "frustum" );
    }
    return frustum.Intersects( this );
}       
*/