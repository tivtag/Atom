using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Atom.Xna.Effects;
using Atom.Xna.Effects.PostProcess;
using Atom.Xna.Effects.Lightning;

namespace Atom.Xna.Effects.HighDynamicRangeTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Atom.Xna.Effects.PostProcess.NewHDR.HighDynamicRange hdr;
        Lightning.LightningBolt bolt, bolt2, bolt3, bolt4;

        Texture2D texture;
        Texture2DLoader textureLoader;
        bool hdrEnabled = true;
        LightMap lightMap;

        Texture2D lightTexture;
        RenderTarget2D renderTarget;
        Atom.Math.IRand rand = new Atom.Math.RandMT();


        private LightningBolt[] bolts = new LightningBolt[4];



        public Game1()
        {
            IsMouseVisible = true;
            this.IsFixedTimeStep = false;
           

            graphics = new GraphicsDeviceManager( this );
            graphics.PreparingDeviceSettings +=new EventHandler<PreparingDeviceSettingsEventArgs>( graphics_PreparingDeviceSettings );
            graphics.SynchronizeWithVerticalRetrace=false;

            this.textureLoader = new Texture2DLoader( new ContentManager( this.Services, "Content/Textures/" ) );
            renderTargetFactory = new RenderTarget2DFactory(
                new Atom.Math.Point2( 800, 600 ),
                this.graphics
            );

            effectLoader = EffectLoader.Create( this.Services );

            this.lightMap = new LightMap( renderTargetFactory, this.graphics );

            //this.luminance = new Luminance( EffectLoader.Create( this.Services ), this.graphics );
            //hdr = new Atom.Xna.Effects.PostProcess.NewHDR.HighDynamicRange( EffectLoader.Create( this.Services ), this.graphics );
            
            settings = new LightningSettings();
            settings.FrameLength = 0.1f;
            settings.IsGlowEnabled = false;
            settings.BaseWidth = 3;
            settings.JitterDeviationRadius = 30.0f;

            settings.Topology.Clear();
            settings.Topology.Add( LightningSubdivisionOp.Jitter );
            settings.Topology.Add( LightningSubdivisionOp.JitterAndFork );
            settings.Topology.Add( LightningSubdivisionOp.JitterAndFork );
            settings.Topology.Add( LightningSubdivisionOp.Jitter );
            settings.Topology.Add( LightningSubdivisionOp.Jitter );
            settings.Topology.Add( LightningSubdivisionOp.JitterAndFork );
            settings.Topology.Add( LightningSubdivisionOp.Jitter );
            settings.Topology.Add( LightningSubdivisionOp.JitterAndFork );
            settings.Topology.Add( LightningSubdivisionOp.Jitter );

            settings.InteriorColor = Color.White;
            settings.ExteriorColor = Color.Red;
            
            glow = new Glow( effectLoader, renderTargetFactory, graphics );

            bolt = new Lightning.LightningBolt( 
                new Atom.Math.Vector3( 200.0f, 0.0f, 0.0f ),
                new Atom.Math.Vector3( 200, 300, 0.0f ), settings, glow, rand );

            bolt2 = new Lightning.LightningBolt(
                new Atom.Math.Vector3( 0.0f, 0.0f, 0.0f ),
                new Atom.Math.Vector3( 400, 400, 0.0f ), settings, glow, rand );

            bolt3 = new Lightning.LightningBolt(
                new Atom.Math.Vector3( 0.0f, 0.0f, 0.0f ),
                new Atom.Math.Vector3( 400, 400, 0.0f ), settings, glow, rand );

            bolt4 = new Lightning.LightningBolt(
                new Atom.Math.Vector3( 0.0f, 0.0f, 0.0f ),
                new Atom.Math.Vector3( 400, 400, 0.0f ), settings, glow, rand );

            bolts[0] = bolt;
            bolts[1] = bolt2;
            bolts[2] = bolt3;
            bolts[3] = bolt4;

            updateContext = new XnaUpdateContext();
        }

        void graphics_PreparingDeviceSettings( object sender, PreparingDeviceSettingsEventArgs e )
        {
           var pp = e.GraphicsDeviceInformation.PresentationParameters;

           pp.BackBufferFormat = SurfaceFormat.Color;
           pp.EnableAutoDepthStencil = false;
           pp.RenderTargetUsage = RenderTargetUsage.PreserveContents;

           pp.BackBufferWidth = 800;
           pp.BackBufferHeight = 600;
        } 

        protected override void Initialize()
        {
            base.Initialize();
        }


        private Atom.Math.Vector3 position = new Math.Vector3( 4, 4, 0 );
        private float length = 200.0f;
        private float jitter = 55.0f;

        private void UpdatePositions()
        {
            MouseState ms = Mouse.GetState();

            //position.X = ms.X - 400;
            //position.Y = -ms.Y + 300;

            rect.X = ms.X;
            rect.Y = ms.Y;

            line = new Math.FastLineSegment2( new Math.Vector2( ms.X, ms.Y ), new Math.Vector2( 100, 100 ) );


            bolt.Source = position;
            bolt.Destination = position + new Math.Vector3( length, jitter * Atom.Math.RandRangeExtensions.UncheckedRandomRange( rand, -1.0f, 1.0f ), 0 );


            bolt2.Source = position;
            bolt2.Destination = position - new Math.Vector3( length, jitter * Atom.Math.RandRangeExtensions.UncheckedRandomRange( rand, -1.0f, 1.0f ), 0 );


            bolt3.Source = position;
            bolt3.Destination = position + new Math.Vector3( jitter * Atom.Math.RandRangeExtensions.UncheckedRandomRange( rand, -1.0f, 1.0f ), length, 0 );


            bolt4.Source = position;
            bolt4.Destination = position - new Math.Vector3( jitter * Atom.Math.RandRangeExtensions.UncheckedRandomRange( rand, -1.0f, 1.0f ), length, 0 );

        }
        

        protected override void LoadContent()
        {
            this.drawContext = new SpriteDrawContext( this.GraphicsDevice ) { Batch = new Atom.Xna.Batches.ComposedSpriteBatch( GraphicsDevice ) };
                      
            glow.LoadContent();

            foreach( var b in bolts )
            {
                b.LoadContent( effectLoader, renderTargetFactory, GraphicsDevice );
            }


            //hdr.LoadContent();
            //luminance.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        KeyboardState oldKeyState;
        float x, y;

        protected override void Update( GameTime gameTime )
        {
            updateContext.GameTime = gameTime;

             _ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
    _TotalFrames++;
 
    if (_ElapsedTime >= 1.0f)
    {
        _Fps = _TotalFrames;
        _TotalFrames = 0;
        _ElapsedTime = 0;

        Window.Title = _Fps.ToString();
    }


            KeyboardState keyState = Keyboard.GetState();


            bool shift = keyState.IsKeyDown( Keys.LeftShift );
            float value = shift ? -1.0f : 1.0f;

            if( keyState.IsKeyDown( Keys.G ) && oldKeyState.IsKeyUp( Keys.G ) )
            {
                Atom.Math.RandExtensions.Shuffle( settings.Topology, rand );
            }

            if( keyState.IsKeyDown( Keys.H ) && oldKeyState.IsKeyUp( Keys.H ) )
            {
                settings.IsGlowEnabled = !settings.IsGlowEnabled;
            }

            if( keyState.IsKeyDown( Keys.F ) && oldKeyState.IsKeyUp( Keys.F ) )
            {
                settings.JitterLeftDeviation = new Math.FloatRange( settings.JitterLeftDeviation.Minimum + value, settings.JitterLeftDeviation.Maximum + value );
                Console.WriteLine( "settings.JitterLeftDeviation: " + settings.JitterLeftDeviation );
            }

            if( keyState.IsKeyDown( Keys.D ) && oldKeyState.IsKeyUp( Keys.D ) )
            {
                settings.JitterForwardDeviation = new Math.FloatRange( settings.JitterForwardDeviation.Minimum + value, settings.JitterForwardDeviation.Maximum + value );
                Console.WriteLine( "settings.JitterForwardDeviation: " + settings.JitterForwardDeviation );
            }
            if( keyState.IsKeyDown( Keys.S ) && oldKeyState.IsKeyUp( Keys.S ) )
            {
                settings.JitterDeviationRadius += value;
                Console.WriteLine( "settings.JitterDeviationRadius: " + settings.JitterDeviationRadius );
            }

            if( keyState.IsKeyDown( Keys.A ) && oldKeyState.IsKeyUp( Keys.A ) )
            {
                settings.JitterDecayRate += value;
                Console.WriteLine( "settings.JitterDecayRate: " + settings.JitterDecayRate );
            }


            if( keyState.IsKeyDown( Keys.Y ) && oldKeyState.IsKeyUp( Keys.Y ) )
            {
                settings.ForkDecayRate += value / 100.0f;
                Console.WriteLine( "settings.ForkDecayRate: " + settings.ForkDecayRate );
            }
            if( keyState.IsKeyDown( Keys.X ) && oldKeyState.IsKeyUp( Keys.X ) )
            {
                value /= 10;
                settings.ForkForwardDeviation = new Math.FloatRange( settings.ForkForwardDeviation.Minimum + value, settings.ForkForwardDeviation.Maximum + value );
                Console.WriteLine( "settings.ForkForwardDeviation: " + settings.JitterForwardDeviation );
            }
            if( keyState.IsKeyDown( Keys.C ) && oldKeyState.IsKeyUp( Keys.C ) )
            {
                value /= 10;
                settings.ForkLeftDeviation = new Math.FloatRange( settings.ForkLeftDeviation.Minimum + value, settings.ForkLeftDeviation.Maximum + value );
                Console.WriteLine( "settings.ForkLeftDeviation: " + settings.ForkLeftDeviation );
            }
            if( keyState.IsKeyDown( Keys.V ) && oldKeyState.IsKeyUp( Keys.V ) )
            {
                settings.ForkLengthPercentage += value / 10.0f;
                Console.WriteLine( "settings.ForkLengthPercentage: " + settings.ForkLengthPercentage );
            }

            if( keyState.IsKeyDown( Keys.E ) && oldKeyState.IsKeyUp( Keys.E ) )
            {
                settings.BaseWidth += value;
                Console.WriteLine( "settings.BaseWidth: " + settings.BaseWidth );
            }


            angle += (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdatePositions();

            foreach( var b in bolts)
                b.Update( updateContext );

            this.oldKeyState = keyState;
            base.Update( gameTime );
        }

        private Atom.Math.FastLineSegment2 line = new Atom.Math.FastLineSegment2( new Math.Vector2( 100, 100 ), new Math.Vector2( 300, 400 ) );
        private Atom.Math.Rectangle rect = new Math.Rectangle( 0, 0, 100, 100 );
        private Atom.Math.Rectangle rect2 = new Math.Rectangle( 110, 110, 210, 215);

        private float angle;
        
        protected override void Draw( GameTime gameTime )
        {
            drawContext.GameTime = gameTime;
            

            //draw it in 2D
            Matrix viewMatrix = Matrix.CreateLookAt( new Vector3( 0, 0, 1 ), new Vector3( 0, 0, 0 ), new Vector3( 0, 1, 0 ) );
            Matrix projectionMatrix = Matrix.CreateOrthographic( GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0.01f, 50 );
            Matrix worldMatrix = Matrix.Identity;

            //generate the lightning rendering

            foreach( var b in bolts )
            {
                b.GenerateTexture( drawContext );
            }


            GraphicsDevice.Clear( Color.Black );


            var batch = drawContext.Batch;
            batch.Begin( drawContext );

            foreach( var b in bolts )
            {
                //draw lightning texture over the scene
                var texture = b.ResolveTexture();

                batch.Draw( texture, new Math.Rectangle( 0, 0, texture.Width, texture.Height ), Color.White );
            }


            var orect = Atom.Math.OrientedRectangleF.FromRectangle( rect, angle );
            var color = orect.Intersects( rect2 ) ? Color.Red : Color.White;

            batch.DrawLineRect( orect, color );
            batch.DrawRect( rect2, color );
            batch.DrawLineRect( (Atom.Math.Rectangle)Atom.Math.RectangleF.FromOrientedRectangle( orect ), Color.Yellow );
            batch.DrawLine( line.Start, line.End, Color.Green );

            var or = Atom.Math.OrientedRectangleF.FromLine( line, 30 );

            batch.DrawLineRect( or, Color.Blue );

            batch.End();


            ////draw lightning texture over the scene
            //bolt.Source = position + new Atom.Math.Vector3( 0, offset, 0 );
            //bolt.Destination = bolt.Source + new Atom.Math.Vector3( 0, length, 0 );

            //bolt.GenerateTexture( worldMatrix, viewMatrix, projectionMatrix, drawContext );
            //var texture2 = bolt.ResolveTexture();


            ////draw lightning texture over the scene
            //bolt.Source = position - new Atom.Math.Vector3( offset, 0, 0 );
            //bolt.Destination = bolt.Source - new Atom.Math.Vector3( length, 0, 0 );

            //bolt.GenerateTexture( worldMatrix, viewMatrix, projectionMatrix, drawContext );
            //var texture3 = bolt.ResolveTexture();


            ////draw lightning texture over the scene
            //bolt.Source = position + new Atom.Math.Vector3( offset, 0, 0 );
            //bolt.Destination = bolt.Source + new Atom.Math.Vector3( length, 0, 0 );

            //bolt.GenerateTexture( worldMatrix, viewMatrix, projectionMatrix, drawContext );
            //var texture4 = bolt.ResolveTexture();

           
            base.Draw( gameTime );
        }

        private SpriteDrawContext drawContext;
        private Lightning.LightningSettings settings;
        private Glow glow;
        private IRenderTarget2DFactory renderTargetFactory;
        private IEffectLoader effectLoader;
        private XnaUpdateContext updateContext;
        private int _TotalFrames;
        private float _ElapsedTime;
        private int _Fps;
    }
}
