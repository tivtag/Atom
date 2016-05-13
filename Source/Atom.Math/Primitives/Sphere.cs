// <copyright file="Sphere.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Sphere structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Represents a sphere.
    /// </summary>
    [Serializable]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.SphereConverter ) )]
    public struct Sphere : IEquatable<Sphere>, ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>
        /// The center point of this <see cref="Sphere"/>.
        /// </summary>
        public Vector3 Center;

        /// <summary>
        /// The radius of this <see cref="Sphere"/>.
        /// </summary>
        public float Radius;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere"/> struct.
        /// </summary>
        /// <param name="center">
        /// The center point of the new Sphere.
        /// </param>
        /// <param name="radius">
        /// The radius of the new Sphere.
        /// </param>
        public Sphere( Vector3 center, float radius )
        {
            if( radius < 0.0f )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "radius" );

            this.Center = center;
            this.Radius = radius;
        }

        #endregion

        #region [ Methods ]

        #region Transform

        /// <summary>
        /// Translates and scales this <see cref="Sphere"/> using the given <see cref="Matrix4"/>.
        /// </summary>
        /// <param name="matrix">
        /// A translation and scaling matrix.
        /// </param>
        /// <returns>
        /// The transformed <see cref="Sphere"/>.
        /// </returns>
        public Sphere Transform( Matrix4 matrix )
        {
            float scalingFactor = (matrix.M11 + matrix.M22 + matrix.M33) / 3f;

            return new Sphere(
                Vector3.Transform( this.Center, matrix ),
                this.Radius * scalingFactor 
            );
        }

        /// <summary>
        /// Translates and scales this <see cref="Sphere"/> using the given <see cref="Matrix4"/>.
        /// </summary>
        /// <param name="matrix">
        /// A translation and scaling matrix.
        /// </param>
        /// <param name="result">The transformed <see cref="Sphere"/>.</param>
        public void Transform( ref Matrix4 matrix, out Sphere result )
        {
            float scalingFactor = (matrix.M11 + matrix.M22 + matrix.M33) / 3f;

            result.Center = Vector3.Transform( this.Center, matrix );
            result.Radius = this.Radius * scalingFactor;
        }

        #endregion

        #region > Creation Helpers <

        #region Merge

        /// <summary>
        /// Creates a <see cref="Sphere"/> that contains the two specified <see cref="Sphere"/> instances.
        /// </summary>
        /// <param name="original">
        /// The original <see cref="Sphere"/>.
        /// </param>
        /// <param name="additional">
        /// The <see cref="Sphere"/> to add to the <paramref name="original"/> <see cref="Sphere"/>.
        /// </param>
        /// <returns>
        /// A new Sphere that contains both of the two specified <see cref="Sphere"/>s.
        /// </returns>
        public static Sphere Merge( Sphere original, Sphere additional )
        {
            Vector3 deltaPosition;
            Vector3.Subtract( ref additional.Center, ref original.Center, out deltaPosition );

            float length = deltaPosition.Length;
            if( (original.Radius + additional.Radius) >= length )
            {
                if( (original.Radius - additional.Radius) >= length )
                    return original;

                if( (additional.Radius - original.Radius) >= length )
                    return additional;
            }

            Vector3 normalizedDeltaPosition;
            Vector3.Multiply( ref deltaPosition, 1.0f / length, out normalizedDeltaPosition );
            
            float minRadius =  System.Math.Min( -original.Radius, length - additional.Radius );
            float maxRadius = (System.Math.Max(  original.Radius, length + additional.Radius ) - minRadius) * 0.5f;
            
            Sphere result;

            result.Center = original.Center + (normalizedDeltaPosition * (maxRadius + minRadius));
            result.Radius = maxRadius;

            return result;
        }

        /// <summary>
        /// Creates a <see cref="Sphere"/> that contains the two specified <see cref="Sphere"/> instances.
        /// </summary>
        /// <param name="original">
        /// The original <see cref="Sphere"/>.
        /// </param>
        /// <param name="additional">
        /// The <see cref="Sphere"/> to add to the <paramref name="original"/> <see cref="Sphere"/>.
        /// </param>
        /// <param name="result">
        /// Will contain the Sphere that contains both of the two specified <see cref="Sphere"/>s.
        /// </param>
        public static void Merge( ref Sphere original, ref Sphere additional, out Sphere result )
        {
            Vector3 deltaPosition;
            Vector3.Subtract( ref additional.Center, ref original.Center, out deltaPosition );

            float length = deltaPosition.Length;
            if( (original.Radius + additional.Radius) >= length )
            {
                if( (original.Radius - additional.Radius) >= length )
                {
                    result = original;
                    return;
                }

                if( (additional.Radius - original.Radius) >= length )
                {
                    result = additional;
                    return;
                }
            }

            Vector3 normalizedDeltaPosition;
            Vector3.Multiply( ref deltaPosition, 1.0f / length, out normalizedDeltaPosition );

            float minRadius =  System.Math.Min( -original.Radius, length - additional.Radius );
            float maxRadius = (System.Math.Max( original.Radius, length + additional.Radius ) - minRadius) * 0.5f;

            result.Center = original.Center + (normalizedDeltaPosition * (maxRadius + minRadius));
            result.Radius = maxRadius;
        }

        #endregion

        #region FromPoints

        /// <summary>
        /// Creates a <see cref="Sphere"/> that can contain a specified list of points.
        /// </summary>
        /// <param name="points">
        /// The list of points the <see cref="Sphere"/> must contain.
        /// </param>
        /// <returns>The created <see cref="Sphere"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="points"/> is null.
        /// </exception>
        public static Sphere FromPoints( System.Collections.Generic.IEnumerable<Vector3> points )
        {
            if( points == null )
                throw new ArgumentNullException( "points" );            

            var enumerator = points.GetEnumerator();

            if( !enumerator.MoveNext() )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedEnumerableContainsNoElements, "points" );

            Vector3 minX, minY, minZ;
            Vector3 maxX, maxY, maxZ;

            minX = maxX = minY = maxY = minZ = maxZ = enumerator.Current;            
            foreach( Vector3 vector in points )
            {
                if( vector.X < minX.X )
                    minX = vector;
                if( vector.X > maxX.X )
                    maxX = vector;

                if( vector.Y < minY.Y )
                    minY = vector;
                if( vector.Y > maxY.Y )
                    maxY = vector;

                if( vector.Z < minZ.Z )
                    minZ = vector;
                if( vector.Z > maxZ.Z )
                    maxZ = vector;
            }
            
            float distanceX, distanceY, distanceZ;
            Vector3.Distance( ref maxX, ref minX, out distanceX );
            Vector3.Distance( ref maxY, ref minY, out distanceY );
            Vector3.Distance( ref maxZ, ref minZ, out distanceZ );
            
            float halfDistance;
            Vector3 centerVector;
            if( distanceX > distanceY )
            {
                if( distanceX > distanceZ )
                {
                    Vector3.Lerp( ref maxX, ref minX, 0.5f, out centerVector );
                    halfDistance = distanceX * 0.5f;
                }
                else
                {
                    Vector3.Lerp( ref maxZ, ref minZ, 0.5f, out centerVector );
                    halfDistance = distanceZ * 0.5f;
                }
            }
            else if( distanceY > distanceZ )
            {
                Vector3.Lerp( ref maxY, ref minY, 0.5f, out centerVector );
                halfDistance = distanceY * 0.5f;
            }
            else
            {
                Vector3.Lerp( ref maxZ, ref minZ, 0.5f, out centerVector );
                halfDistance = distanceZ * 0.5f;
            }
            
            foreach( Vector3 point in points )
            {
                Vector3 delta;
                delta.X = point.X - centerVector.X;
                delta.Y = point.Y - centerVector.Y;
                delta.Z = point.Z - centerVector.Z;

                float deltaLength = delta.Length;

                if( deltaLength > halfDistance )
                {
                    halfDistance  = (halfDistance + deltaLength) * 0.5f;
                    centerVector += (1.0f - (halfDistance / deltaLength)) * delta;
                }
            }
            
            Sphere sphere;

            sphere.Center = centerVector;
            sphere.Radius = halfDistance;

            return sphere;
        }

        #endregion

        #region FromBox

        /// <summary>
        /// Creates the smallest Sphere that can contain the specified Box.
        /// </summary>
        /// <param name="box">
        /// The Box to create the Sphere from.
        /// </param>
        /// <returns>
        /// The created Sphere.
        /// </returns>
        public static Sphere FromBox( Box box )
        {
            float distance;
            Vector3.Distance( ref box.Minimum, ref box.Maximum, out distance );

            Sphere sphere;

            Vector3.Lerp( ref box.Minimum, ref box.Maximum, 0.5f, out sphere.Center );
            sphere.Radius = distance * 0.5f;

            return sphere;
        }

        /// <summary>
        /// Creates the smallest Sphere that can contain the specified Box.
        /// </summary>
        /// <param name="box">
        /// The Box to create the Sphere from.
        /// </param>
        /// <param name="result">
        /// Will contain the created Sphere.
        /// </param>
        public static void FromBox( ref Box box, out Sphere result )
        {
            float distance;
            Vector3.Distance( ref box.Minimum, ref box.Maximum, out distance );

            Vector3.Lerp( ref box.Minimum, ref box.Maximum, 0.5f, out result.Center );
            result.Radius = distance * 0.5f;
        }

        #endregion

        #endregion

        #region > Intersection Tests <

        #region Sphere <-> Plane3

        /// <summary>
        /// Checks whether this <see cref="Sphere"/> intersects with the specified <see cref="Plane3"/>.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with the current Sphere.</param>
        /// <returns>An enumeration indicating the relationship between the Sphere and the Plane3. </returns>
        public PlaneIntersectionType Intersects( Plane3 plane )
        {
            return plane.Intersects( this );
        }

        /// <summary>
        /// Checks whether the this <see cref="Sphere"/> intersects with the specified <see cref="Plane3"/>.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with.</param>
        /// <param name="result">An enumeration indicating whether the Sphere intersects the Plane.</param>
        public void Intersects( ref Plane3 plane, out PlaneIntersectionType result )
        {
            plane.Intersects( ref this, out result );
        }

        #endregion

        #region Sphere <-> Ray3

        /// <summary>
        /// Checks whether the this <see cref="Sphere"/> intersects with a specified Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with the current BoundingSphere.</param>
        /// <returns>Distance at which the ray intersects the BoundingSphere or null if there is no intersection.
        /// </returns>
        public float? Intersects( Ray3 ray )
        {
            return ray.Intersects( this );
        }

        /// <summary>
        /// Checks whether this <see cref="Sphere"/> intersects a Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with.</param>
        /// <param name="result">Distance at which the ray intersects the BoundingSphere or null if there is no intersection.</param>
        public void Intersects( ref Ray3 ray, out float? result )
        {
            ray.Intersects( ref this, out result );
        }

        #endregion

        #region Sphere <-> Sphere

        /// <summary>
        /// Checks whether this <see cref="Sphere"/> intersects with a specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="Sphere"/> to check for intersection with this <see cref="Sphere"/>.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the BoundingSpheres intersect; otherwise <see langword="false"/>.
        /// </returns>
        public bool Intersects( Sphere sphere )
        {
            float squaredDistance;
            Vector3.DistanceSquared( ref this.Center, ref sphere.Center, out squaredDistance );

            return (((this.Radius * this.Radius) + ((2f * this.Radius) * sphere.Radius)) + (sphere.Radius * sphere.Radius)) > squaredDistance;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects another BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param>
        /// <param name="result">
        /// Will be <see langword="true"/> if the BoundingSphere instances intersect; otherwise <see langword="false"/>.
        /// </param>
        public void Intersects( ref Sphere sphere, out bool result )
        {
            float squaredDistance;
            Vector3.DistanceSquared( ref this.Center, ref sphere.Center, out squaredDistance );

            result = (((this.Radius * this.Radius) + ((2f * this.Radius) * sphere.Radius)) + (sphere.Radius * sphere.Radius)) > squaredDistance;
        }

        #endregion

        #region Sphere <-> Box

        /// <summary>
        /// Checks whether this Sphere intersects with the specified Box.
        /// </summary>
        /// <param name="box">The Box to check for intersection with the current BoundingSphere.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the Box and Sphere intersect; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Intersects( Box box )
        {
            Vector3 point;
            Vector3.Clamp( ref this.Center, ref box.Minimum, ref box.Maximum, out point );

            float squaredDistance;
            Vector3.DistanceSquared( ref this.Center, ref point, out squaredDistance );

            return squaredDistance <= (this.Radius * this.Radius);
        }

        /// <summary>
        /// Checks whether the current Sphere intersects a Box.
        /// </summary>
        /// <param name="box">
        /// The Box to check for intersection with.
        /// </param>
        /// <param name="result">
        /// Will be <see langword="true"/> if the Box and Sphere intersect; 
        /// otherwise <see langword="false"/>.
        /// </param>
        public void Intersects( ref Box box, out bool result )
        {
            Vector3 point;
            Vector3.Clamp( ref this.Center, ref box.Minimum, ref box.Maximum, out point );

            float squaredDistance;
            Vector3.DistanceSquared( ref this.Center, ref point, out squaredDistance );

            result = squaredDistance <= (this.Radius * this.Radius);
        }

        #endregion

        #endregion

        #region > Containment Tests <

        #region Vector3

        /// <summary>
        /// Checks whether this <see cref="Sphere"/> contains the specified <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point to check against this <see cref="Sphere"/>.</param>
        /// <returns>
        /// An enumeration indicating the relationship of the specified point to the current <see cref="Sphere"/>.
        /// </returns>
        public ContainmentType Contains( Vector3 point )
        {
            if( Vector3.DistanceSquared( point, this.Center ) >= (this.Radius * this.Radius) )
            {
                return ContainmentType.Disjoint;
            }

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Checks whether this <see cref="Sphere"/> contains the specified <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point to check against this <see cref="Sphere"/>.</param>
        /// <param name="result">
        /// An enumeration indicating the relationship of the specified point to the current <see cref="Sphere"/>.
        /// </param>
        public void Contains( ref Vector3 point, out ContainmentType result )
        {
            float squaredDistance;
            Vector3.DistanceSquared( ref point, ref this.Center, out squaredDistance );

            result = (squaredDistance < (this.Radius * this.Radius)) ? ContainmentType.Contains : ContainmentType.Disjoint;
        }

        #endregion

        #region Sphere

        /// <summary>
        /// Checks whether this <see cref="Sphere"/> contains the specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="Sphere"/> to check against this <see cref="Sphere"/>.</param>
        /// <returns>An enumeration indicating the relationship of the Sphere.</returns>
        public ContainmentType Contains( Sphere sphere )
        {
            float distance;
            Vector3.Distance( ref this.Center, ref sphere.Center, out distance );

            if( (this.Radius + sphere.Radius) < distance )
                return ContainmentType.Disjoint;

            if( (this.Radius - sphere.Radius) < distance )
                return ContainmentType.Intersects;

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Checks whether this <see cref="Sphere"/> contains the specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="Sphere"/> to check against this <see cref="Sphere"/>.</param>
        /// <param name="result">Enumeration indicating the extent of overlap.</param>
        public void Contains( ref Sphere sphere, out ContainmentType result )
        {
            float distance;
            Vector3.Distance( ref this.Center, ref sphere.Center, out distance );

            if( (this.Radius + sphere.Radius) < distance )
                result = ContainmentType.Disjoint;
            else if( (this.Radius - sphere.Radius) < distance )
                result = ContainmentType.Intersects;
            else
                result = ContainmentType.Contains;
        }

        #endregion

        #region Box

        /// <summary>
        /// Checks whether this Sphere contains the specified Box.
        /// </summary>
        /// <param name="box">The Box to check against the  current BoundingSphere.</param>
        /// <returns>
        /// An enumeration indicating the relationship of the specified Box to the current BoundingSphere.
        /// </returns>
        public ContainmentType Contains( Box box )
        {
            Vector3 point;
            if( !box.Intersects( this ) )
                return ContainmentType.Disjoint;

            float squaredRadius = this.Radius * this.Radius;
            point.X = this.Center.X - box.Minimum.X;
            point.Y = this.Center.Y - box.Maximum.Y;
            point.Z = this.Center.Z - box.Maximum.Z;

            if( point.SquaredLength > squaredRadius )
            {
                return ContainmentType.Intersects;
            }
            point.X = this.Center.X - box.Maximum.X;
            point.Y = this.Center.Y - box.Maximum.Y;
            point.Z = this.Center.Z - box.Maximum.Z;
            if( point.SquaredLength > squaredRadius )
            {
                return ContainmentType.Intersects;
            }
            point.X = this.Center.X - box.Maximum.X;
            point.Y = this.Center.Y - box.Minimum.Y;
            point.Z = this.Center.Z - box.Maximum.Z;
            if( point.SquaredLength > squaredRadius )
            {
                return ContainmentType.Intersects;
            }
            point.X = this.Center.X - box.Minimum.X;
            point.Y = this.Center.Y - box.Minimum.Y;
            point.Z = this.Center.Z - box.Maximum.Z;
            if( point.SquaredLength > squaredRadius )
            {
                return ContainmentType.Intersects;
            }
            point.X = this.Center.X - box.Minimum.X;
            point.Y = this.Center.Y - box.Maximum.Y;
            point.Z = this.Center.Z - box.Minimum.Z;
            if( point.SquaredLength > squaredRadius )
            {
                return ContainmentType.Intersects;
            }
            point.X = this.Center.X - box.Maximum.X;
            point.Y = this.Center.Y - box.Maximum.Y;
            point.Z = this.Center.Z - box.Minimum.Z;
            if( point.SquaredLength > squaredRadius )
            {
                return ContainmentType.Intersects;
            }
            point.X = this.Center.X - box.Maximum.X;
            point.Y = this.Center.Y - box.Minimum.Y;
            point.Z = this.Center.Z - box.Minimum.Z;
            if( point.SquaredLength > squaredRadius )
            {
                return ContainmentType.Intersects;
            }
            point.X = this.Center.X - box.Minimum.X;
            point.Y = this.Center.Y - box.Minimum.Y;
            point.Z = this.Center.Z - box.Minimum.Z;
            if( point.SquaredLength > squaredRadius )
            {
                return ContainmentType.Intersects;
            }
            return ContainmentType.Contains;
        }

        /// <summary>
        /// Checks whether this Sphere contains the specified Box.
        /// </summary>
        /// <param name="box">The Box to test for overlap.</param>
        /// <param name="result">Enumeration indicating the extent of overlap.</param>
        public void Contains( ref Box box, out ContainmentType result )
        {
            bool isInside;
            box.Intersects( ref this, out isInside );

            if( !isInside )
            {
                result = ContainmentType.Disjoint;
            }
            else
            {
                float squaredRadius = this.Radius * this.Radius;
                result = ContainmentType.Intersects;

                Vector3 point;
                point.X = this.Center.X - box.Minimum.X;
                point.Y = this.Center.Y - box.Maximum.Y;
                point.Z = this.Center.Z - box.Maximum.Z;

                if( point.SquaredLength <= squaredRadius )
                {
                    point.X = this.Center.X - box.Maximum.X;
                    point.Y = this.Center.Y - box.Maximum.Y;
                    point.Z = this.Center.Z - box.Maximum.Z;

                    if( point.SquaredLength <= squaredRadius )
                    {
                        point.X = this.Center.X - box.Maximum.X;
                        point.Y = this.Center.Y - box.Minimum.Y;
                        point.Z = this.Center.Z - box.Maximum.Z;

                        if( point.SquaredLength <= squaredRadius )
                        {
                            point.X = this.Center.X - box.Minimum.X;
                            point.Y = this.Center.Y - box.Minimum.Y;
                            point.Z = this.Center.Z - box.Maximum.Z;

                            if( point.SquaredLength <= squaredRadius )
                            {
                                point.X = this.Center.X - box.Minimum.X;
                                point.Y = this.Center.Y - box.Maximum.Y;
                                point.Z = this.Center.Z - box.Minimum.Z;

                                if( point.SquaredLength <= squaredRadius )
                                {
                                    point.X = this.Center.X - box.Maximum.X;
                                    point.Y = this.Center.Y - box.Maximum.Y;
                                    point.Z = this.Center.Z - box.Minimum.Z;

                                    if( point.SquaredLength <= squaredRadius )
                                    {
                                        point.X = this.Center.X - box.Maximum.X;
                                        point.Y = this.Center.Y - box.Minimum.Y;
                                        point.Z = this.Center.Z - box.Minimum.Z;

                                        if( point.SquaredLength <= squaredRadius )
                                        {
                                            point.X = this.Center.X - box.Minimum.X;
                                            point.Y = this.Center.Y - box.Minimum.Y;
                                            point.Z = this.Center.Z - box.Minimum.Z;

                                            if( point.SquaredLength <= squaredRadius )
                                            {
                                                result = ContainmentType.Contains;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Determines whether the specified System.Object is equal to this <see cref="Sphere"/>.
        /// </summary>
        /// <param name="obj">The System.Object to compare with this Sphere.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified System.Object is equal to this Sphere; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is Sphere )
                return this.Equals( (Sphere)obj );

            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Sphere"/> is equal to this <see cref="Sphere"/>.
        /// </summary>
        /// <param name="other">The Sphere to compare with the this Sphere.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified Sphere is equal to the current Sphere; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( Sphere other )
        {
            return this.Center == other.Center && this.Radius.IsApproximate( other.Radius );
        }

        #endregion

        #region GetHashCode

        /// <summary>Gets the hash code for this instance.</summary>
        /// <returns>
        /// A hash code for the current BoundingSphere.
        /// </returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.Center );
            hashBuilder.AppendStruct( this.Radius );
            
            return hashBuilder.GetHashCode();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a human-readable text representation of the Sphere.
        /// </summary>
        /// <returns>A human-readable text representation of the Sphere.</returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Sphere.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Sphere.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider, 
                "[Center:{0} Radius:{1}]",
                this.Center.ToString( formatProvider ), 
                this.Radius.ToString( formatProvider )
            );
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        /// <summary>
        /// Determines whether two instances of BoundingSphere are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>
        /// Returns <see langword="true"/> if left is equal to right; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==( Sphere left, Sphere right )
        {
            return left.Center == right.Center && left.Radius.IsApproximate( right.Radius );
        }

        /// <summary>
        /// Determines whether two instances of BoundingSphere are not equal.
        /// </summary>
        /// <param name="left">The BoundingSphere to the left of the inequality operator.</param>
        /// <param name="right">The BoundingSphere to the right of the inequality operator.</param>
        /// <returns>
        /// Returns <see langword="true"/> if left is not equal to right; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=( Sphere left, Sphere right )
        {
            return left.Center != right.Center || !left.Radius.IsApproximate( right.Radius );
        }

        #endregion
    }
}

/* 
        /// <summary>Creates the smallest BoundingSphere that can contain a specified BoundingFrustum.  </summary>
        /// <param name="frustum">The BoundingFrustum to create the BoundingSphere with.</param>
        /// <returns>The created BoundingSphere.</returns>
        public static Sphere CreateFromFrustum( BoundingFrustum frustum )
        {
            if( frustum == null )
            {
                throw new ArgumentNullException( "frustum" );
            }
            return CreateFromPoints( frustum.cornerArray );
        }      

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check for intersection with the current BoundingSphere.</param>
        /// <returns>true if the BoundingFrustum and BoundingSphere intersect; false otherwise.
        /// </returns>
        public bool Intersects( BoundingFrustum frustum )
        {
            bool isInside;
            if( null == frustum )
            {
                throw new ArgumentNullException( "frustum", FrameworkResources.NullNotAllowed );
            }
            frustum.Intersects( ref this, out isInside );
            return isInside;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check against the  current BoundingSphere.</param>
        /// <returns>
        /// An enumeration indicating the relationship of the specified BoundingFrustum to the current BoundingSphere.
        /// </returns>
        public ContainmentType Contains( BoundingFrustum frustum )
        {
            if( null == frustum )
            {
                throw new ArgumentNullException( "frustum", FrameworkResources.NullNotAllowed );
            }
            if( !frustum.Intersects( this ) )
            {
                return ContainmentType.Disjoint;
            }
            float num2 = this.Radius * this.Radius;
            foreach( Vector3 vector2 in frustum.cornerArray )
            {
                Vector3 point;
                point.X = vector2.X - this.Center.X;
                point.Y = vector2.Y - this.Center.Y;
                point.Z = vector2.Z - this.Center.Z;
                if( point.LengthSquared() > num2 )
                {
                    return ContainmentType.Intersects;
                }
            }
            return ContainmentType.Contains;
        }
*/