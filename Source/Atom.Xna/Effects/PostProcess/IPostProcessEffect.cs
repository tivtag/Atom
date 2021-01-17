// <copyright file="IPostProcessEffect.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.PostProcess.IPostProcessEffect interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Effects.PostProcess
{
    using System;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents an effect that is applied to an already
    /// rendered scene.
    /// </summary>
    public interface IPostProcessEffect : IContentLoadable, IDisposable
    {
        /// <summary>
        /// Applies this IPostProcessEffect.
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
        void PostProcess( Texture2D sourceTexture, RenderTarget2D result, IXnaDrawContext drawContext );
    }
}
