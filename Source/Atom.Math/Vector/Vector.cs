// <copyright file="Vector.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Vector class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Defines a N-dimensional single-precision floating-point Vector.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( System.ComponentModel.ExpandableObjectConverter ) )]
    public class Vector : IEnumerable<float>, IEquatable<Vector>, ISerializable, ICultureSensitiveToStringProvider
    {
        #region [ Constants ]

        /// <summary>
        /// Returns a <see cref="Vector"/> with all its components set to zero.
        /// </summary>
        /// <param name="dimensionCount">
        /// The length of the Vector.
        /// </param>
        /// <returns>The vector (0, 0, ..., 0).</returns>
        public static Vector Zero( int dimensionCount ) 
        {
            return new Vector( dimensionCount ); 
        }

        /// <summary>
        /// Returns a <see cref="Vector"/> with all its components set to one.
        /// </summary>
        /// <param name="dimensionCount">
        /// The length of the Vector.
        /// </param>
        /// <returns>The vector (1, 1, ..., 1).</returns>
        public static Vector One( int dimensionCount )
        { 
            Vector vector = new Vector( dimensionCount );

            for( int i = 0; i < vector.elements.Length; ++i )
                vector.elements[i] = 1.0f;

            return vector;
        }

        /// <summary>
        /// Returns the unit <see cref="Vector"/> for the specified <paramref name="axisIndex"/>.
        /// </summary>
        /// <param name="length">
        /// The length of the Vector.
        /// </param>
        /// <param name="axisIndex">
        /// The zero-based index of the axis that is represented.
        /// </param>
        /// <returns>
        /// The vector (0, 0, ..., 0) where the axis at the given <paramref name="axisIndex"/> is set to 1.
        /// </returns>
        public static Vector Unit( int length, int axisIndex )
        {
            if( axisIndex < 0 || axisIndex >= length )
                throw new ArgumentOutOfRangeException( "axisIndex", axisIndex, Atom.ErrorStrings.SpecifiedIndexIsOutOfValidRange ); 

            Vector vector = new Vector( length );
            vector.elements[axisIndex] = 1.0f;
            return vector;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets a value that represents the total
        /// number of elements the Vector has.
        /// </summary>
        /// <value>The total number of elements.</value>
        public int DimensionCount
        {
            get { return this.elements.Length; }
        }

        /// <summary>
        /// Gets or sets the element of the Vector at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to receive or set.
        /// </param>
        /// <returns>
        /// The element at the specified <paramref name="index"/>.
        /// </returns>
        public float this[int index]
        {
            get { return elements[index];  }
            set { elements[index] = value; }
        }

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
                float squaredLength = 0.0f;

                for( int i = 0; i < elements.Length; ++i )
                {
                    float value = elements[i];
                    squaredLength += value * value;
                }

                return (float)System.Math.Sqrt( squaredLength );
            }

            set
            {
                if( value < 0.0f )
                    throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "value" );

                if( float.IsInfinity( value ) )
                {
                    for( int i = 0; i < elements.Length; ++i )
                        elements[i] = value;
                }
                else
                {
                    float length = this.Length;
                    if( length == 0.0f )
                        throw new InvalidOperationException( MathErrorStrings.OperationInvalidOnZeroVector );

                    float ratio = value / length;
                    for( int i = 0; i < elements.Length; ++i )
                        elements[i] *= ratio;
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
                float squaredLength = 0.0f;

                for( int i = 0; i < elements.Length; ++i )
                {
                    float value = elements[i];
                    squaredLength += value * value;
                }

                return squaredLength;
            }

            set
            {
                if( value < 0.0f )
                    throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "value" );

                if( float.IsInfinity( value ) )
                {
                    for( int i = 0; i < elements.Length; ++i )
                        elements[i] = value;
                }
                else
                {
                    float squaredLength = this.SquaredLength;
                    if( squaredLength == 0.0f )
                        throw new InvalidOperationException( MathErrorStrings.OperationInvalidOnZeroVector );

                    float ratio = value / squaredLength;
                    for( int i = 0; i < elements.Length; ++i )
                        elements[i] *= ratio;
                }
            }
        }

        /// <summary>
        /// Gets the sum of all elements of the <see cref="Vector"/>.
        /// </summary>
        /// <value>The elements of the vector summed together.</value>
        public float Sum
        {
            get
            {
                float sum = 0.0f;

                for( int i = 0; i < this.elements.Length; ++i )
                    sum += this.elements[i];

                return sum;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="dimensionCount">
        /// The number of elements the new Vector can hold.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the specified <paramref name="dimensionCount"/> is less than or equal zero.
        /// </exception>
        public Vector( int dimensionCount )
        {
            if( dimensionCount <= 0 )
            {
                throw new ArgumentOutOfRangeException(
                    "dimensionCount",
                    dimensionCount,
                    Atom.ErrorStrings.SpecifiedValueIsZeroOrNegative
                );
            }

            this.elements = new float[dimensionCount];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class
        /// by copying the specified Vector.
        /// </summary>
        /// <param name="vector">
        /// The vector to copy.
        /// </param>
        public Vector( Vector2 vector )
        {
            this.elements = new float[2];

            elements[0] = vector.X;
            elements[1] = vector.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class
        /// by copying the specified Vector.
        /// </summary>
        /// <param name="vector">
        /// The vector to copy.
        /// </param>
        public Vector( Vector3 vector )
        {
            this.elements = new float[3];

            elements[0] = vector.X;
            elements[1] = vector.Y;
            elements[2] = vector.Z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class
        /// by copying the specified Vector.
        /// </summary>
        /// <param name="vector">
        /// The vector to copy.
        /// </param>
        public Vector( Vector4 vector )
        {
            this.elements = new float[4];

            elements[0] = vector.X;
            elements[1] = vector.Y;
            elements[2] = vector.Z;
            elements[3] = vector.W;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="elements">
        /// The elements the new Vector.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="elements"/> is null.
        /// </exception>
        public Vector( IEnumerable<float> elements )
        {
            if( elements == null )
                throw new ArgumentNullException( "elements" );

            this.elements = elements.ToArray();
        }

        #region Vector( SerializationInfo info, StreamingContext context )

        /// <summary> 
        /// Initializes a new instance of the <see cref="Vector"/> class; and
        /// sets the Elements of the new <see cref="Vector"/> to the "Elements"
        /// array inside the specified <see cref="System.Runtime.Serialization.SerializationInfo"/>.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext "/> that 
        /// contains contextual information about the source or destination.
        /// Can be null.
        /// </param>
        protected Vector( 
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        {
            elements = (float[])info.GetValue( "Elements", typeof(Array) );
        }

        #endregion

        #endregion

        #region [ Methods ]

        #region Normalize

        /// <summary>
        /// Normalizes the Vector, setting its length to one.
        /// </summary>
        public void Normalize()
        {
            float squaredLength = this.SquaredLength;
            float invLength     = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            for( int i = 0; i < this.elements.Length; ++i )
                this[i] *= invLength;
        }

        /// <summary>
        /// Returns the result of normalizing the given Vector.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static Vector Normalize( Vector vector )
        {
            if( vector == null )
                throw new ArgumentNullException( "vector" );

            float squaredLength = vector.SquaredLength;
            float invLength     = 1.0f / ((float)System.Math.Sqrt( squaredLength ));

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
                result[i] = vector[i] * invLength;

            return result;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Returns the dot product of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The vector on the left side of the equation.</param>
        /// <param name="vectorB">The vector on the right side of the equation.</param>
        /// <returns>
        /// The dot product.
        /// </returns> 
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static float Dot( Vector vectorA, Vector vectorB )
        {
            Contract.Requires<ArgumentNullException>( vectorA != null );
            Contract.Requires<ArgumentNullException>( vectorB != null );
            Contract.Requires<ArgumentException>( vectorA.DimensionCount == vectorB.DimensionCount );

            float dot = 0.0f;

            for( int i = 0; i < vectorA.elements.Length; ++i )
            {
                float valueA = vectorA.elements[i];
                float valueB = vectorB.elements[i];

                dot += valueA * valueB;
            }

            return dot;
        }

        #endregion

        #region Dyadic

        /// <summary>
        /// Returns the dyadic product (also called outer/tensor product) of the given N-Vectors. </summary>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Tensor_product for more information.
        /// </remarks>
        /// <param name="left">The first input vector.</param>
        /// <param name="right">The second input vector.</param>
        /// <returns> The result of the operation. </returns>
        public static Matrix Dyadic( Vector left, Vector right )
        {
            Contract.Requires<ArgumentNullException>( left != null );
            Contract.Requires<ArgumentNullException>( right != null );
            Contract.Requires<ArgumentException>( left.DimensionCount == right.DimensionCount );

            int    size   = left.DimensionCount;
            Matrix result = new Matrix( size, size );

            for( int row = 0; row < size; ++row )
            {
                float valueLeftRow  = left[row];

                for( int column = 0; column < size; ++column )
                {
                    result[row, column] = valueLeftRow * right[column];
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Returns the result of transforming the given Vector by the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="vector">
        /// The vector on the left side.
        /// </param>
        /// <param name="matrix">
        /// The matrix on the right side.
        /// </param>
        /// <returns>
        /// The transformed Vector.
        /// </returns>
        public static Vector Transform( Vector vector, Matrix matrix )
        {
            Contract.Requires<ArgumentNullException>( vector != null );
            Contract.Requires<ArgumentNullException>( matrix != null );
            Contract.Requires<ArgumentException>( vector.DimensionCount == matrix.RowCount );

            int size   = Math.Min( vector.DimensionCount, matrix.ColumnCount );
            var result = new Vector( size );

            for( int column = 0; column < vector.DimensionCount; ++column )
            {
                float accum = 0.0f;

                for( int row = 0; row < size; ++row )
                {
                    accum += vector[row] * matrix[row, column];
                }

                result[column] = accum;
            }

            return result;
        }

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
        /// <param name="amount">Value between 0 and 1 indicating the amount of interpolation done.</param>
        /// <returns>The linear interpolation of the two vectors.</returns>   
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector Lerp( Vector start, Vector end, float amount )
        {
            Contract.Requires<ArgumentNullException>( start != null );
            Contract.Requires<ArgumentNullException>( end != null );
            Contract.Requires<ArgumentException>( start.DimensionCount == end.DimensionCount );

            Vector result = new Vector( start.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                float startValue = start.elements[i];
                float endValue   = end.elements[i];

                result.elements[i] = startValue + ((endValue - startValue) * amount);
            }

            return result;
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
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector SmoothStep( Vector start, Vector end, float amount )
        {
            Contract.Requires<ArgumentNullException>( start != null );
            Contract.Requires<ArgumentNullException>( end != null );
            Contract.Requires<ArgumentException>( start.DimensionCount == end.DimensionCount );

            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            Vector result = new Vector( start.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                float startValue = start.elements[i];
                float endValue   = end.elements[i];

                result.elements[i] = startValue + ((endValue - startValue) * amount);
            }

            return result;
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
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector Hermite(
            Vector valueA,
            Vector tangentA,
            Vector valueB,
            Vector tangentB,
            float  amount )
        {
            Contract.Requires<ArgumentNullException>( valueA != null );
            Contract.Requires<ArgumentNullException>( tangentA != null );
            Contract.Requires<ArgumentNullException>( valueB != null );
            Contract.Requires<ArgumentNullException>( tangentB != null );
            Contract.Requires<ArgumentException>( valueA.DimensionCount == valueB.DimensionCount );
            Contract.Requires<ArgumentException>( valueA.DimensionCount == tangentA.DimensionCount );
            Contract.Requires<ArgumentException>( tangentA.DimensionCount == tangentB.DimensionCount );

            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            float factorValueA   = ((2f * amountPow3) - (3f * amountPow2)) + 1f;
            float factorValueB   = (-2f * amountPow3) + (3f * amountPow2);
            float factorTangentA = (amountPow3 - (2f * amountPow2)) + amount;
            float factorTangentB = amountPow3 - amountPow2;

            Vector result = new Vector( valueA.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = 
                    (valueA.elements[i] * factorValueA) +
                    (valueB.elements[i] * factorValueB) +
                    (tangentA.elements[i] * factorTangentA) + 
                    (tangentB.elements[i] * factorTangentB);
            }

            return result;
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
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector CatmullRom( Vector valueA, Vector valueB, Vector valueC, Vector valueD, float amount )
        {
            #region Verify Arguments

            if( valueA == null )
                throw new ArgumentNullException( "valueA" );
            if( valueB == null )
                throw new ArgumentNullException( "valueA" );
            if( valueC == null )
                throw new ArgumentNullException( "valueC" );
            if( valueD == null )
                throw new ArgumentNullException( "valueD" );

            if( valueA.DimensionCount != valueB.DimensionCount || valueB.DimensionCount != valueC.DimensionCount || valueC.DimensionCount != valueD.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            Vector result = new Vector( valueA.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                float a = valueA.elements[i];
                float b = valueB.elements[i];
                float c = valueC.elements[i];
                float d = valueD.elements[i];

                result.elements[i] = 0.5f * ((((2f * b) + ((-a + c) * amount)) + (((((2f * a) - (5f * b)) + (4f * c)) - d) * amountPow2)) + ((((-a + (3f * b)) - (3f * c)) + d) * amountPow3));
            }

            return result;
        }

        #endregion

        #endregion

        #region > Operators <

        #region Add

        /// <summary>
        /// Returns the result of adding the <paramref name="right"/> Vector to the the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector Add( Vector left, Vector right )
        {
            #region Verify Arguments

            if( left == null )
                throw new ArgumentNullException( "left" );
            if( right == null )
                throw new ArgumentNullException( "right" );

            if( left.DimensionCount != right.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( left.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = left.elements[i] + right.elements[i];
            }

            return result;
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the thegiven <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified Vectors is null.
        /// </exception>
        public static Vector Add( Vector vector, float scalar )
        {
            #region Verify Arguments

            if( vector == null )
                throw new ArgumentNullException( "vector" );

            #endregion

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = vector.elements[i] + scalar;
            }

            return result;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Vector from the the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>    
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector Subtract( Vector left, Vector right )
        {
            #region Verify Arguments

            if( left == null )
                throw new ArgumentNullException( "left" );
            if( right == null )
                throw new ArgumentNullException( "right" );

            if( left.DimensionCount != right.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( left.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = left.elements[i] - right.elements[i];
            }

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified Vectors is null.
        /// </exception>
        public static Vector Subtract( Vector vector, float scalar )
        {
            #region Verify Arguments

            if( vector == null )
                throw new ArgumentNullException( "vector" );

            #endregion

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = vector.elements[i] - scalar;
            }

            return result;
        }

        #endregion

        #region Negate

        /// <summary>
        /// Returns the result of negating the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified Vectors is null.
        /// </exception>
        public static Vector Negate( Vector vector )
        {
            #region Verify Arguments

            if( vector == null )
                throw new ArgumentNullException( "vector" );

            #endregion

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = -vector.elements[i];
            }

            return result;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Returns the result of multiplcing the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified Vectors is null.
        /// </exception>
        public static Vector Multiply( Vector vector, float scalar )
        {
            #region Verify Arguments

            if( vector == null )
                throw new ArgumentNullException( "vector" );

            #endregion

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = vector.elements[i] * scalar;
            }

            return result;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>  
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector Multiply( Vector left, Vector right )
        {
            #region Verify Arguments

            if( left == null )
                throw new ArgumentNullException( "left" );
            if( right == null )
                throw new ArgumentNullException( "right" );

            if( left.DimensionCount != right.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( left.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = left.elements[i] * right.elements[i];
            }

            return result;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Returns the result of dividing the given <paramref name="vector"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified Vectors is null.
        /// </exception>
        public static Vector Divide( Vector vector, float scalar )
        {
            #region Verify Arguments

            if( vector == null )
                throw new ArgumentNullException( "vector" );

            #endregion

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = vector.elements[i] / scalar;
            }

            return result;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector element-by-element.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns> 
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector Divide( Vector left, Vector right )
        {
            #region Verify Arguments

            if( left == null )
                throw new ArgumentNullException( "left" );
            if( right == null )
                throw new ArgumentNullException( "right" );

            if( left.DimensionCount != right.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( left.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = left.elements[i] / right.elements[i];
            }

            return result;
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region IEnumerable<float>

        /// <summary>
        /// Returns an <see cref="IEnumerator{Single}"/>
        /// over the elements of the <see cref="Vector"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{Single}"/>
        /// over the elements of the <see cref="Vector"/>.
        /// </returns>
        public IEnumerator<float> GetEnumerator()
        {
            for( int i = 0; i < elements.Length; ++i )
                yield return elements[i];
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator{Single}"/>
        /// over the elements of the <see cref="Vector"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{Single}"/>
        /// over the elements of the <see cref="Vector"/>.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region GetObjectData

        /// <summary>
        /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/>
        /// with the data needed to serialize the <see cref="Vector"/>.
        /// </summary>
        /// <remarks>
        /// elements component: "Elements"
        /// </remarks>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/>
        /// to populate with data.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// If the given info is null.
        /// </exception>
        /// <param name="context"> 
        /// The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>)
        /// for this serialization.
        /// </param>
        [System.Security.Permissions.SecurityPermissionAttribute(
         System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true )]
        public virtual void GetObjectData( 
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        {
            if( info == null )
                throw new System.ArgumentNullException( "info" );

            info.SetType( this.GetType() );
            info.AddValue( "Elements", elements.Clone(), elements.GetType() );
        }

        #endregion

        #region Equals

        /// <summary>
        /// Returns whether the given <see cref="Vector"/> has the
        /// same indices set as this Vector.
        /// </summary>
        /// <param name="other">The Vector to test against. Can be null.</param>
        /// <returns>true if the indices are equal; otherwise false.</returns>
        public bool Equals( Vector other )
        {
            if( other == null )
                return false;

            if( this.DimensionCount != other.DimensionCount )
                return false;

            for( int i = 0; i < this.elements.Length; ++i )
            {
                if( !this.elements[i].IsApproximate( other.elements[i] ) )
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns whether the given <see cref="Object"/> is equal to this Vector.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public override bool Equals( object obj )
        {
            Vector vector = obj as Vector;

            if( vector != null )
                return this.Equals( vector );

            return false;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overriden to return a human-readable text representation of the Vector.
        /// </summary>
        /// <returns>A human-readable text representation of the Vector.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Vector.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Vector.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder( this.DimensionCount * 7 );

            sb.Append( '[' );

            int lastIndex = this.elements.Length - 1;
            for( int i = 0; i < this.elements.Length; ++i )
            {
                sb.Append( this.elements[i].ToString( formatProvider ) );

                if( i != lastIndex ) 
                    sb.Append( ' ' );
            }

            sb.Append( ']' );

            return sb.ToString();
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets the hash code of the <see cref="Vector"/>.
        /// </summary>
        /// <returns>
        /// The hash code.
        /// </returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            for( int i = 0; i < this.elements.Length; ++i )
            {
                hashBuilder.AppendStruct( this.elements[i] );
            }

            return hashBuilder.GetHashCode();
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
        /// <exception cref="ArgumentNullException">
        /// If any of the two specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector Max( Vector vectorA, Vector vectorB )
        {
            #region Verify Arguments

            if( vectorA == null )
                throw new ArgumentNullException( "vectorA" );
            if( vectorB == null )
                throw new ArgumentNullException( "vectorB" );

            if( vectorA.DimensionCount != vectorB.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( vectorA.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                float valueA = vectorA.elements[i];
                float valueB = vectorB.elements[i];

                result.elements[i] = (valueA > valueB) ? valueA : valueB;
            }

            return result;
        }

        #endregion

        #region Min

        /// <summary>
        /// Returns a Vector that contains the lowest value from
        /// each matching pair of components of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector.</param>
        /// <param name="vectorB">The second vector.</param>
        /// <returns>
        /// The minimized vector.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If any of the two specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector Min( Vector vectorA, Vector vectorB )
        {
            #region Verify Arguments

            if( vectorA == null )
                throw new ArgumentNullException( "vectorA" );
            if( vectorB == null )
                throw new ArgumentNullException( "vectorB" );

            if( vectorA.DimensionCount != vectorB.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( vectorA.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                float valueA = vectorA.elements[i];
                float valueB = vectorB.elements[i];

                result.elements[i] = (valueA < valueB) ? valueA : valueB;
            }

            return result;
        }

        #endregion

        #region Average

        /// <summary>
        /// Returns the average of the given Vectors.
        /// </summary>
        /// <param name="vectorA">The first vector.</param>
        /// <param name="vectorB">The second vector.</param>
        /// <returns>The average of the given Vectors.</returns> 
        /// <exception cref="ArgumentNullException">
        /// If any of the two specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector Average( Vector vectorA, Vector vectorB )
        {          
            #region Verify Arguments

            if( vectorA == null )
                throw new ArgumentNullException( "vectorA" );
            if( vectorB == null )
                throw new ArgumentNullException( "vectorB" );

            if( vectorA.DimensionCount != vectorB.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( vectorA.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                float valueA = vectorA.elements[i];
                float valueB = vectorB.elements[i];

                result.elements[i] = (valueA + valueB) * 0.5f;
            }

            return result;
        }

        #endregion

        #region Distance

        #region Distance

        /// <summary>
        /// Returns the distance between the two specified Vectors.
        /// </summary>
        /// <param name="vectorA">The first Vector.</param>
        /// <param name="vectorB">The second Vector.</param>
        /// <returns>
        /// The distance from the <paramref name="vectorA"/> to <paramref name="vectorB"/>.
        /// </returns>  
        /// <exception cref="ArgumentNullException">
        /// If any of the two specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static float Distance( Vector vectorA, Vector vectorB )
        {
            #region Verify Arguments

            if( vectorA == null )
                throw new ArgumentNullException( "vectorA" );
            if( vectorB == null )
                throw new ArgumentNullException( "vectorB" );

            if( vectorA.DimensionCount != vectorB.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            float squaredLength = 0.0f;

            for( int i = 0; i < vectorA.elements.Length; ++i )
            {
                float valueA = vectorA.elements[i];
                float valueB = vectorB.elements[i];
                float delta  = valueA - valueB;

                squaredLength += delta * delta;
            }

            return (float)System.Math.Sqrt( squaredLength );
        }

        #endregion

        #region DistanceSquared

        /// <summary>
        /// Returns the squared distance between the two specified Vectors.
        /// </summary>
        /// <param name="vectorA">The first Vector.</param>
        /// <param name="vectorB">The second Vector.</param>
        /// <returns>
        /// The distance from the <paramref name="vectorA"/> to <paramref name="vectorB"/>.
        /// </returns>   
        /// <exception cref="ArgumentNullException">
        /// If any of the two specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static float DistanceSquared( Vector vectorA, Vector vectorB )
        {
            #region Verify Arguments

            if( vectorA == null )
                throw new ArgumentNullException( "vectorA" );
            if( vectorB == null )
                throw new ArgumentNullException( "vectorB" );

            if( vectorA.DimensionCount != vectorB.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            float squaredLength = 0.0f;

            for( int i = 0; i < vectorA.elements.Length; ++i )
            {
                float valueA = vectorA.elements[i];
                float valueB = vectorB.elements[i];
                float delta  = valueA - valueB;

                squaredLength += delta * delta;
            }

            return squaredLength;
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
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector Clamp( Vector vector, Vector min, Vector max )
        {
            #region Verify Arguments

            if( vector == null )
                throw new ArgumentNullException( "vector" );
            if( min == null )
                throw new ArgumentNullException( "min" );
            if( max == null )
                throw new ArgumentNullException( "max" );

            if( max.DimensionCount != min.DimensionCount || vector.DimensionCount != max.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                float orgValue = vector.elements[i];
                float minValue =    min.elements[i];
                float maxValue =    max.elements[i];

                float value;

                if( orgValue > maxValue )
                    value = maxValue;
                else if( orgValue < minValue )
                    value = minValue;
                else
                    value = orgValue;

                result.elements[i] = value;
            }

            return result;
        }

        #endregion

        #region ToMatrix

        /// <summary>
        /// Returns this <see cref="Vector"/> represented as a new <see cref="Matrix"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="Matrix"/> with N rows and 1 column, where N is this vector's DimensionCount.
        /// </returns>
        public Matrix ToMatrix()
        {
            Matrix matrix = new Matrix( this.DimensionCount, 1 );

            for( int i = 0; i < this.elements.Length; ++i )
            {
                matrix[i, 0] = this.elements[i];
            }

            return matrix;
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
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector operator +( Vector left, Vector right )
        {
            #region Verify Arguments

            if( left == null )
                throw new ArgumentNullException( "left" );
            if( right == null )
                throw new ArgumentNullException( "right" );

            if( left.DimensionCount != right.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( left.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = left.elements[i] + right.elements[i];
            }

            return result;
        }

        /// <summary>
        /// Returns the result of adding the given <paramref name="scalar"/> to the thegiven <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified Vectors is null.
        /// </exception>
        public static Vector operator +( Vector vector, float scalar )
        {
            #region Verify Arguments

            if( vector == null )
                throw new ArgumentNullException( "vector" );

            #endregion

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = vector.elements[i] + scalar;
            }

            return result;
        }

        #endregion

        #region -

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Vector from the the <paramref name="left"/> Vector.
        /// </summary>
        /// <param name="left">The value on the left side of the equation.</param>
        /// <param name="right">The value on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>    
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector operator -( Vector left, Vector right )
        {   
            #region Verify Arguments

            if( left == null )
                throw new ArgumentNullException( "left" );
            if( right == null )
                throw new ArgumentNullException( "right" );

            if( left.DimensionCount != right.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( left.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = left.elements[i] - right.elements[i];
            }

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the given <paramref name="scalar"/> from the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified Vectors is null.
        /// </exception>
        public static Vector operator -( Vector vector, float scalar )
        {
            #region Verify Arguments

            if( vector == null )
                throw new ArgumentNullException( "vector" );

            #endregion

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = vector.elements[i] - scalar;
            }

            return result;
        }

        /// <summary>
        /// Returns the result of negating the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified Vectors is null.
        /// </exception>
        public static Vector operator -( Vector vector )
        {           
            #region Verify Arguments

            if( vector == null )
                throw new ArgumentNullException( "vector" );

            #endregion

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = -vector.elements[i];
            }

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
        /// <exception cref="ArgumentNullException">
        /// If the specified Vectors is null.
        /// </exception>
        public static Vector operator *( Vector vector, float scalar )
        {
            #region Verify Arguments

            if( vector == null )
                throw new ArgumentNullException( "vector" );

            #endregion

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = vector.elements[i] * scalar;
            }

            return result;
        }

        /// <summary>
        /// Returns the result of multiplying the left Vector by the right Vector component-by-component.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>  
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector operator *( Vector left, Vector right )
        {
            #region Verify Arguments

            if( left == null )
                throw new ArgumentNullException( "left" );
            if( right == null )
                throw new ArgumentNullException( "right" );

            if( left.DimensionCount != right.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( left.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = left.elements[i] * right.elements[i];
            }

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
        /// <exception cref="ArgumentNullException">
        /// If the specified Vectors is null.
        /// </exception>
        public static Vector operator /( Vector vector, float scalar )
        {
            #region Verify Arguments

            if( vector == null )
                throw new ArgumentNullException( "vector" );

            #endregion

            Vector result = new Vector( vector.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = vector.elements[i] / scalar;
            }

            return result;
        }

        /// <summary>
        /// Returns the result of dividing the left Vector through the right Vector element-by-element.
        /// </summary>
        /// <param name="left">The vector on the left side of the equation.</param>
        /// <param name="right">The vector on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns> 
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Vectors is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified Vectors don't have the same number of elements.
        /// </exception>
        public static Vector operator /( Vector left, Vector right )
        {
            #region Verify Arguments

            if( left == null )
                throw new ArgumentNullException( "left" );
            if( right == null )
                throw new ArgumentNullException( "right" );

            if( left.DimensionCount != right.DimensionCount )
                throw new ArgumentException( MathErrorStrings.VectorLengthMismatch );

            #endregion

            Vector result = new Vector( left.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result.elements[i] = left.elements[i] / right.elements[i];
            }

            return result;
        }

        #endregion
        
        #region > Logic <

        /// <summary>
        /// Returns whether given <see cref="Vector"/> instances are equal.
        /// </summary>
        /// <param name="left">The Vector on the left side of the equation.</param>
        /// <param name="right">The Vector on the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==( Vector left, Vector right )
        {
            // If they are the same instances, or both null:
            if( Object.ReferenceEquals( left, right ) )
                return true;

            // If one of them is null:
            if( (object)left == null ) // || (object)right == null ) // Is checked in left.Equals( right )
                return false;

            return left.Equals( right );
        }

        /// <summary>
        /// Returns whether given <see cref="Vector"/> instances are not equal.
        /// </summary>
        /// <param name="left">The Vector on the left side of the equation.</param>
        /// <param name="right">The Vector on the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=( Vector left, Vector right )
        {
            return !(left == right);
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The elements of the Vector.
        /// </summary>
        private readonly float[] elements;

        #endregion
    }
}
