//// <copyright file="ToneMapping.cs" company="federrot Software">
////     Copyright (c) federrot Software. All rights reserved.
//// </copyright>
//// <summary>
////     Defines the Atom.Xna.Effects.PostProcess.ToneMapping class.
//// </summary>
//// <author>Paul Ennemoser (Tick)</author>

//namespace Atom.Xna.Effects.PostProcess
//{
//    using Microsoft.Xna.Framework;
//    using Microsoft.Xna.Framework.Graphics;

//    /// <summary>
//    /// Maps colors in the High Range color spectrum onto normal - drawable - colors.
//    /// </summary>
//    public sealed class ToneMapping : PostProcessEffect
//    {
//        #region [ Properties ]

//        /// <summary>
//        /// Gets or sets the texture from the Luminance pass.
//        /// </summary>
//        public Texture2D LuminanceTexture
//        {
//            get
//            {
//                return parameterLuminanceTexture.GetValueTexture2D();
//            }

//            set
//            {
//                parameterLuminanceTexture.SetValue( value );
//            }
//        }

//        /// <summary>
//        /// Gets or sets the blending exposure applied when rendering.
//        /// </summary>
//        public float Exposure
//        {
//            get
//            {
//                return this.effect.Parameters["Exposure"].GetValueSingle();
//            }

//            set
//            {
//                this.effect.Parameters["Exposure"].SetValue( value );
//            }
//        }

//        #endregion

//        #region [ Initialization ]

//        /// <summary>
//        /// Initializes a new instance of the ToneMapping class.
//        /// </summary>
//        /// <param name="effectLoader">
//        /// Provides a mechanism that allows loading of effect assets.
//        /// </param>
//        /// <param name="deviceService">
//        /// Provides access to the <see cref="GraphicsDevice"/>.
//        /// </param>
//        public ToneMapping( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
//            : base( effectLoader, deviceService )
//        {
//        }

//        /// <summary>
//        /// Loads the effect used by this RectangularEffect.
//        /// </summary>
//        /// <param name="effectLoader">
//        /// Provides a mechanism that allows loading of effect assets.
//        /// </param>
//        protected override void LoadEffect( IEffectLoader effectLoader )
//        {
//            this.effect = effectLoader.Load( "ToneMapping" );
//            this.effectPass = this.effect.CurrentTechnique.Passes[0];

//            this.parameterSourceTexture = this.effect.Parameters["SourceTexture"];
//            this.parameterLuminanceTexture = this.effect.Parameters["LuminanceTexture"];
//        }

//        #endregion

//        #region [ Methods ]
        
//        /// <summary>
//        /// Sets the texture that contains the result of 
//        /// the previous rendering operation.
//        /// </summary>
//        /// <param name="value">
//        /// The texture that contains the previously rendered content.
//        /// </param>
//        private void SetSourceTexture( Texture2D value )
//        {
//            this.parameterSourceTexture.SetValue( value );
//        }

//        /// <summary>
//        /// Applies this ToneMapping PostProcessEffect.
//        /// </summary>
//        /// <param name="sourceTexture">
//        /// The texture to post-process.
//        /// </param>
//        /// <param name="result">
//        /// The RenderTarget to which to render the result of this PostProcessEffect.
//        /// </param>
//        /// <param name="drawContext">
//        /// The context under which the drawing operation occurrs.
//        /// </param>
//        public override void PostProcess( Texture2D sourceTexture, RenderTarget2D result, IXnaDrawContext drawContext )
//        {
//            this.SetSourceTexture( sourceTexture );

//            this.GraphicsDevice.SetRenderTarget( result );
//            this.GraphicsDevice.Clear( Color.Black );

//            this.Draw( this.effect, this.effectPass );

//            this.GraphicsDevice.SetRenderTarget( null );
//        }

//        /// <summary>
//        /// Disposes the managed resources used by this ToneMapping effect.
//        /// </summary>
//        protected override void DisposeManagedResources()
//        {
//            this.parameterLuminanceTexture = null;
//            this.parameterSourceTexture = null;
//            this.effectPass = null;
//            this.effect = null;
//        }

//        /// <summary>
//        /// Disposes the unmanaged resources used by this ToneMapping effect.
//        /// </summary>
//        protected override void DisposeUnmanagedResources()
//        {
//            if( this.effect != null )
//            {
//                this.effect.Dispose();
//            }
//        }

//        #endregion

//        #region [ Fields ]

//        /// <summary>
//        /// The effect that excutes the actual tone mapping logic.
//        /// </summary>
//        private Effect effect;

//        /// <summary>
//        /// The cached first pass of the effect.
//        /// </summary>
//        private EffectPass effectPass;

//        /// <summary>
//        /// The parameter that sets the source input texture.
//        /// </summary>
//        private EffectParameter parameterSourceTexture;

//        /// <summary>
//        /// The parameter that sets the source luminance input texture.
//        /// </summary>
//        private EffectParameter parameterLuminanceTexture;

//        #endregion
//    }
//}
