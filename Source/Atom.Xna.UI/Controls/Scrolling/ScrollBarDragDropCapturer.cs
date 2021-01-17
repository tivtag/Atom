// <copyright file="ScrollBarDragDropCapturer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Controls.ScrollBarDragDropCapturer class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI.Controls
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;

    /// <summary>
    /// Represents the <see cref="DragDropCapturer"/> that is used internally by a <see cref="ScrollBar"/>. 
    /// </summary>
    public class ScrollBarDragDropCapturer : DragDropCapturer
    {
        /// <summary>
        /// Initializes a new instance of the ScrollBarDragDropCapturer class.
        /// </summary>
        /// <param name="scrollBar">
        /// The ScrollBar that uses the new ScrollBarDragDropCapturer to capture drag-drop events.
        /// </param>
        public ScrollBarDragDropCapturer( ScrollBar scrollBar )
        {
            Contract.Requires<ArgumentNullException>( scrollBar != null );

            this.scrollBar = scrollBar;
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
        protected override bool ShouldStartCapturing( Point2 mousePosition )
        {
            if( this.scrollBar.ThumbArea.Contains( mousePosition ) )
            {
                this.DragOffset = mousePosition - this.scrollBar.ThumbArea.Position;
                return true;
            }

            return false;
        }

        /// <summary>
        /// The ScrollBar that uses this ScrollBarDragDropCapturer to capture drag-drop events.
        /// </summary>
        private readonly ScrollBar scrollBar;
    }
}
