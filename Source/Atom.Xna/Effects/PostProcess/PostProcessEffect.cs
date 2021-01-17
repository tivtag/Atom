// <copyright file="PostProcessEffect.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.PostProcess.PostProcessEffect class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Effects.PostProcess
{
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents the base-class of all post-process effects.
    /// </summary>
    public abstract class PostProcessEffect : RectangularEffect, IPostProcessEffect
    {    
        /// <summary>
        /// Initializes a new instance of the PostProcessEffect class.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        /// <param name="deviceService">
        /// Provides access to the <see cref="GraphicsDevice"/>.
        /// </param>
        protected PostProcessEffect( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
            : base( effectLoader, deviceService )
        {
        }

        /// <summary>
        /// Applies this PostProcessEffect.
        /// </summary>
        /// <param name="sourceTexture">
        /// The texture to post-process.
        /// </param>
        /// <param name="result">
        /// The RenderTarget to which to render the result of this PostProcessEffect.
        /// </param>
        /// <param name="drawContext">
        /// The context under which the drawing operation occurrs.
        /// </param>
        public abstract void PostProcess( Texture2D sourceTexture, RenderTarget2D result, IXnaDrawContext drawContext );

        /// <summary>
        /// Gets the size of the given RenderTarget.
        /// </summary>
        /// <param name="target">
        /// The targeted RenderTarget. Allowed to be null.
        /// </param>
        /// <returns>
        /// The size the target of the post processing effect willcover.
        /// </returns>
        protected Vector2 GetTargetSize( RenderTarget2D target )
        {
            if( target == null )
            {                
                var presenationParameters = this.GraphicsDevice.PresentationParameters;
                return new Vector2( presenationParameters.BackBufferWidth, presenationParameters.BackBufferHeight );
            }
            else
            {
                return new Vector2( target.Width, target.Height );
            }
        }
    }
}
