// <copyright file="QuadTree2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.QuadTree2 class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Atom.Math;

    /// <summary>
    /// This class reprents a 'loose' 2-D Quad Tree which supports moving objects.
    /// </summary>
    /// <remarks>
    /// A Quad Tree is one methods to divide a plane (the 2dim space) into sub-parts:
    /// The tree iteself is the root node.
    /// This node is divided into four quads of same size - the leaves of the first division.
    /// Those leaves can also again each be devided into sub-leaves; 
    /// this happens until the set division level is reached.
    /// Objects are stored in 'End Leaves'. Those are the leaves which aren't subdivided.
    /// The Leaf-Nodes overlap eachother by a predefinded value, this makes the tree 'loose'
    /// <code>
    /// A (non loose) QuadTree which has been sub-divided twice:
    /// +------*------+------*------+
    /// |  AA  |  AB  |      |      |
    /// |      |      |      |      |
    /// *------A------*------B------*
    /// |  AC  |  AD  |      |      |
    /// |      |      |      |      |
    /// |------*------+------*------+
    /// |      |      |      |      |
    /// |      |      |      |      |
    /// *------C------*------D------*
    /// |      |      |      |      |
    /// |      |      |      |      |
    /// +------*------+------*------+
    /// </code>
    /// </remarks>
    public class QuadTree2 : IEnumerable<IQuadTreeItem2>
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the number of items contained in the <see cref="QuadTree2"/>.
        /// </summary>
        /// <value>The number of items contained in the <see cref="QuadTree2"/>.</value>
        public int ItemCount
        {
            get 
            {
                return this.items.Count;
            }
        }

        /// <summary>
        /// Gets the number of sub-divisions the QuadTree2 has.
        /// </summary>
        /// <value>
        /// The number of sub-divisions. Is applied on when the tree is created using <see cref="Create"/>.
        /// </value>
        public int SubdivisionCount
        {
            get
            {
                return this.subdivisionCount;
            }
        }

        /// <summary>
        /// Gets the width of the <see cref="QuadTree2"/>.
        /// </summary>
        /// <value>
        /// The original width of the (non-loose) QuadTree2.
        /// </value>
        public float Width
        {
            get
            {
                return this.width;
            }
        }

        /// <summary>
        /// Gets the height of the <see cref="QuadTree2"/>.
        /// </summary>
        /// <value>
        /// The original height of the (non-loose) QuadTree2.
        /// </value>
        public float Height
        {
            get
            {
                return this.height;
            }
        }

        /// <summary>
        /// Gets the range that is added to the width of a leaf
        /// to make the <see cref="QuadTree2"/> more loose.
        /// </summary>
        /// <value>
        /// The width of the area that is added 
        /// on the left and right to make the QuadTree2 more loose.
        /// </value>
        public float ExtraLooseWidth
        {
            get
            {
                return this.extraLooseWidth;
            }
        }

        /// <summary>
        /// Gets the range that is added to the height of a leaf
        /// to make the <see cref="QuadTree2"/> more loose.
        /// </summary>
        /// <value>
        /// The height of the area that is added 
        /// on the top and bottom to make the QuadTree2 more loose.
        /// </value>
        public float ExtraLooseHeight
        {
            get
            {
                return this.extraLooseHeight;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the QuadTree2 should use
        /// brute force methods to find items.
        /// </summary>
        /// <value>The default value is false.</value>
        public bool UseBruteForce
        {
            get 
            {
                return this.useBruteForce;
            }

            set
            {
                this.useBruteForce = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this QuadTree2 has been created.
        /// </summary>
        /// <value>
        /// The value is false until <see cref="Create"/> has been called.
        /// </value>
        public bool IsInitialized
        {
            get
            {
                return this.upperLeft != null;
            }
        }

        /// <summary>
        /// Gets the area this QuadTree2 takes up.
        /// </summary>
        public RectangleF Area
        {
            get
            {
                return new RectangleF( 0.0f, 0.0f, this.width, this.height );
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

        #region [ Methods ]

        #region > Initialization <

        /// <summary>
        /// Creates and setups a new <see cref="QuadTree2"/>.
        /// </summary>
        /// <param name="offset">
        /// The offset to the upper-left corner at which the tree should start spanning.
        /// </param>
        /// <param name="totalWidth"> 
        /// The width of the field the tree should spann over.
        /// </param>
        /// <param name="totalHeight">
        /// The height of the field the tree should spann over. 
        /// </param>
        /// <param name="extraLooseWidth"> 
        /// The extra value that can me used to make the tree more loose on the x-axis.
        /// This value must be great or equal zero.
        /// </param>
        /// <param name="extraLooseHeight">
        /// The extra value that can me used to make the tree more loose on the y-axis. 
        /// This value must be great or equal zero.
        /// </param>
        /// <param name="subdivisionCount">
        /// The total number of sub divisions the <see cref="QuadTree2"/> should have.
        /// </param>
        /// <param name="itemCount">
        /// The number of items the tree has space for without needing to allocate new space. 
        /// </param>
        public void Create(
            Vector2 offset,
            float totalWidth,
            float totalHeight,
            float extraLooseWidth,
            float extraLooseHeight,
            int subdivisionCount,
            int itemCount )
        {
            Contract.Requires<ArgumentException>( subdivisionCount >= 0 );

            if( extraLooseWidth < 0 )
                extraLooseWidth = 0;
            if( extraLooseHeight < 0 )
                extraLooseHeight = 0;

            this.offset           = offset;
            this.width            = totalWidth;
            this.height           = totalHeight;
            this.extraLooseWidth  = extraLooseWidth;
            this.extraLooseHeight = extraLooseHeight;
            this.subdivisionCount = subdivisionCount;

            float widthHalf  = width  / 2.0f;
            float heightHalf = height / 2.0f;

            this.items.Clear();
            this.items.Capacity = itemCount;

            this.upperLeft = new QuadTreeNode2( this );
            this.upperLeft.Create( offset.X, offset.Y, widthHalf, heightHalf, subdivisionCount, QuadCorner.UpperLeft, null );

            this.upperRight = new QuadTreeNode2( this );
            this.upperRight.Create( offset.X + widthHalf, offset.Y, widthHalf, heightHalf, subdivisionCount, QuadCorner.UpperRight, null );

            this.bottomLeft = new QuadTreeNode2( this );
            this.bottomLeft.Create( offset.X, offset.Y + heightHalf, widthHalf, heightHalf, subdivisionCount, QuadCorner.BottomLeft, null );

            this.bottomRight = new QuadTreeNode2( this );
            this.bottomRight.Create( offset.X + widthHalf, offset.Y + heightHalf, widthHalf, heightHalf, subdivisionCount, QuadCorner.BottomRight, null );
        }

        /// <summary>
        /// Rebuilds this tree by re-creating all leaves and reinserting all items.
        /// </summary>
        public void RebuildTree()
        {
            var tempList = new List<IQuadTreeItem2>( this.items );

            this.Create( offset, width, height, extraLooseWidth, extraLooseHeight, subdivisionCount, tempList.Count );
            this.items.Clear();

            for( int i = 0; i < tempList.Count; ++i )
            {
                this.Insert( tempList[i] );
            }
        }

        #endregion

        #region > Item Organization <

        #region - Insert -

        /// <summary>
        /// Inserts the specified <see cref="IQuadTreeItem2"/> into this QuadTree2.
        /// </summary>
        /// <param name="item">
        /// The item to insert.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the given item has been inserted;
        /// otherweise <see langword="false"/>.
        /// </returns>
        public bool Insert( IQuadTreeItem2 item )
        {
            Contract.Requires<ArgumentNullException>( item != null );
            Contract.Requires<InvalidOperationException>( this.IsInitialized );

            if( this.upperLeft.Insert( item ) )
            {
                this.items.Add( item );
                return true;
            }

            if( this.upperRight.Insert( item ) )
            {
                this.items.Add( item );
                return true;
            }

            if( this.bottomLeft.Insert( item ) )
            {
                this.items.Add( item );
                return true;
            }

            if( this.bottomRight.Insert( item ) )
            {
                this.items.Add( item );
                return true;
            }

            return false;
        }

        /// <summary>
        /// Inserts the specified <see cref="IQuadTreeItem2"/> into this QuadTree2.
        /// </summary>
        /// <param name="item">
        /// The item to insert.
        /// </param>
        internal void Reinsert( IQuadTreeItem2 item )
        {
            if( this.upperLeft.Insert( item ) )
                return;
            if( this.upperRight.Insert( item ) )
                return;
            if( this.bottomLeft.Insert( item ) )
                return;
            if( this.bottomRight.Insert( item ) )
                return;
        }

        #endregion

        #region - Remove -

        /// <summary>
        /// Tries to remove the specified <see cref="IQuadTreeItem2"/> from this QuadTree2.
        /// </summary>
        /// <param name="item">
        /// The item to remove. Can be null.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the given item has been removed;
        /// otherweise <see langword="false"/>.
        /// </returns>
        public bool Remove( IQuadTreeItem2 item )
        {
            if( item == null )
                return false;

            QuadTreeNode2 leaf = item.QuadTreeLeaf;
            if( leaf == null || leaf.Tree != this )
                return false;

            if( this.items.Remove( item ) )
            {
                if( item.QuadTreeListNode != null )
                {
                    LinkedListNode<IQuadTreeItem2> node = item.QuadTreeListNode;

                    if( node != null )
                        if( node.List != null )
                            node.List.Remove( node );

                    item.QuadTreeLeaf     = null;
                    item.QuadTreeListNode = null;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all items from this <see cref="QuadTree2"/>.
        /// </summary>
        public void RemoveAll()
        {
            if( this.IsInitialized )
            {
                this.upperLeft.RemoveAll();
                this.upperRight.RemoveAll();
                this.bottomLeft.RemoveAll();
                this.bottomRight.RemoveAll();
            }

            this.items.Clear();
        }

        #endregion

        #endregion

        #region > Visability Tests <

        /// <summary>
        /// Finds all visible items in the specified <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="items">
        /// This method inserts all found items into the given list.
        /// </param>
        /// <param name="area">
        /// The <see cref="Rectangle"/> representing the 'view window'.
        /// </param>
        public void FindVisible( List<IQuadTreeItem2> items, Rectangle area )
        {
            if( this.useBruteForce )
            {
                this.FindVisibleBruteForce( items, ref area );
            }
            else
            {
                if( this.IsInitialized )
                {
                    this.upperLeft.FindVisible( items, ref area );
                    this.upperRight.FindVisible( items, ref area );
                    this.bottomLeft.FindVisible( items, ref area );
                    this.bottomRight.FindVisible( items, ref area );
                }
            }
        }

        /// <summary>
        /// Finds all visible items in the specified <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="list">
        /// This method inserts all found items into the given list.
        /// </param>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> representing the 'view window'.
        /// </param>
        private void FindVisibleBruteForce( List<IQuadTreeItem2> list, ref Rectangle rectangle )
        {
            for( int i = 0; i < items.Count; ++i )
            {
                IQuadTreeItem2 item = items[i];

                if( item.IsVisibleToQuadTree )
                {
                    bool result;
                    item.QuadTreeRectangle.Intersects( ref rectangle, out result );

                    if( result )
                    {
                        list.Add( item );
                    }
                }
            }
        }

        #endregion

        #region > Containment Tests <

        /// <summary>
        /// Gets whether the <see cref="QuadTree2"/> contains the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item"> The item to check. Can be null. </param>
        /// <returns>
        /// Returns <see langword="true"/> if the QuadTree2 contains the specified <paramref name="item"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Contains( IQuadTreeItem2 item )
        {
            if( item == null )
                return false;

            QuadTreeNode2 leaf = item.QuadTreeLeaf;
            if( leaf == null || leaf.Tree != this )
                return false;

            return this.items.Contains( item );
        }

        /// <summary>
        /// Gets whether this <see cref="QuadTree2"/> contains an <see cref="IQuadTreeItem2"/>
        /// in the given <paramref name="area"/> while the given additional <paramref name="predicate"/> holds true.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// If <paramref name="predicate"/> is null.
        /// </exception>
        /// <param name="area">
        /// The area to look for items in.
        /// </param>
        /// <param name="predicate">
        /// The additional predicate that must hold true.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/>  if this QuadTree2 contains an IQuadTree2Item in the given <paramref name="area"/>
        /// while the given <paramref name="predicate"/> still holds true;
        /// or otherwise  <see langword="false"/>.
        /// </returns>
        public bool Contains( Rectangle area, Predicate<IQuadTreeItem2> predicate )
        {
            Contract.Requires<ArgumentNullException>( predicate != null );

            if( !this.IsInitialized )
                return false;

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

        #region > Misc <

        /// <summary>
        /// Gets an enumerator over the all items in the <see cref="QuadTree2"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{IQuadTree2Item}"/> for the QuadTree2.
        /// </returns>
        public IEnumerator<IQuadTreeItem2> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator over the all items in the <see cref="QuadTree2"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{IQuadTree2Item}"/> for the QuadTree2.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// States whether to use brute force methods to find items
        /// or to actually use the quad tree.
        /// </summary>
        private bool useBruteForce;

        /// <summary> 
        /// The main leaves of the <see cref="QuadTree2"/>.
        /// </summary>
        private QuadTreeNode2 upperLeft, upperRight, bottomLeft, bottomRight;

        /// <summary> 
        /// The width and height (in pixels) of the <see cref="QuadTree2"/>.
        /// </summary>
        private float width, height, extraLooseWidth, extraLooseHeight;

        /// <summary> 
        /// The number of sub-division the <see cref="QuadTree2"/> has. 
        /// </summary>
        private int subdivisionCount;

        /// <summary>
        /// The list of items the <see cref="QuadTree2"/> contains.
        /// </summary>
        private readonly List<IQuadTreeItem2> items = new List<IQuadTreeItem2>();
        private Vector2 offset;

        #endregion
    }
}
