// <copyright file="UIElement.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.UIElement class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.Math;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Represents the abtract base class of all elements in a <see cref="UserInterface"/>.
    /// </summary>
    public abstract class UIElement : IFloorDrawable, IUpdateable
    {
        #region [ Events ]

        /// <summary>
        /// Fired when the <see cref="IsEnabled"/> state of
        /// this this <see cref="UIElement"/> has changed.
        /// </summary>
        public event SimpleEventHandler<UIElement> IsEnabledChanged;

        /// <summary>
        /// Fired when the <see cref="IsVisible"/> state of
        /// this this <see cref="UIElement"/> has changed.
        /// </summary>
        public event SimpleEventHandler<UIElement> IsVisibleChanged;

        /// <summary>
        /// Fired when the <see cref="FloorNumber"/> of this <see cref="UIElement"/> has changed.
        /// </summary>
        public event SimpleEventHandler<UIElement> FloorNumberChanged;

        /// <summary>
        /// Fired when the mouse is entering this <see cref="UIElement"/>.
        /// </summary>
        public event SimpleEventHandler<UIElement> MouseEntering;

        /// <summary>
        /// Fired when the mouse is leaving this <see cref="UIElement"/>.
        /// </summary>
        public event SimpleEventHandler<UIElement> MouseLeaving;

        #endregion

        #region [ Properties ]

        #region > Identification <

        /// <summary>
        /// Gets or sets the name of this <see cref="UIElement"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the given value is null.
        /// </exception>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                Contract.Requires<ArgumentNullException>( value != null );

                this.name = value;
            }
        }

        #endregion

        #region > State <

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="UIElement"/> is enabled.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool IsEnabled
        {
            get
            {
                return this._isEnabled;
            }

            set
            {
                if( value == this._isEnabled )
                    return;
                
                this._isEnabled = value;

                if( !value )
                {
                    this.isMouseOver = this.wasMouseOver = false;
                }

                this.OnIsEnabledChangedPrivate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="UIElement"/> is visible.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool IsVisible
        {
            get
            {
                return this._isVisible;
            }

            set
            {
                if( value == this._isVisible )
                    return;

                this._isVisible = value;
                this.OnIsVisibleChangedPrivate();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is currently over this <see cref="UIElement"/>.
        /// </summary>
        public bool IsMouseOver
        {
            get 
            {
                return this.isMouseOver;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UIElement"/> passes input to elements behind it.
        /// </summary>
        /// <value>The default value is true. Returns true if the element if disabled.</value>
        public bool PassInputToSubElements
        {
            get
            {
                if( !this._isEnabled )
                    return true;

                return this.passInputToSubElements;
            }

            set
            {
                this.passInputToSubElements = value;
            }
        }

        #endregion

        #region > Settings <

        /// <summary>
        /// Gets or sets the value that indicates on what layer this <see cref="UIElement"/> is on.
        /// </summary>
        /// <value>The default value is zero.</value>
        public int FloorNumber
        {
            get
            {
                return this._floorNumber;
            }

            set
            {
                if( value == this._floorNumber )
                    return;

                this._floorNumber = value;
                this.FloorNumberChanged.Raise( this );
            }
        }

        /// <summary>
        /// Gets or sets the draw order of this <see cref="UIElement"/> relative to other <see cref="UIElement"/>s
        /// that have the same <see cref="FloorNumber"/>. This is a value between 0.0f and 1.0f.
        /// </summary>
        /// <value>The default value is zero.</value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If the given value is outside of the valid range of [0; 1].
        /// </exception>
        public float RelativeDrawOrder
        {
            get
            {
                return this._relativeDrawOrder;
            }

            set
            {
                Contract.Requires<ArgumentOutOfRangeException>( value >= 0.0f && value <= 1.0f );

                this._relativeDrawOrder = value;

                if( this.Owner != null )
                    this.Owner.NotifyElementSortNeeded();
            }
        }

        #endregion

        #region > Transform & Collision <

        /// <summary>
        /// Gets or sets the position of this <see cref="UIElement"/>.
        /// </summary>
        /// <value>The position of the element.</value>
        public Vector2 Position
        {
            get
            {
                return this._position;
            }

            set
            {
                if( value == this._position )
                    return;

                this._position = value;
                this.RefreshClientArea();
            }
        }

        /// <summary>
        /// Gets the position of this <see cref="UIElement"/> on the x-axis.
        /// </summary>
        /// <value>The position on the x-axis of the element.</value>
        protected float X
        {
            get 
            {
                return this._position.X;
            }
        }

        /// <summary>
        /// Gets the position of this <see cref="UIElement"/> on the y-axis.
        /// </summary>
        /// <value>The position on the y-axis of the element.</value>
        protected float Y
        {
            get
            { 
                return this._position.Y;
            }
        }

        /// <summary>
        /// Gets or sets the offset from the <see cref="Position"/> 
        /// of this <see cref="UIElement"/> to the start of the <see cref="ClientArea"/>.
        /// </summary>
        /// <value>The offset from the Position of the element to the start of the ClientArea.</value>
        public Vector2 Offset
        {
            get
            {
                return this._offset;
            }

            set
            {
                if( value == this._offset )
                    return;

                this._offset = value;
                this.RefreshClientArea();
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref="UIElement"/>.
        /// </summary>
        /// <value>The size of the element in pixels.</value>
        public Vector2 Size
        {
            get
            {
                return this._size;
            }

            set
            {
                if( value == this._size )
                    return;

                this._size = value;
                this.RefreshClientArea();
            }
        }

        /// <summary>
        /// Gets the width of this <see cref="UIElement"/>.
        /// </summary>
        /// <value>The width of the element in pixels.</value>
        protected float Width
        {
            get 
            {
                return this._size.X;
            }
        }

        /// <summary>
        /// Gets the height of this <see cref="UIElement"/>.
        /// </summary>
        /// <value>The height of the element in pixels.</value>
        protected float Height
        {
            get 
            {
                return this._size.Y;
            }
        }

        /// <summary>
        /// Gets the area this UIEelement covers.
        /// </summary>
        /// <remarks>
        /// Related mouse input is only passed to the UIElement 
        /// if the mouse is inside the UIElements ClientArea.
        /// </remarks>
        /// <value>The area the elements covers.</value>
        public RectangleF ClientArea
        {
            get 
            {
                return this._clientArea;
            }
        }

        #endregion

        #region > Misc <

        /// <summary>
        /// Gets the <see cref="UserInterface"/> that owns this UIElement.
        /// </summary>
        public UserInterface Owner
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets an optional user object.
        /// </summary>
        public object Tag
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElement"/> class.
        /// </summary>
        protected UIElement()
            : this( string.Empty )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElement"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the new <see cref="UIElement"/>.
        /// </param>
        protected UIElement( string name )
        {
            Contract.Requires<ArgumentNullException>( name != null );

            this.name = name;
        }

        #endregion

        #region [ Methods ]

        #region > Updating <

        /// <summary>
        /// Draws this <see cref="UIElement"/>.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            if( !_isEnabled )
                return;

            this.OnUpdate( updateContext );

            if( this.MouseLeaving != null )
            {
                if( this.wasMouseOver && !this.isMouseOver )
                {
                    this.MouseLeaving( this );
                }
            }
        }

        /// <summary>
        /// Called when this <see cref="UIElement"/> is updating.
        /// </summary>
        /// <param name="updateContext">
        /// The current <see cref="IUpdateContext"/>.
        /// </param>
        protected abstract void OnUpdate( IUpdateContext updateContext );

        /// <summary>
        /// Called at the beginning of a new frame - before the real update.
        /// Mostly used to reset items.
        /// </summary>
        internal void PreUpdateInternal()
        {
            if( !this._isEnabled )
                return;

            this.wasMouseOver = this.isMouseOver;
            this.isMouseOver = false;
            this.OnPreUpdate();
        }

        /// <summary>
        /// Called at the beginning of a new frame - before the real update.
        /// Mostly used to reset items.
        /// </summary>
        protected virtual void OnPreUpdate()
        {
        }

        #endregion

        #region > Drawing <

        /// <summary>
        /// Draws this <see cref="UIElement"/>.
        /// </summary>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        public void Draw( ISpriteDrawContext drawContext )
        {
            if( !this._isVisible )
                return;

            this.OnDraw( drawContext );
        }

        /// <summary>
        /// Draws this <see cref="UIElement"/>.
        /// </summary>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        void IDrawable.Draw( IDrawContext drawContext )
        {
            this.Draw( (ISpriteDrawContext)drawContext );
        }

        /// <summary>
        /// Called when this <see cref="UIElement"/> is drawing.
        /// </summary>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        protected abstract void OnDraw( ISpriteDrawContext drawContext );

        #endregion

        #region > Events <

        /// <summary>
        /// Fires the <see cref="IsEnabledChanged"/> event.
        /// </summary>
        private void OnIsEnabledChangedPrivate()
        {
            if( this.IsEnabledChanged != null )
                this.IsEnabledChanged( this );

            this.OnIsEnabledChanged();
        }

        /// <summary>
        /// Gets called when this UIElement has been enabled or disabled.
        /// </summary>
        protected virtual void OnIsEnabledChanged()
        {
        }

        /// <summary>
        /// Fires the <see cref="IsVisibleChanged"/> event.
        /// </summary>
        private void OnIsVisibleChangedPrivate()
        {
            if( this.IsVisibleChanged != null )
                this.IsVisibleChanged( this );

            this.OnIsVisibleChanged();
        }

        /// <summary>
        /// Gets called when this UIElement <see cref="IsVisible"/> state has changed.
        /// </summary>
        protected virtual void OnIsVisibleChanged()
        {
        }

        #endregion

        #region > State <

        /// <summary>
        /// Shows and enables this UIElement 
        /// by setting <see cref="IsVisible"/> and <see cref="IsEnabled"/> to true.
        /// </summary>
        public void ShowAndEnable()
        {
            this.IsVisible = true;
            this.IsEnabled = true;
        }

        /// <summary>
        /// Hides and disables this UIElement 
        /// by setting <see cref="IsVisible"/> and <see cref="IsEnabled"/> to false.
        /// </summary>
        public void HideAndDisable()
        {
            this.IsVisible = false;
            this.IsEnabled = false;
        }

        /// <summary>
        /// Hides and disables this UIElement by setting <see cref="IsVisible"/> and <see cref="IsEnabled"/> to false.
        /// No events are triggered.
        /// </summary>
        protected void HideAndDisableNoEvent()
        {
            this._isVisible = false;
            this._isEnabled = false;
        }

        /// <summary>
        /// Internal method that gets called when the <see cref="UIElement"/>
        /// was added to the given <see cref="UserInterface"/>.
        /// </summary>
        /// <param name="userInterface">
        /// The related UserInterface.
        /// </param>
        internal void OnAddedInternal( UserInterface userInterface )
        {
            this.OnAdded( userInterface );
        }

        /// <summary>
        /// Internal method that gets called when the <see cref="UIElement"/>
        /// was removed from the given <see cref="UserInterface"/>.
        /// </summary>
        /// <param name="userInterface">
        /// The related UserInterface.
        /// </param>
        internal void OnRemovedInternal( UserInterface userInterface )
        {
            this.OnRemoved( userInterface );
        }

        /// <summary>
        /// Gets called when the <see cref="UIElement"/>
        /// was added to the given <see cref="UserInterface"/>.
        /// </summary>
        /// <param name="userInterface">
        /// The related UserInterface object.
        /// </param>
        protected virtual void OnAdded( UserInterface userInterface )
        {
        }

        /// <summary>
        /// Gets called when the <see cref="UIElement"/>
        /// was removed from the given <see cref="UserInterface"/>.
        /// </summary>
        /// <param name="userInterface">
        /// The related UserInterface object.
        /// </param>
        protected virtual void OnRemoved( UserInterface userInterface )
        {
        }

        #endregion

        #region > Input <

        /// <summary>
        /// Called when the mouse is over the <see cref="ClientArea"/> of this <see cref="UIElement"/>.
        /// </summary>
        /// <param name="mouseState"> 
        /// The state of the mouse at the moment of entering. 
        /// Passed by reference to reduce overhead.
        /// </param>
        /// <returns> 
        /// Returns <see langword="true"/> if mouse input should be passed 
        /// to elements that are behind this <see cref="UIElement"/>;
        /// or otherwise <see langword="false"/> if input handling should stop.
        /// </returns>
        internal bool MouseOverInternal( ref MouseState mouseState )
        {
            if( !this._isEnabled )
                return true;

            if( this.wasMouseOver == false && this.MouseEntering != null )
            {
                this.MouseEntering( this );
            }

            this.isMouseOver = true;
            return this.OnMouseOver( ref mouseState );
        }

        /// <summary>
        /// Called when MouseInput happens that is related to this <see cref="UIElement"/>;
        /// aka. inside the Element.
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
        /// Returns <see langword="true"/> if input should be passed to elements that are behind 
        /// the <see cref="UIElement"/>, otherwise <see langword="false"/>.
        /// </returns>
        public bool HandleRelatedMouseInputCore(
            ref Microsoft.Xna.Framework.Input.MouseState mouseState,
            ref Microsoft.Xna.Framework.Input.MouseState oldMouseState )
        {
            if( !this._isEnabled )
                return true;

            return this.HandleRelatedMouseInput( ref mouseState, ref oldMouseState );
        }

        /// <summary>
        /// Called every frame when this <see cref="UIElement"/> is focused by its owning <see cref="UserInterface"/>.
        /// </summary>
        /// <param name="keyState">
        /// The state of the <see cref="Keyboard"/>.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        /// <param name="oldKeyState">
        /// The state of the <see cref="Keyboard"/> one frame ago.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        public void HandleKeyInputCore(
            ref Microsoft.Xna.Framework.Input.KeyboardState keyState,
            ref Microsoft.Xna.Framework.Input.KeyboardState oldKeyState )
        {
            this.HandleKeyInput( ref keyState, ref oldKeyState );
        }

        /// <summary>
        /// Called once every frame, allows this <see cref="UIElement"/> to
        /// react to mouse input.
        /// </summary>
        /// <param name="mouseState">
        /// The state of the <see cref="Mouse"/>.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        /// <param name="oldMouseState">
        /// The state of the <see cref="Mouse"/> one frame ago.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        public void HandleMouseInputCore(
            ref Microsoft.Xna.Framework.Input.MouseState mouseState,
            ref Microsoft.Xna.Framework.Input.MouseState oldMouseState )
        {
            this.HandleMouseInput( ref mouseState, ref oldMouseState );
        }

        /// <summary>
        /// Called when MouseInput happens that is related to this <see cref="UIElement"/>;
        /// aka. inside the Element.
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
        /// Returns <see langword="true"/> if input should be passed to elements that are behind 
        /// the <see cref="UIElement"/>, otherwise <see langword="false"/>.
        /// </returns>
        protected virtual bool HandleRelatedMouseInput(
            ref Microsoft.Xna.Framework.Input.MouseState mouseState,
            ref Microsoft.Xna.Framework.Input.MouseState oldMouseState )
        {
            return this.PassInputToSubElements;
        }

        /// <summary>
        /// Called every frame when this <see cref="UIElement"/> is focused by its owning <see cref="UserInterface"/>.
        /// </summary>
        /// <param name="keyState">
        /// The state of the <see cref="Keyboard"/>.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        /// <param name="oldKeyState">
        /// The state of the <see cref="Keyboard"/> one frame ago.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        protected virtual void HandleKeyInput(
            ref Microsoft.Xna.Framework.Input.KeyboardState keyState,
            ref Microsoft.Xna.Framework.Input.KeyboardState oldKeyState )
        {
        }

        /// <summary>
        /// Called once every frame, allows this <see cref="UIElement"/> to
        /// react to mouse input.
        /// </summary>
        /// <param name="mouseState">
        /// The state of the <see cref="Mouse"/>.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        /// <param name="oldMouseState">
        /// The state of the <see cref="Mouse"/> one frame ago.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        protected virtual void HandleMouseInput(
            ref Microsoft.Xna.Framework.Input.MouseState mouseState,
            ref Microsoft.Xna.Framework.Input.MouseState oldMouseState )
        {
        }

        /// <summary>
        /// Called when the mouse is over the <see cref="ClientArea"/> of this <see cref="UIElement"/>.
        /// </summary>
        /// <param name="mouseState">
        /// The state of the mouse. Passed by reference to reduce overhead.
        /// </param>
        /// <returns>
        /// True if input should be passed to elements that are behind 
        /// the <see cref="UIElement"/>, otherwise false.
        /// </returns>
        protected virtual bool OnMouseOver( ref MouseState mouseState )
        {
            return this.PassInputToSubElements;
        }

        #endregion

        #region > Misc <

        /// <summary>
        /// Sets all of the transform settings of this UIElement all at once.
        /// </summary>
        /// <param name="position">
        /// The position to set.
        /// </param>
        /// <param name="offset">
        /// The offset to set.
        /// </param>
        /// <param name="size">
        /// The size to set.
        /// </param>
        public void SetTransform( Vector2 position, Vector2 offset, Vector2 size )
        {
            this._position = position;
            this._offset   = offset;
            this._size     = size;

            this.RefreshClientArea();
        }

        /// <summary>
        /// Refreshes the <see cref="ClientArea"/> of this UIElement.
        /// </summary>
        private void RefreshClientArea()
        {
            this._clientArea = new RectangleF(
                this._position.X + this._offset.X,
                this._position.Y + this._offset.Y,
                this._size.X,
                this._size.Y
            );

            this.OnClientAreaChanged();
        }

        /// <summary>
        /// Called when the <see cref="ClientArea"/> of this UIElement has been refreshed.
        /// </summary>
        protected virtual void OnClientAreaChanged()
        {
        }

        #endregion

        #endregion

        #region [ Fields ]

        #region > Transform & Collision <

        /// <summary>
        /// Internal storage of the <see cref="Position"/> property.
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// Internal storage of the <see cref="Offset"/> property.
        /// </summary>
        private Vector2 _offset;

        /// <summary>
        /// Internal storage of the <see cref="Size"/> property.
        /// </summary>
        private Vector2 _size;

        /// <summary>
        /// Internal storage of the <see cref="ClientArea"/> property.
        /// </summary>
        private RectangleF _clientArea;

        #endregion

        #region > Settings <

        /// <summary>
        /// The name of this <see cref="UIElement"/>.
        /// </summary>
        private string name;

        /// <summary>
        /// Internal storage of the <see cref="FloorNumber"/> property.
        /// </summary>
        private int _floorNumber;

        /// <summary>
        /// Internal storage of the <see cref="RelativeDrawOrder"/> property.
        /// </summary>
        private float _relativeDrawOrder;

        /// <summary>
        /// Internal storage of the <see cref="IsEnabled"/> property.
        /// </summary>
        private bool _isEnabled = true;

        /// <summary>
        /// Internal storage of the <see cref="IsVisible"/> property.
        /// </summary>
        private bool _isVisible = true;

        /// <summary>
        /// States whether the input is passed to elements behind this UIElement.
        /// </summary>
        private bool passInputToSubElements = true;

        #endregion

        #region > State <

        /// <summary> 
        /// States whether the mouse is currently over the <see cref="UIElement"/>.
        /// </summary>
        private bool isMouseOver;

        /// <summary> 
        /// States whether the mouse was over the <see cref="UIElement"/> last frame.
        /// </summary>
        private bool wasMouseOver;

        #endregion

        #endregion
    }
}
