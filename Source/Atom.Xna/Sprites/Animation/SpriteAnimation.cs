// <copyright file="SpriteAnimation.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteAnimation class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Atom.Xna.Batches;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// A SpriteAnimation is an 'instance' of an <see cref="AnimatedSprite"/>.
    /// This is needed to have multiple instances of an <see cref="AnimatedSprite"/>
    /// playing at the same time.
    /// </summary>
    public sealed class SpriteAnimation : ISprite, ICloneable, IUpdateable
    {
        #region [ Events ]

        /// <summary>
        /// Raised when this SpriteAnimation has completed.
        /// </summary>
        public event SimpleEventHandler<SpriteAnimation> ReachedEnd;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the <see cref="AnimatedSprite"/> that is the template of this <see cref="SpriteAnimation"/> instance.
        /// </summary>
        /// <value>The AnimatedSprite this SpriteAnimation is based on.</value>
        public AnimatedSprite AnimatedSprite
        {
            get
            { 
                return this.animatedSprite;
            }
        }

        /// <summary>
        /// Gets the name of the underlying <see cref="AnimatedSprite"/> of this <see cref="SpriteAnimation"/>.
        /// </summary>
        /// <value>The name that (uniquely) identifies the AnimatedSprite this SpriteAnimation is based on.</value>
        public string Name
        {
            get
            {
                return this.animatedSprite.Name;
            }
        }

        #region > Settings <

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AnimatedSprite"/> is looping.
        /// </summary>
        /// <value>The default value is false.</value>
        public bool IsLooping
        {
            get 
            { 
                return this.isLooping; 
            }

            set 
            { 
                this.isLooping = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether animating of this <see cref="SpriteAnimation"/> is enabled.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool IsAnimatingEnabled
        {
            get 
            {
                return this.isAnimatingEnabled;
            }

            set 
            {
                this.isAnimatingEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the speed this <see cref="AnimatedSprite"/> is animating at.
        /// </summary>
        public float AnimationSpeed
        {
            get 
            {
                return this.animationSpeed;
            }

            set
            {
                this.animationSpeed = value;
            }
        }

        #endregion

        #region > State <

        /// <summary>
        /// Gets the current animation time tick.
        /// </summary>
        /// <value>The current animation time tick value.</value>
        public float Time
        {
            get 
            { 
                return this.time;
            }
        }

        /// <summary>
        /// Gets the total time the <see cref="SpriteAnimation"/> takes.
        /// </summary>
        /// <value>The total time in seconds this SpriteAnimation takes.</value>
        public float TotalTime
        {
            get 
            {
                return this.animatedSprite.TotalTime;
            }
        }

        /// <summary>
        /// Gets or sets the index of the currently displayed frame of the <see cref="SpriteAnimation"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If the given index value is out of valid range.
        /// </exception>
        /// <value>The index, or -1 if invalid.</value>
        public int FrameIndex
        {
            get
            {
                return this.currentFrameIndex;
            }

            set
            {
                Contract.Requires<ArgumentOutOfRangeException>( value >= 0 );
                Contract.Requires<ArgumentOutOfRangeException>( value < this.AnimatedSprite.FrameCount );

                this.currentFrame = this.animatedSprite[value];
                this.currentFrameIndex = value;

                this.time = 0.0f;
                for( int i = 0; i < value; ++i )
                {
                    this.time += this.animatedSprite[i].Time;
                }
            }
        }

        /// <summary>
        /// Gets the currently displayed frame of the <see cref="SpriteAnimation"/>. May be null.
        /// </summary>
        /// <value>The current animation frame.</value>
        public AnimatedSpriteFrame Frame
        {
            get 
            { 
                return this.currentFrame;
            }
        }

        /// <summary>
        /// Gets the width of the <see cref="Sprite"/> of the current AnimatedSprite.Frame.
        /// </summary>
        /// <value>The width of the Sprite of the current Frame.</value>
        public int Width
        {
            get
            {
                if( this.currentFrame == null )
                    return 0;

                Sprite sprite = this.currentFrame.Sprite;
                if( sprite == null )
                    return 0;

                return sprite.Width;
            }
        }

        /// <summary>
        /// Gets the height of the <see cref="Sprite"/> of the current AnimatedSprite.Frame.
        /// </summary>
        /// <value>The height of the Sprite of the current Frame.</value>
        public int Height
        {
            get
            {
                if( this.currentFrame == null )
                    return 0;

                Sprite sprite = this.currentFrame.Sprite;
                if( sprite == null )
                    return 0;

                return sprite.Height;
            }
        }

        /// <summary>
        /// Gets the default origin of the <see cref="Sprite"/> of the current AnimatedSprite.Frame.
        /// </summary>
        /// <value>The default origin of the Sprite of the current Frame.</value>
        public Vector2 Origin
        {
            get
            {
                if( this.currentFrame == null )
                    return Vector2.Zero;

                Sprite sprite = this.currentFrame.Sprite;
                if( sprite == null )
                    return Vector2.Zero;

                return sprite.Origin;
            }
        }

        /// <summary>
        /// Gets the <see cref="Texture2D"/> of the <see cref="Sprite"/> 
        /// of the current <see cref="Frame"/>. May return null.
        /// </summary>
        /// <value>The texture of the Sprite of the current Frame.</value>
        public Texture2D Texture
        {
            get
            {
                if( this.currentFrame == null )
                    return null;

                Sprite sprite = this.currentFrame.Sprite;
                if( sprite == null )
                    return null;

                return sprite.Texture;
            }
        }

        /// <summary>
        /// Gets the source <see cref="Rectangle"/> of the <see cref="Sprite"/>
        /// of the current AnimatedSprite.Frame.
        /// </summary>
        /// <value>The source rectangle of the Sprite of the current Frame.</value>
        public Rectangle Source
        {
            get
            {
                if( this.currentFrame == null )
                    return Rectangle.Empty;

                Sprite sprite = this.currentFrame.Sprite;
                if( sprite == null )
                    return Rectangle.Empty;

                return sprite.Source;
            }
        }

        /// <summary>
        /// Gets the size of the <see cref="Sprite"/>
        /// of the current AnimatedSprite.Frame.
        /// </summary>
        /// <value>The size of the Sprite of the current Frame.</value>
        public Point2 Size
        {
            get
            {
                if( this.currentFrame == null )
                    return Point2.Zero;

                Sprite sprite = this.currentFrame.Sprite;
                if( sprite == null )
                    return Point2.Zero;

                return sprite.Size;
            }
        }

        /// <summary>
        /// Gets the color of the <see cref="Sprite"/>
        /// of the current AnimatedSprite.Frame.
        /// </summary>
        /// <value>The color of the Sprite of the current Frame.</value>
        public Xna.Color Color
        {
            get
            {
                if( this.currentFrame == null )
                    return Xna.Color.White;

                Sprite sprite = this.currentFrame.Sprite;
                if( sprite == null )
                    return Xna.Color.White;

                return sprite.Color;
            }
        }

        /// <summary>
        /// Gets the scaling factor of the <see cref="Sprite"/>
        /// of the current AnimatedSprite.Frame.
        /// </summary>
        /// <value>The scaling factor of the Sprite of the current Frame.</value>
        public Vector2 Scale
        {
            get
            {
                if( this.currentFrame == null )
                    return Vector2.One;

                Sprite sprite = this.currentFrame.Sprite;
                if( sprite == null )
                    return Vector2.One;

                return sprite.Scale;
            }
        }

        /// <summary>
        /// Gets the rotation in radians of the <see cref="Sprite"/>
        /// of the current AnimatedSprite.Frame.
        /// </summary>
        /// <value>The rotation in radians of the Sprite of the current Frame.</value>
        public float Rotation
        {
            get
            {
                if( this.currentFrame == null )
                    return 0.0f;

                Sprite sprite = this.currentFrame.Sprite;
                if( sprite == null )
                    return 0.0f;

                return sprite.Rotation;
            }
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteAnimation"/> class.
        /// </summary>
        /// <param name="animatedSprite">
        /// The <see cref="AnimatedSprite"/> that works as a template for the new <see cref="SpriteAnimation"/> instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="animatedSprite"/> is null.
        /// </exception>
        public SpriteAnimation( AnimatedSprite animatedSprite )
            : this( animatedSprite, animatedSprite.DefaultAnimationSpeed, animatedSprite.IsLoopingByDefault )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteAnimation"/> class.
        /// </summary>
        /// <param name="animatedSprite">
        /// The <see cref="AnimatedSprite"/> that works as a template for the new <see cref="SpriteAnimation"/> instance.
        /// </param>
        /// <param name="animationSpeed"> 
        /// The speed at which the animation is animated.
        /// Negative values 'reverse' the animation.
        /// </param>
        /// <param name="isLooping">
        /// States whether the animation is looping.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="animatedSprite"/> is null.
        /// </exception>
        public SpriteAnimation( AnimatedSprite animatedSprite, float animationSpeed, bool isLooping )
        {
            Contract.Requires<ArgumentNullException>( animatedSprite != null );

            this.animatedSprite = animatedSprite;
            this.animationSpeed = animationSpeed;
            this.isLooping = isLooping;

            if( this.animatedSprite.FrameCount > 0 )
            {
                this.currentFrame = animatedSprite[0];
                this.currentFrameIndex = 0;
            }
        }

        #endregion

        #region [ Methods ]

        #region > Draw <

        /// <summary>
        /// Draws this <see cref="SpriteAnimation"/> at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this <see cref="ISprite"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw( Vector2 position, ISpriteBatch batch )
        {
            if( this.currentFrame == null || this.currentFrame.Sprite == null )
                return;

            position += this.currentFrame.Offset;
            this.currentFrame.Sprite.Draw( position, batch );
        }

        /// <summary>
        /// Draws this <see cref="SpriteAnimation"/> at the given position
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this <see cref="ISprite"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="x">
        /// The position to render the sprite at on the x-axis.
        /// </param>
        /// <param name="y">
        /// The position to render the sprite at on the y-axis.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw( float x, float y, ISpriteBatch batch )
        {
            if( this.currentFrame == null || this.currentFrame.Sprite == null )
                return;

            Vector2 offset = this.currentFrame.Offset;
            x += offset.X;
            y += offset.Y;

            this.currentFrame.Sprite.Draw( x, y, batch );
        }

        /// <summary>
        /// Draws this <see cref="SpriteAnimation"/> at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this <see cref="ISprite"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="depthLayer">The depth layer to draw the sprite at; used for sorting.</param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw( Vector2 position, float depthLayer, ISpriteBatch batch )
        {
            if( this.currentFrame == null || this.currentFrame.Sprite == null )
                return;

            position += currentFrame.Offset;
            this.currentFrame.Sprite.Draw( position, depthLayer, batch );
        }

        /// <summary>
        /// Draws this <see cref="SpriteAnimation"/> at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this <see cref="ISprite"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw( Vector2 position, Xna.Color color, ISpriteBatch batch )
        {
            if( this.currentFrame == null || this.currentFrame.Sprite == null )
                return;

            position += currentFrame.Offset;
            this.currentFrame.Sprite.Draw( position, color, batch );
        }

        /// <summary>
        /// Draws this ISprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this ISprite.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="color">The color channel modulation to use.</param>
        /// <param name="depthLayer">The layer to draw at.</param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw( Vector2 position, Xna.Color color, float depthLayer, ISpriteBatch batch )
        {
            if( this.currentFrame == null || this.currentFrame.Sprite == null )
                return;

            position += this.currentFrame.Offset;
            this.currentFrame.Sprite.Draw( position, color, depthLayer, batch );
        }

        /// <summary>
        /// Draws this <see cref="SpriteAnimation"/> into the given <paramref name="destination"/> rectangle.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="destination">
        /// The rectangle the sprite is drawn in.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw(
            Rectangle destination,
            Xna.Color color,
            ISpriteBatch batch )
        {
            if( currentFrame == null || currentFrame.Sprite == null )
                return;

            Vector2 offset = this.currentFrame.Offset;
            destination.X += (int)offset.X;
            destination.Y += (int)offset.Y;

            this.currentFrame.Sprite.Draw( destination, color, batch );
        }

        /// <summary>
        /// Draws this <see cref="SpriteAnimation"/> at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using a mix of the specified and the default settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="rotation">
        /// The rotation in radians.
        /// </param>
        /// <param name="origin">
        /// The origin of rotation and scaling.
        /// </param>
        /// <param name="scale">
        /// The scaling factor to apply.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw(
            Vector2 position,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            ISpriteBatch batch )
        {
            if( this.currentFrame == null || this.currentFrame.Sprite == null )
                return;

            position += this.currentFrame.Offset;
            this.currentFrame.Sprite.Draw( position, rotation, origin, scale, batch );
        }

        /// <summary>
        /// Draws this <see cref="SpriteAnimation"/> at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using a mix of the specified and the default settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use.
        /// </param>
        /// <param name="rotation">
        /// The rotation in radians.
        /// </param>
        /// <param name="origin">
        /// The orgin of rotation and scaling.
        /// </param>
        /// <param name="scale">
        /// The scaling factor to apply.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw(
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            ISpriteBatch batch )
        {
            if( this.currentFrame == null || this.currentFrame.Sprite == null )
                return;

            position += this.currentFrame.Offset;
            this.currentFrame.Sprite.Draw( position, color, rotation, origin, scale, batch );
        }

        /// <summary>
        /// Draws this <see cref="SpriteAnimation"/> at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the specified settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use.
        /// </param>
        /// <param name="rotation">
        /// The rotation in radians.
        /// </param>
        /// <param name="origin">
        /// The origin of rotation and scaling.
        /// </param>
        /// <param name="scale">
        /// The scaling factor to apply.
        /// </param>
        /// <param name="effects">
        /// The SpriteEffects to apply.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw(
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            ISpriteBatch batch )
        {
            if( this.currentFrame == null || this.currentFrame.Sprite == null )
                return;

            position += this.currentFrame.Offset;
            this.currentFrame.Sprite.Draw( position, color, rotation, origin, scale, effects, 0.0f, batch );
        }

        /// <summary>
        /// Draws this <see cref="SpriteAnimation"/> at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the specified settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use.
        /// </param>
        /// <param name="rotation">
        /// The rotation in radians.
        /// </param>
        /// <param name="origin">
        /// The origin of rotation and scaling.
        /// </param>
        /// <param name="scale">
        /// The scaling factor to apply.
        /// </param>
        /// <param name="effects">
        /// The SpriteEffects to apply.
        /// </param>
        /// <param name="layerDepth">
        /// The depth to draw the sprite at.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw(
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth,
            ISpriteBatch batch )
        {
            if( this.currentFrame == null || this.currentFrame.Sprite == null )
                return;

            position += currentFrame.Offset;
            this.currentFrame.Sprite.Draw( position, color, rotation, origin, scale, effects, layerDepth, batch );
        }

        #endregion

        /// <summary>
        /// Animates this <see cref="SpriteAnimation"/>.
        /// </summary>
        /// <param name="frameTime">
        /// The time the last frame took (in seconds).
        /// </param>
        public void Animate( float frameTime )
        {
            if( !this.isAnimatingEnabled )
                return;

            this.time += this.animationSpeed * frameTime;

            if( this.time >= this.animatedSprite.TotalTime )
            {
                this.currentFrame      = this.animatedSprite.LastFrame;
                this.currentFrameIndex = this.animatedSprite.FrameCount - 1;

                if( this.isLooping )
                {
                    // Reset time if we reached the end and are looping!
                    this.time = 0.0f;
                }

                this.ReachedEnd.Raise( this );
            }
            else
            {
                this.FindCurrentFrame();
            }
        }

        /// <summary>
        /// Finds and sets teh current <see cref="AnimatedSpriteFrame"/>
        /// based on the current animation time.
        /// </summary>
        private void FindCurrentFrame()
        {
            int frameCount = this.animatedSprite.FrameCount;
            float timeOffset = 0.0f;
            this.currentFrame = null;

            for( int i = 0; i < frameCount; ++i )
            {
                AnimatedSpriteFrame frame = this.animatedSprite[i];

                if( this.time >= timeOffset && this.time <= frame.Time + timeOffset )
                {
                    this.currentFrame = frame;
                    this.currentFrameIndex = i;
                    break;
                }

                timeOffset += frame.Time;
            }
        }

        /// <summary>
        /// Updates/Animates this SpriteAnimation.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        void IUpdateable.Update( IUpdateContext updateContext )
        {
            this.Animate( updateContext.FrameTime );
        }

        /// <summary>
        /// Resets this <see cref="SpriteAnimation"/>.
        /// </summary>
        public void Reset()
        {
            this.time = 0.0f;

            if( this.animatedSprite.FrameCount > 0 )
            {
                this.currentFrame = this.animatedSprite[0];
                this.currentFrameIndex = 0;
            }
            else
            {
                this.currentFrame = null;
                this.currentFrameIndex = -1;
            }
        }

        /// <summary>
        /// Creates a clone of this <see cref="SpriteAnimation"/>.
        /// </summary>
        /// <returns>
        /// A clone of the <see cref="SpriteAnimation"/> which has
        /// the same <see cref="AnimatedSprite"/>, animation speed and looping settings
        /// as the original. 
        /// </returns>
        public SpriteAnimation Clone()
        {
            return new SpriteAnimation( this.animatedSprite, this.animationSpeed, this.isLooping );
        }

        /// <summary>
        /// Creates a clone of this <see cref="SpriteAnimation"/>.
        /// </summary>
        /// <returns>
        /// A clone of the <see cref="SpriteAnimation"/> which has
        /// the same <see cref="AnimatedSprite"/>, animation speed and looping settings
        /// as the original. 
        /// </returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Creates a clone of this <see cref="SpriteAnimation"/>.
        /// </summary>
        /// <returns>
        /// A clone of the <see cref="SpriteAnimation"/> which has
        /// the same <see cref="AnimatedSprite"/>, animation speed and looping settings
        /// as the original. 
        /// </returns>
        ISprite ISprite.CloneInstance()
        {
            return this.Clone();
        }

        /// <summary>
        /// Overwritten to return the <see cref="Name"/>
        /// of this <see cref="SpriteAnimation"/>.
        /// </summary>
        /// <returns>The <see cref="Name"/> of this <see cref="SpriteAnimation"/>.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The current time.
        /// </summary>
        private float time;

        /// <summary>
        /// The speed this <see cref="SpriteAnimation"/> is animating at.
        /// </summary>
        private float animationSpeed;

        /// <summary>
        /// States whether this <see cref="SpriteAnimation"/> is looping.
        /// </summary>
        private bool isLooping;

        /// <summary>
        /// States whether animation of this <see cref="SpriteAnimation"/> is enabled.
        /// </summary>
        private bool isAnimatingEnabled = true;

        /// <summary>
        /// The current frame. Can be null.
        /// </summary>
        private AnimatedSpriteFrame currentFrame;

        /// <summary>
        /// The index of the currently selected frame.
        /// </summary>
        private int currentFrameIndex = -1;

        /// <summary>
        /// The <see cref="AnimatedSprite"/> that works as a template for this <see cref="SpriteAnimation"/>.
        /// </summary>
        private readonly AnimatedSprite animatedSprite;

        #endregion
    }
}
