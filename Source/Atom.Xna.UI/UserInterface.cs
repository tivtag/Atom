// <copyright file="UserInterface.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.UserInterface class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Atom.Diagnostics.Contracts;
    using System.Linq;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Manages the drawing, updating and overall input handling
    /// of the UserInterface of an application. 
    /// </summary>
    public class UserInterface
    {
        #region [ Events ]

        /// <summary>
        /// The events which are triggered every frame if the <see cref="UserInterface"/>
        /// has focus.
        /// </summary>
        public event KeyboardInputEventHandler KeyboardInput;

        /// <summary>
        /// The events which are triggered every frame if the <see cref="UserInterface"/>
        /// has focus.
        /// </summary>
        public event MouseInputEventHandler MouseInput;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="UserInterface"/> has focus of the user.
        /// </summary>
        /// <remarks>
        /// Input will only be processed by the UserInterface if this property is true.
        /// </remarks>
        /// <value>The default value is true.</value>
        public bool HasFocus
        {
            get 
            {
                return this.hasFocus;
            }

            set
            {
                this.hasFocus = value;
            }
        }

        /// <summary>
        /// Gets or sets the currently focused <see cref="UIElement"/>.
        /// Only this element can receive keyboard input.
        /// </summary>
        public UIElement FocusedElement
        {
            get
            {
                return this.focusedElement;
            }

            set
            {
                this.focusedElement = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="UIElement"/> that this UserInterface contains.
        /// </summary>
        public IEnumerable<UIElement> Elements
        {
            get
            {
                return this.elements;
            }
        }

        /// <summary>
        /// Gets the current position of the Mouse relative to the Game's window.
        /// </summary>
        /// <value>The position of the mouse.</value>
        public Point2 MousePosition
        {
            get 
            {
                return this.mousePosition;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user currently
        /// presses any of the Shift modifier keys.
        /// </summary>
        public bool IsShiftDown
        {
            get
            {
                return this.keyState.IsKeyDown( Keys.LeftShift ) || this.keyState.IsKeyDown( Keys.RightShift );
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user pressed any of the Shift modifier keys in the last frame.
        /// </summary>
        public bool WasShiftDown
        {
            get
            {
                return this.oldKeyState.IsKeyDown( Keys.LeftShift ) || this.oldKeyState.IsKeyDown( Keys.RightShift );
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user currently
        /// presses any of the Alt modifier keys.
        /// </summary>
        public bool IsAltDown
        {
            get
            {
                return this.keyState.IsKeyDown( Keys.LeftAlt ) || this.keyState.IsKeyDown( Keys.RightAlt );
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user pressed any of the Alt modifier keys in the last frame.
        /// </summary>
        public bool WasAltDown
        {
            get
            {
                return this.oldKeyState.IsKeyDown( Keys.LeftAlt ) || this.oldKeyState.IsKeyDown( Keys.RightAlt );
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user currently
        /// presses any of the Control modifier keys.
        /// </summary>
        public bool IsControlDown
        {
            get
            {
                return this.keyState.IsKeyDown( Keys.LeftControl ) || this.keyState.IsKeyDown( Keys.RightControl );
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user pressed any of the Control modifier keys in the last frame.
        /// </summary>
        public bool WasControlDown
        {
            get
            {
                return this.oldKeyState.IsKeyDown( Keys.LeftControl ) || this.oldKeyState.IsKeyDown( Keys.RightControl );
            }
        }

        /// <summary>
        /// Gets the current state of the <see cref="Keyboard"/>.
        /// </summary>
        public KeyboardState KeyState
        {
            get 
            {
                return this.keyState;
            }
        }

        /// <summary>
        /// Gets the state of the <see cref="Keyboard"/> one frame ago.
        /// </summary>
        public KeyboardState OldKeyState
        {
            get
            {
                return this.oldKeyState;
            }
        }

        /// <summary>
        /// Gets or sets the current state of the <see cref="Mouse"/>.
        /// </summary>
        public MouseState MouseState
        {
            get
            {
                return this.mouseState;
            }

            set
            {
                this.mouseState = value;
            }
        }

        /// <summary>
        /// Gets or sets the state of the <see cref="Mouse"/> one frame ago.
        /// </summary>
        public MouseState OldMouseState
        {
            get
            {
                return this.oldMouseState;
            }

            set
            {
                this.oldMouseState = value;
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether this UserInterface is drawn
        /// when Draw is called.
        /// </summary>
        /// <value>
        /// The default value is true.
        /// </value>
        public bool IsVisible
        {
            get
            {
                return this.isVisible; 
            }

            set
            {
                this.isVisible = value; 
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterface"/> class.
        /// </summary>
        /// <param name="elementCapacity"> 
        /// The initial number of elements the <see cref="UserInterface"/> can contain.
        /// </param>
        public UserInterface( int elementCapacity )
        {
            this.elements = new List<UIElement>( elementCapacity );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Draws this <see cref="UserInterface"/> and its visible <see cref="UIElement"/>s.
        /// </summary>
        /// <param name="drawContext">
        /// The current <see cref="ISpriteDrawContext"/>.
        /// </param>
        public void Draw( ISpriteDrawContext drawContext )
        {
            if( !this.isVisible )
                return;

            Debug.Assert( drawContext != null, "The DrawContext is null." );
            Debug.Assert( drawContext.Batch != null, "The SpriteBatch of the DrawContext is null." );

            if( this.isElementSortNeeded )
            {
                this.SortElements();
            }

            // The elements are first sorted by their FloorNumber
            // and then by their RelativeDrawOrder.
            var batch = drawContext.Batch;

            // Indicates whether the execution is 
            // currently in a SpriteBatch.Begin/End block.
            bool isBeginEndBlock = false;
            int currentFloor = -1;

            // Update mouse position before drawing to reduce mouse lag:
            MouseState mouseState = GetCurrentMouseState();
            this.mousePosition = new Point2( mouseState.X, mouseState.Y );

            try
            {
                // Process all elements:
                foreach( UIElement element in this.elements )
                {
                    if( !element.IsVisible )
                        continue;

                    int floor = element.FloorNumber;

                    // We have to begin a new Begin()/End() block
                    // if the current floor changes
                    if( floor != currentFloor )
                    {
                        if( isBeginEndBlock )
                            batch.End();

                        drawContext.Begin( BlendState.NonPremultiplied, SamplerState.PointClamp, SpriteSortMode.FrontToBack );

                        isBeginEndBlock = true;
                        currentFloor = floor;
                    }

                    // Finally draw the element.
                    element.Draw( drawContext );
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if( isBeginEndBlock )
                    batch.End();
            }
        }

        /// <summary>
        /// Updates this UserInterface.
        /// </summary>
        /// <param name="updateContext">
        /// The current <see cref="IUpdateContext"/>.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            if( !this.HasFocus )
                return;

            if( this.isElementSortNeeded )
                this.SortElements();

            for( int i = 0; i < this.elements.Count; ++i )
            {
                this.elements[i].PreUpdateInternal();
            }

            if( this.ShouldHandleInput() )
            {
                this.HandleMouseInput();
                this.HandleKeyInput();
            }

            for( int i = 0; i < elements.Count; ++i )
            {
                this.elements[i].Update( updateContext );
            }
        }

        #region > Organisation <

        #region - Add -

        /// <summary>
        /// Adds the given <see cref="UIElement"/> to this <see cref="UserInterface"/>.
        /// </summary>
        /// <param name="element">
        /// The element to add.
        /// </param>
        public void AddElement( UIElement element )
        {
            Contract.Requires<ArgumentNullException>( element != null );
            Contract.Requires<ArgumentException>( element.Owner == this || !this.ContainsElement( element ) );

            this.AddElementUnsafe( element );
        }

        /// <summary>
        /// Adds the specified UIElement without validating it.
        /// </summary>
        /// <param name="element">
        /// The element to add.
        /// </param>
        private void AddElementUnsafe( UIElement element )
        {
            if( element.Owner == this )
                return;

            this.elements.Add( element );
            element.Owner = this;

            this.NotifyElementSortNeeded();
            element.OnAddedInternal( this );
        }

        /// <summary>
        /// Adds the given <see cref="UIElement"/>s to this <see cref="UserInterface"/>.
        /// </summary>
        /// <param name="elementsToAdd">
        /// The elements to add.
        /// </param>
        public void AddElements( IEnumerable<UIElement> elementsToAdd )
        {
            Contract.Requires<ArgumentNullException>( elementsToAdd != null );
            Contract.Requires<ArgumentException>( Contract.ForAll<UIElement>( elementsToAdd, element => elementsToAdd != null ) );
            Contract.Requires<ArgumentException>( Contract.ForAll<UIElement>( elementsToAdd, element => element.Owner == this || !this.ContainsElement( element ) ) );

            foreach( var element in elementsToAdd )
            {
                this.AddElementUnsafe( element );
            }
        }

        #endregion

        #region - Remove -

        /// <summary>
        /// Tries to remove the specified <see cref="UIElement"/> from this <see cref="UserInterface"/>.
        /// </summary>
        /// <param name="element">
        /// The element to remove.
        /// </param>
        /// <returns> 
        /// Returns <see langword="true"/> if it has been removed;
        /// otherwhise <see langword="false"/>. 
        /// </returns>
        public bool RemoveElement( UIElement element )
        {
            Contract.Requires<ArgumentNullException>( element != null );

            if( this.elements.Remove( element ) )
            {
                element.Owner = null;
                element.OnRemovedInternal( this );
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to removes the specified UIElements from this UserInterface.
        /// </summary>
        /// <param name="elementsToRemove">
        /// The UIElement to remove.
        /// </param>
        public void RemoveElements( IEnumerable<UIElement> elementsToRemove )
        {
            Contract.Requires<ArgumentNullException>( elementsToRemove != null );
            Contract.Requires<ArgumentException>( Contract.ForAll<UIElement>( elementsToRemove, element => elementsToRemove != null ) );

            foreach( var element in elementsToRemove )
            {
                this.RemoveElement( element );                
            }
        }

        /// <summary>
        /// Removes all <see cref="UIElement"/>s from the <see cref="UserInterface"/>.
        /// </summary>
        public void RemoveAllElements()
        {
            var oldElements = this.elements.ToArray();

            this.focusedElement = null;
            this.elements.Clear();
            this.elementsInRange.Clear();

            foreach( var element in oldElements )
            {
                element.Owner = null;
                element.OnRemovedInternal( this );
            }
        }

        #endregion

        #region - Get -

        /// <summary>
        /// Gets the first <see cref="UIElement"/> that has the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name"> The name of the element to get. Can be null. </param>
        /// <returns> The element if found; otherwise null. </returns>
        public UIElement GetElement( string name )
        {
            if( name == null )
                return null;

            return this.elements.First( element => name.Equals( element.Name, StringComparison.Ordinal ) );
        }

        /// <summary>
        /// Gets the first <see cref="UIElement"/> that has the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the element to receive.
        /// </typeparam>
        /// <returns> The element if found; otherwise null. </returns>
        public T GetElement<T>()
            where T : UIElement
        {
            return this.GetElement( typeof( T ) ) as T;
        }

        /// <summary>
        /// Gets the first <see cref="UIElement"/> that has the specified <see cref="Type"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="type"/> is null.
        /// </exception>
        /// <param name="type">
        /// The type of the element to receive.
        /// </param>
        /// <returns>
        /// The element if found; otherwise null.
        /// </returns>
        public UIElement GetElement( Type type )
        {
            Contract.Requires<ArgumentNullException>( type != null );

            return elements.First( element => type == element.GetType() );
        }

        #endregion

        #region - Contains -
        
        /// <summary>
        /// Gets a value indicating whether this UserInterface contains the specified <see cref="UIElement"/>.
        /// </summary>
        /// <param name="element">
        /// The UIElement to locate.
        /// </param>
        /// <returns>
        /// True if this UserInterface contains the given UIElement;
        /// otherwise false.
        /// </returns>
        [Pure]
        public bool ContainsElement( UIElement element )
        {
            return this.elements.Contains( element );
        }

        #endregion

        #endregion

        #region > Sorting <

        /// <summary>
        /// Notifies this <see cref="UserInterface"/> that its UIElements need to be sorted.
        /// </summary>
        internal void NotifyElementSortNeeded()
        {
            this.isElementSortNeeded = true;
        }

        /// <summary>
        /// Sorts the UIElements of this <see cref="UserInterface"/>.
        /// </summary>
        private void SortElements()
        {
            this.elements.Sort( ElementSortComparision );
        }

        /// <summary> 
        /// Compares two <see cref="UIElement"/> instances.
        /// </summary>
        /// <param name="x">The first UIElement instance.</param>
        /// <param name="y">The second UIElement instance.</param>
        /// <returns>A value that can be used for comparison.</returns>
        private static int CompareElements( UIElement x, UIElement y )
        {
            // x and y are never null.
            int floorX = x.FloorNumber;
            int floorY = y.FloorNumber;

            if( floorX < floorY )
                return -1;
            else if( floorX > floorY )
                return 1;

            float relativeDrawOrderX = x.RelativeDrawOrder;
            float relativeDrawOrderY = y.RelativeDrawOrder;

            if( relativeDrawOrderX < relativeDrawOrderY )
                return -1;
            else if( relativeDrawOrderX > relativeDrawOrderY )
                return 1;
            else
                return 0;
        }

        #endregion

        #region > Input Handling <

        /// <summary>
        /// Gets a value indicating whether the specified Key is currently being pressed.
        /// </summary>
        /// <param name="key">
        /// The key to query.
        /// </param>
        /// <returns>
        /// true if they key is currently being pressed;
        /// otherwise false.
        /// </returns>
        public bool IsKeyDown( Keys key )
        {
            return this.keyState.IsKeyDown( key );
        }

        /// <summary>
        /// Gets a value indicating whether the specified Key is currently not being pressed.
        /// </summary>
        /// <param name="key">
        /// The key to query.
        /// </param>
        /// <returns>
        /// true if they key is currently not being pressed;
        /// otherwise false.
        /// </returns>
        public bool IsKeyUp( Keys key )
        {
            return this.keyState.IsKeyUp( key );
        }

        /// <summary>
        /// Gets a value indicating whether the specified Key was being pressed one frame ago.
        /// </summary>
        /// <param name="key">
        /// The key to query.
        /// </param>
        /// <returns>
        /// true if they key was being pressed;
        /// otherwise false.
        /// </returns>
        public bool WasKeyDown( Keys key )
        {
            return this.oldKeyState.IsKeyDown( key );
        }

        /// <summary>
        /// Gets a value indicating whether the specified Key was not being pressed one frame ago.
        /// </summary>
        /// <param name="key">
        /// The key to query.
        /// </param>
        /// <returns>
        /// true if they key was not being pressed;
        /// otherwise false.
        /// </returns>
        public bool WasKeyUp( Keys key )
        {
            return this.oldKeyState.IsKeyUp( key );
        }

        /// <summary>
        /// Gets a value indicating whether this UserInterface should
        /// be currentl handle input from the user. Can be overwritten to customize.
        /// </summary>
        /// <returns>
        /// Always returns true by default.
        /// </returns>
        protected virtual bool ShouldHandleInput()
        {
            return true;
        }

        /// <summary>
        /// Refreshes the current input state values
        /// without processing the result.
        /// </summary>
        public void RefreshMouseInputState()
        {
            this.mouseState = this.GetCurrentMouseState();
            this.mousePosition = new Point2( this.mouseState.X, this.mouseState.Y );

            this.oldMouseState = this.mouseState;
        }

        /// <summary>
        /// Handles mouse input.
        /// </summary>
        private void HandleMouseInput()
        {
            this.mouseState = this.GetCurrentMouseState();
            this.mousePosition = new Point2( this.mouseState.X, this.mouseState.Y );

            ProcessMouseInput();

            this.oldMouseState = this.mouseState;
        }

        /// <summary>
        /// Processes the current state of the mouse for all UIElements.
        /// </summary>
        public void ProcessMouseInput()
        {
            this.RefreshElementsInRange( this.mouseState.X, this.mouseState.Y );

            for( int i = this.elementsInRange.Count - 1; i >= 0; --i )
            {
                var element = this.elementsInRange[i];
                bool pass = element.MouseOverInternal( ref mouseState );

                if( !pass )
                    break;
            }

            if( this.mouseState != this.oldMouseState )
            {
                for( int i = this.elementsInRange.Count - 1; i >= 0; --i )
                {
                    var element = this.elementsInRange[i];
                    bool pass = element.HandleRelatedMouseInputCore( ref this.mouseState, ref this.oldMouseState );

                    if( !pass )
                        break;
                }
            }

            for( int i = this.elements.Count - 1; i >= 0; --i )
            {
                this.elements[i].HandleMouseInputCore( ref this.mouseState, ref this.oldMouseState );
            }

            if( this.MouseInput != null )
            {
                this.MouseInput( this, ref mouseState, ref oldMouseState );
            }
        }
        
        /// <summary>
        /// Gets the current state of the mouse
        /// which will get used and cached by the UserInterface.
        /// </summary>
        /// <returns>
        /// The current state of the mouse.
        /// </returns>
        protected virtual MouseState GetCurrentMouseState()
        {
            return Mouse.GetState();
        }

        /// <summary>
        /// Handles keyboard input.
        /// </summary>
        private void HandleKeyInput()
        {
            keyState = Keyboard.GetState();

            if( KeyboardInput != null )
                KeyboardInput( this, ref keyState, ref oldKeyState );

            if( focusedElement != null )
                focusedElement.HandleKeyInputCore( ref keyState, ref oldKeyState );

            oldKeyState = keyState;
        }

        /// <summary>
        /// Refreshes the list of <see cref="UIElement"/>s which are in range 
        /// of the mouse.
        /// </summary>
        /// <param name="mouseX">The position of the mouse on the x-axis.</param>
        /// <param name="mouseY">The position of the mouse on the y-axis.</param>
        private void RefreshElementsInRange( int mouseX, int mouseY )
        {
            elementsInRange.Clear();

            for( int i = 0; i < elements.Count; ++i )
            {
                UIElement element = elements[i];

                if( element.IsEnabled && element.ClientArea.Contains( mouseX, mouseY ) )
                    elementsInRange.Add( element );
            }

            elementsInRange.Sort( ElementSortComparision );
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The state of the Mouse.
        /// </summary>
        private MouseState mouseState, oldMouseState;

        /// <summary>
        /// The state of the Keyboard.
        /// </summary>
        private KeyboardState keyState, oldKeyState;

        /// <summary>
        /// The last known position of the mouse.
        /// </summary>
        private Point2 mousePosition;

        /// <summary>
        /// States whether the <see cref="UserInterface"/> has focus of the user currently.
        /// </summary>
        private bool hasFocus = true;

        /// <summary>
        /// The element which is currently focused by the user if any.
        /// </summary>
        private UIElement focusedElement;

        /// <summary>
        /// The list of all elements in this <see cref="UserInterface"/>.
        /// </summary>
        private readonly List<UIElement> elements;

        /// <summary>
        /// The list of elements that are currently in range of the Input Device.
        /// </summary>
        private readonly List<UIElement> elementsInRange = new List<UIElement>();

        /// <summary>
        /// States whether this UserInterface needs to sort its elements.
        /// </summary>
        private bool isElementSortNeeded;

        /// <summary>
        /// Stores the delegate that is used to compare two UIElements while sorting.
        /// </summary>
        private static readonly Comparison<UIElement> ElementSortComparision = 
            new Comparison<UIElement>( UserInterface.CompareElements );

        private bool isVisible = true;

        #endregion
    }
}
