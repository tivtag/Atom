//// <copyright file="HighDynamicRange.cs" company="federrot Software">
////     Copyright (c) federrot Software. All rights reserved.
//// </copyright>
//// <summary>
////     Defines the Atom.Xna.Effects.PostProcess.HighDynamicRange class.
//// </summary>
//// <author>
////     Paul Ennemoser
//// </author>

//namespace Atom.Xna.Effects.PostProcess
//{
//    using System;
//    using Microsoft.Xna.Framework.Content;
//    using Microsoft.Xna.Framework.Graphics;

//    /// <summary>
//    /// Implements a High Dynamic Range IPostProcessEffect.
//    /// </summary>
//    public sealed class HighDynamicRange : ManagedDisposable, IPostProcessEffect
//    {
//        #region [ Properties ]

//        /// <summary>
//        /// Gets or sets the color exposure applied by this HighDynamicRange effect.
//        /// </summary>
//        public float Exposure
//        {
//            get
//            {
//                return this.toneMapping.Exposure;
//            }

//            set
//            {
//                this.brightPass.Exposure = value;
//                this.toneMapping.Exposure = value;
//            }
//        }

//        /// <summary>
//        /// Gets or sets the treshold at which pixels are brightened or darkened.
//        /// </summary>
//        public float BrightnessThreshold
//        {
//            get
//            {
//                return this.brightPass.Threshold;
//            }

//            set
//            {
//                this.brightPass.Threshold = value;
//            }
//        }

//        /// <summary>
//        /// Gets or sets a value indicating whether blurring is currently enabled.
//        /// </summary>
//        public bool GaussianBlurEnabled
//        {
//            get;
//            set;
//        }

//        /// <summary>
//        /// Gets the Gaussian Blur effect that is applied by this HighDynamicRange.
//        /// </summary>
//        public GaussianBlur9x9 GaussianBlur
//        {
//            get
//            {
//                return this.gaussianBlur;
//            }
//        }

//        /// <summary>
//        /// Gets the xna GraphicsDevice requried for rendering.
//        /// </summary>
//        private GraphicsDevice GraphicsDevice
//        {
//            get
//            {
//                return this.graphicsDevice;
//            }
//        }

//        #endregion

//        #region [ Initialization ]

//        /// <summary>
//        /// Initializes a new instance of the HighDynamicRange class.
//        /// </summary>
//        /// <param name="effectLoader">
//        /// Provides a mechanism that allows loading of effect assets.
//        /// </param>
//        /// <param name="deviceService">
//        /// Provides access to the <see cref="GraphicsDevice"/>.
//        /// </param>
//        public HighDynamicRange( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
//        {
//            this.downSample4x4 = new DownSample4x4( effectLoader, deviceService );
//            this.gaussianBlur = new GaussianBlur9x9( effectLoader, deviceService );
//            this.brightPass = new BrightPass( effectLoader, deviceService );
//            this.toneMapping = new ToneMapping( effectLoader, deviceService );
//            this.addRGB = new AddRgb( effectLoader, deviceService );
//            this.luminance = new Luminance( effectLoader, deviceService );

//            this.deviceService = deviceService;
//        }

//        /// <summary>
//        /// Loads the content required by this HighDynamicRange effect.
//        /// </summary>
//        public void LoadContent()
//        {
//            this.downSample4x4.LoadContent();
//            this.gaussianBlur.LoadContent();
//            this.brightPass.LoadContent();
//            this.toneMapping.LoadContent();
//            this.addRGB.LoadContent();
//            this.luminance.LoadContent();

//            this.graphicsDevice = this.deviceService.GraphicsDevice;
//            this.currentLuminance = new RenderTarget2D( 
//                this.GraphicsDevice, 
//                1,
//                1,
//                1,
//                SurfaceFormat.HalfVector2,
//                RenderTargetUsage.DiscardContents
//            );
//        }

//        #endregion

//        #region [ Methods ]

//        /// <summary>
//        /// Applies this HighDynamicRange.
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
//        public void PostProcess( Texture2D sourceTexture, RenderTarget2D result, IXnaDrawContext drawContext )
//        {
//            this.EnsureHelperRenderTargets( sourceTexture );

//            // Luminance Pass.
//            this.luminance.PostProcess( sourceTexture, this.currentLuminance, drawContext );
//            this.toneMapping.LuminanceTexture = this.currentLuminance.GetTexture();

//            // Bright Pass.
//            this.brightPass.LuminanceTexture = this.currentLuminance.GetTexture();
//            this.brightPass.PostProcess( sourceTexture, this.glowEffectTarget, drawContext );

//            // Gaussian Blur Pass.
//            if( this.GaussianBlurEnabled )
//            {
//                this.gaussianBlur.Horizontal = true;
//                this.gaussianBlur.PostProcess( this.glowEffectTarget.GetTexture(), this.glowEffectTarget, drawContext );

//                this.gaussianBlur.Horizontal = false;
//                this.gaussianBlur.PostProcess( this.glowEffectTarget.GetTexture(), this.glowEffectTarget, drawContext );
//            }

//            // Glowing and Tone Mapping Pass.
//            this.glowEffectTarget.ApplyEffect( this.downSample4x4, drawContext );
//            this.toneMapping.PostProcess( sourceTexture, this.addRgbTarget, drawContext );

//            // Add RGB Pass.
//            this.addRGB.OtherTexture = this.glowEffectTarget.GetTexture();
//            this.addRGB.PostProcess( this.addRgbTarget.GetTexture(), result, drawContext );
//        }

//        /// <summary>
//        /// Ensures that the internal render targets have the correct size.
//        /// </summary>
//        /// <param name="sourceTexture">
//        /// The texture to post-process.
//        /// </param>
//        private void EnsureHelperRenderTargets( Texture2D sourceTexture )
//        {
//            int widthOver4 = sourceTexture.Width / 4;
//            int heightOver4 = sourceTexture.Height / 4;

//            if( this.glowEffectTarget == null )
//            {
//                this.glowEffectTarget = new RenderTarget2D( 
//                    this.GraphicsDevice,
//                    widthOver4,
//                    heightOver4,
//                    1,
//                    SurfaceFormat.HalfVector4,
//                    RenderTargetUsage.PreserveContents
//                );
//            }
//            else
//            {
//                if( this.glowEffectTarget.Width != widthOver4 || this.glowEffectTarget.Height != heightOver4 )
//                {
//                    this.glowEffectTarget.Dispose();

//                    this.glowEffectTarget = new RenderTarget2D( 
//                        this.GraphicsDevice,
//                        widthOver4,
//                        heightOver4,
//                        1,
//                        SurfaceFormat.HalfVector4,
//                        RenderTargetUsage.PreserveContents
//                    );
//                }
//            }

//            if( this.addRgbTarget == null )
//            {
//                this.addRgbTarget = new RenderTarget2D( 
//                    this.GraphicsDevice,
//                    sourceTexture.Width, 
//                    sourceTexture.Height,
//                    1,
//                    SurfaceFormat.HalfVector4,
//                    RenderTargetUsage.PreserveContents
//                );
//            }
//            else
//            {
//                if( this.addRgbTarget.Width != sourceTexture.Width ||
//                    this.addRgbTarget.Height != sourceTexture.Height )
//                {
//                    this.addRgbTarget.Dispose();

//                    this.addRgbTarget = new RenderTarget2D(
//                        this.GraphicsDevice, 
//                        sourceTexture.Width,
//                        sourceTexture.Height,
//                        1,
//                        SurfaceFormat.HalfVector4,
//                        RenderTargetUsage.PreserveContents
//                    );
//                }
//            }
//        }        

//        /// <summary>
//        /// Releases all managed resources.
//        /// </summary>
//        protected override void DisposeManagedResources()
//        {
//            this.downSample4x4 = null;
//            this.toneMapping = null;
//            this.brightPass = null;
//            this.gaussianBlur = null;
//            this.addRGB = null;
//            this.luminance = null;
//        }

//        /// <summary>
//        /// Releases all unmanaged resources.
//        /// </summary>
//        protected override void DisposeUnmanagedResources()
//        {
//            this.downSample4x4.Dispose();
//            this.toneMapping.Dispose();
//            this.brightPass.Dispose();
//            this.gaussianBlur.Dispose();
//            this.addRGB.Dispose();
//            this.luminance.Dispose();
//        }

//        #endregion

//        #region [ Fields ]

//        /// <summary>
//        /// The effect that is used to down-sample input. Used for the glow effect.
//        /// </summary>
//        private DownSample4x4 downSample4x4;
        
//        /// <summary>
//        /// The effect that is used to map the HDR input onto a LDR target.
//        /// </summary>
//        private ToneMapping toneMapping;
        
//        /// <summary>
//        /// The effect that is used to creat the glowing effect.
//        /// </summary>
//        private BrightPass brightPass;

//        /// <summary>
//        /// The effect that is used to blur the glowing effect vertically
//        /// and horizontally.
//        /// </summary>
//        private GaussianBlur9x9 gaussianBlur;

//        /// <summary>
//        /// The effect that is used to combine the tone-mapped scene
//        /// with the glowing effect.
//        /// </summary>
//        private AddRgb addRGB;

//        /// <summary>
//        /// The effect that is used to calculate the luminance of the input scene.
//        /// </summary>
//        private Luminance luminance;

//        /// <summary>
//        /// The RenderTarget that stores the luminance of the input scene.
//        /// </summary>
//        private RenderTarget2D currentLuminance;

//        /// <summary>
//        /// The RenderTarget that will contain the glow effect.
//        /// </summary>
//        private RenderTarget2D glowEffectTarget;

//        /// <summary>
//        /// The RenderTarget that is used to combine the tone-mapped scene
//        /// with the glowing effect.
//        /// </summary>
//        private RenderTarget2D addRgbTarget;

//        /// <summary>
//        /// The xna GraphicsDevice requried for rendering.
//        /// </summary>
//        private GraphicsDevice graphicsDevice;

//        /// <summary>
//        /// Provides access to the xna GraphicsDevice that is requried for rendering.
//        /// </summary>
//        private readonly IGraphicsDeviceService deviceService;

//        #endregion
//    }
//}