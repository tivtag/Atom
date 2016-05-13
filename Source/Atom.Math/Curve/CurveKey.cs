// <copyright file="CurveKey.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.CurveKey class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;

    /// <summary>
    /// Represents a point in a multi-point curve.
    /// </summary>
    [System.Serializable]
    [System.ComponentModel.TypeConverter( typeof( System.ComponentModel.ExpandableObjectConverter ) )]
    public class CurveKey : IEquatable<CurveKey>, IComparable<CurveKey>, ICloneable
    {        
        #region [ Properties ]

        /// <summary>
        /// Gets or sets a value that describes whether the segment between 
        /// this point and the next point in the curve is discrete or continuous.
        /// </summary>
        /// <value>The CurveContinuity at this CurveKey.</value>
        public CurveContinuity Continuity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position of this CurveKey in the curve.
        /// </summary>
        /// <value>The position of this CurveKey.</value>
        public float Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value that descripes tangent when approaching this point to the next point in the curve.
        /// </summary>
        /// <value>The tangent when approaching.</value>
        public float TangentIn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value that descripes tangent when leaving this point to the next point in the curve.
        /// </summary>
        /// <value>The tangent when leaving.</value>
        public float TangentOut
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value at this CurveKey.
        /// </summary>
        /// <value>The value at this CurveKey.</value>
        public float Value
        {
            get;
            set;
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveKey"/> class.
        /// </summary>
        public CurveKey()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveKey"/> class.
        /// </summary>
        /// <param name="position">The position in the curve.</param>
        /// <param name="value">The value of the control point.</param>
        public CurveKey(float position, float value)
        {
            this.Position = position;
            this.Value    = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveKey"/> class.
        /// </summary>
        /// <param name="position">The position in the curve.</param>
        /// <param name="value">The value of the control point.</param>
        /// <param name="tangentIn">
        /// The tangent approaching point from the previous point in the curve.
        /// </param>
        /// <param name="tangentOut">
        /// The tangent leaving point toward  the next point in the curve.
        /// </param>
        public CurveKey(float position, float value, float tangentIn, float tangentOut)
        {
            this.Position   = position;
            this.Value      = value;
            this.TangentIn  = tangentIn;
            this.TangentOut = tangentOut;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveKey"/> class.
        /// </summary>
        /// <param name="position">The position in the curve.</param>
        /// <param name="value">The value of the control point.</param>
        /// <param name="tangentIn">
        /// The tangent approaching point from the previous point in the curve.
        /// </param>
        /// <param name="tangentOut">
        /// The tangent leaving point toward  the next point in the curve.
        /// </param>
        /// <param name="continuity">
        /// Enum indicating whether the curve is discrete or continuous.
        /// </param>
        public CurveKey( float position, float value, float tangentIn, float tangentOut, CurveContinuity continuity )
        {
            this.Position   = position;
            this.Value      = value;
            this.TangentIn  = tangentIn;
            this.TangentOut = tangentOut;
            this.Continuity = continuity;
        }

        #endregion

        #region [ Methods ]

        #region > Overrides/Impls <

        #region Equals

        /// <summary>
        /// Determines whether the specified System.Object is equal to the CurveKey. 
        /// </summary>
        /// <param name="other">The CurveKey to compare with the current CurveKey.</param>
        /// <returns>true if the specified System.Object is equal to the current CurveKey; false otherwise. </returns>
        public bool Equals( CurveKey other )
        {
            if( (object)other == null )
                return false;

            return other.Position   == this.Position   &&
                   other.Value      == this.Value      && 
                   other.TangentIn  == this.TangentIn  &&
                   other.TangentOut == this.TangentOut &&
                   other.Continuity == this.Continuity;
        }

        /// <summary>
        /// Determines whether the specified System.Object is equal to the CurveKey. 
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current CurveKey.</param>
        /// <returns>true if the specified System.Object is equal to the current CurveKey; false otherwise. </returns>
        public override bool Equals( object obj )
        {
            return this.Equals( obj as CurveKey );
        }

        #endregion

        #region CompareTo

        /// <summary>
        /// Compares this instance to another CurveKey and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">CurveKey to compare to.</param>
        /// <returns>
        /// Zero if the positions are the same; 
        /// -1 if this CurveKey comes before other and
        /// 1 if this CurveKey comes after other.
        /// </returns>
        public int CompareTo( CurveKey other )
        {
            if( other == null )
                return 1;

            if( this.Position == other.Position )
                return 0;

            if( this.Position > other.Position )
                return 1;

            return -1;
        }

        #endregion

        #region Clone

        /// <summary>
        /// Returns a clone of the <see cref="CurveKey"/>.
        /// </summary>
        /// <returns>The cloned CurveKey.</returns>
        public CurveKey Clone()
        {
            return new CurveKey( this.Position, this.Value, this.TangentIn, this.TangentOut, this.Continuity );
        }

        /// <summary>
        /// Returns a clone of the <see cref="CurveKey"/>.
        /// </summary>
        /// <returns>The cloned CurveKey.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        /// <summary>
        /// Overriden to return the hash-code of the CurveKey.
        /// </summary>
        /// <returns>The hash-code.</returns>
        public override int GetHashCode()
        {
            return this.Position.GetHashCode()  + this.Value.GetHashCode() +
                   this.TangentIn.GetHashCode() + this.TangentOut.GetHashCode()    +
                   this.Continuity.GetHashCode();
        }

        #endregion

        #endregion

        #region [ Operators ]

        /// <summary>
        /// Returns whether the given CurveKeys are equal.
        /// </summary>
        /// <param name="left">The CurveKey instance on the left side of the equation.</param>
        /// <param name="right">The CurveKey instance on the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==( CurveKey left, CurveKey right )
        {
            // If both are null, or both are same instance, return true.
            if( Object.ReferenceEquals( left, right ) )
                return true;

            // If one is null, but not both, return false.
            if( ((object)left == null) || ((object)right == null) )
                return false;

            return left.Position   == right.Position   &&
                   left.Value      == right.Value      && 
                   left.TangentIn  == right.TangentIn  &&
                   left.TangentOut == right.TangentOut &&
                   left.Continuity == right.Continuity;
        }

        /// <summary>
        /// Returns whether the given CurveKeys are inequal.
        /// </summary>
        /// <param name="left">The CurveKey instance on the left side of the equation.</param>
        /// <param name="right">The CurveKey instance on the right side of the equation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if they are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=( CurveKey left, CurveKey right )
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns whether the given <see cref="CurveKey"/> on the <paramref name="left"/> side 
        /// is less than the <see cref="CurveKey"/> on the <paramref name="right"/> side.
        /// </summary>
        /// <param name="left">The CurveKey instance on the left side of the equation.</param>
        /// <param name="right">The CurveKey instance on the right side of the equation.</param>
        /// <returns>The result of the comparisation.</returns>
        public static bool operator <( CurveKey left, CurveKey right )
        {
            return Compare( left, right ) < 0;
        }

        /// <summary>
        /// Returns whether the given <see cref="CurveKey"/> on the <paramref name="left"/> side 
        /// is less than or equal the <see cref="CurveKey"/> on the <paramref name="right"/> side.
        /// </summary>
        /// <param name="left">The CurveKey instance on the left side of the equation.</param>
        /// <param name="right">The CurveKey instance on the right side of the equation.</param>
        /// <returns>The result of the comparisation.</returns>
        public static bool operator <=( CurveKey left, CurveKey right )
        {
            return Compare( left, right ) <= 0;
        }

        /// <summary>
        /// Returns whether the given <see cref="CurveKey"/> on the <paramref name="left"/> side 
        /// is greater than the <see cref="CurveKey"/> on the <paramref name="right"/> side.
        /// </summary>
        /// <param name="left">The CurveKey instance on the left side of the equation.</param>
        /// <param name="right">The CurveKey instance on the right side of the equation.</param>
        /// <returns>The result of the comparisation.</returns>
        public static bool operator >( CurveKey left, CurveKey right )
        {
            return Compare( left, right ) > 0;
        }

        /// <summary>
        /// Returns whether the given <see cref="CurveKey"/> on the <paramref name="left"/> side 
        /// is great than or equal the <see cref="CurveKey"/> on the <paramref name="right"/> side.
        /// </summary>
        /// <param name="left">The CurveKey instance on the left side of the equation.</param>
        /// <param name="right">The CurveKey instance on the right side of the equation.</param>
        /// <returns>The result of the comparisation.</returns>
        public static bool operator >=( CurveKey left, CurveKey right )
        {
            return Compare( left, right ) >= 0;
        }

        /// <summary>
        /// Helper method that compares the given instances,
        /// taking into account the .net design guidelines.
        /// </summary>
        /// <param name="left">The CurveKey instance on the left side of the equation.</param>
        /// <param name="right">The CurveKey instance on the right side of the equation.</param>
        /// <returns>
        /// 0 : if the given instances are equal, or both null.
        /// 1 : if right is greater than left, or left is null.
        /// -1 : if right is less than left, or right is null.
        /// </returns>
        private static int Compare( CurveKey left, CurveKey right )
        {
            if( (object)left == null )
            {
                if( (object)right == null )
                {
                    // both are null:
                    return 0;
                }

                // left is null, but right is not null:
                return -1;
            }

            return left.CompareTo( right );
        }

        #endregion
    }
}
