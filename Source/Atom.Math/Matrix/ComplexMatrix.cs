// <copyright file="ComplexMatrix.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.ComplexMatrix class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Collections;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a NxM matrix whose elements are <see cref="Complex"/> numbers.
    /// </summary>
    [Serializable]
    public class ComplexMatrix : ObjectMatrix<Complex>, 
        ICultureSensitiveToStringProvider, ICloneable
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the non-conjugated transpose of this <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <remarks>
        /// The transpose of a Matrix is the same Matrix but with exchanged rows and columns.
        /// </remarks>
        /// <value>The transposed ComplexMatrix.</value>
        public ComplexMatrix Transpose
        {
            get
            {
                ComplexMatrix transpose = new ComplexMatrix( this.ColumnCount, this.RowCount );

                for( int i = 0; i < this.RowCount; ++i )
                {
                    for( int j = 0; j < this.ColumnCount; ++j )
                    {
                        transpose[j, i] = this[i, j];
                    }
                }

                return transpose;
            }
        }
               
        /// <summary>
        /// Gets the conjugated transpose of this <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <remarks>
        /// The transpose of a Matrix is the same Matrix but with exchanged rows and columns.
        /// </remarks>
        /// <value>The conjugated transposed ComplexMatrix.</value>
        public ComplexMatrix HermitianTranspose
        {
            get
            {
                ComplexMatrix transpose = new ComplexMatrix( this.ColumnCount, this.RowCount );

                for( int i = 0; i < this.RowCount; ++i )
                {
                    for( int j = 0; j < this.ColumnCount; ++j )
                    {
                        transpose[j, i] = this[i, j].Conjugate;
                    }
                }

                return transpose;
            }
        }        

        #region > Norms <

        /// <summary>
        /// Gets the 1-norm of this <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <value>
        /// The greatest absolute column length of the ComplexMatrix.
        /// </value>
        public float ManhattanNorm
        {
            get
            {
                float greatestColumnNorm = 0.0f;

                for( int j = 0; j < this.ColumnCount; ++j )
                {
                    float columnNorm = 0.0f;

                    for( int i = 0; i < this.RowCount; ++i )
                    {
                        columnNorm += this[i, j].Length;
                    }

                    if( columnNorm > greatestColumnNorm )
                        greatestColumnNorm = columnNorm;
                }

                return greatestColumnNorm;
            }
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexMatrix"/> class.
        /// </summary>
        /// <param name="rowCount">
        /// The number of rows the new ComplexMatrix should have.
        /// </param>
        /// <param name="columnCount">
        /// The number of columns the new ComplexMatrix should have.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="rowCount"/> or <paramref name="columnCount"/> is less than or equal 0.
        /// </exception>
        public ComplexMatrix( int rowCount, int columnCount )
            : base( rowCount, columnCount )
        {
        }        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexMatrix"/> class.
        /// </summary>
        /// <param name="elements">
        /// The elements of the new ComplexMatrix.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="elements"/> is null.
        /// </exception>
        public ComplexMatrix( Complex[,] elements )
            : base( elements )
        {
        }
        
        #region ComplexMatrix( SerializationInfo info, StreamingContext context )

        /// <summary> 
        /// Initializes a new instance of the <see cref="ComplexMatrix"/> class; and
        /// sets values of the new <see cref="ComplexMatrix"/> to the
        /// values specified by the <see cref="System.Runtime.Serialization.SerializationInfo"/>.
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
        protected ComplexMatrix( 
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
            : base( info, context )
        {
        }

        #endregion

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns the kronecker product of the given complex matrices.
        /// </summary>
        /// <param name="matrixA">The matrix on the left side. May not be null.</param>
        /// <param name="matrixB">The matrix on the right side. May not be null.</param>
        /// <returns>The result of the operation.</returns>
        public static ComplexMatrix Kronecker( ComplexMatrix matrixA, ComplexMatrix matrixB )
        {
            Contract.Requires<ArgumentNullException>( matrixA != null );
            Contract.Requires<ArgumentNullException>( matrixB != null );
            // Contract.Ensures( Contract.Result<ComplexMatrix>() != null );
            // Contract.Ensures( Contract.Result<ComplexMatrix>().RowCount == (matrixA.RowCount * matrixB.RowCount) );
            // Contract.Ensures( Contract.Result<ComplexMatrix>().ColumnCount == (matrixA.ColumnCount * matrixB.ColumnCount) );

            var result = new ComplexMatrix( 
                matrixA.RowCount * matrixB.RowCount,
                matrixA.ColumnCount * matrixB.ColumnCount
            );            

            for( int row = 0; row < matrixA.RowCount; ++row )
            {
                int rowOffset = row * matrixB.RowCount;
                
                for( int column = 0; column < matrixA.ColumnCount; ++column )
                {
                    int columnOffset = column * matrixB.ColumnCount;

                    // Scale and apply sub-matrix:
                    var partMat = matrixB * matrixA[row, column];

                    result.SetSubMatrix(
                        rowOffset,
                        columnOffset,
                        matrixB.RowCount - 1,
                        matrixB.RowCount - 1,
                        partMat
                    );
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the result of scaling the given <see cref="ComplexMatrix"/> by the given <see cref="Complex"/> scalar.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <param name="scalar">The input scalar value.</param>
        /// <returns>The result of the operation.</returns>
        public static ComplexMatrix operator *( ComplexMatrix matrix, Complex scalar )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );
            Contract.Requires<ArgumentNullException>( scalar != null );
            // Contract.Ensures( Contract.Result<ComplexMatrix>() != null );
            // Contract.Ensures( Contract.Result<ComplexMatrix>().RowCount == matrix.RowCount );
            // Contract.Ensures( Contract.Result<ComplexMatrix>().ColumnCount == matrix.ColumnCount );

            var result = new ComplexMatrix( matrix.RowCount, matrix.ColumnCount );

            for( int row = 0; row < matrix.RowCount; ++row )
            {
                for( int column = 0; column < matrix.ColumnCount; ++column )
                {
                    result[row, column] = matrix[row, column] * scalar;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the result scaling the given <see cref="ComplexMatrix"/> by the given <see cref="Complex"/> scalar.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <param name="scalar">The input scalar value.</param>
        /// <returns>The result of the operation.</returns>
        public static ComplexMatrix Multiply( ComplexMatrix matrix, Complex scalar )
        {
            return matrix * scalar;
        }
        
        #region > Get <

        /// <summary>
        /// Gets the row at the specified zero-based index.
        /// </summary>
        /// <param name="row">
        /// The zero-based index of the row to get.
        /// </param>
        /// <returns>
        /// A ComplexVector containing the values of the requested row.
        /// </returns>
        [Pure]
        public new ComplexVector GetRow( int row )
        {
            Contract.Requires<ArgumentException>( row >= 0 );
            Contract.Requires<ArgumentException>( row < this.RowCount );
            // Contract.Ensures( Contract.Result<ComplexVector>() != null );
            // Contract.Ensures( Contract.Result<ComplexVector>().DimensionCount == this.ColumnCount );

            ComplexVector rowVector = new ComplexVector( this.ColumnCount );

            for( int i = 0; i < this.ColumnCount; ++i )
            {
                rowVector[i] = this[row, i];
            }

            return rowVector;
        }

        /// <summary>
        /// Gets the column at the specified zero-based index.
        /// </summary>
        /// <param name="column">
        /// The zero-based index of the column to get.
        /// </param>
        /// <returns>
        /// A ComplexVector containing the values of the requested column.
        /// </returns>
        [Pure]
        public new ComplexVector GetColumn( int column )
        {
            Contract.Requires<ArgumentException>( column >= 0 );
            Contract.Requires<ArgumentException>( column < this.ColumnCount );
            // Contract.Ensures( Contract.Result<ComplexVector>() != null );
            // Contract.Ensures( Contract.Result<ComplexVector>().DimensionCount == this.RowCount );

            ComplexVector columnVector = new ComplexVector( this.RowCount );

            for( int i = 0; i < this.RowCount; ++i )
            {
                columnVector[i] = this[i, column];
            }

            return columnVector;
        }

        /// <summary>
        /// Gets a sub matrix of this <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <param name="rowStart">
        /// The row start index.
        /// </param>
        /// <param name="columnStart">
        /// The column start index.
        /// </param>
        /// <param name="rowCount">
        /// The number of rows to get.
        /// </param>
        /// <param name="columnCount">
        /// The number of columns to get.
        /// </param>
        /// <returns>
        /// The sub matrix of the current matrix.
        /// </returns>
        [Pure]
        public new ComplexMatrix GetSubMatrix( int rowStart, int columnStart, int rowCount, int columnCount )
        {
            Contract.Requires<ArgumentException>( rowStart >= 0 );
            Contract.Requires<ArgumentException>( rowStart < this.RowCount );
            Contract.Requires<ArgumentException>( columnStart >= 0 );
            Contract.Requires<ArgumentException>( columnStart < this.ColumnCount );

            Contract.Requires<ArgumentException>( rowCount > 0 );
            Contract.Requires<ArgumentException>( columnCount > 0 );
            Contract.Requires<ArgumentException>( (rowStart + rowCount) <= this.RowCount );
            Contract.Requires<ArgumentException>( (columnStart + columnCount) <= this.ColumnCount );

            // Contract.Ensures( Contract.Result<ComplexMatrix>() != null );
            // Contract.Ensures( Contract.Result<ComplexMatrix>().RowCount == rowCount );
            // Contract.Ensures( Contract.Result<ComplexMatrix>().ColumnCount == columnCount );

            ComplexMatrix subMatrix = new ComplexMatrix( rowCount, columnCount );

            int rowEnd = rowStart + rowCount;
            int columnEnd = columnStart + columnCount;

            for( int i = rowStart; i < rowEnd; ++i )
            {
                for( int j = columnStart; j < columnEnd; ++j )
                {
                    subMatrix[i - rowStart, j - columnStart] = this[i, j];
                }
            }

            return subMatrix;
        }

        #endregion

        #region > Impls/Overrides <

        #region Equals

        /// <summary>
        /// Returns whether the specified <see cref="Object"/>
        /// is equal to this <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test against. Can be null.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified <see cref="Object"/> is equal to this <see cref="ComplexMatrix"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            return this.Equals( obj as ComplexMatrix );
        }

        /// <summary>
        /// Returns whether the specified <see cref="ComplexMatrix"/>
        /// is equal to this <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <param name="other">
        /// The <see cref="ComplexMatrix"/> to test against. Can be null.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified <see cref="ComplexMatrix"/> is equal to this <see cref="ComplexMatrix"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( ComplexMatrix other )
        {
            if( other == null )
                return false;

            if( this.RowCount != other.RowCount || this.ColumnCount != other.ColumnCount )
                return false;

            for( int row = 0; row < RowCount; ++row )
            {
                for( int column = 0; column < ColumnCount; ++column )
                {
                    if( this[row, column] != other[row, column] )
                        return false;
                }
            }

            return true;
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Returns the hash code of this <see cref="Matrix"/> instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            // overriden to make the compiler to quite :-)
            return base.GetHashCode();
        }

        #endregion

        #region Clone

        /// <summary>
        /// Returns a clone of this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>
        /// A clone of this Matrix.
        /// </returns>
        public new ComplexMatrix Clone()
        {
            ComplexMatrix clone = new ComplexMatrix( this.RowCount, this.ColumnCount );

            for( int x = 0; x < RowCount; ++x )
            {
                for( int y = 0; y < ColumnCount; ++y )
                {
                    clone[x, y] = this[x, y];
                }
            }

            return clone;
        }

        /// <summary>
        /// Returns a clone of this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>
        /// A clone of this Matrix.
        /// </returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region GetObjectData

        /// <summary>
        /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/>
        /// with the data needed to serialize the <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/>
        /// to populate with data.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// If the given info is null.
        /// </exception>
        /// <param name="context"> 
        /// The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>)
        /// for this serialization. Can be null.
        /// </param>
        [System.Security.Permissions.SecurityPermissionAttribute(
            System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true )]
        public override void GetObjectData(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        {
            base.GetObjectData( info, context );

            info.SetType( typeof( ComplexMatrix ) );
        }

        #endregion
        
        #region ToString

        /// <summary>
        /// Returns a humen-readable representation of the <see cref="ObjectMatrix{T}"/>.
        /// </summary>
        /// <returns>A string representing this ObjectMatrix.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a humen-readable representation of the <see cref="ObjectMatrix{T}"/>.
        /// </summary>
        /// <param name="formatProvider">
        /// Provides access to culture-sensitive formatting information.
        /// </param>
        /// <returns>A string representing this ObjectMatrix.</returns>
        public string ToString( IFormatProvider formatProvider )
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder( RowCount * ColumnCount * 5 );

            for( int row = 0; row < RowCount; ++row )
            {
                sb.Append( '|' );
                for( int column = 0; column < ColumnCount; ++column )
                {
                    sb.Append( this[row, column].ToString( formatProvider ) );

                    if( column != (ColumnCount - 1) )
                        sb.Append( ' ' );
                }
                sb.Append( '|' );

                sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion

        #endregion

        #endregion
    }
}
