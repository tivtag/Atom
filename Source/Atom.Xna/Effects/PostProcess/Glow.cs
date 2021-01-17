// <copyright file="Glow.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.PostProcess.Glow class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Effects.PostProcess
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents a post-process effect that makes the source texture 'glow'.
    /// </summary>
    public class Glow : PostProcessEffect
    {
        /// <summary>
        /// Gets or sets the strength of the flow effect.
        /// </summary>
        public float Strength 
        {
            get
            {
                return this.glowEffect.Parameters["GlowStrength"].GetValueSingle();
            }

            set 
            {
                this.glowEffect.Parameters["GlowStrength"].SetValue( value );
            }
        }
        
        /// <summary>
        /// Sets the size of a single texel.
        /// </summary>
        private Xna.Vector2 TexelSize
        {
            set
            {
                this.glowEffect.Parameters["TexelSize"].SetValue( value );
            }
        }

        /// <summary>
        /// Sets the glow texture that gets combined with the source texture.
        /// </summary>
        private Texture2D GlowMap
        {
            set
            {
                this.glowEffect.Parameters["GlowMap"].SetValue( value );
            }
        }
               
        /// <summary>
        /// Initializes a new instance of the Glow class.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        /// <param name="renderTargetFactory">
        /// Provides a mechanism for creating new screen-sized RenderTarget2Ds.
        /// </param>
        /// <param name="deviceService">
        /// Provides access to the <see cref="GraphicsDevice"/>.
        /// </param>
        public Glow( IEffectLoader effectLoader, IRenderTarget2DFactory renderTargetFactory, IGraphicsDeviceService deviceService )
            : base( effectLoader, deviceService )
        {
            Contract.Requires<ArgumentNullException>( renderTargetFactory != null );

            this.renderTargetFactory = renderTargetFactory;
        }

        /// <summary>
        /// Loads the effect used by this RectangularEffect.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        protected override void LoadEffect( IEffectLoader effectLoader )
        {
            this.glowEffect = effectLoader.Load( "Glow" );

            this.copyTechnique = this.glowEffect.Techniques["Copy"];
            this.blurHorizontallyTechnique = this.glowEffect.Techniques["BlurHorizontal"];
            this.blurVerticallyTechnique = this.glowEffect.Techniques["BlurVertical"];
            this.combineTechnique = this.glowEffect.Techniques["Combine"];
        }

        /// <summary>
        /// Provides a hook that can be overwritten by subclasses to load additional content.
        /// </summary>
        protected override void LoadCustomContent()
        {
            this.combineRT = this.renderTargetFactory.Create();
            this.temporaryRT = this.renderTargetFactory.Create( sizeDivider: new Point2( 4, 4 ) );
            this.downsampleRT =  this.renderTargetFactory.Create( sizeDivider: new Point2( 4, 4 ) );

            this.spriteBatch = new SpriteBatch( this.GraphicsDevice );
        }

        /// <summary>
        /// Releases all managed resources.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            this.blurHorizontallyTechnique = null;
            this.blurVerticallyTechnique = null;
            this.combineTechnique = null;
            this.copyTechnique = null;
            this.glowEffect = null;

            this.spriteBatch = null;
            this.temporaryRT = null;
            this.downsampleRT =  null;
            this.combineRT = null;
        }

        /// <summary>
        /// Releases all unmanaged resources.
        /// </summary>
        protected override void DisposeUnmanagedResources()
        {
            if( this.glowEffect != null )
            {
                this.glowEffect.Dispose();
            }

            if( this.spriteBatch != null )
            {
                this.spriteBatch.Dispose();
            }

            if( this.temporaryRT != null )
            {
                this.temporaryRT.Dispose();
            }

            if( this.downsampleRT != null )
            {
                this.downsampleRT.Dispose();
            }

            if( this.combineRT != null )
            {
                this.combineRT.Dispose();
            }
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
        public override void PostProcess( Texture2D sourceTexture, RenderTarget2D result, IXnaDrawContext drawContext )
        {
            var oldRenderTarget = this.GraphicsDevice.GetRenderTarget2D();

            // Downsample by rendering to a smaller RendetTarget:
            this.ApplyTechnique( this.copyTechnique, sourceTexture, this.downsampleRT );
            this.ApplyTechnique( this.blurHorizontallyTechnique, this.downsampleRT, this.temporaryRT );
            this.ApplyTechnique( this.blurVerticallyTechnique, this.temporaryRT, this.downsampleRT );

            // Combine source with glow map and render into result:
            this.GlowMap = this.downsampleRT;
            this.ApplyTechnique( this.combineTechnique, sourceTexture, this.combineRT );
            this.ApplyTechnique( this.copyTechnique, this.combineRT, result );

            this.GraphicsDevice.SetRenderTarget( oldRenderTarget );
        }
        
        /// <summary>
        /// Applies the specified EffectTechnique to the specified source texture;
        /// rendering the result into the specified target RenderTarget2D.
        /// </summary>
        /// <param name="technique">
        /// The technique to apply.
        /// </param>
        /// <param name="source">
        /// The source texture.
        /// </param>
        /// <param name="target">
        /// The RenderTarget to render into.
        /// </param>
        private void ApplyTechnique( EffectTechnique technique, Texture2D source, RenderTarget2D target )
        {
            var device = this.GraphicsDevice;
            device.SetRenderTarget( target );
            device.Clear( Xna.Color.Transparent );

            this.glowEffect.CurrentTechnique = technique;
            this.TexelSize = new Xna.Vector2( 1.0f / target.Width, 1.0f / target.Height );

            this.spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.Additive );
            {
                var pass = this.glowEffect.CurrentTechnique.Passes[0];
                pass.Apply();
                {
                    this.spriteBatch.Draw( source, new Xna.Rectangle( 0, 0, target.Width, target.Height ), Xna.Color.White );
                }
            }
            this.spriteBatch.End();

            // Resolve
            device.SetRenderTarget( null );
        }

        private Effect glowEffect;
        private EffectTechnique copyTechnique;
        private EffectTechnique blurHorizontallyTechnique;
        private EffectTechnique blurVerticallyTechnique;
        private EffectTechnique combineTechnique;

        private SpriteBatch spriteBatch;
        private RenderTarget2D downsampleRT;
        private RenderTarget2D temporaryRT;
        private RenderTarget2D combineRT;

        /// <summary>
        /// Provides a mechanism for creating new screen-sized RenderTarget2Ds.
        /// </summary>
        private readonly IRenderTarget2DFactory renderTargetFactory;
    }
}
