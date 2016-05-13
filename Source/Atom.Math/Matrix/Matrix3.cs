// <copyright file="Matrix3.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Matrix3 structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary> 
    /// Represents a 3x3 homogenous single-precission floating point Matrix. 
    /// </summary>
    [Serializable]
    public struct Matrix3 : ICultureSensitiveToStringProvider
    {
        #region [ Constants ]

        /// <summary> 
        /// Specifies the Identity Matrix,
        /// which is a matrix that represents no chance in rotation.
        /// This is a readonly field.
        /// </summary>
        public static readonly Matrix3 Identity = new Matrix3(
            1.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 1.0f
        );

        /// <summary> 
        /// A matrix that contains only zeros. ( 0, 0, 0, 0 ) This is a readonly field.
        /// </summary>
        public static readonly Matrix3 Zero = new Matrix3(
            0.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f
        );

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Element of the first row of the Matrix.
        /// </summary>
        public float M11, M12, M13;

        /// <summary>
        /// Element of the second row of the Matrix.
        /// </summary>
        public float M21, M22, M23;

        /// <summary>
        /// Element of the third row of the Matrix.
        /// </summary>
        public float M31, M32, M33;

        #endregion        

        #region [ Properties ]

        #region IsSingular

        /// <summary>
        /// Gets a value indicating whether this <see cref="Matrix3"/> is singular/degenerated.
        /// </summary>
        /// <remarks>
        /// A matrix has no inverse if its singular.
        /// </remarks>
        /// <value>
        /// A matrix is singular if its determinant is zero.
        /// Another way to test for singularity is to prove that
        /// there exists a vector x (x!= null-vector) where:
        /// Ax=0-vector.
        /// </value>
        public bool IsSingular
        {
            get
            {
                return this.Determinant == 0.0f;
            }
        }

        #endregion

        #region Determinant

        /// <summary>
        /// Gets the determinant of this <see cref="Matrix3"/>.
        /// </summary>
        /// <value> 
        /// A Matrix has no inverse if its Determinant is zero.
        /// </value>
        public float Determinant
        {
            get
            {
                return (M11 * M22 * M33) + (M12 * M23 * M31) + (M21 * M32 * M13) -
                       (M13 * M22 * M31) - (M12 * M21 * M33) - (M23 * M32 * M11);
            }
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary> 
        /// Initializes a new instance of the <see cref="Matrix3"/> struct. 
        /// </summary>
        /// <param name="m00">The X-coordiante of the x-axis. </param>
        /// <param name="m01">The Y-coordiante of the x-axis. </param>
        /// <param name="m02">The Z-coordiante of the x-axis. </param>
        /// <param name="m10">The X-coordiante of the y-axis. </param>
        /// <param name="m11">The Y-coordiante of the y-axis. </param>
        /// <param name="m12">The Z-coordiante of the y-axis. </param>     
        /// <param name="m20">The X-coordiante of the z-axis. </param>
        /// <param name="m21">The Y-coordiante of the z-axis. </param>
        /// <param name="m22">The Z-coordiante of the z-axis. </param>
        public Matrix3( 
            float m00, 
            float m01,
            float m02,
            float m10, 
            float m11,
            float m12,
            float m20, 
            float m21,
            float m22 )
        {
            this.M11 = m00;
            this.M12 = m01;
            this.M13 = m02;

            this.M21 = m10;
            this.M22 = m11;
            this.M23 = m12;

            this.M31 = m20;
            this.M32 = m21;
            this.M33 = m22;
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="Matrix3"/> struct. 
        /// </summary>
        /// <param name="axisX"> The-x axis of the new <see cref="Matrix3"/> (m00, m10, m20). </param>
        /// <param name="axisY"> The-y axis of the new <see cref="Matrix3"/> (m01, m11, n21). </param>
        /// <param name="axisZ"> The-z axis of the new <see cref="Matrix3"/> (m02, m12, n22). </param>
        public Matrix3( Vector3 axisX, Vector3 axisY, Vector3 axisZ )
        {
            this.M11 = axisX.X;
            this.M12 = axisY.X;
            this.M13 = axisZ.X;

            this.M21 = axisX.Y;
            this.M22 = axisY.Y;
            this.M23 = axisZ.Y;

            this.M31 = axisX.Z;
            this.M32 = axisY.Z;
            this.M33 = axisZ.Z;
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="Matrix3"/> struct;
        /// and copies the elements from the given <see cref="Matrix3"/> into the new one.
        /// </summary>
        /// <param name="matrix"> 
        /// The <see cref="Matrix3"/> to copy.
        /// </param>
        public Matrix3( Matrix3 matrix )
        {
            this.M11 = matrix.M11;
            this.M12 = matrix.M12;
            this.M13 = matrix.M13;

            this.M21 = matrix.M21;
            this.M22 = matrix.M22;
            this.M23 = matrix.M23;

            this.M31 = matrix.M31;
            this.M32 = matrix.M32;
            this.M33 = matrix.M33;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix3"/> struct.
        /// </summary>
        /// <param name="elements">
        /// The elements of the new Matrix.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="elements"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the length of the specified <paramref name="elements"/> array is less than 9.
        /// </exception>
        public Matrix3( float[] elements )
        {
            if( elements == null )
                throw new ArgumentNullException( "elements" );

            if( elements.Length < 9 )
                throw new ArgumentException( Atom.ErrorStrings.ArrayLengthOutOfValidRange, "elements" );

            this.M11 = elements[0];
            this.M12 = elements[1];
            this.M13 = elements[2];

            this.M21 = elements[3];
            this.M22 = elements[4];
            this.M23 = elements[5];

            this.M31 = elements[6];
            this.M32 = elements[7];
            this.M33 = elements[8];
        }

        #endregion

        #region [ Methods ]

        #region > Creation Helpers <

        /// <summary>
        /// Creates a rotation Matrix given a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="quaternion">
        /// The quaterion to convert.
        /// </param>
        /// <returns>
        /// The converted <see cref="Matrix3"/>.
        /// </returns>
        public static Matrix3 FromQuaternion( Quaternion quaternion )
        {
            float squaredX = quaternion.X * quaternion.X;
            float squaredY = quaternion.Y * quaternion.Y;
            float squaredZ = quaternion.Z * quaternion.Z;

            float xy = quaternion.X * quaternion.Y;
            float xw = quaternion.X * quaternion.W;
            float yw = quaternion.Y * quaternion.W;
            float yz = quaternion.Y * quaternion.Z;
            float zw = quaternion.Z * quaternion.W;
            float zx = quaternion.Z * quaternion.X;

            Matrix3 matrix;

            matrix.M11 = 1f - (2f * (squaredY + squaredZ));
            matrix.M12 = 2f * (xy + zw);
            matrix.M13 = 2f * (zx - yw);
            matrix.M21 = 2f * (xy - zw);
            matrix.M22 = 1f - (2f * (squaredZ + squaredX));
            matrix.M23 = 2f * (yz + xw);
            matrix.M31 = 2f * (zx + yw);
            matrix.M32 = 2f * (yz - xw);
            matrix.M33 = 1f - (2f * (squaredY + squaredX));

            return matrix;
        }

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Returns whether the specified <see cref="Object"/> 
        /// is equal to this <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test against.
        /// </param>
        /// <returns>
        /// Returns <see lang="true"/> if they are equal;
        /// otherwise <see lang="false"/>.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj == null )
                return false;

            if( obj is Matrix3 )
                return this.Equals( (Matrix3)obj );

            return false;
        }

        /// <summary>
        /// Returns whether the specified Matrix3 instance 
        /// is approximately equal to this <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Matrix3"/> instance to test against.
        /// </param>
        /// <returns>
        /// Returns <see lang="true"/> if the elements of the matrices are (approximately) equal;
        /// otherwise <see lang="false"/>.
        /// </returns>
        public bool Equals( Matrix3 other )
        {
            return this.M11.IsApproximate( other.M11 ) && this.M12.IsApproximate( other.M12 ) && 
                   this.M13.IsApproximate( other.M13 ) &&

                   this.M11.IsApproximate( other.M21 ) && this.M12.IsApproximate( other.M22 ) && 
                   this.M13.IsApproximate( other.M23 ) &&

                   this.M11.IsApproximate( other.M31 ) && this.M12.IsApproximate( other.M32 ) && 
                   this.M13.IsApproximate( other.M33 );
        }

        #endregion

        #region ToString

        /// <summary> 
        /// Returns a human-readable representation of this <see cref="Matrix3"/>.
        /// </summary>
        /// <returns> 
        /// A string representation of this <see cref="Matrix3"/>.
        /// </returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary> 
        /// Returns a human-readable representation of this <see cref="Matrix3"/>.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns> 
        /// A string representation of this <see cref="Matrix3"/>.
        /// </returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendFormat( formatProvider, "|{0} {1} {2}|\n", M11.ToString( formatProvider ), M12.ToString( formatProvider ), M13.ToString( formatProvider ) );
            sb.AppendFormat( formatProvider, "|{0} {1} {2}|\n", M21.ToString( formatProvider ), M22.ToString( formatProvider ), M23.ToString( formatProvider ) );
            sb.AppendFormat( formatProvider, "|{0} {1} {2}|\n", M31.ToString( formatProvider ), M32.ToString( formatProvider ), M33.ToString( formatProvider ) );

            return sb.ToString();
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets the hash code of this <see cref="Matrix3"/> instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.M11 );
            hashBuilder.AppendStruct( this.M12 );
            hashBuilder.AppendStruct( this.M13 );
            
            hashBuilder.AppendStruct( this.M21 );
            hashBuilder.AppendStruct( this.M22 );
            hashBuilder.AppendStruct( this.M23 );

            hashBuilder.AppendStruct( this.M31 );
            hashBuilder.AppendStruct( this.M32 );
            hashBuilder.AppendStruct( this.M33 );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        #region Logic

        /// <summary>
        /// Returns whether the specified <see cref="Matrix3"/>s are equal.
        /// </summary>
        /// <param name="left">The Matrix of the left side of the equation.</param>
        /// <param name="right">The Matrix of the right side of the equation.</param>
        /// <returns>
        /// Returns <see lang="true"/> if the corresponding elements of the specified Matrices are approximately equal;
        /// otherwise <see lang="false"/>.
        /// </returns>
        public static bool operator ==( Matrix3 left, Matrix3 right )
        {
            return left.M11.IsApproximate( right.M11 ) && left.M12.IsApproximate( right.M12 ) && left.M13.IsApproximate( right.M13 ) &&
                   left.M21.IsApproximate( right.M21 ) && left.M22.IsApproximate( right.M22 ) && left.M23.IsApproximate( right.M23 ) &&
                   left.M31.IsApproximate( right.M31 ) && left.M32.IsApproximate( right.M32 ) && left.M33.IsApproximate( right.M33 );
        }

        /// <summary>
        /// Returns whether the specified <see cref="Matrix3"/>s are not equal.
        /// </summary>
        /// <param name="left">The Quaternion of the left side of the equation.</param>
        /// <param name="right">The Quaternion of the right side of the equation.</param>
        /// <returns>
        /// Returns <see lang="true"/> if any of the corresponding elements of the specified Quaternions are approximately not equal;
        /// otherwise <see lang="false"/>.
        /// </returns>
        public static bool operator !=( Matrix3 left, Matrix3 right )
        {
            return !left.M11.IsApproximate( right.M11 ) || !left.M12.IsApproximate( right.M12 ) || !left.M13.IsApproximate( right.M13 ) ||
                   !left.M21.IsApproximate( right.M21 ) || !left.M22.IsApproximate( right.M22 ) || !left.M23.IsApproximate( right.M23 ) ||
                   !left.M31.IsApproximate( right.M31 ) || !left.M32.IsApproximate( right.M32 ) || !left.M33.IsApproximate( right.M33 );
        }

        #endregion

        #endregion
    }
}
