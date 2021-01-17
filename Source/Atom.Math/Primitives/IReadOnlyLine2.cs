// <copyright file="IReadOnlyLine2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.IReadOnlyLine2 interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math
{
    using System;

    /// <summary> 
    /// Provides read-only access to line object in 2D space, descriped using the general line equation: Ax + By - c = 0.
    /// </summary>
    public interface IReadOnlyLine2 : ICloneable, ICultureSensitiveToStringProvider
    {
        /// <summary> 
        /// Gets the A parameter of the line.
        /// </summary>
        /// <value>The A parameter as used in the 'Ax + By - c = 0' equation.</value>
        float A { get; }

        /// <summary> 
        /// Gets the B parameter of the line.
        /// </summary>
        /// <value>The B parameter as used in the 'Ax + By - c = 0' equation.</value>
        float B { get; }

        /// <summary> 
        /// Gets the C parameter of the line.
        /// </summary>
        /// <value>The C parameter as used in the 'Ax + By - c = 0' equation.</value>
        float C { get; }

        /// <summary> 
        /// Gets the angle of the line in radians.
        /// </summary>
        /// <value>
        /// The angle in radians.
        /// </value>
        float Angle { get; }

        /// <summary> 
        /// Gets a value indicating whether the line is a vertical line.
        /// </summary>
        /// <value> 
        /// Is <see langword="true"/> if the line is vertical; otherwise <see langword="false"/>. 
        /// </value>
        bool IsVertical { get; }

        /// <summary> 
        /// Gets a value indicating whether the line is a horizontal line. 
        /// </summary>
        /// <value> 
        /// Is <see langword="true"/> if the line is horizontal; otherwise <see langword="false"/>. 
        /// </value>
        bool IsHorizontal { get; }

        /// <summary> 
        /// Returns the distance from a given point to the line. 
        /// </summary>
        /// <param name="point">
        /// The point to get the distance to.
        /// </param>
        /// <returns>
        /// The distance.
        /// </returns>
        float Distance( Vector2 point );

        /// <summary>
        /// Calculates X given <paramref name="y"/>.
        /// </summary>
        /// <exception cref="DivideByZeroException">
        /// Thrown if <see cref="A"/> is 0.
        /// </exception>
        /// <param name="y">
        /// The y coordinate.
        /// </param>
        /// <returns>
        /// The x coordinate.
        /// </returns>
        float GetX( float y );

        /// <summary>
        /// Calculates Y given <paramref name="x"/>.
        /// </summary>
        /// <exception cref="DivideByZeroException">
        /// Thrown if <see cref="B"/> is 0.
        /// </exception>
        /// <param name="x">
        /// The x coordinate.
        /// </param>
        /// <returns>
        /// The y coordinate.
        /// </returns>
        float GetY( float x );
    }
}
