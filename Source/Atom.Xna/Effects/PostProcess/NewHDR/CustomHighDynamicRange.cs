//// <copyright file="HighDynamicRange.cs" company="federrot Software">
////     Copyright (c) federrot Software. All rights reserved.
//// </copyright>
//// <summary>
////     Defines the Atom.Xna.Effects.PostProcess.NewHDR.HighDynamicRange class.
//// </summary>
//// <author>
////     Paul Ennemoser (Tick)
//// </author>

//#pragma warning disable 1591

//namespace Atom.Xna.Effects.PostProcess.NewHDR
//{
//    using System.Collections.Generic;
//    using System.Globalization;
//    using Microsoft.Xna.Framework;
//    using Microsoft.Xna.Framework.Graphics;

//    /// <summary>
//    /// Handles rendering of various post-processing techniques,
//    /// including bloom and tone mapping
//    /// </summary>
//    public sealed class CustomHighDynamicRange : PostProcessEffect
//    {    
//        /// <summary>
//        /// 
//        /// </summary>
//        public float Exposure
//        {
//            get
//            {
//                return this.brightPass.Exposure;
//            }

//            set
//            {
//                this.brightPass.Exposure = value;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public float Threshold
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
//        /// Initializes a new instance of the CustomHighDynamicRange class.
//        /// </summary>
//        /// <param name="effectLoader">
//        /// Provides a mechanism that allows loading of effect assets.
//        /// </param>
//        /// <param name="deviceService">
//        /// Provides access to the <see cref="GraphicsDevice"/>.
//        /// </param>
//        public CustomHighDynamicRange( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
//            : base( effectLoader, deviceService )
//        {
//            this.brightPass = new BrightPass( effectLoader, deviceService );
//            this.toneMapping = new ToneMapping( effectLoader, deviceService );
//        }

//        /// <summary>
//        /// Loads the effects used by this HighDynamicRange effect.
//        /// </summary>
//        /// <param name="effectLoader">
//        /// Provides a mechanism that allows loading of effect assets.
//        /// </param>
//        protected override void LoadEffect( IEffectLoader effectLoader )
//        {
//            this.HDREffect = effectLoader.Load( "HighDynamicRange" );
//            this.blurEffect = effectLoader.Load( "Blur" );
//            this.scalingEffect = effectLoader.Load( "Scale" );
            
//        }

//        /// <summary>
//        /// Loads the content used by this HighDynamicRange effect.
//        /// </summary>
//        protected override void LoadCustomContent()
//        {
//            var graphicsDevice = this.GraphicsDevice;

//            // Initialize our buffers
//            int width = graphicsDevice.PresentationParameters.BackBufferWidth;
//            int height = graphicsDevice.PresentationParameters.BackBufferHeight;

//            // Two buffers we'll swap between, so we can adapt the luminance            
//            this.currentFrameLuminance = new RenderTarget2D( graphicsDevice, 1, 1, 1, SurfaceFormat.Single, RenderTargetUsage.DiscardContents );
//            this.currentFrameAdaptedLuminance = new RenderTarget2D( graphicsDevice, 1, 1, 1, SurfaceFormat.Single, RenderTargetUsage.DiscardContents );
//            this.lastFrameAdaptedLuminance = new RenderTarget2D( graphicsDevice, 1, 1, 1, SurfaceFormat.Single, RenderTargetUsage.DiscardContents );

//            var oldRenderTarget = graphicsDevice.GetRenderTarget( 0 ) as RenderTarget2D;
//            graphicsDevice.SetRenderTarget( 0, this.lastFrameAdaptedLuminance );
//            graphicsDevice.Clear( Color.White );
//            graphicsDevice.SetRenderTarget( 0, oldRenderTarget );

//            // We need a luminance chain
//            int chainLength = 1;
//            int startSize = (int)MathHelper.Min( width / 16, height / 16 );
//            int size = 16;

//            for( size = 16; size < startSize; size *= 4 )
//            {
//                ++chainLength;
//            }

//            this.luminanceChain = new RenderTarget2D[chainLength];
//            size /= 4;
//            for( int i = 0; i < chainLength; i++ )
//            {
//                this.luminanceChain[i] = new RenderTarget2D( graphicsDevice, size, size, 1, SurfaceFormat.Single );
//                size /= 4;
//            }

//            // Check to see if we can filter fp16.
//            this.canFilterFP16 = GraphicsAdapter.DefaultAdapter.CheckDeviceFormat(
//                DeviceType.Hardware,
//                SurfaceFormat.Color,
//                TextureUsage.None,
//                QueryUsages.Filter,
//                ResourceType.Texture2D,
//                SurfaceFormat.HalfVector4
//            );

//            this.brightPass.LoadContent();
//            this.toneMapping.LoadContent();
//        }

//        /// <summary>
//        /// Applies this HighDynamicRange effect.
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
//            float frameTime = (float)drawContext.GameTime.ElapsedGameTime.TotalSeconds;
//            this.ToneMap( sourceTexture, result, frameTime, false, true, drawContext );  
//        }

//        /// <summary>
//        /// Performs tone mapping on the specified render target
//        /// </summary>
//        /// <param name="source">The source render target</param>
//        /// <param name="result">The render target to which the result will be output</param>
//        /// <param name="frameTime">The time elapsed since the last frame</param>
//        /// <param name="encoded">If true, use LogLuv encoding</param>
//        /// <param name="preferHWScaling">If true, will attempt to use hardware filtering</param>
//        /// <param name="drawContext">
//        /// The context under which the drawing operation occurrs.
//        /// </param>
//        public void ToneMap( Texture2D source, RenderTarget2D result, float frameTime, bool encoded, bool preferHWScaling, IXnaDrawContext drawContext )
//        {
//            // Downscale to 1/16 size
//            IntermediateTexture downscaleTarget = this.GetIntermediateTexture( source.Width / 16, source.Height / 16, source.Format );
//            if( preferHWScaling && (encoded || canFilterFP16) )
//                this.GenerateDownscaleTargetHW( source, downscaleTarget.RenderTarget );
//            else
//                this.GenerateDownscaleTargetSW( source, downscaleTarget.RenderTarget, encoded );

//            // Get the luminance
//            this.CalculateAverageLuminance( downscaleTarget.RenderTarget, frameTime, encoded );

//            // Do the bloom first
//            IntermediateTexture threshold = this.GetIntermediateTexture( 
//                downscaleTarget.RenderTarget.Width, 
//                downscaleTarget.RenderTarget.Height, 
//                source.Format 
//            );

//            this.GraphicsDevice.SetRenderTarget( 0, null );
//            this.brightPass.LuminanceTexture = this.currentFrameAdaptedLuminance == null ? this.currentFrameLuminance.GetTexture() : this.currentFrameAdaptedLuminance.GetTexture();
//            this.brightPass.PostProcess( downscaleTarget.RenderTarget.GetTexture(), threshold.RenderTarget, drawContext );

//            IntermediateTexture postBlur = this.GetIntermediateTexture( downscaleTarget.RenderTarget.Width, downscaleTarget.RenderTarget.Height, SurfaceFormat.Color );
//            this.Blur( threshold.RenderTarget, postBlur.RenderTarget, blurSigma, encoded );
//            threshold.InUse = false;

//            // Scale it back to half of full size (will do the final scaling step when sampling
//            // the bloom texture during tone mapping).
//            IntermediateTexture upscale1 = this.GetIntermediateTexture( source.Width / 8, source.Height / 8, SurfaceFormat.Color );
//            this.scalingEffect.CurrentTechnique = this.scalingEffect.Techniques["ScaleHW"];
//            this.ApplyPostProcess( postBlur.RenderTarget, upscale1.RenderTarget, this.scalingEffect );
//            postBlur.InUse = false;

//            IntermediateTexture upscale2 = this.GetIntermediateTexture( source.Width / 4, source.Height / 4, SurfaceFormat.Color );
//            this.ApplyPostProcess( upscale1.RenderTarget, upscale2.RenderTarget, this.scalingEffect );
//            upscale1.InUse = false;

//            IntermediateTexture bloom = GetIntermediateTexture( source.Width / 2, source.Height / 2, SurfaceFormat.Color );
//            this.ApplyPostProcess( upscale2.RenderTarget, bloom.RenderTarget, this.scalingEffect );
//            upscale2.InUse = false;

//            // Now do tone mapping on the main source image, and add in the bloom
//            HDREffect.Parameters["g_fMiddleGrey"].SetValue( toneMapKey );
//            HDREffect.Parameters["g_fMaxLuminance"].SetValue( maxLuminance );
//            HDREffect.Parameters["g_fBloomMultiplier"].SetValue( bloomMultiplier );

//            this.GraphicsDevice.SetRenderTarget( 0, null );
//            Texture2D[] sources3 = new Texture2D[3];
//            sources3[0] = source;
//            sources3[1] = currentFrameAdaptedLuminance.GetTexture();
//            sources3[2] = bloom.RenderTarget.GetTexture();
//            if( encoded )
//                HDREffect.CurrentTechnique = HDREffect.Techniques["ToneMapEncode"];
//            else
//                HDREffect.CurrentTechnique = HDREffect.Techniques["ToneMap"];
//            ApplyPostProcess( sources3, result, HDREffect );

//            // Flip the luminance textures
//            Swap.Them( ref currentFrameAdaptedLuminance, ref lastFrameAdaptedLuminance );

//            bloom.InUse = false;
//            downscaleTarget.InUse = false;
//        }

//        /// <summary>
//        /// Applies a blur to the specified render target, writes the result
//        /// to the specified render target.
//        /// </summary>
//        /// <param name="source">
//        /// The source texture to blur.
//        /// </param>
//        /// <param name="result">
//        /// The target into which the blurred result is written.
//        /// </param>
//        /// <param name="sigma">The standard deviation used for gaussian weights</param>
//        /// <param name="encoded">If true, blurs using LogLuv encoding/decoding</param>
//        private void Blur( RenderTarget2D source, RenderTarget2D result, float sigma, bool encoded )
//        {
//            IntermediateTexture blurH = this.GetIntermediateTexture(
//                source.Width,
//                source.Height,
//                source.Format,
//                source.MultiSampleType,
//                source.MultiSampleQuality
//            );

//            string baseTechniqueName = encoded ? "GaussianBlurEncode" : "GaussianBlur";

//            // Do horizontal pass first
//            this.blurEffect.CurrentTechnique = this.blurEffect.Techniques[baseTechniqueName + "H"];
//            this.blurEffect.Parameters["g_fSigma"].SetValue( sigma );
//            this.ApplyPostProcess( source, blurH.RenderTarget, this.blurEffect );

//            // Now the vertical pass 
//            this.blurEffect.CurrentTechnique = this.blurEffect.Techniques[baseTechniqueName + "V"];
//            this.ApplyPostProcess( blurH.RenderTarget, result, this.blurEffect );
//            blurH.InUse = false;
//        }

//        /// <summary>
//        /// Downscales the source to 1/16th size, using software(shader) filtering
//        /// </summary>
//        /// <param name="source">
//        /// The source to be downscaled.
//        /// </param>
//        /// <param name="result">
//        /// The RenderTarget in which to store the result.
//        /// </param>
//        /// <param name="encoded">
//        /// If true, the source is encoded in LogLuv format;
//        /// otherwise false.
//        /// </param>
//        private void GenerateDownscaleTargetSW( Texture2D source, RenderTarget2D result, bool encoded )
//        {
//            string techniqueName = encoded ? "Downscale4Encode" : "Downscale4";

//            IntermediateTexture downscale1 = this.GetIntermediateTexture( source.Width / 4, source.Height / 4, source.Format );
//            this.scalingEffect.CurrentTechnique = this.scalingEffect.Techniques[techniqueName];
//            this.ApplyPostProcess( source, downscale1.RenderTarget, this.scalingEffect );

//            this.scalingEffect.CurrentTechnique = this.scalingEffect.Techniques[techniqueName];
//            this.ApplyPostProcess( downscale1.RenderTarget, result, this.scalingEffect );
//            downscale1.InUse = false;
//        }

//        /// <summary>
//        /// Downscales the source to 1/16th size, using hardware filtering
//        /// </summary>
//        /// <param name="source">
//        /// The source to be downscaled.
//        /// </param>
//        /// <param name="result">
//        /// The RenderTarget in which to store the result.
//        /// </param>
//        private void GenerateDownscaleTargetHW( Texture2D source, RenderTarget2D result )
//        {
//            IntermediateTexture downscale1 = this.GetIntermediateTexture( source.Width / 2, source.Height / 2, source.Format );
//            this.scalingEffect.CurrentTechnique = this.scalingEffect.Techniques["ScaleHW"];
//            this.ApplyPostProcess( source, downscale1.RenderTarget, this.scalingEffect );

//            IntermediateTexture downscale2 = this.GetIntermediateTexture( source.Width / 2, source.Height / 2, source.Format );
//            this.scalingEffect.CurrentTechnique = this.scalingEffect.Techniques["ScaleHW"];
//            this.ApplyPostProcess( downscale1.RenderTarget, downscale2.RenderTarget, this.scalingEffect );
//            downscale1.InUse = false;

//            IntermediateTexture downscale3 = this.GetIntermediateTexture( source.Width / 2, source.Height / 2, source.Format );
//            this.scalingEffect.CurrentTechnique = this.scalingEffect.Techniques["ScaleHW"];
//            this.ApplyPostProcess( downscale2.RenderTarget, downscale3.RenderTarget, this.scalingEffect );
//            downscale2.InUse = false;

//            this.scalingEffect.CurrentTechnique = this.scalingEffect.Techniques["ScaleHW"];
//            this.ApplyPostProcess( downscale3.RenderTarget, result, this.scalingEffect );
//            downscale3.InUse = false;
//        }

//        /// <summary>
//        /// Calculates the average luminance of the scene
//        /// </summary>
//        /// <param name="downscaleBuffer">The scene texure, downscaled to 1/16th size</param>
//        /// <param name="frameTime">The time delta</param>
//        /// <param name="encoded">If true, the image is encoded in LogLuv format</param>
//        private void CalculateAverageLuminance( RenderTarget2D downscaleBuffer, float frameTime, bool encoded )
//        {
//            // Calculate the initial luminance
//            if( encoded )
//                this.HDREffect.CurrentTechnique = this.HDREffect.Techniques["LuminanceEncode"];
//            else
//                this.HDREffect.CurrentTechnique = this.HDREffect.Techniques["Luminance"];

//            this.ApplyPostProcess( downscaleBuffer, this.luminanceChain[0], this.HDREffect );

//            // Repeatedly downscale            
//            scalingEffect.CurrentTechnique = scalingEffect.Techniques["Downscale4"];
//            for( int i = 1; i < luminanceChain.Length; ++i )
//            {
//                this.ApplyPostProcess( this.luminanceChain[i - 1], this.luminanceChain[i], this.scalingEffect );
//            }

//            // Final downscale            
//            this.scalingEffect.CurrentTechnique = this.scalingEffect.Techniques["Downscale4Luminance"];
//            this.ApplyPostProcess( this.luminanceChain[luminanceChain.Length - 1], this.currentFrameLuminance, this.scalingEffect );

//            // Adapt the luminance, to simulate slowly adjust exposure
//            this.HDREffect.Parameters["g_fDT"].SetValue( frameTime );
//            this.HDREffect.CurrentTechnique = this.HDREffect.Techniques["CalcAdaptedLuminance"];

//            RenderTarget2D[] sources = new RenderTarget2D[2];
//            sources[0] = this.currentFrameLuminance;
//            sources[1] = this.lastFrameAdaptedLuminance;
//            this.ApplyPostProcess( sources, this.currentFrameAdaptedLuminance, this.HDREffect );
//        }

//        /// <summary>
//        /// Disposes all intermediate textures in the cache
//        /// </summary>
//        public void FlushCache()
//        {
//            foreach( IntermediateTexture intermediateTexture in this.intermediateTextures )
//            {
//                intermediateTexture.RenderTarget.Dispose();
//            }

//            this.intermediateTextures.Clear();
//        }

//        /// <summary>
//        /// Performs a post-processing step using a single source texture
//        /// </summary>
//        /// <param name="source">The source texture</param>
//        /// <param name="result">The output render target</param>
//        /// <param name="effect">The effect to use</param>
//        private void ApplyPostProcess( Texture2D source, RenderTarget2D result, Effect effect )
//        {
//            var graphicsDevice = this.GraphicsDevice;
//            graphicsDevice.SetRenderTarget( 0, result );
//            graphicsDevice.Clear( Color.Black );

//            this.ApplyPostProcessSettings( source, result, effect );
//            base.Draw( effect, effect.CurrentTechnique.Passes[0] );
//        }

//        /// <summary>
//        /// Performs a post-processing step using a single source texture
//        /// </summary>
//        /// <param name="sources">The source textures</param>
//        /// <param name="result">The output render target</param>
//        /// <param name="effect">The effect to use</param>
//        private void ApplyPostProcess( Texture2D[] sources, RenderTarget2D result, Effect effect )
//        {
//            var graphicsDevice = this.GraphicsDevice;
//            graphicsDevice.SetRenderTarget( 0, result );
//            graphicsDevice.Clear( Color.Black );

//            this.ApplyPostProcessSettings( sources, result, effect );
//            base.Draw( effect, effect.CurrentTechnique.Passes[0] );
//        }

//        /// <summary>
//        /// Performs a post-processing step using a single source texture
//        /// </summary>
//        /// <param name="source">The source texture</param>
//        /// <param name="result">The output render target</param>
//        /// <param name="effect">The effect to use</param>
//        private void ApplyPostProcess( RenderTarget2D source, RenderTarget2D result, Effect effect )
//        {
//            var graphicsDevice = this.GraphicsDevice;
//            graphicsDevice.SetRenderTarget( 0, result );
//            graphicsDevice.Clear( Color.Black );

//            this.ApplyPostProcessSettings( source.GetTexture(), result, effect );
//            base.Draw( effect, effect.CurrentTechnique.Passes[0] );
//        }

//        /// <summary>
//        /// Performs a post-processing step using multiple source textures
//        /// </summary>
//        /// <param name="sources">The source textures</param>
//        /// <param name="result">The output render target</param>
//        /// <param name="effect">The effect to use</param>
//        private void ApplyPostProcess( RenderTarget2D[] sources, RenderTarget2D result, Effect effect )
//        {
//            var graphicsDevice = this.GraphicsDevice;
//            graphicsDevice.SetRenderTarget( 0, result );
//            graphicsDevice.Clear( Color.Black );

//            this.ApplyPostProcessSettings( sources, result, effect );
//            base.Draw( effect, effect.CurrentTechnique.Passes[0] );
//        }

//        /// <summary>
//        /// Applies the settings for drawing the given source texture into the given result target
//        /// by applying the given effect.
//        /// </summary>
//        /// <param name="source">
//        /// The source texture.
//        /// </param>
//        /// <param name="result">
//        /// The target of the effect.
//        /// </param>
//        /// <param name="effect">
//        /// The effect to apply.
//        /// </param>
//        private void ApplyPostProcessSettings( Texture2D source, RenderTarget2D result, Effect effect )
//        {
//            effect.Parameters["SourceTexture0"].SetValue( source );
//            effect.Parameters["g_vSourceDimensions"].SetValue( new Vector2( source.Width, source.Height ) );

//            if( result == null )
//                effect.Parameters["g_vDestinationDimensions"].SetValue( new Vector2( this.GraphicsDevice.PresentationParameters.BackBufferWidth, this.GraphicsDevice.PresentationParameters.BackBufferHeight ) );
//            else
//                effect.Parameters["g_vDestinationDimensions"].SetValue( new Vector2( result.Width, result.Height ) );
//        }

//        /// <summary>
//        /// Applies the settings for drawing the given source textures into the given result target
//        /// by applying the given effect.
//        /// </summary>
//        /// <param name="sources">
//        /// The source textures.
//        /// </param>
//        /// <param name="result">
//        /// The target of the effect.
//        /// </param>
//        /// <param name="effect">
//        /// The effect to apply.
//        /// </param>
//        private void ApplyPostProcessSettings( Texture2D[] sources, RenderTarget2D result, Effect effect )
//        {
//            for( int i = 1; i < sources.Length; ++i )
//            {
//                effect.Parameters["SourceTexture" + i.ToString( CultureInfo.InvariantCulture )].SetValue( sources[i] );
//            }

//            this.ApplyPostProcessSettings( sources[0], result, effect );
//        }

//        /// <summary>
//        /// Applies the settings for drawing the given source textures into the given result target
//        /// by applying the given effect.
//        /// </summary>
//        /// <param name="sources">
//        /// The source textures.
//        /// </param>
//        /// <param name="result">
//        /// The target of the effect.
//        /// </param>
//        /// <param name="effect">
//        /// The effect to apply.
//        /// </param>
//        private void ApplyPostProcessSettings( RenderTarget2D[] sources, RenderTarget2D result, Effect effect )
//        {
//            for( int i = 1; i < sources.Length; ++i )
//            {
//                effect.Parameters["SourceTexture" + i.ToString( CultureInfo.InvariantCulture )].SetValue( sources[i].GetTexture() );
//            }

//            this.ApplyPostProcessSettings( sources[0].GetTexture(), result, effect );
//        }

//        /// <summary>
//        /// Checks the cache to see if a suitable rendertarget has already been created
//        /// and isn't in use.  Otherwise, creates one according to the parameters
//        /// </summary>
//        /// <param name="width">
//        /// The width of the RenderTarget.
//        /// </param>
//        /// <param name="height">
//        /// The height of the RenderTarget.
//        /// </param>
//        /// <param name="format">
//        /// The format of the RenderTarget.
//        /// </param>
//        /// <returns>
//        /// The suitable RenderTarget.
//        /// </returns>
//        private IntermediateTexture GetIntermediateTexture( int width, int height, SurfaceFormat format )
//        {
//            return this.GetIntermediateTexture( width, height, format, 0 );
//        }

//        /// <summary>
//        /// Checks the cache to see if a suitable rendertarget has already been created
//        /// and isn't in use.  Otherwise, creates one according to the parameters
//        /// </summary>
//        /// <param name="width">
//        /// The width of the RenderTarget.
//        /// </param>
//        /// <param name="height">
//        /// The height of the RenderTarget.
//        /// </param>
//        /// <param name="format">
//        /// The format of the RenderTarget.
//        /// </param>
//        /// <param name="multiSampleCount">
//        /// The number of sample locations of the RenderTarget
//        /// </param>
//        /// <returns>
//        /// The suitable RenderTarget.
//        /// </returns>
//        private IntermediateTexture GetIntermediateTexture( int width, int height, SurfaceFormat format, int multiSampleCount )
//        {
//            // Look for a matching rendertarget in the cache
//            for( int i = 0; i < intermediateTextures.Count; i++ )
//            {
//                var texture = intermediateTextures[i];

//                if( !texture.InUse && 
//                    height == texture.RenderTarget.Height &&
//                    format == texture.RenderTarget.Format &&
//                    width == texture.RenderTarget.Width &&
//                    msType == texture.RenderTarget.MultiSampleCount  )
//                {
//                    texture.InUse = true;
//                    return texture;
//                }
//            }

//            // We didn't find one, let's make one
//            var newTexture = new IntermediateTexture()
//            {
//                RenderTarget = new RenderTarget2D(
//                this.GraphicsDevice,
//                width,
//                height,
//                1,
//                format,
//                DepthFormat.None,
//                multiSampleCount,
//                RenderTargetUsage.DiscardContents
//            )
//            };

//            this.intermediateTextures.Add( newTexture );
//            newTexture.InUse = true;
//            return newTexture;
//        }

//        public float toneMapKey = 0.8f;
//        public float maxLuminance = 512.0f;
//        public float bloomThreshold = 0.85f;
//        public float bloomMultiplier = 1.0f;
//        public float blurSigma = 2.5f;

//        private Effect blurEffect;
//        private Effect scalingEffect;
//        private Effect HDREffect;
//        private BrightPass brightPass;

//        private RenderTarget2D currentFrameLuminance;
//        private RenderTarget2D currentFrameAdaptedLuminance;
//        private RenderTarget2D lastFrameAdaptedLuminance;
//        private RenderTarget2D[] luminanceChain;
//        private bool canFilterFP16;

//        private readonly ToneMapping toneMapping;

//        private readonly List<IntermediateTexture> intermediateTextures = new List<IntermediateTexture>();

//        /// <summary>
//        /// Used for textures that store intermediate results of
//        /// passes during post-processing
//        /// </summary>
//        private sealed class IntermediateTexture
//        {
//            public RenderTarget2D RenderTarget;
//            public bool InUse;
//        }
//    }
//}