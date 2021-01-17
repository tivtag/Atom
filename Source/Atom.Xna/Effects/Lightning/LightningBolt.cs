// <copyright file="LightningBolt.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.Lightning.LightningBolt class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Effects.Lightning
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using Atom.Xna.Effects.PostProcess;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents a 2D lightning-bolt graphics effects.
    /// </summary>
    public class LightningBolt 
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the source position of this lightning.
        /// </summary>
        public Atom.Math.Vector3 Source
        {
            get
            {
                return this.source.ToAtom();
            }

            set
            {
                this.source = value.ToXna(); 
            }
        }

        /// <summary>
        /// Gets or sets the desitination position of this lightning.
        /// </summary>
        public Atom.Math.Vector3 Destination
        {
            get
            {
                return this.destination.ToAtom(); 
            }
            
            set 
            {
                this.destination = value.ToXna();
            }
        }

        /// <summary>
        /// Gets and resolves the lightning that has been rendered.
        /// </summary>
        public Texture2D ResolveTexture()
        {
            return this.lightningTarget;
        }

        /// <summary>
        /// Gets or sets settings that applied to this LightningBolt.
        /// </summary>
        public LightningSettings Settings
        {
            get
            {
                return this.settings;
            }

            set 
            {
                this.settings = value;
            }
        }

        /// <summary>
        /// Gets the lines that the bolt consists of.
        /// </summary>
        public IEnumerable<VirtualLine> VirtualLines
        {
            get
            {
                return this.virtualLines;
            }
        }

        /// <summary>
        /// Gets the length of a fork arm.
        /// </summary>
        private float ForkArmLength
        {
            get
            {
                return this.settings.ForkLengthPercentage * Vector3.Distance( this.source, this.destination );
            }
        }

        /// <summary>
        /// Gets or sets the view matrix that is used when rendering the texture of the LightningBolt.
        /// </summary>
        public Matrix View
        {
            get
            {
                return this.effect.Parameters["View"].GetValueMatrix();
            }

            set
            {
                this.effect.Parameters["View"].SetValue( value );
            }
        }

        /// <summary>
        /// Gets or sets the world matrix that is used when rendering the texture of the LightningBolt.
        /// </summary>
        public Matrix World
        {
            get
            {
                return this.effect.Parameters["World"].GetValueMatrix();
            }

            set
            {
                this.effect.Parameters["World"].SetValue( value );
            }
        }

        /// <summary>
        /// Gets or sets the projection matrix that is used when rendering the texture of the LightningBolt.
        /// </summary>
        public Matrix Projection
        {
            get
            {
                return this.effect.Parameters["Projection"].GetValueMatrix();
            }

            set
            {
                this.effect.Parameters["Projection"].SetValue( value );
            }
        }

        /// <summary>
        /// Gets a value indicating whether this effect is supported on the current hardware.
        /// </summary>
        public bool IsSupported
        {
            get;
            private set;
        }

        #endregion

        #region [ Types ]

        /// <summary>
        /// Keeps track of a single lightning segment.
        /// </summary>
        public struct VirtualLine
        {
            /// <summary>
            /// The index of the first vertex.
            /// </summary>
            public short v0;
            
            /// <summary>
            /// The index of the second vertex.
            /// </summary>
            public short v1;
            
            /// <summary>
            /// The index of the third vertex.
            /// </summary>
            public short v2;
            
            /// <summary>
            /// The index of the fourth vertex.
            /// </summary>
            public short v3;

            /// <summary>
            /// The fork/sub division depth.
            /// </summary>
            public int WidthLevel;
        }

        /// <summary>
        /// Keeps track of a single lightning vertex.
        /// </summary>
        private struct VirtualPoint
        {
            public short v0;
            public short v1;
            public short v2;
            public short v3;
            public int WidthLevel;
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="settings"></param>
        /// <param name="glow"></param>
        /// <param name="rand">
        /// A random number generator.
        /// </param>
        public LightningBolt( Atom.Math.Vector3 source, Atom.Math.Vector3 destination, LightningSettings settings, Glow glow, Atom.Math.IRand rand )
        {
            Contract.Requires<ArgumentNullException>( rand != null );
            Contract.Requires<ArgumentNullException>( settings != null );

            this.glow = glow;
            this.rand = rand;
            this.settings = settings;
            this.topology = settings.Topology;
                        
            int lineCount = ComputeLineCount( 0 );
            int pointCount = ComputePointCount( 0 );

            this.virtualLines = new VirtualLine[lineCount];
            this.virtualPoints = new VirtualPoint[pointCount];            
            this.indices = new short[lineCount * 6 + pointCount * 6];
            this.realVertexCount = lineCount * 4 + pointCount * 4;
            this.lightningPoints = new LightningVertex[realVertexCount];

            for( int i = 0; i < this.realVertexCount; i++ )
            {
                this.lightningPoints[i] = new LightningVertex();
            }

            this.source = source.ToXna();
            this.destination = destination.ToXna();

            this.totalRealVertices = 0;
            this.totalPointIndex = 0;
            this.AddPoint( 1 );
            this.AddPoint( 1 );
            this.BuildIndices( 0, 0, 1 );
        }

        /// <summary>
        /// Loads this LightningBolt.
        /// </summary>
        /// <param name="effectLoader"></param>
        /// <param name="renderTargetFactory"></param>
        /// <param name="device"></param>
        public void LoadContent( IEffectLoader effectLoader, IRenderTarget2DFactory renderTargetFactory, GraphicsDevice device )
        {
            Contract.Requires<ArgumentNullException>( effectLoader != null );
            Contract.Requires<ArgumentNullException>( renderTargetFactory != null );
            Contract.Requires<ArgumentNullException>( device != null );

            this.effect = effectLoader.Load( "LightningBolt" );
            this.effectPass = this.effect.CurrentTechnique.Passes[0];

            this.lightningTarget = renderTargetFactory.Create();

            this.View = Xna.Matrix.CreateLookAt( new Vector3( 0, 0, 1 ), new Vector3( 0, 0, 0 ), new Vector3( 0, 1, 0 ) );
            this.World = Xna.Matrix.Identity;
            this.Projection = Xna.Matrix.CreateOrthographic( this.lightningTarget.Width, this.lightningTarget.Height, 0.01f, 50 );

            this.blendState = new BlendState() {
                AlphaDestinationBlend = Blend.One,
                ColorDestinationBlend = Blend.One,
                ColorBlendFunction = BlendFunction.Max,
                AlphaBlendFunction = BlendFunction.Max,
            };
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Generates the lightning as drawn in the scene.
        /// </summary>
        public void GenerateTexture( IXnaDrawContext drawContext )
        {
            if( !this.IsSupported )
                return;

            var device = drawContext.Device;
            var oldRenderTarget = device.GetRenderTarget2D();

            device.SetRenderTarget( this.lightningTarget );
            device.Clear( ClearOptions.Target, Color.Transparent, 1.0f, 0 );
            
            this.effect.Parameters["StartColor"].SetValue( settings.InteriorColor.ToVector3() );
            this.effect.Parameters["EndColor"].SetValue( settings.ExteriorColor.ToVector3() );
            
            var oldRasterState = device.RasterizerState;
            var oldBlendState = device.BlendState;

            device.BlendState = this.blendState;            
            device.RasterizerState = RasterizerState.CullNone;                 

            this.effectPass.Apply();
            {
                device.DrawUserIndexedPrimitives<LightningVertex>(
                    PrimitiveType.TriangleList,
                    lightningPoints, 
                    0,
                    lightningPoints.Length,
                    indices, 
                    0,
                    indices.Length / 3
                );
            }

            device.SetRenderTarget( oldRenderTarget );

            if( this.settings.IsGlowEnabled && this.glow != null )
            {
                this.glow.Strength = this.settings.GlowIntensity;
                this.glow.PostProcess( this.lightningTarget, this.lightningTarget, drawContext );
            }

            device.BlendState = oldBlendState;
            device.RasterizerState = oldRasterState;
        }

        /// <summary>
        /// Updates the animation of this LightningBolt.
        /// </summary>
        public void Update( IUpdateContext updateContext )
        {
            if( this.settings.FrameLength == 0.0f )
                return;

            if( this.settings.FrameLength == -1.0f )
            {
                this.Animate();
                return;
            }

            this.timeSinceAnimation += updateContext.FrameTime;
            if( this.timeSinceAnimation > this.settings.FrameLength )
            {
                this.timeSinceAnimation -= this.settings.FrameLength;
                this.Animate();
            }
        }

        /// <summary>
        /// Ranodimzes this LightningBolt, taken the current settings and positions.
        /// </summary>
        public void Animate()
        {
            this.totalPointIndex = 0;
            this.SetPointPositions( this.source );
            this.SetPointPositions( this.destination );
            this.BuildVertices( 0, this.source, this.destination, 0 );
        }

        private int ComputeLineCount( int level )
        {
            if( level == this.topology.Count - 1 )
            {
                if( this.topology[level] == LightningSubdivisionOp.Jitter )
                    return 2;
                else
                    return 3;
            }

            if( this.topology[level] == LightningSubdivisionOp.Jitter )
                return 2 * this.ComputeLineCount( level + 1 );
            else
                return 3 * this.ComputeLineCount( level + 1 );
        }

        private int ComputePointCount( int level )
        {
            if( level == this.topology.Count - 1 )
            {
                if( this.topology[level] == LightningSubdivisionOp.Jitter )
                    return 3;
                else
                    return 4;
            }

            if( this.topology[level] == LightningSubdivisionOp.Jitter )
                return 2 * this.ComputePointCount( level + 1 ) - 1;
            else
                return 3 * this.ComputePointCount( level + 1 ) - 2;
        }

        private static float Decay( float amount, int level, float decayRate )
        {
            return amount * (float)Math.Pow( decayRate, level );
        }
        
        private static Vector3 GetLeft( Vector3 forward )
        {
            return Vector3.Normalize( Vector3.Transform( forward, Matrix.CreateRotationZ( MathHelper.PiOver2 ) ) );
        }

        private Vector3 GetJittered( Vector3 start, Vector3 end, Vector3 forward, Vector3 left, int level )
        {
            Vector2 delta = 
                Decay( settings.JitterDeviationRadius, level, settings.JitterDecayRate ) *
                new Vector2( 
                     settings.JitterForwardDeviation.GetRandomValue( this.rand ),
                     settings.JitterLeftDeviation.GetRandomValue( this.rand ) 
                );

            return Vector3.Lerp( start, end, this.settings.SubdivisionFraction.GetRandomValue( this.rand ) ) + delta.X * forward + delta.Y * left;
        }

        private Vector3 GetForkDelta( Vector3 forward, Vector3 left, int level )
        {
            Vector2 forkDelta = 
                Decay( ForkArmLength, level, settings.ForkDecayRate ) *
                new Vector2( this.settings.ForkForwardDeviation.GetRandomValue( this.rand ), this.settings.ForkLeftDeviation.GetRandomValue( this.rand ) );

            return forkDelta.X * forward + forkDelta.Y * left;
        }

        private int BuildVertices( int level, Vector3 start, Vector3 end, int virtualLineIndex )
        {
            if( level == this.topology.Count )
            {
                this.SetLinePositions( virtualLineIndex, start, end );
                return virtualLineIndex + 1;
            }
            
            switch( this.topology[level] )
            {
                case LightningSubdivisionOp.Jitter:
                    return this.JitterStep( level, start, end, virtualLineIndex );

                case LightningSubdivisionOp.JitterAndFork:
                    return this.ForkStep( level, start, end, virtualLineIndex );

                default:
                    return virtualLineIndex;
            }
        }

        private int JitterStep( int level, Vector3 start, Vector3 end, int virtualLineIndex )
        {
            Vector3 forward  = Vector3.Normalize( end - start );
            Vector3 left     = GetLeft( forward );
            Vector3 jittered = GetJittered( start, end, forward, left, level );

            this.SetPointPositions( jittered );

            int lastLineIndex;
            lastLineIndex = BuildVertices( level + 1, start, jittered, virtualLineIndex );
            lastLineIndex = BuildVertices( level + 1, jittered, end, lastLineIndex );

            return lastLineIndex;
        }

        private int ForkStep( int level, Vector3 start, Vector3 end, int virtualLineIndex )
        {
            Vector3 forward  = Vector3.Normalize( end - start );
            Vector3 left     = GetLeft( forward );
            Vector3 jittered = GetJittered( start, end, forward, left, level );
            Vector3 forked   = jittered + GetForkDelta( forward, left, level );

            this.SetPointPositions( jittered );
            this.SetPointPositions( forked );

            int lastLineIndex;
            lastLineIndex = this.BuildVertices( level + 1, start, jittered, virtualLineIndex );
            lastLineIndex = this.BuildVertices( level + 1, jittered, forked, lastLineIndex );
            lastLineIndex = this.BuildVertices( level + 1, jittered, end, lastLineIndex );

            return lastLineIndex;
        }

        private float ComputeWidth( int widthLevel )
        {
            if( this.settings.IsWidthDecreasing )
                return this.settings.BaseWidth / widthLevel;
            else
                return this.settings.BaseWidth;
        }

        private void SetLinePositions( int virtualLineIndex, Vector3 start, Vector3 end )
        {
            VirtualLine line = this.virtualLines[virtualLineIndex]; 
            Vector3 forward  = Vector3.Normalize( end - start );
            Vector3 left     = GetLeft( forward );
            float width      = ComputeWidth( line.WidthLevel );

            this.lightningPoints[line.v0].Position = start + left * width;
            this.lightningPoints[line.v1].Position = end + left * width;
            this.lightningPoints[line.v2].Position = end - left * width;
            this.lightningPoints[line.v3].Position = start - left * width;
        }

        private void SetPointPositions( Vector3 position )
        {
            VirtualPoint point = this.virtualPoints[this.totalPointIndex];
            float width = ComputeWidth( point.WidthLevel );

            this.lightningPoints[point.v0].Position = position + width * new Vector3( -1, -1, 0 );
            this.lightningPoints[point.v1].Position = position + width * new Vector3( 1, -1, 0 );
            this.lightningPoints[point.v2].Position = position + width * new Vector3( 1, 1, 0 );
            this.lightningPoints[point.v3].Position = position + width * new Vector3( -1, 1, 0 );

            ++totalPointIndex;
        }

        private void AddPoint( int width )
        {
            var point = new VirtualPoint() {
                v0 = totalRealVertices,
                v1 = (short)(totalRealVertices + 1),
                v2 = (short)(totalRealVertices + 2),
                v3 = (short)(totalRealVertices + 3),
                WidthLevel = width
            };

            this.virtualPoints[this.totalPointIndex] = point;
            
            this.lightningPoints[point.v0].TextureCoordinates = new Vector2( 0, 0 );
            this.lightningPoints[point.v0].ColorGradient = new Vector2( -1, 1 );
            this.lightningPoints[point.v1].TextureCoordinates = new Vector2( 1, 0 );
            this.lightningPoints[point.v1].ColorGradient = new Vector2( 1, 1 );
            this.lightningPoints[point.v2].TextureCoordinates = new Vector2( 1, 1 );
            this.lightningPoints[point.v2].ColorGradient = new Vector2( 1, -1 );
            this.lightningPoints[point.v3].TextureCoordinates = new Vector2( 0, 1 );
            this.lightningPoints[point.v3].ColorGradient = new Vector2( -1, -1 );
            
            this.indices[totalIndices]     = point.v0;
            this.indices[totalIndices + 1] = point.v1;
            this.indices[totalIndices + 2] = point.v2;

            this.indices[totalIndices + 3] = point.v0;
            this.indices[totalIndices + 4] = point.v2;
            this.indices[totalIndices + 5] = point.v3;
            
            this.totalRealVertices += 4;
            this.totalIndices += 6;
            ++this.totalPointIndex;
        }

        private void AddLine( int virtualLineIndex, int width )
        {
            var line = new VirtualLine()  {
                v0 = totalRealVertices,
                v1 = (short)(totalRealVertices + 1),
                v2 = (short)(totalRealVertices + 2),
                v3 = (short)(totalRealVertices + 3),
                WidthLevel = width
            };

            this.virtualLines[virtualLineIndex] = line;
            
            this.lightningPoints[line.v0].TextureCoordinates = new Vector2( 0, 0 );
            this.lightningPoints[line.v0].ColorGradient = new Vector2( 1, 0 );
            this.lightningPoints[line.v1].TextureCoordinates = new Vector2( 1, 0 );
            this.lightningPoints[line.v1].ColorGradient = new Vector2( 1, 0 );
            this.lightningPoints[line.v2].TextureCoordinates = new Vector2( 1, 1 );
            this.lightningPoints[line.v2].ColorGradient = new Vector2( -1, 0 );
            this.lightningPoints[line.v3].TextureCoordinates = new Vector2( 0, 1 );
            this.lightningPoints[line.v3].ColorGradient = new Vector2( -1, 0 );
            
            this.indices[totalIndices]     = line.v0;
            this.indices[totalIndices + 1] = line.v1;
            this.indices[totalIndices + 2] = line.v2;            
            this.indices[totalIndices + 3] = line.v0;
            this.indices[totalIndices + 4] = line.v2;
            this.indices[totalIndices + 5] = line.v3;
            
            this.totalRealVertices += 4;
            this.totalIndices += 6;
        }

        private int BuildIndices( int level, int lineIndex, int width )
        {
            if( level == this.topology.Count )
            {
                this.AddLine( lineIndex, width );
                return lineIndex + 1;
            }

            int lastLineIndex = 0;

            switch( this.topology[level] )
            {
                case LightningSubdivisionOp.Jitter:
                    this.AddPoint( width );
                    lastLineIndex = this.BuildIndices( level + 1, lineIndex, width );
                    lastLineIndex = this.BuildIndices( level + 1, lastLineIndex, width );
                    break;

                case LightningSubdivisionOp.JitterAndFork:
                    this.AddPoint( width );
                    this.AddPoint( width + 1 );
                    lastLineIndex = this.BuildIndices( level + 1, lineIndex, width );
                    lastLineIndex = this.BuildIndices( level + 1, lastLineIndex, width + 1 );
                    lastLineIndex = this.BuildIndices( level + 1, lastLineIndex, width );
                    break;

                default:
                    break;
            }

            return lastLineIndex;
        }
        
        #endregion

        #region [ Fields ]

        private Vector3 source;
        private Vector3 destination;
        private RenderTarget2D lightningTarget;
        private LightningVertex[] lightningPoints;
        private short[] indices;
        private VirtualLine[] virtualLines;
        private VirtualPoint[] virtualPoints;

        private int totalPointIndex;
        private int realVertexCount;
        private short totalRealVertices;
        private int totalIndices;
        
        private LightningSettings settings;
        private List<LightningSubdivisionOp> topology;
        private double timeSinceAnimation;

        private Effect effect;
        private EffectPass effectPass;

        private readonly Atom.Math.IRand rand;
        private readonly Glow glow;
        private BlendState blendState;

        #endregion
    }
}
