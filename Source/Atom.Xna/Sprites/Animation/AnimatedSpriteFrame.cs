// <copyright file="AnimatedSpriteFrame.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.AnimatedSpriteFrame class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using System;
    using System.ComponentModel;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;

    /// <summary>
    /// Represents a single frame of an <see cref="AnimatedSprite"/>.
    /// This class can't be inherited.
    /// </summary>
    public sealed partial class AnimatedSpriteFrame : INotifyPropertyChanged
    {
        #region [ Events ]

        /// <summary>
        /// Fired when any property of this <see cref="AnimatedSpriteFrame"/> has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the sprite that is rendered in the <see cref="AnimatedSpriteFrame"/>.
        /// </summary>
        /// <value>The <see cref="Sprite"/> of this frame.</value>
        public Sprite Sprite
        {
            get
            {
                return this.sprite;
            }

            set
            {
                if( value == this.sprite )
                    return;

                this.sprite = value;
                this.OnPropertyChanged( "Sprite" );
            }
        }

        /// <summary>
        /// Gets or sets the drawing offset which is applied in the <see cref="AnimatedSpriteFrame"/>.
        /// </summary>
        /// <value>The offset that gets applied when drawing this frame.</value>
        public Vector2 Offset
        {
            get
            {
                return this.offset;
            }

            set
            {
                if( value == this.offset )
                    return;

                this.offset = value;
                this.OnPropertyChanged( "Offset" );
            }
        }

        /// <summary>
        /// Gets or sets the time the <see cref="AnimatedSpriteFrame"/> lasts.
        /// </summary>
        /// <value>The time this frame lasts.</value>
        public float Time
        {
            get
            {
                return this.time;
            }

            set
            {
                Contract.Requires<ArgumentException>( value >= 0.0f );

                if( value == this.time )
                    return;

                this.time = value;
                this.OnPropertyChanged( "Time" );
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedSpriteFrame"/> class.
        /// </summary>
        public AnimatedSpriteFrame()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedSpriteFrame"/> class.
        /// </summary>
        /// <param name="sprite">
        /// The <see cref="Sprite"/> to display in the Frame. Can be null.
        /// </param>
        /// <param name="offset">
        /// The rendering offset to apply when rendering the frame.
        /// </param>
        /// <param name="time">
        /// The time the Frame should last.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="time"/> is negative.
        /// </exception>
        public AnimatedSpriteFrame( Sprite sprite, Vector2 offset, float time )
        {
            Contract.Requires<ArgumentException>( time >= 0.0f );

            this.sprite = sprite;
            this.offset = offset;
            this.time = time;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        private void OnPropertyChanged( string name )
        {
            if( this.PropertyChanged != null )
            {
                this.PropertyChanged( this, new PropertyChangedEventArgs( name ) );
            }
        }

        /// <summary>
        /// Overriden to return a human-readable representation of the <see cref="AnimatedSpriteFrame"/>.
        /// </summary>
        /// <returns>A human-readable representation of this AnimatedSpriteFrame.</returns>
        public override string ToString()
        {
            return this.sprite == null ? "Frame" : "Frame (" + sprite.Name + ')';
        }

        /// <summary>
        /// Creates a clone of this <see cref="AnimatedSpriteFrame"/>.
        /// </summary>
        /// <returns>
        /// The newly created clone.
        /// </returns>
        public AnimatedSpriteFrame Clone()
        {
            return new AnimatedSpriteFrame() {
                Offset = this.offset,
                sprite = this.sprite,
                time = this.time
            };
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The sprite that is rendered in the <see cref="AnimatedSpriteFrame"/>.
        /// </summary>
        private Sprite sprite;

        /// <summary>
        /// The rendering offset which is applied in the <see cref="AnimatedSpriteFrame"/>.
        /// </summary>
        private Vector2 offset;

        /// <summary>
        /// The time the <see cref="AnimatedSpriteFrame"/> lasts.
        /// </summary>
        private float time;

        #endregion
    }
}
