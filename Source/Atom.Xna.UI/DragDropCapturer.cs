// <copyright file="DragDropCapturer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.DragDropCapturer class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI
{
    using Microsoft.Xna.Framework.Input;
    using Atom.Math;

    /// <summary>
    /// Implements a simple mechanism that captures dragging operations
    /// of the user.
    /// </summary>
    public class DragDropCapturer
    {
        /// <summary>
        /// Gets a value indicating whether the use is currently executing
        /// a dragging operation with his mouse.
        /// </summary>
        public bool IsDragging
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of pixels the user has moved his mouse last frame.
        /// </summary>
        public Point2 DeltaMovement
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last captured mouse position.
        /// </summary>
        public Point2 Position
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the drag offset that has been captured.
        /// </summary>
        /// <remarks>
        /// This is usually an offset value starting from the upper left corner of the object
        /// about to be dragged to the drag starting point.
        /// </remarks>
        public Point2 DragOffset
        {
            get;
            protected set;
        }

        /// <summary>
        /// Handles the mouse input of the user.
        /// </summary>
        /// <param name="mouseState">
        /// The state of the <see cref="Mouse"/>.
        /// </param>
        /// <param name="oldMouseState">
        /// The state of the <see cref="Mouse"/> one frame ago.
        /// </param>
        public void HandleMouseInput( ref MouseState mouseState, ref MouseState oldMouseState )
        {
            if( this.IsDragging )
            {
                if( this.IsMouseReleased( ref mouseState ) )
                {
                    this.Release();
                }
                else
                {
                    this.Capture( ref mouseState, ref oldMouseState );
                }
            }
            else
            {
                if( this.IsMousePressed( ref mouseState ) )
                {
                    if( this.ShouldStartCapturing( new Point2( mouseState.X, mouseState.Y ) ) )
                    {
                        this.StartCapture();
                    }
                }
            }
        }

        /// <summary>
        /// Captures the given input data.
        /// </summary>
        /// <param name="mouseState">
        /// The state of the <see cref="Mouse"/>.
        /// </param>
        /// <param name="oldMouseState">
        /// The state of the <see cref="Mouse"/> one frame ago.
        /// </param>
        private void Capture( ref MouseState mouseState, ref MouseState oldMouseState )
        {
            this.Position = new Point2( mouseState.X, mouseState.Y );
            this.DeltaMovement = new Point2( oldMouseState.X - mouseState.X, oldMouseState.Y - mouseState.Y );
        }
        
        /// <summary>
        /// Releases the capturing of movement information;
        /// manually stopping the dragging operation.
        /// </summary>
        public void Release()
        {
            this.DeltaMovement = Point2.Zero;
            this.DragOffset = Point2.Zero;
            this.IsDragging = false;
        }

        /// <summary>
        /// Starts capturing a dragging operation.
        /// </summary>
        private void StartCapture()
        {
            this.IsDragging = true;
        }

        /// <summary>
        /// Gets a value indicating whether the user is currently
        /// pressing the mouse button that starts a dragging operation.
        /// </summary>
        /// <param name="mouseState">
        /// The current state of the mouse.
        /// </param>
        /// <returns>
        /// true if the mouse is pressed;
        /// otherwise false.
        /// </returns>
        private bool IsMousePressed( ref MouseState mouseState )
        {
            return !this.IsMouseReleased( ref mouseState );
        }

        /// <summary>
        /// Gets a value indicating whether the user is currently
        /// pressing the mouse button that ends a dragging operation.
        /// </summary>
        /// <param name="mouseState">
        /// The current state of the mouse.
        /// </param>
        /// <returns>
        /// true if the mouse is not pressed;
        /// otherwise false.
        /// </returns>
        protected virtual bool IsMouseReleased( ref MouseState mouseState )
        {
            return mouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Gets a value indicating whether capturing of input should start.
        /// </summary>
        /// <param name="mousePosition">
        /// The position of the mouse when the user began dragging.
        /// </param>
        /// <returns>
        /// true if this DragDropCapturer should starting capturing;
        /// otherwise false.
        /// </returns>
        protected virtual bool ShouldStartCapturing( Point2 mousePosition )
        {
            return true;
        }
    }
}
