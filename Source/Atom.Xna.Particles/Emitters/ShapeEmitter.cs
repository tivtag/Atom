// <copyright file="ShapeEmitter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Particles.Emitters.ShapeEmitter class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Particles.Emitters
{
    using System;
    using System.Collections.Generic;
    using Atom.Math;

    /// <summary>
    /// Defines an <see cref="Emitter"/> that releases <see cref="Particle"/>s
    /// from a random position on the edge of a shape.
    /// </summary>
    public class ShapeEmitter : Emitter
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the list of points that define the shape.
        /// A valid shape must have at least two control points.
        /// </summary>
        /// <value>The list of points that define the shape.</value>
        public IList<Vector2> Points
        {
            get { return this.points; }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeEmitter"/> class.
        /// </summary>
        /// <param name="points">
        /// A collection of points which make up the two dimensional shape.
        /// A valid shape must have atleast two control points.
        /// </param>
        public ShapeEmitter( IEnumerable<Vector2> points )
        {
            this.points = new List<Vector2>( points );

            if( this.points.Count < 2 )
                throw new ArgumentException( "A valid shape needs at least two control points.", "points" );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Generates an offset vector for a Particle as it is released.
        /// </summary>
        /// <param name="totalSeconds">
        /// The total game time in whole and fractional seconds.
        /// </param>
        /// <param name="triggerPosition">
        /// The position a twhich the Emitter has been triggered.
        /// </param>
        /// <param name="offset">
        /// The value to populate with the generated offset.
        /// </param>
        protected override void GenerateParticleOffset( 
            float totalSeconds,
            ref Microsoft.Xna.Framework.Vector2 triggerPosition, 
            out Microsoft.Xna.Framework.Vector2 offset )
        {
            int index = this.Rand.RandomRange( 0, this.points.Count - 1 );
            Vector2 a = this.points[index];
            Vector2 b = this.points[(index + 1) % this.points.Count];

            float factor = this.Rand.RandomSingle;
            offset.X = MathUtilities.Lerp( a.X, b.X, factor );
            offset.Y = MathUtilities.Lerp( a.Y, b.Y, factor );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The points that define the shape.
        /// </summary>
        private readonly List<Vector2> points;

        #endregion
    }
}
