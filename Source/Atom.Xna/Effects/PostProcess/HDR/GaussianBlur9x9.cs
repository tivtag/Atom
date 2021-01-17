// <copyright file="GaussianBlur9x9.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Effects.PostProcess.GaussianBlur9x9 class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Xna.Effects.PostProcess
{
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements a <see cref="PostProcessEffect"/> that ... .
    /// </summary>
    public sealed class GaussianBlur9x9 : PostProcessEffect
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets a value indicating whether this GaussianBlur9x9 is blurring
        /// horizontally; or vertically.
        /// </summary>
        /// <value>
        /// true if horizontal blurring is enabled;
        /// otherwise false if vertical blurring is enabled.
        /// </value>
        public bool Horizontal 
        {
            get
            {
                return this.horizontal;
            }

            set
            {
                this.horizontal = value;
                this.effect.CurrentTechnique = value ? this.techniqueH : this.techniqueV;
                this.effectPass = this.effect.CurrentTechnique.Passes[0];
            }
        }

        /// <summary>
        /// Gets or sets the Standard Deviation of the Gaussian effect
        /// applied by this GaussianBlur9x9 effect.
        /// </summary>
        public float StandardDeviation
        {
            get
            {
                return this.standardDeviation;
            }

            set
            {
                if( value != this.standardDeviation )
                {
                    this.weightsAreDirthy = true;
                }

                this.standardDeviation = value;
            }
        }

        /// <summary>
        /// Gets or sets the amplitude of the Gaussian effect
        /// applied by this GaussianBlur9x9 effect.
        /// </summary>
        public float Amplitude
        {
            get
            {
                return this.amplitude;
            }

            set
            {
                if( value != this.amplitude )
                {
                    this.weightsAreDirthy = true;
                }

                this.amplitude = value;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the GaussianBlur9x9 class.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        /// <param name="deviceService">
        /// Provides access to the <see cref="GraphicsDevice"/>.
        /// </param>
        public GaussianBlur9x9( IEffectLoader effectLoader, IGraphicsDeviceService deviceService )
            : base( effectLoader, deviceService )
        {
        }

        /// <summary>
        /// Loads the effect used by this GaussianBlur9x9.
        /// </summary>
        /// <param name="effectLoader">
        /// Provides a mechanism that allows loading of effect assets.
        /// </param>
        protected override void LoadEffect( IEffectLoader effectLoader )
        {
            this.effect = effectLoader.Load( "GaussianBlur9x9" );
            this.techniqueH = this.effect.Techniques["GaussianBlur9x9_H"];
            this.techniqueV = this.effect.Techniques["GaussianBlur9x9_V"];
            this.Horizontal = true;

            this.parameterSourceTexture = this.effect.Parameters["SourceTexture"];
            this.parameterOffsets = this.effect.Parameters["Offsets"];
            this.parameterWeights = this.effect.Parameters["Weights"];
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Applies this GaussianBlur9x9 PostProcessEffect.
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
            
            if( this.weightsAreDirthy )
            {
                this.UpdateWeights( result );
                this.weightsAreDirthy = false;
            }
            
            this.GraphicsDevice.SetRenderTarget( result );
            this.GraphicsDevice.Clear( Xna.Color.Black );

            this.Draw( this.effect, this.effectPass );

            this.GraphicsDevice.SetRenderTarget( null );
        }

        /// <summary>
        /// Updates the weights and offsets for the given RenderTarget2D.
        /// </summary>
        /// <param name="result">
        /// The RenderTarget to which to render the result of this PostProcessEffect.
        /// </param>
        private void UpdateWeights( RenderTarget2D result )
        {
            Vector2 targetSize = GetTargetSize( result );

            if( this.horizontal )
            {
                GaussianUtilities.Fill( 
                    this.offsets,
                    1.0f / targetSize.X,
                    this.weights,
                    0,
                    this.standardDeviation,
                    this.amplitude
                );
            }
            else
            {
                GaussianUtilities.Fill( 
                    this.offsets,
                    1.0f / targetSize.Y,
                    this.weights,
                    0,
                    this.standardDeviation,
                    this.amplitude
                );
            }

            this.parameterWeights.SetValue( this.weights );
            this.parameterOffsets.SetValue( this.offsets );
        }

        /// <summary>
        /// Disposes the managed resources used by this GaussianBlur9x9 effect.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            this.parameterOffsets = null;
            this.parameterSourceTexture = null;
            this.parameterWeights = null;
            this.techniqueH = null;
            this.techniqueV = null;
            this.effectPass = null;
            this.effect = null;
        }

        /// <summary>
        /// Disposes the unmanaged resources used by this GaussianBlur9x9 effect.
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
        /// The effect that contain the actual blurring logic.
        /// </summary>
        private Effect effect;

        /// <summary>
        /// The horizontal blurring technique.
        /// </summary>
        private EffectTechnique techniqueH;

        /// <summary>
        /// The vertical blurring technique.
        /// </summary>
        private EffectTechnique techniqueV;

        /// <summary>
        /// The cached first pass of the currently selected technique.
        /// </summary>
        private EffectPass effectPass;

        /// <summary>
        /// The parameter used to set the soruce texture.
        /// </summary>
        private EffectParameter parameterSourceTexture;

        /// <summary>
        /// The parameter used to set the blurring offsets.
        /// </summary>
        private EffectParameter parameterOffsets;

        /// <summary>
        /// The parameter used to set the blurring weights.
        /// </summary>
        private EffectParameter parameterWeights;

        /// <summary>
        /// The current standard deviation used in the blurring calculations.
        /// </summary>
        private float standardDeviation = 0.8f;

        /// <summary>
        /// The current blurring amplitude.
        /// </summary>
        private float amplitude = 0.4f;

        /// <summary>
        /// States whether the image is currently blurred horizontally.
        /// </summary>
        private bool horizontal;

        /// <summary>
        /// States whether the weights are dirty and must be updated.
        /// </summary>
        private bool weightsAreDirthy = true;

        /// <summary>
        /// The blurring weights that have been calculated for the current settings.
        /// </summary>
        private readonly float[] weights = new float[9];

        /// <summary>
        /// The blurring offsets that have been calculated.
        /// </summary>
        private readonly float[] offsets = new float[9];

        #endregion
    }
}
