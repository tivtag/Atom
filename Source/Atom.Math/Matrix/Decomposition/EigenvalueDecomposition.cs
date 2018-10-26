// <copyright file="EigenvalueDecomposition.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.EigenvalueDecomposition class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Provides decomposition of a square real <see cref="Matrix"/>
    /// into its eigenvalues and eigenvectors. 
    /// </summary>
    /// <remarks>
    /// Big thanks to Paul Selormey for the implementation of this algorithm.
    /// See http://www.codeproject.com/KB/recipes/psdotnetmatrix.aspx for the original source code.
    /// </remarks>
    public sealed class EigenvalueDecomposition
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the eigenvalues as a <see cref="ComplexVector"/>.
        /// </summary>
        /// <returns>
        /// A new ComplexVector that contains the decomposed eigenvalues.
        /// </returns>
        public ComplexVector GetEigenvalues()
        {
            ComplexVector eigenValues = new ComplexVector( size );

            for( int i  = 0; i < size; ++i )
            {
                eigenValues[i] = new Complex( (float)realEigenvalues[i], (float)imagEigenvalues[i] );
            }

            return eigenValues;
        }

        /// <summary>
        /// Gets the block diagonal eigenvalue matrix.
        /// </summary>
        /// <returns>
        /// A new Matrix that contains the decomposed eigenvalues in diagonal form.
        /// </returns>
        public Matrix GetDiagonalEigenvalues()
        {
            Matrix matrix = new Matrix( size, size );

            for( int row = 0; row < size; ++row )
            {
                for( int column = 0; column < size; ++column )
                {
                    matrix[row, column] = 0.0f;
                }

                matrix[row, row] = (float)realEigenvalues[row];

                float imag = (float)imagEigenvalues[row];

                if( imag > 0 )
                {
                    matrix[row, row + 1] = imag;
                }
                else if( imag < 0 )
                {
                    matrix[row, row - 1] = imag;
                }
            }

            return matrix;
        }

        /// <summary>
        /// Gets the eigenvector <see cref="Matrix"/>.
        /// </summary>
        /// <returns>
        /// A new Matrix that contains the decomposed eigenvectors.
        /// </returns>
        public Matrix GetEigenvectorMatrix()
        {
            Matrix matrix = new Matrix( size, size );

            for( int row = 0; row < size; ++row )
            {
                for( int column = 0; column < size; ++column )
                {
                    matrix[row, column] = (float)eigenvectors[row][column];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Gets the real parts of the eigenvalues.
        /// </summary>
        /// <returns>
        /// A new array that contains the decomposed real eigenvalues.
        /// </returns>
        public double[] GetRealEigenvalues()
        {
            return (double[])realEigenvalues.Clone();
        }

        /// <summary>
        /// Getsthe imaginary parts of the eigenvalues.
        /// </summary>
        /// <returns>
        /// A new array that contains the decomposed imaginary eigenvalues.
        /// </returns>
        public double[] GetImagEigenvalues()
        {
            return (double[])imagEigenvalues.Clone();
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="EigenvalueDecomposition"/> class.
        /// </summary>
        /// <param name="matrix">
        /// The square input matrix.
        /// </param>
        public EigenvalueDecomposition( Matrix matrix )
        {
            Contract.Requires<ArgumentNullException>( matrix != null );
            Contract.Requires<ArgumentException>( matrix.IsSquare );

            size = matrix.ColumnCount;
            realEigenvalues = new double[size];
            imagEigenvalues = new double[size];
            eigenvectors = new double[size][];
            for( int i = 0; i < size; ++i )
                eigenvectors[i] = new double[size];

            isSymmetric = true;
            for( int j = 0; (j < size) & isSymmetric; j++ )
            {
                for( int i = 0; (i < size) & isSymmetric; i++ )
                {
                    isSymmetric = (matrix[i, j] == matrix[j, i]);
                }
            }

            if( isSymmetric )
            {
                for( int i = 0; i < size; ++i )
                {
                    for( int j = 0; j < size; ++j )
                    {
                        eigenvectors[i][j] = matrix[i, j];
                    }
                }

                ReduceHouseholderToTridiagonal();
                TridiagonalQL();
            }
            else
            {
                H = new double[size][];
                for( int i2 = 0; i2 < size; ++i2 )
                    H[i2] = new double[size];

                ort = new double[size];

                for( int j = 0; j < size; ++j )
                {
                    for( int i = 0; i < size; ++i )
                    {
                        H[i][j] = matrix[i, j];
                    }
                }

                ReduceToHessenberg();
                ReduceHessenbergToSchur();
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Symmetric Householder reduction to tridiagonal form.
        /// </summary>
        private void ReduceHouseholderToTridiagonal()
        {
            // This is derived from the Algol procedures tred2 by
            // Bowdler, Martin, Reinsch, and Wilkinson, Handbook for
            // Auto. Comp., Vol.ii-Linear Algebra, and the corresponding
            // Fortran subroutine in EISPACK.
            for( int j = 0; j < size; ++j )
            {
                realEigenvalues[j] = eigenvectors[size - 1][j];
            }

            // Householder reduction to tridiagonal form.
            for( int i = size - 1; i > 0; --i )
            {
                // Scale to avoid under/overflow.
                double scale = 0.0;
                double h = 0.0;

                for( int k = 0; k < i; ++k )
                {
                    scale = scale + System.Math.Abs( realEigenvalues[k] );
                }

                if( scale == 0.0 )
                {
                    imagEigenvalues[i] = realEigenvalues[i - 1];
                    for( int j = 0; j < i; j++ )
                    {
                        realEigenvalues[j] = eigenvectors[i - 1][j];
                        eigenvectors[i][j] = 0.0;
                        eigenvectors[j][i] = 0.0;
                    }
                }
                else
                {
                    // Generate Householder vector.
                    for( int k = 0; k < i; ++k )
                    {
                        realEigenvalues[k] /= scale;
                        h += realEigenvalues[k] * realEigenvalues[k];
                    }

                    double f = realEigenvalues[i - 1];
                    double g = System.Math.Sqrt( h );

                    if( f > 0 )
                        g = -g;

                    imagEigenvalues[i] = scale * g;
                    h = h - (f * g);
                    realEigenvalues[i - 1] = f - g;

                    for( int j = 0; j < i; ++j )
                    {
                        imagEigenvalues[j] = 0.0;
                    }

                    // Apply similarity transformation to remaining columns.
                    for( int j = 0; j < i; ++j )
                    {
                        f = realEigenvalues[j];
                        eigenvectors[j][i] = f;
                        g = imagEigenvalues[j] + (eigenvectors[j][j] * f);

                        for( int k = j + 1; k <= i - 1; ++k )
                        {
                            g += eigenvectors[k][j] * realEigenvalues[k];
                            imagEigenvalues[k] += eigenvectors[k][j] * f;
                        }

                        imagEigenvalues[j] = g;
                    }

                    f = 0.0;
                    for( int j = 0; j < i; ++j )
                    {
                        imagEigenvalues[j] /= h;
                        f += imagEigenvalues[j] * realEigenvalues[j];
                    }

                    double hh = f / (h + h);
                    for( int j = 0; j < i; ++j )
                    {
                        imagEigenvalues[j] -= hh * realEigenvalues[j];
                    }

                    for( int j = 0; j < i; ++j )
                    {
                        f = realEigenvalues[j];
                        g = imagEigenvalues[j];
                        for( int k = j; k <= i - 1; ++k )
                        {
                            eigenvectors[k][j] -= ((f * imagEigenvalues[k]) + (g * realEigenvalues[k]));
                        }
                        realEigenvalues[j] = eigenvectors[i - 1][j];
                        eigenvectors[i][j] = 0.0;
                    }
                }
                realEigenvalues[i] = h;
            }

            // Accumulate transformations.
            for( int i = 0; i < size - 1; ++i )
            {
                eigenvectors[size - 1][i] = eigenvectors[i][i];
                eigenvectors[i][i] = 1.0;
                double h = realEigenvalues[i + 1];

                if( h != 0.0 )
                {
                    for( int k = 0; k <= i; ++k )
                    {
                        realEigenvalues[k] = eigenvectors[k][i + 1] / h;
                    }

                    for( int j = 0; j <= i; ++j )
                    {
                        double g = 0.0;
                        for( int k = 0; k <= i; ++k )
                        {
                            g += eigenvectors[k][i + 1] * eigenvectors[k][j];
                        }

                        for( int k = 0; k <= i; ++k )
                        {
                            eigenvectors[k][j] -= g * realEigenvalues[k];
                        }
                    }
                }

                for( int k = 0; k <= i; ++k )
                {
                    eigenvectors[k][i + 1] = 0.0;
                }
            }

            for( int j = 0; j < size; ++j )
            {
                realEigenvalues[j] = eigenvectors[size - 1][j];
                eigenvectors[size - 1][j] = 0.0;
            }

            eigenvectors[size - 1][size - 1] = 1.0;
            imagEigenvalues[0] = 0.0;
        }

        /// <summary>
        /// Symmetric tridiagonal QL algorithm.
        /// </summary>
        private void TridiagonalQL()
        {
            // This is derived from the Algol procedures tql2, by
            // Bowdler, Martin, Reinsch, and Wilkinson, Handbook for
            // Auto. Comp., Vol.ii-Linear Algebra, and the corresponding
            // Fortran subroutine in EISPACK.
            for( int i = 1; i < size; ++i )
            {
                imagEigenvalues[i - 1] = imagEigenvalues[i];
            }

            imagEigenvalues[size - 1] = 0.0;

            double f = 0.0;
            double tst1 = 0.0;
            double eps = System.Math.Pow( 2.0, -52.0 );

            for( int l = 0; l < size; ++l )
            {
                // Find small subdiagonal element
                tst1 = System.Math.Max( tst1, System.Math.Abs( realEigenvalues[l] ) + System.Math.Abs( imagEigenvalues[l] ) );
                int m = l;

                while( m < size )
                {
                    if( System.Math.Abs( imagEigenvalues[m] ) <= eps * tst1 )
                    {
                        break;
                    }
                    ++m;
                }

                // If m == l, d[l] is an eigenvalue,
                // otherwise, iterate.
                if( m > l )
                {
                    int iter = 0;
                    do
                    {
                        iter = iter + 1; // (Could check iteration count here.)

                        // Compute implicit shift.
                        double g = realEigenvalues[l];
                        double p = (realEigenvalues[l + 1] - g) / (2.0 * imagEigenvalues[l]);
                        double r = MathUtilities.Hypotenuse( p, 1.0 );
                        if( p < 0 )
                        {
                            r = -r;
                        }

                        realEigenvalues[l] = imagEigenvalues[l] / (p + r);
                        realEigenvalues[l + 1] = imagEigenvalues[l] * (p + r);

                        double dl1 = realEigenvalues[l + 1];
                        double h = g - realEigenvalues[l];

                        for( int i = l + 2; i < size; ++i )
                        {
                            realEigenvalues[i] -= h;
                        }

                        f = f + h;

                        // Implicit QL transformation.
                        p = realEigenvalues[m];
                        double c = 1.0;
                        double c2 = c;
                        double c3 = c;
                        double el1 = imagEigenvalues[l + 1];
                        double s = 0.0;
                        double s2 = 0.0;

                        for( int i = m - 1; i >= l; --i )
                        {
                            c3 = c2;
                            c2 = c;
                            s2 = s;

                            g = c * imagEigenvalues[i];
                            h = c * p;
                            r = MathUtilities.Hypotenuse( p, imagEigenvalues[i] );

                            imagEigenvalues[i + 1] = s * r;

                            s = imagEigenvalues[i] / r;
                            c = p / r;
                            p = (c * realEigenvalues[i]) - (s * g);

                            realEigenvalues[i + 1] = h + (s * ((c * g) + (s * realEigenvalues[i])));

                            // Accumulate transformation.
                            for( int k = 0; k < size; ++k )
                            {
                                h = eigenvectors[k][i + 1];

                                eigenvectors[k][i + 1] = (s * eigenvectors[k][i]) + (c * h);
                                eigenvectors[k][i]     = (c * eigenvectors[k][i]) - (s * h);
                            }
                        }

                        p = (-s) * s2 * c3 * el1 * imagEigenvalues[l] / dl1;
                        imagEigenvalues[l] = s * p;
                        realEigenvalues[l] = c * p;

                        // Check for convergence.
                    }
                    while( System.Math.Abs( imagEigenvalues[l] ) > eps * tst1 );
                }

                realEigenvalues[l] = realEigenvalues[l] + f;
                imagEigenvalues[l] = 0.0;
            }

            // Sort eigenvalues and corresponding vectors.
            for( int i = 0; i < size - 1; ++i )
            {
                int k = i;
                double p = realEigenvalues[i];

                for( int j = i + 1; j < size; ++j )
                {
                    if( realEigenvalues[j] < p )
                    {
                        k = j;
                        p = realEigenvalues[j];
                    }
                }

                if( k != i )
                {
                    realEigenvalues[k] = realEigenvalues[i];
                    realEigenvalues[i] = p;

                    for( int j = 0; j < size; ++j )
                    {
                        p = eigenvectors[j][i];
                        eigenvectors[j][i] = eigenvectors[j][k];
                        eigenvectors[j][k] = p;
                    }
                }
            }
        }
               
        /// <summary>
        /// Nonsymmetric reduction to Hessenberg form.
        /// </summary>
        private void ReduceToHessenberg()
        {
            // This is derived from the Algol procedures orthes and ortran,
            // by Martin and Wilkinson, Handbook for Auto. Comp.,
            // Vol.ii-Linear Algebra, and the corresponding
            // Fortran subroutines in EISPACK.
            int low = 0;
            int high = size - 1;

            for( int m = low + 1; m <= high - 1; ++m )
            {
                // Scale column.
                double scale = 0.0;
                for( int i = m; i <= high; ++i )
                {
                    scale = scale + System.Math.Abs( H[i][m - 1] );
                }

                if( scale != 0.0 )
                {
                    // Compute Householder transformation.
                    double h = 0.0;
                    for( int i = high; i >= m; --i )
                    {
                        ort[i] = H[i][m - 1] / scale;
                        h += ort[i] * ort[i];
                    }

                    double g = System.Math.Sqrt( h );
                    if( ort[m] > 0 )
                    {
                        g = -g;
                    }

                    h = h - (ort[m] * g);
                    ort[m] = ort[m] - g;

                    // Apply Householder similarity transformation
                    // H = (I-u*u'/h)*H*(I-u*u')/h)
                    for( int j = m; j < size; ++j )
                    {
                        double f = 0.0;
                        for( int i = high; i >= m; --i )
                        {
                            f += ort[i] * H[i][j];
                        }

                        f = f / h;
                        for( int i = m; i <= high; ++i )
                        {
                            H[i][j] -= f * ort[i];
                        }
                    }

                    for( int i = 0; i <= high; ++i )
                    {
                        double f = 0.0;
                        for( int j = high; j >= m; --j )
                        {
                            f += ort[j] * H[i][j];
                        }

                        f = f / h;
                        for( int j = m; j <= high; ++j )
                        {
                            H[i][j] -= f * ort[j];
                        }
                    }

                    ort[m] = scale * ort[m];
                    H[m][m - 1] = scale * g;
                }
            }

            // Accumulate transformations (Algol's ortran).
            for( int i = 0; i < size; ++i )
            {
                for( int j = 0; j < size; ++j )
                {
                    eigenvectors[i][j] = (i == j) ? 1.0 : 0.0;
                }
            }

            for( int m = high - 1; m >= low + 1; --m )
            {
                if( H[m][m - 1] != 0.0 )
                {
                    for( int i = m + 1; i <= high; ++i )
                    {
                        ort[i] = H[i][m - 1];
                    }

                    for( int j = m; j <= high; ++j )
                    {
                        double g = 0.0;
                        for( int i = m; i <= high; ++i )
                        {
                            g += ort[i] * eigenvectors[i][j];
                        }

                        // Double division avoids possible underflow
                        g = (g / ort[m]) / H[m][m - 1];
                        for( int i = m; i <= high; ++i )
                        {
                            eigenvectors[i][j] += g * ort[i];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Stores the result of the last call to <see cref="Divide"/>.
        /// </summary>
        [NonSerialized]
        private double cdivr, cdivi;

        /// <summary>
        /// Complex scalar division, storing the result in the cdivr, cdivi fields.
        /// </summary>
        /// <param name="realA">The real part of the first complex number.</param>
        /// <param name="imagA">The imaginary part of the first complex number.</param>
        /// <param name="realB">The real part of the second complex number.</param>
        /// <param name="imagB">The imaginary part of the second complex number.</param>
        private void Divide( double realA, double imagA, double realB, double imagB )
        {
            if( System.Math.Abs( realB ) > System.Math.Abs( imagB ) )
            {
                double ratio = imagB / realB;
                double denom = realB + (ratio * imagB);

                cdivr = (realA + (ratio * imagA)) / denom;
                cdivi = (imagA - (ratio * realA)) / denom;
            }
            else
            {
                double ratio = realB / imagB;
                double denom = imagB + (ratio * realB);

                cdivr = ((ratio * realA) + imagA) / denom;
                cdivi = ((ratio * imagA) - realA) / denom;
            }
        }

        /// <summary>
        /// Non-symmetric reduction from Hessenberg to real Schur form.
        /// </summary>
        private void ReduceHessenbergToSchur()
        {
            // This is derived from the Algol procedure hqr2,
            // by Martin and Wilkinson, Handbook for Auto. Comp.,
            // Vol.ii-Linear Algebra, and the corresponding
            // Fortran subroutine in EISPACK.

            // Initialize.
            int nn = this.size;

            int low  = 0;
            int high = nn - 1;
            double z = 0;

            // Store roots isolated by balanc and compute matrix norm.
            double norm = 0.0;
            for( int i = 0; i < nn; ++i )
            {
                if( i < low | i > high )
                {
                    realEigenvalues[i] = H[i][i];
                    imagEigenvalues[i] = 0.0;
                }

                for( int j = System.Math.Max( i - 1, 0 ); j < nn; ++j )
                {
                    norm = norm + System.Math.Abs( H[i][j] );
                }
            }

            // Outer loop over eigenvalue index.
            ReduceHessenbergToSchurOuterLoop( norm );

            // Backsubstitute to find vectors of upper triangular form.
            if( norm == 0.0 )
                return;

            ReduceHessenbergToSchurBacksubstitue( norm );

            // Vectors of isolated roots
            for( int i = 0; i < nn; ++i )
            {
                if( i < low | i > high )
                {
                    for( int j = i; j < nn; ++j )
                    {
                        eigenvectors[i][j] = H[i][j];
                    }
                }
            }

            // Back transformation to get eigenvectors of original matrix
            for( int j = nn - 1; j >= low; --j )
            {
                for( int i = low; i <= high; ++i )
                {
                    z = 0.0;

                    for( int k = low; k <= System.Math.Min( j, high ); ++k )
                    {
                        z += eigenvectors[i][k] * H[k][j];
                    }

                    eigenvectors[i][j] = z;
                }
            }
        }

        /// <summary>
        /// The outer loop of the non-symmetric reduction from Hessenberg
        /// to real Schur form function.
        /// </summary>
        /// <param name="norm">
        /// The norm of the Hessenberg matrix.
        /// </param>
        private void ReduceHessenbergToSchurOuterLoop( double norm )
        {
            int nn     = this.size;
            int n      = nn - 1;

            int low    = 0;
            int high   = n;

            double eps = System.Math.Pow( 2.0, -52.0 );
            double exshift = 0.0;

            double p = 0, r = 0, s = 0, q = 0, x = 0, y = 0, z = 0, w = 0;

            int iter = 0;
            while( n >= low )
            {
                // Look for single small sub-diagonal element.
                int l = n;

                while( l > low )
                {
                    s = System.Math.Abs( H[l - 1][l - 1] ) + System.Math.Abs( H[l][l] );

                    if( s == 0.0 )
                    {
                        s = norm;
                    }

                    if( System.Math.Abs( H[l][l - 1] ) < eps * s )
                        break;

                    --l;
                }

                // Check for convergence
                // One root found.
                if( l == n )
                {
                    H[n][n]            = H[n][n] + exshift;
                    realEigenvalues[n] = H[n][n];
                    imagEigenvalues[n] = 0.0;

                    --n;
                    iter = 0;

                    // Two roots found
                }
                else if( l == n - 1 )
                {
                    w = H[n][n - 1] * H[n - 1][n];
                    p = (H[n - 1][n - 1] - H[n][n]) / 2.0;
                    q = (p * p) + w;

                    z = System.Math.Sqrt( System.Math.Abs( q ) );

                    H[n][n]         = H[n][n] + exshift;
                    H[n - 1][n - 1] = H[n - 1][n - 1] + exshift;
                    x               = H[n][n];

                    // Real pair
                    if( q >= 0 )
                    {
                        if( p >= 0 )
                        {
                            z = p + z;
                        }
                        else
                        {
                            z = p - z;
                        }

                        realEigenvalues[n - 1] = x + z;
                        realEigenvalues[n]     = realEigenvalues[n - 1];

                        if( z != 0.0 )
                        {
                            realEigenvalues[n] = x - (w / z);
                        }

                        imagEigenvalues[n - 1] = 0.0;
                        imagEigenvalues[n]     = 0.0;
                        x = H[n][n - 1];

                        s = System.Math.Abs( x ) + System.Math.Abs( z );
                        p = x / s;
                        q = z / s;

                        r = System.Math.Sqrt( (p * p) + (q * q) );
                        p = p / r;
                        q = q / r;

                        // Row modification
                        for( int j = n - 1; j < nn; ++j )
                        {
                            z = H[n - 1][j];

                            H[n - 1][j] = (q * z) + (p * H[n][j]);
                            H[n][j]     = (q * H[n][j]) - (p * z);
                        }

                        // Column modification
                        for( int i = 0; i <= n; ++i )
                        {
                            z = H[i][n - 1];

                            H[i][n - 1] = (q * z) + (p * H[i][n]);
                            H[i][n]     = (q * H[i][n]) - (p * z);
                        }

                        // Accumulate transformations
                        for( int i = low; i <= high; ++i )
                        {
                            z = eigenvectors[i][n - 1];
                            eigenvectors[i][n - 1] = (q * z) + (p * eigenvectors[i][n]);
                            eigenvectors[i][n]     = (q * eigenvectors[i][n]) - (p * z);
                        }

                        // Complex pair
                    }
                    else
                    {
                        realEigenvalues[n - 1] = x + p;
                        realEigenvalues[n]     = x + p;

                        imagEigenvalues[n - 1] = z;
                        imagEigenvalues[n]     = -z;
                    }

                    n = n - 2;
                    iter = 0;

                    // No convergence yet
                }
                else
                {
                    // Form shift
                    x = H[n][n];
                    y = 0.0;
                    w = 0.0;

                    if( l < n )
                    {
                        y = H[n - 1][n - 1];
                        w = H[n][n - 1] * H[n - 1][n];
                    }

                    // Wilkinson's original ad hoc shift
                    if( iter == 10 )
                    {
                        exshift += x;
                        for( int i = low; i <= n; i++ )
                        {
                            H[i][i] -= x;
                        }

                        s = System.Math.Abs( H[n][n - 1] ) + System.Math.Abs( H[n - 1][n - 2] );
                        x = y = 0.75 * s;
                        w = (-0.4375) * s * s;
                    }

                    // MATLAB's new ad hoc shift
                    if( iter == 30 )
                    {
                        s = (y - x) / 2.0;
                        s = (s * s) + w;

                        if( s > 0 )
                        {
                            s = System.Math.Sqrt( s );
                            if( y < x )
                            {
                                s = -s;
                            }

                            s = x - (w / (((y - x) / 2.0) + s));
                            for( int i = low; i <= n; ++i )
                            {
                                H[i][i] -= s;
                            }

                            exshift += s;
                            x = y = w = 0.964;
                        }
                    }

                    iter = iter + 1; // (Could check iteration count here.)

                    // Look for two consecutive small sub-diagonal elements
                    int m = n - 2;

                    while( m >= l )
                    {
                        z = H[m][m];
                        r = x - z;
                        s = y - z;

                        p = (((r * s) - w) / H[m + 1][m]) + H[m][m + 1];
                        q = H[m + 1][m + 1] - z - r - s;
                        r = H[m + 2][m + 1];

                        s = System.Math.Abs( p ) + System.Math.Abs( q ) + System.Math.Abs( r );

                        p = p / s;
                        q = q / s;
                        r = r / s;

                        if( m == l )
                            break;

                        if( System.Math.Abs( H[m][m - 1] ) * (System.Math.Abs( q ) + System.Math.Abs( r )) <
                            eps * (System.Math.Abs( p ) * (System.Math.Abs( H[m - 1][m - 1] ) + System.Math.Abs( z ) + System.Math.Abs( H[m + 1][m + 1] ))) )
                        {
                            break;
                        }
                        --m;
                    }

                    for( int i = m + 2; i <= n; ++i )
                    {
                        H[i][i - 2] = 0.0;
                        if( i > m + 2 )
                        {
                            H[i][i - 3] = 0.0;
                        }
                    }

                    this.DoubleStepQR( nn, n, low, high, ref p, ref r, ref s, ref q, ref x, ref y, ref z, l, m );
                } // check convergence
            } // while (n >= low)
        }

        /// <summary>
        /// Double QR step involving rows l:n and columns m:n.
        /// </summary>
        /// <param name="nn"></param>
        /// <param name="n"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <param name="s"></param>
        /// <param name="q"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="l"></param>
        /// <param name="m"></param>
        private void DoubleStepQR( int nn, int n, int low, int high, ref double p, ref double r, ref double s, ref double q, ref double x, ref double y, ref double z, int l, int m )
        {
            for( int k = m; k <= n - 1; ++k )
            {
                bool isNotLast = (k != n - 1);

                if( k != m )
                {
                    p = H[k][k - 1];
                    q = H[k + 1][k - 1];
                    r = isNotLast ? H[k + 2][k - 1] : 0.0;
                    x = System.Math.Abs( p ) + System.Math.Abs( q ) + System.Math.Abs( r );

                    if( x != 0.0 )
                    {
                        p = p / x;
                        q = q / x;
                        r = r / x;
                    }
                }

                if( x == 0.0 )
                    break;

                s = System.Math.Sqrt( (p * p) + (q * q) + (r * r) );

                if( p < 0 )
                    s = -s;

                if( s != 0 )
                {
                    if( k != m )
                    {
                        H[k][k - 1] = (-s) * x;
                    }
                    else if( l != m )
                    {
                        H[k][k - 1] = -H[k][k - 1];
                    }

                    p = p + s;
                    x = p / s;
                    y = q / s;
                    z = r / s;
                    q = q / p;
                    r = r / p;

                    // Row modification.
                    for( int j = k; j < nn; ++j )
                    {
                        p = H[k][j] + (q * H[k + 1][j]);

                        if( isNotLast )
                        {
                            p = p + (r * H[k + 2][j]);
                            H[k + 2][j] = H[k + 2][j] - (p * z);
                        }

                        H[k][j] = H[k][j] - (p * x);
                        H[k + 1][j] = H[k + 1][j] - (p * y);
                    }

                    // Column modification.
                    for( int i = 0; i <= System.Math.Min( n, k + 3 ); ++i )
                    {
                        p = (x * H[i][k]) + (y * H[i][k + 1]);

                        if( isNotLast )
                        {
                            p = p + (z * H[i][k + 2]);
                            H[i][k + 2] = H[i][k + 2] - (p * r);
                        }

                        H[i][k] = H[i][k] - p;
                        H[i][k + 1] = H[i][k + 1] - (p * q);
                    }

                    // Accumulate transformations.
                    for( int i = low; i <= high; ++i )
                    {
                        p = (x * eigenvectors[i][k]) + (y * eigenvectors[i][k + 1]);

                        if( isNotLast )
                        {
                            p = p + (z * eigenvectors[i][k + 2]);
                            eigenvectors[i][k + 2] = eigenvectors[i][k + 2] - (p * r);
                        }

                        eigenvectors[i][k]     = eigenvectors[i][k] - p;
                        eigenvectors[i][k + 1] = eigenvectors[i][k + 1] - (p * q);
                    }
                } // (s != 0)
            } // k loop
        }

        /// <summary>
        /// Backsubstitute to find vectors of upper triangular form
        /// in the non-symmetric reduction from Hessenberg to real Schur form function.
        /// </summary>
        /// <param name="norm">
        /// The norm of the Hessenberg matrix.
        /// </param>
        private void ReduceHessenbergToSchurBacksubstitue( double norm )
        {
            int nn = this.size;

            double eps = System.Math.Pow( 2.0, -52.0 );
            double p = 0, q = 0, r = 0, s = 0, z = 0, t, w, x, y;

            for( int n = nn - 1; n >= 0; --n )
            {
                p = realEigenvalues[n];
                q = imagEigenvalues[n];

                // Real vector
                if( q == 0 )
                {
                    int l = n;
                    H[n][n] = 1.0;

                    for( int i = n - 1; i >= 0; --i )
                    {
                        w = H[i][i] - p;
                        r = 0.0;

                        for( int j = l; j <= n; ++j )
                        {
                            r = r + (H[i][j] * H[j][n]);
                        }

                        if( imagEigenvalues[i] < 0.0 )
                        {
                            z = w;
                            s = r;
                        }
                        else
                        {
                            l = i;

                            if( imagEigenvalues[i] == 0.0 )
                            {
                                if( w != 0.0 )
                                {
                                    H[i][n] = (-r) / w;
                                }
                                else
                                {
                                    H[i][n] = (-r) / (eps * norm);
                                }

                                // Solve real equations
                            }
                            else
                            {
                                x = H[i][i + 1];
                                y = H[i + 1][i];
                                q = ((realEigenvalues[i] - p) * (realEigenvalues[i] - p)) + (imagEigenvalues[i] * imagEigenvalues[i]);
                                t = ((x * s) - (z * r)) / q;
                                H[i][n] = t;

                                if( System.Math.Abs( x ) > System.Math.Abs( z ) )
                                {
                                    H[i + 1][n] = (-r - (w * t)) / x;
                                }
                                else
                                {
                                    H[i + 1][n] = (-s - (y * t)) / z;
                                }
                            }

                            // Overflow control
                            t = System.Math.Abs( H[i][n] );

                            if( (eps * t) * t > 1 )
                            {
                                for( int j = i; j <= n; ++j )
                                {
                                    H[j][n] = H[j][n] / t;
                                }
                            }
                        }
                    }

                    // Complex vector
                }
                else if( q < 0 )
                {
                    int l = n - 1;

                    // Last vector component imaginary so matrix is triangular
                    if( System.Math.Abs( H[n][n - 1] ) > System.Math.Abs( H[n - 1][n] ) )
                    {
                        H[n - 1][n - 1] = q / H[n][n - 1];
                        H[n - 1][n] = (-(H[n][n] - p)) / H[n][n - 1];
                    }
                    else
                    {
                        Divide( 0.0, -H[n - 1][n], H[n - 1][n - 1] - p, q );
                        H[n - 1][n - 1] = cdivr;
                        H[n - 1][n] = cdivi;
                    }

                    H[n][n - 1] = 0.0;
                    H[n][n] = 1.0;

                    for( int i = n - 2; i >= 0; --i )
                    {
                        double ra = 0.0, sa = 0.0;

                        for( int j = l; j <= n; ++j )
                        {
                            ra = ra + (H[i][j] * H[j][n - 1]);
                            sa = sa + (H[i][j] * H[j][n]);
                        }

                        w = H[i][i] - p;

                        if( imagEigenvalues[i] < 0.0 )
                        {
                            z = w;
                            r = ra;
                            s = sa;
                        }
                        else
                        {
                            l = i;

                            if( imagEigenvalues[i] == 0 )
                            {
                                Divide( -ra, -sa, w, q );

                                H[i][n - 1] = cdivr;
                                H[i][n]     = cdivi;
                            }
                            else
                            {
                                // Solve complex equations
                                x = H[i][i + 1];
                                y = H[i + 1][i];

                                double vr = ((realEigenvalues[i] - p) * (realEigenvalues[i] - p)) + (imagEigenvalues[i] * imagEigenvalues[i]) - (q * q);
                                double vi = (realEigenvalues[i] - p) * 2.0 * q;

                                if( vr == 0.0 & vi == 0.0 )
                                {
                                    vr = eps * norm * (System.Math.Abs( w ) + System.Math.Abs( q ) + System.Math.Abs( x ) + System.Math.Abs( y ) + System.Math.Abs( z ));
                                }

                                Divide( (x * r) - (z * ra) + (q * sa), (x * s) - (z * sa) - (q * ra), vr, vi );
                                H[i][n - 1] = cdivr;
                                H[i][n]     = cdivi;

                                if( System.Math.Abs( x ) > (System.Math.Abs( z ) + System.Math.Abs( q )) )
                                {
                                    H[i + 1][n - 1] = (-ra - (w * H[i][n - 1]) + (q * H[i][n])) / x;
                                    H[i + 1][n]     = (-sa - (w * H[i][n])     - (q * H[i][n - 1])) / x;
                                }
                                else
                                {
                                    Divide( -r - (y * H[i][n - 1]), -s - (y * H[i][n]), z, q );

                                    H[i + 1][n - 1] = cdivr;
                                    H[i + 1][n]     = cdivi;
                                }
                            }

                            // Overflow control
                            t = System.Math.Max( System.Math.Abs( H[i][n - 1] ), System.Math.Abs( H[i][n] ) );

                            if( (eps * t) * t > 1 )
                            {
                                for( int j = i; j <= n; ++j )
                                {
                                    H[j][n - 1] = H[j][n - 1] / t;
                                    H[j][n]     = H[j][n]     / t;
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region	[ Fields ]

        /// <summary>
        /// Row and column dimension (square matrix).
        /// </summary>
        private readonly int size;

        /// <summary>Symmetry flag.</summary>
        private readonly bool isSymmetric;

        /// <summary>
        /// Arrays for internal storage of eigenvalues.
        /// </summary>
        private readonly double[] realEigenvalues, imagEigenvalues;

        /// <summary>
        /// Array for internal storage of eigenvectors.
        /// </summary>
        private readonly double[][] eigenvectors;

        /// <summary>
        /// Array for internal storage of nonsymmetric Hessenberg form.
        /// </summary>
        private readonly double[][] H;

        /// <summary>
        /// Working storage for nonsymmetric algorithm.
        /// </summary>
        private readonly double[] ort;

        #endregion 
    }
}