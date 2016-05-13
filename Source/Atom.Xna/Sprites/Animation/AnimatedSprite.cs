// <copyright file="AnimatedSprite.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.AnimatedSprite class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Atom.Collections;
    using Atom.Math;

    /// <summary>
    /// An AnimatedSprite consists of a set <see cref="AnimatedSpriteFrame"/>s.
    /// </summary>
    /// <remarks>
    /// To use an AnimatedSprite an instance of the animation in form of a <see cref="SpriteAnimation"/>
    /// has to be created. This makes it possible to have multiple instances of the same AnimatedSprite
    /// animated at different animation times.
    /// </remarks>
    public sealed partial class AnimatedSprite : ISpriteAsset
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the name that (uniquely) identifies this <see cref="AnimatedSprite"/>.
        /// </summary>
        /// <value>The name that (uniquely) identifies this AnimatedSprite.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default animation speed to apply to <see cref="SpriteAnimation"/>s.
        /// </summary>
        /// <value>
        /// The animation speed that is initially set when creating a new instance of this AnimatedSprite
        /// using the SpriteAnimation class.
        /// </value>
        public float DefaultAnimationSpeed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="SpriteAnimation"/>s initiating 
        /// the <see cref="AnimatedSprite"/> are looping by default.
        /// </summary>
        /// <value>
        /// The looping setting that is initially set when creating a new instance of this AnimatedSprite
        /// using the SpriteAnimation class.
        /// </value>
        public bool IsLoopingByDefault
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="AnimatedSpriteFrame"/> at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index"> The index of the <see cref="AnimatedSpriteFrame"/> to get. </param>
        /// <returns>The <see cref="AnimatedSpriteFrame"/> at the given <paramref name="index"/>. </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"> 
        /// If the given <paramref name="index"/> is out of valid range. 
        /// </exception>
        public AnimatedSpriteFrame this[int index]
        {
            get 
            {
                return this.frames[index];
            }
        }

        /// <summary>
        /// Gets the number of <see cref="AnimatedSpriteFrame"/>s this <see cref="AnimatedSprite"/> has.
        /// </summary>
        /// <value>The number of frames this AnimatedSprite contains.</value>
        public int FrameCount
        {
            get
            {
                return this.frames.Length;
            }
        }

        /// <summary>
        /// Gets the time the <see cref="AnimatedSprite"/> takes to execute.
        /// </summary>
        /// <value>The total time this AnimatedSprite lasts.</value>
        public float TotalTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last frame of the <see cref="AnimatedSprite"/>.
        /// </summary>
        /// <value>The last frame of this AnimatedSprite; or null if there aren't any frames.</value>
        public AnimatedSpriteFrame LastFrame
        {
            get
            {
                return this.lastFrame;
            }
        }

        /// <summary>
        /// Gets a string containg a human-readable description of this <see cref="AnimatedSprite"/>.
        /// </summary>
        /// <value>A detailted description of this AnimatedSprite.</value>
        public string DetailedDescription
        {
            get
            {
                var culture = System.Globalization.CultureInfo.CurrentUICulture;
                var sb = new System.Text.StringBuilder();

                sb.AppendFormat( "AnimatedSprite '{0}'\n", this.Name );
                sb.AppendFormat( "  AnimationSpeed='{0}'\n", this.DefaultAnimationSpeed.ToString( culture ) );
                sb.AppendFormat( "  IsLooping='{0}'\n", this.IsLoopingByDefault.ToString() );
                sb.AppendFormat( 
                    "  FrameCount='{0}' TotalTime='{1}'\n",
                    this.frames.Length.ToString( culture ),
                    this.TotalTime.ToString( culture ) 
                );

                for( int i = 0; i < this.frames.Length; ++i )
                {
                    AnimatedSpriteFrame frame = this.frames[i];

                    sb.AppendFormat( "   Frame_{0}\n", (i + 1).ToString( culture ) );
                    sb.AppendFormat( "     Sprite='{0}'\n", frame.Sprite == null ? string.Empty : frame.Sprite.Name );
                    sb.AppendFormat( "     Offset='{0}'\n", frame.Offset.ToString() );
                    sb.AppendFormat( "     Time='{0}'\n", frame.Time.ToString( culture ) );
                }

                return sb.ToString();
            }
        }      
        
        /// <summary>
        /// Gets the size of the first AnimatedSpriteFrame of this AnimatedSprite.
        /// </summary>
        /// <value>
        /// The size of the Sprite of the first frame (in pixels).
        /// </value>
        public Point2 Size
        {
            get
            {
                if( this.frames.Length == 0 )
                    return Point2.Zero;

                var frame = this.frames[0];
                var sprite = frame.Sprite;
                return sprite.Size;
            }
        }

        /// <summary>
        /// Gets the width of the first AnimatedSpriteFrame of this AnimatedSprite.
        /// </summary>
        /// <value>
        /// The width of the Sprite of the first frame (in pixels).
        /// </value>
        public int Width
        {
            get
            {
                return this.Size.X;
            }
        }

        /// <summary>
        /// Gets the height of the first AnimatedSpriteFrame of this AnimatedSprite.
        /// </summary>
        /// <value>
        /// The height of the Sprite of the first frame (in pixels).
        /// </value>
        public int Height
        {
            get
            {
                return this.Size.Y;
            }
        }

        /// <summary>
        /// Gets the frames of this <see cref="AnimatedSprite"/>
        /// as a read-only collection.
        /// </summary>
        /// <returns>A new read-only collection that contains the frames of this AnimatedSprite.</returns>
        public IList<AnimatedSpriteFrame> GetFrames()
        {
            return new System.Collections.ObjectModel.ReadOnlyCollection<AnimatedSpriteFrame>( this.frames );
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the AnimatedSprite class.
        /// </summary>
        internal AnimatedSprite()
        {
        }
        
        /// <summary>
        /// Manually creates a new <see cref="AnimatedSprite"/>
        /// that has the given name, initial number of frames and a default 
        /// animation speed of 1000.0f.
        /// </summary>
        /// <param name="name"> The name of the <see cref="AnimatedSprite"/>. </param>
        /// <param name="frameCount"> The initial number of frames to create. </param>
        /// <returns> 
        /// A new <see cref="AnimatedSprite"/> instance whos
        /// frames are uninitialized.
        /// </returns>
        public static AnimatedSprite CreateManual( string name, int frameCount )
        {
            return CreateManual( name, frameCount, 1000.0f, true );
        }

        /// <summary>
        /// Manually creates a new <see cref="AnimatedSprite"/>
        /// that has the given name and initial number of frames.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="AnimatedSprite"/>.
        /// </param>
        /// <param name="frameCount">
        /// The initial number of frames to create.
        /// </param>
        /// <param name="defaultAnimationSpeed">
        /// The default animation speed to apply to <see cref="SpriteAnimation"/>s.
        /// </param>
        /// <param name="isLoopingByDefault">
        /// Specifies whether <see cref="SpriteAnimation"/> of the new <see cref="AnimatedSprite"/>
        /// will loop by default.
        /// </param>
        /// <returns> 
        /// A new <see cref="AnimatedSprite"/> instance whos
        /// frames are uninitialized.
        /// </returns>
        public static AnimatedSprite CreateManual(
            string name,
            int frameCount,
            float defaultAnimationSpeed,
            bool isLoopingByDefault )
        {
            var sprite = new AnimatedSprite() {
                Name = name,
                DefaultAnimationSpeed = defaultAnimationSpeed,
                IsLoopingByDefault = isLoopingByDefault
            };

            var frames = new AnimatedSpriteFrame[frameCount];

            for( int i = 0; i < frameCount; ++i )
            {
                frames[i] = new AnimatedSpriteFrame( null, Vector2.Zero, 100.0f );
            }

            sprite.SetFrames( frames );

            return sprite;
        }

        /// <summary>
        /// Sets the frames of this AnimatedSprite.
        /// </summary>
        /// <param name="frames">
        /// The frames this AnimatedSprite should consist of.
        /// </param>
        internal void SetFrames( AnimatedSpriteFrame[] frames )
        {
            Contract.Requires<ArgumentNullException>( frames != null );

            this.frames = frames;
            this.TotalTime = 0.0f;

            if( this.frames.Length > 0 )
            {
                this.lastFrame = this.frames[this.frames.Length - 1];
            }
            else
            {
                this.lastFrame = null;
            }

            foreach( var frame in this.frames )
            {
                frame.PropertyChanged += this.OnFramePropertyChanged;
                this.TotalTime += frame.Time;
            }
        }

        #endregion

        #region [ Methods ]

        #region AddFrame

        /// <summary>
        /// Adds a new initialized <see cref="AnimatedSpriteFrame"/> to this AnimatedSpriteFrame.
        /// </summary>
        /// <param name="sprite">
        /// The <see cref="Sprite"/> to display in the Frame. Can be null.
        /// </param>
        /// <param name="offset">
        /// The rendering offset to apply when rendering the frame.
        /// </param>
        /// <param name="time">
        /// The time the frame should last.
        /// </param>
        /// <returns>
        /// The frame that has been added.
        /// </returns>
        public AnimatedSpriteFrame AddFrame( Sprite sprite, Vector2 offset, float time )
        {
            var frame = new AnimatedSpriteFrame( sprite, offset, time );
            frame.PropertyChanged += this.OnFramePropertyChanged;

            Array.Resize( ref this.frames, this.frames.Length + 1 );
            this.frames[this.frames.Length - 1] = frame;

            this.TotalTime += time;
            return frame;
        }

        /// <summary>
        /// Adds a new uninitialized <see cref="AnimatedSpriteFrame"/> to this AnimatedSprite.
        /// </summary>
        /// <returns>
        /// The frame that has been added.
        /// </returns>
        public AnimatedSpriteFrame AddFrame()
        {
            return this.AddFrame( null, Vector2.Zero, 100.0f );
        }

        #endregion

        #region RemoveFrame

        /// <summary>
        /// Tries to remove the specified <see cref="AnimatedSpriteFrame"/> from this <see cref="AnimatedSprite"/>.
        /// </summary>
        /// <param name="frame">
        /// The AnimatedSpriteFrame to remove.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the Frame has been removed;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool RemoveFrame( AnimatedSpriteFrame frame )
        {
            int index = Array.IndexOf( this.frames, frame );
            if( index == -1 )
                return false;

            ArrayUtilities.RemoveAt( ref frames, index );            
            frame.PropertyChanged -= this.OnFramePropertyChanged;
            this.TotalTime -= frame.Time;
            return true;
        }

        #endregion

        /// <summary>
        /// Swaps the frames at the given indices.
        /// </summary>
        /// <param name="indexA">
        /// The zero-based index of the first frame to swap.
        /// </param>
        /// <param name="indexB">
        /// The zero-based index of the second frame to swap.
        /// </param>
        public void SwapFrames( int indexA, int indexB )
        {
            this.frames.SwapItems( indexA, indexB );
        }

        /// <summary>
        /// Creates a new instance of this AnimatedSprite.
        /// </summary>
        /// <returns>
        /// A new instance of this AnimatedSprite.
        /// </returns>
        public SpriteAnimation CreateInstance()
        {
            return new SpriteAnimation( this );
        }

        /// <summary>
        /// Creates an instance of this ISpriteAsset.
        /// </summary>
        /// <returns>
        /// The instance that has been created.
        /// </returns>
        ISprite ISpriteAsset.CreateInstance()
        {
            return this.CreateInstance();
        }

        /// <summary>
        /// Refreshes the <see cref="TotalTime"/> property of this <see cref="AnimatedSprite"/>.
        /// </summary>
        private void RefreshTotalTime()
        {
            this.TotalTime = 0.0f;

            for( int i = 0; i < frames.Length; ++i )
            {
                this.TotalTime += frames[i].Time;
            }
        }

        /// <summary>
        /// Called when any property of any <see cref="AnimatedSpriteFrame"/> of this AnimatedSprite has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The PropertyChangedEventArgs that contain the event data.
        /// </param>
        private void OnFramePropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
        {
            if( e.PropertyName == "Time" )
            {
                this.RefreshTotalTime();
            }
        }

        /// <summary>
        /// Overriden to return the name of this AnimatedSprite.
        /// </summary>
        /// <returns>The name of this AnimatedSprite.</returns>
        public override string ToString()
        {
            return this.Name ?? string.Empty;
        }

        /// <summary>
        /// Creates a clone of this AnimatedSprite instance.
        /// </summary>
        /// <returns>
        /// The newly created clone.
        /// </returns>
        public AnimatedSprite Clone()
        {
            var clone = new AnimatedSprite() {
                Name = this.Name,
                DefaultAnimationSpeed = this.DefaultAnimationSpeed,
                IsLoopingByDefault = this.IsLoopingByDefault,
                frames = this.frames.Select( f => f.Clone() ).ToArray()
            };

            clone.RefreshTotalTime();
            return clone;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Caches the last frame in this AnimatedSprite.
        /// </summary>
        private AnimatedSpriteFrame lastFrame;

        /// <summary>
        /// The list of frames this AnimatedSprite consists of.
        /// </summary>
        private AnimatedSpriteFrame[] frames;

        #endregion
    }
}
