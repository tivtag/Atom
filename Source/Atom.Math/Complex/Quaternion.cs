// <copyright file="Quaternion.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Quaternion structure.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Defines a Quaternion (V, s). 
    /// Where V is the point part (x, y, z) and
    /// s the scalar part (w) of the Quaternion.
    /// Quaternions can be used to represent rotation in 3D space.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.QuaternionConverter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Quaternion : IEquatable<Quaternion>, System.Collections.Generic.IEnumerable<float>, 
                               ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>
        /// The X-value of the point component of this <see cref="Quaternion"/>.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y-value of the point component of this <see cref="Quaternion"/>.
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z-value of the point component of this <see cref="Quaternion"/>.
        /// </summary>
        public float Z;

        /// <summary>
        /// The value of the scalar component of this <see cref="Quaternion"/>.
        /// </summary>
        public float W;

        #endregion

        #region [ Constants ]

        /// <summary>
        /// Gets the identity <see cref="Quaternion"/>, Which is a Quaternion that represents no rotation.
        /// </summary>
        /// <value>The identity Quaternion (0, 0, 0, 1).</value>
        public static Quaternion Identity
        { 
            get
            { 
                return new Quaternion( 0.0f, 0.0f, 0.0f, 1.0f ); 
            }
        }

        #endregion

        #region [ Properties ]

        #region Length

        /// <summary> 
        /// Gets the 'length' (also called norm) of this <see cref="Quaternion"/>.
        /// </summary>
        /// <value>
        /// The length (also called norm and magnitude) of this Quaternion.
        /// </value>
        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt( (X * X) + (Y * Y) + (Z * Z) + (W * W) );
            }
        }

        /// <summary>
        /// Gets the squared 'length' (also called norm) of this <see cref="Quaternion"/>.
        /// </summary>
        /// <value>
        /// The squared length (also called norm and magnitude) of this Quaternion.
        /// </value>
        public float SquaredLength
        {
            get
            {
                return (X * X) + (Y * Y) + (Z * Z) + (W * W);
            }
        }

        #endregion
        
        #region Axis

        /// <summary>
        /// Gets the local X-axis portion of the rotation this <see cref="Quaternion"/> descripes.
        /// </summary>
        /// <value>The local X-axis portion of the rotation.</value>
        public Vector3 AxisX
        {
            get
            {
                float twoY = 2.0f * Y;
                float twoZ = 2.0f * Z;
                float twoYX = twoY * X;
                float twoYY = twoY * Y;
                float twoYW = twoY * W;
                float twoZX = twoZ * X;
                float twoZZ = twoZ * Z;
                float twoZW = twoZ * W;

                return new Vector3( 1.0f - (twoYY + twoZZ), twoYX + twoZW, twoZX - twoYW );
            }
        }

        /// <summary>
        /// Gets the local Y-axis portion of the rotation this <see cref="Quaternion"/> descripes.
        /// </summary>
        /// <value>The local Y-axis portion of the rotation.</value>
        public Vector3 AxisY
        {
            get
            {
                float twoX = 2.0f * X;
                float twoY = 2.0f * Y;
                float twoZ = 2.0f * Z;

                float twoXX = twoX * X;
                float twoXW = twoX * W;
                float twoYX = twoY * X;
                float twoZY = twoZ * Y;
                float twoZZ = twoZ * Z;
                float twoZW = twoZ * W;

                return new Vector3( twoYX - twoZW, 1.0f - (twoXX + twoZZ), twoZY + twoXW );
            }
        }

        /// <summary>
        /// Gets the local Z-axis portion of the rotation this <see cref="Quaternion"/> descripes.
        /// </summary>
        /// <value>The local Z-axis portion of the rotation.</value>
        public Vector3 AxisZ
        {
            get
            {
                float twoX = 2.0f * X;
                float twoY = 2.0f * Y;
                float twoZ = 2.0f * Z;

                float twoXX = twoX * X;
                float twoXW = twoX * W;
                float twoYY = twoY * Y;
                float twoYW = twoY * W;
                float twoZX = twoZ * X;
                float twoZY = twoZ * Y;

                return new Vector3( twoZX + twoYW, twoZY - twoXW, 1.0f - (twoXX + twoYY) );
            }
        }

        #endregion

        #region Conjugate

        /// <summary> 
        /// Gets or sets the conjugate of this <see cref="Quaternion"/>.
        /// </summary>
        /// <remarks>
        /// The conjugate of a Quaterion is the same quaternion 
        /// but with a negated point component.
        /// </remarks>
        /// <value>The conjugate of this Quaternion.</value>
        public Quaternion Conjugate
        {
            get { return new Quaternion( -X, -Y, -Z, W ); }
            set { this = value.Conjugate; }
        }

        #endregion

        #region Inverse

        /// <summary> 
        /// Gets the inverse of this <see cref="Quaternion"/>.
        /// </summary>
        /// <value>The inverse of this Quaternion.</value>
        public Quaternion Inverse
        {
            get 
            {
                float squaredLength = (X * X) + (Y * Y) + (Z * Z) + (W * W);
                float factor        = 1.0f / squaredLength;

                Quaternion result;

                result.X = X * -factor;
                result.Y = Y * -factor;
                result.Z = Z * -factor;
                result.W = W * factor;

                return result;
            }
        }

        #endregion

        #region Log

        /// <summary>
        /// Gets the natural logarithm of this <see cref="Quaternion"/>.
        /// </summary>
        /// <value>The natural logarithm of this Quaternion.</value>
        public Quaternion Log
        {
            get
            {
                // If q = cos(A)+sin(A)*(x*i+y*j+z*k) where (x,y,z) is unit length, then
                // log(q) = A*(x*i+y*j+z*k).  If sin(A) is near zero, use log(q) =
                // sin(A)*(x*i+y*j+z*k) since sin(A)/A has limit 1.
                Quaternion result = new Quaternion();

                if( System.Math.Abs( W ) < 1.0f )
                {
                    float angle = (float)System.Math.Acos( W );
                    float sin   = (float)System.Math.Sin( angle );

                    if( System.Math.Abs( sin ) >= 1e-03f )
                    {
                        float coeff = angle / sin;

                        result.X = coeff * X;
                        result.Y = coeff * Y;
                        result.Z = coeff * Z;
                    }
                    else
                    {
                        result.X = X;
                        result.Y = Y;
                        result.Z = Z;
                    }
                }

                return result;
            }
        }

        #endregion

        #region Exp

        /// <summary>
        /// Gets the exponential of this <see cref="Quaternion"/>.
        /// </summary>
        /// <value>The exponential of this Quaternion.</value>
        public Quaternion Exp
        {
            get
            {
                // If q = A*(x*i+y*j+z*k) where (x,y,z) is unit length, then
                // exp(q) = cos(A)+sin(A)*(x*i+y*j+z*k).  If sin(A) is near zero,
                // use exp(q) = cos(A)+A*(x*i+y*j+z*k) since A/sin(A) has limit 1.
                float angle = (float)System.Math.Sqrt( (X * X) + (Y * Y) + (Z * Z) );
                float sin   = (float)System.Math.Sin( angle );

                Quaternion result;
                result.W = (float)System.Math.Cos( angle );

                if( System.Math.Abs( sin ) >= 1e-03f )
                {
                    float coeff = sin / angle;

                    result.X = coeff * X;
                    result.Y = coeff * Y;
                    result.Z = coeff * Z;
                }
                else
                {
                    result.X = X;
                    result.Y = Y;
                    result.Z = Z;
                }

                return result;
            }
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="x">
        /// The x-value of the point component of the new Quaternion.
        /// </param>
        /// <param name="y">
        /// The y-value of the point component of the new Quaternion.
        /// </param>
        /// <param name="z">
        /// The z-value of the point component of the new Quaternion.
        /// </param>
        /// <param name="w">
        /// The value of the scalar component of the new Quaternion.
        /// </param>
        public Quaternion( float x, float y, float z, float w )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        } 

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="vectorPart">
        /// The point component of the new Quaternion.
        /// </param>
        /// <param name="scalarPart">
        /// The scalar component of the new Quaternion.
        /// </param>
        public Quaternion( Vector3 vectorPart, float scalarPart )
        {
            this.X = vectorPart.X;
            this.Y = vectorPart.Y;
            this.Z = vectorPart.Z;
            this.W = scalarPart;
        } 

        #endregion

        #region [ Methods ]

        #region Normalize

        /// <summary>
        /// Normalizes this Quaternion, setting its length to one.
        /// </summary>
        public void Normalize()
        {
            float squaredLength = (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z) + (this.W * this.W);
            float invLength     = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            this.X *= invLength;
            this.Y *= invLength;
            this.Z *= invLength;
            this.W *= invLength;
        }

        /// <summary>
        /// Returns the result of normalizing the given Quaternion.
        /// </summary>
        /// <param name="quaternion">The quaternion to normalize.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static Quaternion Normalize( Quaternion quaternion )
        {
            float squaredLength = (quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y) + (quaternion.Z * quaternion.Z) + (quaternion.W * quaternion.W);
            float invLength = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            Quaternion result;

            result.X = quaternion.X * invLength;
            result.Y = quaternion.Y * invLength;
            result.Z = quaternion.Z * invLength;
            result.W = quaternion.W * invLength;

            return result;
        }

        /// <summary>
        /// Stores the result of normalizing the given Vector in the given <paramref name="result"/> Vector.
        /// </summary>
        /// <param name="quaternion">The quaternion to normalize. This value will not be modified by this method.</param>
        /// <param name="result">This value will store the result of the operation.</param>
        public static void Normalize( ref Quaternion quaternion, out Quaternion result )
        {
            float squaredLength = (quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y) + (quaternion.Z * quaternion.Z) + (quaternion.Z * quaternion.Z);
            float invLength     = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            result.X = quaternion.X * invLength;
            result.Y = quaternion.Y * invLength;
            result.Z = quaternion.Z * invLength;
            result.W = quaternion.W * invLength;
        }

        #endregion

        #region Invert

        /// <summary>
        /// Inverts this <see cref="Quaternion"/>.
        /// </summary>
        public void Invert()
        {
            float squaredLength = (X * X) + (Y * Y) + (Z * Z) + (W * W);
            float factor        = 1.0f / squaredLength;

            this.X *= -factor;
            this.Y *= -factor;
            this.Z *= -factor;
            this.W *=  factor;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Returns the dot(scalar) product of the given <see cref="Quaternion"/>s.
        /// </summary>
        /// <param name="left">The Quaternion on the left side of the equation.</param>
        /// <param name="right">The Quaternion on the right side of the equation.</param>
        /// <returns>The dot product.</returns>
        public static float Dot( Quaternion left, Quaternion right )
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
        }

        /// <summary>
        /// Returns the dot(scalar) product of the given <see cref="Quaternion"/>s.
        /// </summary>
        /// <param name="left">The Quaternion on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The Quaternion on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">This value will contain the result of this operation.</param>
        public static void Dot( ref Quaternion left, ref Quaternion right, out float result )
        {
            result = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
        }

        #endregion

        #region Concatenate

        /// <summary>
        /// Concatenates two Quaternions; the result represents 
        /// the <paramref name="left"/> rotation followed by the <paramref name="right"/> rotation.
        /// </summary>
        /// <param name="left">The first Quaternion rotation in the series.</param>
        /// <param name="right">The second Quaternion rotation in the series.</param>
        /// <returns>
        /// A new Quaternion representing the concatenation of the <paramref name="left"/> rotation 
        /// followed by the <paramref name="right"/> rotation. 
        /// </returns>
        public static Quaternion Concatenate( Quaternion left, Quaternion right )
        {
            float factorX = (right.Y * left.Z) - (right.Z * left.Y);
            float factorY = (right.Z * left.X) - (right.X * left.Z);
            float factorZ = (right.X * left.Y) - (right.Y * left.X);
            float factorW = (right.X * left.X) + (right.Y * left.Y) + (right.Z * left.Z);

            Quaternion result;

            result.X = (right.X * left.W) + (left.X * right.W) + factorX;
            result.Y = (right.Y * left.W) + (left.Y * right.W) + factorY;
            result.Z = (right.Z * left.W) + (left.Z * right.W) + factorZ;
            result.W = (right.W * left.W) - factorW;

            return result;
        }

        #endregion

        #region > Operators <

        #region Add

        /// <summary>
        /// Returns the result of adding the <paramref name="right"/> Quaternion to the <paramref name="left"/> Quaternion.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion Add( Quaternion left, Quaternion right )
        {
            Quaternion result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the <paramref name="right"/> Quaternion to the <paramref name="left"/> Quaternion
        /// in the given Quaternion.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Quaternion left, ref Quaternion right, out Quaternion result )
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;
        }

        /// <summary>
        /// This method returns the specified Quaternion.
        /// </summary>
        /// <remarks>
        /// Is equal to "+Quaternion".
        /// </remarks>
        /// <param name="quaternion">
        /// The input Quaternion.
        /// </param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion Plus( Quaternion quaternion )
        {
            return quaternion;
        }

        /// <summary>
        /// This method stores the specified Quaternion in the specified result value.
        /// </summary>
        /// <remarks>
        /// Is equal to "+Quaternion".
        /// </remarks>
        /// <param name="quaternion">
        /// The input Quaternion.
        /// </param>
        /// <param name="result">
        /// Will contain the result of the operation.
        /// </param>
        public static void Plus( ref Quaternion quaternion, out Quaternion result )
        {
            result.X = quaternion.X;
            result.Y = quaternion.Y;
            result.Z = quaternion.Z;
            result.W = quaternion.W;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Quaternion from the <paramref name="left"/> Quaternion.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion Subtract( Quaternion left, Quaternion right )
        {
            Quaternion result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the <paramref name="right"/> Quaternion frpm the <paramref name="left"/> Quaternion
        /// in the given Quaternion.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Quaternion left, ref Quaternion right, out Quaternion result )
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;
        }

        #endregion

        #region Negate

        /// <summary>
        /// Returns the result of negating the elements of the given <paramref name="quaternion"/>.
        /// </summary>
        /// <param name="quaternion">
        /// The Quaternion to negate.
        /// </param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion Negate( Quaternion quaternion )
        {
            Quaternion result;

            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = -quaternion.W;

            return result;
        }

        /// <summary>
        /// Stores the result of negating the elements of the given <paramref name="quaternion"/> in the given Quaternion.
        /// </summary>
        /// <param name="quaternion">
        /// The Quaternion to negate. This value will not be modified by this method.
        /// </param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Negate( ref Quaternion quaternion, out Quaternion result )
        {
            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = -quaternion.W;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Returns the result of multiplying the given <paramref name="quaternion"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="quaternion">The Quaternion on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion Multiply( Quaternion quaternion, float scalar )
        {
            Quaternion result;

            result.X = quaternion.X * scalar;
            result.Y = quaternion.Y * scalar;
            result.Z = quaternion.Z * scalar;
            result.W = quaternion.W * scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the given <paramref name="quaternion"/> by the given <paramref name="scalar"/>.
        /// in the given Quaternion.
        /// </summary>
        /// <param name="quaternion">
        /// The Quaternion on the left side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Multiply( ref Quaternion quaternion, float scalar, out Quaternion result )
        {
            result.X = quaternion.X * scalar;
            result.Y = quaternion.Y * scalar;
            result.Z = quaternion.Z * scalar;
            result.W = quaternion.W * scalar;
        }
                
        /// <summary>
        /// Stores the result of multiplying the given <paramref name="quaternion"/> by the given <paramref name="scalar"/>.
        /// in the given Quaternion.
        /// </summary>
        /// <param name="scalar">
        /// The scalar on the left side of the equation.
        /// </param>
        /// <param name="quaternion">
        /// The Quaternion on the right side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="result">
        /// Will contain the result fo the operation.
        /// </param>
        public static void Multiply( float scalar, ref Quaternion quaternion, out Quaternion result )
        {
            result.X = scalar * quaternion.X;
            result.Y = scalar * quaternion.Y;
            result.Z = scalar * quaternion.Z;
            result.W = scalar * quaternion.W;
        }

        /// <summary>
        /// Returns the result of multiplying the left Quaternion by the right Quaternion.
        /// </summary>
        /// <param name="left">The Quaternion on the left side of the equation.</param>
        /// <param name="right">The Quaternion on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion Multiply( Quaternion left, Quaternion right )
        {
            float factorX = (left.Y * right.Z) - (left.Z * right.Y);
            float factorY = (left.Z * right.X) - (left.X * right.Z);
            float factorZ = (left.X * right.Y) - (left.Y * right.X);
            float factorW = ((left.X * right.X) + (left.Y * right.Y)) + (left.Z * right.Z);

            Quaternion result;

            result.X = ((left.X * right.W) + (right.X * left.W)) + factorX;
            result.Y = ((left.Y * right.W) + (right.Y * left.W)) + factorY;
            result.Z = ((left.Z * right.W) + (right.Z * left.W)) + factorZ;
            result.W = (left.W * right.W) - factorW;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the left Quaternion by the right Quaternion.
        /// in the given result Quaternion.
        /// </summary>
        /// <param name="left">The Quaternion on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The Quaternion on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Multiply( ref Quaternion left, ref Quaternion right, out Quaternion result )
        {
            float factorX = (left.Y * right.Z) - (left.Z * right.Y);
            float factorY = (left.Z * right.X) - (left.X * right.Z);
            float factorZ = (left.X * right.Y) - (left.Y * right.X);
            float factorW = ((left.X * right.X) + (left.Y * right.Y)) + (left.Z * right.Z);

            result.X = ((left.X * right.W) + (right.X * left.W)) + factorX;
            result.Y = ((left.Y * right.W) + (right.Y * left.W)) + factorY;
            result.Z = ((left.Z * right.W) + (right.Z * left.W)) + factorZ;
            result.W = (left.W * right.W) - factorW;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Returns the result of dividing the left Quaternion through the right Quaternion component-by-component.
        /// </summary>
        /// <param name="left">The Quaternion on the left side of the equation.</param>
        /// <param name="right">The Quaternion on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion Divide( Quaternion left, Quaternion right )
        {
            // invert right inline:
            float factor    = 1f / right.SquaredLength;            
            float invRightX = -right.X * factor;
            float invRightY = -right.Y * factor;
            float invRightZ = -right.Z * factor;
            float invRightW = right.W * factor;

            // do Multiply( left, invRight ):
            float factorX = (left.Y * invRightZ) - (left.Z * invRightY);
            float factorY = (left.Z * invRightX) - (left.X * invRightZ);
            float factorZ = (left.X * invRightY) - (left.Y * invRightX);
            float factorW = ((left.X * invRightX) + (left.Y * invRightY)) + (left.Z * invRightZ);

            Quaternion result;

            result.X = ((left.X * invRightW) + (invRightX * left.W)) + factorX;
            result.Y = ((left.Y * invRightW) + (invRightY * left.W)) + factorY;
            result.Z = ((left.Z * invRightW) + (invRightZ * left.W)) + factorZ;
            result.W = (left.W * invRightW) - factorW;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the left Quaternion through the right Quaternion component-by-component.
        /// in the given result Quaternion.
        /// </summary>
        /// <param name="left">The Quaternion on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The Quaternion on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will store the result of the operation.</param>
        public static void Divide( ref Quaternion left, ref Quaternion right, out Quaternion result )
        {
            // invert right inline:
            float factor    = 1f / right.SquaredLength;
            float invRightX = -right.X * factor;
            float invRightY = -right.Y * factor;
            float invRightZ = -right.Z * factor;
            float invRightW = right.W * factor;

            // do Multiply( left, invRight ):
            float factorX = (left.Y * invRightZ) - (left.Z * invRightY);
            float factorY = (left.Z * invRightX) - (left.X * invRightZ);
            float factorZ = (left.X * invRightY) - (left.Y * invRightX);
            float factorW = ((left.X * invRightX) + (left.Y * invRightY)) + (left.Z * invRightZ);

            result.X = ((left.X * invRightW) + (invRightX * left.W)) + factorX;
            result.Y = ((left.Y * invRightW) + (invRightY * left.W)) + factorY;
            result.Z = ((left.Z * invRightW) + (invRightZ * left.W)) + factorZ;
            result.W = (left.W * invRightW) - factorW;
        }

        #endregion

        #endregion

        #region > Interpolation <

        #region Lerp

        /// <summary>
        /// Performs a Linear intERPolation between two <see cref="Quaternion"/>s.
        /// </summary>
        /// <param name="start">
        /// The source Quaternion that represents the start value.
        /// </param>
        /// <param name="end">
        /// The source Quaternion that represents the end value.
        /// </param>
        /// <param name="amount">
        /// Value indicating how far to interpolate between the quaternions.
        /// </param>
        /// <returns>The linear interpolation of the two Quaternions.</returns>
        public static Quaternion Lerp( Quaternion start, Quaternion end, float amount )
        {
            float startAmount = 1f - amount;
            float endAmount   = amount;
            float dotProduct  = (start.X * end.X) + (start.Y * end.Y) + (start.Z * end.Z) + (start.W * end.W);

            Quaternion result;

            if( dotProduct >= 0f )
            {
                result.X = (startAmount * start.X) + (endAmount * end.X);
                result.Y = (startAmount * start.Y) + (endAmount * end.Y);
                result.Z = (startAmount * start.Z) + (endAmount * end.Z);
                result.W = (startAmount * start.W) + (endAmount * end.W);
            }
            else
            {
                result.X = (startAmount * start.X) - (endAmount * end.X);
                result.Y = (startAmount * start.Y) - (endAmount * end.Y);
                result.Z = (startAmount * start.Z) - (endAmount * end.Z);
                result.W = (startAmount * start.W) - (endAmount * end.W);
            }

            result.Normalize();
            return result;
        }

        /// <summary>
        /// Performs a Linear intERPolation between two <see cref="Quaternion"/>s.
        /// </summary>
        /// <param name="start">
        /// The source Quaternion that represents the start value. This value will not be modified by this method.
        /// </param>
        /// <param name="end">
        /// The source Quaternion that represents the end value. This value will not be modified by this method.
        /// </param>
        /// <param name="amount">
        /// Value indicating how far to interpolate between the quaternions.
        /// </param>
        /// <param name="result">
        /// Will contain the linear interpolation of the two Quaternions.
        /// </param>
        public static void Lerp( ref Quaternion start, ref Quaternion end, float amount, out Quaternion result )
        {
            float startAmount = 1f - amount;
            float endAmount   = amount;

            float dotProduct = (start.X * end.X) + (start.Y * end.Y) + (start.Z * end.Z) + (start.W * end.W);

            if( dotProduct >= 0f )
            {
                result.X = (startAmount * start.X) + (endAmount * end.X);
                result.Y = (startAmount * start.Y) + (endAmount * end.Y);
                result.Z = (startAmount * start.Z) + (endAmount * end.Z);
                result.W = (startAmount * start.W) + (endAmount * end.W);
            }
            else
            {
                result.X = (startAmount * start.X) - (endAmount * end.X);
                result.Y = (startAmount * start.Y) - (endAmount * end.Y);
                result.Z = (startAmount * start.Z) - (endAmount * end.Z);
                result.W = (startAmount * start.W) - (endAmount * end.W);
            }

            result.Normalize();
        }

        #endregion

        #region Slerp

        /// <summary>
        /// Performs a Sperical Linear intERPolation between two <see cref="Quaternion"/>s.
        /// </summary>
        /// <param name="start">
        /// The source Quaternion that represents the start value.
        /// </param>
        /// <param name="end">
        /// The source Quaternion that represents the end value.
        /// </param>
        /// <param name="amount">
        /// Value indicating how far to interpolate between the quaternions.
        /// </param>
        /// <returns>The interpolated value.</returns>
        public static Quaternion Slerp( Quaternion start, Quaternion end, float amount )
        {
            float startAmount, endAmount;
            float dotProduct = (start.X * end.X) + (start.Y * end.Y) + (start.Z * end.Z) + (start.W * end.W);

            bool wasInverted = false;            
            if( dotProduct < 0.0f )
            {
                wasInverted = true;
                dotProduct = -dotProduct;
            }

            if( dotProduct > 0.999999f )
            {
                startAmount = 1f - amount;
                endAmount   = wasInverted ? -amount : amount;
            }
            else
            {
                float acos          = (float)System.Math.Acos( dotProduct );
                float invSinOfAcos  = (float)(1.0 / System.Math.Sin( acos ));

                startAmount = (float)System.Math.Sin( (1f - amount) * acos ) * invSinOfAcos;
                endAmount   = (float)System.Math.Sin( amount * acos ) * invSinOfAcos;

                if( wasInverted )
                    endAmount *= -1;
            }

            Quaternion result;

            result.X = (startAmount * start.X) + (endAmount * end.X);
            result.Y = (startAmount * start.Y) + (endAmount * end.Y);
            result.Z = (startAmount * start.Z) + (endAmount * end.Z);
            result.W = (startAmount * start.W) + (endAmount * end.W);

            return result;
        }

        /// <summary>
        /// Performs a Sperical Linear intERPolation between two <see cref="Quaternion"/>s.
        /// </summary>
        /// <param name="start">
        /// The source Quaternion that represents the start value. This value will not be modified by this method.
        /// </param>
        /// <param name="end">
        /// The source Quaternion that represents the end value. This value will not be modified by this method.
        /// </param>
        /// <param name="amount">
        /// Value indicating how far to interpolate between the quaternions.
        /// </param>
        /// <param name="result">
        /// This value will contain the result of this operation.
        /// </param>
        public static void Slerp( ref Quaternion start, ref Quaternion end, float amount, out Quaternion result )
        {
            float startAmount, endAmount;
            float dotProduct = (start.X * end.X) + (start.Y * end.Y) + (start.Z * end.Z) + (start.W * end.W);

            bool wasInverted = false;
            if( dotProduct < 0.0f )
            {
                wasInverted = true;
                dotProduct = -dotProduct;
            }

            if( dotProduct > 0.999999f )
            {
                startAmount = 1f - amount;
                endAmount   = wasInverted ? -amount : amount;
            }
            else
            {
                float acos          = (float)System.Math.Acos( dotProduct );
                float invSinOfAcos  = (float)(1.0 / System.Math.Sin( acos ));

                startAmount = (float)System.Math.Sin( (1f - amount) * acos ) * invSinOfAcos;
                endAmount   = (float)System.Math.Sin( amount * acos ) * invSinOfAcos;

                if( wasInverted )
                    endAmount *= -1;
            }

            result.X = (startAmount * start.X) + (endAmount * end.X);
            result.Y = (startAmount * start.Y) + (endAmount * end.Y);
            result.Z = (startAmount * start.Z) + (endAmount * end.Z);
            result.W = (startAmount * start.W) + (endAmount * end.W);
        }
        
        /// <summary>
        /// Performs Spherical Linear intERPolation between two <see cref="Quaternion"/>s.
        /// </summary>
        /// <param name="quatA">The first <see cref="Quaternion"/>. </param>
        /// <param name="quatB">The second <see cref="Quaternion"/>.</param>
        /// <param name="useShortestPath">
        /// States whether to use the shortest path from between the <see cref="Quaternion"/>s.
        /// </param>
        /// <param name="amount">
        /// Value indicating how far to interpolate between the quaternions.
        /// </param>
        /// <returns> The interpolated quaternion. </returns>
        public static Quaternion Slerp( Quaternion quatA, Quaternion quatB, bool useShortestPath, float amount )
        {
            float dot;
            Dot( ref quatA, ref quatB, out dot );

            float angle = (float)System.Math.Acos( dot );

            if( System.Math.Abs( angle ) < 1e-03f )
                return quatA;

            float sin    = (float)System.Math.Sin( angle );
            float invSin = 1.0f / sin;
            float coeff0 = (float)System.Math.Sin( (1.0f - amount) * angle ) * invSin;
            float coeff1 = (float)System.Math.Sin( amount * angle ) * invSin;

            if( dot < 0.0f && useShortestPath )
            {
                coeff0 = -coeff0;

                // taking the complement requires renormalisation
                Quaternion t = (coeff0 * quatA) + (coeff1 * quatB);
                t.Normalize();

                return t;
            }
            else
            {
                return (coeff0 * quatA) + (coeff1 * quatB);
            }
        }

        /// <summary>
        /// Performs Spherical Linear intERPolation between two <see cref="Quaternion"/>s.
        /// </summary>
        /// <param name="quatA">The first <see cref="Quaternion"/>. This value will not be modified by this method.</param>
        /// <param name="quatB">The second <see cref="Quaternion"/>. This value will not be modified by this method.</param>
        /// <param name="useShortestPath">
        /// States whether to use the shortest path from between the <see cref="Quaternion"/>s.
        /// </param>
        /// <param name="amount">
        /// Value indicating how far to interpolate between the quaternions.
        /// </param>
        /// <param name="result">
        /// This value will contain the result of this operation.
        /// </param>
        public static void Slerp( ref Quaternion quatA, ref Quaternion quatB, bool useShortestPath, float amount, out Quaternion result )
        {
            float dot;
            Dot( ref quatA, ref quatB, out dot );

            float angle = (float)System.Math.Acos( dot );

            if( System.Math.Abs( angle ) < 1e-03f )
            {
                result = quatA;
                return;
            }

            float sin    = (float)System.Math.Sin( angle );
            float invSin = 1.0f / sin;
            float coeff0 = (float)System.Math.Sin( (1.0f - amount) * angle ) * invSin;
            float coeff1 = (float)System.Math.Sin( amount * angle ) * invSin;

            if( dot < 0.0f && useShortestPath )
            {
                coeff0 = -coeff0;

                // taking the complement requires renormalisation
                Quaternion t = (coeff0 * quatA) + (coeff1 * quatB);
                t.Normalize();

                result = t;
            }
            else
            {
                result = (coeff0 * quatA) + (coeff1 * quatB);
            }
        }

        #endregion

        #region Squad

        /// <summary>
        /// Performs spherical quadratic interpolation.
        /// </summary>
        /// <param name="quatP">The first input Quaternion.</param>
        /// <param name="quatA">The seciond input Quaternion.</param>
        /// <param name="quatB">The third input Quaternion.</param>
        /// <param name="quatQ">The fourth input Quaternion.</param>
        /// <param name="amount">
        /// The time to travel.
        /// </param>
        /// <returns>
        /// The interpolated <see cref="Quaternion"/>.
        /// </returns>
        public static Quaternion Squad( Quaternion quatP, Quaternion quatA, Quaternion quatB, Quaternion quatQ,  float amount )
        {
            return Squad( quatP, quatA, quatB, quatQ, false, amount );
        }

        /// <summary>
        /// Performs spherical quadratic interpolation.
        /// </summary>
        /// <param name="quatP">The first input Quaternion.</param>
        /// <param name="quatA">The seciond input Quaternion.</param>
        /// <param name="quatB">The third input Quaternion.</param>
        /// <param name="quatQ">The fourth input Quaternion.</param>
        /// <param name="useShortestPath">
        /// States whether to use the shortest path from between the <see cref="Quaternion"/>s.
        /// </param>
        /// <param name="amount">
        /// The time to travel.
        /// </param>
        /// <returns>
        /// The interpolated <see cref="Quaternion"/>.
        /// </returns>
        public static Quaternion Squad( 
            Quaternion quatP, 
            Quaternion quatA, 
            Quaternion quatB, 
            Quaternion quatQ,
            bool useShortestPath, 
            float amount )
        {
            // use spherical linear interpolation
            Quaternion slerpP = Slerp( quatP, quatQ, useShortestPath, amount );
            Quaternion slerpQ = Slerp( quatA, quatB, amount );

            // run another Slerp on the results of the first 2, and return the results
            float slerpAmount = 2.0f * amount * (1.0f - amount);
            return Slerp( slerpP, slerpQ, slerpAmount );
        }

        #endregion

        #endregion

        #region > Creation Helpers <

        #region FromAxisAngle

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> given an
        /// <paramref name="axis"/> and an <paramref name="angle"/>.
        /// </summary>
        /// <param name="axis">
        /// The axis to rotate about.
        /// </param>
        /// <param name="angle">
        /// The angle to rotate around the axis in radians.
        /// </param>
        /// <returns>
        /// The converted Quaternion.
        /// </returns>
        public static Quaternion FromAxisAngle( Vector3 axis, float angle )
        {
            double halfAngle = angle * 0.5;
            float  sin       = (float)System.Math.Sin( halfAngle );

            Quaternion quaternion;

            quaternion.X = axis.X * sin;
            quaternion.Y = axis.Y * sin;
            quaternion.Z = axis.Z * sin;
            quaternion.W = (float)System.Math.Cos( halfAngle );

            return quaternion;
        }

        #endregion

        #region FromRotationMatrix

        /// <summary>
        /// Converts the given rotation <see cref="Matrix3"/> into a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix to convert.
        /// </param>
        /// <returns>
        /// The converted Quaternion.
        /// </returns>
        public static Quaternion FromRotationMatrix( Matrix3 matrix )
        {
            float trace = matrix.M11 + matrix.M22 + matrix.M33;

            if( trace > 0.0f )
            {
                float root = (float)System.Math.Sqrt( (double)(trace + 1f) );

                Quaternion quaternion;
                quaternion.W = root * 0.5f;

                root         = 0.5f / root;
                quaternion.X = (matrix.M23 - matrix.M32) * root;
                quaternion.Y = (matrix.M31 - matrix.M13) * root;
                quaternion.Z = (matrix.M12 - matrix.M21) * root;

                return quaternion;
            }

            if( (matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33) )
            {
                float root = (float)System.Math.Sqrt( 1.0f + matrix.M11 - matrix.M22 - matrix.M33 );

                Quaternion quaternion;
                quaternion.X = root * 0.5f;

                root         = 0.5f / root;
                quaternion.Y = (matrix.M12 + matrix.M21) * root;
                quaternion.Z = (matrix.M13 + matrix.M31) * root;
                quaternion.W = (matrix.M23 - matrix.M32) * root;

                return quaternion;
            }

            if( matrix.M22 > matrix.M33 )
            {
                float root = (float)System.Math.Sqrt( 1.0f + matrix.M22 - matrix.M11 - matrix.M33 );
                float root2 = 0.5f / root;

                Quaternion quaternion;

                quaternion.X = (matrix.M21 + matrix.M12) * root2;
                quaternion.Y = 0.5f * root;
                quaternion.Z = (matrix.M32 + matrix.M23) * root2;
                quaternion.W = (matrix.M31 - matrix.M13) * root2;

                return quaternion;
            }

            // else
            {
                float root  = (float)System.Math.Sqrt( 1.0f + matrix.M33 - matrix.M11 - matrix.M22 );
                float root2 = 0.5f / root;

                Quaternion quaternion;

                quaternion.X = (matrix.M31 + matrix.M13) * root2;
                quaternion.Y = (matrix.M32 + matrix.M23) * root2;
                quaternion.Z = 0.5f * root;
                quaternion.W = (matrix.M12 - matrix.M21) * root2;

                return quaternion;
            }
        }

        /// <summary>
        /// Converts the given rotation <see cref="Matrix4"/> into a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix to convert.
        /// </param>
        /// <returns>
        /// The converted Quaternion.
        /// </returns>
        public static Quaternion FromRotationMatrix( Matrix4 matrix )
        {
            Quaternion result;

            float trace = matrix.M11 + matrix.M22 + matrix.M33;
            
            if( trace > 0f )
            {
                float root = (float)System.Math.Sqrt( trace + 1.0f );
                
                result.W = root * 0.5f;
                root = 0.5f / root;
                result.X = (matrix.M23 - matrix.M32) * root;
                result.Y = (matrix.M31 - matrix.M13) * root;
                result.Z = (matrix.M12 - matrix.M21) * root;
            }
            else if( (matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33) )
            {
                float root        = (float)System.Math.Sqrt( 1.0f + matrix.M11 - matrix.M22 - matrix.M33 );
                float halfInvRoot = 0.5f / root;
                
                result.X = 0.5f * root;
                result.Y = (matrix.M12 + matrix.M21) * halfInvRoot;
                result.Z = (matrix.M13 + matrix.M31) * halfInvRoot;
                result.W = (matrix.M23 - matrix.M32) * halfInvRoot;
            }
            else if( matrix.M22 > matrix.M33 )
            {
                float root        = (float)System.Math.Sqrt( 1.0f + matrix.M22 - matrix.M11 - matrix.M33 );
                float halfInvRoot = 0.5f / root;
               
                result.X = (matrix.M21 + matrix.M12) * halfInvRoot;
                result.Y = 0.5f * root;
                result.Z = (matrix.M32 + matrix.M23) * halfInvRoot;
                result.W = (matrix.M31 - matrix.M13) * halfInvRoot;
            }
            else
            {
                float root = (float)System.Math.Sqrt( 1.0f + matrix.M33 - matrix.M11 - matrix.M22 );
                float halfInvRoot = 0.5f / root;
                
                result.X = (matrix.M31 + matrix.M13) * halfInvRoot;
                result.Y = (matrix.M32 + matrix.M23) * halfInvRoot;
                result.Z = 0.5f * root;
                result.W = (matrix.M12 - matrix.M21) * halfInvRoot;
            }

            return result;
        }

        /// <summary>
        /// Converts the given rotation <see cref="Matrix4"/> into a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix to convert.
        /// </param>
        /// <param name="result">Will contain the converted Quaternion.</param>
        public static void FromRotationMatrix( ref Matrix4 matrix, out Quaternion result )
        {
            float trace = matrix.M11 + matrix.M22 + matrix.M33;
            if( trace > 0f )
            {
                float root = (float)System.Math.Sqrt( trace + 1f );

                result.W = root * 0.5f;
                root = 0.5f / root;
                result.X = (matrix.M23 - matrix.M32) * root;
                result.Y = (matrix.M31 - matrix.M13) * root;
                result.Z = (matrix.M12 - matrix.M21) * root;
            }
            else if( (matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33) )
            {
                float root        = (float)System.Math.Sqrt( 1.0f + matrix.M11 - matrix.M22 - matrix.M33 );
                float halfInvRoot = 0.5f / root;
                
                result.X = 0.5f * root;
                result.Y = (matrix.M12 + matrix.M21) * halfInvRoot;
                result.Z = (matrix.M13 + matrix.M31) * halfInvRoot;
                result.W = (matrix.M23 - matrix.M32) * halfInvRoot;
            }
            else if( matrix.M22 > matrix.M33 )
            {
                float root        = (float)System.Math.Sqrt( 1.0f + matrix.M22 - matrix.M11 - matrix.M33 );
                float halfInvRoot = 0.5f / root;
                
                result.X = (matrix.M21 + matrix.M12) * halfInvRoot;
                result.Y = 0.5f * root;
                result.Z = (matrix.M32 + matrix.M23) * halfInvRoot;
                result.W = (matrix.M31 - matrix.M13) * halfInvRoot;
            }
            else
            {
                float root        = (float)System.Math.Sqrt( 1.0f + matrix.M33 - matrix.M11 - matrix.M22 );
                float halfInvRoot = 0.5f / root;
                
                result.X = (matrix.M31 + matrix.M13) * halfInvRoot;
                result.Y = (matrix.M32 + matrix.M23) * halfInvRoot;
                result.Z = 0.5f * root;
                result.W = (matrix.M12 - matrix.M21) * halfInvRoot;
            }
        }

        #endregion

        #region FromYawPitchRoll

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from specified
        /// <paramref name="yaw"/>, <paramref name="pitch"/>, and <paramref name="roll"/> angles. 
        /// </summary>
        /// <param name="yaw">The yaw angle, in radians, around the y-axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the x-axis.</param>
        /// <param name="roll">The roll angle, in radians, around the z-axis.</param>
        /// <returns>
        /// A new Quaternion expressing the specified yaw, pitch, and roll angles.
        /// </returns>
        public static Quaternion FromYawPitchRoll( float yaw, float pitch, float roll )
        {
            double halfRoll = roll * 0.5;
            float sinRoll  = (float)System.Math.Sin( halfRoll );
            float cosRoll  = (float)System.Math.Cos( halfRoll );

            double halfPitch = pitch * 0.5;
            float sinPitch = (float)System.Math.Sin( halfPitch );
            float cosPitch = (float)System.Math.Cos( halfPitch );

            double halfYaw = yaw * 0.5;
            float sinYaw = (float)System.Math.Sin( halfYaw );
            float cosYaw = (float)System.Math.Cos( halfYaw );

            Quaternion quaternion;

            quaternion.X = ((cosYaw * sinPitch) * cosRoll) + ((sinYaw * cosPitch) * sinRoll);
            quaternion.Y = ((sinYaw * cosPitch) * cosRoll) - ((cosYaw * sinPitch) * sinRoll);
            quaternion.Z = ((cosYaw * cosPitch) * sinRoll) - ((sinYaw * sinPitch) * cosRoll);
            quaternion.W = ((cosYaw * cosPitch) * cosRoll) + ((sinYaw * sinPitch) * sinRoll);

            return quaternion;
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns Whether the given <see cref="Quaternion"/> has the
        /// same indices set as this Quaternion.
        /// </summary>
        /// <param name="other">The Quaternion to test against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the components of the Quaternions are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( Quaternion other )
        {
            return this.X.IsApproximate( other.X ) &&
                   this.Y.IsApproximate( other.Y ) &&
                   this.Z.IsApproximate( other.Z ) &&
                   this.W.IsApproximate( other.W );
        }

        /// <summary>
        /// Returns whether the given <see cref="Object"/> is equal to this Quaternion.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is Quaternion )
                return Equals( (Quaternion)obj );

            return false;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overriden to return a human-readable text representation of the Quaternion.
        /// </summary>
        /// <returns>A human-readable text representation of the Quaternion.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Quaternion.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Quaternion.</returns>
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

        /// <summary>
        /// Converts the <see cref="Quaternion"/> into a <see cref="Vector4"/>.
        /// </summary>
        /// <returns>The converted point.</returns>
        public Vector4 ToVector4()
        {
            return new Vector4( this.X, this.Y, this.Z, this.W );
        }

        /// <summary>
        /// Overriden to return the hashcode of the <see cref="Quaternion"/>.
        /// </summary>
        /// <returns>The simple Xor-ed hashcode.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.X );
            hashBuilder.AppendStruct( this.Y );
            hashBuilder.AppendStruct( this.Z );
            hashBuilder.AppendStruct( this.W );

            return hashBuilder.GetHashCode();
        }

        #region IEnumerable<float> Members

        /// <summary>
        /// Returns an enumerator that iterates over the components of this <see cref="Quaternion"/>.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public System.Collections.Generic.IEnumerator<float> GetEnumerator()
        {
            yield return X;
            yield return Y;
            yield return Z;
            yield return W;
        }

        /// <summary>
        /// Returns an enumerator that iterates over the components of this <see cref="Quaternion"/>.
        /// </summary>
        /// <returns>An enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]
        
        #region +

        /// <summary>
        /// Returns the result of adding the <paramref name="right"/> Quaternion to the <paramref name="left"/> Quaternion.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion operator +( Quaternion left, Quaternion right )
        {
            Quaternion result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;

            return result;
        }

        /// <summary>
        /// This method returns the specified Quaternion.
        /// </summary>
        /// <remarks>
        /// Is equal to "+Quaternion".
        /// </remarks>
        /// <param name="quaternion">The input Quaternion.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion operator +( Quaternion quaternion )
        {
            return quaternion;
        }

        #endregion

        #region -

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Quaternion from the <paramref name="left"/> Quaternion.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion operator -( Quaternion left, Quaternion right )
        {
            Quaternion result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;

            return result;
        }

        /// <summary>
        /// Returns the result of negating the elements of the given <paramref name="quaternion"/>.
        /// </summary>
        /// <param name="quaternion">
        /// The Quaternion to negate.
        /// </param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion operator -( Quaternion quaternion )
        {
            Quaternion result;

            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = -quaternion.W;

            return result;
        }

        #endregion

        #region *

        /// <summary>
        /// Returns the result of multiplying the given <paramref name="quaternion"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="quaternion">The Quaternion on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion operator *( Quaternion quaternion, float scalar )
        {
            Quaternion result;

            result.X = quaternion.X * scalar;
            result.Y = quaternion.Y * scalar;
            result.Z = quaternion.Z * scalar;
            result.W = quaternion.W * scalar;

            return result;
        }

        /// <summary>
        /// Returns the result of multiplying the given <paramref name="quaternion"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="scalar">The scalar on the left side of the equation.</param>
        /// <param name="quaternion">The Quaternion on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion operator *( float scalar, Quaternion quaternion )
        {
            Quaternion result;

            result.X = scalar * quaternion.X;
            result.Y = scalar * quaternion.Y;
            result.Z = scalar * quaternion.Z;
            result.W = scalar * quaternion.W;

            return result;
        }

        /// <summary>
        /// Returns the result of multiplying the left Quaternion by the right Quaternion.
        /// </summary>
        /// <param name="left">The Quaternion on the left side of the equation.</param>
        /// <param name="right">The Quaternion on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion operator *( Quaternion left, Quaternion right )
        {
            float factorX = (left.Y * right.Z) - (left.Z * right.Y);
            float factorY = (left.Z * right.X) - (left.X * right.Z);
            float factorZ = (left.X * right.Y) - (left.Y * right.X);
            float factorW = ((left.X * right.X) + (left.Y * right.Y)) + (left.Z * right.Z);

            Quaternion result;

            result.X = ((left.X * right.W) + (right.X * left.W)) + factorX;
            result.Y = ((left.Y * right.W) + (right.Y * left.W)) + factorY;
            result.Z = ((left.Z * right.W) + (right.Z * left.W)) + factorZ;
            result.W = (left.W * right.W) - factorW;

            return result;
        }

        #endregion

        #region /

        /// <summary>
        /// Returns the result of dividing the left Quaternion through the right Quaternion component-by-component.
        /// </summary>
        /// <param name="left">The Quaternion on the left side of the equation.</param>
        /// <param name="right">The Quaternion on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Quaternion operator /( Quaternion left, Quaternion right )
        {
            // invert right inline:
            float factor    = 1f / right.SquaredLength;
            float invRightX = -right.X * factor;
            float invRightY = -right.Y * factor;
            float invRightZ = -right.Z * factor;
            float invRightW = right.W * factor;

            // do Multiply( left, invRight ):
            float factorX = (left.Y * invRightZ) - (left.Z * invRightY);
            float factorY = (left.Z * invRightX) - (left.X * invRightZ);
            float factorZ = (left.X * invRightY) - (left.Y * invRightX);
            float factorW = ((left.X * invRightX) + (left.Y * invRightY)) + (left.Z * invRightZ);

            Quaternion result;

            result.X = ((left.X * invRightW) + (invRightX * left.W)) + factorX;
            result.Y = ((left.Y * invRightW) + (invRightY * left.W)) + factorY;
            result.Z = ((left.Z * invRightW) + (invRightZ * left.W)) + factorZ;
            result.W = (left.W * invRightW) - factorW;

            return result;
        }

        #endregion
        
        #region Logic

        /// <summary>
        /// Returns whether the specified <see cref="Quaternion"/>s are equal.
        /// </summary>
        /// <param name="left">The Quaternion of the left side of the equation.</param>
        /// <param name="right">The Quaternion of the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the corresponding elements of the specified Quaternions are approximately equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==( Quaternion left, Quaternion right )
        {
            return left.X.IsApproximate( right.X ) && left.Y.IsApproximate( right.Y ) &&
                   left.Z.IsApproximate( right.Z ) && left.W.IsApproximate( right.W );
        }

        /// <summary>
        /// Returns whether the specified <see cref="Quaternion"/>s are not equal.
        /// </summary>
        /// <param name="left">The Quaternion of the left side of the equation.</param>
        /// <param name="right">The Quaternion of the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if any of the corresponding elements of the specified Quaternions are approximately not equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=( Quaternion left, Quaternion right )
        {
            return !left.X.IsApproximate( right.X ) || !left.Y.IsApproximate( right.Y ) ||
                   !left.Z.IsApproximate( right.Z ) || !left.W.IsApproximate( right.W );
        }

        #endregion

        #endregion
    }
}
