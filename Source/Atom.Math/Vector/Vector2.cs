// <copyright file="Vector2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Vector2 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a two dimensional single-precision floating-vector Vector.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.Vector2Converter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Vector2 : IEquatable<Vector2>, ICultureSensitiveToStringProvider
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

        #endregion

        #region [ Constants ]
        
        /// <summary>
        /// Gets a <see cref="Vector2"/> with all its components set to zero.
        /// </summary>
        /// <value>The vector (0, 0).</value>
        public static Vector2 Zero 
        { 
            get 
            {
                return new Vector2(); 
            }
        }

        /// <summary>
        /// Gets a <see cref="Vector2"/> with all its components set to one.
        /// </summary>
        /// <value>The vector (1, 1).</value>
        public static Vector2 One 
        { 
            get 
            { 
                return new Vector2( 1.0f, 1.0f ); 
            }
        }

        /// <summary>
        /// Gets the unit <see cref="Vector2"/> for the x-axis.
        /// </summary>
        /// <value>The vector (1, 0).</value>
        public static Vector2 UnitX 
        { 
            get 
            { 
                return new Vector2( 1.0f, 0.0f );
            }
        }

        /// <summary>
        /// Gets the unit <see cref="Vector2"/> for the y-axis.
        /// </summary>
        /// <value>The vector (0, 1).</value>
        public static Vector2 UnitY
        { 
            get
            {
                return new Vector2( 0.0f, 1.0f ); 
            }
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the length of the Vector.
        /// </summary>
        /// <value>The length (also called magnitude) of the vector.</value>
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
                return (float)System.Math.Sqrt( (this.X * this.X) + (this.Y * this.Y) );
            }

            set
            {
                if( value < 0.0f )
                    throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "value" );
               
                if( float.IsInfinity( value ) )
                {
                    X = value;
                    Y = value;
                }
                else
                {
                    float length = this.Length;
                    if( length == 0.0f )
                        throw new InvalidOperationException( MathErrorStrings.OperationInvalidOnZeroVector );

                    float ratio = value / length;
                    X *= ratio;
                    Y *= ratio;
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
                return (this.X * this.X) + (this.Y * this.Y);
            }

            set
            {
                if( value < 0.0f )
                    throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "value" );

                if( float.IsInfinity( value ) )
                {
                    X = value;
                    Y = value;
                }
                else
                {
                    float squaredLength = this.SquaredLength;
                    if( squaredLength == 0.0f )
                        throw new InvalidOperationException( MathErrorStrings.OperationInvalidOnZeroVector );

                    float ratio = value / squaredLength;
                    X *= ratio;
                    Y *= ratio;
                }
            }
        }

        /// <summary>
        /// Gets the direction this Vector2 is pointing at.
        /// </summary>
        /// <value>
        /// This vector, normalized.
        /// </value>
        public Vector2 Direction
        {
            get
            {
                float length = this.Length;
                if( length == 0 )
                    return Vector2.Zero;

                Vector2 direction;
                direction.X = this.X / length;
                direction.Y = this.Y / length;

                return direction;
            }
        }

        /// <summary>
        /// Gets the Vector2 that is perpendicular to this Vector2.
        /// </summary>
        public Vector2 Perpendicular
        {
            get
            {
                return new Vector2( this.Y, -this.X );
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
        public float this[Axis2 axis]
        {
            get
            {
                Contract.Requires( axis != Axis2.None );

                switch( axis )
                {
                    case Axis2.X:
                        return this.X;
                        
                    default:
                    case Axis2.Y:
                        return this.Y;
                }
            }

            set
            {
                Contract.Requires( axis != Axis2.None );

                switch( axis )
                {
                    case Axis2.X:
                        this.X = value;
                        break;

                    case Axis2.Y:
                        this.Y = value;
                        break;
                }
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> struct.
        /// </summary>
        /// <param name="x">The X-coordinate of the new Vector.</param>
        /// <param name="y">The Y-coordinate of the new Vector.</param>
        public Vector2( float x, float y )
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Creates a new instance of the Vector2 structure that has set the specified axis
        /// to the specified value.
        /// </summary>
        /// <example>
        /// Vector2 unitX = Vector2.FromAxis( 1.0f, Axis2.X ); // (1 0)
        /// Vector2 unitY = Vector2.FromAxis( 1.0f, Axis2.Y ); // (0 1)
        /// Vector2 zero = Vector2.FromAxis( 1.0f, Axis2.None ); // (0 0)
        /// </example>
        /// <param name="value">
        /// The value of the axis.
        /// </param>
        /// <param name="axis">
        /// The axis to initialize.
        /// </param>
        /// <returns>
        /// The newly created Vector2.
        /// </returns>
        public static Vector2 FromAxis( float value, Axis2 axis )
        {
            switch( axis )
            {
                case Axis2.X:
                    return new Vector2( value, 0.0f );

                case Axis2.Y:
                    return new Vector2( 0.0f, value );

                default:
                    return Vector2.Zero;
            }
        }

        /// <summary>
        /// Creates a new Vector2 that represents an arrow into the specified direction.
        /// </summary>
        /// <param name="angle">
        /// The angle of the arrow.
        /// </param>
        /// <returns>
        /// The newly created Vector2.
        /// </returns>
        public static Vector2 FromAngle( float angle )
        {
            Vector2 vector;
            vector.X = (float)Math.Cos( angle );
            vector.Y = (float)Math.Sin( angle );

            return vector;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns the given vector with each element floored.
        /// </summary>
        /// <param name="vector">
        /// The input vector.
        /// </param>
        /// <returns>
        /// The floored output vector.
        /// </returns>
        public static Vector2 Floor( Vector2 vector )
        {
            return new Vector2( (float)System.Math.Floor( vector.X ), (float)System.Math.Floor( vector.Y ) );
        }

        #region Dot

        /// <summary>
        /// Returns the dot product of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The vector on the left side of the equation.</param>
        /// <param name="vectorB">The vector on the right side of the equation.</param>
        /// <returns>
        /// The dot product.
        /// </returns>
        public static float Dot( Vector2 vectorA, Vector2 vectorB )
        {
            return (vectorA.X * vectorB.X) + (vectorA.Y * vectorB.Y);
        }

        /// <summary>
        /// Stores the dot product of the given Vectors in the given <paramref name="result"/> value.
        /// </summary>
        /// <param name="vectorA">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="vectorB">The vector on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">
        /// This value will contain the result of the operation.
        /// </param>
        public static void Dot( ref Vector2 vectorA, ref Vector2 vectorB, out float result )
        {
            result = (vectorA.X * vectorB.X) + (vectorA.Y * vectorB.Y);
        }

        #endregion

        #region DotPerp

        /// <summary>
        /// Returns the perpendicular dot product of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The vector on the left side of the equation.</param>
        /// <param name="vectorB">The vector on the right side of the equation.</param>
        /// <returns>
        /// The dot product.
        /// </returns>
        public static float DotPerp( Vector2 vectorA, Vector2 vectorB )
        {
            return (vectorA.X * vectorB.Y) + (vectorA.Y * vectorB.X);
        }

        /// <summary>
        /// Stores the perpendicular dot product of the given Vectors in the given <paramref name="result"/> value.
        /// </summary>
        /// <param name="vectorA">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="vectorB">The vector on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">
        /// This value will contain the result of the operation.
        /// </param>
        public static void DotPerp( ref Vector2 vectorA, ref Vector2 vectorB, out float result )
        {
            result = (vectorA.X * vectorB.Y) + (vectorA.Y * vectorB.X);
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
        public static Matrix2 Dyadic( Vector2 left, Vector2 right )
        {
            return new Matrix2(
                left.X * right.X, left.X * right.Y,
                left.Y * right.X, left.Y * right.Y
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
        public static void Dyadic( ref Vector2 left, ref Vector2 right, out Matrix2 result )
        {
            result = new Matrix2(
                left.X * right.X, left.X * right.Y,
                left.Y * right.X, left.Y * right.Y
            );
        }

        #endregion
        
        #region Cross

        /// <summary>
        /// Returns the cross product between the given Vectors.
        /// </summary>
        /// <returns>
        /// The cross product in 2d is not a real cross-product
        /// but more a perpendicular dot product.
        /// </returns>
        /// <param name="vectorA">The vector on the left side of the equation.</param>
        /// <param name="vectorB">The vector on the right side of the equation.</param>
        public static float Cross( Vector2 vectorA, Vector2 vectorB )
        {
            return (vectorA.X * vectorB.Y) - (vectorA.Y * vectorB.X);
        }

        /// <summary>
        /// Returns the cross product between the given Vectors.
        /// </summary>
        /// <param name="vectorA">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="vectorB">The vector on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">
        /// Will contain the result of the operation.
        /// The cross product in 2d is not a real cross-product
        /// but more a perpendicular dot product.
        /// </param>
        public static void Cross( ref Vector2 vectorA, ref Vector2 vectorB, out float result )
        {
            result = (vectorA.X * vectorB.Y) - (vectorA.Y * vectorB.X);
        }

        /// <summary>
        /// Returns the cross product between the given <see cref="Vector2"/> and the given scalar.
        /// </summary>
        /// <param name="vectorA">The vector on the left side of the equation.</param>
        /// <param name="scalarB">The scalar on the right side of the equation.</param>
        /// <returns>The result of this operation.</returns>
        public static Vector2 Cross( Vector2 vectorA, float scalarB )
        {
            Vector2 result;

            result.X = scalarB * vectorA.Y;
            result.Y = -scalarB * vectorA.X;

            return result;
        }

        /// <summary>
        /// Stores the cross product between the given <see cref="Vector2"/> and the given scalar
        /// in the given <paramref name="result"/> value.
        /// </summary>
        /// <param name="vectorA">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalarB">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result of this operation.</param>
        public static void Cross( ref Vector2 vectorA, float scalarB, out Vector2 result )
        {
            result.X = scalarB * vectorA.Y;
            result.Y = -scalarB * vectorA.X;
        }

        /// <summary>
        /// Returns the cross product between the given scalar and the given <see cref="Vector2"/>.
        /// </summary>
        /// <param name="scalarA">The scalar on the left side of the equation.</param>
        /// <param name="vectorB">The vector on the right side of the equation.</param>
        /// <returns>The result of this operation.</returns>
        public static Vector2 Cross( float scalarA, Vector2 vectorB )
        {
            Vector2 result;

            result.X = -scalarA * vectorB.Y;
            result.Y = scalarA * vectorB.X;

            return result;
        }

        /// <summary>
        /// Stores the cross product between the given scalar and the given <see cref="Vector2"/>
        /// in the given <paramref name="result"/> value.
        /// </summary>
        /// <param name="scalarA">The scalar on the left side of the equation.</param>
        /// <param name="vectorB">The vector on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Cross( float scalarA, ref Vector2 vectorB, out Vector2 result )
        {
            result.X = -scalarA * vectorB.Y;
            result.Y = scalarA * vectorB.X;
        }

        #endregion

        #region Normalize

        /// <summary>
        /// Normalizes the Vector, setting its length to one.
        /// </summary>
        public void Normalize()
        {
            float squaredLength = (this.X * this.X) + (this.Y * this.Y);
            float invLength     = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            this.X *= invLength;
            this.Y *= invLength;
        }

        /// <summary>
        /// Returns the result of normalizing the given Vector.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static Vector2 Normalize( Vector2 vector )
        {
            float squaredLength = (vector.X * vector.X) + (vector.Y * vector.Y);
            float invLength     = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            Vector2 result;

            result.X = vector.X * invLength;
            result.Y = vector.Y * invLength;

            return result;
        }

        /// <summary>
        /// Stores the result of normalizing the given Vector in the given <paramref name="result"/> Vector.
        /// </summary>
        /// <param name="vector">The vector to normalize. This value will not be modified by this method.</param>
        /// <param name="result">This value will store the result of the operation.</param>
        public static void Normalize( ref Vector2 vector, out Vector2 result )
        {
            float squaredLength = (vector.X * vector.X) + (vector.Y * vector.Y);
            float invLength     = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            result.X = vector.X * invLength;
            result.Y = vector.Y * invLength;
        }

        #endregion
        
        #region Rotate
        
        /// <summary> 
        /// Rotates the Vector around the Orgin by the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle"> 
        /// The angle to rotate in radians.
        /// </param>
        public void Rotate( float angle )
        {
            float tX  = X;
            float cos = (float)System.Math.Cos( (double)angle );
            float sin = (float)System.Math.Sin( (double)angle );

            X = (X * cos) - (Y * sin);
            Y = (tX * sin) + (Y * cos);
        }

        /// <summary> 
        /// Rotates the Vector around the Orgin by the angle
        /// given with the specified <paramref name="cos"/>/<paramref name="sin"/>.
        /// </summary>
        /// <param name="cos"> 
        /// The cosinus of the angle to rotate.
        /// </param>
        /// <param name="sin"> 
        /// The sinus of the angle to rotate.
        /// </param>
        public void Rotate( float cos, float sin )
        {
            float tX  = X;
            X = (X * cos) - (Y * sin);
            Y = (tX * sin) + (Y * cos);
        }
        
        /// <summary> 
        /// Rotates the Vector around the specified <paramref name="center"/> by the specified <paramref name="angle"/>.
        /// </summary>
        /// <param name="center">
        /// The center to rotate around.
        /// </param>
        /// <param name="angle"> 
        /// The angle to rotate in radians.
        /// </param>
        public void Rotate( Vector2 center, float angle )
        {
            Vector2 delta;

            delta.X = this.X - center.X;
            delta.Y = this.Y - center.Y;

            delta.Rotate( angle );

            this.X = center.X + delta.X;
            this.Y = center.Y + delta.Y;
        }

        /// <summary> 
        /// Rotates the Vector around the specified <paramref name="center"/> by the angle 
        /// given with the specified <paramref name="cos"/>/<paramref name="sin"/>.
        /// </summary>
        /// <param name="center">
        /// The center to rotate around.
        /// </param>
        /// <param name="cos"> 
        /// The cosinus of the angle to rotate.
        /// </param>
        /// <param name="sin"> 
        /// The sinus of the angle to rotate.
        /// </param>
        public void Rotate( Vector2 center, float cos, float sin )
        {
            Vector2 delta;

            delta.X = this.X - center.X;
            delta.Y = this.Y - center.Y;

            delta.Rotate( cos, sin );

            this.X = center.X + delta.X;
            this.Y = center.Y + delta.Y;
        }

        #endregion

        #region Reflect

        /// <summary>
        /// Returns the reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">The normal.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Reflect( Vector2 vector, Vector2 normal )
        {
            float dotProduct = (vector.X * normal.X) + (vector.Y * normal.Y);

            Vector2 result;

            result.X = vector.X - ((2f * dotProduct) * normal.X);
            result.Y = vector.Y - ((2f * dotProduct) * normal.Y);

            return result;
        }

        /// <summary>
        /// Stores the reflect vector of the given vector and normal in the given <paramref name="result"/> vector.
        /// </summary>
        /// <param name="vector">The source vector. This value will not be modified by this method.</param>
        /// <param name="normal">The normal. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Reflect( ref Vector2 vector, ref Vector2 normal, out Vector2 result )
        {
            float dotProduct = (vector.X * normal.X) + (vector.Y * normal.Y);
    
            result.X = vector.X - ((2f * dotProduct) * normal.X);
            result.Y = vector.Y - ((2f * dotProduct) * normal.Y);
        }

        #endregion
        
        #region Angle

        /// <summary> 
        /// Returns the angle between the specified Vectors. 
        /// </summary>
        /// <param name="from">
        /// The start vector. Must be normalized.
        /// </param>
        /// <param name="to">
        /// The end vector. Must be normalized.
        /// </param>
        /// <returns> The angle in radians. </returns>
        public static float Angle( Vector2 from, Vector2 to )
        {
            float cross;
            Cross( ref from, ref to, out cross );
            
            float dot;
            Dot( ref from, ref to, out dot );

            return (float)System.Math.Atan2( (double)cross, (double)dot );
        }

        /// <summary> 
        /// Stores the angle between the specified Vectors in the given result value. 
        /// </summary>
        /// <param name="from">
        /// The start vector. Must be normalized.
        /// This value will not be modified by this method.
        /// </param>
        /// <param name="to">
        /// The end vector. Must be normalized.
        /// This value will not be modified by this method.
        /// </param>
        /// <param name="result">
        /// Will contain the angle in radians.
        /// </param>
        public static void Angle( ref Vector2 from, ref Vector2 to, out float result )
        {
            float cross;
            Cross( ref from, ref to, out cross );

            float dot;
            Dot( ref from, ref to, out dot );

            result = (float)System.Math.Atan2( (double)cross, (double)dot );
        }

        #endregion  

        #region Barycentric

        /// <summary>
        /// Returns a Vector2 containing the 2D Cartesian coordinates of a vector
        /// specified in barycentric (areal) coordinates relative to a 2D triangle.
        /// </summary>
        /// <param name="value1">
        /// A Vector2 containing the 2D Cartesian coordinates of vertex 1 of the triangle.
        /// </param>
        /// <param name="value2">
        /// A Vector2 containing the 2D Cartesian coordinates of vertex 2 of the triangle.
        /// </param>
        /// <param name="value3">
        /// A Vector2 containing the 2D Cartesian coordinates of vertex 3 of the triangle.
        /// </param>
        /// <param name="amount1">
        /// Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).
        /// </param>
        /// <param name="amount2">
        /// Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).
        /// </param>
        /// <returns>
        /// A new Vector2 containing the 2D Cartesian coordinates of the specified vector.
        /// </returns>
        public static Vector2 Barycentric( Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2 )
        {
            Vector2 result;

            result.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
            result.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));

            return result;
        }

        /// <summary>
        /// Stores the 2D Cartesian coordinates of a vector
        /// specified in barycentric (areal) coordinates relative to a 2D triangle
        /// within the given result Vector.
        /// </summary>
        /// <param name="value1">
        /// A Vector2 containing the 2D Cartesian coordinates of vertex 1 of the triangle.
        /// This value will not be modified by this method.
        /// </param>
        /// <param name="value2">
        /// A Vector2 containing the 2D Cartesian coordinates of vertex 2 of the triangle.
        /// This value will not be modified by this method.
        /// </param>
        /// <param name="value3">
        /// A Vector2 containing the 2D Cartesian coordinates of vertex 3 of the triangle.
        /// This value will not be modified by this method.
        /// </param>
        /// <param name="amount1">
        /// Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).
        /// This value will not be modified by this method.
        /// </param>
        /// <param name="amount2">
        /// Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).
        /// This value will not be modified by this method.
        /// </param>
        /// <param name="result">
        /// Will containin the 2D Cartesian coordinates of the specified vector.
        /// </param>
        public static void Barycentric( ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1, float amount2, out Vector2 result )
        {
            result.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
            result.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));
        }

        #endregion

        #region Project

        /// <summary>
        /// Projects the specified Vector onto the other specified Vector.
        /// </summary>
        /// <param name="vector">The vector to project.</param>
        /// <param name="onTo">The vector to project onto.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Project( Vector2 vector, Vector2 onTo )
        {
            float multiplier    = 0.0f;
            float dot           = (onTo.X * vector.X) + (onTo.Y * vector.Y);
            float squaredLength = (onTo.X * onTo.X) + (onTo.Y * onTo.Y);

            if( squaredLength != 0.0f )
            {
                multiplier = dot / squaredLength;
            }

            Vector2 result;

            result.X = onTo.X * multiplier;
            result.Y = onTo.Y * multiplier;

            return result;
        }  
        
        /// <summary>
        /// Projects the specified Vector onto the other specified Vector.
        /// </summary>
        /// <param name="vector">
        /// The vector to project. This value will not be modified by this method.
        /// </param>
        /// <param name="onTo">
        /// The vector to project onto. This value will not be modified by this method.
        /// </param>
        /// <param name="result">
        /// Will contain the result of the operation.
        /// </param>
        public static void Project( ref Vector2 vector, ref Vector2 onTo, out Vector2 result )
        {
            float factor        = 0.0f;
            float dot           = (onTo.X * vector.X) + (onTo.Y * vector.Y);
            float squaredLength = (onTo.X * onTo.X) + (onTo.Y * onTo.Y);

            if( squaredLength != 0.0f )
            {
                factor = dot / squaredLength;
            }

            result.X = onTo.X * factor;
            result.Y = onTo.Y * factor;
        }

        #endregion

        #region > Transformation <

        #region Vector2 by Matrix2

        /// <summary>
        /// Transforms a Vector2 by the given Matrix2.
        /// </summary>
        /// <param name="vector">The source Vector2.</param>
        /// <param name="matrix">The transformation Matrix2.</param>
        /// <returns>The Vector2 resulting from the transformation.</returns>
        public static Vector2 Transform( Vector2 vector, Matrix2 matrix )
        {
            Vector2 result;

            result.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21);
            result.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22);

            return result;
        }

        /// <summary>
        /// Transforms a Vector2 by the given result.
        /// </summary>
        /// <param name="vector">The source Vector2.</param>
        /// <param name="matrix">The transformation result.</param>
        /// <param name="result">The Vector2 resulting from the transformation.</param>
        public static void Transform( ref Vector2 vector, ref Matrix2 matrix, out Vector2 result )
        {
            result.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21);
            result.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22);
        }

        #endregion

        #region Vector2 by Matrix4

        /// <summary>
        /// Transforms a Vector2 by the given result.
        /// </summary>
        /// <param name="vector">The source Vector2.</param>
        /// <param name="matrix">The transformation result.</param>
        /// <returns>The Vector2 resulting from the transformation.</returns>
        public static Vector2 Transform( Vector2 vector, Matrix4 matrix )
        {
            Vector2 result;

            result.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + matrix.M41;
            result.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + matrix.M42;

            return result;
        }

        /// <summary>
        /// Transforms a Vector2 by the given result.
        /// </summary>
        /// <param name="vector">The source Vector2.</param>
        /// <param name="matrix">The transformation result.</param>
        /// <param name="result">The Vector2 resulting from the transformation.</param>
        public static void Transform( ref Vector2 vector, ref Matrix4 matrix, out Vector2 result )
        {
            result.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + matrix.M41;
            result.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + matrix.M42;
        }

        #endregion

        #region Vector2 Normal by Matrix4

        /// <summary>Transforms a 2D vector normal by a matrix.</summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed normal.</returns>
        public static Vector2 TransformNormal( Vector2 normal, Matrix4 matrix )
        {
            Vector2 result;

            result.X = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            result.Y = (normal.X * matrix.M12) + (normal.Y * matrix.M22);

            return result;
        }

        /// <summary>Transforms a vector normal by a matrix.</summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <param name="result">The Vector2 resulting from the transformation.</param>
        public static void TransformNormal( ref Vector2 normal, ref Matrix4 matrix, out Vector2 result )
        {
            result.X = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            result.Y = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
        }

        #endregion

        #region Vector2 by Quaternion

        /// <summary>Transforms a single Vector2, or the vector normal (x, y, 0, 0), by a specified <see cref="Quaternion"/> rotation.</summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <returns>Returns a new Vector2 containing the result of the rotation.</returns>
        public static Vector2 Transform( Vector2 vector, Quaternion rotation )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2Z = rotation.W * rot2Z;
            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotY2Y = rotation.Y * rot2Y;
            float rotZ2Z = rotation.Z * rot2Z;

            Vector2 result;

            result.X = (vector.X * (1f - rotY2Y - rotZ2Z)) + (vector.Y * (rotX2Y - rotW2Z));
            result.Y = (vector.X * (rotX2Y + rotW2Z))      + (vector.Y * (1f - rotX2X - rotZ2Z));

            return result;
        }

        /// <summary>
        /// Transforms a Vector2, or the vector normal (x, y, 0, 0), by a specified Quaternion rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="result">An existing Vector2 filled in with the result of the rotation.</param>
        public static void Transform( ref Vector2 vector, ref Quaternion rotation, out Vector2 result )
        {
            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2Z = rotation.W * rot2Z;
            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotY2Y = rotation.Y * rot2Y;
            float rotZ2Z = rotation.Z * rot2Z;

            result.X = (vector.X * (1f - rotY2Y - rotZ2Z)) + (vector.Y * (rotX2Y - rotW2Z));
            result.Y = (vector.X * (rotX2Y + rotW2Z))      + (vector.Y * (1f - rotX2X - rotZ2Z));
        }

        #endregion

        #region Vector2[] by Matrix4

        /// <summary>Transforms an array of Vector2s by a specified result.</summary>
        /// <param name="sourceArray">The array of Vector2s to transform.</param>
        /// <param name="matrix">The transform result to apply.</param>
        /// <param name="destinationArray">An existing array into which the transformed Vector2s are written.</param>
        public static void Transform( Vector2[] sourceArray, ref Matrix4 matrix, Vector2[] destinationArray )
        {
            #region Verify Arguments

            if( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray, "destinationArray" );

            #endregion

            for( int i = 0; i < sourceArray.Length; ++i )
            {
                Vector2 vector = sourceArray[i];
                Vector2 transformed;

                transformed.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + matrix.M41;
                transformed.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + matrix.M42;

                destinationArray[i] = transformed;
            }
        }

        /// <summary>
        /// Transforms a specified range in an array of Vector2s by a specified result and places the results in a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="sourceIndex">The index of the first Vector2 to transform in the source array.</param>
        /// <param name="matrix">The result to transform by.</param>
        /// <param name="destinationArray">The destination array into which the resulting Vector2s will be written.</param>
        /// <param name="destinationIndex">The index of the position in the destination array where the first result Vector2 should be written.</param>
        /// <param name="length">The number of Vector2s to be transformed.</param>
        public static void Transform( 
            Vector2[]   sourceArray, 
            int         sourceIndex, 
            ref Matrix4 matrix, 
            Vector2[]   destinationArray, 
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
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInSourceArray, "sourceArray" );

            if( destinationArray.Length < (destinationIndex + length) )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray, "destinationArray" );

            #endregion

            while( length > 0 )
            {
                Vector2 vector = sourceArray[sourceIndex];
                Vector2 transformed;

                transformed.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + matrix.M41;
                transformed.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + matrix.M42;

                destinationArray[destinationIndex] = transformed;

                ++sourceIndex;
                ++destinationIndex;
                --length;
            }
        }

        #endregion

        #region Vector2[] Normal by Matrix4

        /// <summary>
        /// Transforms an array of Vector2 vector normals by a specified result.
        /// </summary>
        /// <param name="sourceArray">The array of vector normals to transform.</param>
        /// <param name="matrix">The transform result to apply.</param>
        /// <param name="destinationArray">An existing array into which the transformed vector normals are written.</param>
        public static void TransformNormal( Vector2[] sourceArray, ref Matrix4 matrix, Vector2[] destinationArray )
        {
            #region Verify Arguments

            if( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray, "destinationArray" );

            #endregion

            for( int i = 0; i < sourceArray.Length; i++ )
            {
                Vector2 vector = sourceArray[i];
                Vector2 transformed;

                transformed.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21);
                transformed.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22);

                destinationArray[i] = transformed;
            }
        }

        /// <summary>
        /// Transforms a specified range in an array of Vector2 vector normals by
        /// a specified result and places the results in a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="sourceIndex">The index of the first Vector2 to transform in the source array.</param>
        /// <param name="matrix">The result to apply.</param>
        /// <param name="destinationArray">The destination array into which the resulting Vector2s are written.</param>
        /// <param name="destinationIndex">The index of the position in the destination array where the first result Vector2 should be written.</param>
        /// <param name="length">The number of vector normals to be transformed.</param>
        public static void TransformNormal( 
            Vector2[]   sourceArray, 
            int         sourceIndex,
            ref Matrix4 matrix,
            Vector2[]   destinationArray, 
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
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInSourceArray, "sourceArray" );

            if( destinationArray.Length < (destinationIndex + length) )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray, "destinationArray" );

            #endregion

            while( length > 0 )
            {
                Vector2 vector = sourceArray[sourceIndex];
                Vector2 transformed;

                transformed.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21);
                transformed.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22);

                destinationArray[destinationIndex] = transformed;

                ++sourceIndex;
                ++destinationIndex;
                --length;
            }
        }

        #endregion

        #region Vector2[] by Quaternion

        /// <summary>
        /// Transforms an array of Vector2s by a specified Quaternion.
        /// </summary>
        /// <param name="sourceArray">The array of Vector2s to transform.</param>
        /// <param name="rotation">The transform result to use.</param>
        /// <param name="destinationArray">An existing array into which the transformed Vector2s are written.</param>
        public static void Transform( Vector2[] sourceArray, ref Quaternion rotation, Vector2[] destinationArray )
        {
            #region Verify Arguments

            if( sourceArray == null )
                throw new ArgumentNullException( "sourceArray" );

            if( destinationArray == null )
                throw new ArgumentNullException( "destinationArray" );

            if( destinationArray.Length < sourceArray.Length )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray, "destinationArray" );

            #endregion

            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2Z = rotation.W * rot2Z;
            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotY2Y = rotation.Y * rot2Y;
            float rotZ2Z = rotation.Z * rot2Z;

            float factorXX = (1f - rotY2Y) - rotZ2Z;
            float factorYX = rotX2Y - rotW2Z;
            float factorXY = rotX2Y + rotW2Z;
            float factorYY = (1f - rotX2X) - rotZ2Z;

            for( int i = 0; i < sourceArray.Length; i++ )
            {
                Vector2 vector = sourceArray[i];
                Vector2 transformed;

                transformed.X = (vector.X * factorXX) + (vector.Y * factorYX);
                transformed.Y = (vector.X * factorXY) + (vector.Y * factorYY);

                destinationArray[i] = transformed;
            }
        }

        /// <summary>
        /// Transforms a specified range in an array of Vector2s by a specified Quaternion and places the results in a specified range in a destination array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <param name="sourceIndex">The index of the first Vector2 to transform in the source array.</param>
        /// <param name="rotation">The Quaternion rotation to apply.</param>
        /// <param name="destinationArray">The destination array into which the resulting Vector2s are written.</param>
        /// <param name="destinationIndex">The index of the position in the destination array where the first result Vector2 should be written.</param>
        /// <param name="length">The number of Vector2s to be transformed.</param>
        public static void Transform( 
            Vector2[]      sourceArray, 
            int            sourceIndex, 
            ref Quaternion rotation, 
            Vector2[]      destinationArray,
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
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInSourceArray, "sourceArray" );

            if( destinationArray.Length < (destinationIndex + length) )
                throw new ArgumentException( Atom.ErrorStrings.NotEnoughSpaceInTargetArray, "destinationArray" );

            #endregion

            float rot2X = rotation.X + rotation.X;
            float rot2Y = rotation.Y + rotation.Y;
            float rot2Z = rotation.Z + rotation.Z;

            float rotW2Z = rotation.W * rot2Z;
            float rotX2X = rotation.X * rot2X;
            float rotX2Y = rotation.X * rot2Y;
            float rotY2Y = rotation.Y * rot2Y;
            float rotZ2Z = rotation.Z * rot2Z;

            float factorXX = (1f - rotY2Y) - rotZ2Z;
            float factorYX = rotX2Y - rotW2Z;
            float factorXY = rotX2Y + rotW2Z;
            float factorYY = (1f - rotX2X) - rotZ2Z;

            while( length > 0 )
            {
                Vector2 vector = sourceArray[sourceIndex];
                Vector2 transformed;

                transformed.X = (vector.X * factorXX) + (vector.Y * factorYX);
                transformed.Y = (vector.X * factorXY) + (vector.Y * factorYY);

                destinationArray[destinationIndex] = transformed;

                ++sourceIndex;
                ++destinationIndex;
                --length;
            }
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
        public static Vector2 Add( Vector2 left, Vector2 right )
        {
            Vector2 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the <paramref name="right"/> Vector to the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Vector2 left, ref Vector2 right, out Vector2 result )
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
        }
        
        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Add( Vector2 vector, float scalar )
        {
            Vector2 result;

            result.X = vector.X + scalar;
            result.Y = vector.Y + scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the given <paramref name="scalar"/> to the given <paramref name="vector"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation. </param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Add( ref Vector2 vector, float scalar, out Vector2 result )
        {
            result.X = vector.X + scalar;
            result.Y = vector.Y + scalar;
        }
        
        /// <summary>
        /// This method returns the specified Vector.
        /// </summary>
        /// <remarks>
        /// Is equal to "+vector".
        /// </remarks>
        /// <param name="vector">The input vector.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Plus( Vector2 vector )
        {
            return vector;
        }

        /// <summary>
        /// This method stores the specified Vector in the specified result value.
        /// </summary>
        /// <remarks>
        /// Is equal to "+vector".
        /// </remarks>
        /// <param name="vector">The input vector.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Plus( ref Vector2 vector, out Vector2 result )
        {
            result.X = vector.X;
            result.Y = vector.Y;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Vector from the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Subtract( Vector2 left, Vector2 right )
        {
            Vector2 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the <paramref name="right"/> Vector frpm the <paramref name="left"/> Vector
        /// in the given Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The value on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Vector2 left, ref Vector2 right, out Vector2 result )
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Subtract( Vector2 vector, float scalar )
        {
            Vector2 result;

            result.X = vector.X - scalar;
            result.Y = vector.Y - scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="vector"/>
        /// in the given Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Subtract( ref Vector2 vector, float scalar, out Vector2 result )
        {
            result.X = vector.X - scalar;
            result.Y = vector.Y - scalar;
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
        public static Vector2 Negate( Vector2 vector )
        {
            return new Vector2( -vector.X, -vector.Y );
        }
        
        /// <summary>
        /// Stores the result of negating the elements of the given <paramref name="vector"/> in the given Vector.
        /// </summary>
        /// <param name="vector">
        /// The vector to negate. This value will not be modified by this method.
        /// </param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Negate( ref Vector2 vector, out Vector2 result )
        {
            result.X = -vector.X;
            result.Y = -vector.Y;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Returns the result of multiplying the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Multiply( Vector2 vector, float scalar )
        {
            Vector2 result;

            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// in the given Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Multiply( ref Vector2 vector, float scalar, out Vector2 result )
        {
            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;
        }

        /// <summary>
        /// Returns the result of multiplying the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="scalar">The scalar on the left side of the equation.</param>
        /// <param name="vector">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Multiply( float scalar, Vector2 vector )
        {
            Vector2 result;

            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// in the given Vector.
        /// </summary>
        /// <param name="scalar">The scalar on the left side of the equation.</param>
        /// <param name="vector">The vector on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Multiply( float scalar, ref Vector2 vector, out Vector2 result )
        {
            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Multiply( Vector2 left, Vector2 right )
        {
            Vector2 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the left Vector by the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The vector on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Multiply( ref Vector2 left, ref Vector2 right, out Vector2 result )
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Returns the result of dividing the given <paramref name="vector"/> through the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Divide( Vector2 vector, float scalar )
        {
            Vector2 result;

            result.X = vector.X / scalar;
            result.Y = vector.Y / scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the given <paramref name="vector"/> through the given <paramref name="scalar"/>
        /// in the given result Vector.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <param name="result">Will contain the result fo the operation.</param>
        public static void Divide( ref Vector2 vector, float scalar, out Vector2 result )
        {
            result.X = vector.X / scalar;
            result.Y = vector.Y / scalar;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 Divide( Vector2 left, Vector2 right )
        {
            Vector2 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the left Vector through the right Vector component-by-component.
        /// in the given result Vector.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The vector on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will store the result of the operation.</param>
        public static void Divide( ref Vector2 left, ref Vector2 right, out Vector2 result )
        {
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
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
        public static Vector2 Lerp( Vector2 start, Vector2 end, float amount )
        {
            Vector2 result;

            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);

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
        public static void Lerp( ref Vector2 start, ref Vector2 end, float amount, out Vector2 result )
        {
            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
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
        public static Vector2 Coserp( Vector2 start, Vector2 end, float amount )
        {
            float endFactor = 0.5f * (1.0f - (float)System.Math.Cos( amount * System.Math.PI ));
            float startFactor = 1.0f - endFactor;

            Vector2 result;

            result.X = (start.X * startFactor) + (end.X * endFactor);
            result.Y = (start.Y * startFactor) + (end.Y * endFactor);

            return result;
        }

        /// <summary>
        /// Performs COSine intERPolation between two vectors.
        /// </summary>
        /// <param name="start">The source value that represents the start vector. This value will not be modified by this method.</param>
        /// <param name="end">The source value that represents the end vector. This value will not be modified by this method.</param>
        /// <param name="amount">The weighting factor.</param>
        /// <param name="result">Will contain the interpolated value.</param>
        public static void Coserp( ref Vector2 start, ref Vector2 end, float amount, out Vector2 result )
        {
            float endFactor = 0.5f * (1.0f - (float)System.Math.Cos( amount * System.Math.PI ));
            float startFactor = 1.0f - endFactor;

            result.X = (start.X * startFactor) + (end.X * endFactor);
            result.Y = (start.Y * startFactor) + (end.Y * endFactor);
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
        public static Vector2 SmoothStep( Vector2 start, Vector2 end, float amount )
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            Vector2 result;

            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);

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
        public static void SmoothStep( ref Vector2 start, ref Vector2 end, float amount, out Vector2 result )
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
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
        public static Vector2 Hermite ( 
            Vector2 valueA,
            Vector2 tangentA, 
            Vector2 valueB,
            Vector2 tangentB,
            float amount )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            float factorValueA   = ((2f * amountPow3) - (3f * amountPow2)) + 1f;
            float factorValueB   = (-2f * amountPow3) + (3f * amountPow2);
            float factorTangentA = (amountPow3 - (2f * amountPow2)) + amount;
            float factorTangentB = amountPow3 - amountPow2;

            Vector2 result;

            result.X = (valueA.X * factorValueA) + (valueB.X * factorValueB) + (tangentA.X * factorTangentA) + (tangentB.X * factorTangentB);
            result.Y = (valueA.Y * factorValueA) + (valueB.Y * factorValueB) + (tangentA.Y * factorTangentA) + (tangentB.Y * factorTangentB);

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
            ref Vector2 valueA, 
            ref Vector2 tangentA,
            ref Vector2 valueB,
            ref Vector2 tangentB, 
            float amount, 
            out Vector2 result 
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
        public static Vector2 CatmullRom( Vector2 valueA, Vector2 valueB, Vector2 valueC, Vector2 valueD, float amount )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            Vector2 result;

            result.X = 0.5f * ((((2f * valueB.X) + ((-valueA.X + valueC.X) * amount)) + (((((2f * valueA.X) - (5f * valueB.X)) + (4f * valueC.X)) - valueD.X) * amountPow2)) + ((((-valueA.X + (3f * valueB.X)) - (3f * valueC.X)) + valueD.X) * amountPow3));
            result.Y = 0.5f * ((((2f * valueB.Y) + ((-valueA.Y + valueC.Y) * amount)) + (((((2f * valueA.Y) - (5f * valueB.Y)) + (4f * valueC.Y)) - valueD.Y) * amountPow2)) + ((((-valueA.Y + (3f * valueB.Y)) - (3f * valueC.Y)) + valueD.Y) * amountPow3));

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
        public static void CatmullRom( ref Vector2 valueA, ref Vector2 valueB, ref Vector2 valueC, ref Vector2 valueD, float amount, out Vector2 result )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            result.X = 0.5f * ((((2f * valueB.X) + ((-valueA.X + valueC.X) * amount)) + (((((2f * valueA.X) - (5f * valueB.X)) + (4f * valueC.X)) - valueD.X) * amountPow2)) + ((((-valueA.X + (3f * valueB.X)) - (3f * valueC.X)) + valueD.X) * amountPow3));
            result.Y = 0.5f * ((((2f * valueB.Y) + ((-valueA.Y + valueC.Y) * amount)) + (((((2f * valueA.Y) - (5f * valueB.Y)) + (4f * valueC.Y)) - valueD.Y) * amountPow2)) + ((((-valueA.Y + (3f * valueB.Y)) - (3f * valueC.Y)) + valueD.Y) * amountPow3));
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
        public static Vector2 Max( Vector2 vectorA, Vector2 vectorB )
        {
            Vector2 result;

            result.X = (vectorA.X > vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y > vectorB.Y) ? vectorA.Y : vectorB.Y;

            return result;
        }

        /// <summary>
        /// Returns a vector that contains the highest value from
        /// each matching pair of components of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second vector. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the maximized vector.</param>
        public static void Max( ref Vector2 vectorA, ref Vector2 vectorB, out Vector2 result )
        {
            result.X = (vectorA.X > vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y > vectorB.Y) ? vectorA.Y : vectorB.Y;
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
        public static Vector2 Min( Vector2 vectorA, Vector2 vectorB )
        {
            Vector2 result;

            result.X = (vectorA.X < vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y < vectorB.Y) ? vectorA.Y : vectorB.Y;

            return result;
        }

        /// <summary>
        /// Returns a vector that contains the lowest value from
        /// each matching pair of components of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second vector. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the minimized vector.</param>
        public static void Min( ref Vector2 vectorA, ref Vector2 vectorB, out Vector2 result )
        {
            result.X = (vectorA.X < vectorB.X) ? vectorA.X : vectorB.X;
            result.Y = (vectorA.Y < vectorB.Y) ? vectorA.Y : vectorB.Y;
        }

        #endregion

        #region Average

        /// <summary>
        /// Returns the average of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector.</param>
        /// <param name="vectorB">The second vector.</param>
        /// <returns>The average of the given Vectors.</returns>
        public static Vector2 Average( Vector2 vectorA, Vector2 vectorB )
        {
            Vector2 result;

            result.X = (vectorA.X + vectorB.X) * 0.5f;
            result.Y = (vectorA.Y + vectorB.Y) * 0.5f;

            return result;
        }

        /// <summary>
        /// Returns the average of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second vector. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the average of the given Vectors.</param>
        public static void Average( ref Vector2 vectorA, ref Vector2 vectorB, out Vector2 result )
        {
            result.X = (vectorA.X + vectorB.X) * 0.5f;
            result.Y = (vectorA.Y + vectorB.Y) * 0.5f;
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
        public static float Distance( Vector2 vectorA, Vector2 vectorB )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;

            float squaredLength = (deltaX * deltaX) + (deltaY * deltaY);
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
        public static void Distance( ref Vector2 vectorA, ref Vector2 vectorB, out float result )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;

            float squaredLength = (deltaX * deltaX) + (deltaY * deltaY);
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
        public static float DistanceSquared( Vector2 vectorA, Vector2 vectorB )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;

            return (deltaX * deltaX) + (deltaY * deltaY);
        }

        /// <summary>
        /// Stores the squared distance between the two specified Vectors2 in the given <paramref name="result"/> value.
        /// </summary>
        /// <param name="vectorA">The first Vector. This value will not be modified by this method.</param>
        /// <param name="vectorB">The second Vector. This value will not be modified by this method.</param>
        /// <param name="result">
        /// This value will contain the squared distance from the <paramref name="vectorA"/> to <paramref name="vectorB"/>.
        /// </param>
        public static void DistanceSquared( ref Vector2 vectorA, ref Vector2 vectorB, out float result )
        {
            float deltaX = vectorA.X - vectorB.X;
            float deltaY = vectorA.Y - vectorB.Y;

            result = (deltaX * deltaX) + (deltaY * deltaY);
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
        public static Vector2 Clamp( Vector2 vector, Vector2 min, Vector2 max )
        {
            Vector2 result;

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
        public static void Clamp( ref Vector2 vector, ref Vector2 min, ref Vector2 max, out Vector2 result )
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
        }

        #endregion

        #region Round

        /// <summary>
        /// Rounds the components of the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The input vector.</param>
        /// <returns>The rounded vector.</returns>
        public static Vector2 Round( Vector2 vector )
        {
            vector.X = (float)System.Math.Round( vector.X );
            vector.Y = (float)System.Math.Round( vector.Y );

            return vector;
        }
        
        /// <summary>
        /// Rounds the components of the given <paramref name="vector"/>
        /// to a specified precission.
        /// </summary>
        /// <param name="vector">The input vector.</param>
        /// <param name="digits">The number of digits after the comma.</param>
        /// <returns>The rounded vector.</returns>
        public static Vector2 Round( Vector2 vector, int digits )
        {
            vector.X = (float)System.Math.Round( vector.X, digits );
            vector.Y = (float)System.Math.Round( vector.Y, digits );

            return vector;
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether the given <see cref="Vector2"/> has the
        /// same indices set as this Vector2.
        /// </summary>
        /// <param name="other">The Vector2 to test against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the elements of the vectors are approximately equal; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( Vector2 other )
        {
            return this.X.IsApproximate( other.X ) &&
                   this.Y.IsApproximate( other.Y );
        }

        /// <summary>
        /// Returns whether the given <see cref="Object"/> is equal to this Vector2.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal; 
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is Vector2 )
            {
                return Equals( (Vector2)obj );
            }
            else if( obj is Point2 )
            {
                Point2 vector = (Point2)obj;

                return this.X.IsApproximate( (float)vector.X ) &&
                       this.Y.IsApproximate( (float)vector.Y );
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overriden to return a human-readable text representation of the Vector2.
        /// </summary>
        /// <returns>A human-readable text representation of the Vector2.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Vector2.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Vector2.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "[{0} {1}]",
                X.ToString( formatProvider ),
                Y.ToString( formatProvider )
            );
        }

        #endregion

        /// <summary>
        /// Overriden to return the hash code of the <see cref="Vector2"/>.
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

        #region Logic

        /// <summary>
        /// Returns whether the specified <see cref="Vector2"/> instances are (approximately) equal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator ==( Vector2 left, Vector2 right )
        {
            return left.X.IsApproximate( right.X ) &&
                   left.Y.IsApproximate( right.Y );
        }

        /// <summary>
        /// Returns whether the specified <see cref="Vector2"/> instances are (approximately) inequal.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator !=( Vector2 left, Vector2 right )
        {
            return !left.X.IsApproximate( right.X ) ||
                   !left.Y.IsApproximate( right.Y );
        }

        #endregion

        #region Cast

        /// <summary>
        /// Explicit cast operator that implements conversion
        /// from a <see cref="Vector2"/> to a <see cref="Point2"/>.
        /// </summary>
        /// <param name="vector">
        /// The input vector.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static explicit operator Point2( Vector2 vector )
        {
            return new Point2( (int)vector.X, (int)vector.Y );
        }

        #endregion

        #region +

        /// <summary>
        /// Returns the result of adding the <paramref name="right"/> Vector to the the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 operator +( Vector2 left, Vector2 right )
        {
            return new Vector2( left.X + right.X, left.Y + right.Y );
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the thegiven <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 operator +( Vector2 vector, float scalar )
        {
            return new Vector2( vector.X + scalar, vector.Y + scalar );
        }

        /// <summary>
        /// Returns the original specified vector, doing nothing.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 operator +( Vector2 vector )
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
        public static Vector2 operator -( Vector2 left, Vector2 right )
        {
            return new Vector2( left.X - right.X, left.Y - right.Y );
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 operator -( Vector2 vector, float scalar )
        {
            return new Vector2( vector.X - scalar, vector.Y - scalar );
        }

        /// <summary>
        /// Returns the result of negating the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 operator -( Vector2 vector )
        {
            return new Vector2( -vector.X, -vector.Y );
        }

        #endregion

        #region *

        /// <summary>
        /// Returns the result of multiplcing the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="scalar">The scalar on the left side of the equation.</param>
        /// <param name="vector">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 operator *( float scalar, Vector2 vector )
        {
            return new Vector2( vector.X * scalar, vector.Y * scalar );
        }

        /// <summary>
        /// Returns the result of multiplcing the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 operator *( Vector2 vector, float scalar )
        {
            return new Vector2( vector.X * scalar, vector.Y * scalar );
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 operator *( Vector2 left, Vector2 right )
        {
            Vector2 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;

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
        public static Vector2 operator /( Vector2 vector, float scalar )
        {
            return new Vector2( vector.X / scalar, vector.Y / scalar );
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector element-by-element.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2 operator /( Vector2 left, Vector2 right )
        {
            Vector2 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;

            return result;
        }

        #endregion

        #endregion
    }
}
