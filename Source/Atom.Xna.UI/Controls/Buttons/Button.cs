// <copyright file="Button.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Controls.Button class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI.Controls
{
    using System;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Represent the abstract base class of all Buttons in the UserInterface framework.
    /// </summary>
    public abstract class Button : UIElement
    {
        #region [ Events ]

        /// <summary>
        /// Fired after the user has clicked on this <see cref="Button"/>.
        /// </summary>
        public event MouseInputEventHandler Clicked;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Button"/> is currently selected.
        /// </summary>
        /// <value>The default value is false.</value>
        public bool IsSelected
        {
            get;
            set;
        }

        #endregion

        #region [ Constructors ]
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        protected Button()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the new <see cref="Button"/>.
        /// </param> 
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="name"/> is null.
        /// </exception>
        protected Button( string name )
            : base( name )
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Simulates clicking on this Button by executing the Clicked event.
        /// </summary>
        public void Click()
        {
            MouseState mouseState;
            MouseState oldMouseState;

            if( this.Owner != null )
            {
                mouseState = this.Owner.MouseState;
                oldMouseState = this.Owner.OldMouseState;
            }
            else
            {
                mouseState = new MouseState();
                oldMouseState = new MouseState();
            }

            this.Click( ref mouseState, ref oldMouseState );
        }

        /// <summary>
        /// Simulates clicking on this Button by executing the Clicked event.
        /// </summary>
        /// <param name="mouseState">
        /// The state of the <see cref="Mouse"/>.
        /// </param>
        /// <param name="oldMouseState">
        /// The state of the <see cref="Mouse"/> one frame ago.
        /// </param>
        public void Click( ref MouseState mouseState, ref MouseState oldMouseState )
        {
            if( this.Clicked != null )
            {
                this.Clicked( this, ref mouseState, ref oldMouseState );
            }
        }

        /// <summary>
        /// Overriden to handle 'clicking' of the button.
        /// </summary>
        /// <param name="mouseState">
        /// The state of the <see cref="Mouse"/>.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        /// <param name="oldMouseState">
        /// The state of the <see cref="Mouse"/> one frame ago.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        /// <returns>
        /// True if input should be passed to elements that are behind 
        /// the <see cref="UIElement"/>, otherwise false.
        /// </returns>
        protected override bool HandleRelatedMouseInput(
            ref MouseState mouseState,
            ref MouseState oldMouseState )
        {
            if( WasClicked( ref mouseState, ref oldMouseState ) )
            {
                this.Click( ref mouseState, ref oldMouseState );
            }

            return this.PassInputToSubElements;
        }

        /// <summary>
        /// Gets a value indicating whether the user has just clicked.
        /// </summary>
        /// <param name="mouseState">
        /// The current state of the mouse.
        /// </param>
        /// <param name="oldMouseState">
        /// The state of the mouse last frame.
        /// </param>
        /// <returns>
        /// True if the button has been clicked on;
        /// otherwise false.
        /// </returns>
        private static bool WasClicked( ref MouseState mouseState, ref MouseState oldMouseState )
        {
            return
                (mouseState.LeftButton == ButtonState.Pressed &&
                 oldMouseState.LeftButton == ButtonState.Released) ||               
                (mouseState.RightButton == ButtonState.Pressed &&
                 oldMouseState.RightButton == ButtonState.Released) ||                
                (mouseState.MiddleButton == ButtonState.Pressed &&
                 oldMouseState.MiddleButton == ButtonState.Released);
        }

        #endregion
    }
}
