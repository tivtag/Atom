// <copyright file="ScrollBar.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Controls.ScrollBar class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI.Controls
{
    using System;
    using Atom.Math;

    /// <summary>
    /// Represents a scroll bar.
    /// </summary>
    public abstract class ScrollBar : UIElement
    {
        /// <summary>
        /// Raised when the position of the scroll thumb has changed.
        /// </summary>
        public event EventHandler ThumbPositionChanged;

        /// <summary>
        /// Gets or sets the relative position of the thumb within this ScrollBar.
        /// </summary>
        /// <value>
        /// A value between 0 and 1; where 0 = top/left and 1 = bottom/right.
        /// </value>
        public float RelativeThumbPosition
        {
            get
            {
                return this.relativeThumbPosition;
            }

            set
            {
                if( value == this.relativeThumbPosition )
                    return;

                this.relativeThumbPosition = MathUtilities.Clamp( value, 0.0f, 1.0f );
                this.ThumbPositionChanged.Raise( this );
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the total value that can be maxmally be reached
        /// by setting the RelativeThumbPosition to 1.
        /// </summary>
        public float TotalValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the amount of the value currently visible on the screen;
        /// this determines the size of the thumb.
        /// </summary>
        public float VisibleValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value that represents the amount the VisibleValue has been moved.
        /// </summary>
        public float ScrollValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating how this ScrollBar is oriented.
        /// </summary>
        public Orientation Orientation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the half size of scroll thumb.
        /// </summary>
        protected int HalfThumbSize
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the area the actual slider takes up.
        /// </summary>
        public Rectangle SliderArea
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the area the thumb take sup.
        /// </summary>
        public Rectangle ThumbArea
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the ScrollBar class.
        /// </summary>
        public ScrollBar()
        {
            this.Orientation = Orientation.Vertical;
            this.PassInputToSubElements = false;

            this.dragDropCapturer = new ScrollBarDragDropCapturer( this );
        }

        /// <summary>
        /// Refreshes the various sub areas of this ScrollBar.
        /// </summary>
        public virtual void Refresh()
        {
            float total = this.TotalValue;

            if( this.VisibleValue > this.TotalValue )
            {
                total += this.VisibleValue;
            }

            float thumbSizeFactor = this.VisibleValue / total;

            Point2 thumpSize;

            if( this.Orientation == Orientation.Vertical )
            {
                thumpSize.X = (int)this.Width;
                thumpSize.Y = (int)(thumbSizeFactor * this.Height);

                this.HalfThumbSize = thumpSize.Y / 2;
                this.SliderArea = new Rectangle(
                    new Point2( (int)this.X, (int)this.Y + this.HalfThumbSize ),
                    new Point2( (int)this.Width, (int)this.Height - thumpSize.Y )
                );

                this.ThumbArea = new Rectangle(
                    new Point2( this.SliderArea.X, (int)(this.SliderArea.Y + (this.relativeThumbPosition * this.SliderArea.Height) - this.HalfThumbSize) ),
                    thumpSize
                );
            }
            else
            {
                throw new NotImplementedException();
            }

            this.ScrollValue = this.RelativeThumbPosition * this.TotalValue;
        }

        /// <summary>
        /// Called when this ScrollBar is updating itself.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        protected override void OnUpdate( Atom.IUpdateContext updateContext )
        {
            if( this.dragDropCapturer.IsDragging )
            {
                this.HandleDrag();
            }
        }

        /// <summary>
        /// Handles a dragging operation of the user.
        /// </summary>
        private void HandleDrag()
        {
            Point2 position = this.dragDropCapturer.Position;

            if( this.Orientation == Orientation.Vertical )
            {
                if( this.dragDropCapturer.DeltaMovement.Y != 0 )
                {
                    this.MoveScrollTo( position, this.dragDropCapturer.DragOffset );
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Moves the position of the thumb to the given position.
        /// </summary>
        /// <param name="position">
        /// The position to move the thumb to.
        /// </param>
        /// <param name="offset">
        /// The offset to apply.
        /// </param>
        public void MoveScrollTo( Point2 position, Point2 offset )
        {
            if( this.Orientation == Orientation.Vertical )
            {
                int relativePosition = position.Y - this.SliderArea.Y;
                int centeredPosition = relativePosition + this.HalfThumbSize - offset.Y;

                this.RelativeThumbPosition = centeredPosition / (float)this.SliderArea.Height;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Scrolls this ScrollBar up/left or down/right by the specified
        /// percentage.
        /// </summary>
        /// <param name="percentage">
        /// The percentrage to move. E.g. "-20.0f" to move the thumb up by 20%.
        /// </param>
        public void ScrollBy( float percentage )
        {
            this.RelativeThumbPosition += (percentage / 100.0f);
        }

        /// <summary>
        /// Scrolls this ScrollBar up/left or down/right by the specified
        /// value.
        /// </summary>
        /// <param name="value">
        /// The amount to move.
        /// </param>
        public void ScrollByValue( int value )
        {
            float power = value / 120.0f;

            // Reduce power when turning the wheel very fast
            if( Math.Abs( power ) > 1.0f )
            {
                power *= 0.75f;
            }
            
            float percentage = 5.0f * power;
            
            // Increase scrolled percentage when not much would scroll at all
            float scrolledValue = Math.Abs( TotalValue * percentage );
            if( scrolledValue < 500 )
            {
                percentage *= 1.8f;
            }

            this.ScrollBy( percentage );
        }

        /// <summary>
        /// Handles mouse wheel input of the user.
        /// </summary>
        /// <param name="scrollWheelValue">
        /// The current value of the scroll wheel.
        /// </param>
        /// <param name="oldScrollWheelValue">
        /// The value of the scroll wheel one frame ago.
        /// </param>
        public void HandleMouseWheelInput( int scrollWheelValue, int oldScrollWheelValue )
        {
            if( !this.IsEnabled )
                return;

            int delta = oldScrollWheelValue - scrollWheelValue;

            if( delta != 0 )
            {
                this.ScrollByValue( delta );
            }
        }

        /// <summary>
        /// Handles the mouse input of the user.
        /// </summary>
        /// <param name="mouseState">
        /// The state of the <see cref="Microsoft.Xna.Framework.Input.Mouse"/>.
        /// </param>
        /// <param name="oldMouseState">
        /// The state of the <see cref="Microsoft.Xna.Framework.Input.Mouse"/> one frame ago.
        /// </param>
        protected override void HandleMouseInput(
            ref Microsoft.Xna.Framework.Input.MouseState mouseState,
            ref Microsoft.Xna.Framework.Input.MouseState oldMouseState )
        {
            if( !this.IsEnabled )
                return;

            this.dragDropCapturer.HandleMouseInput( ref mouseState, ref oldMouseState );
            if( this.dragDropCapturer.IsDragging )
                return;

            if( mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released )
            {
                if( this.ClientArea.Contains( mouseState.X, mouseState.Y ) )
                {
                    this.MoveScrollTo( new Point2( mouseState.X, mouseState.Y ), new Point2( this.HalfThumbSize, this.HalfThumbSize ) );
                }
            }

            this.HandleMouseWheelInput( mouseState.ScrollWheelValue, oldMouseState.ScrollWheelValue );
        }

        /// <summary>
        /// Called when the IsEnabled state of this ScrollBar has changed.
        /// </summary>
        protected override void OnIsEnabledChanged()
        {
            if( !this.IsEnabled )
            {
                this.dragDropCapturer.Release();
            }
        }

        /// <summary>
        /// Represents the storage field of the <see cref="RelativeThumbPosition"/> property.
        /// </summary>
        private float relativeThumbPosition;

        /// <summary>
        /// Implements a simple mechanism that captures dragging operations
        /// of the user.
        /// </summary>
        private readonly DragDropCapturer dragDropCapturer;
    }
}
