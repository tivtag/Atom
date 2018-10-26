// <copyright file="Complex.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Complex structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Globalization;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Defines a complex number (x,yi). C = x + yi, where i^2=(-1). 
    /// </summary>
    /// <remarks>
    /// Complexs numbers are an extension of the real numbers to allow 
    /// the representation of square roots of negative numbers.
    /// Any real number is a complex number of type (x,0).
    /// Any complex number of type (0,-y) is a square root of a negative real number.
    /// </remarks>
    [System.Serializable]
    [System.ComponentModel.TypeConverter( typeof( Atom.Math.Design.ComplexConverter ) )]
    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct Complex : ICloneable, IEquatable<Complex>, System.Runtime.Serialization.ISerializable,
                            System.Collections.Generic.IEnumerable<float>, ICultureSensitiveToStringProvider
    {
        #region [ Fields ]

        /// <summary>
        /// The real part of this <see cref="Complex"/> number.
        /// </summary>
        public float Real;

        /// <summary>
        /// The imaginary part of this <see cref="Complex"/> number.
        /// </summary>
        public float Imag;

        #endregion

        #region [ Constants ]

        /// <summary>
        /// Gets the <see cref="Complex"/> number with both of its elements set to zero.
        /// </summary>
        /// <value>The complex number (0 + 0i).</value>
        public static Complex Zero
        {
            get { return new Complex( 0.0f, 0.0f ); }
        }

        /// <summary>
        /// Gets the <see cref="Complex"/> number which represents the real value 'one'.
        /// </summary>
        /// <value>The complex number (1 + 0i).</value>
        public static Complex One
        {
            get { return new Complex( 1.0f, 0.0f ); }
        }

        /// <summary>
        /// Gets the <see cref="Complex"/> number which represents the imaginary value 'one'.
        /// </summary>
        /// <value>The complex number (0 + 1i).</value>
        public static Complex I
        {
            get { return new Complex( 0.0f, 1.0f ); }
        }

        /// <summary>
        /// Gets the <see cref="Complex"/> value that is not a number.</summary>
        /// <value>The complex number (NaN + NaNi).</value>
        public static Complex NaN
        {
            get { return new Complex( float.NaN, float.NaN ); }
        }

        /// <summary>
        /// Gets the <see cref="Complex"/> number which represents infinity.
        /// </summary>
        /// <value>The complex number (PositiveInfinity + PositiveInfinityi).</value>
        public static Complex Infinity
        {
            get { return new Complex( float.PositiveInfinity, float.PositiveInfinity ); }
        }

        #endregion

        #region [ Properties ]

        #region Length

        /// <summary>
        /// Gets or sets the length (also called modulus) of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The length (also called modulus) of this <see cref="Complex"/> number.</value>
        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt( (Real * Real) + (Imag * Imag) );
            }

            set
            {
                if( value < 0.0f )
                    throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "value" );

                if( float.IsInfinity( value ) )
                {
                    Real = value;
                    Imag = value;
                }
                else
                {
                    if( this.IsZero )
                    {
                        Real = value;
                        Imag = 0.0f;
                    }
                    else
                    {
                        float ratio = value / this.Length;

                        Real *= ratio;
                        Imag *= ratio;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the squared length (also called modulus) of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The squared length (also called modulus) of this <see cref="Complex"/> number.</value>
        public float SquaredLength
        {
            get
            {
                return (Real * Real) + (Imag * Imag);
            }

            set
            {
                if( value < 0.0f )
                    throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "value" );

                if( float.IsInfinity( value ) )
                {
                    Real = value;
                    Imag = value;
                }
                else
                {
                    if( this.IsZero )
                    {
                        Real = (float)System.Math.Sqrt( value );
                        Imag = 0.0f;
                    }
                    else
                    {
                        float ratio = value / this.SquaredLength;

                        Real *= ratio;
                        Imag *= ratio;
                    }
                }
            }
        }

        #endregion

        #region Angle

        /// <summary>
        /// Gets or sets the angle (also called argument) of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The angle (also called argument) of this Complex number (in radians).</value>
        public float Angle
        {
            get
            {
                if( Imag.IsApproximate( 0.0f ) )
                {
                    if( Real >= 0.0f )
                        return 0.0f;
                    else // Real < 0.0f                        
                        return Constants.Pi;
                }
                else
                    return (float)System.Math.Atan2( Imag, Real );
            }

            set
            {
                float modulus = this.Length;

                Real = (float)System.Math.Cos( value ) * modulus;
                Imag = (float)System.Math.Sin( value ) * modulus;
            }
        }

        #endregion

        #region Conjugate

        /// <summary> 
        /// Gets or sets the conjugate of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The conjugate of this Complex number.</value>
        public Complex Conjugate
        {
            get { return new Complex( Real, -Imag ); }
            set { this = value.Conjugate; }
        }

        #endregion

        #region > Trigometry <

        #region - Sin -

        /// <summary>
        /// Gets the Sine (sin) of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The sine of this Complex number.</value>
        public Complex Sin
        {
            get
            {
                if( IsReal )
                    return new Complex( System.Math.Sin( Real ), 0.0f );

                Complex c = new Complex( 1.0f, 0.0f ) /
                           (new Complex( 2.0f, 0.0f ) * Complex.I);

                return c * ((Complex.I * this).Exp -
                           (new Complex( -1.0f, 0.0f ) * Complex.I * this).Exp);
            }
        }

        /// <summary>
        /// Gets the Arcus Sine of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The arcus sine of this Complex number.</value>
        public Complex Asin
        {
            get
            {
                return (-I * (1.0f - this.Square).SquareRoot) + (I * this.Log);
            }
        }

        /// <summary>
        /// Gets the Sine Hyperbolicus of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The sine hyperbolicus of this Complex number.</value>
        public Complex Sinh
        {
            get
            {
                if( IsReal )
                    return new Complex( (float)System.Math.Sinh( Real ), 0.0f );

                return new Complex( 
                    System.Math.Sinh( Real ) * System.Math.Cos( Imag ),
                    System.Math.Cosh( Real ) * System.Math.Sin( Imag ) 
                );
            }
        }

        /// <summary>
        /// Gets the Arcus(Area) Sine Hyperbolicus of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The arcus sine hyperbolicus of this Complex number.</value>
        public Complex Asinh
        {
            get
            {
                return (this + (this.Square + 1.0f).SquareRoot).Log;
            }
        }

        #endregion

        #region - Cos -

        /// <summary>
        /// Gets the Cosine (cos) of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The cosine of this Complex number.</value>
        public Complex Cos
        {
            get
            {
                if( IsReal )
                    return new Complex( System.Math.Cos( Real ), 0.0f );

                return new Complex( 
                    System.Math.Cos( Real ) * System.Math.Cosh( Imag ),
                   -System.Math.Sin( Real ) * System.Math.Sinh( Imag )
                );
            }
        }

        /// <summary>
        /// Gets the Cosine Hyperbolicus of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The cosine hyperbolicus of this Complex number.</value>
        public Complex Cosh
        {
            get
            {
                if( IsReal )
                    return new Complex( (float)System.Math.Cosh( Real ), 0.0f );

                return new Complex( 
                    System.Math.Cosh( Real ) * System.Math.Cos( Imag ),
                    System.Math.Sinh( Real ) * System.Math.Sin( Imag ) 
                );
            }
        }

        /// <summary>
        /// Gets the Arcus(Area) Cosine of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The acrus cosine of this Complex number.</value>
        public Complex Acos
        {
            get
            {
                return (-I * this) + (I * (1.0f - this.Square).SquareRoot.Log);
            }
        }

        /// <summary>
        /// Gets the Arcus(Area) Cosine Hyperbolicus of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The arcus cosine hyperbolicus of this Complex number.</value>
        public Complex Acosh
        {
            get
            {
                return (this + ((this - 1.0f).SquareRoot * (this + 1.0f).SquareRoot)).Log;
            }
        }

        #endregion

        #region - Tan -

        /// <summary>
        /// Gets the Tangent (tan) of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The tangent of this Complex number.</value>
        public Complex Tan
        {
            get
            {
                if( IsReal )
                    return new Complex( (float)System.Math.Tan( Real ), 0.0f );

                double cos   = System.Math.Cos( Real );
                double sinh  = System.Math.Sinh( Imag );
                double denom = (cos * cos) + (sinh * sinh);

                return new Complex(
                    System.Math.Sin( Real ) * cos / denom,
                    sinh * System.Math.Cosh( Imag ) / denom
                );
            }
        }

        /// <summary>
        /// Gets the Arcus Tangent (Atan) of this <see cref="Complex"/> number.
        /// </summary>
        /// <remarks>
        /// The Arcus Tangent is the inverse function of the Tangent.
        /// </remarks>
        /// <value>The arcus tangent of this Complex number.</value>
        public Complex Atan
        {
            get
            {
                Complex iz = new Complex( -Imag, Real ); // I*this
                return new Complex( 0.0f, 0.5f ) * ((1.0f - iz).Log - (1.0f + iz).Log);
            }
        }

        /// <summary>
        /// Gets the Hyperbolic Tangent (Tanh) of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The tangent hyperbolicus of this Complex number.</value>
        public Complex Tanh
        {
            get
            {
                if( IsReal )
                    return new Complex( (float)System.Math.Tanh( Real ), 0.0f );

                double cosI = System.Math.Cos( Imag );
                double sinhR = System.Math.Sinh( Real );
                double denom = (cosI * cosI) + (sinhR * sinhR);

                return new Complex( 
                    System.Math.Cosh( Real ) * sinhR / denom,
                    cosI * System.Math.Sin( Imag ) / denom 
                );
            }
        }

        /// <summary>
        /// Gets the Arcus(Area) Tangent Hyperbolicus of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The arcus tangent hyperbolicus of this Complex number.</value>
        public Complex Atanh
        {
            get
            {
                return 0.5f * ((1.0f + this).Log - (1.0f - this).Log);
            }
        }

        /// <summary>
        /// Gets the Cotangent of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The cotangent of this Complex number.</value>
        public Complex Cotan
        {
            get
            {
                if( IsReal )
                    return new Complex(  1.0f / (float)System.Math.Tan( Imag ), 0.0f );
                
                double sin   = System.Math.Sin( Real );
                double sinh  = System.Math.Sinh( Imag );
                double denom = (sin * sin) + (sinh * sinh);

                return new Complex(
                      sin * System.Math.Cos( Real ) / denom,
                    -sinh * System.Math.Cosh( Imag ) / denom
                );
            }
        }

        /// <summary>
        /// Gets the Arcus Cotangens of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The arcus cotangent of this Complex number.</value>
        public Complex Acotan
        {
            get
            {
                Complex iz = new Complex( -Imag, Real ); // I*this
                return (new Complex( 0.0f, 0.5f ) * ((1.0f + iz).Log - (1.0f - iz).Log)) + Constants.PiOver2;
            }
        }

        /// <summary>
        /// Gets the Hyperbolic Cotangent of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The hyperbolic contangent of this Complex number.</value>
        public Complex Cotanh
        {
            get
            {
                if( IsReal )
                    return new Complex( 1.0f / (float)System.Math.Tanh( Real ), 0.0f );

                double sinI  = System.Math.Sin( Imag );
                double sinhR = System.Math.Sinh( Real );
                double denom = (sinI * sinI) + (sinhR * sinhR);

                return new Complex(
                    sinhR * System.Math.Cosh( Real ) / denom,
                    sinI * System.Math.Cos( Imag ) / denom );
            }
        }

        /// <summary>
        /// Gets the Arcus(Area) Hyperbolic Cotangent of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The arcus hyperbolic contangent of this Complex number.</value>
        public Complex Acotanh
        {
            get
            {
                return 0.5f * ((this + 1.0f).Log - (this - 1.0f).Log);
            }
        }

        #endregion

        #region - Sec -

        /// <summary>
        /// Gets the Secant of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The secant of this Complex number.</value>
        public Complex Sec
        {
            get
            {
                if( IsReal )
                {
                    return new Complex( 1.0f / System.Math.Cos( Real ), 0.0f );
                }

                double cos   = System.Math.Cos( Real );
                double sinh  = System.Math.Sinh( Imag );
                double denom = (cos * cos) + (sinh * sinh);

                return new Complex(
                       cos * System.Math.Cosh( Imag ) / denom,
                       System.Math.Sin( Real ) * sinh / denom );
            }
        }

        /// <summary>
        /// Gets the Cosecant (csc, Cosekans) of this <c>Complex</c>.
        /// </summary>
        /// <value>The cosecant of this Complex number.</value>
        public Complex Cosec
        {
            get
            {
                if( IsReal )
                    return new Complex( System.Math.Cos( Real ), 0d );

                double sinr  = System.Math.Sin( Real );
                double sinhi = System.Math.Sinh( Imag );
                double denom = (sinr * sinr) + (sinhi * sinhi);

                return new Complex( 
                    sinr * System.Math.Cosh( Imag ) / denom,
                   -System.Math.Cos( Real ) * sinhi / denom 
                );
            }
        }

        /// <summary>
        /// Gets the Secant Hyperbolicus (Sech) of this <c>Complex</c>.
        /// </summary>
        /// <value>The secant hyperbolicus of this Complex number.</value>
        public Complex Sech
        {
            get
            {
                if( IsReal )
                    return new Complex( 1.0f / (float)System.Math.Cosh( Real ), 0.0f );

                Complex exp = this.Exp;
                return 2.0f * exp / (exp.Square + 1.0f);
            }
        }

        /// <summary>
        /// Gets the Cosecant Hyperbolicus (Csech) of this <c>Complex</c>.
        /// </summary>
        /// <value>The cosecant hyperbolicus of this Complex number.</value>
        public Complex Cosech
        {
            get
            {
                if( IsReal )
                    return new Complex( 1.0f / (float)System.Math.Sinh( Real ), 0.0f );

                Complex exp = this.Exp;
                return 2.0f * exp / (exp.Square - 1.0f);
            }
        }

        /// <summary>
        /// Gets the Arcus Secant of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The arcus secant of this Complex number.</value>
        public Complex Asec
        {
            get
            {
                Complex inv = 1.0f / this;
                return -I * (inv + (I * (1.0f - inv.Square).SquareRoot)).Log;
            }
        }

        /// <summary>
        /// Gets the Arcus Secant Hyperbolicus of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The arcus secant hyperbolicus of this Complex number.</value>
        public Complex Asech
        {
            get
            {
                Complex inv = 1.0f / this;
                return (inv + ((inv - 1.0f).SquareRoot * (inv + 1.0f).SquareRoot)).Log;
            }
        }

        /// <summary>
        /// Gets the Arcus Cosecant of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The arcus cosecant of this Complex number.</value>
        public Complex Acosec
        {
            get
            {
                Complex inv = 1.0f / this;
                return (-I) * ((I * inv) + (1.0f - inv.Square).SquareRoot).Log;
            }
        }

        /// <summary>
        /// Gets the Arcus(Area) Cosecant Hyperbolicus of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The arcus cosecant hyperbolicus of this Complex number.</value>
        public Complex Acosech
        {
            get
            {
                Complex inv = 1.0f / this;
                return (inv + (inv.Square + 1.0f).SquareRoot).Log;
            }
        }

        #endregion

        #endregion

        #region > Arithmetic <

        /// <summary>
        /// Gets the exponential of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The exponential of this Complex number.</value>
        public Complex Exp
        {
            get
            {
                double exp = System.Math.Exp( Real );
                if( IsReal )
                    return new Complex( (float)exp, 0.0f );

                return new Complex( 
                    exp * System.Math.Cos( Imag ),
                    exp * System.Math.Sin( Imag )
                );
            }
        }

        /// <summary>
        /// Gets the natural logarithm of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The natural logarithm of this Complex number.</value>
        public Complex Log
        {
            get
            {
                if( IsReal )
                    return new Complex( (float)System.Math.Log( Real ), 0.0f );

                if( Real > 0.0f && Imag.IsApproximate( 0.0f ) )
                {
                    return new Complex( (float)System.Math.Log( Real ), 0.0f );
                }
                else if( Real == 0.0f )
                {
                    if( Imag > 0.0f )
                    {
                        return new Complex( 
                            System.Math.Log( Imag ),
                            Constants.PiOver2
                        );
                    }
                    else
                    {
                        return new Complex( 
                            System.Math.Log( -Imag ),
                            -Constants.PiOver2
                        );
                    }
                }
                else
                {
                    return new Complex( 
                        System.Math.Log( this.Length ),
                        System.Math.Atan2( Imag, Real )
                    );
                }
            }
        }

        /// <summary>
        /// Gets this <see cref="Complex"/> number squared.
        /// </summary>
        /// <value>The square of this Complex number.</value>
        public Complex Square
        {
            get
            {
                return new Complex(
                    (Real * Real) - (Imag * Imag),
                    2.0f * Real * Imag 
                );
            }
        }

        /// <summary>
        /// Gets the Square Root of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The square root of this Complex number.</value>
        public Complex SquareRoot
        {
            get
            {
                if( this.IsRealNonNegative )
                    return new Complex( (float)System.Math.Sqrt( Real ), 0.0f );

                float norm        = this.Length;
                float halfOfRoot2 = 1.0f / (float)System.Math.Sqrt( 2.0f );

                Complex result;
                result.Real = halfOfRoot2 * (float)System.Math.Sqrt( norm + Real );

                if( this.Imag < 0.0f )
                    result.Imag = -halfOfRoot2 * (float)System.Math.Sqrt( norm - Real );
                else
                    result.Imag = halfOfRoot2 * (float)System.Math.Sqrt( norm - Real );

                return result;
            }
        }

        /// <summary>
        /// Gets the absolute value of this <see cref="Complex"/> number.
        /// </summary>
        /// <value>The absolute of this Complex number.</value>
        public Complex Abs
        {
            get
            {
                return new Complex( 
                    System.Math.Abs( Real ), 
                    System.Math.Abs( Imag ) 
                );
            }
        }

        #endregion

        #region > State <

        /// <summary>
        /// Gets a value indicating whether this <see cref="Complex"/> number is zero.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if both elements of this Complex number are approximately zero;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsZero
        {
            get
            {
                return Real.IsApproximate( 0.0f ) && Imag.IsApproximate( 0.0f );
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Complex"/> number is a real number.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if the imaginary part of this Complex number is approximately zero;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsReal
        {
            get
            {
                return Imag.IsApproximate( 0.0f );
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Complex"/> number is a real non-negative number.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if the real part of this Complex number is positive and the imaginary part approximately zero;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsRealNonNegative
        {
            get
            {
                return Real >= 0 && Imag.IsApproximate( 0.0f );
            }
        }

        /// <summary> 
        /// Gets a value indicating whether this <see cref="Complex"/> number is NaN (Not a number).
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if any of the elements of this Complex number is NaN;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsNaN
        {
            get { return float.IsNaN( Real ) || float.IsNaN( Imag ); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Complex"/> number represents infinity.
        /// </summary>
        /// <remarks>
        /// True if it either the real or imaginary part represents positive or negative infinity.
        /// </remarks>
        /// <value>
        /// Returns <see langword="true"/> if any of the elements of this Complex number is infinite;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsInfinity
        {
            get { return float.IsInfinity( Real ) || float.IsInfinity( Imag ); }
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Complex"/> structure.
        /// </summary>
        /// <param name="real"> The real part of the <see cref="Complex"/> number. </param>
        /// <param name="imag"> The imaginary part of the <see cref="Complex"/> number. </param>
        public Complex( float real, float imag )
        {
            this.Real = real;
            this.Imag = imag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Complex"/> structure.
        /// </summary>
        /// <remarks> The double precision is lost when cast to single precision. </remarks>
        /// <param name="real"> The real part of the <see cref="Complex"/> number. </param>
        /// <param name="imag"> The imaginary part of the <see cref="Complex"/> number. </param>
        public Complex( double real, double imag )
        {
            this.Real = (float)real;
            this.Imag = (float)imag;
        }

        /// <summary>
        /// Constructs a <c>Complex</c> number from its modulus and argument.</summary>
        /// <param name="length"> The length (or modulus) of the <see cref="Complex"/> number. Must be positive. </param>
        /// <param name="angle"> The angle (or argument) of the <see cref="Complex"/> number. </param>
        /// <returns>The constructed Complex number.</returns>
        public static Complex FromLengthAngle( float length, float angle )
        {
            if( length < 0.0f )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsNegative, "length" );

            return new Complex( 
                length * (float)System.Math.Cos( angle ),
                length * (float)System.Math.Sin( angle )
            );
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="Complex"/> structure and
        /// sets the real and imaginary values of the new <see cref="Complex"/> to the "Real" and "Imag"
        /// values inside the specified <see cref="System.Runtime.Serialization.SerializationInfo"/>.
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
        private Complex( 
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        {
            this.Real = info.GetSingle( "Real" );
            this.Imag = info.GetSingle( "Imag" );
        }

        #endregion

        #region [ Methods ]

        #region Pow

        /// <summary>
        /// Returns the result of raising this <see cref="Complex"/> number
        /// by the given <paramref name="exponent"/>.
        /// </summary>
        /// <param name="exponent">
        /// The input exponent.
        /// </param>
        /// <returns>The calculated complex value.</returns>
        [Pure]
        public Complex Pow( Complex exponent )
        {
            return (exponent * this.Log).Exp;
        }

        #endregion

        #region Root

        /// <summary>
        /// Returns the result of raising this <see cref="Complex"/> number
        /// by the inverse of the given <paramref name="rootExponent"/>.
        /// </summary>
        /// <param name="rootExponent">
        /// The input root exponent.
        /// </param>
        /// <returns>The calculated complex value.</returns>
        [Pure]
        public Complex Root( Complex rootExponent )
        {
            return ((1.0f / rootExponent) * this.Log).Exp;
        }

        #endregion

        #region Normalize

        /// <summary>
        /// Normalizes the <see cref="Complex"/> number 
        /// to have a length of one.
        /// </summary>
        public void Normalize()
        {
            float lengthSquared = (Real * Real) + (Imag * Imag);

            if( lengthSquared != 0.0f )
            {
                float length = (float)System.Math.Sqrt( lengthSquared );

                Real /= length;
                Imag /= length;
            }
        }

        #endregion

        #region > Operators <

        #region Add

        /// <summary>
        /// Returns the result of adding 
        /// the <paramref name="right"/> <see cref="Complex"/> number to the <paramref name="left"/>
        /// <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex Add( Complex left, Complex right )
        {
            Complex result;

            result.Real = left.Real + right.Real;
            result.Imag = left.Imag + right.Imag;

            return result;
        }

        /// <summary>
        /// Returns the result of adding 
        /// the <paramref name="right"/> real number to the <paramref name="left"/>
        /// <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex Add( Complex left, float right )
        {
            Complex result;

            result.Real = left.Real + right;
            result.Imag = left.Imag;

            return result;
        }

        /// <summary>
        /// Returns the result of adding 
        /// the <paramref name="left"/> real number to the <paramref name="right"/>
        /// <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex Add( float left, Complex right )
        {
            Complex result;

            result.Real = left + right.Real;
            result.Imag = right.Imag;

            return result;
        }

        #endregion

        #region Subtract

        /// <summary>
        /// Negates the specified <see cref="Complex"/> number.
        /// </summary>
        /// <param name="number"> The number to negate. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex Negate( Complex number )
        {
            Complex result;

            result.Real = -number.Real;
            result.Imag = -number.Imag;

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> <see cref="Complex"/> number
        /// from the left <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex Subtract( Complex left, Complex right )
        {
            Complex result;

            result.Real = left.Real - right.Real;
            result.Imag = left.Imag - right.Imag;

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> <see cref="Complex"/> 
        /// number from the <paramref name="left"/> real number.
        /// </summary>
        /// <param name="left"> The real number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>/param>
        /// <returns></returns>
        public static Complex Subtract( float left, Complex right )
        {
            Complex result;

            result.Real = left - right.Real;
            result.Imag = -right.Imag;

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> real number 
        /// number from the <paramref name="left"/> <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The real number on the right side. </param>
        /// <returns> The result of the operation. </returns>/param>
        /// <returns></returns>
        public static Complex Subtract( Complex left, float right )
        {
            Complex result;

            result.Real = left.Real - right;
            result.Imag = left.Imag;

            return result;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Multiplies the elements of the <see cref="Complex"/> number with the real number.
        /// </summary>
        /// <param name="left"> The real number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex Multiply( float left, Complex right )
        {
            Complex result;

            result.Real = left * right.Real;
            result.Imag = left * right.Imag;

            return result;
        }

        /// <summary>
        /// Multiplies the elements of the <see cref="Complex"/> number with the real number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The real number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex Multiply( Complex left, float right )
        {
            Complex result;

            result.Real = left.Real * right;
            result.Imag = left.Imag * right;

            return result;
        }

        /// <summary>
        /// Multiplies the left <see cref="Complex"/> number with the right <see cref="Complex"/> number.
        /// </summary>
        /// <remarks>
        /// Remember that left*right may not be equal to right*left.
        /// </remarks>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex Multiply( Complex left, Complex right )
        {
            Complex result;

            result.Real = (left.Real * right.Real) - (left.Imag * right.Imag);
            result.Imag = (left.Real * right.Imag) + (left.Imag * right.Real);

            return result;
        }

        #endregion

        #region Divide

        /// <summary>
        /// Divides the left <see cref="Complex"/> number through the right.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex Divide( Complex left, Complex right )
        {
            if( right.IsZero )
                return Complex.Infinity;

            float rightModulus = right.SquaredLength;            
            Complex result;

            result.Real = ((left.Real * right.Real) + (left.Imag * right.Imag)) / rightModulus;
            result.Imag = ((left.Imag * right.Real) - (left.Real * right.Imag)) / rightModulus;

            return result;
        }

        /// <summary>
        /// Divides the left real number through the right <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The real number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex Divide( float left, Complex right )
        {
            if( right.IsZero )
                return Complex.Infinity;

            float mod = right.SquaredLength;
            Complex result;

            result.Real = left * right.Real / mod;
            result.Imag = -left * right.Imag / mod;

            return result;
        }

        /// <summary>
        /// Divides the left <see cref="Complex"/> number through the right real number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The real number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex Divide( Complex left, float right )
        {
            Complex result;

            result.Real = left.Real / right;
            result.Imag = left.Imag / right;

            return result;
        }

        #endregion

        #endregion

        #region > Overrides/Impls <

        #region Equals

        /// <summary> Gets whether the specified <see cref="Object"/> is equal to this <see cref="Complex"/>. </summary>
        /// <param name="obj"> The object to test against. </param>
        /// <returns>true if they are equal, otherwise false. </returns>
        public override bool Equals( object obj )
        {
            return (obj is Complex) && this.Equals( (Complex)obj );
        }

        /// <summary> 
        /// Gets whether the specified <see cref="Complex"/> number is equal 
        /// to this <see cref="Complex"/> number. 
        /// </summary>
        /// <param name="other"> The number to test against. </param>
        /// <returns>true if they are equal, otherwise false. </returns>
        public bool Equals( Complex other )
        {
            if( IsNaN || other.IsNaN )
                return false;
            else
                return Real.IsApproximate( other.Real ) &&
                       Imag.IsApproximate( other.Imag );
        }

        #endregion

        #region GetHashCode

        /// <summary> 
        /// Returns the hash code of this <see cref="Complex"/> number.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hashBuilder = new HashCodeBuilder();

            hashBuilder.AppendStruct( this.Real );
            hashBuilder.AppendStruct( this.Imag );

            return hashBuilder.GetHashCode();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overriden to return a human-readable string that descripes the Complex number.
        /// </summary>
        /// <returns>A string that descripes the Complex number.</returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable text representation of the Complex number.
        /// </summary>
        /// <param name="formatProvider">
        /// The <see cref="System.IFormatProvider"/> that supplies culture specific formatting information.
        /// </param>
        /// <returns>A human-readable text representation of the Complex number.</returns>
        public string ToString( System.IFormatProvider formatProvider )
        {
            return string.Format( 
                formatProvider,
                "({0} + {1}i)",
                this.Real.ToString( formatProvider ),
                this.Imag.ToString( formatProvider )
            );
        }

        #endregion

        #region Clone

        /// <summary> This is a private implementation of an Interface. </summary>
        /// <returns> Cloned <see cref="Complex"/> as an object.</returns>
        object ICloneable.Clone()
        {
            return new Complex( this.Real, this.Imag );
        }

        /// <summary> Creates a copy of this <see cref="Complex"/> number. </summary>
        /// <returns> Cloned <see cref="Complex"/>. </returns>
        public Complex Clone()
        {
            return new Complex( this.Real, this.Imag );
        }

        #endregion

        #region GetObjectData

        /// <summary>
        /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/>
        /// with the data needed to serialize the <see cref="Complex"/>.
        /// </summary>
        /// <remarks>
        /// real component: "Real"
        /// imaginary component: "Imag"
        /// </remarks>
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
        public void GetObjectData( 
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        {
            if( info == null )
                throw new System.ArgumentNullException( "info" );

            info.SetType( this.GetType() );

            info.AddValue( "Real", Real );
            info.AddValue( "Imag", Imag );
        }

        #endregion

        #region Parse

        /// <summary>
        /// Tries to parse the specified <paramref name="text"/> into this <see cref="Complex"/> object
        /// using the <see cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"> If <paramref name="text"/> is null. </exception>
        /// <exception cref="System.FormatException"> If the specified <paramref name="text"/> is in an invalid format. </exception>
        /// <param name="text"> The text to parse. Must be in format "real imag". </param>
        public void Parse( string text )
        {
            Parse( text, CultureInfo.InvariantCulture );
        }

        /// <summary>
        /// Tries to parse the specified <paramref name="text"/> into this <see cref="Complex"/> object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"> If <paramref name="text"/> is null. </exception>
        /// <exception cref="System.FormatException"> If the specified <paramref name="text"/> is in an invalid format. </exception>
        /// <param name="text"> The text to parse. Must be in format "real imag". </param>
        /// <param name="formatProvider"> The format settings to use. </param>
        public void Parse( string text, System.IFormatProvider formatProvider )
        {
            if( text == null )
                throw new System.ArgumentNullException( "text" );

            string[] values = text.Split( ' ' );

            if( values.Length < 2 )
            {
                throw new System.FormatException(
                    string.Format( 
                        System.Globalization.CultureInfo.CurrentCulture,
                        Atom.ErrorStrings.ParsingGivenStringXNotEnoughEntriesExpectedYHasZ,
                        text,
                        "2",
                        values.Length.ToString( System.Globalization.CultureInfo.CurrentCulture ) 
                    ) 
                );
            }

            Real = float.Parse( values[0], formatProvider );
            Imag = float.Parse( values[1], formatProvider );
        }

        #endregion

        #region ToParseable

        /// <summary>
        /// Converts this <see cref="Complex"/> object into a string that 
        /// can be parsed back via Parse using the <see cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <exception cref="System.FormatException"> If an format exception occured. </exception>
        /// <returns> A parseable string in the format "real imag". </returns>
        public string ToParseable()
        {
            return ToParseable( CultureInfo.InvariantCulture );
        }

        /// <summary>
        /// Converts this <see cref="Complex"/> object into a string that can be parsed back via Parse.
        /// </summary>
        /// <exception cref="System.FormatException"> If an format exception occured. </exception>
        /// <param name="formatProvider"> The format settings to use. </param>
        /// <returns> A parseable string in the format "real imag". </returns>
        public string ToParseable( System.IFormatProvider formatProvider )
        {
            return Real.ToString( formatProvider ) + ' ' + Imag.ToString( formatProvider );
        }

        #endregion ToParseable

        #region IEnumerable<float> Members

        /// <summary>
        /// Returns an enumerator that iterates over the components of this <see cref="Complex"/> number.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public System.Collections.Generic.IEnumerator<float> GetEnumerator()
        {
            yield return Real;
            yield return Imag;
        }

        /// <summary>
        /// Returns an enumerator that iterates over the components of this <see cref="Complex"/> number.
        /// </summary>
        /// <returns>An enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        #region - Logic -

        /// <summary>
        /// Returns whether the given Complex numbers are equal.
        /// </summary>
        /// <param name="left">
        /// The Complex number on the left side of the equation.
        /// </param>
        /// <param name="right">
        /// The Complex number on the right side of the equation.
        /// </param>
        /// <returns>
        /// True if they are approximately equal; otherwise false.
        /// </returns>
        public static bool operator ==( Complex left, Complex right )
        {
            if( left.IsNaN || right.IsNaN )
                return false;

            return left.Real.IsApproximate( right.Real ) &&
                   left.Imag.IsApproximate( right.Imag );
        }

        /// <summary>
        /// Returns whether the given Complex numbers are not equal.
        /// </summary>
        /// <param name="left">
        /// The Complex number on the left side of the equation.
        /// </param>
        /// <param name="right">
        /// The Complex number on the right side of the equation.
        /// </param>
        /// <returns>
        /// True if they are approximately not equal; otherwise false.
        /// </returns>
        public static bool operator !=( Complex left, Complex right )
        {
            if( left.IsNaN || right.IsNaN )
                return true;

            return !left.Real.IsApproximate( right.Real ) ||
                   !left.Imag.IsApproximate( right.Imag );
        }

        #endregion

        #region - Cast -

        /// <summary>
        /// Provides implicit casting of a real number into a Complex number.
        /// </summary>
        /// <param name="value">The real number to convert.</param>
        /// <returns>The converted Complex number that represents the real value.</returns>
        public static implicit operator Complex( float value )
        {
            return new Complex( value, 0.0f );
        }

        #endregion

        #region - Math -

        /// <summary>
        /// Returns the result of adding 
        /// the <paramref name="right"/> <see cref="Complex"/> number to the <paramref name="left"/>
        /// <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex operator +( Complex left, Complex right )
        {
            Complex result;

            result.Real = left.Real + right.Real;
            result.Imag = left.Imag + right.Imag;

            return result;
        }

        /// <summary>
        /// Returns the result of adding 
        /// the <paramref name="right"/> real number to the <paramref name="left"/>
        /// <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex operator +( Complex left, float right )
        {
            Complex result;

            result.Real = left.Real + right;
            result.Imag = left.Imag;

            return result;
        }

        /// <summary>
        /// Returns the result of adding 
        /// the <paramref name="left"/> real number to the <paramref name="right"/>
        /// <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex operator +( float left, Complex right )
        {
            Complex result;

            result.Real = left + right.Real;
            result.Imag = right.Imag;

            return result;
        }

        /// <summary>
        /// Negates the specified <see cref="Complex"/> number.
        /// </summary>
        /// <param name="number"> The number to negate. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex operator -( Complex number )
        {
            Complex result;

            result.Real = -number.Real;
            result.Imag = -number.Imag;

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> <see cref="Complex"/> number
        /// from the left <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex operator -( Complex left, Complex right )
        {
            Complex result;

            result.Real = left.Real - right.Real;
            result.Imag = left.Imag - right.Imag;

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> <see cref="Complex"/> 
        /// number from the <paramref name="left"/> real number.
        /// </summary>
        /// <param name="left"> The real number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>/param>
        /// <returns></returns>
        public static Complex operator -( float left, Complex right )
        {
            Complex result;

            result.Real = left - right.Real;
            result.Imag = -right.Imag;

            return result;
        }

        /// <summary>
        /// Returns the result of subtracting the <paramref name="right"/> real number 
        /// number from the <paramref name="left"/> <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The real number on the right side. </param>
        /// <returns> The result of the operation. </returns>/param>
        /// <returns></returns>
        public static Complex operator -( Complex left, float right )
        {
            Complex result;

            result.Real = left.Real - right;
            result.Imag = left.Imag;

            return result;
        }

        /// <summary>
        /// Multiplies the elements of the <see cref="Complex"/> number with the real number.
        /// </summary>
        /// <param name="left"> The real number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex operator *( float left, Complex right )
        {
            Complex result;

            result.Real = left * right.Real;
            result.Imag = left * right.Imag;

            return result;
        }

        /// <summary>
        /// Multiplies the elements of the <see cref="Complex"/> number with the real number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The real number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex operator *( Complex left, float right )
        {
            Complex result;

            result.Real = left.Real * right;
            result.Imag = left.Imag * right;

            return result;
        }

        /// <summary>
        /// Multiplies the left <see cref="Complex"/> number with the right <see cref="Complex"/> number.
        /// </summary>
        /// <remarks>
        /// Remember that left*right may not be equal to right*left.
        /// </remarks>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex operator *( Complex left, Complex right )
        {
            Complex result;

            result.Real = (left.Real * right.Real) - (left.Imag * right.Imag);
            result.Imag = (left.Real * right.Imag) + (left.Imag * right.Real);

            return result;
        }

        /// <summary>
        /// Divides the left <see cref="Complex"/> number through the right.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex operator /( Complex left, Complex right )
        {
            if( right.IsZero )
                return Complex.Infinity;

            float rightModulus = right.SquaredLength;

            Complex result;

            result.Real = ((left.Real * right.Real) + (left.Imag * right.Imag)) / rightModulus;
            result.Imag = ((left.Imag * right.Real) - (left.Real * right.Imag)) / rightModulus;

            return result;
        }

        /// <summary>
        /// Divides the left real number through the right <see cref="Complex"/> number.
        /// </summary>
        /// <param name="left"> The real number on the left side. </param>
        /// <param name="right">The <see cref="Complex"/> number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex operator /( float left, Complex right )
        {
            if( right.IsZero )
                return Complex.Infinity;

            float mod = right.SquaredLength;
            Complex result;

            result.Real = left * right.Real / mod;
            result.Imag = -left * right.Imag / mod;

            return result;
        }

        /// <summary>
        /// Divides the left <see cref="Complex"/> number through the right real number.
        /// </summary>
        /// <param name="left"> The <see cref="Complex"/> number on the left side. </param>
        /// <param name="right">The real number on the right side. </param>
        /// <returns> The result of the operation. </returns>
        public static Complex operator /( Complex left, float right )
        {
            Complex result;

            result.Real = left.Real / right;
            result.Imag = left.Imag / right;

            return result;
        }

        #endregion

        #endregion
    }
}