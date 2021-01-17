// <copyright file="LUDecomposition.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.LUDecomposition class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary> 
    /// Implements LU decomposition of rectangular Matrices.
    /// This is a sealed class.
    /// </summary>
    /// <remarks>
    /// Given a m-by-n Matrix A (with m >= n),
    /// the LU decomposition is 
    /// a m-by-n unit lower triangular Matrix L,
    /// a n-by-n upper triangular Matrix U and 
    /// a permutation point PIV of length m
    /// so that A(piv)=L*U.
    /// If m &lt; n, then L is m-bx-m and U m-by-n.
    /// </remarks>
    /// <remarks>
    /// Adapted from the JAMA package : http://math.nist.gov/javanumerics/jama/
    /// </remarks>
    public sealed class LUDecomposition
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the lower triangular factor L, with A=LU.
        /// </summary>
        /// <value>The lower triangular factor.</value>
        public Matrix LeftFactor
        {
            get 
            { 
                return this.LowerTriangularFactor;
            }
        }

        /// <summary>
        /// Gets the upper triangular factor U, with A=LU.
        /// </summary>
        /// <value>The upper triangular factor.</value>
        public Matrix RightFactor
        {
            get 
            {
                return this.UpperTriangularFactor; 
            }
        }  
        
        /// <summary>
        /// Gets the row-count of the decomposed matrix.
        /// </summary>
        public int RowCountLU
        {
            get
            {
                return this.lu.RowCount;
            }
        }

        /// <summary>
        /// Gets the column-count of the decomposed matrix.
        /// </summary>
        public int ColumnCountLU
        {
            get
            {
                return this.lu.ColumnCount;
            }
        }

        #region IsSingular

        /// <summary>
        /// Gets a value indicating whether the decomposed <see cref="Matrix"/> is singular.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if the Matrix is singular; otherwise <see langword="false"/>.
        /// </value>
        public bool IsSingular
        {
            get
            {
                for( int j = 0; j < lu.ColumnCount; ++j )
                {
                    if( lu[j, j] == 0 )
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        #endregion

        #region Determinant

        /// <summary>
        /// Gets the determinant of the decomposed <see cref="Matrix"/>.
        /// </summary>
        /// <value>The determinant.</value>
        public float Determinant
        {
            get
            {
                float determinant = pivotSign;

                for( int j = 0; j < lu.ColumnCount; ++j )
                {
                    determinant *= lu[j, j];
                }

                return determinant;
            }
        }

        #endregion

        #region LowerTriangularFactor

        /// <summary>
        /// Gets the lower triangular factor L, with A=LU.
        /// </summary>
        /// <value>The lower triangular factor.</value>
        public Matrix LowerTriangularFactor
        {
            get
            {
                Matrix x = new Matrix( lu.RowCount, lu.ColumnCount );

                for( int i = 0; i < lu.RowCount; ++i )
                {
                    for( int j = 0; j < lu.ColumnCount; ++j )
                    {
                        if( i > j )
                        {
                            x[i, j] = lu[i, j];
                        }
                        else if( i == j )
                        {
                            x[i, j] = 1.0f;
                        }
                        else
                        {
                            x[i, j] = 0.0f;
                        }
                    }
                }

                return x;
            }
        }

        #endregion

        #region UpperTriangularFactor

        /// <summary>
        /// Gets the upper triangular factor U, with A=LU.
        /// </summary>
        /// <value>The upper triangular factor.</value>
        public Matrix UpperTriangularFactor
        {
            get
            {
                Matrix x = new Matrix( lu.RowCount, lu.ColumnCount );

                for( int i = 0; i < lu.RowCount; ++i )
                {
                    for( int j = 0; j < lu.ColumnCount; ++j )
                    {
                        if( i <= j )
                        {
                            x[i, j] = lu[i, j];
                        }
                        else
                        {
                            x[i, j] = 0.0f;
                        }
                    }
                }

                return x;
            }
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="LUDecomposition"/> class.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="matrix"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified <paramref name="matrix"/> is not a square Matrix.
        /// </exception>
        public LUDecomposition( Matrix matrix )
        {
            this.Decompose( matrix );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Decomposes the specified matrix using a LU decomposition.
        /// </summary>
        /// <param name="matrix">The matrix to decompose.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="matrix"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified <paramref name="matrix"/> is not a square Matrix.
        /// </exception>
        public void Decompose( Matrix matrix )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );
            Contract.Requires<ArgumentException>( matrix.IsSquare );

            this.lu = matrix.Clone();
            this.pivots = new int[lu.RowCount];

            for( int i = 0; i < lu.RowCount; ++i )
                pivots[i] = i;
            pivotSign = 1;

            float[] column = new float[lu.RowCount];

            for( int j = 0; j < lu.ColumnCount; ++j )
            {
                for( int i = 0; i < lu.RowCount; ++i )
                {
                    column[i] = lu[i, j];
                }

                // Apply previous transformations.
                for( int i = 0; i < lu.RowCount; ++i )
                {
                    // Most of the time is spent in the following dot product.
                    int kmax = System.Math.Min( i, j );
                    float s = 0.0f;

                    for( int k = 0; k < kmax; ++k )
                    {
                        s += lu[i, k] * column[k];
                    }

                    lu[i, j] = column[i] - s;
                    column[i] -= s;
                }

                // Find pivot and exchange if necessary.
                int p = j;

                for( int i = j + 1; i < lu.RowCount; ++i )
                {
                    if( System.Math.Abs( column[i] ) > System.Math.Abs( column[p] ) )
                    {
                        p = i;
                    }
                }

                if( p != j )
                {
                    for( int k = 0; k < lu.ColumnCount; ++k )
                    {
                        float temp = lu[p, k];

                        lu[p, k]   = lu[j, k];
                        lu[j, k]   = temp;
                    }

                    int tempPivot = pivots[p];
                    pivots[p]     = pivots[j];
                    pivots[j]     = tempPivot;

                    pivotSign = -pivotSign;
                }

                // Compute multipliers.
                if( (j < lu.RowCount) && (lu[j, j] != 0.0) )
                {
                    for( int i = j + 1; i < lu.RowCount; ++i )
                    {
                        lu[i, j] /= lu[j, j];
                    }
                }
            }
        }

        /// <summary>
        /// Solves the equation A*X = B.
        /// </summary>
        /// <param name="matrixB">
        /// A <see cref="Matrix"/> with as many rows as A and any number of columns.
        /// </param>
        /// <returns>
        /// The Matrix X, so that A*X = B.
        /// </returns>
        public Matrix Solve( Matrix matrixB )
        {
            Contract.Requires<ArgumentNullException>( matrixB != null );
            Contract.Requires<ArgumentException>( matrixB.RowCount == this.RowCountLU, MathErrorStrings.MatrixRowCountMismatch );
            Contract.Requires<InvalidOperationException>( !this.IsSingular, MathErrorStrings.MatrixIsSingular );
                            
            // Copy right hand side with pivoting
            int nx = matrixB.ColumnCount;

            Matrix x = GetSubMatrix( matrixB, pivots, 0, nx - 1 );

            // Solve L*Y = B(piv);
            for( int k = 0; k < lu.ColumnCount; ++k )
            {
                for( int i = k + 1; i < lu.ColumnCount; ++i )
                {
                    for( int j = 0; j < nx; ++j )
                    {
                        x[i, j] -= x[k, j] * lu[i, k];
                    }
                }
            }

            // Solve U*X = Y;
            for( int k = lu.ColumnCount - 1; k >= 0; --k )
            {
                for( int j = 0; j < nx; ++j )
                {
                    x[k, j] /= lu[k, k];
                }

                for( int i = 0; i < k; ++i )
                {
                    for( int j = 0; j < nx; ++j )
                    {
                        x[i, j] -= x[k, j] * lu[i, k];
                    }
                }
            }

            return x;
        }

        /// <summary>
        /// Helper method that gets the sub matrix specified 
        /// with the row indices, the start column, and the end column.
        /// </summary>
        /// <param name="matrix">The given Matrix.</param>
        /// <param name="rows">The row indices.</param>
        /// <param name="columnStart">The column start.</param>
        /// <param name="columnEnd">The column end.</param>
        /// <returns>The sub matrix.</returns>
        private static Matrix GetSubMatrix( Matrix matrix, int[] rows, int columnStart, int columnEnd )
        {
            Matrix subMatrix = new Matrix( rows.Length, columnEnd - columnStart + 1 );

            for( int i = 0; i < rows.Length; ++i )
            {
                for( int j = columnStart; j <= columnEnd; ++j )
                {
                    subMatrix[i, j - columnStart] = matrix[rows[i], j];
                }
            }

            return subMatrix;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Stores the LU docompusation.
        /// </summary>
        private Matrix lu;

        /// <summary>
        /// Stores the sign of permutation point.
        /// </summary>
        private int pivotSign;

        /// <summary>
        /// The permutation point. 
        /// </summary>
        private int[] pivots;

        #endregion
    }
}
