// <copyright file="CholeskyDecomposition.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.CholeskyDecomposition class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Implements Cholesky Decomposition of a rectangular <see cref="Matrix"/>.
    /// This class can't be inherited.
    /// </summary>
    /// <remarks>
    /// Adapted from
    /// the NGenerics framework,
    /// the JAMA package : http://math.nist.gov/javanumerics/jama/
    /// and from Numerical recipes.
    /// </remarks>
    [Serializable]
    public sealed class CholeskyDecomposition
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the lower triangular factor U^T, with A=U^T x U.
        /// </summary>
        /// <value>The triangular factor 'L'.</value>
        public Matrix LeftFactor
        {
            get 
            {
                return this.TriangularFactorL;
            }
        }

        /// <summary>
        /// Gets the upper triangular factor U, with A=U^T x U.
        /// </summary>
        /// <value>The transpose of the triangular factor 'L'.</value>
        public Matrix RightFactor
        {
            get 
            { 
                return this.TriangularFactorL.Transpose;
            }
        }

        /// <summary>
        /// Gets the triangular factor 'L'.
        /// </summary>
        /// <value>The triangular factor 'L'.</value>
        public Matrix TriangularFactorL
        {
            get
            {
                return this.l;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="CholeskyDecomposition"/> class.
        /// </summary>
        /// <param name="matrix">
        /// A square, symmetric input matrix. 
        /// </param>
        /// <exception cref="ArgumentNullException">If <paramref name="matrix"/> is null.</exception>
        /// <exception cref="ArgumentException">If the specified <paramref name="matrix"/> is a not symmetric.</exception>
        /// <exception cref="ArgumentException">If the specified <paramref name="matrix"/> is a not square.</exception>
        public CholeskyDecomposition( Matrix matrix )
        {
            this.Decompose( matrix );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Given a positive-definite symmetric matrix <c>A[0..n][0..n]</c>, 
        /// this routine constructs its Cholesky decomposition,  <c> A = L*(L^T) </c>. 
        /// </summary>
        /// <remarks>
        /// The operations count is <c>(N^3)/6</c> executions of the inner loop (consisting of 
        /// one multiply and one subtract), with also N square roots. 
        /// This is about a factor 2 better than LU decomposition of <c>A</c> 
        /// (where its symmetry would be ignored).
        /// </remarks>
        /// <param name="matrix">
        /// The square symmetric definite-defined input matrix A. 
        /// </param>
        public void Decompose( Matrix matrix )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );
            Contract.Requires<ArgumentException>( matrix.IsSymmetric );

            this.dimension = matrix.RowCount;
            Matrix result = new Matrix( dimension, dimension );

            for( int i = 0; i < dimension; ++i )
            {
                int j;
                for( j = i; j < dimension; ++j )
                {
                    int k;
                    float sum;

                    for( sum = matrix[i, j], k = i - 1; k >= 0; --k )
                    {
                        sum -= result[i, k] * result[j, k];
                    }

                    if( i == j )
                    {
                        if( sum <= 0 )
                            throw new ArgumentException( MathErrorStrings.MatrixNotPositiveDefinite, "matrix" );
                       
                        result[i, i] = (float)System.Math.Sqrt( sum );
                    }
                    else
                    {
                        result[j, i] = sum / result[i, i];
                    }
                }
            }

            this.l = result;
        }

        /// <summary>
        /// Given a positive-definite symmetric matrix <c>A[0..n][0..n]</c>, 
        /// this routine constructs its Cholesky decomposition,  <c> A = L*(L^T) </c>. 
        /// </summary>
        /// <remarks>
        /// The operations count is <c>(N^3)/6</c> executions of the inner loop (consisting of 
        /// one multiply and one subtract), with also N square roots. 
        /// This is about a factor 2 better than LU decomposition of <c>A</c> 
        /// (where its symmetry would be ignored).
        /// </remarks>
        /// <param name="matrix">
        /// The square symmetric definite-defined input matrix A. 
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="matrix"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="matrix"/> is not symmetric.</exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="matrix"/> is not square.
        /// </exception>
        /// <returns>
        /// The Cholesky factor L.
        /// </returns>
        public static Matrix QuickDecompose( Matrix matrix )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );
            Contract.Requires<ArgumentException>( matrix.IsSymmetric );

            int n = matrix.RowCount;
            Matrix res = new Matrix( n, n );

            for( int i = 0; i < n; ++i )
            {
                for( int j = i; j < n; ++j )
                {
                    int k;
                    float sum;

                    for( sum = matrix[i, j], k = i - 1; k >= 0; --k )
                    {
                        sum -= res[i, k] * res[j, k];
                    }

                    if( i == j )
                    {
                        if( sum <= 0 )
                            throw new ArgumentException( MathErrorStrings.MatrixNotPositiveDefinite, "matrix" );

                        res[i, i] = (float)System.Math.Sqrt( sum );
                    }
                    else
                    {
                        res[j, i] = sum / res[i, i];
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Solves the set of <c>n</c> linear equations <c> A * x = b </c>.
        /// </summary>
        /// <param name="matrix">
        /// A positive-definite symmetric input matrix <c>[0..n][0..n]</c>.
        /// </param>
        /// <param name="vector">
        /// The right-hand side input vector <c>[0..n]</c>.
        /// </param>
        /// <returns>
        /// The solution vector is returned as <c>[0..n]</c>.
        /// </returns>
        public static float[] QuickSolveLinearEquation( Matrix matrix, float[] vector )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );
            Contract.Requires<ArgumentNullException>( vector != null );
            Contract.Requires<ArgumentException>( matrix.RowCount == vector.Length );
            Contract.Requires<ArgumentException>( matrix.IsSymmetric );
            // Contract.Ensures( Contract.Result<float[]>() != null );
            // Contract.Ensures( Contract.Result<float[]>().Length == matrix.RowCount );

            int n = matrix.RowCount;
            Matrix  factorL   = QuickDecompose( matrix );
            float[] solutionX = new float[n];

            int k;
            float sum;

            for( int i = 0; i < n; ++i )
            {
                // Solve <c>L * y = b</c>, storing y in x.
                for( sum = vector[i], k = i - 1; k >= 0; --k )
                    sum -= factorL[i, k] * solutionX[k];

                solutionX[i] = sum / factorL[i, i];
            }

            for( int i = n - 1; i >= 0; --i )
            {
                // Solve L^T * x = y.
                for( sum = solutionX[i], k = i + 1; k < n; ++k )
                    sum -= factorL[k, i] * solutionX[k];

                solutionX[i] = sum / factorL[i, i];
            }

            return solutionX;
        }

        /// <summary>
        /// Solves the equation A*X = B.
        /// </summary>
        /// <param name="matrixB">
        /// The input matrix B, with as many rows as A and any number of columns.
        /// </param>
        /// <returns>
        /// The matrix X; so that L*L'*X = B.
        /// </returns>
        public Matrix Solve( Matrix matrixB )
        {
            Contract.Requires<ArgumentNullException>( matrixB != null );
            Contract.Requires<ArgumentNullException>( matrixB.RowCount == this.LeftFactor.RowCount );
            Contract.Requires<ArgumentNullException>( matrixB.ColumnCount == 1 );

            // Contract.Ensures( Contract.Result<Matrix>() != null );
            // Contract.Ensures( Contract.Result<Matrix>().RowCount == dimension );
            // Contract.Ensures( Contract.Result<Matrix>().ColumnCount == 1 );

            var m = new float[dimension];
            for( int j = 0; j < dimension; ++j )
            {
                m[j] = matrixB[j, 0];
            }

            int k;
            float sum;
            var x = new float[dimension];

            for( int i = 0; i < dimension; ++i )
            {
                // Solve <c>L * y = b</c>, storing y in x.
                for( sum = m[i], k = i - 1; k >= 0; --k )
                {
                    sum -= this.l[i, k] * x[k];
                }
                x[i] = sum / this.l[i, i];
            }

            for( int i = dimension - 1; i >= 0; --i )
            {
                // Solve L^T * x = y.
                for( sum = x[i], k = i + 1; k < dimension; ++k )
                {
                    sum -= l[k, i] * x[k];
                }
                x[i] = sum / l[i, i];
            }

            var result = new Matrix( dimension, 1 );

            for( int i = 0; i < x.Length; ++i )
            {
                result[i, 0] = x[i];
            }

            return result;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Array for internal storage of decomposition.
        /// </summary>
        private Matrix l;

        /// <summary>
        /// Row and column dimension (square matrix).
        /// </summary> 
        private int dimension;

        #endregion
    }
}
