// <copyright file="SpriteDrawContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteDrawContext class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Xna.Batches;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents an <see cref="IDrawContext"/> that allows the drawing
    /// of sprites.
    /// </summary>
    public class SpriteDrawContext : ISpriteDrawContext
    {
        /// <summary>
        /// Gets or sets the <see cref="IComposedSpriteBatch"/> that should be used to draw
        /// sprites when using this ISpriteDrawContext.
        /// </summary>
        public IComposedSpriteBatch Batch
        {
            get
            {
                return this.batch;
            }

            set
            {
                this.batch = value;
            }
        }
        
        /// <summary>
        /// Gets or sets a snapshot of the time it took to execute the last frame.
        /// </summary>
        public GameTime GameTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Xna GraphicsDevice object.
        /// </summary>
        public GraphicsDevice Device
        {
            get
            {
                return this.device;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteDrawContext"/> class.
        /// </summary>
        /// <param name="device">
        /// The Xna GraphicsDevice object.
        /// </param>
        public SpriteDrawContext( GraphicsDevice device )
        {
            Contract.Requires<ArgumentNullException>( device != null );

            this.device = device;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteDrawContext"/> class.
        /// </summary>
        /// <param name="batch">
        /// The <see cref="IComposedSpriteBatch"/> to use.
        /// </param>
        /// <param name="device">
        /// The Xna GraphicsDevice object.
        /// </param>
        public SpriteDrawContext( IComposedSpriteBatch batch, GraphicsDevice device )
            : this( device )
        {
            this.batch = batch;
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites.
        /// </summary>
        public void Begin()
        {                 
            this.batch.Initialize( BlendState.NonPremultiplied, SamplerState.PointClamp, SpriteSortMode.Deferred, Microsoft.Xna.Framework.Matrix.Identity );
            this.batch.Begin( this );
        }

        /// <summary>
        /// Prepares the draw context for drawing sprites with specified blending options.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        public void Begin( BlendState blendState )
        {
            this.batch.Initialize( blendState, null, SpriteSortMode.Deferred, Microsoft.Xna.Framework.Matrix.Identity );
            this.batch.Begin( this );
        }

        /// <summary>
        /// Prepares the draw context for drawing sprites with specified blending,
        /// sorting, and render state options.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        /// <param name="samplerState">Sampler options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        public void Begin( BlendState blendState, SamplerState samplerState, SpriteSortMode sortMode )
        {
            this.batch.Initialize( blendState, samplerState, sortMode, Microsoft.Xna.Framework.Matrix.Identity );
            this.batch.Begin( this );
        }

        /// <summary> 
        /// Prepares the draw context for drawing sprites with specified blending,
        /// sorting, and render state options, and a global transform matrix.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        /// <param name="samplerState">Sampler options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="transform">
        /// A matrix to apply to position, rotation, scale, and depth data passed to Spritebatch.Draw.
        /// </param>
        public void Begin( 
            BlendState blendState, 
            SamplerState samplerState,
            SpriteSortMode sortMode,
            Atom.Math.Matrix4 transform )
        {
            this.batch.Initialize( blendState, samplerState, sortMode, transform );
            this.batch.Begin( this );
        }

        /// <summary> 
        /// Prepares the draw context for drawing sprites with specified blending,
        /// sorting, and render state options, and a global transform matrix.
        /// </summary>
        /// <param name="blendState">Blending options to use when rendering.</param>
        /// <param name="samplerState">Sampler options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="transform">
        /// A matrix to apply to position, rotation, scale, and depth data passed to Spritebatch.Draw.
        /// </param>
        public void Begin(
            BlendState blendState,
            SamplerState samplerState,
            SpriteSortMode sortMode,
            Microsoft.Xna.Framework.Matrix transform )
        {
            this.batch.Initialize( blendState, samplerState, sortMode, transform );
            this.batch.Begin( this );
        }

        /// <summary>
        /// Flushes the sprite batch and restores the device state to how it was before
        /// Spritebatch.Begin was called.
        /// </summary>
        public void End()
        {
            this.batch.End();
        }

        /// <summary>
        /// Represents the storage field of the <see cref="Batch"/> property.
        /// </summary>
        private IComposedSpriteBatch batch;

        /// <summary>
        /// The Xna GraphicsDevice object.
        /// </summary>
        private readonly GraphicsDevice device;
    }
}
