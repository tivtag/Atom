// <copyright file="SpriteTextField.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Controls.SpriteTextField class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI.Controls
{
    using System;
    using Atom.Math;

    /// <summary>
    /// Represents a <see cref="TextField"/> that has got a <see cref="ISprite"/>
    /// drawn in the background.
    /// </summary>
    public class SpriteTextField : TextField
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the <see cref="Sprite"/> displayed in the background of this SpriteTextField.
        /// </summary>
        public ISprite Sprite
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the offset applied when drawing the <see cref="Sprite"/>.
        /// </summary>
        public Vector2 SpriteOffset
        {
            get;
            set;
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteTextField"/> class.
        /// </summary>
        public SpriteTextField()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteTextField"/> class.
        /// </summary>
        /// <param name="name">The name of the new <see cref="SpriteTextField"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="name"/> is null.
        /// </exception>
        public SpriteTextField( string name )
            : base( name )
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Called when this SpriteTextField is drawing itself.
        /// </summary>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        protected override void OnDraw( ISpriteDrawContext drawContext )
        {
            if( this.Sprite != null )
                this.Sprite.Draw( this.Position + this.SpriteOffset, this.RelativeDrawOrder, drawContext.Batch );

            base.OnDraw( drawContext );
        }

        /// <summary>
        /// Called when this SpriteTextField is updating itself.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        protected override void OnUpdate( IUpdateContext updateContext )
        {
            var updateable = this.Sprite as IUpdateable;

            if( updateable != null )
            {
                updateable.Update( updateContext );
            }

            base.OnUpdate( updateContext );
        }

        #endregion
    }
}
