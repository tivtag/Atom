// <copyright file="BrightPass.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Effects.PostProcess.BrightPass class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Effects.PostProcess
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The Bright Pass ... .
    /// </summary>
    public sealed class BrightPass : PostProcessEffect
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the texture from the Luminance pass.
        /// </summary>
        public Texture2D LuminanceTexture
        {
            get
            {
                return this.parameterLuminanceTexture.GetValueTexture2D();
            }

            set
            {
                this.parameterLuminanceTexture.SetValue( value );
            }
        }

        /// <summary>
        /// Gets or sets the blending exposure applied when rendering.
        /// </summary>
        public float Exposure
        {
            get
            {
                return this.effect.Parameters["Exposure"].GetValueSingle();
            }

            set
            {
                this.effect.Parameters["Exposure"].SetValue( value );
            }
        }

        /// <summary>
        /// Gets or sets the threshold brightness at which pixels are brightened.
        /// </summary>
        public float Threshold
        {
            get
            {
                return this.effect.Parameters["Threshold"].GetValueSingle();
            }

            set
            {
                this.effect.Parameters["Threshold"].SetValue( value );
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the BrightPass class.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        /// <param name="deviceService">
        /// Provides access to the <see cref="GraphicsDevice"/>.
        /// </param>
        public BrightPass( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
            : base( effectLoader, deviceService )
        {
        }

        /// <summary>
        /// Loads the effect used by this RectangularEffect.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        protected override void LoadEffect( IEffectLoader effectLoader )
        {
            this.effect = effectLoader.Load( "BrightPass" );
            this.effectPass = this.effect.CurrentTechnique.Passes[0];

            this.parameterTargetSize = this.effect.Parameters["TargetSize"];
            this.parameterSourceSize = this.effect.Parameters["SourceSize"];
            this.parameterSourceTexture = this.effect.Parameters["SourceTexture"];
            this.parameterLuminanceTexture = this.effect.Parameters["LuminanceTexture"];
        }

        #endregion

        #region [ Methods ]

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
            this.SetSourceTexture( sourceTexture );

            var targetSize = this.GetTargetSize( result );
            this.parameterTargetSize.SetValue( targetSize.ToXna() );

            this.GraphicsDevice.SetRenderTarget( result );
            this.GraphicsDevice.Clear( Color.Black );

            this.Draw( this.effect, this.effectPass );

            this.GraphicsDevice.SetRenderTarget( null );
        }

        /// <summary>
        /// Sets the texture that contains the result of 
        /// the previous rendering operation.
        /// </summary>
        /// <param name="value">
        /// The texture that contains the previously rendered content.
        /// </param>
        private void SetSourceTexture( Texture2D value )
        {
            this.parameterSourceTexture.SetValue( value );

            var sourceSize = new Microsoft.Xna.Framework.Vector2();

            if( value == null )
            {
                sourceSize.X = 1;
                sourceSize.Y = 1;
            }
            else
            {
                sourceSize.X = value.Width;
                sourceSize.Y = value.Height;
            }

            this.parameterSourceSize.SetValue( sourceSize );
        }

        /// <summary>
        /// Disposes the managed resources used by this BrightPass effect.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            this.parameterLuminanceTexture = null;
            this.parameterSourceSize = null;
            this.parameterSourceTexture = null;
            this.parameterTargetSize = null;
            this.effectPass = null;
            this.effect = null;
        }

        /// <summary>
        /// Disposes the unmanaged resources used by this BrightPass effect.
        /// </summary>
        protected override void DisposeUnmanagedResources()
        {
            if( this.effect != null )
            {
                this.effect.Dispose();
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The effect that contain the actual brighten logic.
        /// </summary>
        private Effect effect;

        /// <summary>
        /// The cached first pass of the effect.
        /// </summary>
        private EffectPass effectPass;

        /// <summary>
        /// The parameter used to set the size of the target texture.
        /// </summary>
        private EffectParameter parameterTargetSize;

        /// <summary>
        /// The parameter used to set the size of the source texture.
        /// </summary>
        private EffectParameter parameterSourceSize;

        /// <summary>
        /// The parameter used to set the source input texture.
        /// </summary>
        private EffectParameter parameterSourceTexture;

        /// <summary>
        /// The parameter used to set the luminance input texture.
        /// </summary>
        private EffectParameter parameterLuminanceTexture;

        #endregion
    }
}
