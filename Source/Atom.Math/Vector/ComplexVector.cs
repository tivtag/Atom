// <copyright file="ComplexVector.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.ComplexVector class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Defines a N-dimensional Vector whose elements are <see cref="Complex"/> numbers.
    /// </summary>
    [Serializable]
    public class ComplexVector : 
        IEnumerable<Complex>, IEquatable<ComplexVector>, 
        ISerializable, ICultureSensitiveToStringProvider
    {
        #region [ Properties ]

        /// <summary>
        /// Gets a value that represents the total
        /// number of elements the ComplexVector has.
        /// </summary>
        /// <value>The total number of elements the ComplexVector has.</value>
        public int DimensionCount
        {
            get 
            {
                // Contract.Ensures( Contract.Result<int>() > 0 );

                return this.elements.Length;
            }
        }

        /// <summary>
        /// Gets or sets the element of the ComplexVector at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to receive or set.
        /// </param>
        /// <returns>
        /// The element at the specified <paramref name="index"/>.
        /// </returns>
        public Complex this[int index]
        {
            get 
            {
                return this.elements[index];
            }

            set
            { 
                this.elements[index] = value;
            }
        }
        
        /// <summary>
        /// Gets the 1-Norm (also known as Manhattan Norm or Taxicab Norm) of this ComplexVector.
        /// </summary>
        /// <remarks>
        /// The 1-Norm is the sum of the lengths of the <see cref="Complex"/> of this ComplexVector.
        /// </remarks>
        /// <returns>
        /// The 1-Norm of this ComplexVector. ret = sum(abs(this[i]))
        /// </returns>
        public float OneNorm
        {
            get
            {
                float sum = 0.0f;

                for( int i = 0; i < this.elements.Length; ++i )
                {
                    sum += this.elements[i].Length;
                }

                return sum;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexVector"/> class.
        /// </summary>
        /// <param name="dimensionCount">
        /// The number of elements the new Vector can hold.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the specified <paramref name="dimensionCount"/> is less than or equal zero.
        /// </exception>
        public ComplexVector( int dimensionCount )
        {
            Contract.Requires<ArgumentException>( dimensionCount > 0 );

            this.elements = new Complex[dimensionCount];
        }        

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexVector"/> class.
        /// </summary>
        /// <param name="elements">
        /// The elements the new ComplexVector.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="elements"/> is null.
        /// </exception>
        public ComplexVector( IEnumerable<Complex> elements )
        {
            Contract.Requires<ArgumentNullException>( elements != null );
            Contract.Requires<ArgumentException>( elements.Count() > 0 );

            this.elements = elements.ToArray();
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="ComplexVector"/> class and
        /// sets the Elements of the new <see cref="ComplexVector"/> to the "Elements"
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
        protected ComplexVector( SerializationInfo info, StreamingContext context )
        {
            Contract.Requires<ArgumentNullException>( info != null );

            this.elements = (Complex[])info.GetValue( "Elements", typeof(Array) );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns the dot (scalar/inner) product of two given <see cref="ComplexVector"/>s.
        /// </summary> 
        /// <param name="left">
        /// The ComplexVector 'u' on the left side.
        /// </param>
        /// <param name="right">
        /// The ComplexVector 'v' on the right side.
        /// </param>
        /// <returns>
        /// The result of the operation; Scalar dot = sum(u[i] * v[i]).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="left"/> or <paramref name="right"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the dimension count of given ComplexVector doesn't match.
        /// </exception>
        public static Complex Dot( ComplexVector left, ComplexVector right )
        {
            VerifyDimensionsMatch( left, right );
            
            Complex dot = new Complex();

            for( int i = 0; i < left.DimensionCount; ++i )
            {
                dot += left[i] * right[i];
            }

            return dot;
        }

        /// <summary>
        /// Returns the dyadic product of two given <see cref="ComplexVector"/>s.
        /// </summary>
        /// <param name="left">
        /// The ComplexVector 'u' on the left side.
        /// </param>
        /// <param name="right">
        /// The ComplexVector 'v' on the right side.
        /// </param>
        /// <returns>
        /// The result of the operation; ComplexMatrix M[i,j] = u[i] * v[j].
        /// </returns>
        public static ComplexMatrix Dyadic( ComplexVector left, ComplexVector right )
        {
            int sizeU = left.DimensionCount;
            int sizeV = right.DimensionCount;

            var matrix = new ComplexMatrix( sizeU, sizeV );

            for( int i = 0; i < sizeU; ++i )
            {
                for( int j = 0; j < sizeV; ++j )
                {
                    matrix[i, j] = left[i] * right[j];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Returns the result of transforming the given ComplexVector by the given <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <param name="vector">
        /// The vector on the left side.
        /// </param>
        /// <param name="matrix">
        /// The matrix on the right side.
        /// </param>
        /// <returns>
        /// The transformed ComplexVector.
        /// </returns>
        public static ComplexVector Transform( ComplexVector vector, ComplexMatrix matrix )
        {
            Contract.Requires<ArgumentException>( 
                vector.DimensionCount == matrix.RowCount,
                MathErrorStrings.VectorSizeMatrixRowCountMismatch 
            );

            int size   = Math.Min( vector.DimensionCount, matrix.ColumnCount );
            var result = new ComplexVector( size );

            for( int column = 0; column < vector.DimensionCount; ++column )
            {
                Complex accum = Complex.Zero;

                for( int row = 0; row < size; ++row )
                {
                    accum += vector[column] * matrix[row, column];
                }

                result[column] = accum;
            }

            return result;
        }

        #region > Operators <

        /// <summary>
        /// Returns the result of adding the given ComplexVectors together.
        /// </summary>
        /// <param name="left">
        /// The ComplexVector on the left side.
        /// </param>
        /// <param name="right">
        /// The ComplexVector on the right side.
        /// </param>
        /// <returns>
        /// The result of the operation; ComplexMatrix M[i,j] = u[i] + v[j].
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="left"/> or <paramref name="right"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the dimension count of given ComplexVector doesn't match.
        /// </exception>
        public static ComplexVector operator +( ComplexVector left, ComplexVector right )
        {
            VerifyDimensionsMatch( left, right );

            var result = new ComplexVector( left.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result[i] = left[i] + right[i];
            }

            return result;
        }

        /// <summary>
        /// Returns the result of adding the given ComplexVectors together.
        /// </summary>
        /// <param name="left">
        /// The ComplexVector on the left side.
        /// </param>
        /// <param name="right">
        /// The ComplexVector on the right side.
        /// </param>
        /// <returns>
        /// The result of the operation; ComplexMatrix M[i,j] = u[i] + v[j].
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="left"/> or <paramref name="right"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the dimension count of given ComplexVector doesn't match.
        /// </exception>
        public static ComplexVector Add( ComplexVector left, ComplexVector right )
        {
            return left + right;
        }

        /// <summary>
        /// Returns the result of subtracting the <paramref name="left"/> ComplexVector
        /// from the <paramref name="right"/> ComplexVector.
        /// </summary>
        /// <param name="left">
        /// The ComplexVector on the left side.
        /// </param>
        /// <param name="right">
        /// The ComplexVector on the right side.
        /// </param>
        /// <returns>
        /// The result of the operation; ComplexMatrix M[i,j] = u[i] - v[j].
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="left"/> or <paramref name="right"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the dimension count of given ComplexVector doesn't match.
        /// </exception>
        public static ComplexVector operator -( ComplexVector left, ComplexVector right )
        {
            VerifyDimensionsMatch( left, right );

            var result = new ComplexVector( left.DimensionCount );

            for( int i = 0; i < result.DimensionCount; ++i )
            {
                result[i] = left[i] - right[i];
            }

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the <paramref name="left"/> ComplexVector
        /// from the <paramref name="right"/> ComplexVector.
        /// </summary>
        /// <param name="left">
        /// The ComplexVector on the left side.
        /// </param>
        /// <param name="right">
        /// The ComplexVector on the right side.
        /// </param>
        /// <returns>
        /// The result of the operation; ComplexMatrix M[i,j] = u[i] - v[j].
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="left"/> or <paramref name="right"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the dimension count of given ComplexVector doesn't match.
        /// </exception>
        public static ComplexVector Subtract( ComplexVector left, ComplexVector right )
        {
            return left - right;
        }

        /// <summary>
        /// Returns a value indicating whether the specified ComplexVectors are equal.
        /// </summary>
        /// <param name="left">
        /// The ComplexVector on the left side.
        /// </param>
        /// <param name="right">
        /// The ComplexVector on the right side.
        /// </param>
        /// <returns>
        /// true if they are equal;
        /// otherwise false.
        /// </returns>
        public static bool operator ==( ComplexVector left, ComplexVector right )
        {
            if( object.ReferenceEquals( left, right ) )
                return true;

            if( object.Equals( left, null ) )
                return false;

            if( object.Equals( right, null ) )
                return false;

            return left.Equals( right );
        }
        
        /// <summary>
        /// Returns a value indicating whether the specified ComplexVectors are not equal.
        /// </summary>
        /// <param name="left">
        /// The ComplexVector on the left side.
        /// </param>
        /// <param name="right">
        /// The ComplexVector on the right side.
        /// </param>
        /// <returns>
        /// true if they are not equal;
        /// otherwise false.
        /// </returns>
        public static bool operator !=( ComplexVector left, ComplexVector right )
        {
            return !(left == right);
        }

        #endregion

        #region > Helpers <

        /// <summary>
        /// Verifies that the given ComplexVectors are of the same size.
        /// </summary>
        /// <param name="left">
        /// The ComplexVector on the left side.
        /// </param>
        /// <param name="right">
        /// The ComplexVector on the right side.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="left"/> or <paramref name="right"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the dimension count of given ComplexVector doesn't match.
        /// </exception>
        private static void VerifyDimensionsMatch( ComplexVector left, ComplexVector right )
        {
            Contract.Requires<ArgumentNullException>( left != null );
            Contract.Requires<ArgumentNullException>( right != null );
            Contract.Requires<ArgumentException>( 
                left.DimensionCount == right.DimensionCount,
                MathErrorStrings.VectorLengthMismatch
            );
        }

        #endregion

        #region > Overrides/Impls <

        #region IEnumerable<float>

        /// <summary>
        /// Returns an <see cref="IEnumerator{Complex}"/>
        /// over the elements of the <see cref="ComplexVector"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{Complex}"/>
        /// over the elements of the <see cref="ComplexVector"/>.
        /// </returns>
        public IEnumerator<Complex> GetEnumerator()
        {
            for( int i = 0; i < this.elements.Length; ++i )
            {
                yield return this.elements[i];
            }
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator{Complex}"/>
        /// over the elements of the <see cref="ComplexVector"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{Complex}"/>
        /// over the elements of the <see cref="ComplexVector"/>.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region GetObjectData

        /// <summary>
        /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/>
        /// with the data needed to serialize the <see cref="ComplexVector"/>.
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
        /// Returns whether the given <see cref="ComplexVector"/> has the
        /// same indices set as this ComplexVector.
        /// </summary>
        /// <param name="other">The Vector to test against. Can be null.</param>
        /// <returns>true if the indices are equal; otherwise false.</returns>
        public bool Equals( ComplexVector other )
        {
            if( other == null )
                return false;

            if( this.DimensionCount != other.DimensionCount )
                return false;

            for( int i = 0; i < this.elements.Length; ++i )
            {
                if( !this.elements[i].Equals( other.elements[i] ) )
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns whether the given <see cref="Object"/> is equal to this ComplexVector.
        /// </summary>
        /// <param name="obj">The Object to test against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public override bool Equals( object obj )
        {
            ComplexVector vector = obj as ComplexVector;

            if( vector != null )
                return this.Equals( vector );

            return false;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overriden to return a human-readable text representation of the ComplexVector.
        /// </summary>
        /// <returns>A human-readable text representation of the ComplexVector.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the ComplexVector.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the ComplexVector.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder( this.DimensionCount * 20 );

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
            int hash = this.DimensionCount;

            for( int i = 0; i < elements.Length; ++i )
                hash += elements[i].GetHashCode();

            return hash;
        }

        #endregion

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The elements of the Vector.
        /// </summary>
        private readonly Complex[] elements;

        #endregion
    }
}
