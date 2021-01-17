// <copyright file="ISpriteDrawContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ISpriteDrawContext interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using Atom.Xna.Batches;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents an <see cref="IDrawContext"/> that allows the drawing
    /// of sprites.
    /// </summary>
    public interface ISpriteDrawContext : IXnaDrawContext
    {
        /// <summary>
        /// Gets the <see cref="IComposedSpriteBatch"/> that should be used to draw
        /// sprites and various shapes when using this ISpriteDrawContext.
        /// </summary>
        IComposedSpriteBatch Batch
        {
            get;
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites.
        /// </summary>
        void Begin();

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending options.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        void Begin( BlendState blendState );

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending,
        /// sorting, and render state options.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        /// <param name="samplerState">Sampler options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        void Begin( BlendState blendState, SamplerState samplerState, SpriteSortMode sortMode );

        /// <summary> 
        /// Prepares the graphics device for drawing sprites with specified blending,
        /// sorting, and render state options, and a global transform matrix.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        /// <param name="samplerState">Sampler options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="transform">
        /// A matrix to apply to position, rotation, scale, and depth data passed to Spritebatch.Draw.
        /// </param>
        void Begin(
            BlendState blendState,
            SamplerState samplerState,
            SpriteSortMode sortMode,
            Atom.Math.Matrix4 transform
        );

        /// <summary> 
        /// Prepares the graphics device for drawing sprites with specified blending,
        /// sorting, and render state options, and a global transform matrix.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        /// <param name="samplerState">Sampler options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="transform">
        /// A matrix to apply to position, rotation, scale, and depth data passed to Spritebatch.Draw.
        /// </param>
        void Begin(
            BlendState blendState,
            SamplerState samplerState,
            SpriteSortMode sortMode,
            Microsoft.Xna.Framework.Matrix transform
        );

        /// <summary>
        /// Flushes the sprite batch and restores the device state to how it was before
        /// Spritebatch.Begin was called.
        /// </summary>
        void End();
    }
}
