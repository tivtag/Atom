// <copyright file="QuadTreeNode2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.QuadTreeNode2 class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Scene
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;

    /// <summary>
    /// Represents a node in a <see cref="QuadTree2"/>.
    /// This class can't be inherited.
    /// </summary>
    public sealed class QuadTreeNode2
    {
        #region [ Properties ]
        
        /// <summary>
        /// Gets the <see cref="Rectangle"/> descriping the area this <see cref="QuadTreeNode2"/> covers.
        /// </summary>
        public RectangleF Area
        {
            get
            {
                return this.area;
            }
        }

        /// <summary>
        /// Gets the <see cref="Rectangle"/> descriping the 'loose' area this <see cref="QuadTreeNode2"/> covers.
        /// </summary>
        public RectangleF LooseArea
        {
            get
            {
                return this.looseArea;
            }
        }

        /// <summary>
        /// Gets the <see cref="QuadTree2"/> that owns the <see cref="QuadTreeNode2"/>.
        /// </summary>
        /// <value>
        /// The <see cref="QuadTree2"/> that owns this QuadTreeNode2.
        /// </value>
        public QuadTree2 Tree
        {
            get 
            {
                return this.tree; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="QuadTreeNode2"/> is a leaf.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if this QuadTreeNode2 is a leaf and as such may contain items;
        /// or otherwise <see langword="false"/> if this QuadTreeNode2 is not an end-leaf and as such contains sub leaves.
        /// </value>
        public bool IsLeaf
        {
            get
            {
                return this.items != null; 
            }
        }

        /// <summary>
        /// Gets the corner relative to its parent of the <see cref="QuadTreeNode2"/>.
        /// </summary>
        /// <value>
        /// A <see cref="QuadCorner"/> value that represents the position of
        /// this QuadTreeNode2 relative to its parent QuadTreeNode2.
        /// </value>
        public QuadCorner Corner
        {
            get 
            {
                return this.corner;
            }
        }

        /// <summary>
        /// Gets a string that contains the corners reversed back 
        /// from the <see cref="QuadTreeNode2"/> to the <see cref="QuadTree2"/>. 
        /// </summary>
        /// <value>
        /// The string is of the format "UpperLeft.UpperRight.BottumLeft".
        /// </value>
        public string Corners
        {
            get
            {
                string parentCorners = this.parent == null ? string.Empty : this.parent.Corners + '.';
                return parentCorners + this.corner.ToString();
            }
        }

        /// <summary>
        /// Gets the <see cref="QuadTreeNode2"/> in the upper-left corner.
        /// </summary>
        public QuadTreeNode2 UpperLeft
        {
            get
            {
                return this.upperLeft;
            }
        }

        /// <summary>
        /// Gets the <see cref="QuadTreeNode2"/> in the upper-right corner.
        /// </summary>
        public QuadTreeNode2 UpperRight
        {
            get
            {
                return this.upperRight;
            }
        }

        /// <summary>
        /// Gets the <see cref="QuadTreeNode2"/> in the bottom-left corner.
        /// </summary>
        public QuadTreeNode2 BottomLeft
        {
            get
            {
                return this.bottomLeft;
            }
        }

        /// <summary>
        /// Gets the <see cref="QuadTreeNode2"/> in the bottom-right corner.
        /// </summary>
        public QuadTreeNode2 BottomRight
        {
            get
            {
                return this.bottomRight;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadTreeNode2"/> class.
        /// </summary>
        /// <param name="tree">
        /// The tree that owns the <see cref="QuadTreeNode2"/>.
        /// </param>
        internal QuadTreeNode2( QuadTree2 tree )
        {
            this.tree = tree;
        }

        /// <summary> 
        /// Setups this <see cref="QuadTreeNode2"/> and also sub-divides it if needed.
        /// </summary>
        /// <param name="x">
        /// The position on the x-axis of the leaf.
        /// </param>
        /// <param name="y">
        /// The position on the y-axis of the leaf.
        /// </param>
        /// <param name="width"> 
        /// The width of the leaf.
        /// </param>
        /// <param name="height">
        /// The height of the leaf.
        /// </param>
        /// <param name="subdivisionsLeft">
        /// The number of sub divisions left.
        /// </param>
        /// <param name="corner">
        /// The corner this leaf is.
        /// </param>
        /// <param name="parent">
        /// The parent leaf of the <see cref="QuadTreeNode2"/>.
        /// </param>
        internal void Create(
            float x,
            float y,
            float width,
            float height,
            int subdivisionsLeft,
            QuadCorner corner,
            QuadTreeNode2 parent )
        {
            this.corner = corner;
            this.parent = parent;

            this.area = new RectangleF( x, y, width, height );
            this.looseArea = new RectangleF(
                x - tree.ExtraLooseWidth,
                y - tree.ExtraLooseHeight,
                width  + (2.0f * tree.ExtraLooseWidth),
                height + (2.0f * tree.ExtraLooseHeight)
            );

            // Is it an End QuadTreeLeaf?
            if( subdivisionsLeft <= 0 )
            {
                this.items = new LinkedList<IQuadTreeItem2>();
                return;
            }
            else
            {
                this.items = null;
            }

            --subdivisionsLeft;

            float widthHalf  = width  / 2.0f;
            float heightHalf = height / 2.0f;
            float xwh        = x + widthHalf;
            float yhh        = y + heightHalf;

            this.upperLeft = new QuadTreeNode2( tree );
            this.upperLeft.Create( x, y, widthHalf, heightHalf, subdivisionsLeft, QuadCorner.UpperLeft, this );

            this.upperRight = new QuadTreeNode2( tree );
            this.upperRight.Create( xwh, y, widthHalf, heightHalf, subdivisionsLeft, QuadCorner.UpperRight, this );

            this.bottomLeft = new QuadTreeNode2( tree );
            this.bottomLeft.Create( x, yhh, widthHalf, heightHalf, subdivisionsLeft, QuadCorner.BottomLeft, this );

            this.bottomRight = new QuadTreeNode2( tree );
            this.bottomRight.Create( xwh, yhh, widthHalf, heightHalf, subdivisionsLeft, QuadCorner.BottomRight, this );
        }

        #endregion

        #region [ Methods ]

        #region Insert

        /// <summary>
        /// Tries to insert the specified <see cref="IQuadTreeItem2"/>
        /// into this QuadTreeNode2.
        /// </summary>
        /// <param name="item"> 
        /// The <see cref="IQuadTreeItem2"/> to add.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified <see cref="IQuadTreeItem2"/> has been inserted;
        /// otherwise <see langword="false"/>. 
        /// </returns>
        public bool Insert( IQuadTreeItem2 item )
        {
            Contract.Requires<ArgumentNullException>( item != null );

            if( area.Contains( item.QuadTreeRectangle.Center ) == false )
                return false;

            // If the items list is null then this leaf has sub leaves
            // and as such doesn't contain any items.
            if( items == null )
            {
                if( this.upperLeft.Insert( item ) )
                    return true;
                if( this.upperRight.Insert( item ) )
                    return true;
                if( this.bottomLeft.Insert( item ) )
                    return true;
                if( this.bottomRight.Insert( item ) )
                    return true;

                return false;
            }
            else
            {
                var node = this.items.AddLast( item );
                item.QuadTreeLeaf     = this;
                item.QuadTreeListNode = node;

                return true;
            }
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes all items from the <see cref="QuadTreeNode2"/>.
        /// </summary>
        public void RemoveAll()
        {
            if( items != null )
            {
                foreach( IQuadTreeItem2 item in items )
                {
                    item.QuadTreeLeaf     = null;
                    item.QuadTreeListNode = null;
                }

                items.Clear();
                return;
            }

            upperLeft.RemoveAll();
            upperRight.RemoveAll();
            bottomLeft.RemoveAll();
            bottomRight.RemoveAll();
        }

        #endregion

        #region CheckItem

        /// <summary>
        /// Tests whether this item (if it's part of this leaf) is still inside the bounderies.
        /// Reinserts the item if needed.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="item"/> is null.
        /// </exception>
        /// <param name="item"> The item to check. </param>
        public void CheckItem( IQuadTreeItem2 item )
        {
            Contract.Requires<ArgumentException>( item.QuadTreeLeaf == this );

            if( !this.looseArea.Contains( item.QuadTreeRectangle ) )
            {
                var node = item.QuadTreeListNode;
                if( node == null )
                {
                    RemoveFromScene( item );
                    return;
                }

                try
                {
                    this.items.Remove( node );
                    this.tree.Reinsert( item );
                }
                catch( InvalidOperationException )
                {
                    RemoveFromScene( item );
                }
            }
        }

        /// <summary>
        /// Removes the owner of the given IQuadTree2Item from its current IScene.
        /// </summary>
        /// <param name="item">
        /// The item to remove.
        /// </param>
        private static void RemoveFromScene( IQuadTreeItem2 item )
        {
            var entity = item.Owner as ISceneEntity;

            if( entity != null )
            {
                entity.RemoveFromScene();
            }
        }

        #endregion

        #region > Visability Tests <

        /// <summary>
        /// Finds all visible items in the specified <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="items">
        /// This method inserts all found items in this list.
        /// </param>
        /// <param name="area">
        /// The rectangle descriping the 'view window'. All items within the rectangle are inserted.
        /// </param>
        internal void FindVisible( List<IQuadTreeItem2> items, ref Rectangle area )
        {
            bool isInside;
            this.looseArea.Intersects( ref area, out isInside );

            if( !isInside )
                return;

            // Is the leaf an "End QuadTreeLeaf"?
            if( this.items != null )
            {
                items.Capacity += this.items.Count;

                foreach( var item in this.items )
                {
                    if( item.IsVisibleToQuadTree )
                    {
                        items.Add( item );
                    }
                }

                return;
            }

            this.upperLeft.FindVisible( items, ref area );
            this.upperRight.FindVisible( items, ref area );
            this.bottomLeft.FindVisible( items, ref area );
            this.bottomRight.FindVisible( items, ref area );
        }

        #endregion

        #region > Containment Tests <

        /// <summary>
        /// Gets whether this <see cref="QuadTreeNode2"/> contains an <see cref="IQuadTreeItem2"/>
        /// in the given <paramref name="area"/> while the given additional <paramref name="predicate"/> holds true.
        /// </summary>
        /// <param name="area">
        /// The area to look for items in.
        /// </param>
        /// <param name="predicate">
        /// The additional predicate that must hold true.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if this QuadTree2 contains an IQuadTree2Item in the given <paramref name="area"/>
        /// while the given <paramref name="predicate"/> still holds true;
        /// otherwise <see langword="false"/>.
        /// </returns>
        internal bool ContainsItem( ref Rectangle area, Predicate<IQuadTreeItem2> predicate )
        {
            bool isInside;
            this.looseArea.Intersects( ref area, out isInside );

            if( !isInside )
                return false;

            // Is the leaf an "End QuadTreeLeaf"?
            if( this.items != null )
            {
                foreach( var item in this.items )
                {
                    if( item.IsVisibleToQuadTree )
                    {
                        if( area.Intersects( item.QuadTreeRectangle ) )
                        {
                            if( predicate( item ) )
                                return true;
                        }
                    }
                }

                return false;
            }

            if( this.upperLeft.ContainsItem( ref area, predicate ) )
                return true;
            if( this.upperRight.ContainsItem( ref area, predicate ) )
                return true;
            if( this.bottomLeft.ContainsItem( ref area, predicate ) )
                return true;
            if( this.bottomRight.ContainsItem( ref area, predicate ) )
                return true;

            return false;
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The <see cref="Rectangle"/> descriping the area this <see cref="QuadTreeNode2"/> covers.
        /// </summary>
        private RectangleF area;

        /// <summary>
        /// The <see cref="Rectangle"/> descriping the 'loose' area this <see cref="QuadTreeNode2"/> covers.
        /// </summary>
        private RectangleF looseArea;

        /// <summary>
        /// The corner relative to its parent of this leaf.
        /// </summary>
        private QuadCorner corner;

        /// <summary> 
        /// The leaf that owns the <see cref="QuadTreeNode2"/>.
        /// Null if it's the first leaf in the hirarchy. 
        /// </summary>
        private QuadTreeNode2 parent;

        /// <summary> 
        /// The sub-leaves of the <see cref="QuadTreeNode2"/>.
        /// Null if it's the last leaf in the hirarchy; aka an end leaf.
        /// </summary>
        private QuadTreeNode2 upperLeft, upperRight, bottomLeft, bottomRight;

        /// <summary>
        /// The <see cref="QuadTree2"/> that owns the <see cref="QuadTreeNode2"/>.
        /// </summary>
        private QuadTree2 tree;

        /// <summary> 
        /// The items that are stored in this <see cref="QuadTreeNode2"/>.
        /// Only end leaves can contain items -> null if no end leaf.
        /// </summary>
        private LinkedList<IQuadTreeItem2> items;

        #endregion
    }
}
