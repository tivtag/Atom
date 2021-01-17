// <copyright file="Positonal2VertexData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Data.Positonal2VertexData class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph.Data
{
    using System;

    /// <summary>
    /// Defines Vertex data which represents a position in 2d space.
    /// </summary>
    public class Positonal2VertexData : IPositionable2, ICultureSensitiveToStringProvider
    {
        /// <summary>
        /// Gets or sets the position stored within this <see cref="Positonal2VertexData"/>.
        /// </summary>
        /// <value>The position stored within this <see cref="Positonal2VertexData"/>.</value>
        public Vector2 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Positonal2VertexData"/> class.
        /// </summary>
        /// <param name="position">The position stored within the new <see cref="Positonal2VertexData"/>. </param>
        public Positonal2VertexData( Vector2 position )
        {
            this.Position = position;
        }

        /// <summary>
        /// Returns a string representation of this <see cref="Positonal2VertexData"/> instance.
        /// </summary>
        /// <returns>A string that represents the Positonal2VertexData.</returns>
        public override string ToString()
        {
            return this.ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a string representation of this <see cref="Positonal2VertexData"/> instance.
        /// </summary>
        /// <param name="formatProvider">
        /// Provides access to culture-sensitive formatting information.
        /// </param>
        /// <returns>A string that represents the Positonal2VertexData.</returns>
        public string ToString( IFormatProvider formatProvider )
        {
            return this.Position.ToString( formatProvider );
        }
    }
}
