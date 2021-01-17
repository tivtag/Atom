// <copyright file="ListBox.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Controls.ListBox{T} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;

    /// <summary>
    /// Represents a scroll-able list of ListItem{T}s.
    /// </summary>
    /// <remarks>
    /// This default base implementation doesn't contain any
    /// drawing logic.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the value stored in an ListItem{T}s of the ListBox{T}.
    /// </typeparam>
    public abstract class ListBox<T> : UIElement
    {
        /// <summary>
        /// Raised when the currently <see cref="SelectedItem"/> has changed.
        /// </summary>
        public event EventHandler SelectedItemChanged;

        /// <summary>
        /// Gets the number of items this ListBox{T, TListItem} contains.
        /// </summary>
        public int ItemCount
        {
            get
            {
                return this.items.Count;
            }
        }

        /// <summary>
        /// Gets the <see cref="ListItem{T}"/>s this ListBox{T, TListItme} contains.
        /// </summary>
        public IEnumerable<ListItem<T>> Items
        {
            get
            {
                return this.items;
            }
        }

        /// <summary>
        /// Gets the ListItem{T} that is currently selected in this ListBox{T}.
        /// </summary>
        public ListItem<T> SelectedItem
        {
            get
            {
                return this.selectedItem;
            }

            private set
            {
                if( this.selectedItem == value )
                    return;

                this.selectedItem = value;
                this.SelectedItemChanged.Raise( this );
            }
        }

        /// <summary>
        /// Gets the value of the currently <see cref="SelectedItem"/>.
        /// </summary>
        public T SelectedValue
        {
            get
            {
                if( this.selectedItem != null )
                {
                    return this.selectedItem.Value;
                }
                else
                {
                    return default( T );
                }                    
            }
        }

        /// <summary>
        /// Gets or sets a value indicating into which direction this ListBox grows.
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return this.orientation;
            }

            set
            {
                this.orientation = value;
                this.scrollBar.Orientation =  value;
            }
        }

        /// <summary>
        /// Gets the total area the ListItem{T}s of this ListBox{T} take up.
        /// </summary>
        protected RectangleF TotalItemArea
        {
            get
            {
                return this.totalItemArea;
            }
        }

        /// <summary>
        /// Gets the total number of pixels the items take up,
        /// related to the orientation of this ListBox.
        /// </summary>
        protected float TotalItemSize
        {
            get
            {
                switch( this.orientation )
                {
                    case Atom.Math.Orientation.Vertical:
                        return this.totalItemArea.Height;

                    case Atom.Math.Orientation.Horizontal:
                        return this.totalItemArea.Width;

                    default:
                        return 0.0f;
                }
            }
        }
        
        /// <summary>
        /// Gets the total number of pixels the items take up,
        /// related to the orientation of this ListBox.
        /// </summary>
        protected float AvailableItemSize
        {
            get
            {
                switch( this.orientation )
                {
                    case Atom.Math.Orientation.Vertical:
                        return this.Height;

                    case Atom.Math.Orientation.Horizontal:
                        return this.Width;

                    default:
                        return 0.0f;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="ScrollBar"/> that allows scrolling the content of this ListBox.
        /// </summary>
        protected ScrollBar ScrollBar
        {
            get
            {
                return this.scrollBar;
            }
        }

        /// <summary>
        /// Gets the number of pixels this ListBox is scrolled by.
        /// </summary>
        private float ScrollValue
        {
            get
            {
                return this.scrollBar.ScrollValue;
            }
        }

        /// <summary>
        /// Gets the sub-area this ListBox{T} currently shows;
        /// based on the current scrolling value.
        /// </summary>
        private RectangleF ScrolledArea
        { 
            get
            {
                switch( this.orientation )
                {
                    case Atom.Math.Orientation.Vertical:
                        return new RectangleF(
                            this.X,
                            this.Y + this.ScrollValue,
                            this.Size.X,
                            this.Size.Y
                        );

                    default:
                        throw new NotImplementedException();
                }
            } 
        }

        /// <summary>
        /// Initializes a new instance of the ListBox{T} class.
        /// </summary>
        /// <param name="scrollBar">
        /// Represents the object that is responsible for managing the scrolling in this ListBox.
        /// </param>
        protected ListBox( ScrollBar scrollBar )
        {
            Contract.Requires<ArgumentNullException>( scrollBar != null );

            this.scrollBar = scrollBar;
            this.scrollBar.ThumbPositionChanged += this.OnThumbPositionChanged;
        }

        /// <summary>
        /// Creates a new instance of the ListItem{T} class that contains
        /// the specified value.
        /// </summary>
        /// <param name="value">
        /// The value te new ListItem{T} should contain.
        /// </param>
        /// <returns>
        /// The newly created ListItem{T}.
        /// </returns>
        protected abstract ListItem<T> CreateItem( T value );

        /// <summary>
        /// Adds a new ListItem{T} to this ListBox{T} that contains
        /// the specified value.
        /// </summary>
        /// <param name="value">
        /// The value to add.
        /// </param>
        /// <returns>
        /// The newly added ListItem{T}.
        /// </returns>
        public ListItem<T> AddItem( T value )
        {
            ListItem<T> item = this.CreateItem( value );

            this.items.Add( item );

            return item;
        }

        /// <summary>
        /// Removes all items from this ListBox{T}.
        /// </summary>
        public virtual void ClearItems()
        {
            this.items.Clear();
            this.Refresh();
            this.SelectedItem = null;
        }

        /// <summary>
        /// Called when the user has changed the position of the scroll thumb.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The EventArgs that contain the event data.
        /// </param>
        private void OnThumbPositionChanged( object sender, EventArgs e )
        {
            this.RefreshScroll();
        }

        /// <summary>
        /// Scrolls this ListBox up/left or down/right by the specified
        /// percentage.
        /// </summary>
        /// <param name="percentage">
        /// The percentrage to move. E.g. "-20.0f" to move the thumb up by 20%.
        /// </param>
        public void ScrollBy( float percentage )
        {
            this.scrollBar.ScrollBy( percentage );
        }

        /// <summary>
        /// Fully refreshes the internal layout of this ListBox{T}.
        /// </summary>
        public virtual void Refresh()
        {
            this.LayoutItems();
            this.RefreshTotalItemArea();
            this.RefreshScroll();
        }

        /// <summary>
        /// Refreshes the scrolling related properties of this ListBox{T}.
        /// </summary>
        private void RefreshScroll()
        {
            float totalSize = this.TotalItemSize;

            this.scrollBar.VisibleValue = this.AvailableItemSize;
            this.scrollBar.TotalValue = this.TotalItemSize - this.scrollBar.VisibleValue;
            this.scrollBar.Refresh();

            if( this.scrollBar.VisibleValue >= this.TotalItemSize )
            {
                this.ShowAllItems();
                this.scrollBar.HideAndDisable();
            }
            else
            {
                this.ShowItemsIn( this.ScrolledArea );
                this.scrollBar.ShowAndEnable();
            }

            this.RefreshScrolledItemPosition();
        }

        /// <summary>
        /// Refreshes the actual position of the ListItem{T}s this ListBox{T} owns.
        /// </summary>
        private void RefreshScrolledItemPosition()
        {
            if( this.orientation == Atom.Math.Orientation.Vertical )
            {
                foreach( var item in this.items )
                {
                    item.Position = item.UnscrolledPosition - new Vector2( 0.0f, this.ScrollValue );
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Shows and enables all of the ListItem{T}s of this ListBox{T}.
        /// </summary>
        private void ShowAllItems()
        {
            foreach( var item in this.items )
            {
                item.ShowAndEnable();
            }
        }

        /// <summary>
        /// Shows and enables all of the ListItem{T}s that intersect the given area of this ListBox{T},
        /// while hiding and disabling those that don't.
        /// </summary>
        /// <param name="area">
        /// The current area of this.
        /// </param>
        private void ShowItemsIn( RectangleF area )
        {
            foreach( var item in this.items )
            {
                if( area.Intersects( item.UnscrolledClientArea ) )
                {
                    item.ShowAndEnable();
                }
                else
                {
                    item.HideAndDisable();
                }
            }
        }
             
        /// <summary>
        /// Refreshes the <see cref="totalItemArea"/> field.
        /// </summary>
        private void RefreshTotalItemArea()
        {
            this.totalItemArea.Position = this.Position;
            this.totalItemArea.Size = Vector2.Zero;

            switch( this.orientation )
            {
                case Orientation.Vertical:
                    foreach( var item in this.items )
                    {
                        this.totalItemArea.Height += item.Size.Y;
                    }

                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Positions all of the items within this ListBox.
        /// </summary>
        private void LayoutItems()
        {
            switch( this.orientation )
            {
                case Orientation.Vertical:
                    this.LayoutItemsVertically();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Positions all of the items within this ListBox;
        /// stacking them ontop of eachother downwards.
        /// </summary>
        private void LayoutItemsVertically()
        {
            Vector2 position = this.Position;

            for( int index = 0; index < this.items.Count; ++index )
            {
                ListItem<T> item = this.items[index];

                item.Layout( index, position );
                position.Y += item.Size.Y;
            }
        }
        
        /// <summary>
        /// Called when this ListBox{T} has been shown or hidden.
        /// </summary>
        protected override void OnIsVisibleChanged()
        {
            if( !this.IsVisible )
            {
                foreach( var item in this.items )
                {
                    item.IsVisible = false;
                }

                this.scrollBar.IsVisible = false;
            }
        }

        /// <summary>
        /// Called when this ListBox{T} has been enabled or disabled.
        /// </summary>
        protected override void OnIsEnabledChanged()
        {
            if( !this.IsEnabled )
            {
                foreach( var item in this.items )
                {
                    item.IsEnabled = false;
                }

                this.scrollBar.IsEnabled = false;
            }
        }

        /// <summary>
        /// Called when this ListBox was added to the specified UserInterface.
        /// </summary>
        /// <param name="userInterface">
        /// The related UserInterface.
        /// </param>
        protected override void OnAdded( UserInterface userInterface )
        {
            userInterface.AddElement( this.scrollBar );
        }

        /// <summary>
        /// Called when this ListBox was removed from the specified UserInterface.
        /// </summary>
        /// <param name="userInterface">
        /// The related UserInterface.
        /// </param>
        protected override void OnRemoved( UserInterface userInterface )
        {
            userInterface.RemoveElement( this.scrollBar );
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
            this.scrollBar.HandleMouseWheelInput( mouseState.ScrollWheelValue, oldMouseState.ScrollWheelValue );

            foreach( var item in this.items )
            {
                if( item.IsVisible && item.ClientArea.Contains( mouseState.X, mouseState.Y ) )
                {
                    return item.HandleRelatedMouseInputCore( ref mouseState, ref oldMouseState );
                }
            }

            return base.HandleRelatedMouseInput( ref mouseState, ref oldMouseState );
        }

        /// <summary>
        /// Selects the specified ListItem{T}.
        /// </summary>
        /// <param name="item">
        /// The ListItem{T} to select. Can be null.
        /// </param>
        public void Select( ListItem<T> item )
        {
            foreach( var i in this.items )
            {
                i.IsSelected = false;                
            }

            if( item != null )
            {
                item.IsSelected = true;
            }

            this.SelectedItem = item;
        }
        
        /// <summary>
        /// Stores the total area the ListItem{T}s of this ListBox{T} take up.
        /// </summary>
        private RectangleF totalItemArea;

        /// <summary>
        /// Represents the object that is responsible for managing the scrolling in this ListBox.
        /// </summary>
        private readonly ScrollBar scrollBar;

        /// <summary>
        /// Represents the storage field of the Orientation property.
        /// </summary>
        private Orientation orientation;

        /// <summary>
        /// Represents the storage field of the <see cref="SelectedItem"/> property.
        /// </summary>
        private ListItem<T> selectedItem;
    
        /// <summary>
        /// Stores the <see cref="ListItem{T}"/> this ListBox{T} contains.
        /// </summary>
        private readonly List<ListItem<T>> items = new List<ListItem<T>>();
    }
}
