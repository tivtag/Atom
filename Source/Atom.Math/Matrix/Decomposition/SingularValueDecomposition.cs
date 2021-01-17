// <copyright file="SingularValueDecomposition.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.SingularValueDecomposition class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Implements Singular Value decomposition of rectangular Matrices.
    /// This is a sealed class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For an m-by-n matrix A with m >= n, the singular value decomposition 
    /// is an m-by-n orthogonal matrix U, an n-by-n diagonal matrix S, and
    /// an n-by-n orthogonal matrix V so that A = U*S*V'.
    /// </para><para>
    /// The singular values, sigma[k] = S[k, k], are ordered so that
    /// sigma[0] >= sigma[1] >= ... >= sigma[n-1].
    /// </para><para>
    /// The singular value decompostion always exists, so the constructor will
    /// never fail.  The matrix condition number and the effective numerical
    /// rank can be computed from this decomposition.
    /// </para>
    /// </remarks>
    public sealed class SingularValueDecomposition
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the two norm.
        /// </summary>
        /// <value>The two norm; max(S).</value>
        public float Norm2
        {
            get
            {
                return values[0];
            }
        }

        /// <summary>
        /// Gets the two-norm condition number.
        /// </summary>
        /// <value>The two-norm condition number; max(S)/min(S).</value>
        public float Condition
        {
            get
            {
                return values[0] / values[System.Math.Min( rowCount, columnCount ) - 1];
            }
        }

        /// <summary>
        /// Gets the effective numerical matrix rank.
        /// </summary>
        /// <value>The number of non-negligible singular values.</value>
        public int Rank
        {
            get
            {
                float eps = (float)System.Math.Pow( 2.0, -52.0 );
                float tol = System.Math.Max( rowCount, columnCount ) * values[0] * eps;
                int r = 0;

                for( int i = 0; i < this.values.Length; ++i )
                {
                    if( this.values[i] > tol )
                    {
                        ++r;
                    }
                }

                return r;
            }
        }

        /// <summary>
        /// Gets the one-dimensional array of singular values.
        /// </summary>
        /// <remarks>This operation clones internal data, be careful when calling this method.</remarks>
        /// <returns>The diagonal matrix of S. </returns>
        public float[] GetSingularValues()
        {
            // Contract.Ensures( Contract.Result<float[]>() != null );

            return (float[])values.Clone();
        }

        /// <summary>
        /// Gets the diagonal matrix of singular values.
        /// </summary>
        /// <remarks>This operation clones internal data, be careful when calling this method.</remarks>
        /// <returns>A new Matrix that contains the singular values of the decomposed Matrix.</returns>
        public Matrix GetDiagonalSingularValues()
        {
            // Contract.Ensures( Contract.Result<Matrix>() != null );

            Matrix matrix = new Matrix( columnCount, columnCount );

            for( int i = 0; i < columnCount; ++i )
            {
                for( int j = 0; j < columnCount; ++j )
                    matrix[i, j] = 0.0f;

                matrix[i, i] = this.values[i];
            }

            return matrix;
        }

        /// <summary>
        /// Gets the left singular vectors (U matrix).
        /// </summary>
        /// <remarks>This operation clones internal data, be careful when calling this method.</remarks>
        /// <returns>A new Matrix that contains the left singular vectors (U matrix).</returns>
        public Matrix GetLeftSingularVectors()
        {
            // Contract.Ensures( Contract.Result<Matrix>() != null );

            return this.factorU.Clone();
        }

        /// <summary>
        /// Gets the right singular vectors (V matrix).
        /// </summary>
        /// <remarks>This operation clones internal data, be careful when calling this method.</remarks>
        /// <returns>A new Matrix that contains the right singular vectors (V matrix).</returns>
        public Matrix GetRightSingularVectors()
        {
            // Contract.Ensures( Contract.Result<Matrix>() != null );

            return this.factorV.Clone();
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="SingularValueDecomposition"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="argument"/> is null.
        /// </exception>
        /// <param name="argument">
        /// A rectangular input matrix.
        /// </param>
        public SingularValueDecomposition( Matrix argument )
        {
            Contract.Requires<ArgumentNullException>( argument != null );

            this.toTranspose = (argument.RowCount < argument.ColumnCount);

            // Derived from LINPACK code.
            Matrix matrixA = argument.Clone();
            if( this.toTranspose ) 
                matrixA = matrixA.Transpose;

            // Initialize.
            this.rowCount    = matrixA.RowCount;
            this.columnCount = matrixA.ColumnCount;
            int minDimension = System.Math.Min( rowCount, columnCount );

            this.values  = new float[System.Math.Min( rowCount + 1, columnCount )];
            this.factorU = new Matrix( rowCount, minDimension );
            this.factorV = new Matrix( columnCount, columnCount );
            float[] e    = new float[columnCount];

            int order;

            // Find the singular values.
            this.ReduceToBidiagonal( matrixA, e, out order );
            this.MainIterationLoop( e, order );

            // (vermorel) transposing the results if needed
            if( this.toTranspose )
            {
                // swaping U and V
                Matrix temp = factorV;

                factorV = factorU;
                factorU = temp;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Reduce A to bidiagonal form, storing the diagonal elements
        /// in s and the super-diagonal elements in e.
        /// </summary>
        /// <param name="matrixA">The input matrix.</param>
        /// <param name="e">
        /// Will contain the the super-diagonal elements.
        /// </param>
        /// <param name="order">Will contain the order of the reduced bidiagonal matrix.</param>
        private void ReduceToBidiagonal( Matrix matrixA, float[] e, out int order )
        {
            int minDimension = System.Math.Min( rowCount, columnCount );
            int nct          = System.Math.Min( rowCount - 1, columnCount );
            int nrt          = System.Math.Max( 0, System.Math.Min( columnCount - 2, rowCount ) );
            int maxNctNrt    = System.Math.Max( nct, nrt );

            float[] work = new float[rowCount];

            for( int k = 0; k < maxNctNrt; ++k )
            {
                if( k < nct )
                {
                    // Compute the transformation for the k-th column and
                    // place the k-th diagonal in s[k].
                    // Compute 2-norm of k-th column without under/overflow.
                    values[k] = 0;

                    for( int i = k; i < rowCount; ++i )
                    {
                        values[k] = MathUtilities.Hypotenuse( values[k], matrixA[i, k] );
                    }

                    if( values[k] != 0.0f )
                    {
                        if( matrixA[k, k] < 0.0f )
                        {
                            values[k] = -values[k];
                        }

                        for( int i = k; i < rowCount; ++i )
                        {
                            matrixA[i, k] /= values[k];
                        }

                        matrixA[k, k] += 1.0f;
                    }

                    values[k] = -values[k];
                }

                for( int j = k + 1; j < columnCount; ++j )
                {
                    if( (k < nct) & (values[k] != 0.0f) )
                    {
                        // Apply the transformation.
                        float t = 0.0f;
                        for( int i = k; i < rowCount; ++i )
                        {
                            t += matrixA[i, k] * matrixA[i, j];
                        }

                        t = (-t) / matrixA[k, k];
                        for( int i = k; i < rowCount; ++i )
                        {
                            matrixA[i, j] += t * matrixA[i, k];
                        }
                    }

                    // Place the k-th row of A into e for the
                    // subsequent calculation of the row transformation.
                    e[j] = matrixA[k, j];
                }

                if( wantU & (k < nct) )
                {
                    // Place the transformation in U for subsequent back
                    // multiplication.
                    for( int i = k; i < rowCount; ++i )
                    {
                        factorU[i, k] = matrixA[i, k];
                    }
                }

                if( k < nrt )
                {
                    ReduceBidiagonalComputeRowTransformation( matrixA, e, work, k );

                    if( wantV )
                    {
                        // Place the transformation in V for subsequent
                        // back multiplication.
                        for( int i = k + 1; i < columnCount; ++i )
                        {
                            factorV[i, k] = e[i];
                        }
                    }
                }
            }

            // Set up the final bidiagonal matrix or order p.
            int p = System.Math.Min( columnCount, rowCount + 1 );
            if( nct < columnCount )
                values[nct] = matrixA[nct, nct];

            if( rowCount < p )
                values[p - 1] = 0.0f;

            if( nrt + 1 < p )
                e[nrt] = matrixA[nrt, p - 1];

            e[p - 1] = 0.0f;

            // If required, generate U.
            if( wantU )
            {
                ReduceToBidiagonalGenerateU( minDimension, nct );
            }

            // If required, generate V.
            if( wantV )
            {
                ReduceToBidiagonalGenerateV( e, minDimension, nrt );
            }

            order = p;
        }

        /// <summary>
        /// Computes the k-th row transformation and place the k-th super-diagonal in e[k].
        /// </summary>
        /// <remarks>
        /// Computes 2-norm without under/overflow.
        /// </remarks>
        /// <param name="matrixA">
        /// The input matrix.
        /// </param>
        /// <param name="e">
        /// Will contain the the super-diagonal elements.
        /// </param>
        /// <param name="work">An array that may be used for internal work.</param>
        /// <param name="k">The current index.</param>
        private void ReduceBidiagonalComputeRowTransformation( Matrix matrixA, float[] e, float[] work, int k )
        {
            e[k] = 0.0f;
            for( int i = k + 1; i < columnCount; ++i )
            {
                e[k] = MathUtilities.Hypotenuse( e[k], e[i] );
            }

            if( e[k] != 0.0f )
            {
                if( e[k + 1] < 0.0f )
                {
                    e[k] = -e[k];
                }

                for( int i = k + 1; i < columnCount; ++i )
                {
                    e[i] /= e[k];
                }

                e[k + 1] += 1.0f;
            }

            e[k] = -e[k];

            if( (k + 1 < rowCount) & (e[k] != 0.0f) )
            {
                // Apply the transformation.
                for( int i = k + 1; i < rowCount; ++i )
                {
                    work[i] = 0.0f;
                }

                for( int j = k + 1; j < columnCount; ++j )
                {
                    for( int i = k + 1; i < rowCount; ++i )
                    {
                        work[i] += e[j] * matrixA[i, j];
                    }
                }

                for( int j = k + 1; j < columnCount; ++j )
                {
                    float t = (-e[j]) / e[k + 1];
                    for( int i = k + 1; i < rowCount; ++i )
                    {
                        matrixA[i, j] += t * work[i];
                    }
                }
            }
        }
        
        /// <summary>
        /// Generates the U factor.
        /// </summary>
        /// <param name="minDimension">
        /// The smaller dimension.
        /// </param>
        /// <param name="nct">
        /// The column start index.
        /// </param>
        private void ReduceToBidiagonalGenerateU( int minDimension, int nct )
        {
            for( int j = nct; j < minDimension; ++j )
            {
                for( int i = 0; i < rowCount; ++i )
                {
                    factorU[i, j] = 0.0f;
                }
                factorU[j, j] = 1.0f;
            }

            for( int k = nct - 1; k >= 0; --k )
            {
                if( values[k] != 0.0f )
                {
                    for( int j = k + 1; j < minDimension; ++j )
                    {
                        float t = 0.0f;
                        for( int i = k; i < rowCount; ++i )
                        {
                            t += factorU[i, k] * factorU[i, j];
                        }

                        t = (-t) / factorU[k, k];

                        for( int i = k; i < rowCount; ++i )
                        {
                            factorU[i, j] += t * factorU[i, k];
                        }
                    }

                    for( int i = k; i < rowCount; ++i )
                    {
                        factorU[i, k] = -factorU[i, k];
                    }

                    factorU[k, k] = 1.0f + factorU[k, k];

                    for( int i = 0; i < k - 1; ++i )
                    {
                        factorU[i, k] = 0.0f;
                    }
                }
                else
                {
                    for( int i = 0; i < rowCount; ++i )
                    {
                        factorU[i, k] = 0.0f;
                    }
                    
                    factorU[k, k] = 1.0f;
                }
            }
        }

        /// <summary>
        /// Generates the V factor.
        /// </summary>
        /// <param name="e">
        /// Will contain the the super-diagonal elements.
        /// </param>
        /// <param name="minDimension">
        /// The smaller dimension.
        /// </param>
        /// <param name="nrt">
        /// The row start index.
        /// </param>
        private void ReduceToBidiagonalGenerateV( float[] e, int minDimension, int nrt )
        {
            for( int k = columnCount - 1; k >= 0; --k )
            {
                if( (k < nrt) & (e[k] != 0.0f) )
                {
                    for( int j = k + 1; j < minDimension; ++j )
                    {
                        float t = 0.0f;
                        for( int i = k + 1; i < columnCount; ++i )
                        {
                            t += factorV[i, k] * factorV[i, j];
                        }

                        t = (-t) / factorV[k + 1, k];

                        for( int i = k + 1; i < columnCount; ++i )
                        {
                            factorV[i, j] += t * factorV[i, k];
                        }
                    }
                }

                for( int i = 0; i < columnCount; ++i )
                {
                    factorV[i, k] = 0.0f;
                }

                factorV[k, k] = 1.0f;
            }
        }

        /// <summary>
        /// The main iteration loop for finding the singular values.
        /// </summary>
        /// <param name="e">Contains the the super-diagonal elements.</param>
        /// <param name="order">Will contain the order of the reduced bidiagonal matrix.</param>
        private void MainIterationLoop( float[] e, int order )
        {
            int lastOrder = order - 1;
            int iter = 0;
            float eps = (float)System.Math.Pow( 2.0, -52.0 );

            while( order > 0 )
            {
                int k, kase;

                // Here is where a test for too many iterations would go.
                //
                // This section of the program inspects for
                // negligible elements in the s and e arrays.  On
                // completion the variables kase and k are set as follows.
                //
                // kase = 1     if s(p) and e[k-1] are negligible and k<p
                // kase = 2     if s(k) is negligible and k<p
                // kase = 3     if e[k-1] is negligible, k<p, and
                //              s(k), ..., s(p) are not negligible (qr step).
                // kase = 4     if e(p-1) is negligible (convergence).
                for( k = order - 2; k >= -1; --k )
                {
                    if( k == -1 )
                        break;

                    if( System.Math.Abs( e[k] ) <= eps * (System.Math.Abs( values[k] ) + System.Math.Abs( values[k + 1] )) )
                    {
                        e[k] = 0.0f;
                        break;
                    }
                }

                if( k == order - 2 )
                {
                    kase = 4;
                }
                else
                {
                    int ks;
                    for( ks = order - 1; ks >= k; --ks )
                    {
                        if( ks == k )
                        {
                            break;
                        }

                        double t = (ks != order ? System.Math.Abs( e[ks] ) : 0.0f) + (ks != k + 1 ? System.Math.Abs( e[ks - 1] ) : 0.0f);
                        if( System.Math.Abs( values[ks] ) <= eps * t )
                        {
                            values[ks] = 0.0f;
                            break;
                        }
                    }

                    if( ks == k )
                    {
                        kase = 3;
                    }
                    else if( ks == order - 1 )
                    {
                        kase = 1;
                    }
                    else
                    {
                        kase = 2;
                        k = ks;
                    }
                }

                ++k;

                // Perform the task indicated by kase.
                switch( kase )
                {
                    case 1:
                        this.DeflateNegligible( e, order, k );
                        break;

                    case 2:
                        this.SplitAtNegligibles( e, order, k );
                        break;

                    case 3:
                        this.PerformStepQR( e, order, ref iter, k );
                        break;

                    case 4:
                        this.Convergence( lastOrder, ref order, ref iter, ref k );
                        break;
                }
            }
        }

        /// <summary>
        /// Performs a single QR step.
        /// </summary>
        /// <param name="e">
        /// Will contain the the super-diagonal elements.
        /// </param>
        /// <param name="order"></param>
        /// <param name="iter"></param>
        /// <param name="k"></param>
        private void PerformStepQR( float[] e, int order, ref int iter, int k )
        {
            // Calculate the shift.
            float scale = System.Math.Max( 
                System.Math.Max(
                    System.Math.Max( 
                        System.Math.Max( System.Math.Abs( values[order - 1] ), System.Math.Abs( values[order - 2] ) ),
                        System.Math.Abs( e[order - 2] )
                    ),
                    System.Math.Abs( values[k] ) 
                ),
                System.Math.Abs( e[k] )
            );

            float sp   = values[order - 1] / scale;
            float spm1 = values[order - 2] / scale;
            float epm1 = e[order - 2] / scale;
            float sk = values[k] / scale;
            float ek = e[k] / scale;

            float b = (((spm1 + sp) * (spm1 - sp)) + (epm1 * epm1)) / 2.0f;
            float c = (sp * epm1) * (sp * epm1);

            float shift = 0.0f;
            if( (b != 0.0f) | (c != 0.0f) )
            {
                shift = (float)System.Math.Sqrt( (b * b) + c );

                if( b < 0.0f )
                    shift = -shift;

                shift = c / (b + shift);
            }

            float f = ((sk + sp) * (sk - sp)) + shift;
            float g = sk * ek;

            // Chase zeros.
            for( int j = k; j < order - 1; ++j )
            {
                float t = MathUtilities.Hypotenuse( f, g );
                float cs = f / t;
                float sn = g / t;

                if( j != k )
                {
                    e[j - 1] = t;
                }

                f = (cs * values[j]) + (sn * e[j]);
                e[j] = (cs * e[j]) - (sn * values[j]);
                g = sn * values[j + 1];

                values[j + 1] = cs * values[j + 1];

                if( wantV )
                {
                    for( int i = 0; i < columnCount; ++i )
                    {
                        t = (cs * factorV[i, j]) + (sn * factorV[i, j + 1]);

                        factorV[i, j + 1] = (-sn * factorV[i, j]) + (cs * factorV[i, j + 1]);
                        factorV[i, j]     = t;
                    }
                }

                t =  MathUtilities.Hypotenuse( f, g );
                cs = f / t;
                sn = g / t;

                values[j] = t;
                f = (cs * e[j]) + (sn * values[j + 1]);
                values[j + 1] = (-sn * e[j]) + (cs * values[j + 1]);
                g = sn * e[j + 1];
                e[j + 1] = cs * e[j + 1];

                if( wantU && (j < rowCount - 1) )
                {
                    for( int i = 0; i < rowCount; ++i )
                    {
                        t = (cs * factorU[i, j]) + (sn * factorU[i, j + 1]);

                        factorU[i, j + 1] = (-sn * factorU[i, j]) + (cs * factorU[i, j + 1]);
                        factorU[i, j]     = t;
                    }
                }
            }

            e[order - 2] = f;
            iter = iter + 1;
        }

        /// <summary>
        /// Executes the Split at negligibles task.
        /// </summary>
        /// <param name="e">
        /// Will contain the the super-diagonal elements.
        ///</param>
        /// <param name="order"></param>
        /// <param name="k"></param>
        private void SplitAtNegligibles( float[] e, int order, int k )
        {
            float f = e[k - 1];
            e[k - 1] = 0.0f;

            for( int j = k; j < order; ++j )
            {
                float t = MathUtilities.Hypotenuse( values[j], f );
                float cs = values[j] / t;
                float sn = f / t;

                values[j] = t;
                f = (-sn) * e[j];
                e[j] = cs * e[j];

                if( wantU )
                {
                    for( int i = 0; i < rowCount; ++i )
                    {
                        t = (cs * factorU[i, j]) + (sn * factorU[i, k - 1]);

                        factorU[i, k - 1] = (-sn * factorU[i, j]) + (cs * factorU[i, k - 1]);
                        factorU[i, j]     = t;
                    }
                }
            }
        }

        /// <summary>
        /// Executes the Deflate Negligibles task.
        /// </summary>
        /// <param name="e">
        /// Will contain the the super-diagonal elements.
        ///</param>
        /// <param name="order"></param>
        /// <param name="k"></param>
        private void DeflateNegligible( float[] e, int order, int k )
        {
            float f = e[order - 2];
            e[order - 2] = 0.0f;
            for( int j = order - 2; j >= k; j-- )
            {
                float t = MathUtilities.Hypotenuse( values[j], f );
                float cs = values[j] / t;
                float sn = f / t;
                values[j] = t;

                if( j != k )
                {
                    f = (-sn) * e[j - 1];
                    e[j - 1] = cs * e[j - 1];
                }

                if( wantV )
                {
                    for( int i = 0; i < columnCount; ++i )
                    {
                        t = (cs * factorV[i, j]) + (sn * factorV[i, order - 1]);

                        factorV[i, order - 1] = (-sn * factorV[i, j]) + (cs * factorV[i, order - 1]);
                        factorV[i, j]         = t;
                    }
                }
            }
        }

        /// <summary>
        /// Executes the Convergence task.
        /// </summary>
        /// <param name="lastOrder"></param>
        /// <param name="order"></param>
        /// <param name="iter"></param>
        /// <param name="k"></param>
        private void Convergence( int lastOrder, ref int order, ref int iter, ref int k )
        {
            // Make the singular values positive.
            if( values[k] <= 0.0f )
            {
                values[k] = (values[k] < 0.0f ? -values[k] : 0.0f);

                if( wantV )
                {
                    for( int i = 0; i <= lastOrder; ++i )
                    {
                        factorV[i, k] = -factorV[i, k];
                    }
                }
            }

            // Order the singular values.
            while( k < lastOrder )
            {
                if( values[k] >= values[k + 1] )
                {
                    break;
                }

                float t = values[k];
                values[k] = values[k + 1];
                values[k + 1] = t;

                if( wantV && (k < columnCount - 1) )
                {
                    for( int i = 0; i < columnCount; ++i )
                    {
                        // Exchange.
                        t = factorV[i, k + 1];

                        factorV[i, k + 1] = factorV[i, k];
                        factorV[i, k]     = t;
                    }
                }

                if( wantU && (k < rowCount - 1) )
                {
                    for( int i = 0; i < rowCount; ++i )
                    {
                        // Exchange.
                        t = factorU[i, k + 1];

                        factorU[i, k + 1] = factorU[i, k];
                        factorU[i, k]     = t;
                    }
                }

                ++k;
            }

            iter = 0;
            --order;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The matrices that store the U and V factors.
        /// </summary>
        private Matrix factorU, factorV;

        /// <summary>
        /// States whether U factor is required to be calcualted.
        /// </summary>
        private const bool wantU = true;

        /// <summary>
        /// States whether U factor is required to be calcualted.
        /// </summary>
        private const bool wantV = true;

        /// <summary>
        /// The array for internal storage of singular values.
        /// </summary>
        private readonly float[] values;

        /// <summary>
        /// The number of rows of the input matrix.
        /// </summary>
        private readonly int rowCount;

        /// <summary>
        /// The number of columns of the input matrix.
        /// </summary>
        private readonly int columnCount;

        /// <summary>
        /// States whether all the results provided by the method or properties should be transposed.
        /// </summary>
        /// <remarks>
        /// (vermorel) The initial implementation was assuming that
        /// m &gt;= n, but in fact, it is easy to handle the case m &lt; n
        /// by transposing all the results.
        /// </remarks>
        private bool toTranspose;

        #endregion
    }
}
