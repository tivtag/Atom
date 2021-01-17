// <copyright file="TextField.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Controls.TextField class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI.Controls
{
    using System;
    using Atom.Math;
    using Atom.Xna.Fonts;

    /// <summary>
    /// Represents a simple UIElement
    /// that provides a mechanism to draw <see cref="Text"/>.
    /// </summary>
    public class TextField : UIElement
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the text stored within this <see cref="TextField"/>.
        /// </summary>
        public Text Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                this.RefreshTextClientArea();
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="TextField"/> class.
        /// </summary>
        public TextField()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextField"/> class.
        /// </summary>
        /// <param name="name">The name of the new <see cref="TextField"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="name"/> is null.
        /// </exception>
        public TextField( string name )
            : base( name )
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Called when this TextField is drawing itself.
        /// </summary>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        protected override void OnDraw( ISpriteDrawContext drawContext )
        {
            if( this.text != null )
                this.text.Draw( this.Position, drawContext );
        }

        /// <summary>
        /// Called when this TextField is updating itself.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        protected override void OnUpdate( IUpdateContext updateContext )
        {
            if( this.text != null )
                this.text.Update( updateContext );
        }

        /// <summary>
        /// Refreshes the client area based on the currently set <see cref="Text"/>.
        /// </summary>
        private void RefreshTextClientArea()
        {
            if( this.Text == null )
            {
                this.SetTransform( this.Position, Vector2.Zero, Vector2.Zero );
            }
            else
            {
                Vector2 size = this.text.TextBlockSize;

                switch( this.text.Align )
                {
                    case TextAlign.Right:
                        this.SetTransform( this.Position, new Vector2( -(int)size.X, 0.0f ), size );
                        break;

                    case TextAlign.Center:
                        this.SetTransform( this.Position, new Vector2( -(int)(size.X / 2.0f), 0.0f ), size );
                        break;

                    default:
                        this.SetTransform( this.Position, Vector2.Zero, size );
                        break;
                }
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Stores the text data.
        /// </summary>
        private Text text;

        #endregion
    }
}
