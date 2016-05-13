// <copyright file="Matrix4.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Matrix4 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Globalization;

    /// <summary> 
    /// Represents a 4x4 homogenous single-precission floating point result. 
    /// </summary>
    [System.Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.Matrix4Converter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Matrix4 : IEquatable<Matrix4>, ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>Value at row 1 column 1 of the Matrix.</summary>
        public float M11;

        /// <summary>Value at row 1 column 2 of the Matrix.</summary>
        public float M12;

        /// <summary>Value at row 1 column 3 of the Matrix.</summary>
        public float M13;

        /// <summary>Value at row 1 column 4 of the Matrix.</summary>
        public float M14;

        /// <summary>Value at row 2 column 1 of the Matrix.</summary>
        public float M21;

        /// <summary>Value at row 2 column 2 of the Matrix.</summary>
        public float M22;

        /// <summary>Value at row 2 column 3 of the Matrix.</summary>
        public float M23;

        /// <summary>Value at row 2 column 4 of the Matrix.</summary>
        public float M24;

        /// <summary>Value at row 3 column 1 of the Matrix.</summary>
        public float M31;

        /// <summary>Value at row 3 column 2 of the Matrix.</summary>
        public float M32;

        /// <summary>Value at row 3 column 3 of the Matrix.</summary>
        public float M33;

        /// <summary>Value at row 3 column 4 of the Matrix.</summary>
        public float M34;

        /// <summary>Value at row 4 column 1 of the Matrix.</summary>
        public float M41;

        /// <summary>Value at row 4 column 2 of the Matrix.</summary>
        public float M42;

        /// <summary>Value at row 4 column 3 of the Matrix.</summary>
        public float M43;

        /// <summary>Value at row 4 column 4 of the Matrix.</summary>
        public float M44; 

        #endregion

        #region [ Constants ]

        /// <summary>
        /// The 4x4 identity matrix.
        /// </summary>
        private static readonly Matrix4 identity = new Matrix4(
            1.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f 
        );

        /// <summary>
        /// Gets the 4x4 identity Matrix,
        /// which is a matrix that represents no chance in rotation.
        /// </summary>
        /// <value>The identity result.</value>
        public static Matrix4 Identity
        {
            get
            {
                return identity;
            }
        }

        #endregion

        #region [ Properties ]

        /// <summary>Gets or sets the up point of the Matrix4.</summary>
        /// <value>The up point of the Matrix4.</value>
        public Vector3 Up
        {
            get
            {
                Vector3 vector;
                vector.X = this.M21;
                vector.Y = this.M22;
                vector.Z = this.M23;
                return vector;
            }

            set
            {
                this.M21 = value.X;
                this.M22 = value.Y;
                this.M23 = value.Z;
            }
        }

        /// <summary>Gets or sets the down point of the Matrix4.</summary>
        /// <value>The down point of the Matrix4.</value>
        public Vector3 Down
        {
            get
            {
                Vector3 vector;
                vector.X = -this.M21;
                vector.Y = -this.M22;
                vector.Z = -this.M23;
                return vector;
            }

            set
            {
                this.M21 = -value.X;
                this.M22 = -value.Y;
                this.M23 = -value.Z;
            }
        }

        /// <summary>Gets or sets the right point of the Matrix4.</summary>
        /// <value>The right point of the Matrix4.</value>
        public Vector3 Right
        {
            get
            {
                Vector3 vector;
                vector.X = this.M11;
                vector.Y = this.M12;
                vector.Z = this.M13;
                return vector;
            }

            set
            {
                this.M11 = value.X;
                this.M12 = value.Y;
                this.M13 = value.Z;
            }
        }

        /// <summary>Gets or sets the left point of the Matrix4.</summary>
        /// <value>The left point of the Matrix4.</value>
        public Vector3 Left
        {
            get
            {
                Vector3 vector;

                vector.X = -this.M11;
                vector.Y = -this.M12;
                vector.Z = -this.M13;

                return vector;
            }

            set
            {
                this.M11 = -value.X;
                this.M12 = -value.Y;
                this.M13 = -value.Z;
            }
        }

        /// <summary>Gets or sets the forward point of the Matrix4.</summary>
        /// <value>The forward point of the Matrix4.</value>
        public Vector3 Forward
        {
            get
            {
                Vector3 vector;

                vector.X = -this.M31;
                vector.Y = -this.M32;
                vector.Z = -this.M33;

                return vector;
            }

            set
            {
                this.M31 = -value.X;
                this.M32 = -value.Y;
                this.M33 = -value.Z;
            }
        }

        /// <summary>Gets or sets the backward point of the Matrix4.</summary>
        /// <value>The backward point of the Matrix4.</value>
        public Vector3 Backward
        {
            get
            {
                Vector3 vector;

                vector.X = this.M31;
                vector.Y = this.M32;
                vector.Z = this.M33;

                return vector;
            }

            set
            {
                this.M31 = value.X;
                this.M32 = value.Y;
                this.M33 = value.Z;
            }
        }

        /// <summary>Gets or sets the translation point of the Matrix4.</summary>
        /// <value>The translation point of the Matrix4.</value>
        public Vector3 Translation
        {
            get
            {
                Vector3 vector;

                vector.X = this.M41;
                vector.Y = this.M42;
                vector.Z = this.M43;

                return vector;
            }

            set
            {
                this.M41 = value.X;
                this.M42 = value.Y;
                this.M43 = value.Z;
            }
        }
        
        /// <summary>
        /// Gets the determinant of this <see cref="Matrix4"/>.
        /// </summary>
        /// <remarks>
        /// A matrix cannot be inverted if its determinant is zero.
        /// </remarks>
        /// <value>The determinant of the result.</value>
        public float Determinant
        {
            get
            {
                float num18 = (M33 * M44) - (M34 * M43);
                float num17 = (M32 * M44) - (M34 * M42);
                float num16 = (M32 * M43) - (M33 * M42);
                float num15 = (M31 * M44) - (M34 * M41);
                float num14 = (M31 * M43) - (M33 * M41);
                float num13 = (M31 * M42) - (M32 * M41);

                return (M11 * ((M22 * num18) - (M23 * num17) + (M24 * num16))) - 
                       (M12 * ((M21 * num18) - (M23 * num15) + (M24 * num14))) + 
                       (M13 * ((M21 * num17) - (M22 * num15) + (M24 * num13))) -
                       (M14 * ((M21 * num16) - (M22 * num14) + (M23 * num13)));
            }
        }

        /// <summary>
        /// Gets the inverse of this <see cref="Matrix4"/>.
        /// </summary>
        /// <value>
        /// The inverse of a matrix A is called A^-1
        /// and can be used to 'undo' the the matrix:
        /// A * A^-1 = I (Identity).
        /// </value>
        public Matrix4 Inverse
        {
            get
            {
                return Invert( this );
            }
        }
   
        /// <summary>
        /// Gets the trace of this <see cref="Matrix4"/>,
        /// which is the sum of its diagonal elements.
        /// </summary>
        /// <value> The trace value of the <see cref="Matrix4"/>. </value>
        public float Trace
        {
            get
            {
                return M11 + M22 + M33 + M44;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this Matrix4 is symmetric.
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
                for( int i = 0; i < 4; ++i )
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

        #region Indexer

        #region this[int index]

        /// <summary> Gets/Sets the element of the <see cref="Matrix4"/> at the specified <paramref name="index"/>. </summary>
        /// <param name="index"> An index value. Indices=[0, 1, 2, .., 13, 14, 15]. </param>
        /// <returns> The value at the specified <paramref name="index"/>. </returns>
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
                        return M13;
                    case 3:
                        return M14;

                    case 4:
                        return M21;
                    case 5:
                        return M22;
                    case 6:
                        return M23;
                    case 7:
                        return M24;

                    case 8:
                        return M31;
                    case 9:
                        return M32;
                    case 10:
                        return M33;
                    case 11:
                        return M34;

                    case 12:
                        return M41;
                    case 13:
                        return M42;
                    case 14:
                        return M43;
                    case 15:
                        return M44;

                    default:
                        throw new System.ArgumentOutOfRangeException(
                            "index", 
                            index,
                            Atom.ErrorStrings.SpecifiedIndexIsOutOfValidRange
                        );
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
                        M13 = value;
                        break;
                    case 3:
                        M14 = value;
                        break;

                    case 4:
                        M21 = value;
                        break;
                    case 5:
                        M22 = value;
                        break;
                    case 6:
                        M23 = value;
                        break;
                    case 7:
                        M24 = value;
                        break;

                    case 8:
                        M31 = value;
                        break;
                    case 9:
                        M32 = value;
                        break;
                    case 10:
                        M33 = value;
                        break;
                    case 11:
                        M34 = value;
                        break;

                    case 12:
                        M41 = value;
                        break;
                    case 13:
                        M42 = value;
                        break;
                    case 14:
                        M43 = value;
                        break;
                    case 15:
                        M44 = value;
                        break;

                    default:
                        throw new System.ArgumentOutOfRangeException(
                            "index",
                            index,
                            Atom.ErrorStrings.SpecifiedIndexIsOutOfValidRange
                        );
                }
            }
        }

        #endregion

        #region this[int row, int col]

        /// <summary>
        /// Gets/Sets the element of the <see cref="Matrix4"/> 
        /// at the specified <paramref name="column"/> and <paramref name="row"/>. 
        /// </summary>
        /// <param name="row"> The matrix row. Valid values: [0, 1, 2, 3].</param>
        /// <param name="column"> The matrix column. Valid values: [0, 1, 2, 3].</param>
        /// <exception cref="System.ArgumentOutOfRangeException"> 
        /// If the <paramref name="row"/> or <paramref name="column"/> is invalid.
        /// </exception>
        /// <returns>
        /// The matrix element at the specified <paramref name="row"/> and <paramref name="column"/>,
        /// </returns>
        public float this[int row, int column]
        {
            get
            {
                return this[(row * 4) + column];
            }

            set
            {
                this[(row * 4) + column] = value;
            }
        }

        #endregion

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix4"/> structure.
        /// </summary>
        /// <param name="m11">Value to initialize m11 to.</param>
        /// <param name="m12">Value to initialize m12 to.</param>
        /// <param name="m13">Value to initialize m13 to.</param>
        /// <param name="m14">Value to initialize m14 to.</param>
        /// <param name="m21">Value to initialize m21 to.</param>
        /// <param name="m22">Value to initialize m22 to.</param>
        /// <param name="m23">Value to initialize m23 to.</param>
        /// <param name="m24">Value to initialize m24 to.</param>
        /// <param name="m31">Value to initialize m31 to.</param>
        /// <param name="m32">Value to initialize m32 to.</param>
        /// <param name="m33">Value to initialize m33 to.</param>
        /// <param name="m34">Value to initialize m34 to.</param>
        /// <param name="m41">Value to initialize m41 to.</param>
        /// <param name="m42">Value to initialize m42 to.</param>
        /// <param name="m43">Value to initialize m43 to.</param>
        /// <param name="m44">Value to initialize m44 to.</param>
        public Matrix4( 
            float m11, float m12, float m13, float m14, 
            float m21, float m22, float m23, float m24, 
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44 )
        {
            this.M11 = m11;
            this.M12 = m12;
            this.M13 = m13;
            this.M14 = m14;

            this.M21 = m21;
            this.M22 = m22;
            this.M23 = m23;
            this.M24 = m24;

            this.M31 = m31;
            this.M32 = m32;
            this.M33 = m33;
            this.M34 = m34;

            this.M41 = m41;
            this.M42 = m42;
            this.M43 = m43;
            this.M44 = m44;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix4"/> structure.
        /// </summary>
        /// <param name="elements">
        /// The elements of the new Matrix.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="elements"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the length of the specified <paramref name="elements"/> array is less than 16.
        /// </exception>
        public Matrix4( float[] elements )
        {
            if( elements == null )
                throw new ArgumentNullException( "elements" );

            if( elements.Length < 16 )
                throw new ArgumentException( Atom.ErrorStrings.ArrayLengthOutOfValidRange, "elements" );

            this.M11 = elements[0];
            this.M12 = elements[1];
            this.M13 = elements[2];
            this.M14 = elements[3];

            this.M21 = elements[4];
            this.M22 = elements[5];
            this.M23 = elements[6];
            this.M24 = elements[7];

            this.M31 = elements[8];
            this.M32 = elements[9];
            this.M33 = elements[10];
            this.M34 = elements[11];

            this.M41 = elements[12];
            this.M42 = elements[13];
            this.M43 = elements[14];
            this.M44 = elements[15];
        }

        #endregion

        #region [ Methods ]

        #region Transform

        /// <summary>
        /// Transforms a result by applying a Quaternion rotation.
        /// </summary>
        /// <param name="matrix">The result to transform.</param>
        /// <param name="rotation">The rotation to apply, expressed as a Quaternion.</param>
        /// <returns>
        /// A new result that is the result of the transform.
        /// </returns>
        public static Matrix4 Transform( Matrix4 matrix, Quaternion rotation )
        {
            float num21 = rotation.X + rotation.X;
            float num11 = rotation.Y + rotation.Y;
            float num10 = rotation.Z + rotation.Z;
            float num20 = rotation.W * num21;
            float num19 = rotation.W * num11;
            float num18 = rotation.W * num10;
            float num17 = rotation.X * num21;
            float num16 = rotation.X * num11;
            float num15 = rotation.X * num10;
            float num14 = rotation.Y * num11;
            float num13 = rotation.Y * num10;
            float num12 = rotation.Z * num10;
            float num9 = (1f - num14) - num12;
            float num8 = num16 - num18;
            float num7 = num15 + num19;
            float num6 = num16 + num18;
            float num5 = (1f - num17) - num12;
            float num4 = num13 - num20;
            float num3 = num15 - num19;
            float num2 = num13 + num20;
            float num = (1f - num17) - num14;

            Matrix4 result;

            result.M11 = ((matrix.M11 * num9) + (matrix.M12 * num8)) + (matrix.M13 * num7);
            result.M12 = ((matrix.M11 * num6) + (matrix.M12 * num5)) + (matrix.M13 * num4);
            result.M13 = ((matrix.M11 * num3) + (matrix.M12 * num2)) + (matrix.M13 * num);
            result.M14 = matrix.M14;

            result.M21 = ((matrix.M21 * num9) + (matrix.M22 * num8)) + (matrix.M23 * num7);
            result.M22 = ((matrix.M21 * num6) + (matrix.M22 * num5)) + (matrix.M23 * num4);
            result.M23 = ((matrix.M21 * num3) + (matrix.M22 * num2)) + (matrix.M23 * num);
            result.M24 = matrix.M24;

            result.M31 = ((matrix.M31 * num9) + (matrix.M32 * num8)) + (matrix.M33 * num7);
            result.M32 = ((matrix.M31 * num6) + (matrix.M32 * num5)) + (matrix.M33 * num4);
            result.M33 = ((matrix.M31 * num3) + (matrix.M32 * num2)) + (matrix.M33 * num);
            result.M34 = matrix.M34;

            result.M41 = ((matrix.M41 * num9) + (matrix.M42 * num8)) + (matrix.M43 * num7);
            result.M42 = ((matrix.M41 * num6) + (matrix.M42 * num5)) + (matrix.M43 * num4);
            result.M43 = ((matrix.M41 * num3) + (matrix.M42 * num2)) + (matrix.M43 * num);
            result.M44 = matrix.M44;

            return result;
        }

        /// <summary>
        /// Transforms a result by applying a Quaternion rotation.
        /// </summary>
        /// <param name="matrix">The result to transform.</param>
        /// <param name="rotation">The rotation to apply, expressed as a Quaternion.</param>
        /// <param name="result">An existing result filled in with the result of the transform.</param>
        public static void Transform( ref Matrix4 matrix, ref Quaternion rotation, out Matrix4 result )
        {
            float num21 = rotation.X + rotation.X;
            float num11 = rotation.Y + rotation.Y;
            float num10 = rotation.Z + rotation.Z;
            float num20 = rotation.W * num21;
            float num19 = rotation.W * num11;
            float num18 = rotation.W * num10;
            float num17 = rotation.X * num21;
            float num16 = rotation.X * num11;
            float num15 = rotation.X * num10;
            float num14 = rotation.Y * num11;
            float num13 = rotation.Y * num10;
            float num12 = rotation.Z * num10;
            float num9 = (1f - num14) - num12;
            float num8 = num16 - num18;
            float num7 = num15 + num19;
            float num6 = num16 + num18;
            float num5 = (1f - num17) - num12;
            float num4 = num13 - num20;
            float num3 = num15 - num19;
            float num2 = num13 + num20;
            float num = (1f - num17) - num14;

            result.M11 = ((matrix.M11 * num9) + (matrix.M12 * num8)) + (matrix.M13 * num7);
            result.M12 = ((matrix.M11 * num6) + (matrix.M12 * num5)) + (matrix.M13 * num4);
            result.M13 = ((matrix.M11 * num3) + (matrix.M12 * num2)) + (matrix.M13 * num);
            result.M14 = matrix.M14;

            result.M21 = ((matrix.M21 * num9) + (matrix.M22 * num8)) + (matrix.M23 * num7);
            result.M22 = ((matrix.M21 * num6) + (matrix.M22 * num5)) + (matrix.M23 * num4);
            result.M23 = ((matrix.M21 * num3) + (matrix.M22 * num2)) + (matrix.M23 * num);
            result.M24 = matrix.M24;

            result.M31 = ((matrix.M31 * num9) + (matrix.M32 * num8)) + (matrix.M33 * num7);
            result.M32 = ((matrix.M31 * num6) + (matrix.M32 * num5)) + (matrix.M33 * num4);
            result.M33 = ((matrix.M31 * num3) + (matrix.M32 * num2)) + (matrix.M33 * num);
            result.M34 = matrix.M34;

            result.M41 = ((matrix.M41 * num9) + (matrix.M42 * num8)) + (matrix.M43 * num7);
            result.M42 = ((matrix.M41 * num6) + (matrix.M42 * num5)) + (matrix.M43 * num4);
            result.M43 = ((matrix.M41 * num3) + (matrix.M42 * num2)) + (matrix.M43 * num);
            result.M44 = matrix.M44;
        }

        #endregion

        #region Transpose

        /// <summary>Transposes the rows and columns of a result.</summary>
        /// <param name="matrix">The source result.</param>
        /// <returns>Transposed result.</returns>
        public static Matrix4 Transpose( Matrix4 matrix )
        {
            Matrix4 result;

            result.M11 = matrix.M11;
            result.M12 = matrix.M21;
            result.M13 = matrix.M31;
            result.M14 = matrix.M41;
            result.M21 = matrix.M12;
            result.M22 = matrix.M22;
            result.M23 = matrix.M32;
            result.M24 = matrix.M42;
            result.M31 = matrix.M13;
            result.M32 = matrix.M23;
            result.M33 = matrix.M33;
            result.M34 = matrix.M43;
            result.M41 = matrix.M14;
            result.M42 = matrix.M24;
            result.M43 = matrix.M34;
            result.M44 = matrix.M44;

            return result;
        }

        /// <summary>Transposes the rows and columns of a result.</summary>
        /// <param name="matrix">Source result.</param>
        /// <param name="result">Transposed result.</param>
        public static void Transpose( ref Matrix4 matrix, out Matrix4 result )
        {
            result.M11 = matrix.M11;
            result.M12 = matrix.M21;
            result.M13 = matrix.M31;
            result.M14 = matrix.M41;
            result.M21 = matrix.M12;
            result.M22 = matrix.M22;
            result.M23 = matrix.M32;
            result.M24 = matrix.M42;
            result.M31 = matrix.M13;
            result.M32 = matrix.M23;
            result.M33 = matrix.M33;
            result.M34 = matrix.M43;
            result.M41 = matrix.M14;
            result.M42 = matrix.M24;
            result.M43 = matrix.M34;
            result.M44 = matrix.M44;
        }

        #endregion

        #region Invert

        /// <summary>Calculates the inverse of a result.</summary>
        /// <param name="matrix">Source result.</param>
        /// <returns>The inverse of the result.</returns>
        public static Matrix4 Invert( Matrix4 matrix )
        {
            Matrix4 result;

            float num5 = matrix.M11;
            float num4 = matrix.M12;
            float num3 = matrix.M13;
            float num2 = matrix.M14;
            float num9 = matrix.M21;
            float num8 = matrix.M22;
            float num7 = matrix.M23;
            float num6 = matrix.M24;
            float num17 = matrix.M31;
            float num16 = matrix.M32;
            float num15 = matrix.M33;
            float num14 = matrix.M34;
            float num13 = matrix.M41;
            float num12 = matrix.M42;
            float num11 = matrix.M43;
            float num10 = matrix.M44;

            float num23 = (num15 * num10) - (num14 * num11);
            float num22 = (num16 * num10) - (num14 * num12);
            float num21 = (num16 * num11) - (num15 * num12);
            float num20 = (num17 * num10) - (num14 * num13);
            float num19 = (num17 * num11) - (num15 * num13);
            float num18 = (num17 * num12) - (num16 * num13);
            float num39 = ((num8 * num23) - (num7 * num22)) + (num6 * num21);
            float num38 = -(((num9 * num23) - (num7 * num20)) + (num6 * num19));
            float num37 = ((num9 * num22) - (num8 * num20)) + (num6 * num18);
            float num36 = -(((num9 * num21) - (num8 * num19)) + (num7 * num18));
            float num = 1f / ((((num5 * num39) + (num4 * num38)) + (num3 * num37)) + (num2 * num36));

            result.M11 = num39 * num;
            result.M21 = num38 * num;
            result.M31 = num37 * num;
            result.M41 = num36 * num;
            result.M12 = -(((num4 * num23) - (num3 * num22)) + (num2 * num21)) * num;
            result.M22 = (((num5 * num23) - (num3 * num20)) + (num2 * num19)) * num;
            result.M32 = -(((num5 * num22) - (num4 * num20)) + (num2 * num18)) * num;
            result.M42 = (((num5 * num21) - (num4 * num19)) + (num3 * num18)) * num;
            float num35 = (num7 * num10) - (num6 * num11);
            float num34 = (num8 * num10) - (num6 * num12);
            float num33 = (num8 * num11) - (num7 * num12);
            float num32 = (num9 * num10) - (num6 * num13);
            float num31 = (num9 * num11) - (num7 * num13);
            float num30 = (num9 * num12) - (num8 * num13);
            result.M13 = (((num4 * num35) - (num3 * num34)) + (num2 * num33)) * num;
            result.M23 = -(((num5 * num35) - (num3 * num32)) + (num2 * num31)) * num;
            result.M33 = (((num5 * num34) - (num4 * num32)) + (num2 * num30)) * num;
            result.M43 = -(((num5 * num33) - (num4 * num31)) + (num3 * num30)) * num;
            float num29 = (num7 * num14) - (num6 * num15);
            float num28 = (num8 * num14) - (num6 * num16);
            float num27 = (num8 * num15) - (num7 * num16);
            float num26 = (num9 * num14) - (num6 * num17);
            float num25 = (num9 * num15) - (num7 * num17);
            float num24 = (num9 * num16) - (num8 * num17);
            result.M14 = -(((num4 * num29) - (num3 * num28)) + (num2 * num27)) * num;
            result.M24 = (((num5 * num29) - (num3 * num26)) + (num2 * num25)) * num;
            result.M34 = -(((num5 * num28) - (num4 * num26)) + (num2 * num24)) * num;
            result.M44 = (((num5 * num27) - (num4 * num25)) + (num3 * num24)) * num;
            return result;
        }

        /// <summary>Calculates the inverse of a result.</summary>
        /// <param name="matrix">Source result.</param>
        /// <param name="result">The inverse of the result.</param>
        public static void Invert( ref Matrix4 matrix, out Matrix4 result )
        {
            float num5 = matrix.M11;
            float num4 = matrix.M12;
            float num3 = matrix.M13;
            float num2 = matrix.M14;
            float num9 = matrix.M21;
            float num8 = matrix.M22;
            float num7 = matrix.M23;
            float num6 = matrix.M24;
            float num17 = matrix.M31;
            float num16 = matrix.M32;
            float num15 = matrix.M33;
            float num14 = matrix.M34;
            float num13 = matrix.M41;
            float num12 = matrix.M42;
            float num11 = matrix.M43;
            float num10 = matrix.M44;
            float num23 = (num15 * num10) - (num14 * num11);
            float num22 = (num16 * num10) - (num14 * num12);
            float num21 = (num16 * num11) - (num15 * num12);
            float num20 = (num17 * num10) - (num14 * num13);
            float num19 = (num17 * num11) - (num15 * num13);
            float num18 = (num17 * num12) - (num16 * num13);
            float num39 = ((num8 * num23) - (num7 * num22)) + (num6 * num21);
            float num38 = -(((num9 * num23) - (num7 * num20)) + (num6 * num19));
            float num37 = ((num9 * num22) - (num8 * num20)) + (num6 * num18);
            float num36 = -(((num9 * num21) - (num8 * num19)) + (num7 * num18));
            float num = 1f / ((((num5 * num39) + (num4 * num38)) + (num3 * num37)) + (num2 * num36));
            result.M11 = num39 * num;
            result.M21 = num38 * num;
            result.M31 = num37 * num;
            result.M41 = num36 * num;
            result.M12 = -(((num4 * num23) - (num3 * num22)) + (num2 * num21)) * num;
            result.M22 = (((num5 * num23) - (num3 * num20)) + (num2 * num19)) * num;
            result.M32 = -(((num5 * num22) - (num4 * num20)) + (num2 * num18)) * num;
            result.M42 = (((num5 * num21) - (num4 * num19)) + (num3 * num18)) * num;
            float num35 = (num7 * num10) - (num6 * num11);
            float num34 = (num8 * num10) - (num6 * num12);
            float num33 = (num8 * num11) - (num7 * num12);
            float num32 = (num9 * num10) - (num6 * num13);
            float num31 = (num9 * num11) - (num7 * num13);
            float num30 = (num9 * num12) - (num8 * num13);
            result.M13 = (((num4 * num35) - (num3 * num34)) + (num2 * num33)) * num;
            result.M23 = -(((num5 * num35) - (num3 * num32)) + (num2 * num31)) * num;
            result.M33 = (((num5 * num34) - (num4 * num32)) + (num2 * num30)) * num;
            result.M43 = -(((num5 * num33) - (num4 * num31)) + (num3 * num30)) * num;
            float num29 = (num7 * num14) - (num6 * num15);
            float num28 = (num8 * num14) - (num6 * num16);
            float num27 = (num8 * num15) - (num7 * num16);
            float num26 = (num9 * num14) - (num6 * num17);
            float num25 = (num9 * num15) - (num7 * num17);
            float num24 = (num9 * num16) - (num8 * num17);
            result.M14 = -(((num4 * num29) - (num3 * num28)) + (num2 * num27)) * num;
            result.M24 = (((num5 * num29) - (num3 * num26)) + (num2 * num25)) * num;
            result.M34 = -(((num5 * num28) - (num4 * num26)) + (num2 * num24)) * num;
            result.M44 = (((num5 * num27) - (num4 * num25)) + (num3 * num24)) * num;
        }

        #endregion

        #region > Operators <

        #region Add

        /// <summary>
        /// Returns the result of adding the <paramref name="right"/> Matrix
        /// to the <paramref name="left"/> Matrix.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix4 Add( Matrix4 left, Matrix4 right )
        {
            Matrix4 result;

            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M13 = left.M13 + right.M13;
            result.M14 = left.M14 + right.M14;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;
            result.M23 = left.M23 + right.M23;
            result.M24 = left.M24 + right.M24;
            result.M31 = left.M31 + right.M31;
            result.M32 = left.M32 + right.M32;
            result.M33 = left.M33 + right.M33;
            result.M34 = left.M34 + right.M34;
            result.M41 = left.M41 + right.M41;
            result.M42 = left.M42 + right.M42;
            result.M43 = left.M43 + right.M43;
            result.M44 = left.M44 + right.M44;

            return result;
        }

        /// <summary>
        /// Stores the result of adding the <paramref name="right"/> Matrix
        /// to the <paramref name="left"/> Matrix in the specified <paramref name="result"/> value.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The Matrix on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of this operation.</param>
        public static void Add( ref Matrix4 left, ref Matrix4 right, out Matrix4 result )
        {
            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M13 = left.M13 + right.M13;
            result.M14 = left.M14 + right.M14;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;
            result.M23 = left.M23 + right.M23;
            result.M24 = left.M24 + right.M24;
            result.M31 = left.M31 + right.M31;
            result.M32 = left.M32 + right.M32;
            result.M33 = left.M33 + right.M33;
            result.M34 = left.M34 + right.M34;
            result.M41 = left.M41 + right.M41;
            result.M42 = left.M42 + right.M42;
            result.M43 = left.M43 + right.M43;
            result.M44 = left.M44 + right.M44;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Matrix
        /// from the <paramref name="left"/> Matrix.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>
        /// <returns>
        /// The result of the subtraction.
        /// </returns>
        public static Matrix4 Subtract( Matrix4 left, Matrix4 right )
        {
            Matrix4 result;

            result.M11 = left.M11 - right.M11;
            result.M12 = left.M12 - right.M12;
            result.M13 = left.M13 - right.M13;
            result.M14 = left.M14 - right.M14;
            result.M21 = left.M21 - right.M21;
            result.M22 = left.M22 - right.M22;
            result.M23 = left.M23 - right.M23;
            result.M24 = left.M24 - right.M24;
            result.M31 = left.M31 - right.M31;
            result.M32 = left.M32 - right.M32;
            result.M33 = left.M33 - right.M33;
            result.M34 = left.M34 - right.M34;
            result.M41 = left.M41 - right.M41;
            result.M42 = left.M42 - right.M42;
            result.M43 = left.M43 - right.M43;
            result.M44 = left.M44 - right.M44;

            return result;
        }

        /// <summary>
        /// Stores the result of subtracting the <paramref name="right"/> Matrix
        /// from the <paramref name="left"/> Matrix im the specified <paramref name="result"/> value.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The Matrix on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the subtraction.</param>
        public static void Subtract( ref Matrix4 left, ref Matrix4 right, out Matrix4 result )
        {
            result.M11 = left.M11 - right.M11;
            result.M12 = left.M12 - right.M12;
            result.M13 = left.M13 - right.M13;
            result.M14 = left.M14 - right.M14;
            result.M21 = left.M21 - right.M21;
            result.M22 = left.M22 - right.M22;
            result.M23 = left.M23 - right.M23;
            result.M24 = left.M24 - right.M24;
            result.M31 = left.M31 - right.M31;
            result.M32 = left.M32 - right.M32;
            result.M33 = left.M33 - right.M33;
            result.M34 = left.M34 - right.M34;
            result.M41 = left.M41 - right.M41;
            result.M42 = left.M42 - right.M42;
            result.M43 = left.M43 - right.M43;
            result.M44 = left.M44 - right.M44;
        }

        #endregion

        #region Negate

        /// <summary>
        /// Returns the result of negating the specified <see cref="Matrix4"/>.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>The negated matrix.</returns>
        public static Matrix4 Negate( Matrix4 matrix )
        {
            Matrix4 result;

            result.M11 = -matrix.M11;
            result.M12 = -matrix.M12;
            result.M13 = -matrix.M13;
            result.M14 = -matrix.M14;
            result.M21 = -matrix.M21;
            result.M22 = -matrix.M22;
            result.M23 = -matrix.M23;
            result.M24 = -matrix.M24;
            result.M31 = -matrix.M31;
            result.M32 = -matrix.M32;
            result.M33 = -matrix.M33;
            result.M34 = -matrix.M34;
            result.M41 = -matrix.M41;
            result.M42 = -matrix.M42;
            result.M43 = -matrix.M43;
            result.M44 = -matrix.M44;

            return result;
        }

        /// <summary>
        /// Stores the result of negating the specified <see cref="Matrix4"/>
        /// in the specified <paramref name="result"/> value.
        /// </summary>
        /// <param name="matrix">The input matrix. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the negated matrix.</param>
        public static void Negate( ref Matrix4 matrix, out Matrix4 result )
        {
            result.M11 = -matrix.M11;
            result.M12 = -matrix.M12;
            result.M13 = -matrix.M13;
            result.M14 = -matrix.M14;
            result.M21 = -matrix.M21;
            result.M22 = -matrix.M22;
            result.M23 = -matrix.M23;
            result.M24 = -matrix.M24;
            result.M31 = -matrix.M31;
            result.M32 = -matrix.M32;
            result.M33 = -matrix.M33;
            result.M34 = -matrix.M34;
            result.M41 = -matrix.M41;
            result.M42 = -matrix.M42;
            result.M43 = -matrix.M43;
            result.M44 = -matrix.M44;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Returns the result of multiplying the <paramref name="left"/> Matrix
        /// by the <paramref name="right"/> Matrix.
        /// Matrix multiplication can be used to 'combine' the rotations the matrices represent.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Matrix4 Multiply( Matrix4 left, Matrix4 right )
        {
            Matrix4 result;

            result.M11 = (((left.M11 * right.M11) + (left.M12 * right.M21)) + (left.M13 * right.M31)) + (left.M14 * right.M41);
            result.M12 = (((left.M11 * right.M12) + (left.M12 * right.M22)) + (left.M13 * right.M32)) + (left.M14 * right.M42);
            result.M13 = (((left.M11 * right.M13) + (left.M12 * right.M23)) + (left.M13 * right.M33)) + (left.M14 * right.M43);
            result.M14 = (((left.M11 * right.M14) + (left.M12 * right.M24)) + (left.M13 * right.M34)) + (left.M14 * right.M44);
            result.M21 = (((left.M21 * right.M11) + (left.M22 * right.M21)) + (left.M23 * right.M31)) + (left.M24 * right.M41);
            result.M22 = (((left.M21 * right.M12) + (left.M22 * right.M22)) + (left.M23 * right.M32)) + (left.M24 * right.M42);
            result.M23 = (((left.M21 * right.M13) + (left.M22 * right.M23)) + (left.M23 * right.M33)) + (left.M24 * right.M43);
            result.M24 = (((left.M21 * right.M14) + (left.M22 * right.M24)) + (left.M23 * right.M34)) + (left.M24 * right.M44);
            result.M31 = (((left.M31 * right.M11) + (left.M32 * right.M21)) + (left.M33 * right.M31)) + (left.M34 * right.M41);
            result.M32 = (((left.M31 * right.M12) + (left.M32 * right.M22)) + (left.M33 * right.M32)) + (left.M34 * right.M42);
            result.M33 = (((left.M31 * right.M13) + (left.M32 * right.M23)) + (left.M33 * right.M33)) + (left.M34 * right.M43);
            result.M34 = (((left.M31 * right.M14) + (left.M32 * right.M24)) + (left.M33 * right.M34)) + (left.M34 * right.M44);
            result.M41 = (((left.M41 * right.M11) + (left.M42 * right.M21)) + (left.M43 * right.M31)) + (left.M44 * right.M41);
            result.M42 = (((left.M41 * right.M12) + (left.M42 * right.M22)) + (left.M43 * right.M32)) + (left.M44 * right.M42);
            result.M43 = (((left.M41 * right.M13) + (left.M42 * right.M23)) + (left.M43 * right.M33)) + (left.M44 * right.M43);
            result.M44 = (((left.M41 * right.M14) + (left.M42 * right.M24)) + (left.M43 * right.M34)) + (left.M44 * right.M44);

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the <paramref name="left"/> Matrix
        /// by the <paramref name="right"/> Matrix in the specified <paramref name="result"/> value.
        /// Matrix multiplication can be used to 'combine' the rotations the matrices represent.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The Matrix on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the multiplication.</param>
        public static void Multiply( ref Matrix4 left, ref Matrix4 right, out Matrix4 result )
        {
            result.M11 = (((left.M11 * right.M11) + (left.M12 * right.M21)) + (left.M13 * right.M31)) + (left.M14 * right.M41);
            result.M12 = (((left.M11 * right.M12) + (left.M12 * right.M22)) + (left.M13 * right.M32)) + (left.M14 * right.M42);
            result.M13 = (((left.M11 * right.M13) + (left.M12 * right.M23)) + (left.M13 * right.M33)) + (left.M14 * right.M43);
            result.M14 = (((left.M11 * right.M14) + (left.M12 * right.M24)) + (left.M13 * right.M34)) + (left.M14 * right.M44);
            result.M21 = (((left.M21 * right.M11) + (left.M22 * right.M21)) + (left.M23 * right.M31)) + (left.M24 * right.M41);
            result.M22 = (((left.M21 * right.M12) + (left.M22 * right.M22)) + (left.M23 * right.M32)) + (left.M24 * right.M42);
            result.M23 = (((left.M21 * right.M13) + (left.M22 * right.M23)) + (left.M23 * right.M33)) + (left.M24 * right.M43);
            result.M24 = (((left.M21 * right.M14) + (left.M22 * right.M24)) + (left.M23 * right.M34)) + (left.M24 * right.M44);
            result.M31 = (((left.M31 * right.M11) + (left.M32 * right.M21)) + (left.M33 * right.M31)) + (left.M34 * right.M41);
            result.M32 = (((left.M31 * right.M12) + (left.M32 * right.M22)) + (left.M33 * right.M32)) + (left.M34 * right.M42);
            result.M33 = (((left.M31 * right.M13) + (left.M32 * right.M23)) + (left.M33 * right.M33)) + (left.M34 * right.M43);
            result.M34 = (((left.M31 * right.M14) + (left.M32 * right.M24)) + (left.M33 * right.M34)) + (left.M34 * right.M44);
            result.M41 = (((left.M41 * right.M11) + (left.M42 * right.M21)) + (left.M43 * right.M31)) + (left.M44 * right.M41);
            result.M42 = (((left.M41 * right.M12) + (left.M42 * right.M22)) + (left.M43 * right.M32)) + (left.M44 * right.M42);
            result.M43 = (((left.M41 * right.M13) + (left.M42 * right.M23)) + (left.M43 * right.M33)) + (left.M44 * right.M43);
            result.M44 = (((left.M41 * right.M14) + (left.M42 * right.M24)) + (left.M43 * right.M34)) + (left.M44 * right.M44);
        }

        /// <summary>
        /// Returns the result of multiplying the specified <see cref="Matrix4"/>
        /// by the specified <paramref name="scalar"/>.
        /// </summary>
        /// <param name="matrix">The Matrix on the left side of the equation.</param>
        /// <param name="scalar">The scalar value on the right side of the equation.</param>
        /// <returns>Result of the multiplication.</returns>
        public static Matrix4 Multiply( Matrix4 matrix, float scalar )
        {
            Matrix4 result;

            result.M11 = matrix.M11 * scalar;
            result.M12 = matrix.M12 * scalar;
            result.M13 = matrix.M13 * scalar;
            result.M14 = matrix.M14 * scalar;
            result.M21 = matrix.M21 * scalar;
            result.M22 = matrix.M22 * scalar;
            result.M23 = matrix.M23 * scalar;
            result.M24 = matrix.M24 * scalar;
            result.M31 = matrix.M31 * scalar;
            result.M32 = matrix.M32 * scalar;
            result.M33 = matrix.M33 * scalar;
            result.M34 = matrix.M34 * scalar;
            result.M41 = matrix.M41 * scalar;
            result.M42 = matrix.M42 * scalar;
            result.M43 = matrix.M43 * scalar;
            result.M44 = matrix.M44 * scalar;

            return result;
        }

        /// <summary>
        /// Stores the result of multiplying the specified <see cref="Matrix4"/>
        /// by the specified <paramref name="scalar"/> in the specified <paramref name="result"/> value.
        /// </summary>
        /// <param name="matrix">The Matrix on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="scalar">The scalar value on the right side of the equation.</param>
        /// <param name="result">Will contain the result of the multiplication.</param>
        public static void Multiply( ref Matrix4 matrix, float scalar, out Matrix4 result )
        {
            result.M11 = matrix.M11 * scalar;
            result.M12 = matrix.M12 * scalar;
            result.M13 = matrix.M13 * scalar;
            result.M14 = matrix.M14 * scalar;
            result.M21 = matrix.M21 * scalar;
            result.M22 = matrix.M22 * scalar;
            result.M23 = matrix.M23 * scalar;
            result.M24 = matrix.M24 * scalar;
            result.M31 = matrix.M31 * scalar;
            result.M32 = matrix.M32 * scalar;
            result.M33 = matrix.M33 * scalar;
            result.M34 = matrix.M34 * scalar;
            result.M41 = matrix.M41 * scalar;
            result.M42 = matrix.M42 * scalar;
            result.M43 = matrix.M43 * scalar;
            result.M44 = matrix.M44 * scalar;
        }

        #endregion

        #region Devide

        /// <summary>
        /// Returns the result of dividing the <paramref name="left"/> Matrix
        /// through the <paramref name="right"/> Matrix - component wise.
        /// </summary>
        /// <param name="left">The matrix on the left side of the equation.</param>
        /// <param name="right">The matrix on the right side of the equation.</param>
        /// <returns>
        /// The result of the division.
        /// </returns>
        public static Matrix4 Divide( Matrix4 left, Matrix4 right )
        {
            Matrix4 result;

            result.M11 = left.M11 / right.M11;
            result.M12 = left.M12 / right.M12;
            result.M13 = left.M13 / right.M13;
            result.M14 = left.M14 / right.M14;
            result.M21 = left.M21 / right.M21;
            result.M22 = left.M22 / right.M22;
            result.M23 = left.M23 / right.M23;
            result.M24 = left.M24 / right.M24;
            result.M31 = left.M31 / right.M31;
            result.M32 = left.M32 / right.M32;
            result.M33 = left.M33 / right.M33;
            result.M34 = left.M34 / right.M34;
            result.M41 = left.M41 / right.M41;
            result.M42 = left.M42 / right.M42;
            result.M43 = left.M43 / right.M43;
            result.M44 = left.M44 / right.M44;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the <paramref name="left"/> Matrix
        /// through the <paramref name="right"/> Matrix - component wise
        /// in the specified <paramref name="result"/> value.
        /// </summary>
        /// <param name="left">The matrix on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="right">The matrix on the right side of the equation. This value will not be modified by this method.</param>
        /// <param name="result">Will contain the result of the division.</param>
        public static void Divide( ref Matrix4 left, ref Matrix4 right, out Matrix4 result )
        {
            result.M11 = left.M11 / right.M11;
            result.M12 = left.M12 / right.M12;
            result.M13 = left.M13 / right.M13;
            result.M14 = left.M14 / right.M14;
            result.M21 = left.M21 / right.M21;
            result.M22 = left.M22 / right.M22;
            result.M23 = left.M23 / right.M23;
            result.M24 = left.M24 / right.M24;
            result.M31 = left.M31 / right.M31;
            result.M32 = left.M32 / right.M32;
            result.M33 = left.M33 / right.M33;
            result.M34 = left.M34 / right.M34;
            result.M41 = left.M41 / right.M41;
            result.M42 = left.M42 / right.M42;
            result.M43 = left.M43 / right.M43;
            result.M44 = left.M44 / right.M44;
        }

        /// <summary>
        /// Returns the result of dividing the specified <paramref name="matrix"/>
        /// through the specified <paramref name="divider"/> componentwise.
        /// </summary>
        /// <param name="matrix">The matrix on the left side of the equation.</param>
        /// <param name="divider">The scalar on the left side of the equation.</param>
        /// <returns>The result of the division.</returns>
        public static Matrix4 Divide( Matrix4 matrix, float divider )
        {
            float invDivider = 1f / divider;
            Matrix4 result;

            result.M11 = matrix.M11 * invDivider;
            result.M12 = matrix.M12 * invDivider;
            result.M13 = matrix.M13 * invDivider;
            result.M14 = matrix.M14 * invDivider;
            result.M21 = matrix.M21 * invDivider;
            result.M22 = matrix.M22 * invDivider;
            result.M23 = matrix.M23 * invDivider;
            result.M24 = matrix.M24 * invDivider;
            result.M31 = matrix.M31 * invDivider;
            result.M32 = matrix.M32 * invDivider;
            result.M33 = matrix.M33 * invDivider;
            result.M34 = matrix.M34 * invDivider;
            result.M41 = matrix.M41 * invDivider;
            result.M42 = matrix.M42 * invDivider;
            result.M43 = matrix.M43 * invDivider;
            result.M44 = matrix.M44 * invDivider;

            return result;
        }

        /// <summary>
        /// Stores the result of dividing the specified <paramref name="matrix"/>
        /// through the specified <paramref name="divider"/> componentwise
        /// in the specified <paramref name="result"/> value.
        /// </summary>
        /// <param name="matrix">The matrix on the left side of the equation. This value will not be modified by this method.</param>
        /// <param name="divider">The scalar on the left side of the equation.</param>
        /// <param name="result">Will contain the result of the division.</param>
        public static void Divide( ref Matrix4 matrix, float divider, out Matrix4 result )
        {
            float invDivider = 1f / divider;

            result.M11 = matrix.M11 * invDivider;
            result.M12 = matrix.M12 * invDivider;
            result.M13 = matrix.M13 * invDivider;
            result.M14 = matrix.M14 * invDivider;
            result.M21 = matrix.M21 * invDivider;
            result.M22 = matrix.M22 * invDivider;
            result.M23 = matrix.M23 * invDivider;
            result.M24 = matrix.M24 * invDivider;
            result.M31 = matrix.M31 * invDivider;
            result.M32 = matrix.M32 * invDivider;
            result.M33 = matrix.M33 * invDivider;
            result.M34 = matrix.M34 * invDivider;
            result.M41 = matrix.M41 * invDivider;
            result.M42 = matrix.M42 * invDivider;
            result.M43 = matrix.M43 * invDivider;
            result.M44 = matrix.M44 * invDivider;
        }

        #endregion

        #endregion

        #region > Interpolation <

        #region Lerp

        /// <summary>Linearly interpolates between the corresponding values of two matrices.</summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        /// <param name="amount">The amount to interpolate from the start to the end value.</param>
        /// <returns>The interpolated value.</returns>
        public static Matrix4 Lerp( Matrix4 start, Matrix4 end, float amount )
        {
            Matrix4 result;

            result.M11 = start.M11 + ((end.M11 - start.M11) * amount);
            result.M12 = start.M12 + ((end.M12 - start.M12) * amount);
            result.M13 = start.M13 + ((end.M13 - start.M13) * amount);
            result.M14 = start.M14 + ((end.M14 - start.M14) * amount);
            result.M21 = start.M21 + ((end.M21 - start.M21) * amount);
            result.M22 = start.M22 + ((end.M22 - start.M22) * amount);
            result.M23 = start.M23 + ((end.M23 - start.M23) * amount);
            result.M24 = start.M24 + ((end.M24 - start.M24) * amount);
            result.M31 = start.M31 + ((end.M31 - start.M31) * amount);
            result.M32 = start.M32 + ((end.M32 - start.M32) * amount);
            result.M33 = start.M33 + ((end.M33 - start.M33) * amount);
            result.M34 = start.M34 + ((end.M34 - start.M34) * amount);
            result.M41 = start.M41 + ((end.M41 - start.M41) * amount);
            result.M42 = start.M42 + ((end.M42 - start.M42) * amount);
            result.M43 = start.M43 + ((end.M43 - start.M43) * amount);
            result.M44 = start.M44 + ((end.M44 - start.M44) * amount);

            return result;
        }

        /// <summary>Linearly interpolates between the corresponding values of two matrices.</summary>
        /// <param name="start">The start value. The value of this argument will not be changed.</param>
        /// <param name="end">The end value. The value of this argument will not be changed.</param>
        /// <param name="amount">The amount to interpolate from the start to the end value.</param>
        /// <param name="result">Will contain the interpolated value.</param>
        public static void Lerp( ref Matrix4 start, ref Matrix4 end, float amount, out Matrix4 result )
        {
            result.M11 = start.M11 + ((end.M11 - start.M11) * amount);
            result.M12 = start.M12 + ((end.M12 - start.M12) * amount);
            result.M13 = start.M13 + ((end.M13 - start.M13) * amount);
            result.M14 = start.M14 + ((end.M14 - start.M14) * amount);
            result.M21 = start.M21 + ((end.M21 - start.M21) * amount);
            result.M22 = start.M22 + ((end.M22 - start.M22) * amount);
            result.M23 = start.M23 + ((end.M23 - start.M23) * amount);
            result.M24 = start.M24 + ((end.M24 - start.M24) * amount);
            result.M31 = start.M31 + ((end.M31 - start.M31) * amount);
            result.M32 = start.M32 + ((end.M32 - start.M32) * amount);
            result.M33 = start.M33 + ((end.M33 - start.M33) * amount);
            result.M34 = start.M34 + ((end.M34 - start.M34) * amount);
            result.M41 = start.M41 + ((end.M41 - start.M41) * amount);
            result.M42 = start.M42 + ((end.M42 - start.M42) * amount);
            result.M43 = start.M43 + ((end.M43 - start.M43) * amount);
            result.M44 = start.M44 + ((end.M44 - start.M44) * amount);
        }

        #endregion

        #endregion

        #region > Creation Helpers <
        
        #region FromAxisAngle

        /// <summary>Creates a new result that rotates around an arbitrary point.</summary>
        /// <param name="axis">The axis to rotate around.</param>
        /// <param name="angle">The angle to rotate around the point.</param>
        /// <returns>The created result.</returns>
        public static Matrix4 FromAxisAngle( Vector3 axis, float angle )
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float num2 = (float)System.Math.Sin( (double)angle );
            float num = (float)System.Math.Cos( (double)angle );
            float num11 = x * x;
            float num10 = y * y;
            float num9 = z * z;
            float num8 = x * y;
            float num7 = x * z;
            float num6 = y * z;

            Matrix4 result;

            result.M11 = num11 + (num * (1f - num11));
            result.M12 = (num8 - (num * num8)) + (num2 * z);
            result.M13 = (num7 - (num * num7)) - (num2 * y);
            result.M14 = 0f;
            result.M21 = (num8 - (num * num8)) - (num2 * z);
            result.M22 = num10 + (num * (1f - num10));
            result.M23 = (num6 - (num * num6)) + (num2 * x);
            result.M24 = 0f;
            result.M31 = (num7 - (num * num7)) + (num2 * y);
            result.M32 = (num6 - (num * num6)) - (num2 * x);
            result.M33 = num9 + (num * (1f - num9));

            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;

            return result;
        }

        /// <summary>
        /// Creates a new result that rotates around an arbitrary point.
        /// </summary>
        /// <param name="axis">The axis to rotate around.</param>
        /// <param name="angle">The angle to rotate around the point.</param>
        /// <param name="result">The created result.</param>
        public static void FromAxisAngle( ref Vector3 axis, float angle, out Matrix4 result )
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float sin = (float)System.Math.Sin( angle );
            float cos = (float)System.Math.Cos( angle );

            float xx = x * x;
            float yy = y * y;
            float zz = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;

            result.M11 = xx + (cos * (1f - xx));
            result.M12 = xy - (cos * xy) + (sin * z);
            result.M13 = xz - (cos * xz) - (sin * y);
            result.M14 = 0.0f;
            
            result.M21 = xy - (cos * xy) - (sin * z);
            result.M22 = yy + (cos * (1f - yy));
            result.M23 = yz - (cos * yz) + (sin * x);
            result.M24 = 0.0f;
            
            result.M31 = xz - (cos * xz) + (sin * y);
            result.M32 = yz - (cos * yz) - (sin * x);
            result.M33 = zz + (cos * (1f - zz));
            result.M34 = 0.0f;

            result.M41 = 0.0f;
            result.M42 = 0.0f;
            result.M43 = 0.0f;
            result.M44 = 1.0f;
        }

        #endregion

        #region FromQuaternion

        /// <summary>Creates a rotation result from a Quaternion.</summary>
        /// <param name="quaternion">Quaternion to create the result from.</param>
        /// <returns>The created result.</returns>
        public static Matrix4 FromQuaternion( Quaternion quaternion )
        {
            Matrix4 result;

            float xx = quaternion.X * quaternion.X;
            float yy = quaternion.Y * quaternion.Y;
            float zz = quaternion.Z * quaternion.Z;
            float xy = quaternion.X * quaternion.Y;
            float zw = quaternion.Z * quaternion.W;
            float zx = quaternion.Z * quaternion.X;
            float yw = quaternion.Y * quaternion.W;
            float yz = quaternion.Y * quaternion.Z;
            float xw = quaternion.X * quaternion.W;

            result.M11 = 1f - (2f * (yy + zz));
            result.M12 = 2f * (xy + zw);
            result.M13 = 2f * (zx - yw);
            result.M14 = 0f;

            result.M21 = 2f * (xy - zw);
            result.M22 = 1f - (2f * (zz + xx));
            result.M23 = 2f * (yz + xw);
            result.M24 = 0f;

            result.M31 = 2f * (zx + yw);
            result.M32 = 2f * (yz - xw);
            result.M33 = 1f - (2f * (yy + xx));
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;

            return result;
        }

        /// <summary>
        /// Creates a rotation result from a Quaternion.
        /// </summary>
        /// <param name="quaternion">Quaternion to create the result from.</param>
        /// <param name="result">The created result.</param>
        public static void FromQuaternion( ref Quaternion quaternion, out Matrix4 result )
        {
            float xx = quaternion.X * quaternion.X;
            float yy = quaternion.Y * quaternion.Y;
            float zz = quaternion.Z * quaternion.Z;
            float xy = quaternion.X * quaternion.Y;
            float zw = quaternion.Z * quaternion.W;
            float zx = quaternion.Z * quaternion.X;
            float yw = quaternion.Y * quaternion.W;
            float yz = quaternion.Y * quaternion.Z;
            float xw = quaternion.X * quaternion.W;

            result.M11 = 1f - (2f * (yy + zz));
            result.M12 = 2f * (xy + zw);
            result.M13 = 2f * (zx - yw);
            result.M14 = 0f;

            result.M21 = 2f * (xy - zw);
            result.M22 = 1f - (2f * (zz + xx));
            result.M23 = 2f * (yz + xw);
            result.M24 = 0f;

            result.M31 = 2f * (zx + yw);
            result.M32 = 2f * (yz - xw);
            result.M33 = 1f - (2f * (yy + xx));
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        #endregion

        #region FromYawPitchRoll

        /// <summary>Creates a new rotation result from a specified yaw, pitch, and roll.</summary>
        /// <param name="yaw">Angle of rotation, in radians, around the y-axis.</param>
        /// <param name="pitch">Angle of rotation, in radians, around the x-axis.</param>
        /// <param name="roll">Angle of rotation, in radians, around the z-axis.</param>
        /// <returns>A new rotation result with the specified yaw, pitch, and roll.</returns>
        public static Matrix4 FromYawPitchRoll( float yaw, float pitch, float roll )
        {
            return FromQuaternion( Quaternion.FromYawPitchRoll( yaw, pitch, roll ) );
        }

        /// <summary>Fills in a rotation result from a specified yaw, pitch, and roll.</summary>
        /// <param name="yaw">Angle of rotation, in radians, around the y-axis.</param>
        /// <param name="pitch">Angle of rotation, in radians, around the x-axis.</param>
        /// <param name="roll">Angle of rotation, in radians, around the z-axis.</param>
        /// <param name="result">An existing result filled in to represent the specified yaw, pitch, and roll.</param>
        public static void FromYawPitchRoll( float yaw, float pitch, float roll, out Matrix4 result )
        {
            Quaternion quaternion = Quaternion.FromYawPitchRoll( yaw, pitch, roll );
            FromQuaternion( ref quaternion, out result );
        }

        #endregion

        #region CreateBillboard

        /// <summary>Creates a spherical billboard that rotates around a specified object position.</summary>
        /// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="cameraUpVector">The up point of the camera.</param>
        /// <param name="cameraForwardVector">Optional forward point of the camera.</param>
        /// <returns>The created billboard result.</returns>
        public static Matrix4 CreateBillboard( Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3? cameraForwardVector )
        {
            Vector3 offset;
            offset.X = objectPosition.X - cameraPosition.X;
            offset.Y = objectPosition.Y - cameraPosition.Y;
            offset.Z = objectPosition.Z - cameraPosition.Z;

            float squaredLength = offset.SquaredLength;
            if( squaredLength < 0.0001f )
            {
                offset = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
            }
            else
            {
                Vector3.Multiply( ref offset, (float)(1f / (float)System.Math.Sqrt( (double)squaredLength )), out offset );
            }

            Vector3 vector3;
            Vector3.Cross( ref cameraUpVector, ref offset, out vector3 );
            vector3.Normalize();

            Vector3 vector2;
            Vector3.Cross( ref offset, ref vector3, out vector2 );

            Matrix4 result;
            result.M11 = vector3.X;
            result.M12 = vector3.Y;
            result.M13 = vector3.Z;
            result.M14 = 0f;

            result.M21 = vector2.X;
            result.M22 = vector2.Y;
            result.M23 = vector2.Z;
            result.M24 = 0f;

            result.M31 = offset.X;
            result.M32 = offset.Y;
            result.M33 = offset.Z;
            result.M34 = 0f;

            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1f;

            return result;
        }

        /// <summary>Creates a spherical billboard that rotates around a specified object position.</summary>
        /// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="cameraUpVector">The up point of the camera.</param>
        /// <param name="cameraForwardVector">Optional forward point of the camera.</param>
        /// <param name="result">The created billboard result.</param>
        public static void CreateBillboard( ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 cameraUpVector, Vector3? cameraForwardVector, out Matrix4 result )
        {
            Vector3 deltaPosition;

            deltaPosition.X = objectPosition.X - cameraPosition.X;
            deltaPosition.Y = objectPosition.Y - cameraPosition.Y;
            deltaPosition.Z = objectPosition.Z - cameraPosition.Z;

            float deltaPositionSquaredLength = deltaPosition.SquaredLength;
            if( deltaPositionSquaredLength < 0.0001f )
            {
                deltaPosition = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
            }
            else
            {
                Vector3.Multiply( ref deltaPosition, 1f / ((float)System.Math.Sqrt( deltaPositionSquaredLength )), out deltaPosition );
            }

            Vector3 crossPosWithUp;
            Vector3.Cross( ref cameraUpVector, ref deltaPosition, out crossPosWithUp );
            crossPosWithUp.Normalize();

            Vector3 crossPosWithCrossPosUp;
            Vector3.Cross( ref deltaPosition, ref crossPosWithUp, out crossPosWithCrossPosUp );

            result.M11 = crossPosWithUp.X;
            result.M12 = crossPosWithUp.Y;
            result.M13 = crossPosWithUp.Z;
            result.M14 = 0f;
            result.M21 = crossPosWithCrossPosUp.X;
            result.M22 = crossPosWithCrossPosUp.Y;
            result.M23 = crossPosWithCrossPosUp.Z;
            result.M24 = 0f;
            result.M31 = deltaPosition.X;
            result.M32 = deltaPosition.Y;
            result.M33 = deltaPosition.Z;
            result.M34 = 0f;
            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1f;
        }

        #endregion

        #region CreateConstrainedBillboard

        /// <summary>Creates a cylindrical billboard that rotates around a specified axis.</summary>
        /// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="rotateAxis">Axis to rotate the billboard around.</param>
        /// <param name="cameraForwardVector">Optional forward point of the camera.</param>
        /// <param name="objectForwardVector">Optional forward point of the object.</param>
        /// <returns>The created billboard result.</returns>
        public static Matrix4 CreateConstrainedBillboard( Vector3 objectPosition, Vector3 cameraPosition, Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector )
        {
            float num;
            Vector3 vector;
            Vector3 vector3;

            Vector3 offset;
            offset.X = objectPosition.X - cameraPosition.X;
            offset.Y = objectPosition.Y - cameraPosition.Y;
            offset.Z = objectPosition.Z - cameraPosition.Z;

            float squaredLength = offset.SquaredLength;
            if( squaredLength < 0.0001f )
            {
                offset = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
            }
            else
            {
                Vector3.Multiply( ref offset, (float)(1f / ((float)System.Math.Sqrt( (double)squaredLength ))), out offset );
            }

            Vector3 vector4 = rotateAxis;
            Vector3.Dot( ref rotateAxis, ref offset, out num );
            if( System.Math.Abs( num ) > 0.9982547f )
            {
                if( objectForwardVector.HasValue )
                {
                    vector = objectForwardVector.Value;
                    Vector3.Dot( ref rotateAxis, ref vector, out num );
                    if( System.Math.Abs( num ) > 0.9982547f )
                    {
                        num = ((rotateAxis.X * Vector3.Forward.X) + (rotateAxis.Y * Vector3.Forward.Y)) + (rotateAxis.Z * Vector3.Forward.Z);
                        vector = (System.Math.Abs( num ) > 0.9982547f) ? Vector3.Right : Vector3.Forward;
                    }
                }
                else
                {
                    num = ((rotateAxis.X * Vector3.Forward.X) + (rotateAxis.Y * Vector3.Forward.Y)) + (rotateAxis.Z * Vector3.Forward.Z);
                    vector = (System.Math.Abs( num ) > 0.9982547f) ? Vector3.Right : Vector3.Forward;
                }

                Vector3.Cross( ref rotateAxis, ref vector, out vector3 );
                vector3.Normalize();
                Vector3.Cross( ref vector3, ref rotateAxis, out vector );
                vector.Normalize();
            }
            else
            {
                Vector3.Cross( ref rotateAxis, ref offset, out vector3 );
                vector3.Normalize();
                Vector3.Cross( ref vector3, ref vector4, out vector );
                vector.Normalize();
            }

            Matrix4 result;

            result.M11 = vector3.X;
            result.M12 = vector3.Y;
            result.M13 = vector3.Z;
            result.M14 = 0f;

            result.M21 = vector4.X;
            result.M22 = vector4.Y;
            result.M23 = vector4.Z;
            result.M24 = 0f;

            result.M31 = vector.X;
            result.M32 = vector.Y;
            result.M33 = vector.Z;
            result.M34 = 0f;

            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1f;

            return result;
        }

        /// <summary>Creates a cylindrical billboard that rotates around a specified axis.</summary>
        /// <param name="objectPosition">Position of the object the billboard will rotate around.</param>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="rotateAxis">Axis to rotate the billboard around.</param>
        /// <param name="cameraForwardVector">Optional forward point of the camera.</param>
        /// <param name="objectForwardVector">Optional forward point of the object.</param>
        /// <param name="result">The created billboard result.</param>
        public static void CreateConstrainedBillboard( ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector, out Matrix4 result )
        {
            float num;
            Vector3 vector;
            Vector3 vector3;

            Vector3 offset;
            offset.X = objectPosition.X - cameraPosition.X;
            offset.Y = objectPosition.Y - cameraPosition.Y;
            offset.Z = objectPosition.Z - cameraPosition.Z;

            float squaredLength = offset.SquaredLength;
            if( squaredLength < 0.0001f )
            {
                offset = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3.Forward;
            }
            else
            {
                Vector3.Multiply( ref offset, (float)(1f / ((float)System.Math.Sqrt( (double)squaredLength ))), out offset );
            }

            Vector3 vector4 = rotateAxis;
            Vector3.Dot( ref rotateAxis, ref offset, out num );
            if( System.Math.Abs( num ) > 0.9982547f )
            {
                if( objectForwardVector.HasValue )
                {
                    vector = objectForwardVector.Value;
                    Vector3.Dot( ref rotateAxis, ref vector, out num );

                    if( System.Math.Abs( num ) > 0.9982547f )
                    {
                        num = ((rotateAxis.X * Vector3.Forward.X) + (rotateAxis.Y * Vector3.Forward.Y)) + (rotateAxis.Z * Vector3.Forward.Z);
                        vector = (System.Math.Abs( num ) > 0.9982547f) ? Vector3.Right : Vector3.Forward;
                    }
                }
                else
                {
                    num = ((rotateAxis.X * Vector3.Forward.X) + (rotateAxis.Y * Vector3.Forward.Y)) + (rotateAxis.Z * Vector3.Forward.Z);
                    vector = (System.Math.Abs( num ) > 0.9982547f) ? Vector3.Right : Vector3.Forward;
                }

                Vector3.Cross( ref rotateAxis, ref vector, out vector3 );
                vector3.Normalize();
                Vector3.Cross( ref vector3, ref rotateAxis, out vector );
                vector.Normalize();
            }
            else
            {
                Vector3.Cross( ref rotateAxis, ref offset, out vector3 );
                vector3.Normalize();
                Vector3.Cross( ref vector3, ref vector4, out vector );
                vector.Normalize();
            }

            result.M11 = vector3.X;
            result.M12 = vector3.Y;
            result.M13 = vector3.Z;
            result.M14 = 0f;

            result.M21 = vector4.X;
            result.M22 = vector4.Y;
            result.M23 = vector4.Z;
            result.M24 = 0f;

            result.M31 = vector.X;
            result.M32 = vector.Y;
            result.M33 = vector.Z;
            result.M34 = 0f;

            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1f;
        }

        #endregion

        #region CreateTranslation

        /// <summary>
        /// Creates a translation result.
        /// </summary>
        /// <param name="position">Amounts to translate by on the x, y, and z axes.</param>
        /// <returns>The created translation result.</returns>
        public static Matrix4 CreateTranslation( Vector3 position )
        {
            Matrix4 result;

            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;

            result.M41 = position.X;
            result.M42 = position.Y;
            result.M43 = position.Z;
            result.M44 = 1f;

            return result;
        }

        /// <summary>
        /// Creates a translation result.
        /// </summary>
        /// <param name="position">Amounts to translate by on the x, y, and z axes.</param>
        /// <param name="result">The created translation result.</param>
        public static void CreateTranslation( ref Vector3 position, out Matrix4 result )
        {
            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;

            result.M41 = position.X;
            result.M42 = position.Y;
            result.M43 = position.Z;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a translation result.
        /// </summary>
        /// <param name="positionX">Value to translate by on the x-axis.</param>
        /// <param name="positionY">Value to translate by on the y-axis.</param>
        /// <param name="positionZ">Value to translate by on the z-axis.</param>
        /// <returns>The created translation result.</returns>
        public static Matrix4 CreateTranslation( float positionX, float positionY, float positionZ )
        {
            Matrix4 result;

            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;

            result.M41 = positionX;
            result.M42 = positionY;
            result.M43 = positionZ;
            result.M44 = 1f;

            return result;
        }

        /// <summary>
        /// Creates a translation result.
        /// </summary>
        /// <param name="positionX">Value to translate by on the x-axis.</param>
        /// <param name="positionY">Value to translate by on the y-axis.</param>
        /// <param name="positionZ">Value to translate by on the z-axis.</param>
        /// <param name="result">The created translation result.</param>
        public static void CreateTranslation( float positionX, float positionY, float positionZ, out Matrix4 result )
        {
            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;

            result.M41 = positionX;
            result.M42 = positionY;
            result.M43 = positionZ;
            result.M44 = 1f;
        }

        #endregion

        #region CreateScale

        /// <summary>
        /// Creates a scaling result.
        /// </summary>
        /// <param name="scaleX">Value to scale by on the x-axis.</param>
        /// <param name="scaleY">Value to scale by on the y-axis.</param>
        /// <param name="scaleZ">Value to scale by on the z-axis.</param>
        /// <returns>
        /// The created scaling result.
        /// </returns>
        public static Matrix4 CreateScale( float scaleX, float scaleY, float scaleZ )
        {
            Matrix4 result;

            result.M11 = scaleX;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = scaleY;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = scaleZ;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;

            return result;
        }

        /// <summary>
        /// Creates a scaling result.
        /// </summary>
        /// <param name="scaleX">Value to scale by on the x-axis.</param>
        /// <param name="scaleY">Value to scale by on the y-axis.</param>
        /// <param name="scaleZ">Value to scale by on the z-axis.</param>
        /// <param name="result">The created scaling result.</param>
        public static void CreateScale( float scaleX, float scaleY, float scaleZ, out Matrix4 result )
        {
            result.M11 = scaleX;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = scaleY;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = scaleZ;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a scaling result.
        /// </summary>
        /// <param name="scales">Amounts to scale by on the x, y, and z axes.</param>
        /// <returns>
        /// The created scaling result.
        /// </returns>
        public static Matrix4 CreateScale( Vector3 scales )
        {
            Matrix4 result;

            result.M11 = scales.X;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = scales.Y;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = scales.Z;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;

            return result;
        }

        /// <summary>
        /// Creates a scaling result.
        /// </summary>
        /// <param name="scales">Amounts to scale by on the x, y, and z axes.</param>
        /// <param name="result">The created scaling result.</param>
        public static void CreateScale( ref Vector3 scales, out Matrix4 result )
        {
            result.M11 = scales.X;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = scales.Y;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = scales.Z;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>Creates a scaling result.</summary>
        /// <param name="scale">Amount to scale by.</param>
        /// <returns>The created scaling result.</returns>
        public static Matrix4 CreateScale( float scale )
        {
            Matrix4 result;

            result.M11 = scale;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = scale;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = scale;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;

            return result;
        }

        /// <summary>
        /// Creates a scaling result.
        /// </summary>
        /// <param name="scale">Value to scale by.</param>
        /// <param name="result">The created scaling result.</param>
        public static void CreateScale( float scale, out Matrix4 result )
        {
            result.M11 = scale;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = scale;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = scale;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        #endregion

        #region CreateRotation

        /// <summary>Returns an x-axis rotation result.</summary>
        /// <param name="radians">The rotation in radians.</param>
        /// <returns>The rotation result.</returns>
        public static Matrix4 CreateRotationX( float radians )
        {
            float cos = (float)System.Math.Cos( (double)radians );
            float sin = (float)System.Math.Sin( (double)radians );

            Matrix4 result;

            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = cos;
            result.M23 = sin;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = -sin;
            result.M33 = cos;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;

            return result;
        }

        /// <summary>Returns an x-axis rotation result.</summary>
        /// <param name="radians">The rotation in radians.</param>
        /// <param name="result">The rotation result.</param>
        public static void CreateRotationX( float radians, out Matrix4 result )
        {
            float cos = (float)System.Math.Cos( (double)radians );
            float sin = (float)System.Math.Sin( (double)radians );

            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = cos;
            result.M23 = sin;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = -sin;
            result.M33 = cos;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>Returns a y-axis rotation result.</summary>
        /// <param name="radians">The rotation in radians.</param>
        /// <returns>The rotation result.</returns>
        public static Matrix4 CreateRotationY( float radians )
        {
            float cos = (float)System.Math.Cos( (double)radians );
            float sin = (float)System.Math.Sin( (double)radians );

            Matrix4 result;

            result.M11 = cos;
            result.M12 = 0f;
            result.M13 = -sin;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = sin;
            result.M32 = 0f;
            result.M33 = cos;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;

            return result;
        }

        /// <summary>Returns a y-axis rotation result.</summary>
        /// <param name="radians">The rotation in radians.</param>
        /// <param name="result">The rotation result.</param>
        public static void CreateRotationY( float radians, out Matrix4 result )
        {
            float cos = (float)System.Math.Cos( (double)radians );
            float sin = (float)System.Math.Sin( (double)radians );

            result.M11 = cos;
            result.M12 = 0f;
            result.M13 = -sin;
            result.M14 = 0f;

            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = sin;
            result.M32 = 0f;
            result.M33 = cos;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>Returns a z-axis rotation result.</summary>
        /// <param name="radians">The rotation in radians.</param>
        /// <returns>The rotation result.</returns>
        public static Matrix4 CreateRotationZ( float radians )
        {
            float cos = (float)System.Math.Cos( (double)radians );
            float sin = (float)System.Math.Sin( (double)radians );

            Matrix4 result;

            result.M11 = cos;
            result.M12 = sin;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = -sin;
            result.M22 = cos;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;

            return result;
        }

        /// <summary>Returns an z-axis rotation result.</summary>
        /// <param name="radians">The rotation in radians.</param>
        /// <param name="result">The rotation result.</param>
        public static void CreateRotationZ( float radians, out Matrix4 result )
        {
            float cos = (float)System.Math.Cos( (double)radians );
            float sin = (float)System.Math.Sin( (double)radians );

            result.M11 = cos;
            result.M12 = sin;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M21 = -sin;
            result.M22 = cos;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        #endregion

        #region CreatePerspectiveFieldOfView

        /// <summary>Builds a perspective projection result based on a field of view.</summary>
        /// <param name="fieldOfView">Field of view in radians.</param>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to the far view plane.</param>
        /// <returns>The perspective projection result.</returns>
        public static Matrix4 CreatePerspectiveFieldOfView( float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance )
        {
            if( (fieldOfView <= 0f) || (fieldOfView >= 3.141593f) )
            {
                throw new ArgumentOutOfRangeException(
                    "fieldOfView",
                    string.Format( CultureInfo.CurrentCulture, MathErrorStrings.OutRangeFieldOfView, fieldOfView )
                );
            }

            if( nearPlaneDistance <= 0f )
            {
                throw new ArgumentOutOfRangeException(
                    "nearPlaneDistance",
                    string.Format( CultureInfo.CurrentCulture, MathErrorStrings.NegativePlaneDistance, nearPlaneDistance )
                );
            }

            if( farPlaneDistance <= 0f )
            {
                throw new ArgumentOutOfRangeException(
                    "farPlaneDistance",
                    string.Format( CultureInfo.CurrentCulture, MathErrorStrings.NegativePlaneDistance, farPlaneDistance )
                );
            }

            if( nearPlaneDistance >= farPlaneDistance )
            {
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", MathErrorStrings.OppositePlanes );
            }

            float num  = 1f / (float)System.Math.Tan( (double)(fieldOfView * 0.5f) );
            float num9 = num / aspectRatio;
            
            Matrix4 result;

            result.M11 = num9;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M22 = num;
            result.M21 = 0f;
            result.M23 = 0f; 
            result.M24 = 0f;

            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M34 = -1f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M44 = 0f;
            result.M43 = (nearPlaneDistance * farPlaneDistance) / (nearPlaneDistance - farPlaneDistance);
            
            return result;
        }

        #endregion

        #region CreatePerspective

        /// <summary>Builds a perspective projection result.</summary>
        /// <param name="width">Width of the view volume at the near view plane.</param>
        /// <param name="height">Height of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to the far view plane.</param>
        /// <returns>The projection result.</returns>
        public static Matrix4 CreatePerspective( float width, float height, float nearPlaneDistance, float farPlaneDistance )
        {
            if( nearPlaneDistance <= 0f )
            {
                throw new ArgumentOutOfRangeException(
                    "nearPlaneDistance",
                    string.Format( CultureInfo.CurrentCulture, MathErrorStrings.NegativePlaneDistance, nearPlaneDistance ) 
                );
            }

            if( farPlaneDistance <= 0f )
            {
                throw new ArgumentOutOfRangeException(
                    "farPlaneDistance",
                    string.Format( CultureInfo.CurrentCulture, MathErrorStrings.NegativePlaneDistance, farPlaneDistance )
                );
            }

            if( nearPlaneDistance >= farPlaneDistance )
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", MathErrorStrings.OppositePlanes );

            Matrix4 result;

            result.M11 = (2f * nearPlaneDistance) / width;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M22 = (2f * nearPlaneDistance) / height;
            result.M21 = 0f;
            result.M23 = 0f;
            result.M24 = 0f;
            
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M31 = 0f;
            result.M32 = 0f;
            result.M34 = -1f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M44 = 0f;
            result.M43 = (nearPlaneDistance * farPlaneDistance) / (nearPlaneDistance - farPlaneDistance);

            return result;
        }

        #endregion

        #region CreatePerspectiveOffCenter

        /// <summary>Builds a customized, perspective projection result.</summary>
        /// <param name="left">Minimum x-value of the view volume at the near view plane.</param>
        /// <param name="right">Maximum x-value of the view volume at the near view plane.</param>
        /// <param name="bottom">Minimum y-value of the view volume at the near view plane.</param>
        /// <param name="top">Maximum y-value of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to of the far view plane.</param>
        /// <returns>The created projection result.</returns>
        public static Matrix4 CreatePerspectiveOffCenter( float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance )
        {
            if( nearPlaneDistance <= 0f )
            {
                throw new ArgumentOutOfRangeException(
                    "nearPlaneDistance",
                    string.Format( CultureInfo.CurrentCulture, MathErrorStrings.NegativePlaneDistance, nearPlaneDistance )
                );
            }

            if( farPlaneDistance <= 0f )
            {
                throw new ArgumentOutOfRangeException(
                    "farPlaneDistance",
                    string.Format( CultureInfo.CurrentCulture, MathErrorStrings.NegativePlaneDistance, farPlaneDistance )
                );
            }

            if( nearPlaneDistance >= farPlaneDistance )
            {
                throw new ArgumentOutOfRangeException( "nearPlaneDistance", MathErrorStrings.OppositePlanes );
            }

            Matrix4 result;

            result.M11 = (2f * nearPlaneDistance) / (right - left);
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M22 = (2f * nearPlaneDistance) / (top - bottom);
            result.M21 = 0f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M31 = (left + right) / (right - left);
            result.M32 = (top + bottom) / (top - bottom);
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M34 = -1f;

            result.M43 = (nearPlaneDistance * farPlaneDistance) / (nearPlaneDistance - farPlaneDistance);
            result.M41 = 0f;
            result.M42 = 0f;
            result.M44 = 0f;

            return result;
        }

        #endregion

        #region CreateOrthographic

        /// <summary>Builds an orthogonal projection result.</summary>
        /// <param name="width">Width of the view volume.</param>
        /// <param name="height">Height of the view volume.</param>
        /// <param name="nearPlaneZ">Minimum z-value of the view volume.</param>
        /// <param name="farPlaneZ">Maximum z-value of the view volume.</param>
        /// <returns>The projection result.</returns>
        public static Matrix4 CreateOrthographic( float width, float height, float nearPlaneZ, float farPlaneZ )
        {
            Matrix4 result;

            result.M11 = 2f / width;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M22 = 2f / height;
            result.M21 = 0f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M33 = 1f / (nearPlaneZ - farPlaneZ);
            result.M31 = 0f;
            result.M32 = 0f;
            result.M34 = 0f;

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = nearPlaneZ / (nearPlaneZ - farPlaneZ);
            result.M44 = 1f;

            return result;
        }

        #endregion

        #region CreateOrthographicOffCenter

        /// <summary>Builds a customized, orthogonal projection result.</summary>
        /// <param name="left">Minimum x-value of the view volume.</param>
        /// <param name="right">Maximum x-value of the view volume.</param>
        /// <param name="bottom">Minimum y-value of the view volume.</param>
        /// <param name="top">Maximum y-value of the view volume.</param>
        /// <param name="nearPlaneZ">Minimum z-value of the view volume.</param>
        /// <param name="farPlaneZ">Maximum z-value of the view volume.</param>
        /// <returns>The projection result.</returns>
        public static Matrix4 CreateOrthographicOffCenter( float left, float right, float bottom, float top, float nearPlaneZ, float farPlaneZ )
        {
            Matrix4 result;

            result.M11 = 2f / (right - left);
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;

            result.M22 = 2f / (top - bottom);
            result.M21 = 0f;
            result.M23 = 0f;
            result.M24 = 0f;

            result.M33 = 1f / (nearPlaneZ - farPlaneZ);
            result.M31 = 0f;
            result.M32 = 0f;
            result.M34 = 0f;

            result.M41 = (left + right) / (left - right);
            result.M42 = (top + bottom) / (bottom - top);
            result.M43 = nearPlaneZ / (nearPlaneZ - farPlaneZ);
            result.M44 = 1f;

            return result;
        }

        #endregion

        #region CreateLookAt

        /// <summary>Creates a view result.</summary>
        /// <param name="cameraPosition">The position of the camera.</param>
        /// <param name="cameraTarget">The direction that the camera is pointing.</param>
        /// <param name="cameraUpVector">The direction that is "up" from the camera's point of view.</param>
        /// <returns>The created view result.</returns>
        public static Matrix4 CreateLookAt( Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector )
        {
            Vector3 offset  = Vector3.Normalize( cameraPosition - cameraTarget );
            Vector3 vector2 = Vector3.Normalize( Vector3.Cross( cameraUpVector, offset ) );
            Vector3 vector3 = Vector3.Cross( offset, vector2 );

            Matrix4 result;

            result.M11 = vector2.X;
            result.M12 = vector3.X;
            result.M13 = offset.X;
            result.M14 = 0f;

            result.M21 = vector2.Y;
            result.M22 = vector3.Y;
            result.M23 = offset.Y;
            result.M24 = 0f;

            result.M31 = vector2.Z;
            result.M32 = vector3.Z;
            result.M33 = offset.Z;
            result.M34 = 0f;

            result.M41 = -Vector3.Dot( vector2, cameraPosition );
            result.M42 = -Vector3.Dot( vector3, cameraPosition );
            result.M43 = -Vector3.Dot( offset, cameraPosition );
            result.M44 = 1f;

            return result;
        }

        /// <summary>Creates a view result.</summary>
        /// <param name="cameraPosition">The position of the camera.</param>
        /// <param name="cameraTarget">The direction that the camera is pointing.</param>
        /// <param name="cameraUpVector">The direction that is "up" from the camera's point of view.</param>
        /// <param name="result">The created view result.</param>
        public static void CreateLookAt( ref Vector3 cameraPosition, ref Vector3 cameraTarget, ref Vector3 cameraUpVector, out Matrix4 result )
        {
            Vector3 vector = Vector3.Normalize( cameraPosition - cameraTarget );
            Vector3 vector2 = Vector3.Normalize( Vector3.Cross( cameraUpVector, vector ) );
            Vector3 vector3 = Vector3.Cross( vector, vector2 );
            result.M11 = vector2.X;
            result.M12 = vector3.X;
            result.M13 = vector.X;
            result.M14 = 0f;
            result.M21 = vector2.Y;
            result.M22 = vector3.Y;
            result.M23 = vector.Y;
            result.M24 = 0f;
            result.M31 = vector2.Z;
            result.M32 = vector3.Z;
            result.M33 = vector.Z;
            result.M34 = 0f;
            result.M41 = -Vector3.Dot( vector2, cameraPosition );
            result.M42 = -Vector3.Dot( vector3, cameraPosition );
            result.M43 = -Vector3.Dot( vector, cameraPosition );
            result.M44 = 1f;
        }

        #endregion

        #region CreateWorld

        /// <summary>Creates a world result with the specified parameters.</summary>
        /// <param name="position">Position of the object. This value is used in translation operations.</param>
        /// <param name="forward">Forward direction of the object.</param>
        /// <param name="up">Upward direction of the object; usually [0, 1, 0].</param>
        /// <returns>The created world result.</returns>
        public static Matrix4 CreateWorld( Vector3 position, Vector3 forward, Vector3 up )
        {
            Vector3 offset  = Vector3.Normalize( position - forward );
            Vector3 vector2 = Vector3.Normalize( Vector3.Cross( up, offset ) );
            Vector3 vector3 = Vector3.Cross( offset, vector2 );

            Matrix4 result;

            result.M11 = vector2.X;
            result.M12 = vector2.Y;
            result.M13 = vector2.Z;

            result.M14 = -Vector3.Dot( vector2, position );

            result.M21 = vector3.X;
            result.M22 = vector3.Y;
            result.M23 = vector3.Z;
            result.M24 = -Vector3.Dot( vector3, position );

            result.M31 = offset.X;
            result.M32 = offset.Y;
            result.M33 = offset.Z;
            result.M34 = -Vector3.Dot( offset, position );

            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;

            return result;
        }

        /// <summary>Creates a world result with the specified parameters.</summary>
        /// <param name="position">Position of the object. This value is used in translation operations.</param>
        /// <param name="forward">Forward direction of the object.</param>
        /// <param name="up">Upward direction of the object; usually [0, 1, 0].</param>
        /// <param name="result">The created world result.</param>
        public static void CreateWorld( ref Vector3 position, ref Vector3 forward, ref Vector3 up, out Matrix4 result )
        {
            Vector3 vector = Vector3.Normalize( position - forward );
            Vector3 vector2 = Vector3.Normalize( Vector3.Cross( up, vector ) );
            Vector3 vector3 = Vector3.Cross( vector, vector2 );
            result.M11 = vector2.X;
            result.M12 = vector2.Y;
            result.M13 = vector2.Z;
            result.M21 = vector3.X;
            result.M22 = vector3.Y;
            result.M23 = vector3.Z;
            result.M31 = vector.X;
            result.M32 = vector.Y;
            result.M33 = vector.Z;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            result.M14 = -Vector3.Dot( vector2, position );
            result.M24 = -Vector3.Dot( vector3, position );
            result.M34 = -Vector3.Dot( vector, position );
        }

        #endregion

        #region CreateReflection

        /// <summary>
        /// Creates a <see cref="Matrix4"/> that reflects the coordinate system 
        /// about the specified <see cref="Plane3"/>.
        /// </summary>
        /// <param name="plane">The Plane about which to create a reflection.</param>
        /// <returns>
        /// A new result expressing the reflection.
        /// </returns>
        public static Matrix4 CreateReflection( Plane3 plane )
        {
            plane.Normalize();

            float x     = plane.Normal.X;
            float y     = plane.Normal.Y;
            float z     = plane.Normal.Z;
            float neg2X = -2f * x;
            float neg2Y = -2f * y;
            float neg2Z = -2f * z;

            Matrix4 result;

            result.M11 = (neg2X * x) + 1f;
            result.M12 = neg2Y * x;
            result.M13 = neg2Z * x;
            result.M14 = 0.0f;

            result.M21 = neg2X * y;
            result.M22 = (neg2Y * y) + 1f;
            result.M23 = neg2Z * y;
            result.M24 = 0.0f;

            result.M31 = neg2X * z;
            result.M32 = neg2Y * z;
            result.M33 = (neg2Z * z) + 1f;
            result.M34 = 0.0f;

            result.M41 = neg2X * plane.Distance;
            result.M42 = neg2Y * plane.Distance;
            result.M43 = neg2Z * plane.Distance;
            result.M44 = 0.0f;

            return result;
        }

        /// <summary>Fills in an existing result so that it reflects the coordinate system about a specified Plane.</summary>
        /// <param name="plane">The Plane about which to create a reflection.</param>
        /// <param name="result">A result that creates the reflection.</param>
        public static void CreateReflection( ref Plane3 plane, out Matrix4 result )
        {
            Plane3 planeNorm;
            Plane3.Normalize( ref plane, out planeNorm );

            float x     = planeNorm.Normal.X;
            float y     = planeNorm.Normal.Y;
            float z     = planeNorm.Normal.Z;
            float neg2X = -2f * x;
            float neg2Y = -2f * y;
            float neg2Z = -2f * z;

            result.M11 = (neg2X * x) + 1f;
            result.M12 = neg2Y * x;
            result.M13 = neg2Z * x;
            result.M14 = 0.0f;

            result.M21 = neg2X * y;
            result.M22 = (neg2Y * y) + 1f;
            result.M23 = neg2Z * y;
            result.M24 = 0.0f;

            result.M31 = neg2X * z;
            result.M32 = neg2Y * z;
            result.M33 = (neg2Z * z) + 1f;
            result.M34 = 0.0f;

            result.M41 = neg2X * plane.Distance;
            result.M42 = neg2Y * plane.Distance;
            result.M43 = neg2Z * plane.Distance;
            result.M44 = 0.0f;
        }

        #endregion

        #region CreateShadow

        /// <summary>
        /// Creates a result that flattens geometry into a specified Plane3 as if casting a shadow from a specified light source.
        /// </summary>
        /// <param name="lightDirection">A Vector3 specifying the direction from which the light that will cast the shadow is coming.</param>
        /// <param name="plane">The Plane onto which the new result should flatten geometry so as to cast a shadow.</param>
        /// <returns>
        /// A new result that can be used to flatten geometry onto the specified plane from the specified direction.
        /// </returns>
        public static Matrix4 CreateShadow( Vector3 lightDirection, Plane3 plane )
        {
            Plane3 normPlane;
            Plane3.Normalize( ref plane, out normPlane );

            float dot  = (normPlane.Normal.X * lightDirection.X) + (normPlane.Normal.Y * lightDirection.Y) + (normPlane.Normal.Z * lightDirection.Z);
            
            float negX = -normPlane.Normal.X;
            float negY = -normPlane.Normal.Y;
            float negZ = -normPlane.Normal.Z;
            float negD = -normPlane.Distance;

            Matrix4 result;

            result.M11 = (negX * lightDirection.X) + dot;
            result.M21 = negY * lightDirection.X;
            result.M31 = negZ * lightDirection.X;
            result.M41 = negD * lightDirection.X;

            result.M12 = negX * lightDirection.Y;
            result.M22 = (negY * lightDirection.Y) + dot;
            result.M32 = negZ * lightDirection.Y;
            result.M42 = negD * lightDirection.Y;

            result.M13 = negX * lightDirection.Z;
            result.M23 = negY * lightDirection.Z;
            result.M33 = (negZ * lightDirection.Z) + dot;
            result.M43 = negD * lightDirection.Z;

            result.M14 = 0.0f;
            result.M24 = 0.0f;
            result.M34 = 0.0f;
            result.M44 = dot;

            return result;
        }

        /// <summary>Fills in a result to flatten geometry into a specified Plane as if casting a shadow from a specified light source.</summary>
        /// <param name="lightDirection">A Vector3 specifying the direction from which the light that will cast the shadow is coming.</param>
        /// <param name="plane">The Plane onto which the new result should flatten geometry so as to cast a shadow.</param>
        /// <param name="result">A result that can be used to flatten geometry onto the specified plane from the specified direction.</param>
        public static void CreateShadow( ref Vector3 lightDirection, ref Plane3 plane, out Matrix4 result )
        {
            Plane3 normPlane;
            Plane3.Normalize( ref plane, out normPlane );

            float dot  = (normPlane.Normal.X * lightDirection.X) + (normPlane.Normal.Y * lightDirection.Y) + (normPlane.Normal.Z * lightDirection.Z);

            float negX = -normPlane.Normal.X;
            float negY = -normPlane.Normal.Y;
            float negZ = -normPlane.Normal.Z;
            float negD = -normPlane.Distance;

            result.M11 = (negX * lightDirection.X) + dot;
            result.M21 = negY * lightDirection.X;
            result.M31 = negZ * lightDirection.X;
            result.M41 = negD * lightDirection.X;

            result.M12 = negX * lightDirection.Y;
            result.M22 = (negY * lightDirection.Y) + dot;
            result.M32 = negZ * lightDirection.Y;
            result.M42 = negD * lightDirection.Y;

            result.M13 = negX * lightDirection.Z;
            result.M23 = negY * lightDirection.Z;
            result.M33 = (negZ * lightDirection.Z) + dot;
            result.M43 = negD * lightDirection.Z;

            result.M14 = 0.0f;
            result.M24 = 0.0f;
            result.M34 = 0.0f;
            result.M44 = dot;
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether the specified <see cref="Object"/> 
        /// is equal to this <see cref="Matrix4"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test against.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj == null )
                return false;

            if( obj is Matrix4 )
                return this.Equals( (Matrix4)obj );

            return false;
        }

        /// <summary>
        /// Returns whether the specified Matrix44 instance 
        /// is approximately equal to this <see cref="Matrix4"/>.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Matrix4"/> instance to test against.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the elements of the matrices are (approximately) equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals( Matrix4 other )
        {
            return this.M11.IsApproximate( other.M11 ) && this.M12.IsApproximate( other.M12 ) && 
                   this.M13.IsApproximate( other.M13 ) && this.M14.IsApproximate( other.M14 ) &&

                   this.M21.IsApproximate( other.M21 ) && this.M22.IsApproximate( other.M22 ) && 
                   this.M23.IsApproximate( other.M23 ) && this.M24.IsApproximate( other.M24 ) &&

                   this.M31.IsApproximate( other.M31 ) && this.M32.IsApproximate( other.M32 ) && 
                   this.M33.IsApproximate( other.M33 ) && this.M34.IsApproximate( other.M34 ) &&

                   this.M11.IsApproximate( other.M41 ) && this.M11.IsApproximate( other.M42 ) && 
                   this.M13.IsApproximate( other.M43 ) && this.M13.IsApproximate( other.M44 );
        }

        #endregion

        #region ToString

        /// <summary> 
        /// Returns a human-readable representation of this <see cref="Matrix4"/>.
        /// </summary>
        /// <returns> 
        /// A string representation of this <see cref="Matrix4"/>.
        /// </returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary> 
        /// Returns a human-readable representation of this <see cref="Matrix4"/>.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns> 
        /// A string representation of this <see cref="Matrix4"/>.
        /// </returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendFormat( formatProvider, "|{0} {1} {2} {3}|\n", M11.ToString( formatProvider ), M12.ToString( formatProvider ), M13.ToString( formatProvider ), M14.ToString( formatProvider ) );
            sb.AppendFormat( formatProvider, "|{0} {1} {2} {3}|\n", M21.ToString( formatProvider ), M22.ToString( formatProvider ), M23.ToString( formatProvider ), M24.ToString( formatProvider ) );
            sb.AppendFormat( formatProvider, "|{0} {1} {2} {3}|\n", M31.ToString( formatProvider ), M32.ToString( formatProvider ), M33.ToString( formatProvider ), M34.ToString( formatProvider ) );
            sb.AppendFormat( formatProvider, "|{0} {1} {2} {3}|\n", M41.ToString( formatProvider ), M42.ToString( formatProvider ), M43.ToString( formatProvider ), M44.ToString( formatProvider ) );

            return sb.ToString();
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets the hash code of this <see cref="Matrix4"/> instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.M11 );
            hashBuilder.AppendStruct( this.M12 );
            hashBuilder.AppendStruct( this.M13 );
            hashBuilder.AppendStruct( this.M14 );

            hashBuilder.AppendStruct( this.M21 );
            hashBuilder.AppendStruct( this.M22 );
            hashBuilder.AppendStruct( this.M23 );
            hashBuilder.AppendStruct( this.M24 );

            hashBuilder.AppendStruct( this.M31 );
            hashBuilder.AppendStruct( this.M32 );
            hashBuilder.AppendStruct( this.M33 );
            hashBuilder.AppendStruct( this.M34 );

            hashBuilder.AppendStruct( this.M41 );
            hashBuilder.AppendStruct( this.M42 );
            hashBuilder.AppendStruct( this.M43 );
            hashBuilder.AppendStruct( this.M44 );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #endregion

        #endregion

        #region > Operators <

        #region +

        /// <summary>
        /// Returns the result of adding the <paramref name="right"/> Matrix
        /// to the <paramref name="left"/> Matrix.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>
        /// <returns>
        /// The result of this operation.
        /// </returns>
        public static Matrix4 operator +( Matrix4 left, Matrix4 right )
        {
            Matrix4 result;

            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M13 = left.M13 + right.M13;
            result.M14 = left.M14 + right.M14;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;
            result.M23 = left.M23 + right.M23;
            result.M24 = left.M24 + right.M24;
            result.M31 = left.M31 + right.M31;
            result.M32 = left.M32 + right.M32;
            result.M33 = left.M33 + right.M33;
            result.M34 = left.M34 + right.M34;
            result.M41 = left.M41 + right.M41;
            result.M42 = left.M42 + right.M42;
            result.M43 = left.M43 + right.M43;
            result.M44 = left.M44 + right.M44;

            return result;
        }

        #endregion

        #region -

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> Matrix
        /// from the <paramref name="left"/> Matrix.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>
        /// <returns>
        /// The result of the subtraction.
        /// </returns>
        public static Matrix4 operator -( Matrix4 left, Matrix4 right )
        {
            Matrix4 result;

            result.M11 = left.M11 - right.M11;
            result.M12 = left.M12 - right.M12;
            result.M13 = left.M13 - right.M13;
            result.M14 = left.M14 - right.M14;
            result.M21 = left.M21 - right.M21;
            result.M22 = left.M22 - right.M22;
            result.M23 = left.M23 - right.M23;
            result.M24 = left.M24 - right.M24;
            result.M31 = left.M31 - right.M31;
            result.M32 = left.M32 - right.M32;
            result.M33 = left.M33 - right.M33;
            result.M34 = left.M34 - right.M34;
            result.M41 = left.M41 - right.M41;
            result.M42 = left.M42 - right.M42;
            result.M43 = left.M43 - right.M43;
            result.M44 = left.M44 - right.M44;

            return result;
        }

        /// <summary>
        /// Returns the result of negating the specified <see cref="Matrix4"/>.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>The negated matrix.</returns>
        public static Matrix4 operator -( Matrix4 matrix )
        {
            Matrix4 result;

            result.M11 = -matrix.M11;
            result.M12 = -matrix.M12;
            result.M13 = -matrix.M13;
            result.M14 = -matrix.M14;
            result.M21 = -matrix.M21;
            result.M22 = -matrix.M22;
            result.M23 = -matrix.M23;
            result.M24 = -matrix.M24;
            result.M31 = -matrix.M31;
            result.M32 = -matrix.M32;
            result.M33 = -matrix.M33;
            result.M34 = -matrix.M34;
            result.M41 = -matrix.M41;
            result.M42 = -matrix.M42;
            result.M43 = -matrix.M43;
            result.M44 = -matrix.M44;

            return result;
        }

        #endregion

        #region *

        /// <summary>
        /// Returns the result of multiplying the <paramref name="left"/> Matrix
        /// by the <paramref name="right"/> Matrix.
        /// Matrix multiplication can be used to 'combine' the rotations the matrices represent.
        /// </summary>
        /// <param name="left">The Matrix on the left side of the equation.</param>
        /// <param name="right">The Matrix on the right side of the equation.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Matrix4 operator *( Matrix4 left, Matrix4 right )
        {
            Matrix4 result;

            result.M11 = (((left.M11 * right.M11) + (left.M12 * right.M21)) + (left.M13 * right.M31)) + (left.M14 * right.M41);
            result.M12 = (((left.M11 * right.M12) + (left.M12 * right.M22)) + (left.M13 * right.M32)) + (left.M14 * right.M42);
            result.M13 = (((left.M11 * right.M13) + (left.M12 * right.M23)) + (left.M13 * right.M33)) + (left.M14 * right.M43);
            result.M14 = (((left.M11 * right.M14) + (left.M12 * right.M24)) + (left.M13 * right.M34)) + (left.M14 * right.M44);
            result.M21 = (((left.M21 * right.M11) + (left.M22 * right.M21)) + (left.M23 * right.M31)) + (left.M24 * right.M41);
            result.M22 = (((left.M21 * right.M12) + (left.M22 * right.M22)) + (left.M23 * right.M32)) + (left.M24 * right.M42);
            result.M23 = (((left.M21 * right.M13) + (left.M22 * right.M23)) + (left.M23 * right.M33)) + (left.M24 * right.M43);
            result.M24 = (((left.M21 * right.M14) + (left.M22 * right.M24)) + (left.M23 * right.M34)) + (left.M24 * right.M44);
            result.M31 = (((left.M31 * right.M11) + (left.M32 * right.M21)) + (left.M33 * right.M31)) + (left.M34 * right.M41);
            result.M32 = (((left.M31 * right.M12) + (left.M32 * right.M22)) + (left.M33 * right.M32)) + (left.M34 * right.M42);
            result.M33 = (((left.M31 * right.M13) + (left.M32 * right.M23)) + (left.M33 * right.M33)) + (left.M34 * right.M43);
            result.M34 = (((left.M31 * right.M14) + (left.M32 * right.M24)) + (left.M33 * right.M34)) + (left.M34 * right.M44);
            result.M41 = (((left.M41 * right.M11) + (left.M42 * right.M21)) + (left.M43 * right.M31)) + (left.M44 * right.M41);
            result.M42 = (((left.M41 * right.M12) + (left.M42 * right.M22)) + (left.M43 * right.M32)) + (left.M44 * right.M42);
            result.M43 = (((left.M41 * right.M13) + (left.M42 * right.M23)) + (left.M43 * right.M33)) + (left.M44 * right.M43);
            result.M44 = (((left.M41 * right.M14) + (left.M42 * right.M24)) + (left.M43 * right.M34)) + (left.M44 * right.M44);

            return result;
        }

        /// <summary>
        /// Returns the result of multiplying the specified <see cref="Matrix4"/>
        /// by the specified <paramref name="scalar"/>.
        /// </summary>
        /// <param name="matrix">The Matrix on the left side of the equation.</param>
        /// <param name="scalar">The scalar value on the right side of the equation.</param>
        /// <returns>Result of the multiplication.</returns>
        public static Matrix4 operator *( Matrix4 matrix, float scalar )
        {
            Matrix4 result;

            result.M11 = matrix.M11 * scalar;
            result.M12 = matrix.M12 * scalar;
            result.M13 = matrix.M13 * scalar;
            result.M14 = matrix.M14 * scalar;
            result.M21 = matrix.M21 * scalar;
            result.M22 = matrix.M22 * scalar;
            result.M23 = matrix.M23 * scalar;
            result.M24 = matrix.M24 * scalar;
            result.M31 = matrix.M31 * scalar;
            result.M32 = matrix.M32 * scalar;
            result.M33 = matrix.M33 * scalar;
            result.M34 = matrix.M34 * scalar;
            result.M41 = matrix.M41 * scalar;
            result.M42 = matrix.M42 * scalar;
            result.M43 = matrix.M43 * scalar;
            result.M44 = matrix.M44 * scalar;

            return result;
        }        

        #endregion

        #region /

        /// <summary>
        /// Returns the result of dividing the <paramref name="left"/> Matrix
        /// through the <paramref name="right"/> Matrix - component wise.
        /// </summary>
        /// <param name="left">The matrix on the left side of the equation.</param>
        /// <param name="right">The matrix on the right side of the equation.</param>
        /// <returns>
        /// The result of the division.
        /// </returns>
        public static Matrix4 operator /( Matrix4 left, Matrix4 right )
        {
            Matrix4 result;

            result.M11 = left.M11 / right.M11;
            result.M12 = left.M12 / right.M12;
            result.M13 = left.M13 / right.M13;
            result.M14 = left.M14 / right.M14;
            result.M21 = left.M21 / right.M21;
            result.M22 = left.M22 / right.M22;
            result.M23 = left.M23 / right.M23;
            result.M24 = left.M24 / right.M24;
            result.M31 = left.M31 / right.M31;
            result.M32 = left.M32 / right.M32;
            result.M33 = left.M33 / right.M33;
            result.M34 = left.M34 / right.M34;
            result.M41 = left.M41 / right.M41;
            result.M42 = left.M42 / right.M42;
            result.M43 = left.M43 / right.M43;
            result.M44 = left.M44 / right.M44;

            return result;
        }

        /// <summary>
        /// Returns the result of dividing the specified <paramref name="matrix"/>
        /// through the specified <paramref name="divider"/> componentwise.
        /// </summary>
        /// <param name="matrix">The matrix on the left side of the equation.</param>
        /// <param name="divider">The scalar on the left side of the equation.</param>
        /// <returns>The result of the division.</returns>
        public static Matrix4 operator /( Matrix4 matrix, float divider )
        {
            float invDivider = 1f / divider;
            Matrix4 result;

            result.M11 = matrix.M11 * invDivider;
            result.M12 = matrix.M12 * invDivider;
            result.M13 = matrix.M13 * invDivider;
            result.M14 = matrix.M14 * invDivider;
            result.M21 = matrix.M21 * invDivider;
            result.M22 = matrix.M22 * invDivider;
            result.M23 = matrix.M23 * invDivider;
            result.M24 = matrix.M24 * invDivider;
            result.M31 = matrix.M31 * invDivider;
            result.M32 = matrix.M32 * invDivider;
            result.M33 = matrix.M33 * invDivider;
            result.M34 = matrix.M34 * invDivider;
            result.M41 = matrix.M41 * invDivider;
            result.M42 = matrix.M42 * invDivider;
            result.M43 = matrix.M43 * invDivider;
            result.M44 = matrix.M44 * invDivider;

            return result;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Returns whether the specified <see cref="Matrix4"/>s are equal.
        /// </summary>
        /// <param name="left">The result of the left side of the equation.</param>
        /// <param name="right">The result of the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the corresponding elements of the specified Matrices are approximately equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==( Matrix4 left, Matrix4 right )
        {
            return left.M11.IsApproximate( right.M11 ) && left.M12.IsApproximate( right.M12 ) && 
                   left.M13.IsApproximate( right.M13 ) && left.M14.IsApproximate( right.M14 ) &&

                   left.M21.IsApproximate( right.M21 ) && left.M22.IsApproximate( right.M22 ) && 
                   left.M23.IsApproximate( right.M23 ) && left.M24.IsApproximate( right.M24 ) &&

                   left.M31.IsApproximate( right.M31 ) && left.M32.IsApproximate( right.M32 ) && 
                   left.M33.IsApproximate( right.M33 ) && left.M34.IsApproximate( right.M34 ) &&

                   left.M11.IsApproximate( right.M41 ) && left.M11.IsApproximate( right.M42 ) && 
                   left.M13.IsApproximate( right.M43 ) && left.M13.IsApproximate( right.M44 );
        }

        /// <summary>
        /// Returns whether the specified <see cref="Matrix4"/>s are not equal.
        /// </summary>
        /// <param name="left">The result of the left side of the equation.</param>
        /// <param name="right">The result of the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if any of the corresponding elements of the specified Matrices are approximately not equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=( Matrix4 left, Matrix4 right )
        {
            return !left.M11.IsApproximate( right.M11 ) || !left.M12.IsApproximate( right.M12 ) || 
                   !left.M13.IsApproximate( right.M13 ) || !left.M14.IsApproximate( right.M14 ) ||

                   !left.M21.IsApproximate( right.M21 ) || !left.M22.IsApproximate( right.M22 ) || 
                   !left.M23.IsApproximate( right.M23 ) || !left.M24.IsApproximate( right.M24 ) ||

                   !left.M31.IsApproximate( right.M31 ) || !left.M32.IsApproximate( right.M32 ) || 
                   !left.M33.IsApproximate( right.M33 ) || !left.M34.IsApproximate( right.M34 ) ||

                   !left.M11.IsApproximate( right.M41 ) || !left.M11.IsApproximate( right.M42 ) || 
                   !left.M13.IsApproximate( right.M43 ) || !left.M13.IsApproximate( right.M44 );
        }

        #endregion

        #endregion
    } 
}