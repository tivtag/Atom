// <copyright file="LightningDescriptor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.Lightning.LightningDescriptor class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>


namespace Atom.Xna.Effects.Lightning
{
    using System.Collections.Generic;
    using Atom.Math;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Descripes the properties of a Lightning Bolt.
    /// </summary>
    public sealed class LightningSettings
    {
        /// <summary>
        /// Gets a new LightningDescriptor that has default settings.
        /// </summary>
        public static LightningSettings Default
        {
            get
            {
                var ld = new LightningSettings();
                ld.topology.Add( LightningSubdivisionOp.JitterAndFork );
                ld.topology.Add( LightningSubdivisionOp.JitterAndFork );
                ld.topology.Add( LightningSubdivisionOp.Jitter );
                ld.topology.Add( LightningSubdivisionOp.Jitter );
                ld.topology.Add( LightningSubdivisionOp.JitterAndFork );
                return ld;
            }
        }

        /// <summary>
        /// Gets a new LightningDescriptor that has the settings for an electricity bolt.
        /// </summary>
        public static LightningSettings ElectricityBolt
        {
            get
            {
                var ld = new LightningSettings();
                ld.ForkDecayRate = 1.0f;
                ld.ForkLengthPercentage = 0.2f;
                ld.BaseWidth = 0.3f;
                ld.ExteriorColor = Xna.Color.Navy;
                ld.ForkForwardDeviation = new FloatRange( -1, 1 );
                ld.ForkLeftDeviation = new FloatRange( -1, 1 );
                ld.ForkLengthPercentage = 0.2f;
                ld.GlowIntensity = 1.5f;
                ld.InteriorColor = Xna.Color.White;
                ld.IsGlowEnabled = true;
                ld.IsWidthDecreasing = false;
                ld.JitterDecayRate = 0.5f;
                ld.SubdivisionFraction = new FloatRange( 0.45f, 0.55f );
                ld.Topology.Add( LightningSubdivisionOp.Jitter );
                ld.Topology.Add( LightningSubdivisionOp.Jitter );
                ld.Topology.Add( LightningSubdivisionOp.Jitter );
                ld.Topology.Add( LightningSubdivisionOp.Jitter );
                ld.Topology.Add( LightningSubdivisionOp.Jitter );
                ld.Topology.Add( LightningSubdivisionOp.Jitter );
                return ld;
            }
        }

        /// <summary>
        /// Gets a new LightningDescriptor that has example settings.
        /// </summary>
        public static LightningSettings ExamplePreset
        {
            get
            {
                var ld = new LightningSettings();
                ld.BaseWidth = 0.3f;
                ld.ExteriorColor = Xna.Color.Red;
                ld.ForkDecayRate = 0.7f;
                ld.ForkForwardDeviation = new FloatRange( -1, 1 );
                ld.ForkLeftDeviation = new FloatRange( -1, 1 );
                ld.ForkLengthPercentage = 0.2f;
                ld.GlowIntensity = 2.5f;
                ld.InteriorColor = Xna.Color.Green;
                ld.IsGlowEnabled = true;
                ld.IsWidthDecreasing = false;
                ld.JitterDeviationRadius = 0.4f;
                ld.JitterDecayRate = 1.0f;
                ld.SubdivisionFraction = new FloatRange( 0.2f, 0.8f );
                ld.FrameLength = 20.0f;
                ld.Topology.Add( LightningSubdivisionOp.Jitter );
                ld.Topology.Add( LightningSubdivisionOp.Jitter );
                ld.Topology.Add( LightningSubdivisionOp.Jitter );
                ld.Topology.Add( LightningSubdivisionOp.Jitter );
                ld.Topology.Add( LightningSubdivisionOp.JitterAndFork );
                ld.Topology.Add( LightningSubdivisionOp.Jitter );
                ld.Topology.Add( LightningSubdivisionOp.Jitter );

                return ld;
            }
        }
        
        /// <summary>
        /// Gets or sets the sub-division operation to apply when creating the Lightning.
        /// </summary>
        public List<LightningSubdivisionOp> Topology
        {
            get { return topology; }
            set { topology = value; }
        }

        /// <summary>
        /// Gets or sets the length a frame lasts; before a new one is generated.
        /// </summary>
        public float FrameLength
        {
            get { return animationFramerate; }
            set { animationFramerate = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating how strong the lightning should glow.
        /// </summary>
        public float GlowIntensity
        {
            get { return glowIntensity; }
            set { glowIntensity = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the lightning should glow.
        /// </summary>
        public bool IsGlowEnabled
        {
            get { return isGlowEnabled; }
            set { isGlowEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the width of the beam should decrease over time.
        /// </summary>
        public bool IsWidthDecreasing
        {
            get { return isWidthDecreasing; }
            set { isWidthDecreasing = value; }
        }

        /// <summary>
        /// Gets or sets the width the beam starts at.
        /// </summary>
        public float BaseWidth
        {
            get { return baseWidth; }
            set { baseWidth = value; }
        }

        /// <summary>
        /// Gets or sets the exterior color of the beam.
        /// </summary>
        public Xna.Color ExteriorColor
        {
            get { return exteriorColor; }
            set { exteriorColor = value; }
        }

        /// <summary>
        /// Gets or sets the interior color of the beam.
        /// </summary>
        public Xna.Color InteriorColor
        {
            get { return interiorColor; }
            set { interiorColor = value; }
        }

        /// <summary>
        /// Gets or sets the range of values that is used when creating a new jitter.
        /// </summary>
        public FloatRange SubdivisionFraction
        {
            get { return subdivisionFraction; }
            set { subdivisionFraction = value; }
        }

        /// <summary>
        /// Gets or sets the range of values in which the beam can jitter forward.
        /// </summary>
        public FloatRange JitterForwardDeviation
        {
            get { return jitterForwardDeviation; }
            set { jitterForwardDeviation = value; }
        }

        /// <summary>
        /// Gets or sets the range of values in which the beam can jitter left.
        /// </summary>
        public FloatRange JitterLeftDeviation
        {
            get { return jitterLeftDeviation; }
            set { jitterLeftDeviation = value; }
        }

        /// <summary>
        /// Gets or sets the radius in which the beam jitters.
        /// </summary>
        public float JitterDeviationRadius
        {
            get { return jitterDeviationRadius; }
            set { jitterDeviationRadius = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating how fast the jitter gets weaker.
        /// </summary>
        public float JitterDecayRate
        {
            get { return jitterDecayRate; }
            set { jitterDecayRate = value; }
        }

        /// <summary>
        /// Gets or sets the length of a fork arm as a percentage of the forked beam.
        /// </summary>
        public float ForkLengthPercentage
        {
            get { return forkLengthPercentage; }
            set { forkLengthPercentage = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating how fast the fork gets weaker.
        /// </summary>
        public float ForkDecayRate
        {
            get { return forkDecayRate; }
            set { forkDecayRate = value; }
        }

        /// <summary>
        /// Gets or sets the range of values in which the beam can fork fordward.
        /// </summary>
        public FloatRange ForkForwardDeviation
        {
            get { return forkForwardDeviation; }
            set { forkForwardDeviation = value; }
        }

        /// <summary>
        /// Gets or sets the range of values in which the beam can fork left.
        /// </summary>
        public FloatRange ForkLeftDeviation
        {
            get { return forkLeftDeviation; }
            set { forkLeftDeviation = value; }
        }

        private FloatRange subdivisionFraction = new FloatRange( 0.45f, 0.55f );
        private FloatRange jitterForwardDeviation= new FloatRange( -1.0f, 1.0f );
        private FloatRange jitterLeftDeviation = new FloatRange( -1.0f, 1.0f );
        private float jitterDeviationRadius = 3.0f;
        private float jitterDecayRate = 0.6f;
        private float forkLengthPercentage = 0.5f;
        private float forkDecayRate = 0.5f;
        private FloatRange forkForwardDeviation = new FloatRange( 0.0f, 1.0f );
        private FloatRange forkLeftDeviation = new FloatRange( -1.0f, 1.0f );
        private Xna.Color interiorColor = Xna.Color.White;
        private Xna.Color exteriorColor = Xna.Color.Blue;
        private float baseWidth = 0.6f;
        private bool isWidthDecreasing = true;
        private bool isGlowEnabled = true;
        private float glowIntensity = 0.5f;
        private float animationFramerate = -1.0f;
        private List<LightningSubdivisionOp> topology = new List<LightningSubdivisionOp>();
    }
}
