// <copyright file="BloomSettings.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Xna.Effects.PostProcess.BloomSettings class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Xna.Effects.PostProcess
{
    /// <summary>
    /// Holds the settings used to tweak the <see cref="Bloom"/> effect.
    /// </summary>
    public sealed class BloomSettings
    {
        #region [ Fields ]

        /// <summary>
        /// Gets or sets the name of this BloomSettings instance.
        /// </summary>
        public string Name 
        {
            get;
            set; 
        }
        
        /// <summary>
        /// Controls how bright a pixel needs to be before it will bloom.
        /// Zero makes everything bloom equally, while higher values select
        /// only brighter colors. Somewhere between 0.25 and 0.5 is good.
        /// </summary>
        public float BloomThreshold;
        
        /// <summary>
        /// Controls how much blurring is applied to the bloom image.
        /// The typical range is from 1 up to 10 or so.
        /// </summary>
        public float BlurAmount;

        /// <summary>
        /// Controls the amount of the bloom image that will be mixed 
        /// into the final scene. Range 0 to 1.
        /// </summary>
        public float BloomIntensity;
        
        /// <summary>
        /// Controls the amount of the base image that will be mixed
        /// into the final scene. Range 0 to 1.
        /// </summary>
        public float BaseIntensity;

        /// <summary>
        /// Independently control the color saturation of the bloom image. 
        /// Zero is totally desaturated, 1.0 leaves saturation
        /// unchanged, while higher values increase the saturation level.
        /// </summary>
        public float BloomSaturation;
        
        /// <summary>
        /// Independently control the color saturation of the base image.
        /// Zero is totally desaturated, 1.0 leaves saturation
        /// unchanged, while higher values increase the saturation level.
        /// </summary>
        public float BaseSaturation;

        #endregion

        #region [ Presets ]

        /// <summary>
        /// Gets the default pre-set BloomSettings.
        /// </summary>
        public static BloomSettings Default
        {
            get
            {
                return new BloomSettings( "Default", 0.25f, 4, 1.25f, 1, 1, 1 );
            }
        }

        /// <summary>
        /// Gets the soft pre-set BloomSettings.
        /// </summary>
        public static BloomSettings Soft
        {
            get
            {
                return new BloomSettings( "Soft", 0, 3, 1, 1, 1, 1 );
            }
        }

        /// <summary>
        /// Gets the desaturated pre-set BloomSettings.
        /// </summary>
        public static BloomSettings Desaturated
        {
            get
            {
                return new BloomSettings( "Desaturated", 0.5f, 8, 2, 1, 0, 1 );
            }
        }

        /// <summary>
        /// Gets the saturated pre-set BloomSettings.
        /// </summary>
        public static BloomSettings Saturated
        {
            get
            {
                return new BloomSettings( "Saturated", 0.25f, 4, 2, 1, 2, 0 );
            }
        }

        /// <summary>
        /// Gets the blurry pre-set BloomSettings.
        /// </summary>
        public static BloomSettings Blurry
        {
            get
            {
                return new BloomSettings( "Blurry", 0, 2, 1, 0.1f, 1, 1 );
            }
        }

        /// <summary>
        /// Gets the subtle pre-set BloomSettings.
        /// </summary>
        public static BloomSettings Subtle
        {
            get
            {
                return new BloomSettings( "Subtle", 0.5f, 2, 1, 1, 1, 1 );
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the BloomSettings class.
        /// </summary>
        /// <param name="name">
        /// The name of the new BloomSettings instance.
        /// </param>
        /// <param name="bloomThreshold">
        /// Controls how bright a pixel needs to be before it will bloom.
        /// Zero makes everything bloom equally, while higher values select
        /// only brighter colors. Somewhere between 0.25 and 0.5 is good.
        /// </param>
        /// <param name="blurAmount">
        /// Controls how much blurring is applied to the bloom image.
        /// The typical range is from 1 up to 10 or so.
        /// </param>
        /// <param name="bloomIntensity">
        /// Controls the amount of the bloom image that will be mixed 
        /// into the final scene. Range 0 to 1.
        /// </param>
        /// <param name="baseIntensity">
        /// Controls the amount of the base image that will be mixed
        /// into the final scene. Range 0 to 1.
        /// </param>
        /// <param name="bloomSaturation">
        /// Independently control the color saturation of the bloom image. 
        /// Zero is totally desaturated, 1.0 leaves saturation
        /// unchanged, while higher values increase the saturation level.
        /// </param>
        /// <param name="baseSaturation">
        /// Independently control the color saturation of the base image.
        /// Zero is totally desaturated, 1.0 leaves saturation
        /// unchanged, while higher values increase the saturation level.
        /// </param>
        public BloomSettings( 
            string name,
            float bloomThreshold,
            float blurAmount,
            float bloomIntensity,
            float baseIntensity,
            float bloomSaturation,
            float baseSaturation )
        {
            this.Name = name;
            this.BloomThreshold = bloomThreshold;
            this.BlurAmount = blurAmount;
            this.BloomIntensity = bloomIntensity;
            this.BaseIntensity = baseIntensity;
            this.BloomSaturation = bloomSaturation;
            this.BaseSaturation = baseSaturation;
        }

        /// <summary>
        /// Overwritten to return a human-readable description of this BloomSettings instance.
        /// </summary>
        /// <returns>
        /// The various properties of this BloomSettings instance.
        /// </returns>
        public override string ToString()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            
            return string.Format(
                culture,
                "Threshold={0}, BlurAmount={1}, Intensity={2}, Saturation={3}, BaseIntensity={4}, BaseSaturation={5}",
                this.BloomThreshold.ToString( culture ),
                this.BlurAmount.ToString( culture ),
                this.BloomIntensity.ToString( culture ),
                this.BloomSaturation.ToString( culture ),
                this.BaseIntensity.ToString( culture ),
                this.BaseSaturation.ToString( culture )
            );
        }
    }
}
