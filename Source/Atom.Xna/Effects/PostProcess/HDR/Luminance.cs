//// <copyright file="Luminance.cs" company="federrot Software">
////     Copyright (c) federrot Software. All rights reserved.
//// </copyright>
//// <summary>Defines the Atom.Xna.Effects.PostProcess.Luminance class.</summary>
//// <author>Paul Ennemoser (Tick)</author>

//namespace Atom.Xna.Effects.PostProcess
//{
//    using Atom.Math;
//    using Microsoft.Xna.Framework.Graphics;

//    /// <summary>
//    /// Implements a <see cref="PostProcessEffect"/> that ... .
//    /// </summary>
//    public sealed class Luminance : PostProcessEffect
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
//        public Luminance( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
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
//            this.effect = effectLoader.Load( "Luminance" );
//            this.effectPass = this.effect.CurrentTechnique.Passes[0];

//            this.parameterTargetSize = this.effect.Parameters["TargetSize"];
//            this.parameterSourceSize = this.effect.Parameters["SourceSize"];
//            this.parameterSourceTexture = this.effect.Parameters["SourceTexture"];
//            this.parameterShaderIndex = this.effect.Parameters["ShaderIndex"];
//        }

//        /// <summary>
//        /// Creates the luminance chain.
//        /// </summary>
//        /// <param name="width">
//        /// The width of the source texture.
//        /// </param>
//        /// <param name="height">
//        /// The width of the height texture.
//        /// </param>
//        private void CreateLuminanceChain( int width, int height )
//        {
//            int size = 1;
//            int chainLength = 1;
//            int startSize = System.Math.Min( width / 4, height / 4 );

//            for(; size < startSize; size *= 3 )
//            {
//                ++chainLength;
//            }

//            this.luminanceChain = new RenderTarget2D[chainLength];

//            for( int i = 0; i < chainLength; ++i )
//            {
//                this.luminanceChain[i] = new RenderTarget2D(
//                    this.GraphicsDevice,
//                    size,
//                    size,
//                    1,
//                    SurfaceFormat.HalfVector2
//                );

//                size /= 3;
//            }

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

//            var sourceSize = new Microsoft.Xna.Framework.Vector2();

//            if( value == null )
//            {
//                sourceSize.X = 1;
//                sourceSize.Y = 1;
//            }
//            else
//            {
//                sourceSize.X = value.Width;
//                sourceSize.Y = value.Height;
//            }

//            this.parameterSourceSize.SetValue( sourceSize );
//        }

//        /// <summary>
//        /// Applies this Luminance PostProcessEffect.
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
//            this.EnsureLuminanceChain( sourceTexture );
//            this.SetTargetSize( result );

//            this.DrawInitialLuminance( sourceTexture );
//            this.DrawRemainingLuminance();            
//            this.DrawLuminanceToTarget( result );
//        }

//        /// <summary>
//        /// Draws the initial luminance at index 0.
//        /// </summary>
//        /// <param name="sourceTexture">
//        /// The texture to post-process.
//        /// </param>
//        private void DrawInitialLuminance( Texture2D sourceTexture )
//        {
//            this.parameterShaderIndex.SetValue( 0 );
//            this.SetupLuminance( 0 );
//            this.DrawLuminance( sourceTexture );
//        }
        
//        /// <summary>
//        /// Draws the remaining luminance targets.
//        /// </summary>
//        private void DrawRemainingLuminance()
//        {
//            this.parameterShaderIndex.SetValue( 1 );

//            for( int i = 1; i < this.luminanceChain.Length; ++i )
//            {
//                this.SetupLuminance( i );
//                this.DrawLuminance( luminanceChain[i - 1].GetTexture() );
//            }
//        }
        
//        /// <summary>
//        /// Finishes-up this Luminance effect, drawing the finished effect to the given RenderTarget.
//        /// </summary>
//        /// <param name="result">
//        /// The target that should contain the result of this LuminanceEffect.
//        /// </param>
//        private void DrawLuminanceToTarget( RenderTarget2D result )
//        {
//            this.GraphicsDevice.SetRenderTarget( 0, result );
//            this.SetTargetSize( result );

//            int last = this.luminanceChain.Length - 1;

//            var texture = this.luminanceChain[last].GetTexture();
//            this.SetSourceTexture( texture );

//            this.GraphicsDevice.Clear( Color.Black );
//            this.DrawEffect();
//            this.GraphicsDevice.SetRenderTarget( 0, null );
//        }
        
//        /// <summary>
//        /// Sets the targetSize field to contain the given RenderTarget2D.
//        /// </summary>
//        /// <param name="renderTarget">
//        /// The target to render to.
//        /// </param>
//        private void SetTargetSize( RenderTarget2D renderTarget )
//        {
//            Vector2 targetSize = this.GetTargetSize( renderTarget );
//            this.parameterTargetSize.SetValue( targetSize.ToXna() );
//        }

//        /// <summary>
//        /// Setups for rendering the luminance at the given chain-index.
//        /// </summary>
//        /// <param name="index">
//        /// The zero-based index of the luminance in the luminanceChain.
//        /// </param>
//        private void SetupLuminance( int index )
//        {
//            var luminanceTarget = this.luminanceChain[index];

//            this.GraphicsDevice.SetRenderTarget( 0, luminanceTarget );
//            this.SetTargetSize( luminanceTarget );
//        }

//        /// <summary>
//        /// Actually draws the Luminance effect.
//        /// </summary>
//        /// <param name="sourceTexture">
//        /// The texture that contains the previous rendering result.
//        /// </param>
//        private void DrawLuminance( Texture2D sourceTexture )
//        {
//            this.SetSourceTexture( sourceTexture );
//            this.GraphicsDevice.Clear( Color.Black );

//            this.DrawEffect();
//            this.GraphicsDevice.SetRenderTarget( 0, null );
//        }

//        /// <summary>
//        /// Draws the luminance effect.
//        /// </summary>
//        private void DrawEffect()
//        {
//            this.Draw( this.effect, this.effectPass );
//        }
        
//        /// <summary>
//        /// Disposes the managed resources used by this Luminance effect.
//        /// </summary>
//        protected override void DisposeManagedResources()
//        {
//            this.effect = null;
//            this.luminanceChain = null;
//            this.parameterTargetSize = null;
//            this.parameterSourceSize = null;
//            this.parameterShaderIndex = null;
//            this.parameterSourceTexture = null;
//        }

//        /// <summary>
//        /// Disposes the unmanaged resources used by this Luminance effect.
//        /// </summary>
//        protected override void DisposeUnmanagedResources()
//        {
//            if( this.luminanceChain != null )
//            {
//                for( int i = 0; i < this.luminanceChain.Length; ++i )
//                {
//                    this.luminanceChain[i].Dispose();
//                }
//            }

//            if( this.effect != null )
//            {
//                this.effect.Dispose();
//            }
//        }

//        #endregion

//        #region [ Fields ]

//        /// <summary>
//        /// The current width of the chain.
//        /// </summary>
//        private int chainWidth = -1;

//        /// <summary>
//        /// The current height of the chain.
//        /// </summary>
//        private int chainHeight = -1;

//        /// <summary>
//        /// The individual RenderTarget2Ds that beginning at the chain size
//        /// get smaller and smaller until the last item has a size of 1x1.
//        /// </summary>
//        private RenderTarget2D[] luminanceChain;

//        /// <summary>
//        /// The effect that excutes the actual luminance calculation logic.
//        /// </summary>
//        private Effect effect;

//        /// <summary>
//        /// The cached first pass of the effect.
//        /// </summary>
//        private EffectPass effectPass;

//        /// <summary>
//        /// The parameter used to set the size of the target texture.
//        /// </summary>
//        private EffectParameter parameterTargetSize;

//        /// <summary>
//        /// The parameter used to set the size of the source texture.
//        /// </summary>
//        private EffectParameter parameterSourceSize;

//        /// <summary>
//        /// The parameter used to set the source input texture.
//        /// </summary>
//        private EffectParameter parameterSourceTexture;

//        /// <summary>
//        /// The parameter used to set the shader index which indicates
//        /// what shader is choosen internally.
//        /// </summary>
//        private EffectParameter parameterShaderIndex;

//        #endregion
//    }
//}
