// <copyright file="DownSample4x4.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Effects.PostProcess.DownSample4x4 class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Effects.PostProcess
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements a <see cref="PostProcessEffect"/> that ... .
    /// </summary>
    public sealed class DownSample4x4 : PostProcessEffect
    {
        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the DownSample4x4 class.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        /// <param name="deviceService">
        /// Provides access to the <see cref="GraphicsDevice"/>.
        /// </param>
        public DownSample4x4( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
            : base( effectLoader, deviceService )
        {
        }

        /// <summary>
        /// Loads the effect used by this DownSample4x4.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        protected override void LoadEffect( IEffectLoader effectLoader )
        {
            this.effect = effectLoader.Load( "DownSample4x4" );
            this.effectPass = this.effect.CurrentTechnique.Passes[0];

            this.parameterTargetSize = effect.Parameters["TargetSize"];
            this.parameterSourceSize = effect.Parameters["SourceSize"];
            this.parameterSourceTexture = effect.Parameters["SourceTexture"];
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Applies this DownSample4x4 PostProcessEffect.
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

            var sourceSize = Microsoft.Xna.Framework.Vector2.One;

            if( value != null )
            {
                sourceSize.X = value.Width;
                sourceSize.Y = value.Height;
            }

            this.parameterSourceSize.SetValue( sourceSize );
        }
        
        /// <summary>
        /// Disposes the managed resources used by this DownSample4x4 effect.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            this.parameterTargetSize = null;
            this.parameterSourceSize = null;
            this.parameterSourceTexture = null;
            this.effectPass = null;
            this.effect = null;
        }

        /// <summary>
        /// Disposes the unmanaged resources used by this DownSample4x4 effect.
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
        /// The effect that contain the actual down-sampling logic.
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

        #endregion
    }
}
