// <copyright file="Matrix.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Matrix class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Threading;
    using Atom.Diagnostics.Contracts;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a N x M single-precission floating point Matrix.
    /// </summary>
    [Serializable]
    public class Matrix : Atom.Collections.ObjectMatrix<float>, 
        IEquatable<Matrix>, ICloneable, ICultureSensitiveToStringProvider
    {
        #region [ Properties ]

        #region Transpose

        /// <summary>
        /// Gets the transpose of this <see cref="Matrix"/>.
        /// </summary>
        /// <remarks>
        /// The transpose of a Matrix is the same Matrix but with exchanged rows and columns.
        /// </remarks>
        /// <value>The transposed Matrix.</value>
        public Matrix Transpose
        {
            get
            {
                Matrix transpose = new Matrix( this.ColumnCount, this.RowCount );

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

        #endregion

        #region Inverse

        /// <summary>
        /// Gets the inverse of this <see cref="Matrix"/>.
        /// </summary>
        /// <value>
        /// The inverse of a matrix A is called A^-1
        /// and can be used to 'undo' the the matrix:
        /// A * A^-1 = I (Identity).
        /// </value>
        public Matrix Inverse
        {
            get
            {
                return Solve( this, Identity( this.RowCount, this.RowCount ) );
            }
        }

        #endregion

        #region Determinant

        /// <summary>
        /// Gets the determinant of this <see cref="Matrix"/>.
        /// </summary>
        /// <value>If the determinant of a matrix is zero then the matrix is not invertable.</value>
        public float Determinant
        {
            get
            {
                return new LUDecomposition( this ).Determinant;
            }
        }

        #endregion

        #region Trace

        /// <summary>
        /// Gets the trace of this <see cref="Matrix"/>,
        /// which is the sum of the diagonal elements of the Matrix.
        /// </summary>
        /// <value>The sum of the diagonal elements of the matrix.</value>
        public float Trace
        {
            get
            {
                float trace = 0;

                int diagonalSize = System.Math.Min( this.RowCount, this.ColumnCount );

                for( int i = 0; i < diagonalSize; ++i )
                {
                    trace += this[i, i];
                }

                return trace;
            }
        }

        #endregion

        #region Rank

        /// <summary> 
        /// Gets the effective numerical rank of this <see cref="Matrix"/>.
        /// </summary>
        /// <remarks>
        /// This property re-calculates the <see cref="SingularValueDecomposition"/> of this Matrix,
        /// consider caching the decomposition or the value.
        /// </remarks>
        /// <value>
        /// The effective numerical rank, obtained from <see cref="SingularValueDecomposition"/>.
        /// </value>
        public int Rank
        {
            get
            {
                return new SingularValueDecomposition( this ).Rank;
            }
        }

        #endregion

        #region IsSymmetric

        /// <summary>
        /// Gets a value indicating whether this <see cref="Matrix"/> is symmetric.
        /// </summary>
        /// <remarks>
        /// A symmetric matrix is a square matrix, that is equal to its transpose.
        /// </remarks>
        /// <value>
        /// Returns <see langword="true"/> if this Matrix is symmetric;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsSymmetric
        {
            get
            {
                if( this.IsSquare )
                {
                    for( int i = 0; i < RowCount; ++i )
                    {
                        for( int j = 0; j < i; ++j )
                        {
                            if( this[i, j] != this[j, i] )
                                return false;
                        }
                    }

                    return true;
                }

                return false;
            }
        }

        #endregion

        #region IsDiagonal

        /// <summary>
        /// Gets a value indicating whether this <see cref="Matrix"/> is diagonal.
        /// </summary>
        /// <remarks>
        /// A diagonal matrix is a square matrix in which 
        /// the entries outside the main diagonal (↘) are all zero.
        /// </remarks>
        /// <value>
        /// Returns <see langword="true"/> if the Matrix is diagonal; otherwise <see langword="false"/>.
        /// </value>
        public bool IsDiagonal
        {
            get
            {
                if( this.IsSquare )
                {
                    for( int i = 0; i < RowCount; ++i )
                    {
                        for( int j = 0; j < i; ++j )
                        {
                            if( (this[i, j] != 0) || (this[j, i] != 0) )
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }

                return false;
            }
        }

        #endregion

        #region IsSingular

        /// <summary>
        /// Gets a value indicating whether this <see cref="Matrix"/> is singular.
        /// </summary>
        /// <remarks>
        /// A square matrix is singular or degenerated
        /// if its not invertable. (The <see cref="Determinant"/> is zero.)
        /// </remarks>
        /// <value>
        /// Returns <see langword="true"/> if the Matrix is singular; otherwise <see langword="false"/>.
        /// </value>
        public bool IsSingular
        {
            get
            {
                return this.Determinant == 0;
            }
        }

        #endregion

        #region IsTriangular

        /// <summary>
        /// Gets a value indicating whether this <see cref="Matrix"/> is triangular.
        /// </summary>
        /// <value>
        /// A <see cref="TriangularMatrixType"/> enum value indicating whether 
        /// the matrix is UpperTriangular, LowerTriangular, Diagonal or not.
        /// </value>      
        public TriangularMatrixType IsTriangular
        {
            get
            {
                if( this.IsSquare )
                {
                    bool hasUpper = true;
                    bool hasLower = true;

                    for( int i = 0; i < RowCount; ++i )
                    {
                        for( int j = 0; j < i; ++j )
                        {
                            if( this[i, j] != 0 )
                                hasUpper = false;

                            if( this[j, i] != 0 )
                                hasLower = false;
                        }
                    }

                    if( hasUpper )
                    {
                        return hasLower ? TriangularMatrixType.Diagonal : TriangularMatrixType.Upper;
                    }
                    else
                    {
                        return hasLower ? TriangularMatrixType.Lower : TriangularMatrixType.None;
                    }
                }

                return TriangularMatrixType.None;
            }
        }

        #endregion

        #region > Norms <

        #region Condition

        /// <summary>
        /// Gets the 2-norm condition number of this Matrixs;
        /// which is the ratio of largest to smallest singular value.
        /// </summary>
        /// <remarks>
        /// This property re-calculates the <see cref="SingularValueDecomposition"/> of this Matrix,
        /// consider caching the decomposition or the value.
        /// </remarks>
        /// <value>The ratio of largest to smallest singular value.</value>
        public float Condition
        {
            get
            {
                return new SingularValueDecomposition( this ).Condition;
            }
        }

        #endregion

        #region ManhattanNorm

        /// <summary>
        /// Gets the 1-norm of this <see cref="Matrix"/>,
        /// which is the greatest absolute column sum of the Matrix.
        /// </summary>
        /// <value>
        /// The greatest absolute column sum of the Matrix.
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
                        columnNorm += System.Math.Abs( this[i, j] );
                    }

                    if( columnNorm > greatestColumnNorm )
                        greatestColumnNorm = columnNorm;
                }

                return greatestColumnNorm;
            }
        }

        #endregion

        #region InfinityNorm

        /// <summary>
        /// Gets the infinity norm of this <see cref="Matrix"/>,
        /// which is the greatest absolute row sum of the Matrix.
        /// </summary>
        /// <value>
        /// The greatest absolute row sum of the Matrix.
        /// </value>
        public float InfinityNorm
        {
            get
            {
                float greatestRowNorm = 0;

                for( int i = 0; i < RowCount; ++i )
                {
                    float rowNorm = 0;

                    for( int j = 0; j < ColumnCount; ++j )
                    {
                        rowNorm += System.Math.Abs( this[i, j] );
                    }

                    if( greatestRowNorm < rowNorm )
                        greatestRowNorm = rowNorm;
                }

                return greatestRowNorm;
            }
        }

        #endregion

        #region FrobeniusNorm

        /// <summary>
        /// Gets the Frobenius Norm of this <see cref="Matrix"/>,
        /// which is the square root of sum of squares of all elements.
        /// </summary>
        /// <value>The square root of sum of squares of all elements.</value>
        public double FrobeniusNorm
        {
            get
            {
                float result = 0;

                for( int i = 0; i < RowCount; ++i )
                {
                    for( int j = 0; j < ColumnCount; ++j )
                    {
                        result =  MathUtilities.Hypotenuse( result, this[i, j] );
                    }
                }

                return result;
            }
        }

        #endregion

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="rowCount">
        /// The number of rows the new Matrix should have.
        /// </param>
        /// <param name="columnCount">
        /// The number of columns the new Matrix should have.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="rowCount"/> or <paramref name="columnCount"/> is less than or equal 0.
        /// </exception>
        public Matrix( int rowCount, int columnCount )
            : base( rowCount, columnCount )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class,
        /// cloning the given 2x2 <see cref="Matrix2"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix to clone.
        /// </param>
        public Matrix( Matrix2 matrix )
            : base( 2, 2 )
        {
            this[0, 0] = matrix.M11;
            this[0, 1] = matrix.M12;
            this[1, 0] = matrix.M21;
            this[1, 1] = matrix.M22;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class,
        /// cloning the given 4x4 <see cref="Matrix4"/>.
        /// </summary>
        /// <param name="matrix">
        /// The matrix to clone.
        /// </param>
        public Matrix( Matrix4 matrix )
            : base( 4, 4 )
        {
            this[0, 0] = matrix.M11;
            this[1, 0] = matrix.M21;
            this[2, 0] = matrix.M31;
            this[3, 0] = matrix.M41;

            this[0, 1] = matrix.M12;
            this[1, 1] = matrix.M22;
            this[2, 1] = matrix.M32;
            this[3, 1] = matrix.M41;

            this[0, 2] = matrix.M13;
            this[1, 2] = matrix.M23;
            this[2, 2] = matrix.M33;
            this[3, 2] = matrix.M43;

            this[0, 3] = matrix.M14;
            this[1, 3] = matrix.M24;
            this[2, 3] = matrix.M34;
            this[3, 3] = matrix.M44;
        }                 
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="elements">
        /// The elements of the new Matrix.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="elements"/> is null.
        /// </exception>
        public Matrix( float[,] elements )
            : base( elements )
        {
        }

        #region Matrix( SerializationInfo info, StreamingContext context )

        /// <summary> 
        /// Initializes a new instance of the <see cref="Matrix"/> class; and
        /// sets values of the new <see cref="Matrix"/> to the
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
        protected Matrix( 
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
            : base( info, context )
        {
        }

        #endregion

        #endregion

        #region [ Methods ]
        
        /// <summary>
        /// Calculates the LHS solution point if the matrix is square or the least squares solution otherwise.
        /// </summary>
        /// <param name="left">The Matrix on the left side.</param>
        /// <param name="right">The Matrix on the right side.</param>
        /// <returns>Returns the LHS solution point if the matrix is square or the least squares solution otherwise.</returns>
        public static Matrix Solve( Matrix left, Matrix right )
        {
            if( left.IsSquare )
            {
                return new LUDecomposition( left ).Solve( right );
            }
            else
            {
                return new QRDecomposition( left ).Solve( right );
            }
        }
        
        /// <summary>
        /// Concatenates two matrices in horizontal manner.
        /// </summary>
        /// <param name="left">The matrix on the left side of the equation.</param>
        /// <param name="right">The matrix on the right side of the equation.</param>
        /// <returns>The result of the concatenate operation.</returns>
        public static Matrix Concatenate( Matrix left, Matrix right )
        {
            Contract.Requires<ArgumentNullException>( left != null );
            Contract.Requires<ArgumentNullException>( right != null );
            Contract.Requires<ArgumentException>( left.RowCount == right.RowCount, MathErrorStrings.MatrixRowCountMismatch );

            // Contract.Ensures( Contract.Result<Matrix>() != null );
            // Contract.Ensures( Contract.Result<Matrix>().RowCount == left.RowCount );
            // Contract.Ensures( Contract.Result<Matrix>().ColumnCount == (left.ColumnCount + right.ColumnCount) );

            Matrix result = new Matrix(
                left.RowCount,
                left.ColumnCount + right.ColumnCount
            );

            // Copy the left hand matrix into the new matrix
            for( int i = 0; i < left.RowCount; ++i )
            {
                for( int j = 0; j < left.ColumnCount; ++j )
                {
                    result[i, j] = left[i, j];
                }
            }

            // Copy right hand matrix into the new matrix
            for( int i = 0; i < right.RowCount; i++ )
            {
                for( int j = 0; j < right.ColumnCount; j++ )
                {
                    result[i, j + left.ColumnCount] = right[i, j];
                }
            }

            return result;
        }

        #region Get

        /// <summary>
        /// Gets the row at the specified zero-based index.
        /// </summary>
        /// <param name="row">
        /// The zero-based index of the row to get.
        /// </param>
        /// <returns>
        /// A Vector containing the values of the requested row.
        /// </returns>
        [Pure]
        public new Vector GetRow( int row )
        {
            Contract.Requires<ArgumentException>( row >= 0 );
            Contract.Requires<ArgumentException>( row < this.RowCount );
            // Contract.Ensures( Contract.Result<Vector>() != null );
            // Contract.Ensures( Contract.Result<Vector>().Length == this.ColumnCount );

            Vector rowVector = new Vector( this.ColumnCount );

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
        /// A Vector containing the values of the requested column.
        /// </returns>
        [Pure]
        public new Vector GetColumn( int column )
        {
            Contract.Requires<ArgumentException>( column >= 0 );
            Contract.Requires<ArgumentException>( column < this.ColumnCount );
            // Contract.Ensures( Contract.Result<Vector>() != null );
            // Contract.Ensures( Contract.Result<Vector>().Length == this.RowCount );

            Vector columnVector = new Vector( this.RowCount );

            for( int i = 0; i < this.RowCount; ++i )
            {
                columnVector[i] = this[i, column];
            }

            return columnVector;
        }

        /// <summary>
        /// Gets a sub matrix of this <see cref="Matrix"/>.
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
        public new Matrix GetSubMatrix( int rowStart, int columnStart, int rowCount, int columnCount )
        {
            Contract.Requires<ArgumentException>( rowStart >= 0 );
            Contract.Requires<ArgumentException>( rowStart < this.RowCount );
            Contract.Requires<ArgumentException>( columnStart >= 0 );
            Contract.Requires<ArgumentException>( columnStart < this.ColumnCount );

            Contract.Requires<ArgumentException>( rowCount > 0 );
            Contract.Requires<ArgumentException>( columnCount > 0 );
            Contract.Requires<ArgumentException>( (rowStart + rowCount) <= this.RowCount );
            Contract.Requires<ArgumentException>( (columnStart + columnCount) <= this.ColumnCount );

            // Contract.Ensures( Contract.Result<Matrix>() != null );
            // Contract.Ensures( Contract.Result<Matrix>().RowCount == rowCount );
            // Contract.Ensures( Contract.Result<Matrix>().ColumnCount == columnCount );

            Matrix subMatrix = new Matrix( rowCount, columnCount );

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

        #region > Operators <

        #region Multiply

        #region Multiply( Matrix left, Matrix right )

        /// <summary>
        /// Returns the result of multiplying two matrices.
        /// </summary>
        /// <param name="left">The matrix on the left side of the equation.</param>
        /// <param name="right">The matrix on the right side of the equation.</param>
        /// <returns>The result of the times operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="left"/> or <paramref name="right"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the <see cref="Atom.Collections.ObjectMatrix{T}.ColumnCount"/> of the left Matrix is
        /// not equal the <see cref="Atom.Collections.ObjectMatrix{T}.RowCount"/> of the right Matrix.
        /// </exception>
        public static Matrix Multiply( Matrix left, Matrix right )
        {
            Contract.Requires<ArgumentNullException>( left != null );
            Contract.Requires<ArgumentNullException>( right != null );
            Contract.Requires<ArgumentException>( left.ColumnCount == right.RowCount, MathErrorStrings.IncompatibleMatricesTimes );
            // Contract.Ensures( Contract.Result<Matrix>() != null );
            // Contract.Ensures( Contract.Result<Matrix>().RowCount == left.RowCount );
            // Contract.Ensures( Contract.Result<Matrix>().ColumnCount == right.ColumnCount );

            Matrix result = new Matrix( left.RowCount, right.ColumnCount );

            for( int i = 0; i < left.RowCount; i++ )
            {
                for( int j = 0; j < right.ColumnCount; ++j )
                {
                    float sum = 0.0f;

                    for( int k = 0; k < left.ColumnCount; ++k )
                    {
                        sum += left[i, k] * right[k, j];
                    }

                    result[i, j] = sum; // result[i,j] = (row i of left) DOT (column j of right);
                }
            }

            return result;
        }

        #endregion

        #region Multiply( Matrix matrix, float scalar )

        /// <summary>
        /// Multiplies the given <see cref="Matrix"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static Matrix Multiply( Matrix matrix, float scalar )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );
            // Contract.Ensures( Contract.Result<Matrix>() != null );
            // Contract.Ensures( Contract.Result<Matrix>().RowCount == matrix.RowCount );
            // Contract.Ensures( Contract.Result<Matrix>().ColumnCount == matrix.ColumnCount );

            Matrix result = new Matrix( matrix.RowCount, matrix.ColumnCount );

            for( int row = 0; row < matrix.RowCount; ++row )
            {
                for( int column = 0; column < matrix.ColumnCount; ++column )
                {
                    result[row, column] = matrix[row, column] * scalar;
                }
            }

            return result;
        }

        #endregion

        #region MultiplyParallel( Matrix left, Matrix right )

        /// <summary>
        /// Returns the result of multiplying two matrices.
        /// This operation tries to use parallism to calcualte the result.
        /// </summary>
        /// <param name="left">The matrix on the left side of the equation.</param>
        /// <param name="right">The matrix on the right side of the equation.</param>
        /// <returns>The result of the times operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="left"/> or <paramref name="right"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the <see cref="Atom.Collections.ObjectMatrix{T}.ColumnCount"/> of the left Matrix is
        /// not equal the <see cref="Atom.Collections.ObjectMatrix{T}.RowCount"/> of the right Matrix.
        /// </exception>
        public static Matrix MultiplyParallel( Matrix left, Matrix right )
        {
            Contract.Requires<ArgumentNullException>( left != null );
            Contract.Requires<ArgumentNullException>( right != null );
            Contract.Requires<ArgumentException>( left.ColumnCount == right.RowCount, MathErrorStrings.IncompatibleMatricesTimes );

            Matrix result = new Matrix( left.RowCount, right.ColumnCount );

            Parallel.For( 0, left.RowCount, ( int i ) => {
                for( int j = 0; j < right.ColumnCount; ++j )
                {
                    float sum = 0.0f;

                    for( int k = 0; k < left.ColumnCount; ++k )
                    {
                        sum += left[i, k] * right[k, j];
                    }

                    result[i, j] = sum;
                }
            });

            return result;
        }

        #endregion

        #endregion

        #region Add

        /// <summary>
        /// Returns the result of adding 
        /// the <paramref name="right"/> <see cref="Matrix"/> to
        /// the <paramref name="left"/> <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>    
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static Matrix Add( Matrix left, Matrix right )
        {
            Contract.Requires<ArgumentNullException>( left != null );
            Contract.Requires<ArgumentNullException>( right != null );
            Contract.Requires<ArgumentException>( left.RowCount == right.RowCount, MathErrorStrings.IncompatibleMatricesTimes );
            Contract.Requires<ArgumentException>( left.ColumnCount == right.ColumnCount, MathErrorStrings.IncompatibleMatricesTimes );

            // Contract.Ensures( Contract.Result<Matrix>() != null );
            // Contract.Ensures( Contract.Result<Matrix>().RowCount == left.RowCount );
            // Contract.Ensures( Contract.Result<Matrix>().ColumnCount == left.ColumnCount );

            Matrix result = new Matrix( left.RowCount, left.ColumnCount );

            for( int row = 0; row < left.RowCount; ++row )
            {
                for( int column = 0; column < left.ColumnCount; ++column )
                {
                    result[row, column] = left[row, column] + right[row, column];
                }
            }

            return result;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Returns the result of subtracting 
        /// the <paramref name="right"/> <see cref="Matrix"/> from
        /// the <paramref name="left"/> <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>    
        /// <returns>
        /// The result of the operation.
        /// </returns>     
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Matrices is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the dimensions of the specified Matrices is not equal.
        /// </exception>
        public static Matrix Subtract( Matrix left, Matrix right )
        {
            Contract.Requires<ArgumentNullException>( left != null );
            Contract.Requires<ArgumentNullException>( right != null );
            Contract.Requires<ArgumentException>( left.RowCount == right.RowCount, MathErrorStrings.IncompatibleMatricesTimes );
            Contract.Requires<ArgumentException>( left.ColumnCount == right.ColumnCount, MathErrorStrings.IncompatibleMatricesTimes );

            // Contract.Ensures( Contract.Result<Matrix>() != null );
            // Contract.Ensures( Contract.Result<Matrix>().RowCount == left.RowCount );
            // Contract.Ensures( Contract.Result<Matrix>().ColumnCount == left.ColumnCount );

            Matrix result = new Matrix( left.RowCount, left.ColumnCount );

            for( int row = 0; row < left.RowCount; ++row )
                for( int column = 0; column < left.ColumnCount; ++column )
                    result[row, column] = left[row, column] - right[row, column];

            return result;
        }

        #endregion

        #region Negate

        /// <summary>
        /// Returns the result of negating the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">
        /// The Matrix to negate.
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static Matrix Negate( Matrix matrix )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );
            // Contract.Ensures( Contract.Result<Matrix>() != null );
            // Contract.Ensures( Contract.Result<Matrix>().RowCount == matrix.RowCount );
            // Contract.Ensures( Contract.Result<Matrix>().ColumnCount == matrix.ColumnCount );

            Matrix result = new Matrix( matrix.RowCount, matrix.ColumnCount );

            for( int row = 0; row < matrix.RowCount; ++row )
                for( int column = 0; column < matrix.ColumnCount; ++column )
                    result[row, column] = -matrix[row, column];

            return result;
        }

        #endregion

        #endregion

        #region > Creation Helpers <

        /// <summary>
        /// Constructs an identity <see cref="Matrix"/> of the specified size.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <returns>An identity matrix of the specified size.</returns>
        public static Matrix Identity( int rowCount, int columnCount )
        {
            return Diagonal( rowCount, columnCount, 1 );
        }
        
        /// <summary>
        /// Constructs a diagonal <see cref="Matrix"/> of the specified size with the specified value.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="value">The value of diagonal elements.</param>
        /// <returns>A diagonal matrix of the specified size.</returns>
        public static Matrix Diagonal( int rowCount, int columnCount, float value )
        {
            // Contract.Ensures( Contract.Result<Matrix>() != null );
            // Contract.Ensures( Contract.Result<Matrix>().RowCount == rowCount );
            // Contract.Ensures( Contract.Result<Matrix>().ColumnCount == rowCount );

            Matrix matrix = new Matrix( rowCount, columnCount );
            int diagonalSize = System.Math.Min( rowCount, columnCount );

            for( int i = 0; i < diagonalSize; ++i )
            {
                matrix[i, i] = value;
            }

            return matrix;
        }

        #endregion

        #region > Impls/Overrides <

        #region Equals

        /// <summary>
        /// Returns whether the specified <see cref="Object"/>
        /// is equal to this <see cref="Matrix"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test against. Can be null.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified <see cref="Object"/> is equal to this <see cref="Matrix"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            return this.Equals( obj as Matrix );
        }

        /// <summary>
        /// Returns whether the specified <see cref="Matrix"/>
        /// is equal to this <see cref="Matrix"/>.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Matrix"/> to test against. Can be null.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified <see cref="Matrix"/> is equal to this <see cref="Matrix"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( Matrix other )
        {
            if( other == null )
                return false;

            if( this.RowCount != other.RowCount || this.ColumnCount != other.ColumnCount )
                return false;

            for( int row = 0; row < RowCount; ++row )
            {
                for( int column = 0; column < ColumnCount; ++column )
                {
                    if( !this[row, column].IsApproximate( other[row, column] ) )
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
        public new Matrix Clone()
        {
            Matrix clone = new Matrix( RowCount, ColumnCount );

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
        /// with the data needed to serialize the <see cref="Matrix"/>.
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
        /// for this serialization.
        /// </param>
        [System.Security.Permissions.SecurityPermissionAttribute(
            System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true )]
        public override void GetObjectData(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        {
            base.GetObjectData( info, context );

            info.SetType( typeof( Matrix ) );
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a humen-readable representation of the <see cref="Matrix"/>.
        /// </summary>
        /// <returns>A string representing this ObjectMatrix.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a humen-readable representation of the <see cref="Matrix"/>.
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
                    float value = this[row, column];

                    if( value >= 0.0f )
                        sb.Append( ' ' );

                    sb.Append( value.ToString( formatProvider ) );

                    if( column != (ColumnCount - 1) )
                        sb.Append( ' ' );
                }
                sb.Append( " |" );

                sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion

        #region ToStringNoZero

        /// <summary>
        /// Returns a humen-readable representation of the <see cref="Matrix"/>,
        /// replacing all zeroes with an empty space.
        /// </summary>
        /// <returns>A string representing this ObjectMatrix.</returns>
        public string ToStringNoZero()
        {
            return ToStringNoZero( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a humen-readable representation of the <see cref="Matrix"/>,
        /// replacing all zeroes with an empty space.
        /// </summary>
        /// <param name="formatProvider">
        /// Provides access to culture-sensitive formatting information.
        /// </param>
        /// <returns>A string representing this ObjectMatrix.</returns>
        public string ToStringNoZero( IFormatProvider formatProvider )
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder( RowCount * ColumnCount * 5 );

            for( int row = 0; row < RowCount; ++row )
            {
                sb.Append( '|' );
                for( int column = 0; column < ColumnCount; ++column )
                {
                    float value = this[row, column];

                    if( value == 0.0f )
                    {
                        sb.Append( "  " );
                    }
                    else
                    {
                        if( value > 0.0f )
                            sb.Append( ' ' );

                        sb.Append( value.ToString( formatProvider ) );
                    }

                    if( column != (ColumnCount - 1) )
                        sb.Append( ' ' );
                }
                sb.Append( " |" );

                sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        #region *

        #region *( Matrix left, Matrix right )

        /// <summary>
        /// Returns the result of multiplying two matrices.
        /// </summary>
        /// <param name="left">The matrix on the left side of the equation.</param>
        /// <param name="right">The matrix on the right side of the equation.</param>
        /// <returns>The result of the times operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="left"/> or <paramref name="right"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the <see cref="Atom.Collections.ObjectMatrix{T}.ColumnCount"/> of the left Matrix is
        /// not equal the <see cref="Atom.Collections.ObjectMatrix{T}.RowCount"/> of the right Matrix.
        /// </exception>
        public static Matrix operator *( Matrix left, Matrix right )
        {
            return Multiply( left, right );
        }

        #endregion

        #region *( Matrix matrix, float scalar )

        /// <summary>
        /// Multiplies the given <see cref="Matrix"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> on the left side of the equation.</param>
        /// <param name="scalar">The scalar on the right side of the equation.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="matrix"/> is null.
        /// </exception>
        public static Matrix operator *( Matrix matrix, float scalar )
        {
            return Multiply( matrix, scalar );
        }

        #endregion

        #region *( float scalar, Matrix matrix )

        /// <summary>
        /// Multiplies the given <see cref="Matrix"/> by the given <paramref name="scalar"/>.
        /// </summary>
        /// <param name="scalar">The scalar on the left side of the equation.</param>
        /// <param name="matrix">The <see cref="Matrix"/> on the right side of the equation.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="matrix"/> is null.
        /// </exception>
        public static Matrix operator *( float scalar, Matrix matrix )
        {
            return Multiply( matrix, scalar );
        }

        #endregion

        #endregion

        #region +

        /// <summary>
        /// Returns the result of adding 
        /// the <paramref name="right"/> <see cref="Matrix"/> to
        /// the <paramref name="left"/> <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>    
        /// <returns>
        /// The result of the operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Matrices is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the dimensions of the specified Matrices is not equal.
        /// </exception>
        public static Matrix operator +( Matrix left, Matrix right )
        {
            return Add( left, right );
        }

        #endregion

        #region -

        /// <summary>
        /// Returns the result of subtracting 
        /// the <paramref name="right"/> <see cref="Matrix"/> from
        /// the <paramref name="left"/> <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>    
        /// <returns>
        /// The result of the operation.
        /// </returns>     
        /// <exception cref="ArgumentNullException">
        /// If any of the specified Matrices is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the dimensions of the specified Matrices is not equal.
        /// </exception>
        public static Matrix operator -( Matrix left, Matrix right )
        {
            return Subtract( left, right );
        }

        /// <summary>
        /// Returns the result of negating the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">
        /// The Matrix to negate.
        /// </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static Matrix operator -( Matrix matrix )
        {
            return Negate( matrix );
        }

        #endregion

        #region > Logic <

        /// <summary>
        /// Returns whether given <see cref="Matrix"/> instances are equal.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==( Matrix left, Matrix right )
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
        /// Returns whether given <see cref="Matrix"/> instances are not equal.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=( Matrix left, Matrix right )
        {
            return !(left == right);
        }

        #endregion

        #endregion
    }
}
