// <copyright file="Plane3.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Plane3 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Defines a plane in 3D space.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.Plane3Converter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Plane3 : IEquatable<Plane3>, ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>
        /// The normal point of this Plane.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// The distance of the Plane along its normal from the origin.
        /// </summary>
        public float Distance;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane3"/> struct.
        /// </summary>
        /// <param name="x">X component of the normal defining the Plane.</param>
        /// <param name="y">Y component of the normal defining the Plane.</param>
        /// <param name="z">Z component of the normal defining the Plane.</param>
        /// <param name="distance">Distance of the Plane along its normal from the origin.</param>
        public Plane3( float x, float y, float z, float distance )
        {
            this.Normal.X = x;
            this.Normal.Y = y;
            this.Normal.Z = z;

            this.Distance = distance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane3"/> struct.
        /// </summary>
        /// <param name="normal">
        /// The normal point to the Plane.
        /// </param>
        /// <param name="distance">
        /// The Plane's distance along its normal from the origin.
        /// </param>
        public Plane3( Vector3 normal, float distance )
        {
            this.Normal   = normal;
            this.Distance = distance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane3"/> struct.
        /// </summary>
        /// <param name="value">
        /// A <see cref="Vector4"/> where the X, Y, and Z components define the normal of the new Plane,
        /// and the W component defines the distance of the Plane along the normal from the origin.
        /// </param>
        public Plane3( Vector4 value )
        {
            this.Normal.X = value.X;
            this.Normal.Y = value.Y;
            this.Normal.Z = value.Z;
            this.Distance = value.W;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane3"/> struct.
        /// </summary>
        /// <param name="pointA">
        /// The first point of a triangle defining the Plane.
        /// </param>
        /// <param name="pointB">
        /// The second point of a triangle defining the Plane.
        /// </param>
        /// <param name="pointC">
        /// The third point of a triangle defining the Plane.
        /// </param>
        public Plane3( Vector3 pointA, Vector3 pointB, Vector3 pointC )
        {
            float deltaX1 = pointB.X - pointA.X;
            float deltaY1 = pointB.Y - pointA.Y;
            float deltaZ1 = pointB.Z - pointA.Z;

            float deltaX2 = pointC.X - pointA.X;
            float deltaY2 = pointC.Y - pointA.Y;
            float deltaZ2 = pointC.Z - pointA.Z;

            float crossX = (deltaY1 * deltaZ2) - (deltaZ1 * deltaY2);
            float crossY = (deltaZ1 * deltaX2) - (deltaX1 * deltaZ2);
            float crossZ = (deltaX1 * deltaY2) - (deltaY1 * deltaX2);

            float squaredLength = (crossX * crossX) + (crossY * crossY) + (crossZ * crossZ);
            float invLength     = 1.0f / (float)System.Math.Sqrt( squaredLength );

            this.Normal.X = crossX * invLength;
            this.Normal.Y = crossY * invLength;
            this.Normal.Z = crossZ * invLength;
            this.Distance = -(this.Normal.X * pointA.X) + (this.Normal.Y * pointA.Y) + (this.Normal.Z * pointA.Z);
        }

        #endregion

        #region [ Methods ]

        #region Normalize

        /// <summary>
        /// Changes the coefficients of the Plane.Normal point of this Plane to make it of unit length.
        /// </summary>
        public void Normalize()
        {
            float squaredLength = (this.Normal.X * this.Normal.X) + (this.Normal.Y * this.Normal.Y) + (this.Normal.Z * this.Normal.Z);

            if( System.Math.Abs( squaredLength - 1.0f ) >= 1.192093E-07f )
            {
                float invLength = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

                this.Normal.X *= invLength;
                this.Normal.Y *= invLength;
                this.Normal.Z *= invLength;

                this.Distance *= invLength;
            }
        }

        /// <summary>
        /// Changes the coefficients of the Plane.Normal point of a Plane to make it of unit length.
        /// </summary>
        /// <param name="value">The Plane to normalize.</param>
        /// <returns>A new Plane with a normal having unit length.</returns>
        public static Plane3 Normalize( Plane3 value )
        {
            float squaredLength = (value.Normal.X * value.Normal.X) + (value.Normal.Y * value.Normal.Y) + (value.Normal.Z * value.Normal.Z);

            if( System.Math.Abs( squaredLength - 1.0f ) < 1.192093E-07f )
            {
                return value;
            }

            float invLength = 1.0f / (float)System.Math.Sqrt( squaredLength );

            Plane3 result;

            result.Normal.X = value.Normal.X * invLength;
            result.Normal.Y = value.Normal.Y * invLength;
            result.Normal.Z = value.Normal.Z * invLength;
            result.Distance = value.Distance * invLength;

            return result;
        }

        /// <summary>
        /// Changes the coefficients of the Plane.Normal point of a Plane to make it of unit length.
        /// </summary>
        /// <param name="value">The Plane to normalize.</param>
        /// <param name="result">An existing plane Plane filled in with a normalized version of the specified plane.</param>
        public static void Normalize( ref Plane3 value, out Plane3 result )
        {
            float squaredLength = (value.Normal.X * value.Normal.X) + (value.Normal.Y * value.Normal.Y) + (value.Normal.Z * value.Normal.Z);

            if( System.Math.Abs( squaredLength - 1.0f ) < 1.192093E-07f )
            {
                result.Normal   = value.Normal;
                result.Distance = value.Distance;
            }
            else
            {
                float invLength = 1.0f / (float)System.Math.Sqrt( squaredLength );

                result.Normal.X = value.Normal.X * invLength;
                result.Normal.Y = value.Normal.Y * invLength;
                result.Normal.Z = value.Normal.Z * invLength;
                result.Distance = value.Distance * invLength;
            }
        }

        #endregion

        #region Transform

        #region Transform( Plane3 plane, result matrix )

        /// <summary>
        /// Returns the result of transforming the specified
        /// normalized Plane by the specified <see cref="Matrix4"/>.
        /// </summary>
        /// <param name="plane">The normalized Plane to transform. This Plane must already be normalized, so that its Plane.Normal point is of unit length, before this method is called.</param>
        /// <param name="matrix">The transform Matrix to apply to the Plane.</param>
        /// <returns>
        /// A new Plane that results from applying the transform.
        /// </returns>
        public static Plane3 Transform( Plane3 plane, Matrix4 matrix )
        {
            Matrix4 invMatrix;
            Matrix4.Invert( ref matrix, out invMatrix );

            Plane3 result;

            result.Normal.X = (plane.Normal.X * invMatrix.M11) + (plane.Normal.Y * invMatrix.M12) + (plane.Normal.Z * invMatrix.M13) + (plane.Distance * invMatrix.M14);
            result.Normal.Y = (plane.Normal.X * invMatrix.M21) + (plane.Normal.Y * invMatrix.M22) + (plane.Normal.Z * invMatrix.M23) + (plane.Distance * invMatrix.M24);
            result.Normal.Z = (plane.Normal.X * invMatrix.M31) + (plane.Normal.Y * invMatrix.M32) + (plane.Normal.Z * invMatrix.M33) + (plane.Distance * invMatrix.M34);
            result.Distance = (plane.Normal.X * invMatrix.M41) + (plane.Normal.Y * invMatrix.M42) + (plane.Normal.Z * invMatrix.M43) + (plane.Distance * invMatrix.M44);

            return result;
        }

        /// <summary>
        /// Stores the result of transforming the specified
        /// normalized Plane by the specified <see cref="Matrix4"/>
        /// in the specified <paramref name="result"/> value.
        /// </summary>
        /// <param name="plane">
        /// The normalized Plane to transform. This Plane must already be normalized,
        /// so that its Plane.Normal point is of unit length, before this method is called.
        /// </param>
        /// <param name="matrix">The transform Matrix to apply to the Plane.</param>
        /// <param name="result">An existing Plane filled in with the results of applying the transform.</param>
        public static void Transform( ref Plane3 plane, ref Matrix4 matrix, out Plane3 result )
        {
            Matrix4 invMatrix;
            Matrix4.Invert( ref matrix, out invMatrix );

            result.Normal.X = (plane.Normal.X * invMatrix.M11) + (plane.Normal.Y * invMatrix.M12) + (plane.Normal.Z * invMatrix.M13) + (plane.Distance * invMatrix.M14);
            result.Normal.Y = (plane.Normal.X * invMatrix.M21) + (plane.Normal.Y * invMatrix.M22) + (plane.Normal.Z * invMatrix.M23) + (plane.Distance * invMatrix.M24);
            result.Normal.Z = (plane.Normal.X * invMatrix.M31) + (plane.Normal.Y * invMatrix.M32) + (plane.Normal.Z * invMatrix.M33) + (plane.Distance * invMatrix.M34);
            result.Distance = (plane.Normal.X * invMatrix.M41) + (plane.Normal.Y * invMatrix.M42) + (plane.Normal.Z * invMatrix.M43) + (plane.Distance * invMatrix.M44);
        }

        #endregion

        #region Transform( Plane3 plane, Quaternion rotation )

        /// <summary>
        /// Transforms a normalized Plane by a Quaternion rotation.
        /// </summary>
        /// <param name="plane">The normalized Plane to transform. This Plane must already be normalized, so that its Plane.Normal point is of unit length, before this method is called.</param>
        /// <param name="rotation">The Quaternion rotation to apply to the Plane.</param>
        /// <returns>
        /// A new Plane that results from applying the rotation.
        /// </returns>
        public static Plane3 Transform( Plane3 plane, Quaternion rotation )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotWmul2X = rotation.W * rot2X;
            float rotWmul2Y = rotation.W * rot2Y;
            float rotWmul2Z = rotation.W * rot2Z;

            float rotXmul2X = rotation.X * rot2X;
            float rotXmul2Y = rotation.X * rot2Y;
            float rotXmul2Z = rotation.X * rot2Z;

            float rotYmul2Y = rotation.Y * rot2Y;
            float rotYmul2Z = rotation.Y * rot2Z;
            float rotZmul2Z = rotation.Z * rot2Z;

            float factorXX = (1f - rotYmul2Y) - rotZmul2Z;
            float factorXY = rotXmul2Y + rotWmul2Z;
            float factorXZ = rotXmul2Z - rotWmul2Y;

            float factorYX = rotXmul2Y - rotWmul2Z;
            float factorYY = (1f - rotXmul2X) - rotZmul2Z;
            float factorYZ = rotYmul2Z + rotWmul2X;

            float factorZX = rotXmul2Z + rotWmul2Y;
            float factorZY = rotYmul2Z - rotWmul2X;
            float factorZZ = (1f - rotXmul2X) - rotYmul2Y;

            Plane3 result;

            result.Normal.X = (plane.Normal.X * factorXX) + (plane.Normal.Y * factorYX) + (plane.Normal.Z * factorZX);
            result.Normal.Y = (plane.Normal.X * factorXY) + (plane.Normal.Y * factorYY) + (plane.Normal.Z * factorZY);
            result.Normal.Z = (plane.Normal.X * factorXZ) + (plane.Normal.Y * factorYZ) + (plane.Normal.Z * factorZZ);
            result.Distance = plane.Distance;

            return result;
        }

        /// <summary>
        /// Transforms a normalized Plane by a Quaternion rotation.
        /// </summary>
        /// <param name="plane">The normalized Plane to transform. This Plane must already be normalized, so that its Plane.Normal point is of unit length, before this method is called.</param>
        /// <param name="rotation">The Quaternion rotation to apply to the Plane.</param>
        /// <param name="result">An existing Plane filled in with the results of applying the rotation.</param>
        public static void Transform( ref Plane3 plane, ref Quaternion rotation, out Plane3 result )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotWmul2X = rotation.W * rot2X;
            float rotWmul2Y = rotation.W * rot2Y;
            float rotWmul2Z = rotation.W * rot2Z;

            float rotXmul2X = rotation.X * rot2X;
            float rotXmul2Y = rotation.X * rot2Y;
            float rotXmul2Z = rotation.X * rot2Z;

            float rotYmul2Y = rotation.Y * rot2Y;
            float rotYmul2Z = rotation.Y * rot2Z;
            float rotZmul2Z = rotation.Z * rot2Z;

            float factorXX = (1f - rotYmul2Y) - rotZmul2Z;
            float factorXY = rotXmul2Y + rotWmul2Z;
            float factorXZ = rotXmul2Z - rotWmul2Y;

            float factorYX = rotXmul2Y - rotWmul2Z;
            float factorYY = (1f - rotXmul2X) - rotZmul2Z;
            float factorYZ = rotYmul2Z + rotWmul2X;

            float factorZX = rotXmul2Z + rotWmul2Y;
            float factorZY = rotYmul2Z - rotWmul2X;
            float factorZZ = (1f - rotXmul2X) - rotYmul2Y;

            result.Normal.X = (plane.Normal.X * factorXX) + (plane.Normal.Y * factorYX) + (plane.Normal.Z * factorZX);
            result.Normal.Y = (plane.Normal.X * factorXY) + (plane.Normal.Y * factorYY) + (plane.Normal.Z * factorZY);
            result.Normal.Z = (plane.Normal.X * factorXZ) + (plane.Normal.Y * factorYZ) + (plane.Normal.Z * factorZZ);
            result.Distance = plane.Distance;
        }

        #endregion

        #endregion

        #region Dot

        #region Dot

        /// <summary>
        /// Calculates the dot product of a specified Vector4 and this Plane.
        /// </summary>
        /// <param name="value">The Vector4 to multiply this Plane by.</param>
        /// <returns>
        /// The dot product of the specified Vector4 and this Plane.
        /// </returns>
        public float Dot( Vector4 value )
        {
            return (this.Normal.X * value.X) + (this.Normal.Y * value.Y) + (this.Normal.Z * value.Z) + (this.Distance * value.W);
        }

        /// <summary>
        /// Calculates the dot product of a specified Vector4 and this Plane.
        /// </summary>
        /// <param name="value">The Vector4 to multiply this Plane by.</param>
        /// <param name="result">The dot product of the specified Vector4 and this Plane.</param>
        public void Dot( ref Vector4 value, out float result )
        {
            result = (this.Normal.X * value.X) + (this.Normal.Y * value.Y) + (this.Normal.Z * value.Z) + (this.Distance * value.W);
        }

        #endregion

        #region DotCoordinate

        /// <summary>
        /// Returns the dot product of a specified Vector3 and the Plane.Normal point of this
        /// Plane plus the Plane.Distance value of this Plane.
        /// </summary>
        /// <param name="value">The Vector3 to multiply by.</param>
        /// <returns>The resulting value.</returns>
        public float DotCoordinate( Vector3 value )
        {
            return (this.Normal.X * value.X) + (this.Normal.Y * value.Y) + (this.Normal.Z * value.Z) + this.Distance;
        }

        /// <summary>
        /// Returns the dot product of a specified Vector3 and the Plane.Normal point of this
        /// Plane plus the Plane.Distance value of this Plane.
        /// </summary>
        /// <param name="value">The Vector3 to multiply by.</param>
        /// <param name="result">The resulting value.</param>
        public void DotCoordinate( ref Vector3 value, out float result )
        {
            result = (this.Normal.X * value.X) + (this.Normal.Y * value.Y) + (this.Normal.Z * value.Z) + this.Distance;
        }

        #endregion

        #region DotNormal

        /// <summary>
        /// Returns the dot product of a specified Vector3
        /// and the Plane.Normal point of this Plane.
        /// </summary>
        /// <param name="value">The Vector3 to multiply by.</param>
        /// <returns>The resulting dot product.</returns>
        public float DotNormal( Vector3 value )
        {
            return (this.Normal.X * value.X) + (this.Normal.Y * value.Y) + (this.Normal.Z * value.Z);
        }

        /// <summary>
        /// Returns the dot product of a specified Vector3 
        /// and the Plane.Normal point of this Plane.
        /// </summary>
        /// <param name="value">The Vector3 to multiply by.</param>
        /// <param name="result">The resulting dot product.</param>
        public void DotNormal( ref Vector3 value, out float result )
        {
            result = (this.Normal.X * value.X) + (this.Normal.Y * value.Y) + (this.Normal.Z * value.Z);
        }

        #endregion

        #endregion

        #region > Intersection Tests <

        #region Plane3 <-> Sphere

        /// <summary>
        /// Checks whether this <see cref="Plane3"/> intersects the specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="Sphere"/> to check for intersection with.</param>
        /// <returns>An enumeration indicating the relationship between the Plane and the Sphere.</returns>
        public PlaneIntersectionType Intersects( Sphere sphere )
        {
            float dot      = (sphere.Center.X * this.Normal.X) + (sphere.Center.Y * this.Normal.Y) + (sphere.Center.Z * this.Normal.Z);
            float distance = dot + this.Distance;

            if( distance > sphere.Radius )
                return PlaneIntersectionType.Front;

            if( distance < -sphere.Radius )
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Checks whether this <see cref="Plane3"/> intersects the specified <see cref="Sphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="Sphere"/> to check for intersection with.</param>
        /// <param name="result">An enumeration indicating whether the Plane intersects the Sphere.</param>
        public void Intersects( ref Sphere sphere, out PlaneIntersectionType result )
        {
            float dot      = (sphere.Center.X * this.Normal.X) + (sphere.Center.Y * this.Normal.Y) + (sphere.Center.Z * this.Normal.Z);
            float distance = dot + this.Distance;
            
            if( distance > sphere.Radius )
            {
                result = PlaneIntersectionType.Front;
            }
            else if( distance < -sphere.Radius )
            {
                result = PlaneIntersectionType.Back;
            }
            else
            {
                result = PlaneIntersectionType.Intersecting;
            }
        }

        #endregion

        #region Plane3 <-> Box

        /// <summary>Checks whether this <see cref="Plane3"/> intersects the specified <see cref="Box"/>.</summary>
        /// <param name="box">The <see cref="Box"/> to test for intersection with.</param>
        /// <returns>An enumeration indicating the relationship between the Plane and the Box.</returns>
        public PlaneIntersectionType Intersects( Box box )
        {
            Vector3 front;
            front.X = (this.Normal.X >= 0f) ? box.Minimum.X : box.Maximum.X;
            front.Y = (this.Normal.Y >= 0f) ? box.Minimum.Y : box.Maximum.Y;
            front.Z = (this.Normal.Z >= 0f) ? box.Minimum.Z : box.Maximum.Z;
            
            float dot = (this.Normal.X * front.X) + (this.Normal.Y * front.Y) + (this.Normal.Z * front.Z);
            if( (dot + this.Distance) > 0.0f )
            {
                return PlaneIntersectionType.Front;
            }

            Vector3 back;
            back.X = (this.Normal.X >= 0f) ? box.Maximum.X : box.Minimum.X;
            back.Y = (this.Normal.Y >= 0f) ? box.Maximum.Y : box.Minimum.Y;
            back.Z = (this.Normal.Z >= 0f) ? box.Maximum.Z : box.Minimum.Z;
            
            dot = (this.Normal.X * back.X) + (this.Normal.Y * back.Y) + (this.Normal.Z * back.Z);
            if( (dot + this.Distance) < 0.0f )
            {
                return PlaneIntersectionType.Back;
            }
            
            return PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Checks whether the current Plane intersects a Box.
        /// </summary>
        /// <param name="box">The Box to check for intersection with.</param>
        /// <param name="result">An enumeration indicating whether the Plane intersects the Box.</param>
        public void Intersects( ref Box box, out PlaneIntersectionType result )
        {
            Vector3 front;
            front.X = (this.Normal.X >= 0f) ? box.Minimum.X : box.Maximum.X;
            front.Y = (this.Normal.Y >= 0f) ? box.Minimum.Y : box.Maximum.Y;
            front.Z = (this.Normal.Z >= 0f) ? box.Minimum.Z : box.Maximum.Z;

            float dot = (this.Normal.X * front.X) + (this.Normal.Y * front.Y) + (this.Normal.Z * front.Z);
            if( (dot + this.Distance) > 0.0f )
            {
                result = PlaneIntersectionType.Front;
                return;
            }

            Vector3 back;
            back.X = (this.Normal.X >= 0f) ? box.Maximum.X : box.Minimum.X;
            back.Y = (this.Normal.Y >= 0f) ? box.Maximum.Y : box.Minimum.Y;
            back.Z = (this.Normal.Z >= 0f) ? box.Maximum.Z : box.Minimum.Z;

            dot = (this.Normal.X * back.X) + (this.Normal.Y * back.Y) + (this.Normal.Z * back.Z);
            if( (dot + this.Distance) < 0.0f )
            {
                result = PlaneIntersectionType.Back;
                return;
            }

            result = PlaneIntersectionType.Intersecting;
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Determines whether the specified <see cref="Plane3"/> is equal to this <see cref="Plane3"/>.
        /// </summary>
        /// <param name="other">The Plane to compare with the current Plane.</param>
        /// <returns>true if the specified Plane is equal to the current Plane; false otherwise.
        /// </returns>
        public bool Equals( Plane3 other )
        {
            return this.Distance.IsApproximate( other.Distance ) &&
                   this.Normal.Equals( other.Normal );
        }

        /// <summary>
        /// Determines whether the specified System.Object is equal to this <see cref="Plane3"/>.
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current Plane.</param>
        /// <returns>true if the specified System.Object is equal to the current Plane; false otherwise.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is Plane3 )
                return this.Equals( (Plane3)obj );

            return false;
        }

        #endregion

        #region GetHashCode

        /// <summary>Gets the hash code of this <see cref="Plane3"/> instance.</summary>
        /// <returns>
        /// A hash code for the current Plane.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Normal.GetHashCode() + this.Distance.GetHashCode();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a human-readable text representation of the Plane3.
        /// </summary>
        /// <returns>A human-readable text representation of the Plane3.</returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Plane3.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Plane3.</returns>
        public string ToString( IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider, 
                "[Normal:{0} D:{1}]",
                this.Normal.ToString( formatProvider ),
                this.Distance.ToString( formatProvider )
            );
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        /// <summary>
        /// Determines whether two instances of Plane are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if left is equal to right; false otherwise.
        /// </returns>
        public static bool operator ==( Plane3 left, Plane3 right )
        {
            return left.Equals( right );
        }

        /// <summary>
        /// Determines whether two instances of Plane are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the inequality operator.</param>
        /// <param name="right">The object to the right of the inequality operator.</param>
        /// <returns>true if left is not equal to right; false otherwise.
        /// </returns>
        public static bool operator !=( Plane3 left, Plane3 right )
        {
            return left.Normal != right.Normal || !left.Distance.IsApproximate( right.Distance );
        }

        #endregion
    }
}
    /*     

    /// <summary>Checks whether the current Plane intersects a specified BoundingFrustum.</summary>
    /// <param name="frustum">The BoundingFrustum to check for intersection with.</param>
    /// <returns>An enumeration indicating the relationship between the Plane and the BoundingFrustum.</returns>
    public PlaneIntersectionType Intersects( BoundingFrustum frustum )
    {
        if( null == frustum )
        {
            throw new ArgumentNullException( "frustum", FrameworkResources.NullNotAllowed );
        }
        return frustum.Intersects( this );
    }    
     */