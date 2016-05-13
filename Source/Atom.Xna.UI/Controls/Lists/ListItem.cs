// <copyright file="ListItem.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Controls.ListItem{T} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI.Controls
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.Math;

    /// <summary>
    /// Represents an item in a <see cref="ListBox{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value stored in an ListItem{T}.
    /// </typeparam>
    public abstract class ListItem<T> : UIElement
    {
        /// <summary>
        /// Gets or sets the value that is stored in this ListItem{T}.
        /// </summary>
        public T Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
                this.OnValueChanged();
            }
        }

        /// <summary>
        /// Gets the area this ListItem{T} covers in its default position;
        /// when no scrolling is applied.
        /// </summary>
        public RectangleF UnscrolledClientArea
        {
            get
            {
                return new RectangleF(
                    this.unscrolledPosition,
                    this.Size
                );
            }
        }

        /// <summary>
        /// Gets the default position of this ListItem{T};
        /// when no scrolling is applied.
        /// </summary>
        public Vector2 UnscrolledPosition
        {
            get
            {
                return this.unscrolledPosition;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this ListItem{T} is currently selected by the user.
        /// </summary>
        public bool IsSelected
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the ListBox{T} that owns this ListItem{T}.
        /// </summary>
        protected ListBox<T> List
        {
            get
            {
                return this.list;
            }
        }
        
        /// <summary>
        /// Gets the zero-based index of this ListItem{T} into the ListBox{T} that owns it.
        /// </summary>
        protected int ListIndex
        {
            get;
            private set; 
        }

        /// <summary>
        /// Initializes a new instance of the ListItem{T} class.
        /// </summary>
        /// <param name="list">
        /// The ListBox{T} that owns the new ListItem{T}.
        /// </param>
        protected ListItem( ListBox<T> list )
        {
            Contract.Requires<ArgumentNullException>( list != null );

            this.list = list;
            this.IsVisible = false;
            this.IsEnabled = false;
            this.PassInputToSubElements = false;
        }

        /// <summary>
        /// Called when the <see cref="Value"/> of this ListItem{T} has changed.
        /// </summary>
        protected virtual void OnValueChanged()
        {
        }

        /// <summary>
        /// Positions this ListItem{T} during the layout phase of the ListBox{T}
        /// that owns it.
        /// </summary>
        /// <param name="listIndex">
        /// The index of this ListItem{T} into the ListBox{T} that owns it.
        /// </param>
        /// <param name="unscrolledPosition">
        /// The initial position of this ListItem{T}.
        /// </param>
        internal void Layout( int listIndex, Vector2 unscrolledPosition )
        {
            this.unscrolledPosition = unscrolledPosition;
            this.ListIndex = listIndex;
            this.Position = unscrolledPosition;
        }

        /// <summary>
        /// Called when MouseInput happens that is related to this <see cref="UIElement"/>;
        /// aka. inside the Element.
        /// </summary>
        /// <param name="mouseState">
        /// The state of the <see cref="Microsoft.Xna.Framework.Input.Mouse"/>.
        /// </param>
        /// <param name="oldMouseState">
        /// The state of the <see cref="Microsoft.Xna.Framework.Input.Mouse"/> one frame ago.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if input should be passed to elements that are behind 
        /// the <see cref="UIElement"/>, otherwise <see langword="false"/>.
        /// </returns>
        protected override bool HandleRelatedMouseInput( 
            ref Microsoft.Xna.Framework.Input.MouseState mouseState,
            ref Microsoft.Xna.Framework.Input.MouseState oldMouseState )
        {
            if( mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released )
            {
                this.list.Select( this );
            }

            return this.PassInputToSubElements;
        }

        /// <summary>
        /// Called when this ListItem{T} is updating itself.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        protected override void OnUpdate( IUpdateContext updateContext )
        {
            // no op.
        }

        /// <summary>
        /// Represents the storage field of the <see cref="Value"/> property.
        /// </summary>
        private T value;

        /// <summary>
        /// Caches the initial position of this ListItem;
        /// with the scrolling value not applied.
        /// </summary>
        private Vector2 unscrolledPosition;

        /// <summary>
        /// The ListBox{T} that owns the new ListItem.
        /// </summary>
        private readonly ListBox<T> list;
    }
}
