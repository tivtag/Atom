// <copyright file="AddRgb.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.PostProcess.AddRgb class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Effects.PostProcess
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements a <see cref="PostProcessEffect"/> that merges the content of a render target
    /// with another texture.
    /// </summary>
    public sealed class AddRgb : PostProcessEffect
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the other texture added to the source texture.
        /// </summary>
        public Texture2D OtherTexture
        {
            get
            {
                return this.parameterOtherTexture.GetValueTexture2D();
            }

            set
            {
                this.parameterOtherTexture.SetValue( value );
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the AddRgb class.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        /// <param name="deviceService">
        /// Provides access to the <see cref="GraphicsDevice"/>.
        /// </param>
        public AddRgb( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
            : base( effectLoader, deviceService )
        {
        }

        /// <summary>
        /// Loads the effect used by this AddRgb.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        protected override void LoadEffect( IEffectLoader effectLoader )
        {
            this.effect = effectLoader.Load( "AddRgb" );
            this.effectPass = this.effect.CurrentTechnique.Passes[0];

            this.parameterSourceTexture = this.effect.Parameters["SourceTexture1"];
            this.parameterOtherTexture = this.effect.Parameters["SourceTexture2"];
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Applies this AddRgb PostProcessEffect.
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
            this.parameterSourceTexture.SetValue( sourceTexture );

            this.GraphicsDevice.SetRenderTarget( result );
            this.GraphicsDevice.Clear( Color.Black );

            this.Draw( this.effect, this.effectPass );

            this.GraphicsDevice.SetRenderTarget( null );
        }
        
        /// <summary>
        /// Disposes the managed resources used by this AddRgb effect.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            this.effect = null;
            this.effectPass = null;
            this.parameterOtherTexture = null;
            this.parameterSourceTexture = null;
        }

        /// <summary>
        /// Disposes the unmanaged resources used by this AddRgb effect.
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
        /// The effect that excutes the actual combination logic.
        /// </summary>
        private Effect effect;

        /// <summary>
        /// The cached first pass of the effect.
        /// </summary>
        private EffectPass effectPass;

        /// <summary>
        /// The parameter used to change the source texture.
        /// </summary>
        private EffectParameter parameterSourceTexture;

        /// <summary>
        /// The paramter used to change the other texture that is combined
        /// with the source texture.
        /// </summary>
        private EffectParameter parameterOtherTexture;

        #endregion
    }
}
