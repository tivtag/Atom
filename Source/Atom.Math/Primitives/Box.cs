// <copyright file="Box.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Box structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents an axis-aligned box-shaped 3D volume.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.BoxConverter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Box : IEquatable<Box>, ICultureSensitiveToStringProvider
    {
        #region [ Constants ]

        /// <summary>
        /// Specifies the total number of corners (8) in the Box.
        /// </summary>
        public const int CornerCount = 8;

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Gets or sets the minimum point of this <see cref="Box"/>.
        /// </summary>
        public Vector3 Minimum;

        /// <summary>
        /// Gets or sets the maximum point of this <see cref="Box"/>.
        /// </summary>
        public Vector3 Maximum;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets an array of points that make up the corners of the Box.
        /// </summary>
        /// <returns>
        /// An array of Vector3 points that represent the corners of the Box.
        /// </returns>
        public Vector3[] GetCorners()
        {
            return new Vector3[] { 
                new Vector3( this.Minimum.X, this.Maximum.Y, this.Maximum.Z ), 
                new Vector3( this.Maximum.X, this.Maximum.Y, this.Maximum.Z ),
                new Vector3( this.Maximum.X, this.Minimum.Y, this.Maximum.Z ),
                new Vector3( this.Minimum.X, this.Minimum.Y, this.Maximum.Z ),
                new Vector3( this.Minimum.X, this.Maximum.Y, this.Minimum.Z ),
                new Vector3( this.Maximum.X, this.Maximum.Y, this.Minimum.Z ),
                new Vector3( this.Maximum.X, this.Minimum.Y, this.Minimum.Z ), 
                new Vector3( this.Minimum.X, this.Minimum.Y, this.Minimum.Z )
           };
        }

        /// <summary>
        /// Gets the array of points that make up the corners of the Box.
        /// </summary>
        /// <param name="corners">
        /// An existing array of at least 8 Vector3 points where the corners of the Box are written.
        /// </param>
        public void GetCorners( Vector3[] corners )
        {
            Contract.Requires<ArgumentNullException>( corners != null );
            Contract.Requires<ArgumentException>( corners.Length >= 8 );

            corners[0].X = this.Minimum.X;
            corners[0].Y = this.Maximum.Y;
            corners[0].Z = this.Maximum.Z;

            corners[1].X = this.Maximum.X;
            corners[1].Y = this.Maximum.Y;
            corners[1].Z = this.Maximum.Z;

            corners[2].X = this.Maximum.X;
            corners[2].Y = this.Minimum.Y;
            corners[2].Z = this.Maximum.Z;

            corners[3].X = this.Minimum.X;
            corners[3].Y = this.Minimum.Y;
            corners[3].Z = this.Maximum.Z;

            corners[4].X = this.Minimum.X;
            corners[4].Y = this.Maximum.Y;
            corners[4].Z = this.Minimum.Z;

            corners[5].X = this.Maximum.X;
            corners[5].Y = this.Maximum.Y;
            corners[5].Z = this.Minimum.Z;

            corners[6].X = this.Maximum.X;
            corners[6].Y = this.Minimum.Y;
            corners[6].Z = this.Minimum.Z;

            corners[7].X = this.Minimum.X;
            corners[7].Y = this.Minimum.Y;
            corners[7].Z = this.Minimum.Z;
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Box"/> struct.
        /// </summary>
        /// <param name="minimum">The minimum point the Box includes.</param>
        /// <param name="maximum">The maximum point the Box includes.</param>
        public Box( Vector3 minimum, Vector3 maximum )
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        #endregion

        #region [ Methods ]

        #region > Creation Helpers <

        #region Merge

        /// <summary>
        /// Creates the smallest <see cref="Box"/> that contains the two specified <see cref="Box"/> instances.
        /// </summary>
        /// <param name="original">The original Box.</param>
        /// <param name="additional">The Box to merge with the <paramref name="original"/> Box.</param>
        /// <returns>The created Box.</returns>
        public static Box Merge( Box original, Box additional )
        {
            Box box;

            Vector3.Min( ref original.Minimum, ref additional.Minimum, out box.Minimum );
            Vector3.Max( ref original.Maximum, ref additional.Maximum, out box.Maximum );

            return box;
        }

        /// <summary>
        /// Creates the smallest Box that contains the two specified Box instances.
        /// </summary>
        /// <param name="original">The original Box.</param>
        /// <param name="additional">The Box to merge with the <paramref name="original"/> Box.</param>
        /// <param name="result">
        /// Will contain the created Box.
        /// </param>
        public static void Merge( ref Box original, ref Box additional, out Box result )
        {
            Vector3.Min( ref original.Minimum, ref additional.Minimum, out result.Minimum );
            Vector3.Max( ref original.Maximum, ref additional.Maximum, out result.Maximum );
        }

        #endregion

        #region FromSphere

        /// <summary>
        /// Creates the smallest Box that will contain the specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to contain.</param>
        /// <returns>The created Box.</returns>
        public static Box FromSphere( Sphere sphere )
        {
            Box box;

            box.Minimum.X = sphere.Center.X - sphere.Radius;
            box.Minimum.Y = sphere.Center.Y - sphere.Radius;
            box.Minimum.Z = sphere.Center.Z - sphere.Radius;

            box.Maximum.X = sphere.Center.X + sphere.Radius;
            box.Maximum.Y = sphere.Center.Y + sphere.Radius;
            box.Maximum.Z = sphere.Center.Z + sphere.Radius;

            return box;
        }

        /// <summary>
        /// Creates the smallest Box that will contain the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to contain.</param>
        /// <param name="result">The created Box.</param>
        public static void FromSphere( ref Sphere sphere, out Box result )
        {
            result.Minimum.X = sphere.Center.X - sphere.Radius;
            result.Minimum.Y = sphere.Center.Y - sphere.Radius;
            result.Minimum.Z = sphere.Center.Z - sphere.Radius;

            result.Maximum.X = sphere.Center.X + sphere.Radius;
            result.Maximum.Y = sphere.Center.Y + sphere.Radius;
            result.Maximum.Z = sphere.Center.Z + sphere.Radius;
        }

        #endregion

        #region FromPoints

        /// <summary>Creates the smallest Box that will contain a group of points.</summary>
        /// <param name="points">A list of points the Box should contain.</param>
        /// <returns>The created Box.</returns>
        public static Box FromPoints( System.Collections.Generic.IEnumerable<Vector3> points )
        {
            if( points == null )
                throw new ArgumentNullException( "points" );

            Vector3 min = new Vector3( float.MaxValue, float.MaxValue, float.MaxValue );
            Vector3 max = new Vector3( float.MinValue, float.MinValue, float.MinValue );
            bool hasPoints = false;

            foreach( Vector3 vector in points )
            {
                min = Vector3.Min( min, vector );
                max = Vector3.Max( max, vector );

                hasPoints = true;
            }

            if( !hasPoints )
                throw new ArgumentException( MathErrorStrings.OperationRequiresAtleastOnePoint, "points" );

            return new Box( min, max );
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether the specified <see cref="Object"/> is equal to this <see cref="Box"/>.
        /// </summary>
        /// <param name="obj">The System.Object to compare with this Box.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified System.Object is equal to this Box; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is Box )
                return this.Equals( (Box)obj );

            return false;
        }

        /// <summary>
        /// Returns whether the specified <see cref="Box"/> is equal to this <see cref="Box"/>.
        /// </summary>
        /// <param name="other">The Box to compare with this Box.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified Box is equal to this Box; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( Box other )
        {
            return this.Minimum == other.Minimum && this.Maximum == other.Maximum;
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets the hash code for this <see cref="Box"/>.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return this.Minimum.GetHashCode() + this.Maximum.GetHashCode();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a human-readable text representation of the Box.
        /// </summary>
        /// <returns>A human-readable text representation of the Box.</returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Box.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Box.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format(
                formatProvider, 
                "[Min:{0} Max:{1}]", 
                this.Minimum.ToString(), 
                this.Maximum.ToString()
            );
        }

        #endregion

        #endregion

        #region > Intersection Tests <

        #region Box <-> Box

        /// <summary>
        /// Checks whether this <see cref="Box"/> intersects the specified <see cref="Box"/>.
        /// </summary>
        /// <param name="box">The Box to check for intersection with.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the Box instances intersect; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool Intersects( Box box )
        {
            if( (this.Maximum.X < box.Minimum.X) || (this.Minimum.X > box.Maximum.X) )
                return false;

            if( (this.Maximum.Y < box.Minimum.Y) || (this.Minimum.Y > box.Maximum.Y) )
                return false;

            return (this.Maximum.Z >= box.Minimum.Z) && (this.Minimum.Z <= box.Maximum.Z);
        }

        /// <summary>
        /// Checks whether this <see cref="Box"/> intersects the specified <see cref="Box"/>.
        /// </summary>
        /// <param name="box">The Box to check for intersection with.</param>
        /// <param name="result">
        /// Will be <see langword="true"/> if the Box instances intersect; 
        /// otherwise <see langword="false"/>.
        /// </param>
        public void Intersects( ref Box box, out bool result )
        {
            if( (this.Maximum.X >= box.Minimum.X) && (this.Minimum.X <= box.Maximum.X) &&
                (this.Maximum.Y >= box.Minimum.Y) && (this.Minimum.Y <= box.Maximum.Y) &&
                (this.Maximum.Z >= box.Minimum.Z) && (this.Minimum.Z <= box.Maximum.Z) )
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }

        #endregion    

        #region Box <-> Plane3

        /// <summary>
        /// Checks whether this <see cref="Box"/> intersects with the specified <see cref="Plane3"/>.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with.</param>
        /// <returns>An enumeration indicating whether the Box intersects the Plane.</returns>
        [Pure]
        public PlaneIntersectionType Intersects( Plane3 plane )
        {
            Vector3 frontVector;
            frontVector.X = (plane.Normal.X >= 0f) ? this.Minimum.X : this.Maximum.X;
            frontVector.Y = (plane.Normal.Y >= 0f) ? this.Minimum.Y : this.Maximum.Y;
            frontVector.Z = (plane.Normal.Z >= 0f) ? this.Minimum.Z : this.Maximum.Z;

            float dot = (plane.Normal.X * frontVector.X) + (plane.Normal.Y * frontVector.Y) + (plane.Normal.Z * frontVector.Z);
            if( (dot + plane.Distance) > 0.0f )
            {
                return PlaneIntersectionType.Front;
            }

            Vector3 backVector;
            backVector.X  = (plane.Normal.X >= 0f) ? this.Maximum.X : this.Minimum.X;
            backVector.Y  = (plane.Normal.Y >= 0f) ? this.Maximum.Y : this.Minimum.Y;
            backVector.Z  = (plane.Normal.Z >= 0f) ? this.Maximum.Z : this.Minimum.Z;

            dot = (plane.Normal.X * backVector.X) + (plane.Normal.Y * backVector.Y) + (plane.Normal.Z * backVector.Z);
            if( (dot + plane.Distance) < 0.0f )
            {
                return PlaneIntersectionType.Back;
            }

            return PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Checks whether this <see cref="Box"/> intersects with the specified <see cref="Plane3"/>.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with.</param>
        /// <param name="result">An enumeration indicating whether the Box intersects the Plane.</param>
        public void Intersects( ref Plane3 plane, out PlaneIntersectionType result )
        {
            Vector3 frontVector;
            frontVector.X = (plane.Normal.X >= 0f) ? this.Minimum.X : this.Maximum.X;
            frontVector.Y = (plane.Normal.Y >= 0f) ? this.Minimum.Y : this.Maximum.Y;
            frontVector.Z = (plane.Normal.Z >= 0f) ? this.Minimum.Z : this.Maximum.Z;

            float dot = (plane.Normal.X * frontVector.X) + (plane.Normal.Y * frontVector.Y) + (plane.Normal.Z * frontVector.Z);
            if( (dot + plane.Distance) > 0.0f )
            {
                result = PlaneIntersectionType.Front;
                return;
            }

            Vector3 backVector;
            backVector.X  = (plane.Normal.X >= 0f) ? this.Maximum.X : this.Minimum.X;
            backVector.Y  = (plane.Normal.Y >= 0f) ? this.Maximum.Y : this.Minimum.Y;
            backVector.Z  = (plane.Normal.Z >= 0f) ? this.Maximum.Z : this.Minimum.Z;

            dot = (plane.Normal.X * backVector.X) + (plane.Normal.Y * backVector.Y) + (plane.Normal.Z * backVector.Z);
            if( (dot + plane.Distance) < 0.0f )
            {
                result = PlaneIntersectionType.Back;
                return;
            }

            result = PlaneIntersectionType.Intersecting;
        }

        #endregion

        #region Box <-> Ray3

        /// <summary>
        /// Checks whether the current Box intersects a Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with.</param>
        /// <returns>
        /// The distance at which the ray intersects the Box, or null if there is no intersection.
        /// </returns>
        [Pure]
        public float? Intersects( Ray3 ray )
        {
            float minValue = 0.0f;
            float maxValue = float.MaxValue;

            // Analyze X
            if( System.Math.Abs( ray.Direction.X ) < 1E-06f )
            {
                if( (ray.Origin.X < this.Minimum.X) || (ray.Origin.X > this.Maximum.X) )
                    return null;
            }
            else
            {
                float invRayDirX = 1.0f / ray.Direction.X;
                float deltaMinX  = (this.Minimum.X - ray.Origin.X) * invRayDirX;
                float deltaMaxX  = (this.Maximum.X - ray.Origin.X) * invRayDirX;

                if( deltaMinX > deltaMaxX )
                {
                    float temp = deltaMinX;
                    deltaMinX  = deltaMaxX;
                    deltaMaxX  = temp;
                }

                minValue = System.Math.Max( deltaMinX, minValue );
                maxValue = System.Math.Min( deltaMaxX, maxValue );

                if( minValue > maxValue )
                    return null;
            }

            // Analyze Y
            if( System.Math.Abs( ray.Direction.Y ) < 1E-06f )
            {
                if( (ray.Origin.Y < this.Minimum.Y) || (ray.Origin.Y > this.Maximum.Y) )
                    return null;
            }
            else
            {
                float invRayDirY = 1f / ray.Direction.Y;
                float deltaMinY  = (this.Minimum.Y - ray.Origin.Y) * invRayDirY;
                float deltaMaxY  = (this.Maximum.Y - ray.Origin.Y) * invRayDirY;

                if( deltaMinY > deltaMaxY )
                {
                    float temp = deltaMinY;
                    deltaMinY  = deltaMaxY;
                    deltaMaxY  = temp;
                }

                minValue = System.Math.Max( deltaMinY, minValue );
                maxValue = System.Math.Min( deltaMaxY, maxValue );
                if( minValue > maxValue )
                    return null;
            }

            // Analyze Z
            if( System.Math.Abs( ray.Direction.Z ) < 1E-06f )
            {
                if( (ray.Origin.Z < this.Minimum.Z) || (ray.Origin.Z > this.Maximum.Z) )
                    return null;
            }
            else
            {
                float invRayDirZ = 1f / ray.Direction.Z;
                float deltaMinZ  = (this.Minimum.Z - ray.Origin.Z) * invRayDirZ;
                float deltaMaxZ  = (this.Maximum.Z - ray.Origin.Z) * invRayDirZ;

                if( deltaMinZ > deltaMaxZ )
                {
                    float temp = deltaMinZ;
                    deltaMinZ = deltaMaxZ;
                    deltaMaxZ = temp;
                }

                minValue = System.Math.Max( deltaMinZ, minValue );
                maxValue = System.Math.Min( deltaMaxZ, maxValue );
                
                if( minValue > maxValue )
                    return null;
            }

            return new float?( minValue );
        }

        /// <summary>
        /// Checks whether the current Box intersects a Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with.</param>
        /// <param name="result">Distance at which the ray intersects the Box, or null if there is no intersection.</param>
        public void Intersects( ref Ray3 ray, out float? result )
        {
            result = null;
            float minValue = 0.0f;
            float maxValue = float.MaxValue;

            // Analyze X
            if( System.Math.Abs( ray.Direction.X ) < 1E-06f )
            {
                if( (ray.Origin.X < this.Minimum.X) || (ray.Origin.X > this.Maximum.X) )
                    return;
            }
            else
            {
                float invRayDirX = 1.0f / ray.Direction.X;
                float deltaMinX  = (this.Minimum.X - ray.Origin.X) * invRayDirX;
                float deltaMaxX  = (this.Maximum.X - ray.Origin.X) * invRayDirX;

                if( deltaMinX > deltaMaxX )
                {
                    float temp = deltaMinX;
                    deltaMinX  = deltaMaxX;
                    deltaMaxX  = temp;
                }

                minValue = System.Math.Max( deltaMinX, minValue );
                maxValue = System.Math.Min( deltaMaxX, maxValue );

                if( minValue > maxValue )
                    return;
            }

            // Analyze Y
            if( System.Math.Abs( ray.Direction.Y ) < 1E-06f )
            {
                if( (ray.Origin.Y < this.Minimum.Y) || (ray.Origin.Y > this.Maximum.Y) )
                    return;
            }
            else
            {
                float invRayDirY = 1f / ray.Direction.Y;
                float deltaMinY  = (this.Minimum.Y - ray.Origin.Y) * invRayDirY;
                float deltaMaxY  = (this.Maximum.Y - ray.Origin.Y) * invRayDirY;

                if( deltaMinY > deltaMaxY )
                {
                    float temp = deltaMinY;
                    deltaMinY  = deltaMaxY;
                    deltaMaxY  = temp;
                }

                minValue = System.Math.Max( deltaMinY, minValue );
                maxValue = System.Math.Min( deltaMaxY, maxValue );
                if( minValue > maxValue )
                    return;
            }

            // Analyze Z
            if( System.Math.Abs( ray.Direction.Z ) < 1E-06f )
            {
                if( (ray.Origin.Z < this.Minimum.Z) || (ray.Origin.Z > this.Maximum.Z) )
                    return;
            }
            else
            {
                float invRayDirZ = 1f / ray.Direction.Z;
                float deltaMinZ  = (this.Minimum.Z - ray.Origin.Z) * invRayDirZ;
                float deltaMaxZ  = (this.Maximum.Z - ray.Origin.Z) * invRayDirZ;

                if( deltaMinZ > deltaMaxZ )
                {
                    float temp = deltaMinZ;
                    deltaMinZ = deltaMaxZ;
                    deltaMaxZ = temp;
                }

                minValue = System.Math.Max( deltaMinZ, minValue );
                maxValue = System.Math.Min( deltaMaxZ, maxValue );

                if( minValue > maxValue )
                    return;
            }

            result = new float?( minValue );
        }

        #endregion

        #region Box <-> Sphere

        /// <summary>
        /// Checks whether this <see cref="Box"/> intersects the specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">The Sphere to check for intersection with.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the Box and the Sphere intersect; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool Intersects( Sphere sphere )
        {
            Vector3 vector;
            Vector3.Clamp( ref sphere.Center, ref this.Minimum, ref this.Maximum, out vector );

            float squaredDistance;
            Vector3.DistanceSquared( ref sphere.Center, ref vector, out squaredDistance );

            return squaredDistance <= (sphere.Radius * sphere.Radius);
        }

        /// <summary>
        /// Checks whether this <see cref="Box"/> intersects the specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">The Sphere to check for intersection with.</param>     
        /// <param name="result">
        /// Will contain <see langword="true"/> if the Box and the Sphere intersect; 
        /// otherwise <see langword="false"/>.
        /// </param>
        public void Intersects( ref Sphere sphere, out bool result )
        {
            Vector3 vector;
            Vector3.Clamp( ref sphere.Center, ref this.Minimum, ref this.Maximum, out vector );

            float squaredDistance;
            Vector3.DistanceSquared( ref sphere.Center, ref vector, out squaredDistance );

            result = squaredDistance <= (sphere.Radius * sphere.Radius);
        }

        #endregion

        #endregion

        #region > Containment Tests <

        #region Box

        /// <summary>Tests whether the Box contains another Box.</summary>
        /// <param name="box">The Box to test for overlap.</param>
        /// <returns>Enumeration indicating the extent of overlap.</returns>
        [Pure]
        public ContainmentType Contains( Box box )
        {
            if( ((this.Maximum.X < box.Minimum.X) || (this.Minimum.X > box.Maximum.X)) ||
                ((this.Maximum.Y < box.Minimum.Y) || (this.Minimum.Y > box.Maximum.Y)) ||
                ((this.Maximum.Z < box.Minimum.Z) || (this.Minimum.Z > box.Maximum.Z)) )
            {
                return ContainmentType.Disjoint;
            }

            if( (this.Minimum.X <= box.Minimum.X) && (box.Maximum.X <= this.Maximum.X) && 
                (this.Minimum.Y <= box.Minimum.Y) && (box.Maximum.Y <= this.Maximum.Y) && 
                (this.Minimum.Z <= box.Minimum.Z) && (box.Maximum.Z <= this.Maximum.Z) )
            {
                return ContainmentType.Contains;
            }

            return ContainmentType.Intersects;
        }

        /// <summary>
        /// Tests whether the Box contains a Box.
        /// </summary>
        /// <param name="box">The Box to test for overlap.</param>
        /// <param name="result">Enumeration indicating the extent of overlap.</param>
        public void Contains( ref Box box, out ContainmentType result )
        {
            if( ((this.Maximum.X < box.Minimum.X) || (this.Minimum.X > box.Maximum.X)) ||
                ((this.Maximum.Y < box.Minimum.Y) || (this.Minimum.Y > box.Maximum.Y)) ||
                ((this.Maximum.Z < box.Minimum.Z) || (this.Minimum.Z > box.Maximum.Z)) )
            {
                result = ContainmentType.Disjoint;
                return;
            }

            if( (this.Minimum.X <= box.Minimum.X) && (box.Maximum.X <= this.Maximum.X) && 
                (this.Minimum.Y <= box.Minimum.Y) && (box.Maximum.Y <= this.Maximum.Y) && 
                (this.Minimum.Z <= box.Minimum.Z) && (box.Maximum.Z <= this.Maximum.Z) )
            {
                result = ContainmentType.Contains;
                return;
            }

            result = ContainmentType.Intersects;
        }

        #endregion

        #region Point

        /// <summary>Tests whether the Box contains a point.</summary>
        /// <param name="point">The point to test for overlap.</param>
        /// <returns>Enumeration indicating the extent of overlap.</returns>
        [Pure]
        public ContainmentType Contains( Vector3 point )
        {
            if( (this.Minimum.X <= point.X) && (point.X <= this.Maximum.X) &&
                (this.Minimum.Y <= point.Y) && (point.Y <= this.Maximum.Y) && 
                (this.Minimum.Z <= point.Z) && (point.Z <= this.Maximum.Z) )
            {
                return ContainmentType.Contains;
            }
            else
            {
                return ContainmentType.Disjoint;
            }
        }

        /// <summary>
        /// Tests whether the Box contains a point.
        /// </summary>
        /// <param name="point">The point to test for overlap.</param>
        /// <param name="result">Enumeration indicating the extent of overlap.</param>
        public void Contains( ref Vector3 point, out ContainmentType result )
        {
            if( (this.Minimum.X <= point.X) && (point.X <= this.Maximum.X) &&
                (this.Minimum.Y <= point.Y) && (point.Y <= this.Maximum.Y) && 
                (this.Minimum.Z <= point.Z) && (point.Z <= this.Maximum.Z) )
            {
                result = ContainmentType.Contains;
            }
            else
            {
                result = ContainmentType.Disjoint;
            }
        }

        #endregion

        #region Sphere

        /// <summary>
        /// Returns whether this <see cref="Box"/> contains the specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">The Sphere to test for overlap.</param>
        /// <returns>Enumeration indicating the extent of overlap.</returns>
        [Pure]
        public ContainmentType Contains( Sphere sphere )
        {
            float radius = sphere.Radius;

            Vector3 vector;
            Vector3.Clamp( ref sphere.Center, ref this.Minimum, ref this.Maximum, out vector );

            float squaredDistance;
            Vector3.DistanceSquared( ref sphere.Center, ref vector, out squaredDistance );

            if( squaredDistance > (radius * radius) )
            {
                return ContainmentType.Disjoint;
            }
            else if(
                ((this.Minimum.X + radius) <= sphere.Center.X) && (sphere.Center.X <= (this.Maximum.X - radius)) && ((this.Maximum.X - this.Minimum.X) > radius) && 
                ((this.Minimum.Y + radius) <= sphere.Center.Y) && (sphere.Center.Y <= (this.Maximum.Y - radius)) && ((this.Maximum.Y - this.Minimum.Y) > radius) &&
                ((this.Minimum.Z + radius) <= sphere.Center.Z) && (sphere.Center.Z <= (this.Maximum.Z - radius)) && ((this.Maximum.Z - this.Minimum.Z) > radius) )
            {
                return ContainmentType.Contains;
            }
            else
            {
                return ContainmentType.Intersects;
            }
        }

        /// <summary>
        /// Returns whether this <see cref="Box"/> contains the specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">The Sphere to test for overlap.</param>
        /// <param name="result">Enumeration indicating the extent of overlap.</param>
        public void Contains( ref Sphere sphere, out ContainmentType result )
        {
            float radius = sphere.Radius;

            Vector3 vector;
            Vector3.Clamp( ref sphere.Center, ref this.Minimum, ref this.Maximum, out vector );

            float squaredDistance;
            Vector3.DistanceSquared( ref sphere.Center, ref vector, out squaredDistance );

            if( squaredDistance > (radius * radius) )
            {
                result = ContainmentType.Disjoint;
            }
            else if(
                ((this.Minimum.X + radius) <= sphere.Center.X) && (sphere.Center.X <= (this.Maximum.X - radius)) && ((this.Maximum.X - this.Minimum.X) > radius) && 
                ((this.Minimum.Y + radius) <= sphere.Center.Y) && (sphere.Center.Y <= (this.Maximum.Y - radius)) && ((this.Maximum.Y - this.Minimum.Y) > radius) &&
                ((this.Minimum.Z + radius) <= sphere.Center.Z) && (sphere.Center.Z <= (this.Maximum.Z - radius)) && ((this.Maximum.Z - this.Minimum.Z) > radius) )
            {
                result = ContainmentType.Contains;
            }
            else
            {
                result = ContainmentType.Intersects;
            }
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        /// <summary>
        /// Returns whether the specified <see cref="Box"/> instances are equal.
        /// </summary>
        /// <param name="left">The <see cref="Box"/> instance on the left side of the equation.</param>
        /// <param name="right">The <see cref="Box"/> instance on the right side of the equation.</param>
        /// <returns>true if the two Boxes are equal; false otherwise.</returns>
        public static bool operator ==( Box left, Box right )
        {
            return left.Equals( right );
        }

        /// <summary>
        /// Returns whether the specified <see cref="Box"/> instances are not equal.
        /// </summary>
        /// <param name="left">The <see cref="Box"/> instance on the left side of the equation.</param>
        /// <param name="right">The <see cref="Box"/> instance on the right side of the equation.</param>
        /// <returns>true if the two Boxes are not equal; false otherwise.</returns>
        public static bool operator !=( Box left, Box right )
        {
            return left.Minimum != right.Minimum || left.Maximum != right.Maximum;
        }

        #endregion
    }
}
/*
 
        internal void SupportMapping( ref Vector3 v, out Vector3 result )
        {
            result.X = (v.X >= 0f) ? this.Maximum.X : this.Minimum.X;
            result.Y = (v.Y >= 0f) ? this.Maximum.Y : this.Minimum.Y;
            result.Z = (v.Z >= 0f) ? this.Maximum.Z : this.Minimum.Z;
        }
 *     /// <summary>
        /// Checks whether the current Box intersects a BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check for intersection with.</param>
        /// <returns>true if the Box and BoundingFrustum intersect; false otherwise.</returns>
        public bool Intersects( BoundingFrustum frustum )
        {
            if( null == frustum )
            {
                throw new ArgumentNullException( "frustum", FrameworkResources.NullNotAllowed );
            }
            return frustum.Intersects( this );
        }
 * 
 *   /// <summary>Tests whether the Box contains a BoundingFrustum.</summary>
        /// <param name="frustum">The BoundingFrustum to test for overlap.</param>
        /// <returns>Enumeration indicating the extent of overlap.</returns>
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
            foreach( Vector3 point in frustum.cornerArray )
            {
                if( this.Contains( point ) == ContainmentType.Disjoint )
                {
                    return ContainmentType.Intersects;
                }
            }
            return ContainmentType.Contains;
        }

 */