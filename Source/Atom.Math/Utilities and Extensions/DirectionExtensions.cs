// <copyright file="DirectionExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.DirectionExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    /// <summary>
    /// Defines extension methods related to the various Direction and Orientation enumerations.
    /// </summary>
    /// <seealso cref="Direction4"/>
    /// <seealso cref="Direction8"/>
    /// <seealso cref="Directions"/>
    /// <seealso cref="Orientation"/>
    /// <seealso cref="TurnDirection"/>
    public static class DirectionExtensions
    {
        #region - Orientation -

        /// <summary>
        /// Inverts the given <see cref="Orientation"/>.
        /// </summary>
        /// <param name="orientation"> 
        /// The <see cref="Orientation"/> to invert.
        /// </param>
        /// <returns>
        /// The inverted Orientation.
        /// </returns>
        public static Orientation Invert( this Orientation orientation )
        {
            switch( orientation )
            {
                case Orientation.Horizontal:
                    return Orientation.Vertical;

                case Orientation.Vertical:
                    return Orientation.Horizontal;

                default:
                    return Orientation.None;
            }
        }

        #endregion

        #region - Invert4 -

        /// <summary>
        /// Inverts the given <see cref="Direction4"/>.
        /// </summary>
        /// <param name="direction"> 
        /// The <see cref="Direction4"/> to invert.
        /// </param>
        /// <returns>
        /// The inverted direction.
        /// </returns>
        public static Direction4 Invert( this Direction4 direction )
        {
            switch( direction )
            {
                case Direction4.Left:
                    return Direction4.Right;
                case Direction4.Right:
                    return Direction4.Left;
                case Direction4.Up:
                    return Direction4.Down;
                case Direction4.Down:
                    return Direction4.Up;
                default:
                    return Direction4.None;
            }
        }

        /// <summary>
        /// Inverts the given <see cref="Direction4"/> horizontally (on the x-axis).
        /// </summary>
        /// <param name="direction">
        /// The <see cref="Direction4"/> to invert on the x-axis.
        /// </param>
        /// <returns>
        /// The inverted direction.
        /// </returns>
        public static Direction4 InvertX( this Direction4 direction )
        {
            switch( direction )
            {
                case Direction4.Left:
                    return Direction4.Right;
                case Direction4.Right:
                    return Direction4.Left;
                default:
                    return direction;
            }
        }

        /// <summary>
        /// Inverts the given <see cref="Direction4"/> vertically (on the y-axis).
        /// </summary>
        /// <param name="direction">
        /// The <see cref="Direction4"/> to invert on the y-axis.
        /// </param>
        /// <returns>
        /// The inverted direction.
        /// </returns>
        public static Direction4 InvertY( this Direction4 direction )
        {
            switch( direction )
            {
                case Direction4.Up:
                    return Direction4.Down;
                case Direction4.Down:
                    return Direction4.Up;
                default:
                    return direction;
            }
        }

        #endregion

        #region - Invert8 -

        /// <summary>
        /// Inverts the given <see cref="Direction8"/>.
        /// </summary>
        /// <param name="direction"> 
        /// The <see cref="Direction8"/> to invert.
        /// </param>
        /// <returns>
        /// The inverted direction.
        /// </returns>
        public static Direction8 Invert( this Direction8 direction )
        {
            switch( direction )
            {
                case Direction8.Left:
                    return Direction8.Right;
                case Direction8.Right:
                    return Direction8.Left;
                case Direction8.Up:
                    return Direction8.Down;
                case Direction8.Down:
                    return Direction8.Up;
                case Direction8.LeftDown:
                    return Direction8.RightUp;
                case Direction8.LeftUp:
                    return Direction8.RightDown;
                case Direction8.RightDown:
                    return Direction8.LeftUp;
                case Direction8.RightUp:
                    return Direction8.LeftDown;
                default:
                    return Direction8.None;
            }
        }

        /// <summary>
        /// Inverts the given <see cref="Direction8"/> horizontally (on the x-axis).
        /// </summary>
        /// <param name="direction">
        /// The <see cref="Direction8"/> to invert on the x-axis.
        /// </param>
        /// <returns>
        /// The inverted direction.
        /// </returns>
        public static Direction8 InvertX( this Direction8 direction )
        {
            switch( direction )
            {
                case Direction8.Left:
                    return Direction8.Right;
                case Direction8.Right:
                    return Direction8.Left;

                case Direction8.LeftDown:
                    return Direction8.RightDown;
                case Direction8.LeftUp:
                    return Direction8.RightUp;
                case Direction8.RightDown:
                    return Direction8.LeftDown;
                case Direction8.RightUp:
                    return Direction8.LeftUp;

                default:
                    return direction;
            }
        }

        /// <summary>
        /// Inverts the given <see cref="Direction8"/> vertically (on the y-axis).
        /// </summary>
        /// <param name="direction">
        /// The <see cref="Direction8"/> to invert on the y-axis.
        /// </param>
        /// <returns>
        /// The inverted direction.
        /// </returns>
        public static Direction8 InvertY( this Direction8 direction )
        {
            switch( direction )
            {
                case Direction8.Up:
                    return Direction8.Down;
                case Direction8.Down:
                    return Direction8.Up;

                case Direction8.LeftDown:
                    return Direction8.LeftUp;
                case Direction8.LeftUp:
                    return Direction8.LeftDown;
                case Direction8.RightDown:
                    return Direction8.RightUp;
                case Direction8.RightUp:
                    return Direction8.RightDown;
                default:
                    return direction;
            }
        }

        #endregion

        #region - ToVector -

        /// <summary>
        /// Converts the given <see cref="Directions"/> enumerations
        /// into a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="directions">
        /// The input direction enumeration.
        /// </param>
        /// <returns>
        /// The direction vector.
        /// </returns>
        public static Vector2 ToVector( this Directions directions )
        {
            Vector2 vector = new Vector2();

            if( (directions & Directions.Left) != 0 )
                vector.X -= 1.0f;
            if( (directions & Directions.Right) != 0 )
                vector.X += 1.0f;

            if( (directions & Directions.Up) != 0 )
                vector.Y -= 1.0f;
            if( (directions & Directions.Down) != 0 )
                vector.Y += 1.0f;

            return vector;
        }

        /// <summary>
        /// Converts the given <see cref="Direction4"/> enumerations
        /// into a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="direction">
        /// The input direction enumeration.
        /// </param>
        /// <returns>
        /// The direction vector.
        /// </returns>
        public static Vector2 ToVector( this Direction4 direction )
        {
            switch( direction )
            {
                case Direction4.Left:
                    return new Vector2( -1.0f, 0.0f );

                case Direction4.Right:
                    return new Vector2( 1.0f, 0.0f );

                case Direction4.Up:
                    return new Vector2( 0.0f, -1.0f );

                case Direction4.Down:
                    return new Vector2( 0.0f, 1.0f );

                default:
                case Direction4.None:
                    return new Vector2( 0.0f, 0.0f );
            }
        }

        #endregion

        #region - ToDirection -

        /// <summary>
        /// Extracts the <see cref="Direction4"/> from the given <see cref="Vector2"/>.
        /// </summary>
        /// <remarks>
        /// Positive Y is considered Down.
        /// </remarks>
        /// <param name="vector">Vector2 to extract the direction from.</param>
        /// <returns> The extracted <see cref="Direction4"/>. </returns>
        public static Direction4 ToDirection4( this Vector2 vector )
        {
            // First extract the individual axes:
            Direction4 vert = vector.Y > 0.0f ? Direction4.Down : (vector.Y < 0.0f ? Direction4.Up   : Direction4.None);
            Direction4 hori = vector.X > 0.0f ? Direction4.Right : (vector.X < 0.0f ? Direction4.Left : Direction4.None);

            // If one axis is 0.0f then we already know it:
            if( vert == Direction4.None )
            {
                return hori;
            }
            else if( hori == Direction4.None )
            {
                return vert;
            }
            else
            {
                // Both axes are present, return the one that 'weights' more:
                if( System.Math.Abs( vector.Y ) > System.Math.Abs( vector.X ) )
                {
                    return vert;
                }
                else
                {
                    return hori;
                }
            }
        }

        /// <summary>
        /// Extracts the <see cref="Direction8"/> from the given <see cref="Vector2"/>.
        /// </summary>
        /// <remarks>
        /// Positive Y is considered Down.
        /// </remarks>
        /// <param name="vector">Vector2 to extract the direction from.</param>
        /// <returns> The extracted <see cref="Direction8"/>. </returns>
        public static Direction8 ToDirection8( this Vector2 vector )
        {
            // First extract the individual axes:
            Direction8 vert = vector.Y > 0.0f ? Direction8.Down : (vector.Y < 0.0f ? Direction8.Up : Direction8.None);
            Direction8 hori = vector.X > 0.0f ? Direction8.Right : (vector.X < 0.0f ? Direction8.Left : Direction8.None);

            // If one axis is 0.0f then we already know it:
            if( vert == Direction8.None )
            {
                return hori;
            }
            else if( hori == Direction8.None )
            {
                return vert;
            }
            else
            {
                // Combine them now:
                switch( hori )
                {
                    case Direction8.Left:
                        switch( vert )
                        {
                            case Direction8.Up:
                                return Direction8.LeftUp;
                            case Direction8.Down:
                                return Direction8.LeftDown;
                            default:
                                break;
                        }
                        break;

                    case Direction8.Right:
                        switch( vert )
                        {
                            case Direction8.Up:
                                return Direction8.RightUp;
                            case Direction8.Down:
                                return Direction8.RightDown;
                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                // should never happen:
                return Direction8.None;
            }
        }

        #endregion

        #region - GetNext -

        /// <summary>
        /// Gets the Direction4 following this Direction4 following the given <see cref="TurnDirection"/>.
        /// </summary>
        /// <param name="direction">
        /// The starting Direction4.
        /// </param>
        /// <param name="turnDirection">
        /// The direction to turn.
        /// </param>
        /// <returns>
        /// The next Direction4; following this Direction4.
        /// </returns>
        public static Direction4 GetNext( this Direction4 direction, TurnDirection turnDirection )
        {
            switch( turnDirection )
            {
                default:
                case TurnDirection.None:
                    return direction;

                case TurnDirection.Clockwise:
                    return GetNextClockwise( direction );

                case TurnDirection.AntiClockwise:
                    return GetNextAntiClockwise( direction );
            }
        }

        /// <summary>
        /// Gets the Direction4 following this Direction4 clockwise.
        /// </summary>
        /// <param name="direction">
        /// The starting Direction4.
        /// </param>
        /// <returns>
        /// The next Direction4; following this Direction4.
        /// </returns>
        private static Direction4 GetNextClockwise( Direction4 direction )
        {
            switch( direction )
            {
                default:
                case Direction4.None:
                    return Direction4.None;

                case Direction4.Left:
                    return Direction4.Up;

                case Direction4.Down:
                    return Direction4.Left;

                case Direction4.Up:
                    return Direction4.Right;

                case Direction4.Right:
                    return Direction4.Down;
            }
        }

        /// <summary>
        /// Gets the Direction4 following this Direction4 anti-clockwise.
        /// </summary>
        /// <param name="direction">
        /// The starting Direction4.
        /// </param>
        /// <returns>
        /// The next Direction4; following this Direction4.
        /// </returns>
        private static Direction4 GetNextAntiClockwise( Direction4 direction )
        {
            switch( direction )
            {
                default:
                case Direction4.None:
                    return Direction4.None;

                case Direction4.Left:
                    return Direction4.Down;

                case Direction4.Down:
                    return Direction4.Right;

                case Direction4.Up:
                    return Direction4.Left;

                case Direction4.Right:
                    return Direction4.Up;
            }
        }

        #endregion
    }
}
