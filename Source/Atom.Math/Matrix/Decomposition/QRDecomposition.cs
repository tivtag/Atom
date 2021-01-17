// <copyright file="QRDecomposition.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.QRDecomposition class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// In linear algebra, the QR decomposition (also called the QR factorization) of a <see cref="Matrix"/> 
    /// is a decomposition of the matrix into an orthogonal and a triangular matrix. 
    /// The QR decomposition is often used to solve the linear least squares problem.
    /// The QR decomposition is also the basis for a particular eigenvalue algorithm, the QR algorithm.
    /// </summary>
    /// <remarks>
    /// Given a Row-by-Columns matrix A (with Rows >= Columns),
    /// the result of the QR decomposition is 
    /// a Rows-by-Columns orthogonal matrix Q and
    /// a Columns-by-Columns upper triangular matrix R
    /// so that A = Q*R.
    /// </remarks>
    /// <remarks>
    /// Adapted from the JAMA package : http://math.nist.gov/javanumerics/jama/
    /// </remarks>
    [Serializable]
    public sealed class QRDecomposition
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the (economy-sized) orthogonal factor Q, with A=QR.
        /// </summary>
        /// <value>The orthogonal factor.</value>
        public Matrix LeftFactor
        {
            get
            { 
                return this.OrthogonalFactorQ; 
            }
        }

        /// <summary>
        /// Gets the the upper triangular factor R, with A=QR.
        /// </summary>
        /// <value>The upper triangular factor.</value>
        public Matrix RightFactor
        {
            get 
            { 
                return this.UpperTriangularFactorR;
            }
        }

        /// <summary>
        /// Gets the row-count of the decomposed matrix.
        /// </summary>
        public int RowCountQR
        {
            get
            {
                return this.qr.RowCount;
            }
        }

        /// <summary>
        /// Gets the column-count of the decomposed matrix.
        /// </summary>
        public int ColumnCountQR
        {
            get
            {
                return this.qr.ColumnCount;
            }
        }

        #region IsFullRank

        /// <summary>
        /// Gets a value indicating whether the matrix is full rank.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if R, and hence A, has full rank; otherwise <see langword="false"/>.
        /// </value>
        public bool IsFullRank
        {
            get
            {
                for( int j = 0; j < qr.ColumnCount; ++j )
                {
                    if( diagonal[j] == 0 )
                        return false;
                }

                return true;
            }
        }

        #endregion

        #region HouseholderVectors

        /// <summary>
        /// Gets the Householder vectors.
        /// </summary>
        /// <value>Lower trapezoidal matrix whose columns define the reflections.</value>
        public Matrix HouseholderVectors
        {
            get
            {
                Matrix x = new Matrix( qr.RowCount, qr.ColumnCount );

                for( int i = 0; i < qr.RowCount; ++i )
                {
                    for( int j = 0; j < qr.ColumnCount; ++j )
                    {
                        if( i >= j )
                        {
                            x[i, j] = qr[i, j];
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

        #region UpperTriangularFactorR

        /// <summary>
        /// Gets the the upper triangular factor of the decomposition.
        /// </summary>
        /// <value>A new Matrix that contains the upper triangular factor.</value>
        public Matrix UpperTriangularFactorR
        {
            get
            {
                Matrix factorR = new Matrix( qr.ColumnCount, qr.ColumnCount );

                for( int i = 0; i < qr.ColumnCount; ++i )
                {
                    for( int j = 0; j < qr.ColumnCount; ++j )
                    {
                        if( i < j )
                        {
                            factorR[i, j] = qr[i, j];
                        }
                        else if( i == j )
                        {
                            factorR[i, j] = diagonal[i];
                        }
                        else
                        {
                            factorR[i, j] = 0.0f;
                        }
                    }
                }

                return factorR;
            }
        }

        #endregion

        #region OrthogonalFactorQ

        /// <summary>
        /// Gets the (economy-sized) orthogonal factor.
        /// </summary>
        /// <value>A new Matrix that contains the (economy-sized) orthogonal factor.</value>
        public Matrix OrthogonalFactorQ
        {
            get
            {
                Matrix factorQ = new Matrix( qr.RowCount, qr.ColumnCount );

                for( int k = qr.ColumnCount - 1; k >= 0; --k )
                {
                    for( int i = 0; i < qr.RowCount; ++i )
                    {
                        factorQ[i, k] = 0.0f;
                    }

                    factorQ[k, k] = 1.0f;

                    for( int j = k; j < qr.ColumnCount; ++j )
                    {
                        if( qr[k, k] != 0 )
                        {
                            float s = 0.0f;
                            for( int i = k; i < qr.RowCount; ++i )
                            {
                                s += qr[i, k] * factorQ[i, j];
                            }

                            s = (-s) / qr[k, k];

                            for( int i = k; i < qr.RowCount; ++i )
                            {
                                factorQ[i, j] += s * qr[i, k];
                            }
                        }
                    }
                }

                return factorQ;
            }
        }

        #endregion

        #endregion
        
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="QRDecomposition"/> class.
        /// </summary>
        /// <param name="matrix">A rectangular matrix.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="matrix"/> is null.
        /// </exception>
        public QRDecomposition( Matrix matrix )
        {
            this.Decompose( matrix );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Decomposes the specified matrix, using a QR decomposition.
        /// </summary>
        /// <param name="matrix">The matrix to decompose.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="matrix"/> is null.
        /// </exception>
        public void Decompose( Matrix matrix )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );

            this.qr = matrix.Clone();
            this.diagonal = new float[qr.ColumnCount];

            // Main loop.
            for( int k = 0; k < qr.ColumnCount; k++ )
            {
                // Compute 2-norm of k-th column without under/overflow.
                float nrm = 0.0f;

                for( int i = k; i < qr.RowCount; i++ )
                {
                    nrm = MathUtilities.Hypotenuse( nrm, qr[i, k] );
                }

                if( nrm != 0.0f )
                {
                    // Form k-th Householder vector.
                    if( qr[k, k] < 0 )
                    {
                        nrm = -nrm;
                    }

                    for( int i = k; i < qr.RowCount; i++ )
                    {
                        qr[i, k] = qr[i, k] / nrm;
                    }

                    qr[k, k] = qr[k, k] + 1.0f;

                    // Apply transformation to remaining columns.
                    for( int j = k + 1; j < qr.ColumnCount; j++ )
                    {
                        float s = 0.0f;

                        for( int i = k; i < qr.RowCount; i++ )
                        {
                            s += qr[i, k] * qr[i, j];
                        }

                        s = (-s) / qr[k, k];

                        for( int i = k; i < qr.RowCount; i++ )
                        {
                            qr[i, j] = qr[i, j] + (s * qr[i, k]);
                        }
                    }
                }

                diagonal[k] = -nrm;
            }
        }

        /// <summary>
        /// Computes the least squares solution of A*X = B.
        /// </summary>
        /// <param name="matrixB">
        /// A Matrix with as many rows as A and any number of columns.
        /// </param>
        /// <returns>
        /// The Matrix X that minimizes the two norm of Q*R*X-B.
        /// </returns>
        /// <exception cref="ArgumentException">The row dimensions of the matrices must agree.</exception>
        /// <exception cref="InvalidOperationException">Matrix is rank deficient.</exception>
        public Matrix Solve( Matrix matrixB )
        {
            Contract.Requires<ArgumentNullException>( matrixB != null );
            Contract.Requires<ArgumentException>( matrixB.RowCount == this.RowCountQR );
            Contract.Requires<InvalidOperationException>( this.IsFullRank, MathErrorStrings.MatrixRankIsNotFull );

            // Copy right hand side
            int nx = matrixB.ColumnCount;
            Matrix x = matrixB.Clone();

            // Compute Y = transpose(Q)*B
            for( int k = 0; k < qr.ColumnCount; ++k )
            {
                for( int j = 0; j < nx; ++j )
                {
                    float s = 0.0f;
                    for( int i = k; i < qr.RowCount; ++i )
                    {
                        s += qr[i, k] * x[i, j];
                    }

                    s = (-s) / qr[k, k];

                    for( int i = k; i < qr.RowCount; ++i )
                    {
                        x[i, j] += s * qr[i, k];
                    }
                }
            }

            // Solve R*X = Y;
            for( int k = qr.ColumnCount - 1; k >= 0; --k )
            {
                for( int j = 0; j < nx; j++ )
                {
                    x[k, j] /= diagonal[k];
                }

                for( int i = 0; i < k; ++i )
                {
                    for( int j = 0; j < nx; ++j )
                    {
                        x[i, j] -= x[k, j] * qr[i, k];
                    }
                }
            }

            return x.GetSubMatrix( 0, 0, qr.ColumnCount, nx );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Stores the result of the the QR decomposition.
        /// </summary>
        private Matrix qr;

        /// <summary>
        /// Stores the diagonale of R.
        /// </summary>
        private float[] diagonal;

        #endregion
    }
}
