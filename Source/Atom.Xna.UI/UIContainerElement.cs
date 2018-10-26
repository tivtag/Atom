// <copyright file="UIContainerElement.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.UIContainerElement class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents an <see cref="UIElement"/> that contains other UIElements.
    /// </summary>
    /// <remarks>
    /// When <see cref="UIElement.IsVisible"/> or <see cref="UIElement.IsEnabled"/> state of this UIContainerElement
    /// change then the respective states of all its child <see cref="UIElement"/>s also change.
    /// </remarks>
    public abstract class UIContainerElement : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIContainerElement"/> class.
        /// </summary>
        protected UIContainerElement()
            : this( 0 )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIContainerElement"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The initial number of child UIElements the new UIContainerElement can contain.
        /// </param>
        protected UIContainerElement( int capacity )
            : this( string.Empty, capacity )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIContainerElement"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the new UIContainerElement.
        /// </param>
        /// <param name="capacity">
        /// The initial number of child UIElements the new UIContainerElement can contain.
        /// </param>
        protected UIContainerElement( string name, int capacity )
            : base( name )
        {
            this.children = new List<UIElement>( capacity );

            // Hook events instead of overriding to perserv behaviour when overriding.
            this.IsVisibleChanged += this.OnIsVisibleChanged;
            this.IsEnabledChanged += this.OnIsEnabledChanged;
        }

        /// <summary>
        /// Adds the given UIElement to this UIContainerElement.
        /// </summary>
        /// <param name="element">
        /// The UIElement to add.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="element"/> is null.
        /// </exception>
        protected void AddChild( UIElement element )
        {
            Contract.Requires<ArgumentNullException>( element != null );

            if( !this.children.Contains( element ) )
            {
                this.children.Add( element );
            }
        }

        /// <summary>
        /// Called when this UIContainerElement has been shown or hidden.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        private void OnIsVisibleChanged( UIElement sender )
        {
            bool isVisible = this.IsVisible;

            foreach( var child in this.children )
            {
                child.IsVisible = isVisible;
            }
        }

        /// <summary>
        /// Called when this UIContainerElement has been enabled or disabled.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        private void OnIsEnabledChanged( UIElement sender )
        {
            bool isEnabled = this.IsEnabled;

            foreach( var child in this.children )
            {
                child.IsEnabled = isEnabled;
            }
        }

        /// <summary>
        /// Called when this UIContainerElement has been added to the given UserInterface.
        /// </summary>
        /// <param name="userInterface">
        /// The related UserInterface.
        /// </param>
        protected override void OnAdded( UserInterface userInterface )
        {
            foreach( var child in this.children )
            {
                userInterface.AddElement( child );
            }
        }

        /// <summary>
        /// Called when this UIContainerElement has been removed from the given UserInterface.
        /// </summary>
        /// <param name="userInterface">
        /// The related UserInterface.
        /// </param>
        protected override void OnRemoved( UserInterface userInterface )
        {
            foreach( var child in this.children )
            {
                userInterface.RemoveElement( child );
            }
        }

        /// <summary>
        /// The child UIElements of this UIContainerElement.
        /// </summary>
        private readonly List<UIElement> children;
    }
}
