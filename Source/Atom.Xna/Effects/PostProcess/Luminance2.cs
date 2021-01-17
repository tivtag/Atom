// <copyright file="Luminance2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.PostProcess.Luminance2 class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;

//namespace Atom.Xna.Effects.PostProcess
//{
//    public class Luminance2: PostProcessEffect
//    {
//        #region [ Initialization ]

//        /// <summary>
//        /// Initializes a new instance of the Luminance class.
//        /// </summary>
//        /// <param name="effectLoader">
//        /// Provides a mechanism that allows loading of effect assets.
//        /// </param>
//        /// <param name="deviceService">
//        /// Provides access to the <see cref="GraphicsDevice"/>.
//        /// </param>
//        public Luminance2( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
//            : base( effectLoader, deviceService )
//        {
//        }

//        /// <summary>
//        /// Loads the effect used by this Luminance.
//        /// </summary>
//        /// <param name="effectLoader">
//        /// Provides a mechanism that allows loading of effect assets.
//        /// </param>
//        protected override void LoadEffect( IEffectLoader effectLoader )
//        {
//        }

//        /// <summary>
//        /// Creates the luminance chain.
//        /// </summary>
//        /// <param name="width">
//        /// The width of the source texture.
//        /// </param>
//        /// <param name="height">
//        /// The height of the source texture.
//        /// </param>
//        private void CreateLuminanceChain( int width, int height )
//        {
//            this.fullLuminanceTarget = new RenderTarget2D(
//                this.GraphicsDevice,
//                width,
//                height,
//                1,
//                SurfaceFormat.HalfVector2
//            );

//            this.chainWidth = width;
//            this.chainHeight = height;
//        }

//        /// <summary>
//        /// Ensures that the luminane chain has been created and fits the size of the given
//        /// texture.
//        /// </summary>
//        /// <param name="sourceTexture">
//        /// The texture that contains the previously rendered content.
//        /// </param>
//        private void EnsureLuminanceChain( Texture2D sourceTexture )
//        {
//            int sourceWidth = sourceTexture.Width;
//            int sourceHeight = sourceTexture.Height;

//            if( sourceWidth != this.chainWidth || 
//                sourceHeight != this.chainHeight )
//            {
//                this.CreateLuminanceChain( sourceWidth, sourceHeight );
//            }
//        }

//        public override void PostProcess( Texture2D sourceTexture, RenderTarget2D result )
//        {
//            this.EnsureLuminanceChain( sourceTexture );
//        }

//        public override void Dispose()
//        {
//        }

//        #endregion

//        private RenderTarget2D fullLuminanceTarget;
//        private Effect effect;

//        private int chainWidth = -1;
//        private int chainHeight = -1;
//    }
//}
