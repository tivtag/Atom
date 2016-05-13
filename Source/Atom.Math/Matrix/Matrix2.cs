// <copyright file="Matrix2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Matrix2 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary> 
    /// Represents a 2x2 homogenous single-precission floating point Matrix. 
    /// </summary>
    [Serializable]
    public struct Matrix2 : IEquatable<Matrix2>, ICultureSensitiveToStringProvider
    {
        #region [ Constants ]

        /// <summary> 
        /// The identity matrix. (1, 0, 0, 1) This is a readonly field.
        /// </summary>
        public static readonly Matrix2 Identity = new Matrix2(
            1.0f, 0.0f,
            0.0f, 1.0f
        );

        /// <summary> 
        /// A matrix that contains only zeros. (0, 0, 0, 0) This is a readonly field.
        /// </summary>
        public static readonly Matrix2 Zero = new Matrix2(
            0.0f, 0.0f,
            0.0f, 0.0f
        );

        #endregion

        #region [ Fields ]

        /// <summary> 
        /// Element of the first row of the Matrix.
        /// </summary>
        public float M11, M12;

        /// <summary> 
        /// Element of the second row of the Matrix.
        /// </summary>
        public float M21, M22;

        #endregion

        #region [ Properties ]

        #region IsSymmetric

        /// <summary>
        /// Gets a value indicating whether this <see cref="Matrix2"/> is symmetric.
        /// </summary>   
        /// <remarks>
        /// A symmetric matrix is a square matrix, that is equal to its transpose.
        /// </remarks>
        /// <value>
        /// Returns <see langword="true"/> if the Matrix is symmetric; otherwise <see langword="false"/>.
        /// </value>
        public bool IsSymmetric
        {
            get
            {
                for( int i = 0; i < 2; ++i )
                {
                    for( int j = 0; j <= i; ++j )
                    {
                        if( this[i, j].IsApproximate( this[j, i] ) == false )
                            return false;
                    }
                }

                return true;
            }
        }

        #endregion
        
        #region IsSingular

        /// <summary>
        /// Gets a value indicating whether this <see cref="Matrix2"/> is singular/degenerated.
        /// A matrix has no inverse if its singular.
        /// </summary>
        /// <remarks>
        /// A matrix is singular if its determinant is zero.
        /// Another way to test for singularity is to prove that
        /// there exists a vector x (x!= null-vector) where:
        /// Ax=0-vector.
        /// </remarks>
        /// <value>
        /// Returns <see langword="true"/> if the Matrix is singular; otherwise <see langword="false"/>.
        /// </value>
        public bool IsSingular
        {
            get
            {
                return this.Determinant == 0.0f;
            }
        }

        #endregion

        #region Transpose

        /// <summary>
        /// Gets the transpose of this <see cref="Matrix2"/>. 
        /// </summary>
        /// <value>
        /// The transpose of this Matrix. T(T(m)) == m.
        /// </value>
        public Matrix2 Transpose
        {
            get
            {
                return new Matrix2( 
                    M11, M21,
                    M12, M22 
                );
            }
        }

        #endregion

        #region Inverse

        /// <summary> 
        /// Gets the inverse of this <see cref="Matrix2"/>.
        /// </summary>
        /// <value>
        /// Adjoint / Determinant is the Inverse of the matrix. 
        /// A matrix doesn't has an inverse if the determinant is zero.
        /// </value>
        public Matrix2 Inverse
        {
            get
            {
                // Calculate the determinant and then inverse it to improve speed.( * is faster than / )
                float invDet = 1.0f / ((M11 * M22) - (M12 * M21));

                return new Matrix2(
                     M22 * invDet, -M12 * invDet,
                    -M21 * invDet, M11 * invDet
                );
            }
        }

        #endregion

        #region Adjoint

        /// <summary>
        /// Gets the adjoint of this Matrix2.
        /// </summary>
        /// <value>
        /// Adjoint / Determinant is the Inverse of the matrix.
        /// </value>
        public Matrix2 Adjoint
        {
            get
            {
                return new Matrix2( M22, -M12, -M21, M11 );
            }
        }

        #endregion

        #region Determinant

        /// <summary>
        /// Gets the determinant of this <see cref="Matrix2"/>. 
        /// </summary>
        /// <value> 
        /// A Matrix has no inverse if its Determinant is zero.
        /// </value>
        public float Determinant
        {
            get
            {
                return (M11 * M22) - (M12 * M21);
            }
        }

        #endregion

        #region Angle

        /// <summary>
        /// Gets the angle of this Matrix2, starting from the <see cref="Matrix2.Identity"/>.
        /// </summary>
        /// <value>The angle in radians.</value>
        public float Angle
        {
            get
            {
                // return Acos( M11 / Sqrt( M11 * M11 + M21 * M21 ) );
                // faster than above:
                return (float)System.Math.Atan2( M21, M11 ); 
            }
        }

        #endregion

        #region Reflection

        /// <summary> 
        /// Gets the reflection of this <see cref="Matrix2"/> through the Y-axis. 
        /// </summary>
        /// <value>The reflection of this <see cref="Matrix2"/> through the Y-axis.</value>
        public Matrix2 ReflectionY
        {
            get
            {
                Matrix2 reflectionMatrix = new Matrix2(
                    -1.0f, 0.0f,
                    0.0f, 1.0f
                );

                return this * reflectionMatrix;
            }
        }

        /// <summary> 
        /// Gets the reflection of this <see cref="Matrix2"/> through the X-axis. 
        /// </summary>
        /// <value>The reflection of this <see cref="Matrix2"/> through the X-axis.</value>
        public Matrix2 ReflectionX
        {
            get
            {
                Matrix2 reflectionMatrix = new Matrix2(
                    1.0f, 0.0f,
                    0.0f, -1.0f 
                );

                return this * reflectionMatrix;
            }
        }

        /// <summary> 
        /// Gets the reflection of this <see cref="Matrix2"/> through the (Y+X)-axis. 
        /// </summary>
        /// <value>The reflection of this <see cref="Matrix2"/> through the (Y+X)-axis.</value>
        public Matrix2 ReflectionYX
        {
            get
            {
                Matrix2 reflectionMatrix = new Matrix2(
                    0.0f, 1.0f,
                    1.0f, 0.0f
                );

                return this * reflectionMatrix;
            }
        }

        /// <summary> 
        /// Gets the reflection of this <see cref="Matrix2"/> through the (Y-X)-axis. 
        /// </summary>
        /// <value>The reflection of this <see cref="Matrix2"/> through the (Y-X)-axis.</value>
        public Matrix2 ReflectionYMinusX
        {
            get
            {
                Matrix2 reflectionMatrix = new Matrix2(
                    0.0f, -1.0f,
                    -1.0f, 0.0f 
                );

                return this * reflectionMatrix;
            }
        }

        #endregion

        #region Trace

        /// <summary>
        /// Gets the trace of this <see cref="Matrix2"/>;
        /// which is the sum of its diagonal elements.
        /// </summary>
        /// <value>The trace value of this Matrix.</value>
        public float Trace
        {
            get
            {
                return M11 + M22;
            }
        }

        #endregion

        #region Indexer

        #region this[int index]

        /// <summary>
        /// Gets or sets the element of this <see cref="Matrix2"/> at the given index. (M11=0, M12=1, M21=2, M22=3).
        /// </summary>
        /// <param name="index">The zero-based index.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> 
        /// If the <paramref name="index"/> is invalid.
        /// </exception>
        public float this[int index]
        {
            get
            {
                switch( index )
                {
                    case 0:
                        return M11;
                    case 1:
                        return M12;
                    case 2:
                        return M21;
                    case 3:
                        return M22;

                    default:
                        throw new System.ArgumentOutOfRangeException(
                            "index", index, Atom.ErrorStrings.SpecifiedIndexIsInvalid );
                }
            }

            set
            {
                switch( index )
                {
                    case 0:
                        M11 = value;
                        break;
                    case 1:
                        M12 = value;
                        break;
                    case 2:
                        M21 = value;
                        break;
                    case 3:
                        M22 = value;
                        break;

                    default:
                        throw new System.ArgumentOutOfRangeException(
                            "index",
                            index, 
                            Atom.ErrorStrings.SpecifiedIndexIsInvalid
                        );
                }
            }
        }

        #endregion

        #region this[int row, int column]

        /// <summary>
        /// Gets or sets the matrix element in the specified <paramref name="row"/> and <paramref name="column"/>.
        /// </summary>
        /// <param name="row">The zero-based row index.</param>
        /// <param name="column">The zero-based column index.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> 
        /// If the <paramref name="row"/> or <paramref name="column"/> is invalid.
        /// </exception>
        public float this[int row, int column]
        {
            get
            {
                switch( row )
                {
                    case 0:
                        switch( column )
                        {
                            case 0:
                                return M11;
                            case 1:
                                return M12;

                            default:
                                throw new ArgumentOutOfRangeException( 
                                    "column", 
                                    column, 
                                    Atom.ErrorStrings.SpecifiedIndexIsOutOfValidRange
                                );
                        }

                    case 1:
                        switch( column )
                        {
                            case 0:
                                return M21;
                            case 1:
                                return M22;

                            default:
                                throw new ArgumentOutOfRangeException( "column", column, Atom.ErrorStrings.SpecifiedIndexIsOutOfValidRange );
                        }

                    default:
                        throw new ArgumentOutOfRangeException( "row", row, Atom.ErrorStrings.SpecifiedIndexIsOutOfValidRange );
                }
            }

            set
            {
                switch( row )
                {
                    case 0:
                        switch( column )
                        {
                            case 0:
                                M11 = value;
                                break;
                            case 1:
                                M12 = value;
                                break;

                            default:
                                throw new ArgumentOutOfRangeException( "column", column, Atom.ErrorStrings.SpecifiedIndexIsOutOfValidRange );
                        }
                        break;

                    case 1:
                        switch( column )
                        {
                            case 0:
                                M21 = value;
                                break;
                            case 1:
                                M22 = value;
                                break;

                            default:
                                throw new ArgumentOutOfRangeException( "column", column, Atom.ErrorStrings.SpecifiedIndexIsOutOfValidRange );
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException( "row", row, Atom.ErrorStrings.SpecifiedIndexIsOutOfValidRange );
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix2"/> struct.
        /// </summary>
        /// <param name="m11">
        /// The X-coordiante of the x-axis.
        /// </param>
        /// <param name="m12">
        /// The Y-coordiante of the x-axis.
        /// </param>
        /// <param name="m21">
        /// The X-coordiante of the y-axis.
        /// </param>
        /// <param name="m22">
        /// The Y-coordiante of the y-axis.
        /// </param>
        public Matrix2( float m11, float m12, float m21, float m22 )
        {
            this.M11 = m11;
            this.M12 = m12;
            this.M21 = m21;
            this.M22 = m22;
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="Matrix2"/> struct. 
        /// </summary>
        /// <param name="axisX"> The x-axis of the new <see cref="Matrix2"/> (m00, m10). </param>
        /// <param name="axisY"> The y-axis of the new <see cref="Matrix2"/> (m01, m11). </param>
        public Matrix2( Vector2 axisX, Vector2 axisY )
        {
            this.M11 = axisX.X;
            this.M12 = axisY.X;

            this.M21 = axisX.Y;
            this.M22 = axisY.Y;
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="Matrix2"/> struct;
        /// and copies the elements from the given <see cref="Matrix2"/> into the new one.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix2"/> to copy. </param>
        public Matrix2( Matrix2 matrix )
        {
            this.M11 = matrix.M11;
            this.M12 = matrix.M12;
            this.M21 = matrix.M21;
            this.M22 = matrix.M22;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix2"/> struct.
        /// </summary>
        /// <param name="elements">
        /// The elements of the new Matrix.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="elements"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the length of the specified <paramref name="elements"/> array is less than 4.
        /// </exception>
        public Matrix2( float[] elements )
        {
            if( elements == null )
                throw new ArgumentNullException( "elements" );

            if( elements.Length < 4 )
                throw new ArgumentException( Atom.ErrorStrings.ArrayLengthOutOfValidRange, "elements" );

            this.M11 = elements[0];
            this.M12 = elements[1];
            this.M21 = elements[2];
            this.M22 = elements[3];
        }

        #endregion

        #region [ Methods ]
        
        /// <summary>
        /// Gets the row at the specified <paramref name="rowIndex"/>.
        /// </summary>
        /// <param name="rowIndex">
        /// The index of the row to get.
        /// </param>
        /// <returns> An array contain the row of the specifie index. </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="rowIndex"/> is not 0 or 1.
        /// </exception>
        [Pure]
        public float[] GetRow( int rowIndex )
        {
            Contract.Ensures( Contract.Result<float[]>() != null );

            switch( rowIndex )
            {
                case 0:
                    return new float[2] { M11, M12 };
                case 1:
                    return new float[2] { M21, M22 };

                default:
                    throw new ArgumentOutOfRangeException( 
                        "rowIndex",
                        rowIndex,
                        Atom.ErrorStrings.SpecifiedIndexIsOutOfValidRange
                    );
            }
        }

        /// <summary>
        /// Gets the column at the specified <paramref name="columnIndex"/>.
        /// </summary>
        /// <param name="columnIndex"> The index of the column to get. </param>
        /// <returns> An array contain the column of the specifie index. </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="columnIndex"/> is not 0 or 1.
        /// </exception>
        public float[] GetColumn( int columnIndex )
        {
            Contract.Ensures( Contract.Result<float[]>() != null );

            switch( columnIndex )
            {
                case 0:
                    return new float[2] { M11, M21 };
                case 1:
                    return new float[2] { M12, M22 };

                default:
                    throw new ArgumentOutOfRangeException(                        
                        "columnIndex", 
                        columnIndex,
                        Atom.ErrorStrings.SpecifiedIndexIsOutOfValidRange
                    );
            }
        }            

        #region > Operations <

        #region OrthoNormalize

        /// <summary>
        /// Orthogonal normalization algorithm. (Gram-Schmidt)
        /// Can be used to regenerate a degenerated orthogonal matrix. Such degeneration
        /// can happen because of floating point errors.
        /// </summary>
        /// <remarks>
        /// If the matrix is M = [m0|left], then orthonormal output matrix is Q = [q0|q1].
        /// </remarks>
        public void OrthoNormalize()
        {
            // Compute q0:
            float invLen =  1.0f / (float)System.Math.Sqrt( (M11 * M11) + (M21 * M21) );
            M11 *= invLen;
            M21 *= invLen;

            // Compute q1:
            float dot = (M11 * M12) + (M21 * M22);
            M12 -= M11 * dot;
            M22 -= M21 * dot;

            invLen = 1.0f / (float)System.Math.Sqrt( (M12 * M12) + (M22 * M22) );
            M12 *= invLen;
            M22 *= invLen;
        }

        #endregion

        #region TransposeTimes

        /// <summary> TransposeTimes method. </summary>
        /// <remarks> P = matrix^VertexData*B, P[r][c] = sum_m matrix[Rows][r]*B[Rows][c]</remarks>
        /// <param name="left">The matrix on the left side of the equation.</param>
        /// <param name="right">The matrix on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix2 TransposeTimes( Matrix2 left, Matrix2 right )
        {
            Matrix2 result = new Matrix2();

            for( int row = 0; row < 2; ++row )
            {
                for( int column = 0; column < 2; ++column )
                {
                    int row2  = row * 2;
                    int index = column + row2;

                    result[index] = 0.0f;

                    for( int mid = 0; mid < 2; ++mid )
                    {
                        result[index] += left[mid + row2] * right[mid + (column * 2)];
                    }
                }
            }

            return result;
        }

        #endregion

        #region TimesTranspose

        /// <summary> TimesTranspose method. </summary>
        /// <remarks> P = matrix*B^VertexData, P[r][c] = sum_m matrix[r][Rows]*B[c][Rows]</remarks>
        /// <param name="left">The matrix on the left side of the equation.</param>
        /// <param name="right">The matrix on the right side of the equation.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix2 TimesTranspose( Matrix2 left, Matrix2 right )
        {
            Matrix2 result = new Matrix2();

            for( int row = 0; row < 2; ++row )
            {
                for( int column = 0; column < 2; ++column )
                {
                    int index = column + (2 * row);

                    result[index] = 0.0f;
                    for( int mid = 0; mid < 2; ++mid )
                    {
                        result[index] += left[(mid * 2) + row] * right[(mid * 2) + column];
                    }
                }
            }

            return result;
        }

        #endregion

        #region Tensor

        /// <summary>
        /// Returns the tensor product (also called outer product) of the given 2x2 matrices. </summary>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Tensor_product for more information.
        /// </remarks>
        /// <param name="left">The first input matrix.</param>
        /// <param name="right">The second input matrix.</param>
        /// <returns> The result of the operation. </returns>
        public static Matrix4 Tensor( Matrix2 left, Matrix2 right )
        {
            return new Matrix4(
                (left.M11 * right.M11), (left.M11 * right.M12), (left.M12 * right.M11), (left.M12 * right.M12),
                (left.M11 * right.M21), (left.M11 * right.M22), (left.M12 * right.M21), (left.M12 * right.M22),
                (left.M21 * right.M11), (left.M21 * right.M12), (left.M22 * right.M11), (left.M22 * right.M12),
                (left.M21 * right.M21), (left.M21 * right.M22), (left.M22 * right.M21), (left.M22 * right.M22)
            );
        }

        /// <summary>
        /// Returns the tensor product (also called outer product) of the given 2x2 matrices. </summary>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Tensor_product for more information.
        /// </remarks>
        /// <param name="left">The first input matrix. This value will not be modified by this method.</param>
        /// <param name="right">The second input matrix. This value will not be modified by this method.</param>
        /// <param name="result"> Will contain the result of the operation. </param>
        public static void Tensor( ref Matrix2 left, ref Matrix2 right, out Matrix4 result )
        {
            result = new Matrix4(
                (left.M11 * right.M11), (left.M11 * right.M12), (left.M12 * right.M11), (left.M12 * right.M12),
                (left.M11 * right.M21), (left.M11 * right.M22), (left.M12 * right.M21), (left.M12 * right.M22),
                (left.M21 * right.M11), (left.M21 * right.M12), (left.M22 * right.M11), (left.M22 * right.M12),
                (left.M21 * right.M21), (left.M21 * right.M22), (left.M22 * right.M21), (left.M22 * right.M22)
            );
        }

        #endregion

        #region - Eigen Related -
        
        /// <summary>
        /// Decomposites the specified <paramref name="matrix"/> into
        /// a <paramref name="rotation"/> matrix and a <paramref name="diagonal"/> matrix.
        /// </summary>
        /// <param name="matrix">
        /// The matrix to decompose.
        /// </param>
        /// <param name="rotation">
        /// Will contain the rotational portion of the <see cref="Matrix2"/>.
        /// </param>
        /// <param name="diagonal">
        /// Will contain the diagonal portion of the <see cref="Matrix2"/>.
        /// </param>
        public static void EigenDecomposition( Matrix2 matrix, out Matrix2 rotation, out Matrix2 diagonal )
        {
            float absTrace = System.Math.Abs( matrix.M11 ) + System.Math.Abs( matrix.M22 );

            if( (System.Math.Abs( matrix.M12 ) + absTrace).IsApproximate( absTrace ) )
            {
                rotation = Matrix2.Identity;
                diagonal = new Matrix2(
                    matrix.M11, 0.0f,
                    0.0f, matrix.M22
                );

                return;
            }

            float trace = matrix.Trace;
            float diff  = matrix.M11 - matrix.M22;

            float discr = (float)System.Math.Sqrt( (diff * diff) + (4.0f * matrix.M12 * matrix.M12) );

            float eigenValueA = 0.5f * (trace - discr);
            float eigenValueB = 0.5f * (trace + discr);

            diagonal = new Matrix2( 
                eigenValueA, 0.0f,
                0.0f, eigenValueB
            );

            // Find cosin and sinus.
            float cos, sin;

            if( diff >= 0.0f )
            {
                cos = matrix.M12;
                sin = eigenValueA - matrix.M11;
            }
            else
            {
                cos = eigenValueA - matrix.M22;
                sin = matrix.M12;
            }

            // Normalize.
            float invLength = 1.0f / (float)System.Math.Sqrt( (cos * cos) + (sin * sin) );
            cos *= invLength;
            sin *= invLength;

            rotation = new Matrix2(
                cos, -sin,
                sin, cos
            );
        }

        #region GetEigenValues

        /// <summary>
        /// Finds the eigen values of this <see cref="Matrix2"/>.
        /// </summary>
        /// <param name="lambdaA">
        /// Will contain the first eigen value.
        /// </param>
        /// <param name="lambdaB">
        /// Will contain the second eigen value.
        /// </param>
        public void GetEigenValues( out Complex lambdaA, out Complex lambdaB )
        {
            /*
                We are after the zeros of the characteristic polynom:
                det( A - yI ) = 0;

                where A is 'this' matrix, 
                and   I is the Identity matrix,
                and   y is lambda.
             
             * We multiply the equation by (-1):             
                det( yI - A ) = 0;

             * yI - A:
                |y - m11, -m12|
                |-m21, y - m22|

             * det( yI - A ) = 0:
                (y - m11)(y - m22) - (-m12 * -m21) = 0
                        P1               Q2
              
             * (y^2 - y*m22 - y*m11 + m11*m22) - (-m12 * -m21) = 0
             
             * Now we got the equation we have to solve:
                    y^2 - y(m22 - m11) + (m11*m22 - (-m12*-m21)) = 0                      
            */

            Complex constant = new Complex( (M11 + M22), 0.0f );
            Complex factor   = new Complex( ((M11 - M22) * (M11 - M22)) + (4 * M12 * M21), 0.0f );
            Complex variable = factor.SquareRoot;

            // Find zeroes of characteristic polynom:
            lambdaA = (constant + variable) / 2.0f;
            lambdaB = (constant - variable) / 2.0f;
        }

        #endregion

        #region GetEigenVectors

        /// <summary>
        /// Receives the eigenvectors of this <see cref="Matrix2"/>.
        /// </summary>
        /// <param name="eigenVectorA">
        /// Will contain the first eigenvector of this <see cref="Matrix2"/>.
        /// </param>
        /// <param name="eigenVectorB">
        /// Will contain the second eigenvector of this <see cref="Matrix2"/>.
        /// </param>
        public void GetEigenVectors( out ComplexVector2 eigenVectorA, out ComplexVector2 eigenVectorB )
        {
            Complex eigenValueA, eigenValueB;
            this.GetEigenValues( out eigenValueA, out eigenValueB );

            this.GetEigenVectors( eigenValueA, eigenValueB, out eigenVectorA, out eigenVectorB );
        }

        /// <summary>
        /// Receives the eigenvectors of this <see cref="Matrix2"/>.
        /// </summary>
        /// <param name="eigenValueA">
        /// The first pre-computed eigenvalue of this <see cref="Matrix2"/>.
        /// </param>
        /// <param name="eigenValueB">
        /// The second pre-computed eigenvalue of this <see cref="Matrix2"/>.
        /// </param>
        /// <param name="eigenVectorA">
        /// Will contain the first eigenvector of this <see cref="Matrix2"/>.
        /// </param>
        /// <param name="eigenVectorB">
        /// Will contain the second eigenvector of this <see cref="Matrix2"/>.
        /// </param>
        public void GetEigenVectors( Complex eigenValueA, Complex eigenValueB, out ComplexVector2 eigenVectorA, out ComplexVector2 eigenVectorB )
        {
            /*
             * The general equation to find the eigen vectors is:
                 (A - lambda*I) [x y] = [0 0];
              
               In 2D there is an easier way.
             */

            if( this.M21 != 0.0f )
            {
                eigenVectorA.X = eigenValueA - this.M22;
                eigenVectorA.Y = this.M21;

                eigenVectorB.X = eigenValueB - this.M22;
                eigenVectorB.Y = this.M21;
            }
            else if( this.M12 != 0.0f )
            {
                eigenVectorA.X = this.M12;
                eigenVectorA.Y = eigenValueB - this.M11;

                eigenVectorB.X = this.M12;
                eigenVectorB.Y = eigenValueB - this.M11;
            }
            else
            {
                eigenVectorA.X = 1.0f;
                eigenVectorA.Y = 0.0f;

                eigenVectorB.X = 0.0f;
                eigenVectorB.Y = 1.0f;
            }
        }

        #endregion

        #endregion

        #endregion

        #region > Operators <

        #region Add

        /// <summary>
        /// Returns the result of adding the right <see cref="Matrix2"/> to the left <see cref="Matrix2"/>. 
        /// </summary>
        /// <param name="left"> The <see cref="Matrix2"/> on the left side of the equation. </param>
        /// <param name="right"> The <see cref="Matrix2"/> on the right side of the equation. </param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix2 Add( Matrix2 left, Matrix2 right )
        {
            Matrix2 result;

            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the right <see cref="Matrix2"/> to the left <see cref="Matrix2"/>
        /// in the given <paramref name="result"/> variable.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix2"/> on the left side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="right">
        /// The <see cref="Matrix2"/> on the right side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Add( ref Matrix2 left, ref Matrix2 right, out Matrix2 result )
        {
            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Returns the result of subtracting the right <see cref="Matrix2"/> from the left <see cref="Matrix2"/>. 
        /// </summary>
        /// <param name="left"> The <see cref="Matrix2"/> on the left side of the equation. </param>
        /// <param name="right"> The <see cref="Matrix2"/> on the right side of the equation. </param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix2 Subtract( Matrix2 left, Matrix2 right )
        {
            Matrix2 result;

            result.M11 = left.M11 - right.M11;
            result.M12 = left.M12 - right.M12;
            result.M21 = left.M21 - right.M21;
            result.M22 = left.M22 - right.M22;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the right <see cref="Matrix2"/> from the left <see cref="Matrix2"/>
        /// in the given <paramref name="result"/> variable.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix2"/> on the left side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="right">
        /// The <see cref="Matrix2"/> on the right side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Subtract( ref Matrix2 left, ref Matrix2 right, out Matrix2 result )
        {
            result.M11 = left.M11 - right.M11;
            result.M12 = left.M12 - right.M12;
            result.M21 = left.M21 - right.M21;
            result.M22 = left.M22 - right.M22;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Returns the result of multiplying the left <see cref="Matrix2"/> by the right <see cref="Matrix2"/>. 
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix2"/> on the left side of the equation.
        /// </param>
        /// <param name="right">
        /// The <see cref="Matrix2"/> on the right side of the equation.
        /// </param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix2 Multiply( Matrix2 left, Matrix2 right )
        {
            Matrix2 result;

            result.M11 = (left.M11 * right.M11) + (left.M12 * right.M21);
            result.M21 = (left.M21 * right.M11) + (left.M22 * right.M21);
            result.M12 = (left.M11 * right.M12) + (left.M12 * right.M22);
            result.M22 = (left.M21 * right.M12) + (left.M22 * right.M22);

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the left <see cref="Matrix2"/> by the right <see cref="Matrix2"/>
        /// in the given <paramref name="result"/> variable.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Matrix2"/> on the left side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="right">
        /// The <see cref="Matrix2"/> on the right side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="result">Will contain the result of the operation.</param>
        public static void Multiply( ref Matrix2 left, ref Matrix2 right, out Matrix2 result )
        {
            result.M11 = (left.M11 * right.M11) + (left.M12 * right.M21);
            result.M21 = (left.M21 * right.M11) + (left.M22 * right.M21);
            result.M12 = (left.M11 * right.M12) + (left.M12 * right.M22);
            result.M22 = (left.M21 * right.M12) + (left.M22 * right.M22);
        }

        /// <summary>
        /// Returns the result of multiplying the given <see cref="Matrix2"/> by the given <paramref name="scalar"/>. 
        /// </summary>
        /// <param name="matrix">
        /// The <see cref="Matrix2"/> on the left side of the equation.
        /// </param>
        /// <param name="scalar">
        /// The scalar on the right side of the equation.
        /// </param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix2 Multiply( Matrix2 matrix, float scalar )
        {
            Matrix2 result;

            result.M11 = matrix.M11 * scalar;
            result.M21 = matrix.M21 * scalar;
            result.M12 = matrix.M12 * scalar;
            result.M22 = matrix.M22 * scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the given <see cref="Matrix2"/> by the given <paramref name="scalar"/>
        /// in the given <paramref name="result"/> variable. 
        /// </summary>
        /// <param name="matrix">
        /// The <see cref="Matrix2"/> on the left side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="scalar">
        /// The scalar on the right side of the equation.
        /// </param>
        /// <param name="result">
        /// Will contain the result of the operation.
        /// </param>
        public static void Multiply( ref Matrix2 matrix, float scalar, out Matrix2 result )
        {
            result.M11 = matrix.M11 * scalar;
            result.M21 = matrix.M21 * scalar;
            result.M12 = matrix.M12 * scalar;
            result.M22 = matrix.M22 * scalar;
        }
        
        /// <summary>
        /// Returns the result of multiplying the given <see cref="Matrix2"/> by the given <paramref name="scalar"/>. 
        /// </summary>
        /// <param name="scalar">
        /// The scalar on the left side of the equation.
        /// </param>
        /// <param name="matrix">
        /// The <see cref="Matrix2"/> on the right side of the equation.
        /// </param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix2 Multiply( float scalar, Matrix2 matrix )
        {
            Matrix2 result;

            result.M11 = matrix.M11 * scalar;
            result.M21 = matrix.M21 * scalar;
            result.M12 = matrix.M12 * scalar;
            result.M22 = matrix.M22 * scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the given <see cref="Matrix2"/> by the given <paramref name="scalar"/>
        /// in the given <paramref name="result"/> variable. 
        /// </summary>
        /// <param name="scalar">
        /// The scalar on the left side of the equation.
        /// </param>
        /// <param name="matrix">
        /// The <see cref="Matrix2"/> on the right side of the equation. This value will not be modified by this method.
        /// </param>
        /// <param name="result">
        /// Will contain the result of the operation.
        /// </param>
        public static void Multiply( float scalar, ref Matrix2 matrix, out Matrix2 result )
        {
            result.M11 = matrix.M11 * scalar;
            result.M21 = matrix.M21 * scalar;
            result.M12 = matrix.M12 * scalar;
            result.M22 = matrix.M22 * scalar;
        }

        #endregion

        #endregion

        #region > Creation Helpers <

        #region FromAngle

        /// <summary>
        /// Creates a new <see cref="Matrix2"/> the represents the specified <paramref name="angle"/>. 
        /// </summary>
        /// <param name="angle"> 
        /// The angle of the new <see cref="Matrix2"/> in radians.
        /// </param>
        /// <returns>The newly created Matrix2.</returns>
        public static Matrix2 FromAngle( float angle )
        {
            float cos = (float)System.Math.Cos( angle );
            float sin = (float)System.Math.Sin( angle );

            return new Matrix2( 
                cos, sin,
                -sin, cos 
            );
        }

        #endregion

        #region CreateOrthographicProjection

        /// <summary>
        /// Creates an orthopgrahic projection matrix.
        /// </summary>
        /// <param name="normal">
        /// The normal of the plane to project onto.
        /// </param>
        /// <returns>
        /// A new <see cref="Matrix2"/> instance.
        /// </returns>
        public static Matrix2 CreateOrthographicProjection( Vector2 normal )
        {
            float xx = normal.X * normal.X;
            float xy = normal.X * normal.Y;
            float yy = normal.Y * normal.Y;

            return new Matrix2(
                1.0f - xx, -xy,
                -xy, 1.0f - yy );
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether the specified <see cref="Object"/>
        /// is equal to this <see cref="Matrix2"/> instance.
        /// </summary>
        /// <param name="obj">The object to test against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public override bool Equals( object obj )
        {
            if( obj == null )
                return false;

            if( obj is Matrix2 )
                return this.Equals( (Matrix2)obj );

            return false;
        }

        /// <summary>
        /// Returns whether the specified <see cref="Matrix2"/> instance
        /// is equal to this <see cref="Matrix2"/> instance.
        /// </summary>
        /// <param name="other">The object to test against.</param>
        /// <returns>true if they are equal; otherwise false.</returns>
        public bool Equals( Matrix2 other )
        {
            return this.M11.IsApproximate( other.M11 ) && this.M12.IsApproximate( other.M12 ) &&
                   this.M21.IsApproximate( other.M21 ) && this.M22.IsApproximate( other.M22 );
        }

        #endregion

        #region ToString

        /// <summary> 
        /// Returns a human-readable representation of this <see cref="Matrix2"/>.
        /// </summary>
        /// <returns> 
        /// A string representation of this <see cref="Matrix2"/>.
        /// </returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary> 
        /// Returns a human-readable representation of this <see cref="Matrix2"/>.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns> 
        /// A string representation of this <see cref="Matrix2"/>.
        /// </returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendFormat( formatProvider, "|{0} {1}|\n", M11.ToString( formatProvider ), M12.ToString( formatProvider ) );
            sb.AppendFormat( formatProvider, "|{0} {1}|\n", M21.ToString( formatProvider ), M22.ToString( formatProvider ) );

            return sb.ToString();
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets the hash code of this <see cref="Matrix2"/> instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.M11 );
            hashBuilder.AppendStruct( this.M12 );
            hashBuilder.AppendStruct( this.M21 );
            hashBuilder.AppendStruct( this.M22 );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        #region *

        #region Matrix2 * Matrix2

        /// <summary>
        /// Returns the result of multiplying the left <see cref="Matrix2"/> by the right <see cref="Matrix2"/>. 
        /// </summary>
        /// <param name="left"> The <see cref="Matrix2"/> on the left side of the equation. </param>
        /// <param name="right"> The <see cref="Matrix2"/> on the right side of the equation. </param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix2 operator *( Matrix2 left, Matrix2 right )
        {
            Matrix2 result;

            result.M11 = (left.M11 * right.M11) + (left.M12 * right.M21);
            result.M21 = (left.M21 * right.M11) + (left.M22 * right.M21);
            result.M12 = (left.M11 * right.M12) + (left.M12 * right.M22);
            result.M22 = (left.M21 * right.M12) + (left.M22 * right.M22);

            return result;
        }

        #endregion

        #region Matrix2 * float

        /// <summary>
        /// Returns the result of multiplying the given <see cref="Matrix2"/> by the given <paramref name="scalar"/>. 
        /// </summary>
        /// <param name="matrix">
        /// The <see cref="Matrix2"/> on the left side of the equation.
        /// </param>
        /// <param name="scalar">
        /// The scalar on the right side of the equation.
        /// </param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix2 operator *( Matrix2 matrix, float scalar )
        {
            Matrix2 result;

            result.M11 = matrix.M11 * scalar;
            result.M21 = matrix.M21 * scalar;
            result.M12 = matrix.M12 * scalar;
            result.M22 = matrix.M22 * scalar;

            return result;
        }

        #endregion

        #region float * Matrix2

        /// <summary>
        /// Returns the result of multiplying the given <see cref="Matrix2"/> by the given <paramref name="scalar"/>. 
        /// </summary>
        /// <param name="scalar"> The scalar on the left side of the equation. </param>
        /// <param name="matrix"> The <see cref="Matrix2"/> on the right side of the equation. </param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix2 operator *( float scalar, Matrix2 matrix )
        {
            Matrix2 result;

            result.M11 = matrix.M11 * scalar;
            result.M21 = matrix.M21 * scalar;
            result.M12 = matrix.M12 * scalar;
            result.M22 = matrix.M22 * scalar;

            return result;
        }

        #endregion

        #region Vector2 * Matrix2

        /// <summary>
        /// Transforms a Vector2 by the given Matrix2.
        /// </summary>
        /// <param name="vector">The source Vector2.</param>
        /// <param name="matrix">The transformation Matrix2.</param>
        /// <returns>The Vector2 resulting from the transformation.</returns>
        public static Vector2 operator *( Vector2 vector, Matrix2 matrix )
        {
            Vector2 result;

            result.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21);
            result.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22);

            return result;
        }

        #endregion

        #region Matrix2* Vector2

        /// <summary>
        /// Transforms a Vector2 by the given Matrix2.
        /// </summary>
        /// <param name="matrix">The transformation Matrix2.</param>
        /// <param name="vector">The source Vector2.</param>
        /// <returns>The Vector2 resulting from the transformation.</returns>
        public static Vector2 operator *( Matrix2 matrix, Vector2 vector )
        {
            Vector2 result;
            result.X = (vector.X * matrix.M11) + (vector.Y * matrix.M21);
            result.Y = (vector.X * matrix.M12) + (vector.Y * matrix.M22);

            return result;
        }

        #endregion

        #endregion

        #region +

        #region Matrix2 + Matrix2

        /// <summary>
        /// Returns the result of adding the right <see cref="Matrix2"/> to the left <see cref="Matrix2"/>. 
        /// </summary>
        /// <param name="left"> The <see cref="Matrix2"/> on the left side of the equation. </param>
        /// <param name="right"> The <see cref="Matrix2"/> on the right side of the equation. </param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix2 operator +( Matrix2 left, Matrix2 right )
        {
            Matrix2 result;

            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;

            return result;
        }

        #endregion

        #endregion

        #region -

        #region Matrix2 - Matrix2

        /// <summary>
        /// Returns the result of subtracting the right <see cref="Matrix2"/> from the left <see cref="Matrix2"/>. 
        /// </summary>
        /// <param name="left"> The <see cref="Matrix2"/> on the left side of the equation. </param>
        /// <param name="right"> The <see cref="Matrix2"/> on the right side of the equation. </param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix2 operator -( Matrix2 left, Matrix2 right )
        {
            Matrix2 result;

            result.M11 = left.M11 - right.M11;
            result.M12 = left.M12 - right.M12;
            result.M21 = left.M21 - right.M21;
            result.M22 = left.M22 - right.M22;

            return result;
        }

        #endregion

        #endregion

        #region Logic

        /// <summary>
        /// Returns whether the specified <see cref="Matrix2"/>s are equal.
        /// </summary>
        /// <param name="left">The Matrix of the left side of the equation.</param>
        /// <param name="right">The Matrix of the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the corresponding elements of the specified Matrices are approximately equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==( Matrix2 left, Matrix2 right )
        {
            return left.M11.IsApproximate( right.M11 ) && left.M12.IsApproximate( right.M12 ) &&
                   left.M21.IsApproximate( right.M21 ) && left.M22.IsApproximate( right.M22 );
        }

        /// <summary>
        /// Returns whether the specified <see cref="Matrix2"/>s are not equal.
        /// </summary>
        /// <param name="left">The Quaternion of the left side of the equation.</param>
        /// <param name="right">The Quaternion of the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if any of the corresponding elements of the specified Matrices are approximately not equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=( Matrix2 left, Matrix2 right )
        {
            return !left.M11.IsApproximate( right.M11 ) || !left.M12.IsApproximate( right.M12 ) ||
                   !left.M21.IsApproximate( right.M21 ) || !left.M22.IsApproximate( right.M22 );
        }

        #endregion
         
        #endregion
    }
}
