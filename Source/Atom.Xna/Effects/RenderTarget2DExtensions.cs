// <copyright file="RenderTarget2DExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Effects.RenderTarget2DExtensions class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Effects
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.Xna.Effects.PostProcess;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines extension methods that extend the RenderTarget2D class
    /// with methods related to effects.
    /// </summary>
    public static class RenderTarget2DExtensions
    {
        /// <summary>
        /// Applies the specified <see cref="IPostProcessEffect"/> to this <see cref="RenderTarget2D"/>.
        /// </summary>
        /// <param name="target">
        /// The target of the effect.
        /// </param>
        /// <param name="effect">
        /// The effect to apply.
        /// </param>
        /// <param name="drawContext">
        /// The context under which the drawing operation occurrs.
        /// </param>
        public static void ApplyEffectSlow( this RenderTarget2D target, IPostProcessEffect effect, IXnaDrawContext drawContext )
        {
            Contract.Requires<ArgumentNullException>( target != null );
            Contract.Requires<ArgumentNullException>( effect != null );

            var texture = new Texture2D( target.GraphicsDevice, target.Width, target.Height, false, target.Format );

            byte[] data = new byte[target.Width * target.Height * 4]; 
            target.GetData( data );
            texture.SetData( data );
    
            effect.PostProcess( texture, target, drawContext );
        }
    }
}
