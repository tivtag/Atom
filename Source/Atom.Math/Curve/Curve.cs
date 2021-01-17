// <copyright file="Curve.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Curve class and various enumerations.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;
    using Atom.Diagnostics.Contracts;

    #region enum CurveTangent

    /// <summary>
    /// Specifies different tangent types to be calculated for CurveKey points in a Curve.
    /// </summary>
    public enum CurveTangent
    {
        /// <summary>
        /// A Flat tangent always has a value equal to zero.
        /// </summary>
        Flat,

        /// <summary>
        /// A Linear tangent at a CurveKey is equal to the difference between its CurveKey.Value and the Value
        /// of the preceding or succeeding CurveKey.
        /// </summary>
        /// <example>
        /// For example, in Curve MyCurve, where i is greater than zero and (i + 1) is less than 
        /// the total number of CurveKeys in MyCurve, the linear CurveKey.TangentIn of MyCurve.Keys[i] is equal to:
        /// ( MyCurve.Keys[i].Value - MyCurve.Keys[i - 1].Value )Similarly, the linear CurveKey.TangentOut is equal to: 
        /// ( MyCurve.Keys[i + 1].Value - MyCurve.Keys[i].Value.)
        /// </example>
        Linear,

        /// <summary>
        /// A Smooth tangent smooths the inflection between a CurveKey.TangentIn and CurveKey.TangentOut
        /// by taking into account the values of both neighbors of the CurveKey.
        /// </summary>
        /// <example>
        /// <para>
        /// The smooth CurveKey.TangentIn of MyCurve.Keys[i] is equal to: 
        /// ( ( MyCurve.Keys[i + 1].Value - MyCurve.Keys[i - 1].Value ) * 
        /// ( ( MyCurve.Keys[i].Position - MyCurve.Keys[i - 1].Position ) / 
        /// ( MyCurve.Keys[i + 1].Position - MyCurve.Keys[i-1].Position ) ) )
        /// </para><para>
        /// Similarly, the smooth CurveKey.TangentOut is equal to: 
        /// ( ( MyCurve.Keys[i + 1].Value - MyCurve.Keys[i - 1].Value ) *
        /// ( ( MyCurve.Keys[i + 1].Position - MyCurve.Keys[i].Position ) /
        /// ( MyCurve.Keys[i + 1].Position - MyCurve.Keys[i - 1].Position ) ) )
        /// </para>
        /// </example>
        Smooth
    }

    #endregion

    #region enum CurveLoopType

    /// <summary>
    /// Defines how the value of a Curve will be determined for positions
    /// before the first point on the Curve or after the last point on the Curve.
    /// </summary>
    public enum CurveLoopType
    {
        /// <summary>
        /// The Curve will evaluate to its first key for positions before 
        /// the first point in the Curve and to the last key for positions after the last point.
        /// </summary>
        Constant,

        /// <summary>
        /// Positions specified past the ends of the curve will wrap around to the opposite side of the Curve.
        /// </summary>
        Cycle,

        /// <summary>
        /// Positions specified past the ends of the curve will wrap around to the opposite side of the Curve.
        /// The value will be offset by the difference between the values of the first and last CurveKey multiplied
        /// by the number of times the position wraps around. If the position is before the first point in the Curve,
        /// the difference will be subtracted from its value; otherwise, the difference will be added.
        /// </summary>
        CycleOffset,

        /// <summary>
        /// Linear interpolation will be performed to determine the value.
        /// </summary>
        Oscillate,

        /// <summary>
        /// Positions specified past the ends of the Curve act as an offset from the same side of the Curve toward the opposite side.
        /// </summary>
        Linear
    }

    #endregion

    #region enum CurveContinuity

    /// <summary>
    /// Defines the continuity of CurveKeys on a Curve.
    /// </summary>
    public enum CurveContinuity
    {
        /// <summary>
        /// Interpolation can be used between this CurveKey and the next.
        /// </summary>
        Smooth = 0,

        /// <summary>
        /// Interpolation cannot be used between this CurveKey and the next. 
        /// Specifying a position between the two points returns this point.
        /// </summary>
        Step
    }

    #endregion

    /// <summary>
    /// Stores an arbitrary collection of 2D CurveKey points, 
    /// and provides methods for evaluating features of the curve they define.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverter( typeof( System.ComponentModel.ExpandableObjectConverter ) )]
    public class Curve : ICloneable
    {
        #region [ Properties ]

        /// <summary>
        /// Gets a value indicating whether this Curve is constant.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if the curve is constant (has one or fewer points);
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsConstant
        {
            get
            {
                return this.keys.Count <= 1;
            }
        }

        /// <summary>
        /// Gets the <see cref="CurveKeyCollection"/> that contains
        /// the <see cref="CurveKey"/>s that make-up this Curve.
        /// </summary>
        /// <value>
        /// The reference of the <see cref="CurveKeyCollection"/> that contains the <see cref="CurveKey"/>s that make-up this Curve.
        /// </value>
        public CurveKeyCollection Keys
        {
            get
            {
                return this.keys;
            }
        }

        /// <summary>
        /// Gets or sets how to handle weighting values that are greater than the last control point in the curve.
        /// </summary>
        /// <value>Specifies how to handle weighting values.</value>
        public CurveLoopType PostLoop
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets how to handle weighting values that are less than the first control point in the curve.
        /// </summary>
        /// <value>Specifies how to handle weighting values.</value>
        public CurveLoopType PreLoop
        {
            get;
            set;
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve"/> class.
        /// </summary>
        public Curve()
        {
            this.keys = new CurveKeyCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve"/> class,
        /// cloning the given Curve.
        /// </summary>
        /// <param name="curve">The curve to clone.</param>
        public Curve( Curve curve )
        {
            Contract.Requires<ArgumentNullException>( curve != null );

            this.keys     = curve.keys.Clone();
            this.PreLoop  = curve.PreLoop;
            this.PostLoop = curve.PostLoop;
        }

        #endregion

        #region [ Methods ]

        #region ComputeTangent

        /// <summary>
        /// Computes both the CurveKey.TangentIn and the CurveKey.TangentOut for a CurveKey specified by its index.
        /// </summary>
        /// <param name="keyIndex">
        /// The index of the CurveKey for which to compute tangents (in the Curve.Keys collection of the Curve).
        /// </param>
        /// <param name="tangentType">
        /// The type of tangents to compute.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If the given <paramref name="keyIndex"/> is out of valid range.
        /// </exception>
        public void ComputeTangent( int keyIndex, CurveTangent tangentType )
        {
            this.ComputeTangent( keyIndex, tangentType, tangentType );
        }

        /// <summary>
        /// Computes a specified type of CurveKey.TangentIn and a specified type of CurveKey.TangentOut for a given CurveKey. 
        /// </summary>
        /// <param name="keyIndex">
        /// The index of the CurveKey for which to compute tangents (in the Curve.Keys collection of the Curve).
        /// </param>
        /// <param name="tangentInType">
        /// The type of CurveKey.TangentIn to compute.
        /// </param>
        /// <param name="tangentOutType">
        /// The type of CurveKey.TangentOut to compute.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If the given <paramref name="keyIndex"/> is out of valid range.
        /// </exception>
        public void ComputeTangent( int keyIndex, CurveTangent tangentInType, CurveTangent tangentOutType )
        {
            Contract.Requires<ArgumentOutOfRangeException>( keyIndex >= 0 && keyIndex < this.Keys.Count );
            
            CurveKey key = this.Keys[keyIndex];
            float position       = key.Position;
            float positionNext   = key.Position;
            float positionMiddle = key.Position;
            float value          = key.Value;
            float valueNext      = key.Value;
            float valueMiddle    = key.Value;

            if( keyIndex > 0 )
            {
                position = this.Keys[keyIndex - 1].Position;
                value    = this.Keys[keyIndex - 1].Value;
            }

            if( (keyIndex + 1) < this.keys.Count )
            {
                positionNext = this.Keys[keyIndex + 1].Position;
                valueNext    = this.Keys[keyIndex + 1].Value;
            }

            if( tangentInType == CurveTangent.Smooth )
            {
                float positionDelta = positionNext - position;
                float valueDelta    = valueNext    - value;

                if( System.Math.Abs( valueDelta ) < 1.192093E-07f )
                {
                    key.TangentIn = 0f;
                }
                else
                {
                    key.TangentIn = (valueDelta * System.Math.Abs( (float)(position - positionMiddle) )) / positionDelta;
                }
            }
            else if( tangentInType == CurveTangent.Linear )
            {
                key.TangentIn = valueMiddle - value;
            }
            else
            {
                key.TangentIn = 0f;
            }

            if( tangentOutType == CurveTangent.Smooth )
            {
                float positionDelta = positionNext - position;
                float valueDelta = valueNext - value;

                if( System.Math.Abs( valueDelta ) < 1.192093E-07f )
                {
                    key.TangentOut = 0f;
                }
                else
                {
                    key.TangentOut = (valueDelta * System.Math.Abs( (float)(positionNext - positionMiddle) )) / positionDelta;
                }
            }
            else if( tangentOutType == CurveTangent.Linear )
            {
                key.TangentOut = valueNext - valueMiddle;
            }
            else
            {
                key.TangentOut = 0f;
            }
        }

        #endregion

        #region ComputeTangents

        /// <summary>
        /// Computes all tangents for all CurveKeys in the Curve, 
        /// using a specified tangent type for both CurveKey.TangentIn and CurveKey.TangentOut. 
        /// </summary>
        /// <param name="tangentType">
        /// The type of CurveKey.TangentOut and CurveKey.TangentIn to compute.
        /// </param>
        public void ComputeTangents( CurveTangent tangentType )
        {
            this.ComputeTangents( tangentType, tangentType );
        }

        /// <summary>
        /// Computes all tangents for all CurveKeys in the Curve, 
        /// using different tangent types for CurveKey.TangentOut and CurveKey.TangentIn. 
        /// </summary>
        /// <param name="tangentInType">The type of CurveKey.TangentIn to compute.</param>
        /// <param name="tangentOutType">The type of CurveKey.TangentOut to compute.</param>
        public void ComputeTangents( CurveTangent tangentInType, CurveTangent tangentOutType )
        {
            int count = this.Keys.Count;
            for( int i = 0; i < count; ++i )
            {
                this.ComputeTangent( i, tangentInType, tangentOutType );
            }
        }

        #endregion

        #region Evaluate

        /// <summary>
        /// Finds the value at a position on the Curve.
        /// </summary>
        /// <param name="position">The position on the Curve.</param>
        /// <returns>The value at the position on the Curve.</returns>
        public float Evaluate( float position )
        {
            if( this.keys.Count == 0 )
                return 0f;

            if( this.keys.Count == 1 )
                return this.keys[0].Value;

            CurveKey keyStart = this.keys[0];
            CurveKey keyEnd   = this.keys[this.keys.Count - 1];

            float time = position;
            float value = 0f;

            if( time < keyStart.Position )
            {
                if( this.PreLoop == CurveLoopType.Constant )
                    return keyStart.Value;

                if( this.PreLoop == CurveLoopType.Linear )
                    return keyStart.Value - (keyStart.TangentIn * (keyStart.Position - time));

                if( !this.keys.IsCacheAvailable )
                    this.keys.ComputeCacheValues();

                float factor = this.CalcCycle( time );
                float delta  = time - (keyStart.Position + (factor * this.keys.TimeRange));
                
                if( this.PreLoop == CurveLoopType.Cycle )
                {
                    time = keyStart.Position + delta;
                }
                else if( this.PreLoop == CurveLoopType.CycleOffset )
                {
                    time = keyStart.Position + delta;
                    value = (keyEnd.Value - keyStart.Value) * factor;
                }
                else
                {
                    time = ((((int)factor) & 1) != 0) ? (keyEnd.Position - delta) : (keyStart.Position + delta);
                }
            }
            else if( keyEnd.Position < time )
            {
                if( this.PostLoop == CurveLoopType.Constant )
                    return keyEnd.Value;

                if( this.PostLoop == CurveLoopType.Linear )
                    return keyEnd.Value - (keyEnd.TangentOut * (keyEnd.Position - time));

                if( !this.keys.IsCacheAvailable )
                    this.keys.ComputeCacheValues();

                float factor = this.CalcCycle( time );
                float delta  = time - (keyStart.Position + (factor * this.keys.TimeRange));

                if( this.PostLoop == CurveLoopType.Cycle )
                {
                    time = keyStart.Position + delta;
                }
                else if( this.PostLoop == CurveLoopType.CycleOffset )
                {
                    time  = keyStart.Position + delta;
                    value = (keyEnd.Value - keyStart.Value) * factor;
                }
                else
                {
                    time = ((((int)factor) & 1) != 0) ? (keyEnd.Position - delta) : (keyStart.Position + delta);
                }
            }

            CurveKey key3, key4;
            time = this.FindSegment( time, out key4, out key3 );
            return value + Hermite( key4, key3, time );
        }

        #endregion

        #region CalcCycle

        /// <summary>
        /// Calculates ... .
        /// </summary>
        /// <param name="time">
        /// The time to move on the Curve.
        /// </param>
        /// <returns>
        /// The calculated value.
        /// </returns>
        private float CalcCycle( float time )
        {
            float number = (time - this.keys[0].Position) * this.keys.InvTimeRange;

            if( number < 0.0f )
                --number;

            return (float)((int)number);
        }

        #endregion

        #region FindSegment

        /// <summary>
        /// Finds the curve segment at the given <paramref name="time"/>.
        /// </summary>
        /// <param name="time">The time to move on the Curve.</param>
        /// <param name="keyA">Will contains the first CurveKey of the segement.</param>
        /// <param name="keyB">Will contains the second CurveKey of the segement.</param>
        /// <returns>The position on the Curve that marks the segment.</returns>
        private float FindSegment( float time, out CurveKey keyA, out CurveKey keyB )
        {
            keyA = this.keys[0];
            keyB = null;

            for( int i = 1; i < this.keys.Count; ++i )
            {
                keyB = this.keys[i];

                if( keyB.Position >= time )
                {
                    double positionA     = keyA.Position;
                    double positionB     = keyB.Position;
                    double positionDelta = positionB - positionA;
                    
                    if( positionDelta > 1E-10 )
                    {
                        return (float)((time - positionA) / positionDelta);
                    }
                    else
                    {
                        return 0f;
                    }
                }

                keyA = keyB;
            }

            return time;
        }

        #endregion

        #region Hermite

        /// <summary>
        /// Performs hermite-spline interpolation on two CurveKeys.
        /// </summary>
        /// <param name="keyA">The first key.</param>
        /// <param name="keyB">The second key.</param>
        /// <param name="time">The weighting factor.</param>
        /// <returns>The interpolated value. </returns>
        private static float Hermite( CurveKey keyA, CurveKey keyB, float time )
        {
            if( keyA.Continuity == CurveContinuity.Step )
            {
                if( time >= 1f )
                    return keyB.Value;
                return keyA.Value;
            }

            float timePow2 = time * time;
            float timePow3 = timePow2 * time;

            float valueA     = keyA.Value;
            float valueB     = keyB.Value;
            float tangentOut = keyA.TangentOut;
            float tangentIn  = keyB.TangentIn;

            return (((valueA * (((2f * timePow3) - (3f * timePow2)) + 1f)) + (valueB * ((-2f * timePow3) + (3f * timePow2)))) + (tangentOut * ((timePow3 - (2f * timePow2)) + time))) + (tangentIn * (timePow3 - timePow2));
        }

        #endregion

        #region Clone

        /// <summary>
        /// Creates a clone of the <see cref="Curve"/>.
        /// </summary>
        /// <returns>The cloned curve.</returns>
        public Curve Clone()
        {
            return new Curve( this );
        }

        /// <summary>
        /// Creates a clone of the <see cref="Curve"/>.
        /// </summary>
        /// <returns>The cloned curve.</returns>
        object ICloneable.Clone()
        {
            return new Curve( this );
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The collection of CurveKeys that make up this Curve.
        /// </summary>
        private readonly CurveKeyCollection keys;

        #endregion
    }
}
