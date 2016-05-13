// <copyright file="Vector4.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Vector4 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Represents a four dimensional single-precision floating-vector Vector.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.Vector4Converter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Vector4 : IEquatable<Vector4>, ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>
        /// The X-coordinate of the Vector.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y-coordinate of the Vector.
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z-coordinate of the Vector.
        /// </summary>
        public float Z;

        /// <summary>
        /// The W-coordinate of the Vector.
        /// </summary>
        public float W;

        #endregion

        #region [ Constants ]

        /// <summary>
        /// Gets a <see cref="Vector4"/> with all its components set to zero.
        /// </summary>
        /// <value>The vector (0, 0, 0, 0).</value>
        public static Vector4 Zero 
        { 
            get { return new Vector4(); } 
        }

        /// <summary>
        /// Gets a <see cref="Vector4"/> with all its components set to one.
        /// </summary>
        /// <value>The vector (1, 1, 1, 1).</value>
        public static Vector4 One 
        {
            get { return new Vector4( 1.0f, 1.0f, 1.0f, 1.0f ); }
        }

        /// <summary>
        /// Gets the unit <see cref="Vector4"/> for the x-axis.
        /// </summary>
        /// <value>The vector (1, 0, 0, 0).</value>
        public static Vector4 UnitX 
        {
            get { return new Vector4( 1.0f, 0.0f, 0.0f, 0.0f ); }
        }

        /// <summary>
        /// Gets the unit <see cref="Vector4"/> for the y-axis.
        /// </summary>
        /// <value>The vector (0, 1, 0, 0).</value>
        public static Vector4 UnitY
        {
            get { return new Vector4( 0.0f, 1.0f, 0.0f, 0.0f ); }
        }

        /// <summary>
        /// Gets the unit <see cref="Vector4"/> for the z-axis.
        /// </summary>
        /// <value>The vector (0, 0, 1, 0).</value>
        public static Vector4 UnitZ
        {
            get { return new Vector4( 0.0f, 0.0f, 1.0f, 0.0f ); }
        }

        /// <summary>
        /// Gets the unit <see cref="Vector4"/> for the w-axis.
        /// </summary>
        /// <value>The vector (0, 0, 0, 1).</value>
        public static Vector4 UnitW 
        {
            get { return new Vector4( 0.0f, 0.0f, 0.0f, 1.0f ); }
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the length of this Vector4.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when trying to set a new length on a Vector with a length of zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when trying to set the length to a negative value.
        /// </exception>
        /// <value>The length (also called magnitude) of the vector.</value>
        [System.Xml.Serialization.XmlIgnore]
        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt( (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W) );
            }

            set
            {
                if( value < 0.0f )
                    throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "value" );

                if( float.IsInfinity( value ) )
                {
                    X = value;
                    Y = value;
                    Z = value;
                    W = value;
                }
                else
                {
                    float length = this.Length;
                    if( length == 0.0f )
                        throw new InvalidOperationException( MathErrorStrings.OperationInvalidOnZeroVector );

                    float ratio = value / length;
                    X *= ratio;
                    Y *= ratio;
                    Z *= ratio;
                    W *= ratio;
                }
            }
        }

        /// <summary>
        /// Gets or sets the squared length of the Vector.
        /// </summary>
        /// <value>The squared length (also called magnitude) of the vector.</value>
        /// <exception cref="InvalidOperationException">
        /// Thrown when trying to set a new length on a Vector with a length of zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when trying to set the length to a negative value.
        /// </exception>
        [System.Xml.Serialization.XmlIgnore]
        public float SquaredLength
        {
            get
            {
                return (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W);
            }

            set
            {
                if( value < 0.0f )
                    throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "value" );

                if( float.IsInfinity( value ) )
                {
                    X = value;
                    Y = value;
                    Z = value;
                    W = value;
                }
                else
                {
                    float squaredLength = this.SquaredLength;
                    if( squaredLength == 0.0f )
                        throw new InvalidOperationException( MathErrorStrings.OperationInvalidOnZeroVector );

                    float ratio = value / squaredLength;
                    X *= ratio;
                    Y *= ratio;
                    Z *= ratio;
                    W *= ratio;
                }
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="x">The X-coordinate of the new Vector.</param>
        /// <param name="y">The Y-coordinate of the new Vector.</param>
        /// <param name="z">The Z-coordinate of the new Vector.</param>
        /// <param name="w">The W-coordinate of the new Vector.</param>
        public Vector4( float x, float y, float z, float w )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="vector">Stores the X, Y and W components of the new Vector.</param>
        /// <param name="w">The W-coordinate of the new Vector.</param>
        public Vector4( Vector3 vector, float w )
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
            this.W = w;
        }

        #endregion

        #region [ Methods ]

        #region Normalize

        /// <summary>
        /// Normalizes the Vector, setting its length to one.
        /// </summary>
        public void Normalize()
        {
            float squaredLength = (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W);
            float invLength = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            this.X *= invLength;
            this.Y *= invLength;
            this.Z *= invLength;
            this.W *= invLength;
        }

        /// <summary>
        /// Returns the result of normalizing the given Vector.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static Vector4 Normalize( Vector4 vector )
        {
            float squaredLength = (vector.X * vector.X) + (vector.Y * vector.Y) + (vector.Z * vector.Z) + (vector.W * vector.W);
            float invLength = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            Vector4 result;

            result.X = vector.X * invLength;
            result.Y = vector.Y * invLength;
            result.Z = vector.Z * invLength;
            result.W = vector.W * invLength;

            return result;
        }

        /// <summary>
        /// Stores the result of normalizing the given Vector in the given <paramref name="result"/> Vector.
        /// </summary>
        /// <param name="vector">The vector to normalize. This value will not be modified by this method.</param>
        /// <param name="result">This value will store the result of the operation.</param>
        public static void Normalize( ref Vector4 vector, out Vector4 result )
        {
            float squaredLength = (vector.X * vector.X) + (vector.Y * vector.Y) + (vector.Z * vector.Z) + (vector.Z * vector.Z);
            float invLength = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            result.X = vector.X * invLength;
            result.Y = vector.Y * invLength;
            result.Z = vector.Z * invLength;
            result.W = vector.W * invLength;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Returns the dot(scalar) product of the given <see cref="Vector4"/>s.
        /// </summary>
        /// <param name="left">The Vector4 on the left side of the equation.</param>
        /// <param name="right">The Vector4 on the right side of the equation.</param>
        /// <returns>The dot product.</returns>
        public static float Dot( Vector4 left, Vector4 right )
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
        }

        /// <summary>
        /// Returns the dot(scalar) product of the given <see cref="Vector4"/>s.
        /// </summary>
        /// <param name="left">The Vector4 on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The Vector4 on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">This value will contain the result of this operation.</param>
        public static void Dot( ref Vector4 left, ref Vector4 right, out float result )
        {
            result = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
        }

        #endregion

        #region Dyadic

        /// <summary>
        /// Returns the tensor/dyadic product (also called outer product) of the given Vectors. </summary>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Tensor_product for more information.
        /// </remarks>
        /// <param name="left">The first input vector.</param>
        /// <param name="right">The second input vector.</param>
        /// <returns> The result of the operation. </returns>
        public static Matrix4 Dyadic( Vector4 left, Vector4 right )
        {
            return new Matrix4(
                left.X * right.X, left.X * right.Y, left.X * right.Z, left.X * right.W,
                left.Y * right.X, left.Y * right.Y, left.Y * right.Z, left.Y * right.W,
                left.Z * right.X, left.Z * right.Y, left.Z * right.Z, left.Z * right.W,
                left.W * right.X, left.W * right.Y, left.W * right.Z, left.W * right.W
            );
        }

        /// <summary>
        /// Returns the tensor/dyadic product (also called outer product) of the given Vectors. </summary>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Tensor_product for more information.
        /// </remarks>
        /// <param name="left">The first input vector. This value will not be modified by this method.</param>
        /// <param name="right">The second input vector. This value will not be modified by this method.</param>
        /// <param name="result"> Will contain the result of the operation. </param>
        public static void Dyadic( ref Vector4 left, ref Vector4 right, out Matrix4 result )
        {
            result = new Matrix4(
                left.X * right.X, left.X * right.Y, left.X * right.Z, left.X * right.W,
                left.Y * right.X, left.Y * right.Y, left.Y * right.Z, left.Y * right.W,
                left.Z * right.X, left.Z * right.Y, left.Z * right.Z, left.Z * right.W,
                left.W * right.X, left.W * right.Y, left.W * right.Z, left.W * right.W
            );
        }

        #endregion

        #region > Transformation <

        #region Vector2 by result

        /// <summary>
        /// Transforms a Vector2 by the given result.
        /// </summary>
        /// <param name="position">The source Vector2.</param>
        /// <param name="matrix">The transformation result.</param>
        /// <returns>The transformed Vector4.</returns>
        public static Vector4 Transform( Vector2 position, Matrix4 matrix )
        {
            Vector4 result;

            result.X = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41;
            result.Y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42;
            result.Z = (position.X * matrix.M13) + (position.Y * matrix.M23) + matrix.M43;
            result.W = (position.X * matrix.M14) + (position.Y * matrix.M24) + matrix.M44;

            return result;
        }

        /// <summary>
        /// Transforms a Vector2 by the given result.
        /// </summary>
        /// <param name="position">The source Vector2.</param>
        /// <param name="matrix">The transformation result.</param>
        /// <param name="result">The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector2 position, ref Matrix4 matrix, out Vector4 result )
        {
            result.X = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41;
            result.Y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42;
            result.Z = (position.X * matrix.M13) + (position.Y * matrix.M23) + matrix.M43;
            result.W = (position.X * matrix.M14) + (position.Y * matrix.M24) + matrix.M44;
        }

        #endregion

        #region Vector3 by Matrix4

        /// <summary>
        /// Transforms a Vector3 by the given result.
        /// </summary>
        /// <param name="position">The source Vector3.</param>
        /// <param name="matrix">The transformation result.</param>
        /// <returns>The Vector4 resulting from the transformation.</returns>
        public static Vector4 Transform( Vector3 position, Matrix4 matrix )
        {
            Vector4 result;

            result.X = (position.X * matrix.M11) + (position.Y * matrix.M21) + (position.Z * matrix.M31) + matrix.M41;
            result.Y = (position.X * matrix.M12) + (position.Y * matrix.M22) + (position.Z * matrix.M32) + matrix.M42;
            result.Z = (position.X * matrix.M13) + (position.Y * matrix.M23) + (position.Z * matrix.M33) + matrix.M43;
            result.W = (position.X * matrix.M14) + (position.Y * matrix.M24) + (position.Z * matrix.M34) + matrix.M44;

            return result;
        }

        /// <summary>
        /// Transforms a Vector3 by the given result.
        /// </summary>
        /// <param name="position">The source Vector3.</param>
        /// <param name="matrix">The transformation result.</param>
        /// <param name="result">The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector3 position, ref Matrix4 matrix, out Vector4 result )
        {
            result.X = (position.X * matrix.M11) + (position.Y * matrix.M21) + (position.Z * matrix.M31) + matrix.M41;
            result.Y = (position.X * matrix.M12) + (position.Y * matrix.M22) + (position.Z * matrix.M32) + matrix.M42;
            result.Z = (position.X * matrix.M13) + (position.Y * matrix.M23) + (position.Z * matrix.M33) + matrix.M43;
            result.W = (position.X * matrix.M14) + (position.Y * matrix.M24) + (position.Z * matrix.M34) + matrix.M44;
        }

        #endregion

        #region Vector4 by Matrix4

        /// <summary>
        /// Transforms a Vector4 by the specified result.
        /// </summary>
        /// <param name="vector">The source Vector4.</param>
        /// <param name="matrix">The transformation result.</param>
        /// <returns>The transformed Vector4.</returns>
        public static Vector4 Transform( Vector4 vector, Matrix4 matrix )
        {
            Vector4 result;

            result.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31) + (vector.W * matrix.M41);
            result.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32) + (vector.W * matrix.M42);
            result.Z = (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33) + (vector.W * matrix.M43);
            result.W = (vector.X * matrix.M14) + (vector.Y * matrix.M24) + (vector.Z * matrix.M34) + (vector.W * matrix.M44);

            return result;
        }

        /// <summary>
        /// Transforms a Vector4 by the given result.
        /// </summary>
        /// <param name="vector">The source Vector4.</param>
        /// <param name="matrix">The transformation result.</param>
        /// <param name="result">The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector4 vector, ref Matrix4 matrix, out Vector4 result )
        {
            result.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31) + (vector.W * matrix.M41);
            result.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32) + (vector.W * matrix.M42);
            result.Z = (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33) + (vector.W * matrix.M43);
            result.W = (vector.X * matrix.M14) + (vector.Y * matrix.M24) + (vector.Z * matrix.M34) + (vector.W * matrix.M44);
        }

        #endregion

        #region Vector2 by Quaternion

        /// <summary>
        /// Transforms a Vector2 by a specified Quaternion into a Vector4.
        /// </summary>
        /// <param name="value">The Vector2 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <returns>
        /// Returns the Vector4 resulting from the transformation.
        /// </returns>
        public static Vector4 Transform( Vector2 value, Quaternion rotation )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2X = rotation.W * rot2X;
            float rotW2Y = rotation.W * rot2Y;
            float rotW2Z = rotation.W * rot2Z;

            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotX2Z = rotation.X * rot2Z;

            float rotY2Y = rotation.Y * rot2Y;
            float rotY2Z = rotation.Y * rot2Z;
            float rotZ2Z = rotation.Z * rot2Z;

            Vector4 result;

            result.X = (value.X * ((1f - rotY2Y) - rotZ2Z)) + (value.Y * (rotX2Y - rotW2Z));
            result.Y = (value.X * (rotX2Y + rotW2Z))        + (value.Y * ((1f - rotX2X) - rotZ2Z));
            result.Z = (value.X * (rotX2Z - rotW2Y))        + (value.Y * (rotY2Z + rotW2X));
            result.W = 1f;

            return result;
        }

        /// <summary>
        /// Transforms a Vector2 by a specified Quaternion into a Vector4.
        /// </summary>
        /// <param name="value">The Vector2 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="result">The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector2 value, ref Quaternion rotation, out Vector4 result )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2X = rotation.W * rot2X;
            float rotW2Y = rotation.W * rot2Y;
            float rotW2Z = rotation.W * rot2Z;

            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotX2Z = rotation.X * rot2Z;

            float rotY2Y = rotation.Y * rot2Y;
            float rotY2Z = rotation.Y * rot2Z;
            float rotZ2Z = rotation.Z * rot2Z;

            result.X = (value.X * ((1f - rotY2Y) - rotZ2Z)) + (value.Y * (rotX2Y - rotW2Z));
            result.Y = (value.X * (rotX2Y + rotW2Z))        + (value.Y * ((1f - rotX2X) - rotZ2Z));
            result.Z = (value.X * (rotX2Z - rotW2Y))        + (value.Y * (rotY2Z + rotW2X));
            result.W = 1f;
        }

        #endregion

        #region Vector3 by Quaternion

        /// <summary>
        /// Transforms a Vector3 by a specified Quaternion into a Vector4.
        /// </summary>
        /// <param name="value">The Vector3 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <returns>The Vector4 resulting from the transformation.</returns>
        public static Vector4 Transform( Vector3 value, Quaternion rotation )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2X = rotation.W * rot2X;
            float rotW2Y = rotation.W * rot2Y;
            float rotW2Z = rotation.W * rot2Z;

            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotX2Z = rotation.X * rot2Z;

            float rotY2Y = rotation.Y * rot2Y;
            float rotY2Z = rotation.Y * rot2Z;
            float rotZ2Z = rotation.Z * rot2Z;

            Vector4 result;

            result.X = (value.X * ((1f - rotY2Y) - rotZ2Z)) + (value.Y * (rotX2Y - rotW2Z))        + (value.Z * (rotX2Z + rotW2Y));
            result.Y = (value.X * (rotX2Y + rotW2Z))        + (value.Y * ((1f - rotX2X) - rotZ2Z)) + (value.Z * (rotY2Z - rotW2X));
            result.Z = (value.X * (rotX2Z - rotW2Y))        + (value.Y * (rotY2Z + rotW2X))        + (value.Z * ((1f - rotX2X) - rotY2Y));
            result.W = 1f;

            return result;
        }

        /// <summary>
        /// Transforms a Vector3 by a specified Quaternion into a Vector4.
        /// </summary>
        /// <param name="value">The Vector3 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="result">The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector3 value, ref Quaternion rotation, out Vector4 result )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2X = rotation.W * rot2X;
            float rotW2Y = rotation.W * rot2Y;
            float rotW2Z = rotation.W * rot2Z;

            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotX2Z = rotation.X * rot2Z;

            float rotY2Y = rotation.Y * rot2Y;
            float rotY2Z = rotation.Y * rot2Z;
            float rotZ2Z = rotation.Z * rot2Z;

            result.X = (value.X * ((1f - rotY2Y) - rotZ2Z)) + (value.Y * (rotX2Y - rotW2Z))        + (value.Z * (rotX2Z + rotW2Y));
            result.Y = (value.X * (rotX2Y + rotW2Z))        + (value.Y * ((1f - rotX2X) - rotZ2Z)) + (value.Z * (rotY2Z - rotW2X));
            result.Z = (value.X * (rotX2Z - rotW2Y))        + (value.Y * (rotY2Z + rotW2X))        + (value.Z * ((1f - rotX2X) - rotY2Y));
            result.W = 1f;
        }

        #endregion

        #region Vector4 by Quaternion

        /// <summary>Transforms a Vector4 by a specified Quaternion.</summary>
        /// <param name="value">The Vector4 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <returns>The Vector4 resulting from the transformation.</returns>
        public static Vector4 Transform( Vector4 value, Quaternion rotation )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2X = rotation.W * rot2X;
            float rotW2Y = rotation.W * rot2Y;
            float rotW2Z = rotation.W * rot2Z;

            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotX2Z = rotation.X * rot2Z;

            float rotY2Y = rotation.Y * rot2Y;
            float rotY2Z = rotation.Y * rot2Z;
            float rotZ2Z = rotation.Z * rot2Z;

            Vector4 result;

            result.X = (value.X * ((1f - rotY2Y) - rotZ2Z)) + (value.Y * (rotX2Y - rotW2Z))        + (value.Z * (rotX2Z + rotW2Y));
            result.Y = (value.X * (rotX2Y + rotW2Z))        + (value.Y * ((1f - rotX2X) - rotZ2Z)) + (value.Z * (rotY2Z - rotW2X));
            result.Z = (value.X * (rotX2Z - rotW2Y))        + (value.Y * (rotY2Z + rotW2X))        + (value.Z * ((1f - rotX2X) - rotY2Y));
            result.W = value.W;

            return result;
        }

        /// <summary>Transforms a Vector4 by a specified Quaternion.</summary>
        /// <param name="value">The Vector4 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="result">The Vector4 resulting from the transformation.</param>
        public static void Transform( ref Vector4 value, ref Quaternion rotation, out Vector4 result )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2X = rotation.W * rot2X;
            float rotW2Y = rotation.W * rot2Y;
            float rotW2Z = rotation.W * rot2Z;

            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotX2Z = rotation.X * rot2Z;

            float rotY2Y = rotation.Y * rot2Y;
            float rotY2Z = rotation.Y * rot2Z;
            float rotZ2Z = rotation.Z * rot2Z;

            result.X = (value.X * ((1f - rotY2Y) - rotZ2Z)) + (value.Y * (rotX2Y - rotW2Z))        + (value.Z * (rotX2Z + rotW2Y));
            result.Y = (value.X * (rotX2Y + rotW2Z))        + (value.Y * ((1f - rotX2X) - rotZ2Z)) + (value.Z * (rotY2Z - rotW2X));
            result.Z = (value.X * (rotX2Z - rotW2Y))        + (value.Y * (rotY2Z + rotW2X))        + (value.Z * ((1f - rotX2X) - rotY2Y));
            result.W = value.W;
        }

        #endregion

        #region Vector4[] by Matrix4

        /// <summary>
        /// Transforms a specified range in an array of Vector4s by a specified result 
        /// into a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The array of Vector4s containing the range to transform.</param>
        /// <param name="sourceIndex">The index in the source array of the first Vector4 to transform.</param>
        /// <param name="matrix">The transform result to apply.</param>
        /// <param name="destinationArray">The existing destination array of Vector4s into which to write the results.</param>
        /// <param name="destinationIndex">The index in the destination array of the first result Vector4 to write.</param>
        /// <param name="length">The number of Vector4s to transform.</param>
        public static void Transform( 
            Vector4[]   sourceArray,      
            int         sourceIndex, 
            ref Matrix4 matrix,
            Vector4[]   destinationArray, 
            int         destinationIndex,
            int         length )
        {
            #region Verify Arguments

            if( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if( length < 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "length" );

            if( sourceIndex < 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "sourceIndex" );

            if( destinationIndex < 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "destinationIndex" );

            if( sourceArray.Length < (sourceIndex + length) )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInSourceArray );

            if( destinationArray.Length < (destinationIndex + length) )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray );

            #endregion

            while( length > 0 )
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;
                float z = sourceArray[sourceIndex].Z;
                float w = sourceArray[sourceIndex].W;

                destinationArray[destinationIndex].X = (x * matrix.M11) + (y * matrix.M21) + (z * matrix.M31) + (w * matrix.M41);
                destinationArray[destinationIndex].Y = (x * matrix.M12) + (y * matrix.M22) + (z * matrix.M32) + (w * matrix.M42);
                destinationArray[destinationIndex].Z = (x * matrix.M13) + (y * matrix.M23) + (z * matrix.M33) + (w * matrix.M43);
                destinationArray[destinationIndex].W = (x * matrix.M14) + (y * matrix.M24) + (z * matrix.M34) + (w * matrix.M44);
                
                ++sourceIndex;
                ++destinationIndex;
                --length;
            }
        }

        /// <summary>
        /// Transforms an array of Vector4s by a specified result.
        /// </summary>
        /// <param name="sourceArray">The array of Vector4s to transform.</param>
        /// <param name="matrix">The transform result to apply.</param>
        /// <param name="destinationArray">The existing destination array into which the transformed Vector4s are written.</param>
        public static void Transform( Vector4[] sourceArray, ref Matrix4 matrix, Vector4[] destinationArray )
        {
            if( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray );

            for( int i = 0; i < sourceArray.Length; i++ )
            {
                float x = sourceArray[i].X;
                float y = sourceArray[i].Y;
                float z = sourceArray[i].Z;
                float w = sourceArray[i].W;
                destinationArray[i].X = (((x * matrix.M11) + (y * matrix.M21)) + (z * matrix.M31)) + (w * matrix.M41);
                destinationArray[i].Y = (((x * matrix.M12) + (y * matrix.M22)) + (z * matrix.M32)) + (w * matrix.M42);
                destinationArray[i].Z = (((x * matrix.M13) + (y * matrix.M23)) + (z * matrix.M33)) + (w * matrix.M43);
                destinationArray[i].W = (((x * matrix.M14) + (y * matrix.M24)) + (z * matrix.M34)) + (w * matrix.M44);
            }
        }

        #endregion

        #region Vector4[] by Quaternion

        /// <summary>Transforms an array of Vector4s by a specified Quaternion.</summary>
        /// <param name="sourceArray">The array of Vector4s to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="destinationArray">The existing destination array into which the transformed Vector4s are written.</param>
        public static void Transform( Vector4[] sourceArray, ref Quaternion rotation, Vector4[] destinationArray )
        {
            #region Verify Arguments

            if( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray );

            #endregion

            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2X = rotation.W * rot2X;
            float rotW2Y = rotation.W * rot2Y;
            float rotW2Z = rotation.W * rot2Z;

            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotX2Z = rotation.X * rot2Z;

            float rotY2Y = rotation.Y * rot2Y;
            float rotY2Z = rotation.Y * rot2Z;
            float rotZ2Z = rotation.Z * rot2Z;

            float factorXX = (1f - rotY2Y) - rotZ2Z;
            float factorXY = rotX2Y + rotW2Z;
            float factorXZ = rotX2Z - rotW2Y;

            float factorYX = rotX2Y - rotW2Z;
            float factorYY = (1f - rotX2X) - rotZ2Z;
            float factorYZ = rotY2Z + rotW2X;

            float factorZX = rotX2Z + rotW2Y;
            float factorZY = rotY2Z - rotW2X;
            float factorZZ = (1f - rotX2X) - rotY2Y;

            for( int i = 0; i < sourceArray.Length; ++i )
            {
                Vector4 vector = sourceArray[i];

                destinationArray[i].X = (vector.X * factorXX) + (vector.Y * factorYX) + (vector.Z * factorZX);
                destinationArray[i].Y = (vector.X * factorXY) + (vector.Y * factorYY) + (vector.Z * factorZY);
                destinationArray[i].Z = (vector.X * factorXZ) + (vector.Y * factorYZ) + (vector.Z * factorZZ);
                destinationArray[i].W = vector.W;
            }
        }

        /// <summary>
        /// Transforms a specified range in an array of Vector4s by a specified Quaternion
        /// into a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The array of Vector4s containing the range to transform.</param>
        /// <param name="sourceIndex">The index in the source array of the first Vector4 to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="destinationArray">The existing destination array of Vector4s into which to write the results.</param>
        /// <param name="destinationIndex">The index in the destination array of the first result Vector4 to write.</param>
        /// <param name="length">The number of Vector4s to transform.</param>
        public static void Transform( 
            Vector4[]      sourceArray, 
            int            sourceIndex, 
            ref Quaternion rotation, 
            Vector4[]      destinationArray, 
            int            destinationIndex,
            int            length )
        {
            #region Verify Arguments

            if( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if( length < 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "length" );

            if( sourceIndex < 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "sourceIndex" );

            if( destinationIndex < 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "destinationIndex" );

            if( sourceArray.Length < (sourceIndex + length) )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInSourceArray );

            if( destinationArray.Length < (destinationIndex + length) )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray );

            #endregion

            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2X = rotation.W * rot2X;
            float rotW2Y = rotation.W * rot2Y;
            float rotW2Z = rotation.W * rot2Z;

            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotX2Z = rotation.X * rot2Z;

            float rotY2Y = rotation.Y * rot2Y;
            float rotY2Z = rotation.Y * rot2Z;
            float rotZ2Z = rotation.Z * rot2Z;

            float factorXX = (1f - rotY2Y) - rotZ2Z;
            float factorXY = rotX2Y + rotW2Z;
            float factorXZ = rotX2Z - rotW2Y;

            float factorYX = rotX2Y - rotW2Z;
            float factorYY = (1f - rotX2X) - rotZ2Z;
            float factorYZ = rotY2Z + rotW2X;

            float factorZX = rotX2Z + rotW2Y;
            float factorZY = rotY2Z - rotW2X;
            float factorZZ = (1f - rotX2X) - rotY2Y;

            while( length > 0 )
            {
                Vector4 vector = sourceArray[sourceIndex];

                destinationArray[destinationIndex].X = (vector.X * factorXX) + (vector.Y * factorYX) + (vector.Z * factorZX);
                destinationArray[destinationIndex].Y = (vector.X * factorXY) + (vector.Y * factorYY) + (vector.Z * factorZY);
                destinationArray[destinationIndex].Z = (vector.X * factorXZ) + (vector.Y * factorYZ) + (vector.Z * factorZZ);
                destinationArray[destinationIndex].W = vector.W;
                
                ++sourceIndex;
                ++destinationIndex;
                --length;
            }
        }

        #endregion

        #endregion

        #region > Interpolation <

        #region Lerp

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="start">
        /// The source vector that represents the start value.
        /// </param>
        /// <param name="end">
        /// The source vector that represents the end value.
        /// </param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>The linear interpolation of the two vectors.</returns>
        public static Vector4 Lerp( Vector4 start, Vector4 end, float amount )
        {
            Vector4 result;

            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
            result.Z = start.Z + ((end.Z - start.Z) * amount);
            result.W = start.W + ((end.W - start.W) * amount);

            return result;
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="start">
        /// The source vector that represents the start value. This value will not be modified by this method.
        /// </param>
        /// <param name="end">
        /// The source vector that represents the end value. This value will not be modified by this method.
        /// </param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <param name="result">Will contain the linear interpolation of the two vectors.</param>
        public static void Lerp( ref Vector4 start, ref Vector4 end, float amount, out Vector4 result )
        {
            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
            result.Z = start.Z + ((end.Z - start.Z) * amount);
            result.W = start.W + ((end.W - start.W) * amount);
        }

        #endregion

        #region SmoothStep

        /// <summary>
        /// Performs interpolationbetween two values using a cubic equation.
        /// </summary>
        /// <param name="start">
        /// The source vector that represents the start value.
        /// </param>
        /// <param name="end">
        /// The source vector that represents the end value.
        /// </param>
        /// <param name="amount">
        /// The weighting value.
        /// </param>
        /// <returns>The interpolated value.</returns>
        public static Vector4 SmoothStep( Vector4 start, Vector4 end, float amount )
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            Vector4 result;

            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
            result.Z = start.Z + ((end.Z - start.Z) * amount);
            result.W = start.W + ((end.W - start.W) * amount);

            return result;
        }

        /// <summary>
        /// Performs interpolationbetween two values using a cubic equation.
        /// </summary>
        /// <param name="start">
        /// The source vector that represents the start value.
        /// This value will not be modified by this method.
        /// </param>
        /// <param name="end">
        /// The source vector that represents the end value.
        /// This value will not be modified by this method.
        /// </param>
        /// <param name="amount">
        /// The weighting value.
        /// </param>
        /// <param name="result">Will contain the interpolated value.</param>
        public static void SmoothStep( ref Vector4 start, ref Vector4 end, float amount, out Vector4 result )
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
            result.Z = start.Z + ((end.Z - start.Z) * amount);
            result.W = start.W + ((end.W - start.W) * amount);
        }

        #endregion

        #region Coserp

        /// <summary>
        /// Performs COSine intERPolation between two vectors.
        /// </summary>
        /// <param name="start">The source value that represents the start vector.</param>
        /// <param name="end">The source value that represents the end vector.</param>
        /// <param name="amount">The weighting factor.</param>
        /// <returns>The interpolated value.</returns>
        public static Vector4 Coserp( Vector4 start, Vector4 end, float amount )
        {
            float endFactor   = 0.5f * (1.0f - (float)System.Math.Cos( amount * System.Math.PI ));
            float startFactor = 1.0f - endFactor;

            Vector4 result;

            result.X = (start.X * startFactor) + (end.X * endFactor);
            result.Y = (start.Y * startFactor) + (end.Y * endFactor);
            result.Z = (start.Z * startFactor) + (end.Z * endFactor);
            result.W = (start.W * startFactor) + (end.W * endFactor);

            return result;
        }

        /// <summary>
        /// Performs COSine intERPolation between two vectors.
        /// </summary>
        /// <param name="start">The source value that represents the start vector. This value will not be modified by this method.</param>
        /// <param name="end">The source value that represents the end vector. This value will not be modified by this method.</param>
        /// <param name="amount">The weighting factor.</param>
        /// <param name="result">Will contain the interpolated value.</param>
        public static void Coserp( ref Vector4 start, ref Vector4 end, float amount, out Vector4 result )
        {
            float endFactor   = 0.5f * (1.0f - (float)System.Math.Cos( amount * System.Math.PI ));
            float startFactor = 1.0f - endFactor;

            result.X = (start.X * startFactor) + (end.X * endFactor);
            result.Y = (start.Y * startFactor) + (end.Y * endFactor);
            result.Z = (start.Z * startFactor) + (end.Z * endFactor);
            result.W = (start.W * startFactor) + (end.W * endFactor);
        }

        #endregion

        #region Hermite

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="valueA">
        /// The first source position vector.
        /// </param>
        /// <param name="tangentA">
        /// The first source tangent vector.
        /// </param>
        /// <param name="valueB">
        /// The second source position vector.
        /// </param>
        /// <param name="tangentB">
        /// The second source tangent vector.
        /// </param>
        /// <param name="amount">
        /// The weighting factor.
        /// </param>
        /// <returns>
        /// The result of the Hermite spline interpolation.
        /// </returns>
        public static Vector4 Hermite (
            Vector4 valueA,
            Vector4 tangentA,
            Vector4 valueB,
            Vector4 tangentB,
            float amount 
        )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            float factorValueA   = ((2f * amountPow3) - (3f * amountPow2)) + 1f;
            float factorValueB   = (-2f * amountPow3) + (3f * amountPow2);
            float factorTangentA = (amountPow3 - (2f * amountPow2)) + amount;
            float factorTangentB = amountPow3 - amountPow2;

            Vector4 result;

            result.X = (valueA.X * factorValueA) + (valueB.X * factorValueB) + (tangentA.X * factorTangentA) + (tangentB.X * factorTangentB);
            result.Y = (valueA.Y * factorValueA) + (valueB.Y * factorValueB) + (tangentA.Y * factorTangentA) + (tangentB.Y * factorTangentB);
            result.Z = (valueA.Z * factorValueA) + (valueB.Z * factorValueB) + (tangentA.Z * factorTangentA) + (tangentB.Z * factorTangentB);
            result.W = (valueA.W * factorValueA) + (valueB.W * factorValueB) + (tangentA.W * factorTangentA) + (tangentB.W * factorTangentB);

            return result;
        }

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="valueA">
        /// The first source position vector. This value will not be modified by this method.
        /// </param>
        /// <param name="tangentA">
        /// The first source tangent vector. This value will not be modified by this method.
        /// </param>
        /// <param name="valueB">
        /// The second source position vector. This value will not be modified by this method.
        /// </param>
        /// <param name="tangentB">
        /// The second source tangent vector. This value will not be modified by this method.
        /// </param>
        /// <param name="amount">
        /// The weighting factor.
        /// </param>
        /// <param name="result">
        /// Will store the result of the Hermite spline interpolation.
        /// </param>
        public static void Hermite (
            ref Vector4 valueA,
            ref Vector4 tangentA,
            ref Vector4 valueB,
            ref Vector4 tangentB,
            float amount,
            out Vector4 result
        )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            float factorValueA   = ((2f * amountPow3) - (3f * amountPow2)) + 1f;
            float factorValueB   = (-2f * amountPow3) + (3f * amountPow2);
            float factorTangentA = (amountPow3 - (2f * amountPow2)) + amount;
            float factorTangentB = amountPow3 - amountPow2;

            result.X = (valueA.X * factorValueA) + (valueB.X * factorValueB) + (tangentA.X * factorTangentA) + (tangentB.X * factorTangentB);
            result.Y = (valueA.Y * factorValueA) + (valueB.Y * factorValueB) + (tangentA.Y * factorTangentA) + (tangentB.Y * factorTangentB);
            result.Z = (valueA.Z * factorValueA) + (valueB.Z * factorValueB) + (tangentA.Z * factorTangentA) + (tangentB.Z * factorTangentB);
            result.W = (valueA.W * factorValueA) + (valueB.W * factorValueB) + (tangentA.W * factorTangentA) + (tangentB.W * factorTangentB);
        }

        #endregion

        #region CatmullRom

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="valueA">
        /// The first position in the interpolation.
        /// </param>
        /// <param name="valueB">
        /// The second position in the interpolation.
        /// </param>
        /// <param name="valueC">
        /// The third position in the interpolation.
        /// </param>
        /// <param name="valueD">
        /// The fourth position in the interpolation.
        /// </param>
        /// <param name="amount">The weighting factor.</param>
        /// <returns>A new Vector that contains the result of the Catmull-Rom interpolation.</returns>
        public static Vector4 CatmullRom( Vector4 valueA, Vector4 valueB, Vector4 valueC, Vector4 valueD, float amount )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            Vector4 result;

            result.X = 0.5f * ((((2f * valueB.X) + ((-valueA.X + valueC.X) * amount)) + (((((2f * valueA.X) - (5f * valueB.X)) + (4f * valueC.X)) - valueD.X) * amountPow2)) + ((((-valueA.X + (3f * valueB.X)) - (3f * valueC.X)) + valueD.X) * amountPow3));
            result.Y = 0.5f * ((((2f * valueB.Y) + ((-valueA.Y + valueC.Y) * amount)) + (((((2f * valueA.Y) - (5f * valueB.Y)) + (4f * valueC.Y)) - valueD.Y) * amountPow2)) + ((((-valueA.Y + (3f * valueB.Y)) - (3f * valueC.Y)) + valueD.Y) * amountPow3));
            result.Z = 0.5f * ((((2f * valueB.Z) + ((-valueA.Z + valueC.Z) * amount)) + (((((2f * valueA.Z) - (5f * valueB.Z)) + (4f * valueC.Z)) - valueD.Z) * amountPow2)) + ((((-valueA.Z + (3f * valueB.Z)) - (3f * valueC.Z)) + valueD.Z) * amountPow3));
            result.W = 0.5f * ((((2f * valueB.W) + ((-valueA.W + valueC.W) * amount)) + (((((2f * valueA.W) - (5f * valueB.W)) + (4f * valueC.W)) - valueD.W) * amountPow2)) + ((((-valueA.W + (3f * valueB.W)) - (3f * valueC.W)) + valueD.W) * amountPow3));

            return result;
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="valueA">
        /// The first position in the interpolation. This value will not be modified by this method.
        /// </param>
        /// <param name="valueB">
        /// The second position in the interpolation. This value will not be modified by this method.
        /// </param>
        /// <param name="valueC">
        /// The third position in the interpolation. This value will not be modified by this method.
        /// </param>
        /// <param name="valueD">
        /// The fourth position in the interpolation. This value will not be modified by this method.
        /// </param>
        /// <param name="amount">The weighting factor.</param>
        /// <param name="result">Will contain the result of the Catmull-Rom interpolation.</param>
        public static void CatmullRom( ref Vector4 valueA, ref Vector4 valueB, ref Vector4 valueC, ref Vector4 valueD, float amount, out Vector4 result )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            result.X = 0.5f * ((((2f * valueB.X) + ((-valueA.X + valueC.X) * amount)) + (((((2f * valueA.X) - (5f * valueB.X)) + (4f * valueC.X)) - valueD.X) * amountPow2)) + ((((-valueA.X + (3f * valueB.X)) - (3f * valueC.X)) + valueD.X) * amountPow3));
            result.Y = 0.5f * ((((2f * valueB.Y) + ((-valueA.Y + valueC.Y) * amount)) + (((((2f * valueA.Y) - (5f * valueB.Y)) + (4f * valueC.Y)) - valueD.Y) * amountPow2)) + ((((-valueA.Y + (3f * valueB.Y)) - (3f * valueC.Y)) + valueD.Y) * amountPow3));
            result.Z = 0.5f * ((((2f * valueB.Z) + ((-valueA.Z + valueC.Z) * amount)) + (((((2f * valueA.Z) - (5f * valueB.Z)) + (4f * valueC.Z)) - valueD.Z) * amountPow2)) + ((((-valueA.Z + (3f * valueB.Z)) - (3f * valueC.Z)) + valueD.Z) * amountPow3));
            result.W = 0.5f * ((((2f * valueB.W) + ((-valueA.W + valueC.W) * amount)) + (((((2f * valueA.W) - (5f * valueB.W)) + (4f * valueC.W)) - valueD.W) * amountPow2)) + ((((-valueA.W + (3f * valueB.W)) - (3f * valueC.W)) + valueD.W) * amountPow3));
        }

        #endregion

        #endregion

        #region > Operators <

        #region Add

        /// <summary>
        /// Returns the result of adding the <paramref name="right"/> Vector to the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 Add( Vector4 left, Vector4 right )
        {
            Vector4 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the <paramref name="right"/> Vector to the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Vector4 left, ref Vector4 right, out Vector4 result )
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 Add( Vector4 vector, float scalar )
        {
            Vector4 result;

            result.X = vector.X + scalar;
            result.Y = vector.Y + scalar;
            result.Z = vector.Z + scalar;
            result.W = vector.W + scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the given <paramref name="scalar"/> to the given <paramref name="vector"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation. </param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Vector4 vector, float scalar, out Vector4 result )
        {
            result.X = vector.X + scalar;
            result.Y = vector.Y + scalar;
            result.Z = vector.Z + scalar;
            result.W = vector.W + scalar;
        }

        /// <summary>
        /// This method returns the specified Vector.
        /// </summary>
        /// <remarks>
        /// Is equal to "+vector".
        /// </remarks>
        /// <param name="vector">The input vector.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 Plus( Vector4 vector )
        {
            return vector;
        }

        /// <summary>
        /// This method stores the specified Vector in the specified result value.
        /// </summary>
        /// <remarks>
        /// Is equal to "+vector".
        /// </remarks>
        /// <param name="vector">The inputvector.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Plus( ref Vector4 vector, out Vector4 result )
        {
            result.X = vector.X;
            result.Y = vector.Y;
            result.Z = vector.Z;
            result.W = vector.W;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Vector from the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 Subtract( Vector4 left, Vector4 right )
        {
            Vector4 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the <paramref name="right"/> Vector frpm the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Vector4 left, ref Vector4 right, out Vector4 result )
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 Subtract( Vector4 vector, float scalar )
        {
            Vector4 result;

            result.X = vector.X - scalar;
            result.Y = vector.Y - scalar;
            result.Z = vector.Z - scalar;
            result.W = vector.W - scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="vector"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Vector4 vector, float scalar, out Vector4 result )
        {
            result.X = vector.X - scalar;
            result.Y = vector.Y - scalar;
            result.Z = vector.Z - scalar;
            result.W = vector.W - scalar;
        }

        #endregion

        #region Negate

        /// <summary>
        /// Returns the result of negating the elements of the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">
        /// The vector to negate.
        /// </param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 Negate( Vector4 vector )
        {
            Vector4 result;

            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;
            result.W = -vector.W;

            return result;
        }

        /// <summary>
        /// Stores the result of negating the elements of the given <paramref name="vector"/> in the given Vector.
        /// </summary>
        /// <param name="vector">
        /// The vector to negate. This value will not be modified by this method.
        /// </param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Negate( ref Vector4 vector, out Vector4 result )
        {
            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;
            result.W = -vector.W;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Returns the result of multiplying the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 Multiply( Vector4 vector, float scalar )
        {
            Vector4 result;

            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;
            result.Z = vector.Z * scalar;
            result.W = vector.W * scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// in the given Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Multiply( ref Vector4 vector, float scalar, out Vector4 result )
        {
            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;
            result.Z = vector.Z * scalar;
            result.W = vector.W * scalar;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 Multiply( Vector4 left, Vector4 right )
        {
            Vector4 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
            result.W = left.W * right.W;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the left Vector by the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The vector on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Multiply( ref Vector4 left, ref Vector4 right, out Vector4 result )
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
            result.W = left.W * right.W;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Returns the result of dividing the given <paramref name="vector"/> through the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 Divide( Vector4 vector, float scalar )
        {
            Vector4 result;

            result.X = vector.X / scalar;
            result.Y = vector.Y / scalar;
            result.Z = vector.Z / scalar;
            result.W = vector.W / scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the given <paramref name="vector"/> through the given <paramref name="scalar"/>
        /// in the given result Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Divide( ref Vector4 vector, float scalar, out Vector4 result )
        {
            result.X = vector.X / scalar;
            result.Y = vector.Y / scalar;
            result.Z = vector.Z / scalar;
            result.W = vector.W / scalar;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 Divide( Vector4 left, Vector4 right )
        {
            Vector4 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
            result.W = left.W / right.W;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the left Vector through the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The vector on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will store the result of the operation.</param>
        public static void Divide( ref Vector4 left, ref Vector4 right, out Vector4 result )
        {
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
            result.W = left.W / right.W;
        }

        #endregion

        #endregion

        #region > Misc <

        #region Max

        /// <summary>
        /// Returns a vector that contains the highest value from
        /// each matching pair of components of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector.</param>
        /// <param name="vectorB">The second vector.</param>
        /// <returns>
        /// The maximized vector.
        /// </returns>
        public static Vector4 Max( Vector4 vectorA, Vector4 vectorB )
        {
            Vector4 result;

            result.X = (vectorA.X > vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y > vectorB.Y) ? vectorA.Y : vectorB.Y;
            result.Z = (vectorA.Z > vectorB.Z) ? vectorA.Z : vectorB.Z;
            result.W = (vectorA.W > vectorB.W) ? vectorA.W : vectorB.W;

            return result;
        }

        /// <summary>
        /// Returns a vector that contains the highest value from
        /// each matching pair of components of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second vector. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the maximized vector.</param>
        public static void Max( ref Vector4 vectorA, ref Vector4 vectorB, out Vector4 result )
        {
            result.X = (vectorA.X > vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y > vectorB.Y) ? vectorA.Y : vectorB.Y;
            result.Z = (vectorA.Z > vectorB.Z) ? vectorA.Z : vectorB.Z;
            result.W = (vectorA.W > vectorB.W) ? vectorA.W : vectorB.W;
        }

        #endregion

        #region Min

        /// <summary>
        /// Returns a vector that contains the lowest value from
        /// each matching pair of components of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector.</param>
        /// <param name="vectorB">The second vector.</param>
        /// <returns>
        /// The minimized vector.
        /// </returns>
        public static Vector4 Min( Vector4 vectorA, Vector4 vectorB )
        {
            Vector4 result;

            result.X = (vectorA.X < vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y < vectorB.Y) ? vectorA.Y : vectorB.Y;
            result.Z = (vectorA.Z < vectorB.Z) ? vectorA.Z : vectorB.Z;
            result.W = (vectorA.W < vectorB.W) ? vectorA.W : vectorB.W;

            return result;
        }

        /// <summary>
        /// Returns a vector that contains the lowest value from
        /// each matching pair of components of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second vector. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the minimized vector.</param>
        public static void Min( ref Vector4 vectorA, ref Vector4 vectorB, out Vector4 result )
        {
            result.X = (vectorA.X < vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y < vectorB.Y) ? vectorA.Y : vectorB.Y;
            result.Z = (vectorA.Z < vectorB.Z) ? vectorA.Z : vectorB.Z;
            result.W = (vectorA.W < vectorB.W) ? vectorA.W : vectorB.W;
        }

        #endregion

        #region Average

        /// <summary>
        /// Returns the average of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector.</param>
        /// <param name="vectorB">The second vector.</param>
        /// <returns>The average of the given Vectors.</returns>
        public static Vector4 Average( Vector4 vectorA, Vector4 vectorB )
        {
            Vector4 result;

            result.X = (vectorA.X + vectorB.X) * 0.5f;
            result.Y = (vectorA.Y + vectorB.Y) * 0.5f;
            result.Z = (vectorA.Z + vectorB.Z) * 0.5f;
            result.W = (vectorA.W + vectorB.W) * 0.5f;

            return result;
        }

        /// <summary>
        /// Returns the average of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second vector. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the average of the given Vectors.</param>
        public static void Average( ref Vector4 vectorA, ref Vector4 vectorB, out Vector4 result )
        {
            result.X = (vectorA.X + vectorB.X) * 0.5f;
            result.Y = (vectorA.Y + vectorB.Y) * 0.5f;
            result.Z = (vectorA.Z + vectorB.Z) * 0.5f;
            result.W = (vectorA.W + vectorB.W) * 0.5f;
        }

        #endregion

        #region Distance

        #region Distance

        /// <summary>
        /// Returns the distance between the two specified Vectors2.
        /// </summary>
        /// <param name="vectorA">The first Vector.</param>
        /// <param name="vectorB">The second Vector.</param>
        /// <returns>
        /// The distance from the <paramref name="vectorA"/> to <paramref name="vectorB"/>.
        /// </returns>
        public static float Distance( Vector4 vectorA, Vector4 vectorB )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;
            float deltaZ = vectorA.Z - vectorB.Z;
            float deltaW = vectorA.W - vectorB.W;

            float squaredLength = (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ) + (deltaW * deltaW);
            return (float)System.Math.Sqrt( squaredLength );
        }

        /// <summary>
        /// Stores the distance between the two specified Vectors2 in the given <paramref name="result"/> value.
        /// </summary>
        /// <param name="vectorA">The first Vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second Vector. This value will not be modified by this method.</param>
        /// <param name="result">
        /// Will contain the distance from the <paramref name="vectorA"/> to <paramref name="vectorB"/>.
        /// </param>
        public static void Distance( ref Vector4 vectorA, ref Vector4 vectorB, out float result )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;
            float deltaZ = vectorA.Z - vectorB.Z;
            float deltaW = vectorA.W - vectorB.W;

            float squaredLength = (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ) + (deltaW * deltaW);
            result = (float)System.Math.Sqrt( squaredLength );
        }

        #endregion

        #region DistanceSquared

        /// <summary>
        /// Returns the squared distance between the two specified Vectors2.
        /// </summary>
        /// <param name="vectorA">The first Vector.</param>
        /// <param name="vectorB">The second Vector.</param>
        /// <returns>
        /// The distance from the <paramref name="vectorA"/> to <paramref name="vectorB"/>.
        /// </returns>
        public static float DistanceSquared( Vector4 vectorA, Vector4 vectorB )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;
            float deltaZ = vectorA.Z - vectorB.Z;
            float deltaW = vectorA.W - vectorB.W;

            return (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ) + (deltaW * deltaW);
        }

        /// <summary>
        /// Stores the squared distance between the two specified Vectors2 in the given <paramref name="result"/> value.
        /// </summary>
        /// <param name="vectorA">The first Vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second Vector. This value will not be modified by this method.</param>
        /// <param name="result">
        /// This value will contain the squared distance from the <paramref name="vectorA"/> to <paramref name="vectorB"/>.
        /// </param>
        public static void DistanceSquared( ref Vector4 vectorA, ref Vector4 vectorB, out float result )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;
            float deltaZ = vectorA.Z - vectorB.Z;
            float deltaW = vectorA.W - vectorB.W;

            result = (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ) + (deltaW * deltaW);
        }

        #endregion

        #endregion

        #region Clamp

        /// <summary>
        /// Returns the result of clamping the given Vector to be in the range
        /// defined by <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="vector">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>
        /// The clamped value.
        /// </returns>
        public static Vector4 Clamp( Vector4 vector, Vector4 min, Vector4 max )
        {
            Vector4 result;

            if( vector.X > max.X )
                result.X = max.X;
            else if( vector.X < min.X )
                result.X = min.X;
            else
                result.X = vector.X;

            if( vector.Y > max.Y )
                result.Y = max.Y;
            else if( vector.Y < min.Y )
                result.Y = min.Y;
            else
                result.Y = vector.Y;

            if( vector.Z > max.Z )
                result.Z = max.Z;
            else if( vector.Z < min.Z )
                result.Z = min.Z;
            else
                result.Z = vector.Z;

            if( vector.W > max.W )
                result.W = max.W;
            else if( vector.W < min.W )
                result.W = min.W;
            else
                result.W = vector.W;

            return result;
        }

        /// <summary>
        /// Stores the result of clamping the given Vector to be in the range
        /// defined by <paramref name="min"/> and <paramref name="max"/>
        /// in the given <paramref name="result"/> value.
        /// </summary>
        /// <param name="vector">The value to clamp. This value will not be modified by this method.</param>
        /// <param name="min">The minimum value. This value will not be modified by this method.</param>
        /// <param name="max">The maximum value. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the clamped value.</param>
        public static void Clamp( ref Vector4 vector, ref Vector4 min, ref Vector4 max, out Vector4 result )
        {
            if( vector.X > max.X )
                result.X = max.X;
            else if( vector.X < min.X )
                result.X = min.X;
            else
                result.X = vector.X;

            if( vector.Y > max.Y )
                result.Y = max.Y;
            else if( vector.Y < min.Y )
                result.Y = min.Y;
            else
                result.Y = vector.Y;

            if( vector.Z > max.Z )
                result.Z = max.Z;
            else if( vector.Z < min.Z )
                result.Z = min.Z;
            else
                result.Z = vector.Z;

            if( vector.W > max.W )
                result.W = max.W;
            else if( vector.W < min.W )
                result.W = min.W;
            else
                result.W = vector.W;
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether the given <see cref="Vector4"/> has the
        /// same indices set as this Vector4.
        /// </summary>
        /// <param name="other">The Vector4 to test against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the elements of the vectors are approximately equal; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( Vector4 other )
        {
            return this.X.IsApproximate( other.X ) &&
                   this.Y.IsApproximate( other.Y ) &&
                   this.Z.IsApproximate( other.Z ) &&
                   this.W.IsApproximate( other.W );
        }

        /// <summary>
        /// Returns whether the given <see cref="Object"/> is equal to this Vector4.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is Vector4 )
            {
                return Equals( (Vector4)obj );
            }
            else if( obj is Point4 )
            {
                Point4 vector = (Point4)obj;

                return this.X.IsApproximate( (float)vector.X ) &&
                       this.Y.IsApproximate( (float)vector.Y ) &&
                       this.Z.IsApproximate( (float)vector.Z ) &&
                       this.W.IsApproximate( (float)vector.W );
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overriden to return a human-readable text representation of the Vector4.
        /// </summary>
        /// <returns>A human-readable text representation of the Vector4.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Vector4.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Vector4.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "[{0} {1} {2} {3}]",
                X.ToString( formatProvider ),
                Y.ToString( formatProvider ),
                Z.ToString( formatProvider ),
                W.ToString( formatProvider )
            );
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Overriden to return the hashcode of the <see cref="Vector4"/>.
        /// </summary>
        /// <returns>The simple xor-ed hashcode.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.X );
            hashBuilder.AppendStruct( this.Y );
            hashBuilder.AppendStruct( this.Z );
            hashBuilder.AppendStruct( this.W );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        #region +

        /// <summary>
        /// Returns the result of adding the <paramref name="right"/> Vector to the the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 operator +( Vector4 left, Vector4 right )
        {
            Vector4 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;

            return result;
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the thegiven <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 operator +( Vector4 vector, float scalar )
        {
            Vector4 result;

            result.X = vector.X + scalar;
            result.Y = vector.Y + scalar;
            result.Z = vector.Z + scalar;
            result.W = vector.W + scalar;

            return result;
        }

        /// <summary>
        /// Returns the original specified vector, doing nothing.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 operator +( Vector4 vector )
        {
            return vector;
        }

        #endregion

        #region -

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Vector from the the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 operator -( Vector4 left, Vector4 right )
        {
            Vector4 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 operator -( Vector4 vector, float scalar )
        {
            Vector4 result;

            result.X = vector.X - scalar;
            result.Y = vector.Y - scalar;
            result.Z = vector.Z - scalar;
            result.W = vector.W - scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of negating the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 operator -( Vector4 vector )
        {
            Vector4 result;

            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;
            result.W = -vector.W;

            return result;
        }

        #endregion

        #region *

        /// <summary>
        /// Returns the result of multiplcing the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 operator *( Vector4 vector, float scalar )
        {
            Vector4 result;

            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;
            result.Z = vector.Z * scalar;
            result.W = vector.W * scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 operator *( Vector4 left, Vector4 right )
        {
            Vector4 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
            result.W = left.W * right.W;

            return result;
        }

        #endregion

        #region /

        /// <summary>
        /// Returns the result of dividing the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 operator /( Vector4 vector, float scalar )
        {
            Vector4 result;

            result.X = vector.X / scalar;
            result.Y = vector.Y / scalar;
            result.Z = vector.Z / scalar;
            result.W = vector.W / scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector element-by-element.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector4 operator /( Vector4 left, Vector4 right )
        {
            Vector4 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
            result.W = left.W / right.W;

            return result;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Returns whether the specified <see cref="Vector4"/> instances are (approximately) equal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator ==( Vector4 left, Vector4 right )
        {
            return left.X.IsApproximate( right.X ) &&
                   left.Y.IsApproximate( right.Y ) &&
                   left.Z.IsApproximate( right.Z ) &&
                   left.W.IsApproximate( right.W );
        }

        /// <summary>
        /// Returns whether the specified <see cref="Vector4"/> instances are (approximately) inequal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator !=( Vector4 left, Vector4 right )
        {
            return !left.X.IsApproximate( right.X ) ||
                   !left.Y.IsApproximate( right.Y ) ||
                   !left.Z.IsApproximate( right.Z ) ||
                   !left.W.IsApproximate( right.W );
        }

        #endregion

        #region Cast

        /// <summary>
        /// Explicit cast operator that implements conversion
        /// from a <see cref="Vector4"/> to a <see cref="Point4"/>.
        /// </summary>
        /// <param name="vector">
        /// The input vector.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static explicit operator Point4( Vector4 vector )
        {
            return new Point4( (int)vector.X, (int)vector.Y, (int)vector.Z, (int)vector.W );
        }

        #endregion

        #endregion
    }
}
