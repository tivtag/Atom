using Atom.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace Atom.Xna.Particles.Tests.Manual
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager( this );

            graphics.PreferredBackBufferWidth = 1400;
            graphics.PreferredBackBufferHeight = 900;

            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch( GraphicsDevice );

            this.whiteTexture = new Texture2D( GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color );
            this.whiteTexture.SetData( new Color[] { Color.White } );
        }

        private Texture2D whiteTexture;

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update( GameTime gameTime )
        {
            MouseState mouseState  = Mouse.GetState();
            
            if( mouseState.LeftButton == ButtonState.Pressed &&
                    oldMouseState.LeftButton == ButtonState.Released )
            {
                this.points.Add( new Atom.Math.Vector2(
                    mouseState.X, mouseState.Y )
                    );

                this.Recalculate();
            }
            
            if( mouseState.RightButton == ButtonState.Pressed &&
                oldMouseState.RightButton == ButtonState.Released )
            {
                this.points.Clear();
                this.Recalculate();
            }

            this.oldMouseState = mouseState;

            base.Update( gameTime );
        }

        private void Recalculate()
        {
            this.polynom = new LagrangePolynom( this.points );
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw( GameTime gameTime )
        {
            this.GraphicsDevice.Clear( Color.Black );

            if( polynom != null )
            {
                spriteBatch.Begin();

                for( float x = 0.0f; x < 1400.0f; x += 0.025f )
                {
                    DrawPoint( x, polynom.GetY( x ) );
                }

                spriteBatch.End();
            }

            base.Draw( gameTime );
        }

        private void DrawPoint( float x, float y )
        {
            spriteBatch.Draw(
                this.whiteTexture,
                new Microsoft.Xna.Framework.Rectangle( (int)x, (int)y, 1, 1 ),
                Color.White
            );
        }

        private List<Atom.Math.Vector2> points = new List<Atom.Math.Vector2>();
       
        private LagrangePolynom polynom;
        private MouseState oldMouseState;

    }
}
