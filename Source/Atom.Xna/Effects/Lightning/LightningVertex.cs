// <copyright file="LightningVertex.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.Lightning.LightningVertex class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Effects.Lightning
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents a vertex within a LightningBolt.
    /// </summary>
    internal struct LightningVertex : IVertexType
    {
        /// <summary>
        /// The position of the vertex.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The texture coordinates of the vertex.
        /// </summary>
        public Vector2 TextureCoordinates;
        
        /// <summary>
        /// The color gradient of the vertex.
        /// </summary>
        public Vector2 ColorGradient;
                
        public VertexDeclaration VertexDeclaration
        {
            get 
            {
                return vertexDeclaration;
            }
        }

        /// <summary>
        /// The layout of this vertex structure.
        /// </summary>
        public static readonly VertexElement[] VertexElements = 
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(20, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1),
        };
        
        private static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration( LightningVertex.VertexElements );

        /// <summary>
        /// The size of this vertex structure.
        /// </summary>
        public const int SizeInBytes = 28;
    }
}
