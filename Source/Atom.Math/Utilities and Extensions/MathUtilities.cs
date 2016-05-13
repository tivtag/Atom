// <copyright file="MathUtilities.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.MathUtilities class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Static class that contains math-utility methods.
    /// </summary>
    public static class MathUtilities
    {
        #region > Clamp <

        /// <summary>
        /// Returns the given <paramref name="value"/> clamped
        /// into the range defined by <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static float Clamp( this float value, float min, float max )
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;

            return value;
        }

        /// <summary>
        /// Returns the given <paramref name="value"/> clamped
        /// into the range defined by <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static double Clamp( this double value, double min, double max )
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;

            return value;
        }

        /// <summary>
        /// Returns the given <paramref name="value"/> clamped
        /// into the range defined by <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static decimal Clamp( this decimal value, decimal min, decimal max )
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;

            return value;
        }

        /// <summary>
        /// Returns the given <paramref name="value"/> clamped
        /// into the range defined by <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static byte Clamp( this byte value, byte min, byte max )
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;

            return value;
        }

        /// <summary>
        /// Returns the given <paramref name="value"/> clamped
        /// into the range defined by <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static short Clamp( this short value, short min, short max )
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;

            return value;
        }

        /// <summary>
        /// Returns the given <paramref name="value"/> clamped
        /// into the range defined by <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static int Clamp( this int value, int min, int max )
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;

            return value;
        }

        /// <summary>
        /// Returns the given <paramref name="value"/> clamped
        /// into the range defined by <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static long Clamp( this long value, long min, long max )
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;

            return value;
        }

        #endregion

        #region > Wrap <

        /// <summary>
        /// Reduces a given angle to a value between <b>π</b> and -<b>π</b>.
        /// </summary>
        /// <param name="angle">
        /// The angle to reduce, in radians.
        /// </param>
        /// <returns>
        /// The new angle, in radians.
        /// </returns>
        public static float WrapAngle( float angle )
        {
            angle = (float)Math.IEEERemainder( (double)angle, 6.2831854820251465 );

            if( angle <= -3.141593f )
            {
                angle += 6.283185f;
                return angle;
            }

            if( angle > 3.141593f )
                angle -= 6.283185f;

            return angle;
        }

        #endregion

        #region > Angle Conversation <

        #region - ToRadians -

        /// <summary>
        /// Converts degrees into radians.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        public static float ToRadians( float degrees )
        {
            return degrees * 0.01745329f;
        }

        /// <summary>
        /// Converts degrees into radians.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        public static double ToRadians( double degrees )
        {
            return degrees * 0.01745329;
        }

        /// <summary>
        /// Converts degrees into radians.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        public static decimal ToRadians( decimal degrees )
        {
            return degrees * 0.01745329m;
        }

        #endregion

        #region - ToDegrees -

        /// <summary>
        /// Converts radians into degrees.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        public static float ToDegrees( float radians )
        {
            return radians * 57.29578f;
        }

        /// <summary>
        /// Converts radians into degrees.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        public static double ToDegrees( double radians )
        {
            return radians * 57.29578;
        }

        /// <summary>
        /// Converts radians into degrees.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        public static decimal ToDegrees( decimal radians )
        {
            return radians * 57.29578m;
        }

        #endregion

        #endregion

        #region > Interpolation <

        /// <summary>
        /// Performs Linear intERPolation between two values.
        /// </summary>
        /// <param name="start">The source value that represents the start point.</param>
        /// <param name="end">The source value that represents the end point.</param>
        /// <param name="amount">The weighting factor.</param>
        /// <returns>The interpolated value.</returns>
        public static byte Lerp( byte start, byte end, float amount )
        {
            return (byte)(start + ((end - start) * amount));
        }

        /// <summary>
        /// Performs Linear intERPolation between two values.
        /// </summary>
        /// <param name="start">The source value that represents the start point.</param>
        /// <param name="end">The source value that represents the end point.</param>
        /// <param name="amount">The weighting factor.</param>
        /// <returns>The interpolated value.</returns>
        public static float Lerp( float start, float end, float amount )
        {
            return start + ((end - start) * amount);
        }

        /// <summary>
        /// Performs interpolation between two values using a cubic equation.
        /// </summary>
        /// <param name="start">The source value that represents the start point.</param>
        /// <param name="end">The source value that represents the end point.</param>
        /// <param name="amount">The weighting factor.</param>
        /// <returns>The interpolated value.</returns>
        public static float SmoothStep( float start, float end, float amount )
        {
            amount = Clamp( amount, 0f, 1f );
            return Lerp( start, end, (amount * amount) * (3f - (2f * amount)) );
        }

        /// <summary>
        /// Performs COSine intERPolation between two values.
        /// </summary>
        /// <param name="start">The source value that represents the start point.</param>
        /// <param name="end">The source value that represents the end point.</param>
        /// <param name="amount">The weighting factor.</param>
        /// <returns>The interpolated value.</returns>
        public static float Coserp( float start, float end, float amount )
        {
            float factor = 0.5f * (1.0f - (float)System.Math.Cos( amount * System.Math.PI ));
            return (start * (1.0f - factor)) + (end * factor);
        }

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="valueA">The first source position.</param>
        /// <param name="tangentA">The first source tangent.</param>
        /// <param name="valueB">The second source position.</param>
        /// <param name="tangentB">The second source tangent.</param>
        /// <param name="amount">The weighting factor.</param>
        /// <returns>The interpolated value.</returns>
        public static float Hermite( float valueA, float tangentA, float valueB, float tangentB, float amount )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            float weightA = ((2f * amountPow3) - (3f * amountPow2)) + 1f;
            float weightB = (-2f * amountPow3) + (3f * amountPow2);
            float weightA2 = (amountPow3 - (2f * amountPow2)) + amount;
            float weightB2 = amountPow3 - amountPow2;

            return (valueA * weightA) + (valueB * weightB) + (tangentA * weightA2) + (tangentB * weightB2);
        }

        /// <summary>
        /// Performs Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">The weighting factor.</param>
        /// <returns>A position that is the result of the Catmull-Rom interpolation.</returns>
        public static float CatmullRom( float value1, float value2, float value3, float value4, float amount )
        {
            float amountPow2 = amount * amount;
            float amountPow3 = amount * amountPow2;

            return 0.5f * ((((2f * value2) + ((-value1 + value3) * amount)) + (((((2f * value1) - (5f * value2)) + (4f * value3)) - value4) * amountPow2)) + ((((-value1 + (3f * value2)) - (3f * value3)) + value4) * amountPow3));
        }

        #endregion

        #region > Misc <

        /// <summary>
        /// Rounds the given value to a multiple of the given target value.
        /// </summary>
        /// <param name="value">
        /// The value to round.
        /// </param>
        /// <param name="target">
        /// The target value to round to.
        /// </param>
        /// <returns>
        /// The rounded value.
        /// </returns>
        public static float RoundToMultiple( float value, float target )
        {
            return (float)(Math.Round( value / target ) * target);
        }

        /// <summary>
        /// Gets the nearest value that is a multiplicative of a given base value.
        /// </summary>
        /// <param name="value">
        /// The input value.
        /// </param>
        /// <param name="baseValue">
        /// The base value.
        /// </param>
        /// <returns>
        /// The nearest value.
        /// </returns>
        public static int GetNearestMul( int value, int baseValue )
        {
            return baseValue * (int)System.Math.Round( value / (double)baseValue );
        }

        /// <summary>
        /// Gets the nearest value that is a multiplicative of the given baseValue.
        /// </summary>
        /// <param name="baseValue">
        /// The base value to scale.
        /// </param>
        /// <param name="maximumValue">
        /// The maximum allowed value.
        /// </param>
        /// <returns>
        /// The scaled value.
        /// </returns>
        public static Point2 GetNearestSmallerMul( Point2 maximumValue, Point2 baseValue )
        {
            Contract.Requires<ArgumentException>( baseValue.X > 0 );
            Contract.Requires<ArgumentException>( baseValue.Y > 0 );
            Contract.Requires<ArgumentException>( baseValue.X <= maximumValue.X );
            Contract.Requires<ArgumentException>( baseValue.Y <= maximumValue.Y );

            int multiplier = 2;

            do
            {
                Point2 scaledValue = baseValue * multiplier;

                if( scaledValue.X > maximumValue.X || scaledValue.Y > maximumValue.Y )
                {
                    --multiplier;
                    break;
                }
                else
                {
                    ++multiplier;
                }
            }
            while( true );

            return baseValue * multiplier;
        }

        #region - Barycentric -

        /// <summary>
        /// Returns the Cartesian coordinate for one axis of a point that is defined
        /// by a given triangle and two normalized barycentric (areal) coordinates.
        /// </summary>
        /// <param name="value1">
        /// The coordinate on one axis of vertex 1 of the defining triangle.
        /// </param>
        /// <param name="value2">
        /// The coordinate on the same axis of vertex 2 of the defining triangle.
        /// </param>
        /// <param name="value3">
        /// The coordinate on the same axis of vertex 3 of the defining triangle.
        /// </param>
        /// <param name="amount1">
        /// The normalized barycentric (areal) coordinate b2,
        /// equal to the weighting factor for vertex 2,
        /// the coordinate of which is specified in value2.
        /// </param>
        /// <param name="amount2">
        /// The normalized barycentric (areal) coordinate b3,
        /// equal to the weighting factor for vertex 3,
        /// the coordinate of which is specified in value3.
        /// </param>
        /// <returns>
        /// Cartesian coordinate of the specified point with respect to the axis being used.
        /// </returns>
        public static float Barycentric( float value1, float value2, float value3, float amount1, float amount2 )
        {
            return value1 + (amount1 * (value2 - value1)) + (amount2 * (value3 - value1));
        }

        #endregion

        #region - Hypotenuse -

        /// <summary>
        /// Calculates the Hypotenuse of a triangle.
        /// </summary>
        /// <remarks>
        /// The hypotenuse of a right triangle is the triangle's longest side; 
        /// the side opposite the right angle.
        /// </remarks>
        /// <param name="cathetusA">
        /// The first shorter sid of the triangle.
        /// </param>
        /// <param name="cathetusB">
        /// The second short side of the triangle.
        /// </param>
        /// <returns>
        /// The hypotenuse of a triangle.
        /// </returns>
        public static float Hypotenuse( float cathetusA, float cathetusB )
        {
            float hypotenuse;

            float absA = System.Math.Abs( cathetusA );
            float absB = System.Math.Abs( cathetusB );

            if( absA > absB )
            {
                hypotenuse = cathetusB / cathetusA;
                hypotenuse = absA * (float)System.Math.Sqrt( 1 + (hypotenuse * hypotenuse) );
            }
            else if( cathetusB != 0 )
            {
                hypotenuse = cathetusA / cathetusB;
                hypotenuse = absB * (float)System.Math.Sqrt( 1 + (hypotenuse * hypotenuse) );
            }
            else
            {
                hypotenuse = 0.0f;
            }

            return hypotenuse;
        }

        /// <summary>
        /// Calculates the Hypotenuse of a triangle.
        /// </summary>
        /// <remarks>
        /// The hypotenuse of a right triangle is the triangle's longest side; 
        /// the side opposite the right angle.
        /// </remarks>
        /// <param name="cathetusA">
        /// The first shorter sid of the triangle.
        /// </param>
        /// <param name="cathetusB">
        /// The second short side of the triangle.
        /// </param>
        /// <returns>
        /// The hypotenuse of a triangle.
        /// </returns>
        public static double Hypotenuse( double cathetusA, double cathetusB )
        {
            double hypotenuse;

            double absA = System.Math.Abs( cathetusA );
            double absB = System.Math.Abs( cathetusB );

            if( absA > absB )
            {
                hypotenuse = cathetusB / cathetusA;
                hypotenuse = absA * System.Math.Sqrt( 1 + (hypotenuse * hypotenuse) );
            }
            else if( cathetusB != 0 )
            {
                hypotenuse = cathetusA / cathetusB;
                hypotenuse = absB * System.Math.Sqrt( 1 + (hypotenuse * hypotenuse) );
            }
            else
            {
                hypotenuse = 0.0;
            }

            return hypotenuse;
        }

        #endregion

        #region - Super Formula -

        /// <summary>
        /// The Super Formula can be used to create 2 dimensional shapes
        /// using one varying angle and six constants.
        /// </summary>
        /// <param name="angle"> The angle to calculate. </param>
        /// <param name="a">The first input constants.</param>
        /// <param name="b">The second input constants.</param>
        /// <param name="m">The third input constant. Modifies how many extra 'legs' the shape has. </param>
        /// <param name="n1">The fourth input constant.</param>
        /// <param name="n2">The fifth input constant.</param>
        /// <param name="n3">The sixth input constant.</param>
        /// <returns>
        /// The radius from the orgin for the current angle and constants.
        /// </returns>
        public static double SuperFormula(
            double angle,
            double a,
            double b,
            double m,
            double n1,
            double n2,
            double n3 )
        {
            double anglePart = (m * angle) / 4.0;

            double cosPart = System.Math.Cos( anglePart ) / a;
            cosPart = System.Math.Pow( System.Math.Abs( cosPart ), n2 );

            double sinPart = System.Math.Sin( anglePart ) / b;
            sinPart = System.Math.Pow( System.Math.Abs( sinPart ), n3 );

            return System.Math.Pow( (cosPart + sinPart), -1.0 / n1 );
        }

        #endregion

        #region - ForN -

        /// <summary>
        /// Executes the given <paramref name="action"/> in the range defined by
        /// <paramref name="fromInclusive"/> and <paramref name="toExclusive"/> in N-dimensions.
        /// </summary>
        /// <param name="fromInclusive">The starting value.</param>
        /// <param name="toExclusive">The end value.</param>
        /// <param name="step">The size of a single step.</param>
        /// <param name="dimensionCount">The number of dimensions.</param>
        /// <param name="action">
        /// The action to execute. The given int array represents the arguments of the function.
        /// The arguments array is reused by this recursive function. Clone the arguments array if you want to modify it.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="action"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the given <paramref name="dimensionCount"/> is less or equals zero.
        /// -/-
        /// Or if <paramref name="step"/> is zero.
        /// </exception>
        public static void ForN( int fromInclusive, int toExclusive, int step, int dimensionCount, Action<int[]> action )
        {
            #region Verify Arguments

            if( action == null )
                throw new ArgumentNullException( "action" );

            if( dimensionCount <= 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsZeroOrNegative, "dimensionCount" );

            if( step == 0 )
                throw new ArgumentException( Atom.ErrorStrings.SpecifiedValueIsZero, "step" );

            #endregion

            int[] arguments = new int[dimensionCount];

            if( dimensionCount == 1 )
            {
                for( int i = fromInclusive; i < toExclusive; i += step )
                {
                    arguments[0] = i;
                    action( arguments );
                }
            }
            else
            {
                for( int i = fromInclusive; i < toExclusive; i += step )
                {
                    arguments[0] = i;
                    RecursiveForN( fromInclusive, toExclusive, step, 1, dimensionCount, arguments, action );
                }
            }
        }

        /// <summary>
        /// Executes the given <paramref name="action"/> in the range defined by
        /// <paramref name="fromInclusive"/> and <paramref name="toExclusive"/> in N-dimensions.
        /// </summary>
        /// <param name="fromInclusive">The starting value.</param>
        /// <param name="toExclusive">The end value.</param>
        /// <param name="step">The size of a single step.</param>
        /// <param name="currentDimension">The current dimension beeing executed by the recursive function.</param>
        /// <param name="dimensionCount">The number of dimensions.</param>
        /// <param name="arguments">The current arguments for the action.</param>
        /// <param name="action">The action to execute.</param>
        private static void RecursiveForN(
            int fromInclusive,
            int toExclusive,
            int step,
            int currentDimension,
            int dimensionCount,
            int[] arguments,
            Action<int[]> action )
        {
            for( int i = fromInclusive; i < toExclusive; i += step )
            {
                arguments[currentDimension] = i;

                if( (currentDimension + 1) == dimensionCount )
                {
                    action( arguments );
                }
                else
                {
                    RecursiveForN( fromInclusive, toExclusive, step, currentDimension + 1, dimensionCount, arguments, action );
                }
            }
        }

        #endregion

        #endregion
    }
}
