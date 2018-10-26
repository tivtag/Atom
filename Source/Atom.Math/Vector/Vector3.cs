// <copyright file="Vector3.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Vector3 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a three dimensional single-precision floating-vector Vector.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.Vector3Converter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Vector3 : IEquatable<Vector3>, ICultureSensitiveToStringProvider
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

        #endregion

        #region [ Constants ]
        
        /// <summary>
        /// Gets a <see cref="Vector3"/> with all its components set to zero.
        /// </summary>
        /// <value>The vector (0, 0, 0).</value>
        public static Vector3 Zero
        { 
            get { return new Vector3(); } 
        }

        /// <summary>
        /// Gets a <see cref="Vector3"/> with all its components set to one.
        /// </summary>
        /// <value>The vector (1, 1, 1).</value>
        public static Vector3 One
        {
            get { return new Vector3( 1.0f, 1.0f, 1.0f ); }
        }

        /// <summary>
        /// Gets the unit <see cref="Vector3"/> for the x-axis.
        /// </summary>
        /// <value>The vector (1, 0, 0).</value>
        public static Vector3 UnitX 
        { 
            get { return new Vector3( 1.0f, 0.0f, 0.0f ); } 
        }

        /// <summary>
        /// Gets the unit <see cref="Vector3"/> for the y-axis.
        /// </summary>
        /// <value>The vector (0, 1, 0).</value>
        public static Vector3 UnitY 
        {
            get { return new Vector3( 0.0f, 1.0f, 0.0f ); }
        }

        /// <summary>
        /// Gets the unit <see cref="Vector3"/> for the z-axis.
        /// </summary>
        /// <value>The vector (0, 0, 1).</value>
        public static Vector3 UnitZ
        { 
            get { return new Vector3( 0.0f, 0.0f, 1.0f ); }
        }

        /// <summary>
        /// Gets a unit Vector3 vectoring up (0, 1, 0).
        /// </summary>
        /// <value>
        /// A unit Vector3 vectoring up.
        /// </value>
        public static Vector3 Up 
        { 
            get { return new Vector3( 0.0f, 1.0f, 0.0f ); }
        }

        /// <summary>
        /// Gets a unit Vector3 vectoring down (0, −1, 0).
        /// </summary>
        /// <value>
        /// A unit Vector3 vectoring down.
        /// </value>
        public static Vector3 Down 
        {
            get { return new Vector3( 0.0f, -1.0f, 0.0f ); }
        }

        /// <summary>
        /// Gets a unit Vector3 vectoring to the right (1, 0, 0).
        /// </summary>
        /// <value>A unit Vector3 vectoring to the right.</value>
        public static Vector3 Right 
        { 
            get { return new Vector3( 1.0f, 0.0f, 0.0f ); }
        }

        /// <summary>
        /// Gets a unit Vector3 vectoring left (−1, 0, 0).
        /// </summary>
        /// <value>
        /// A unit Vector3 vectoring left.
        /// </value>
        public static Vector3 Left 
        {
            get { return new Vector3( -1.0f, 0.0f, 0.0f ); } 
        }

        /// <summary>
        /// Gets a unit Vector3 vectoring forward (0, 0, −1).
        /// </summary>
        /// <value>
        /// A unit Vector3 vectoring forward.
        /// </value>
        public static Vector3 Forward 
        {
            get { return new Vector3( 0.0f, 0.0f, -1.0f ); }
        }

        /// <summary>
        /// Gets a unit Vector3 vectoring backward (0, 0, 1).
        /// </summary>
        /// <value>
        /// A unit Vector3 vectoring backward.
        /// </value>
        public static Vector3 Backward 
        {
            get { return new Vector3( 0.0f, 0.0f, 1.0f ); }
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the length of the Vector.
        /// </summary>
        /// <value>The length (also caleld magnitude) of the vector.</value>
        /// <exception cref="InvalidOperationException">
        /// Thrown when trying to set a new length on a Vector with a length of zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when trying to set the length to a negative value.
        /// </exception>
        [System.Xml.Serialization.XmlIgnore]
        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt( (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) );
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
                }
            }
        }

        /// <summary>
        /// Gets or sets the squared length of the Vector.
        /// </summary>
        /// <value>The squared length (also caleld magnitude) of the vector.</value>
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
                return (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z);
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
                }
            }
        }

        /// <summary>
        /// Gets or sets the value at the given axis.
        /// </summary>
        /// <param name="axis">
        /// The axis whose value should be get or set.
        /// </param>
        /// <returns>
        /// The value at the given axis.
        /// </returns>
        public float this[Axis3 axis]
        {
            get
            {
                Contract.Requires( axis != Axis3.None );

                switch( axis )
                {
                    case Axis3.X:
                        return this.X;

                    case Axis3.Y:
                        return this.Y;

                    default:
                    case Axis3.Z:
                        return this.Z;
                }
            }

            set
            {
                Contract.Requires( axis != Axis3.None );

                switch( axis )
                {
                    case Axis3.X:
                        this.X = value;
                        break;

                    case Axis3.Y:
                        this.Y = value;
                        break;

                    case Axis3.Z:
                        this.Z = value;
                        break;
                }
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> struct.
        /// </summary>
        /// <param name="x">The X-coordinate of the new Vector.</param>
        /// <param name="y">The Y-coordinate of the new Vector.</param>
        /// <param name="z">The Z-coordinate of the new Vector.</param>
        public Vector3( float x, float y, float z )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> struct.
        /// </summary>
        /// <param name="vector">
        /// The two dimensional vector to convert.
        /// </param>
        public Vector3( Vector2 vector )
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = 0.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> struct.
        /// </summary>
        /// <param name="vector">
        /// The two dimensional vector to convert.
        /// </param>
        /// <param name="z">The Z-coordinate of the new Vector.</param>
        public Vector3( Vector2 vector, float z )
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = z;
        }

        /// <summary>
        /// Creates a new instance of the Vector3 structure that has set the specified axis
        /// to the specified value.
        /// </summary>
        /// <example>
        /// Vector3 unitX = Vector3.FromAxis( 1.0f, Axis3.X ); // (1 0 0)
        /// Vector3 unitY = Vector3.FromAxis( 1.0f, Axis3.Y ); // (0 1 0)
        /// Vector3 unitZ = Vector3.FromAxis( 1.0f, Axis3.Z ); // (0 0 1)
        /// Vector3 zero  = Vector3.FromAxis( 1.0f, Axis3.None ); // (0 0 0)
        /// </example>
        /// <param name="value">
        /// The value of the axis.
        /// </param>
        /// <param name="axis">
        /// The axis to initialize.
        /// </param>
        /// <returns>
        /// The newly created Vector3.
        /// </returns>
        public static Vector3 FromAxis( float value, Axis3 axis )
        {
            switch( axis )
            {
                case Axis3.X:
                    return new Vector3( value, 0.0f, 0.0f );

                case Axis3.Y:
                    return new Vector3( 0.0f, value, 0.0f );

                case Axis3.Z:
                    return new Vector3( 0.0f, 0.0f, value );

                default:
                    return Vector3.Zero;
            }
        }

        #endregion

        #region [ Methods ]

        #region Normalize

        /// <summary>
        /// Normalizes the Vector, setting its length to one.
        /// </summary>
        public void Normalize()
        {
            float squaredLength = (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z);
            float invLength     = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            this.X *= invLength;
            this.Y *= invLength;
            this.Z *= invLength;
        }

        /// <summary>
        /// Returns the result of normalizing the given Vector.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static Vector3 Normalize( Vector3 vector )
        {
            float squaredLength = (vector.X * vector.X) + (vector.Y * vector.Y) + (vector.Z * vector.Z);
            float invLength     = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            Vector3 result;

            result.X = vector.X * invLength;
            result.Y = vector.Y * invLength;
            result.Z = vector.Z * invLength;

            return result;
        }

        /// <summary>
        /// Stores the result of normalizing the given Vector in the given <paramref name="result"/> Vector.
        /// </summary>
        /// <param name="vector">The vector to normalize. This value will not be modified by this method.</param>
        /// <param name="result">This value will store the result of the operation.</param>
        public static void Normalize( ref Vector3 vector, out Vector3 result )
        {
            float squaredLength = (vector.X * vector.X) + (vector.Y * vector.Y) + (vector.Z * vector.Z);
            float invLength     = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            result.X = vector.X * invLength;
            result.Y = vector.Y * invLength;
            result.Z = vector.Z * invLength;
        }

        #endregion

        #region RotateAroundAxis

        /// <summary>
        /// Rotates the given vector around an arbitrary axis defined by a vector.
        /// </summary>
        /// <param name="vectorToRotate">
        /// The vector to rotate.
        /// </param>
        /// <param name="axis">
        /// The axis to rotate the vector around.
        /// </param>
        /// <param name="angle">
        /// The angle, in radians, to rotate the vector by.
        /// </param>
        /// <returns>The vector rotated by the specified amount around the give axis.</returns>
        public static Vector3 RotateAroundAxis(
            Vector3 vectorToRotate, Vector3 axis, float angle )
        {
            float sinAngle = (float)System.Math.Sin( (double)angle );
            float cosAngle = (float)System.Math.Cos( (double)angle );

            float xx = axis.X * vectorToRotate.X;
            float xy = axis.X * vectorToRotate.Y;
            float xz = axis.X * vectorToRotate.Z;
            float yx = axis.Y * vectorToRotate.X;
            float yy = axis.Y * vectorToRotate.Y;
            float yz = axis.Y * vectorToRotate.Z;
            float zx = axis.Z * vectorToRotate.X;
            float zy = axis.Z * vectorToRotate.Y;
            float zz = axis.Z * vectorToRotate.Z;

            Vector3 result;

            result.X = (axis.X * (xx + yy + zz)) +
                       ((vectorToRotate.X * ((axis.Y * axis.Y) + (axis.Z * axis.Z))) - ((axis.X * (yy + zz)) * cosAngle)) +
                       ((-zy + yz) * sinAngle);

            result.Y = (axis.Y * (xx + yy + zz)) +
                       ((vectorToRotate.Y * ((axis.X * axis.X) + (axis.Z * axis.Z))) - ((axis.Y * (xx + zz)) * cosAngle)) +
                       ((zx - xz) * sinAngle);

            result.Z = (axis.Z * (xx + yy + zz)) +
                       ((vectorToRotate.Z * ((axis.X * axis.X) + (axis.Y * axis.Y))) - ((axis.Z * (xx + yy)) * cosAngle)) +
                       ((-yx + xy) * sinAngle);

            return result;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Returns the dot(scalar) product of the given <see cref="Vector3"/>s.
        /// </summary>
        /// <param name="left">The Vector3 on the left side of the equation.</param>
        /// <param name="right">The Vector3 on the right side of the equation.</param>
        /// <returns>The dot product.</returns>
        public static float Dot( Vector3 left, Vector3 right )
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        /// <summary>
        /// Returns the dot(scalar) product of the given <see cref="Vector3"/>s.
        /// </summary>
        /// <param name="left">The Vector3 on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The Vector3 on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">This value will contain the result of this operation.</param>
        public static void Dot( ref Vector3 left, ref Vector3 right, out float result )
        {
            result = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        #endregion

        #region Cross

        /// <summary>
        /// Returns the cross product of the given <see cref="Vector3"/>s.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The cross product of the vectors.</returns>
        public static Vector3 Cross( Vector3 left, Vector3 right )
        {
            Vector3 result;

            result.X = (left.Y * right.Z) - (left.Z * right.Y);
            result.Y = (left.Z * right.X) - (left.X * right.Z);
            result.Z = (left.X * right.Y) - (left.Y * right.X);

            return result;
        }

        /// <summary>
        /// Returns the cross product of the given <see cref="Vector3"/>s.
        /// </summary>
        /// <param name="left">
        /// The vector on the left side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="right">
        /// The vector on the right side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="result">
        /// Will contain the cross product of the vectors.
        /// </param>
        public static void Cross( ref Vector3 left, ref Vector3 right, out Vector3 result )
        {
            result.X = (left.Y * right.Z) - (left.Z * right.Y);
            result.Y = (left.Z * right.X) - (left.X * right.Z);
            result.Z = (left.X * right.Y) - (left.Y * right.X);
        }

        #endregion

        #region Dyadic

        /// <summary>
        /// Returns the tensor/dyadic product (also called outer product) of the given Vectors.. </summary>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Tensor_product for more information.
        /// </remarks>
        /// <param name="left">The first input vector.</param>
        /// <param name="right">The second input vector.</param>
        /// <returns> The result of the operation. </returns>
        public static Matrix3 Dyadic( Vector3 left, Vector3 right )
        {
            return new Matrix3(
                left.X * right.X, left.X * right.Y, left.X * right.Z,
                left.Y * right.X, left.Y * right.Y, left.Y * right.Z,
                left.Z * right.X, left.Z * right.Y, left.Z * right.Z
            );
        }

        /// <summary>
        /// Returns the tensor/dyadic product (also called outer product) of the given Vectors.. </summary>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Tensor_product for more information.
        /// </remarks>
        /// <param name="left">The first input vector. This value will not be modified by this method.</param>
        /// <param name="right">The second input vector. This value will not be modified by this method.</param>
        /// <param name="result"> Will contain the result of the operation. </param>
        public static void Dyadic( ref Vector3 left, ref Vector3 right, out Matrix3 result )
        {
            result = new Matrix3(
                left.X * right.X, left.X * right.Y, left.X * right.Z,
                left.Y * right.X, left.Y * right.Y, left.Y * right.Z,
                left.Z * right.X, left.Z * right.Y, left.Z * right.Z
            );
        }

        #endregion

        #region > Transformation <

        #region Vector3 by result

        /// <summary>
        /// Returns the result of transforming the specified <see cref="Vector3"/>
        /// by the specified <see cref="Matrix4"/>.
        /// </summary>
        /// <param name="vector">
        /// The source vector.
        /// </param>
        /// <param name="matrix">
        /// The transformation matrix.
        /// </param>
        /// <returns>
        /// The transformed vector.
        /// </returns>
        public static Vector3 Transform( Vector3 vector, Matrix4 matrix )
        {
            Vector3 result;

            result.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31) + matrix.M41;
            result.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32) + matrix.M42;
            result.Z = (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33) + matrix.M43;

            return result;
        }

        /// <summary>
        /// Stores the result of transforming the specified <see cref="Vector3"/>
        /// by the specified <see cref="Matrix4"/> in the specified <paramref name="result"/> value.
        /// </summary>
        /// <param name="vector">
        /// The source vector. This value will not be modified by this method.
        /// </param>
        /// <param name="matrix">
        /// The transformation matrix. This value will not be modified by this method.
        /// </param>
        /// <param name="result">Will contain the transformed vector.</param>
        public static void Transform( ref Vector3 vector, ref Matrix4 matrix, out Vector3 result )
        {
            result.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31) + matrix.M41;
            result.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32) + matrix.M42;
            result.Z = (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33) + matrix.M43;
        }

        #endregion

        #region Vector3 Normal by result

        /// <summary>Transforms a 3D vector normal by a result.</summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The transformation result.</param>
        /// <returns>The transformed vector normal.</returns>
        public static Vector3 TransformNormal( Vector3 normal, Matrix4 matrix )
        {
            Vector3 result;

            result.X = (normal.X * matrix.M11) + (normal.Y * matrix.M21) + (normal.Z * matrix.M31);
            result.Y = (normal.X * matrix.M12) + (normal.Y * matrix.M22) + (normal.Z * matrix.M32);
            result.Z = (normal.X * matrix.M13) + (normal.Y * matrix.M23) + (normal.Z * matrix.M33);

            return result;
        }

        /// <summary>Transforms a vector normal by a result.</summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The transformation result.</param>
        /// <param name="result">The Vector3 resulting from the transformation.</param>
        public static void TransformNormal( ref Vector3 normal, ref Matrix4 matrix, out Vector3 result )
        {
            result.X = (normal.X * matrix.M11) + (normal.Y * matrix.M21) + (normal.Z * matrix.M31);
            result.Y = (normal.X * matrix.M12) + (normal.Y * matrix.M22) + (normal.Z * matrix.M32);
            result.Z = (normal.X * matrix.M13) + (normal.Y * matrix.M23) + (normal.Z * matrix.M33);
        }

        #endregion

        #region Vector3 by Quaternion

        /// <summary>
        /// Transforms a Vector3 by a specified Quaternion rotation.
        /// </summary>
        /// <param name="vector">The Vector3 to rotate.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <returns>Returns a new Vector3 that results from the rotation.</returns>
        public static Vector3 Transform( Vector3 vector, Quaternion rotation )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rowW2X = rotation.W * rot2X;
            float rowW2Y = rotation.W * rot2Y;
            float rowW2Z = rotation.W * rot2Z;

            float rowX2X = rotation.X * rot2X;
            float rowX2Y = rotation.X * rot2Y;
            float rowX2Z = rotation.X * rot2Z;

            float rowY2Y = rotation.Y * rot2Y;
            float rotY2Z = rotation.Y * rot2Z;
            float rotZ2Z = rotation.Z * rot2Z;

            Vector3 result;

            result.X = (vector.X * (1f - rowY2Y - rotZ2Z)) + (vector.Y * (rowX2Y - rowW2Z))      + (vector.Z * (rowX2Z + rowW2Y));
            result.Y = (vector.X * (rowX2Y + rowW2Z))      + (vector.Y * (1f - rowX2X - rotZ2Z)) + (vector.Z * (rotY2Z - rowW2X));
            result.Z = (vector.X * (rowX2Z - rowW2Y))      + (vector.Y * (rotY2Z + rowW2X))      + (vector.Z * (1f - rowX2X - rowY2Y)); 
            
            return result;
        }

        /// <summary>
        /// Transforms a Vector3 by a specified Quaternion rotation.
        /// </summary>
        /// <param name="vector">The Vector3 to rotate.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="result">An existing Vector3 filled in with the results of the rotation.</param>
        public static void Transform( ref Vector3 vector, ref Quaternion rotation, out Vector3 result )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rowW2X = rotation.W * rot2X;
            float rowW2Y = rotation.W * rot2Y;
            float rowW2Z = rotation.W * rot2Z;

            float rowX2X = rotation.X * rot2X;
            float rowX2Y = rotation.X * rot2Y;
            float rowX2Z = rotation.X * rot2Z;

            float rowY2Y = rotation.Y * rot2Y;
            float rotY2Z = rotation.Y * rot2Z;
            float rotZ2Z = rotation.Z * rot2Z;

            result.X = (vector.X * (1f - rowY2Y - rotZ2Z)) + (vector.Y * (rowX2Y - rowW2Z))      + (vector.Z * (rowX2Z + rowW2Y));
            result.Y = (vector.X * (rowX2Y + rowW2Z))      + (vector.Y * (1f - rowX2X - rotZ2Z)) + (vector.Z * (rotY2Z - rowW2X));
            result.Z = (vector.X * (rowX2Z - rowW2Y))      + (vector.Y * (rotY2Z + rowW2X))      + (vector.Z * (1f - rowX2X - rowY2Y));
        }

        #endregion

        #region Vector3[] by Matrix4

        /// <summary>
        /// Transforms a source array of Vector3s by a specified result and 
        /// writes the results to an existing destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="matrix">The transform result to apply.</param>
        /// <param name="destinationArray">An existing destination array into which the transformed Vector3s are written.</param>
        public static void Transform( Vector3[] sourceArray, ref Matrix4 matrix, Vector3[] destinationArray )
        {
            #region Verify Arguments

            if( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray );

            #endregion

            for( int i = 0; i < sourceArray.Length; ++i )
            {
                Vector3 vector = sourceArray[i];

                Vector3 transformed;
                transformed.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31) + matrix.M41;
                transformed.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32) + matrix.M42;
                transformed.Z = (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33) + matrix.M43;

                destinationArray[i] = transformed;
            }
        }

        /// <summary>
        /// Applies a specified transform result to a specified range of an array of Vector3s
        /// and writes the results into a specified range of a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="sourceIndex">The index in the source array at which to start.</param>
        /// <param name="matrix">The transform result to apply.</param>
        /// <param name="destinationArray">The existing destination array.</param>
        /// <param name="destinationIndex">The index in the destination array at which to start.</param>
        /// <param name="length">The number of Vector3s to transform.</param>
        public static void Transform( 
            Vector3[]   sourceArray, 
            int         sourceIndex, 
            ref Matrix4 matrix,
            Vector3[]   destinationArray, 
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
                Vector3 vector = sourceArray[sourceIndex];

                Vector3 transformed;
                transformed.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31) + matrix.M41;
                transformed.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32) + matrix.M42;
                transformed.Z = (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33) + matrix.M43;

                destinationArray[destinationIndex] = transformed;

                ++sourceIndex;
                ++destinationIndex;
                --length;
            }
        }

        #endregion

        #region Vector3[] Normal by Matrix4

        /// <summary>Transforms an array of 3D vector normals by a specified result.</summary>
        /// <param name="sourceArray">The array of Vector3 normals to transform.</param>
        /// <param name="matrix">The transform result to apply.</param>
        /// <param name="destinationArray">An existing Vector3 array into which the results of the transforms are written.</param>
        public static void TransformNormal( Vector3[] sourceArray, ref Matrix4 matrix, Vector3[] destinationArray )
        {
            #region Verify Arguments

            if( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray );

            #endregion

            for( int i = 0; i < sourceArray.Length; ++i )
            {
                Vector3 vector = sourceArray[i];
                Vector3 transformed;

                transformed.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31);
                transformed.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32);
                transformed.Z = (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33);

                destinationArray[i] = transformed;
            }
        }

        /// <summary>
        /// Transforms a specified range in an array of 3D vector normals 
        /// by a specified result and writes the results to a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array of Vector3 normals.</param>
        /// <param name="sourceIndex">The starting index in the source array.</param>
        /// <param name="matrix">The transform result to apply.</param>
        /// <param name="destinationArray">The destination Vector3 array.</param>
        /// <param name="destinationIndex">The starting index in the destination array.</param>
        /// <param name="length">The number of vectors to transform.</param>
        public static void TransformNormal( 
            Vector3[]   sourceArray,
            int         sourceIndex, 
            ref Matrix4 matrix, 
            Vector3[]   destinationArray, 
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
                Vector3 vector = sourceArray[sourceIndex];
                Vector3 transformed;

                transformed.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31);
                transformed.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32);
                transformed.Z = (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33);

                destinationArray[destinationIndex] = transformed;

                ++sourceIndex;
                ++destinationIndex;
                --length;
            }
        }

        #endregion

        #region Vector3[] by Quaternion

        /// <summary>
        /// Transforms a source array of Vector3s by a specified Quaternion rotation and writes the results to an existing destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="destinationArray">An existing destination array into which the transformed Vector3s are written.</param>
        public static void Transform( Vector3[] sourceArray, ref Quaternion rotation, Vector3[] destinationArray )
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
                Vector3 vector = sourceArray[i];
                Vector3 transformed;

                transformed.X = (vector.X * factorXX) + (vector.Y * factorYX) + (vector.Z * factorZX);
                transformed.Y = (vector.X * factorXY) + (vector.Y * factorYY) + (vector.Z * factorZY);
                transformed.Z = (vector.X * factorXZ) + (vector.Y * factorYZ) + (vector.Z * factorZZ);

                destinationArray[i] = transformed;
            }
        }

        /// <summary>
        /// Applies a specified Quaternion rotation to a specified range of an array of Vector3s 
        /// and writes the results into a specified range of a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="sourceIndex">The index in the source array at which to start.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="destinationArray">The existing destination array.</param>
        /// <param name="destinationIndex">The index in the destination array at which to start.</param>
        /// <param name="length">The number of Vector3s to transform.</param>
        public static void Transform(
            Vector3[]      sourceArray, 
            int            sourceIndex,      
            ref Quaternion rotation, 
            Vector3[]      destinationArray, 
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
                Vector3 vector = sourceArray[sourceIndex];
                Vector3 transformed;

                transformed.X = (vector.X * factorXX) + (vector.Y * factorYX) + (vector.Z * factorZX);
                transformed.Y = (vector.X * factorXY) + (vector.Y * factorYY) + (vector.Z * factorZY);
                transformed.Z = (vector.X * factorXZ) + (vector.Y * factorYZ) + (vector.Z * factorZZ);

                destinationArray[destinationIndex] = transformed;

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
        public static Vector3 Lerp( Vector3 start, Vector3 end, float amount )
        {
            Vector3 result;

            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
            result.Z = start.Z + ((end.Z - start.Z) * amount);

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
        public static void Lerp( ref Vector3 start, ref Vector3 end, float amount, out Vector3 result )
        {
            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
            result.Z = start.Z + ((end.Z - start.Z) * amount);
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
        public static Vector3 Coserp( Vector3 start, Vector3 end, float amount )
        {
            float endFactor = 0.5f * (1.0f - (float)System.Math.Cos( amount * System.Math.PI ));
            float startFactor = 1.0f - endFactor;

            Vector3 result;

            result.X = (start.X * startFactor) + (end.X * endFactor);
            result.Y = (start.Y * startFactor) + (end.Y * endFactor);
            result.Z = (start.Z * startFactor) + (end.Z * endFactor);

            return result;
        }

        /// <summary>
        /// Performs COSine intERPolation between two vectors.
        /// </summary>
        /// <param name="start">The source value that represents the start vector. This value will not be modified by this method.</param>
        /// <param name="end">The source value that represents the end vector. This value will not be modified by this method.</param>
        /// <param name="amount">The weighting factor.</param>
        /// <param name="result">Will contain the interpolated value.</param>
        public static void Coserp( ref Vector3 start, ref Vector3 end, float amount, out Vector3 result )
        {
            float endFactor = 0.5f * (1.0f - (float)System.Math.Cos( amount * System.Math.PI ));
            float startFactor = 1.0f - endFactor;

            result.X = (start.X * startFactor) + (end.X * endFactor);
            result.Y = (start.Y * startFactor) + (end.Y * endFactor);
            result.Z = (start.Z * startFactor) + (end.Z * endFactor);
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
        public static Vector3 SmoothStep( Vector3 start, Vector3 end, float amount )
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            Vector3 result;

            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
            result.Z = start.Z + ((end.Z - start.Z) * amount);

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
        public static void SmoothStep( ref Vector3 start, ref Vector3 end, float amount, out Vector3 result )
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
            result.Z = start.Z + ((end.Z - start.Z) * amount);
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
        /// <returns>The result of the Hermite spline interpolation.</returns>
        public static Vector3 Hermite(
            Vector3 valueA,
            Vector3 tangentA,
            Vector3 valueB, 
            Vector3 tangentB,
            float amount
        )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            float factorValueA = ((2f * amountPow3) - (3f * amountPow2)) + 1f;
            float factorValueB = (-2f * amountPow3) + (3f * amountPow2);
            float factorTangentA = (amountPow3 - (2f * amountPow2)) + amount;
            float factorTangentB = amountPow3 - amountPow2;

            Vector3 result;

            result.X = (valueA.X * factorValueA) + (valueB.X * factorValueB) + (tangentA.X * factorTangentA) + (tangentB.X * factorTangentB);
            result.Y = (valueA.Y * factorValueA) + (valueB.Y * factorValueB) + (tangentA.Y * factorTangentA) + (tangentB.Y * factorTangentB);
            result.Z = (valueA.Z * factorValueA) + (valueB.Z * factorValueB) + (tangentA.Z * factorTangentA) + (tangentB.Z * factorTangentB);

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
            ref Vector3 valueA, 
            ref Vector3 tangentA,
            ref Vector3 valueB,
            ref Vector3 tangentB,
            float amount,
            out Vector3 result
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
        public static Vector3 CatmullRom( Vector3 valueA, Vector3 valueB, Vector3 valueC, Vector3 valueD, float amount )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            Vector3 result;

            result.X = 0.5f * ((((2f * valueB.X) + ((-valueA.X + valueC.X) * amount)) + (((((2f * valueA.X) - (5f * valueB.X)) + (4f * valueC.X)) - valueD.X) * amountPow2)) + ((((-valueA.X + (3f * valueB.X)) - (3f * valueC.X)) + valueD.X) * amountPow3));
            result.Y = 0.5f * ((((2f * valueB.Y) + ((-valueA.Y + valueC.Y) * amount)) + (((((2f * valueA.Y) - (5f * valueB.Y)) + (4f * valueC.Y)) - valueD.Y) * amountPow2)) + ((((-valueA.Y + (3f * valueB.Y)) - (3f * valueC.Y)) + valueD.Y) * amountPow3));
            result.Z = 0.5f * ((((2f * valueB.Z) + ((-valueA.Z + valueC.Z) * amount)) + (((((2f * valueA.Z) - (5f * valueB.Z)) + (4f * valueC.Z)) - valueD.Z) * amountPow2)) + ((((-valueA.Z + (3f * valueB.Z)) - (3f * valueC.Z)) + valueD.Z) * amountPow3));

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
        public static void CatmullRom( ref Vector3 valueA, ref Vector3 valueB, ref Vector3 valueC, ref Vector3 valueD, float amount, out Vector3 result )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            result.X = 0.5f * ((((2f * valueB.X) + ((-valueA.X + valueC.X) * amount)) + (((((2f * valueA.X) - (5f * valueB.X)) + (4f * valueC.X)) - valueD.X) * amountPow2)) + ((((-valueA.X + (3f * valueB.X)) - (3f * valueC.X)) + valueD.X) * amountPow3));
            result.Y = 0.5f * ((((2f * valueB.Y) + ((-valueA.Y + valueC.Y) * amount)) + (((((2f * valueA.Y) - (5f * valueB.Y)) + (4f * valueC.Y)) - valueD.Y) * amountPow2)) + ((((-valueA.Y + (3f * valueB.Y)) - (3f * valueC.Y)) + valueD.Y) * amountPow3));
            result.Z = 0.5f * ((((2f * valueB.Z) + ((-valueA.Z + valueC.Z) * amount)) + (((((2f * valueA.Z) - (5f * valueB.Z)) + (4f * valueC.Z)) - valueD.Z) * amountPow2)) + ((((-valueA.Z + (3f * valueB.Z)) - (3f * valueC.Z)) + valueD.Z) * amountPow3));
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
        public static Vector3 Add( Vector3 left, Vector3 right )
        {
            Vector3 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the <paramref name="right"/> Vector to the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Vector3 left, ref Vector3 right, out Vector3 result )
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 Add( Vector3 vector, float scalar )
        {
            Vector3 result;

            result.X = vector.X + scalar;
            result.Y = vector.Y + scalar;
            result.Z = vector.Z + scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the given <paramref name="scalar"/> to the given <paramref name="vector"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation. </param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Vector3 vector, float scalar, out Vector3 result )
        {
            result.X = vector.X + scalar;
            result.Y = vector.Y + scalar;
            result.Z = vector.Z + scalar;
        }

        /// <summary>
        /// This method returns the specified Vector.
        /// </summary>
        /// <remarks>
        /// Is equal to "+vector".
        /// </remarks>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 Plus( Vector3 vector )
        {
            return vector;
        }

        /// <summary>
        /// This method stores the specified Vector in the specified result value.
        /// </summary>
        /// <remarks>
        /// Is equal to "+vector".
        /// </remarks>
        /// <param name="vector">The vector.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Plus( ref Vector3 vector, out Vector3 result )
        {
            result.X = vector.X;
            result.Y = vector.Y;
            result.Z = vector.Z;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Vector from the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 Subtract( Vector3 left, Vector3 right )
        {
            Vector3 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the <paramref name="right"/> Vector frpm the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Vector3 left, ref Vector3 right, out Vector3 result )
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 Subtract( Vector3 vector, float scalar )
        {
            Vector3 result;

            result.X = vector.X - scalar;
            result.Y = vector.Y - scalar;
            result.Z = vector.Z - scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="vector"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Vector3 vector, float scalar, out Vector3 result )
        {
            result.X = vector.X - scalar;
            result.Y = vector.Y - scalar;
            result.Z = vector.Z - scalar;
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
        public static Vector3 Negate( Vector3 vector )
        {
            Vector3 result;

            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;

            return result;
        }

        /// <summary>
        /// Stores the result of negating the elements of the given <paramref name="vector"/> in the given Vector.
        /// </summary>
        /// <param name="vector">
        /// The vector to negate. This value will not be modified by this method.
        /// </param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Negate( ref Vector3 vector, out Vector3 result )
        {
            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Returns the result of multiplying the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 Multiply( Vector3 vector, float scalar )
        {
            Vector3 result;

            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;
            result.Z = vector.Z * scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// in the given Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Multiply( ref Vector3 vector, float scalar, out Vector3 result )
        {
            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;
            result.Z = vector.Z * scalar;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 Multiply( Vector3 left, Vector3 right )
        {
            Vector3 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the left Vector by the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The vector on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Multiply( ref Vector3 left, ref Vector3 right, out Vector3 result )
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Returns the result of dividing the given <paramref name="vector"/> through the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 Divide( Vector3 vector, float scalar )
        {
            Vector3 result;

            result.X = vector.X / scalar;
            result.Y = vector.Y / scalar;
            result.Z = vector.Z / scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the given <paramref name="vector"/> through the given <paramref name="scalar"/>
        /// in the given result Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Divide( ref Vector3 vector, float scalar, out Vector3 result )
        {
            result.X = vector.X / scalar;
            result.Y = vector.Y / scalar;
            result.Z = vector.Z / scalar;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 Divide( Vector3 left, Vector3 right )
        {
            Vector3 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the left Vector through the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The vector on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will store the result of the operation.</param>
        public static void Divide( ref Vector3 left, ref Vector3 right, out Vector3 result )
        {
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
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
        public static Vector3 Max( Vector3 vectorA, Vector3 vectorB )
        {
            Vector3 result;

            result.X = (vectorA.X > vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y > vectorB.Y) ? vectorA.Y : vectorB.Y;
            result.Z = (vectorA.Z > vectorB.Z) ? vectorA.Z : vectorB.Z;

            return result;
        }

        /// <summary>
        /// Returns a vector that contains the highest value from
        /// each matching pair of components of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second vector. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the maximized vector.</param>
        public static void Max( ref Vector3 vectorA, ref Vector3 vectorB, out Vector3 result )
        {
            result.X = (vectorA.X > vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y > vectorB.Y) ? vectorA.Y : vectorB.Y;
            result.Z = (vectorA.Z > vectorB.Z) ? vectorA.Z : vectorB.Z;
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
        public static Vector3 Min( Vector3 vectorA, Vector3 vectorB )
        {
            Vector3 result;

            result.X = (vectorA.X < vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y < vectorB.Y) ? vectorA.Y : vectorB.Y;
            result.Z = (vectorA.Z < vectorB.Z) ? vectorA.Z : vectorB.Z;

            return result;
        }

        /// <summary>
        /// Returns a vector that contains the lowest value from
        /// each matching pair of components of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second vector. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the minimized vector.</param>
        public static void Min( ref Vector3 vectorA, ref Vector3 vectorB, out Vector3 result )
        {
            result.X = (vectorA.X < vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y < vectorB.Y) ? vectorA.Y : vectorB.Y;
            result.Z = (vectorA.Z < vectorB.Z) ? vectorA.Z : vectorB.Z;
        }

        #endregion

        #region Average

        /// <summary>
        /// Returns the average of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector.</param>
        /// <param name="vectorB">The second vector.</param>
        /// <returns>The average of the given Vectors.</returns>
        public static Vector3 Average( Vector3 vectorA, Vector3 vectorB )
        {
            Vector3 result;

            result.X = (vectorA.X + vectorB.X) * 0.5f;
            result.Y = (vectorA.Y + vectorB.Y) * 0.5f;
            result.Z = (vectorA.Z + vectorB.Z) * 0.5f;

            return result;
        }

        /// <summary>
        /// Returns the average of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second vector. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the average of the given Vectors.</param>
        public static void Average( ref Vector3 vectorA, ref Vector3 vectorB, out Vector3 result )
        {
            result.X = (vectorA.X + vectorB.X) * 0.5f;
            result.Y = (vectorA.Y + vectorB.Y) * 0.5f;
            result.Z = (vectorA.Z + vectorB.Z) * 0.5f;
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
        public static float Distance( Vector3 vectorA, Vector3 vectorB )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;
            float deltaZ = vectorA.Z - vectorB.Z;

            float squaredLength = (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ);
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
        public static void Distance( ref Vector3 vectorA, ref Vector3 vectorB, out float result )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;
            float deltaZ = vectorA.Z - vectorB.Z;

            float squaredLength = (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ);
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
        public static float DistanceSquared( Vector3 vectorA, Vector3 vectorB )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;
            float deltaZ = vectorA.Z - vectorB.Z;

            return (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ);
        }

        /// <summary>
        /// Stores the squared distance between the two specified Vectors2 in the given <paramref name="result"/> value.
        /// </summary>
        /// <param name="vectorA">The first Vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second Vector. This value will not be modified by this method.</param>
        /// <param name="result">
        /// This value will contain the squared distance from the <paramref name="vectorA"/> to <paramref name="vectorB"/>.
        /// </param>
        public static void DistanceSquared( ref Vector3 vectorA, ref Vector3 vectorB, out float result )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;
            float deltaZ = vectorA.Z - vectorB.Z;

            result = (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ);
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
        public static Vector3 Clamp( Vector3 vector, Vector3 min, Vector3 max )
        {
            Vector3 result;

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
        public static void Clamp( ref Vector3 vector, ref Vector3 min, ref Vector3 max, out Vector3 result )
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
        }

        #endregion

        #region Angle

        /// <summary>
        /// Returns the angle between the specified Vectors.
        /// </summary>
        /// <remarks>
        /// angle = atan2(length(cross(A,B)), dot(A,B))
        /// </remarks>
        /// <param name="from">The start vector.</param>
        /// <param name="to">The end vector.</param>
        /// <returns>
        /// The angle from the 'from' vector to the 'to' vector.
        /// </returns>
        public static float Angle( Vector3 from, Vector3 to )
        {
            Vector3 cross;
            Cross( ref from, ref to, out cross );

            float dot;
            Dot( ref from, ref to, out dot );

            return (float)System.Math.Atan2( cross.Length, dot );
        }

        /// <summary>
        /// Stores the angle between the specified Vectors
        /// in the specified <paramref name="result"/> value.
        /// </summary>
        /// <remarks>
        /// angle = atan2(length(cross(A,B)), dot(A,B))
        /// </remarks>
        /// <param name="from">The start vector. This value will not be modified by this method.</param>
        /// <param name="to">The end vector. This value will not be modified by this method.</param>
        /// <param name="result">
        /// The angle from the 'from' vector to the 'to' vector.
        /// </param>
        public static void Angle( ref Vector3 from, ref Vector3 to, out float result )
        {
            Vector3 cross;
            Cross( ref from, ref to, out cross );

            float dot;
            Dot( ref from, ref to, out dot );

            result = (float)System.Math.Atan2( cross.Length, dot );
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether the given <see cref="Vector3"/> has the
        /// same indices set as this Vector3.
        /// </summary>
        /// <param name="other">The Vector3 to test against.</param>      
        /// <returns>
        /// Returns <see langword="true"/> if the elements of the vectors are approximately equal; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( Vector3 other )
        {
            return this.X.IsApproximate( other.X ) &&
                   this.Y.IsApproximate( other.Y ) &&
                   this.Z.IsApproximate( other.Z );
        }

        /// <summary>
        /// Returns whether the given <see cref="Object"/> is equal to this Vector3.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public override bool Equals( object obj )
        {
            if( obj is Vector3 )
            {
                return Equals( (Vector3)obj );
            }
            else if( obj is Point3 )
            {
                Point3 vector = (Point3)obj;

                return this.X.IsApproximate( (float)vector.X ) &&
                       this.Y.IsApproximate( (float)vector.Y ) &&
                       this.Z.IsApproximate( (float)vector.Z );
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overriden to return a human-readable text representation of the Vector3.
        /// </summary>
        /// <returns>A human-readable text representation of the Vector3.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Vector3.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Vector3.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "[{0} {1} {2}]",
                X.ToString( formatProvider ),
                Y.ToString( formatProvider ),
                Z.ToString( formatProvider )
            );
        }

        #endregion

        /// <summary>
        /// Overriden to return the hashcode of the <see cref="Vector3"/>.
        /// </summary>
        /// <returns>The simple xor-ed hashcode.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.X );
            hashBuilder.AppendStruct( this.Y );
            hashBuilder.AppendStruct( this.Z );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #endregion

        #region [ Operators ]

        #region Logic

        /// <summary>
        /// Returns whether the specified <see cref="Vector3"/> instances are (approximately) equal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator ==( Vector3 left, Vector3 right )
        {
            return left.X.IsApproximate( right.X ) &&
                   left.Y.IsApproximate( right.Y ) &&
                   left.Z.IsApproximate( right.Z );
        }

        /// <summary>
        /// Returns whether the specified <see cref="Vector3"/> instances are (approximately) inequal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator !=( Vector3 left, Vector3 right )
        {
            return !left.X.IsApproximate( right.X ) ||
                   !left.Y.IsApproximate( right.Y ) ||
                   !left.Z.IsApproximate( right.Z );
        }

        #endregion

        #region Cast

        /// <summary>
        /// Explicit cast operator that implements conversion
        /// from a <see cref="Vector3"/> to a <see cref="Point3"/>.
        /// </summary>
        /// <param name="vector">
        /// The input vector.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static explicit operator Point3( Vector3 vector )
        {
            return new Point3( (int)vector.X, (int)vector.Y, (int)vector.Z );
        }

        #endregion

        #region +

        /// <summary>
        /// Returns the result of adding the <paramref name="right"/> Vector to the the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 operator +( Vector3 left, Vector3 right )
        {
            Vector3 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;

            return result;
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the thegiven <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 operator +( Vector3 vector, float scalar )
        {
            Vector3 result;

            result.X = vector.X + scalar;
            result.Y = vector.Y + scalar;
            result.Z = vector.Z + scalar;

            return result;
        }

        /// <summary>
        /// Returns the original specified vector, doing nothing.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 operator +( Vector3 vector )
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
        public static Vector3 operator -( Vector3 left, Vector3 right )
        {
            Vector3 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 operator -( Vector3 vector, float scalar )
        {
            Vector3 result;

            result.X = vector.X - scalar;
            result.Y = vector.Y - scalar;
            result.Z = vector.Z - scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of negating the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 operator -( Vector3 vector )
        {
            Vector3 result;

            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;

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
        public static Vector3 operator *( Vector3 vector, float scalar )
        {
            Vector3 result;

            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;
            result.Z = vector.Z * scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of multiplcing the given <paramref name="scalar"/> by the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="scalar">The scalar on the left side of the equation.</param>
        /// <param name="vector">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 operator *( float scalar, Vector3 vector )
        {
            Vector3 result;

            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;
            result.Z = vector.Z * scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 operator *( Vector3 left, Vector3 right )
        {
            Vector3 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;

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
        public static Vector3 operator /( Vector3 vector, float scalar )
        {
            Vector3 result;

            result.X = vector.X / scalar;
            result.Y = vector.Y / scalar;
            result.Z = vector.Z / scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector element-by-element.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector3 operator /( Vector3 left, Vector3 right )
        {
            Vector3 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;

            return result;
        }

        #endregion

        #endregion
    }
}
