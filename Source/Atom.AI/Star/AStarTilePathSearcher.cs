// <copyright file="AStarTilePathSearcher.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.AI.AStarTilePathSearcher class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.AI
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Atom.Scene.Tiles;

    /// <summary>
    /// An implementation of the A* path algorithm for <see cref="TileMap"/>s.
    /// </summary>
    public class AStarTilePathSearcher : ITilePathSearcher
    {
        #region [ Constants ]

        /// <summary>
        /// States the costs for moving..
        /// </summary>
        private const int CostDiagonal = 14, CostNonDiagonal = 10;

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Setups the A* path searcher to use the specified <see cref="TileMapDataLayer"/>.
        /// </summary>
        /// <param name="layer">
        /// The map layer to setup this <see cref="AStarTilePathSearcher"/> for.
        /// </param>
        public void Setup( TileMapDataLayer layer )
        {
            Contract.Requires<ArgumentNullException>( layer != null );

            this.dataLayer = layer;
            this.tileSize = dataLayer.Map.TileSize;
            this.mapWidth = dataLayer.Map.Width;
            this.mapHeight = dataLayer.Map.Height;

            int listSize = (mapWidth * mapHeight) + 2;
            this.openX = new int[listSize];
            this.openY = new int[listSize];
            this.costF = new int[listSize];
            this.costH = new int[listSize];
            this.openList = new int[listSize];

            int listWidth = mapWidth + 1;
            int listHeight = mapHeight + 1;

            this.costG = new int[listWidth, listHeight];
            this.parentX = new int[listWidth, listHeight];
            this.parentY = new int[listWidth, listHeight];
            this.whichList = new int[listWidth, listHeight];
        }

        /// <summary>
        /// Tries to find a path from the starting point to the target point.
        /// </summary>
        /// <param name="startX"> The starting point on the x-axis. </param>
        /// <param name="startY"> The starting point on the y-axis.</param>
        /// <param name="targetX"> The end point on the x-axis.</param>
        /// <param name="targetY"> The end point on the y-axis.</param>
        /// <param name="caller">The object to search a path for.</param>
        /// <param name="tileHandler">
        /// The <see cref="ITileHandler&lt;TCallerType&gt;"/> 
        /// to use when checking whether a tile is walkable.
        /// </param>
        /// <returns> The path from the starting point to the target point. </returns>
        /// <typeparam name="TCallerType">The type of the object that a path is searched for.</typeparam>
        public TilePath FindPath<TCallerType>(
            int startX,
            int startY,
            int targetX,
            int targetY,
            TCallerType caller,
            ITileHandler<TCallerType> tileHandler )
        {
            startX /= this.tileSize;
            startY /= this.tileSize;
            targetX /= this.tileSize;
            targetY /= this.tileSize;

            return this.FindPathTile<TCallerType>( startX, startY, targetX, targetY, caller, tileHandler );
        }

        /// <summary>
        /// Tries to find a path from the starting tile to the target tile.
        /// </summary>
        /// <param name="startX">
        /// The starting point on the x-axis in tile-space.
        /// </param>
        /// <param name="startY">
        /// The starting point on the y-axis in tile-space.
        /// </param>
        /// <param name="targetX">
        /// The end point on the x-axis in tile-space.
        /// </param>
        /// <param name="targetY">
        /// The end point on the y-axis in tile-space.
        /// </param>
        /// <param name="caller">
        /// The object to search a path for.
        /// </param>
        /// <param name="tileHandler">
        /// The <see cref="ITileHandler&lt;TCallerType&gt;"/> 
        /// to use when checking whether a tile is walkable.
        /// </param>
        /// <returns>
        /// The path from the starting tile to the target tile.
        /// </returns>
        /// <typeparam name="TCallerType">
        /// The type of the object that a path is searched for.
        /// </typeparam>
        public TilePath FindPathTile<TCallerType>(
            int startX,
            int startY,
            int targetX,
            int targetY,
            TCallerType caller,
            ITileHandler<TCallerType> tileHandler )
        {
            try
            {
                // Temp values
                int onOpenList = 0, parentXval = 0, parentYval = 0,
                a = 0, b = 0, m = 0, u = 0, v = 0, temp = 0,
                addedGCost = 0, tempGcost = 0,
                tempx, pathX, pathY,
                newOpenListItemID = 0;

                bool isCornerWalkable = false;

                // 1.Quick Path Checks:
                // Under the some circumstances no path needs to be generated ...
                if( tileHandler.IsWalkable( dataLayer.GetTileAt( targetX, targetY ), caller ) == false )
                    return TilePath.CreateNotFound( dataLayer );

                // If starting location and target are in the same location...
                if( startX == targetX && startY == targetY )
                    return TilePath.CreateFoundStartIsTarget( dataLayer );

                // 2.Reset some variables that need to be cleared
                if( this.onClosedList > 1000000 )
                {
                    Array.Clear( this.whichList, 0, this.whichList.Length );
                    this.onClosedList = 10;
                }

                // changing the values of onOpenList and onClosed list is faster than redimming whichList() array
                this.onClosedList = this.onClosedList + 2;
                onOpenList = this.onClosedList - 1;

                this.costG[startX, startY] = 0; // reset starting square's G value to 0

                // 4.Add the starting location to the open list of squares to be checked.
                int numberOfOpenListItems = 1;
                this.openList[1] = 1;
                this.openX[1] = startX;
                this.openY[1] = startY;

                // 5.Do the following until a path is found or deemed nonexistent.
                do
                {
                    // 6.If the open list is not empty, take the first cell off of the list.
                    //   This is the lowest F cost cell on the open list.
                    if( numberOfOpenListItems != 0 )
                    {
                        // 7. Pop the first item off the open list.
                        parentXval = this.openX[this.openList[1]];
                        parentYval = this.openY[this.openList[1]]; // record cell coordinates of the item
                        this.whichList[parentXval, parentYval] = this.onClosedList; // add the item to the closed list

                        // Open List = Binary Heap: Delete this item from the open list, which
                        // is maintained as a binary heap.
                        --numberOfOpenListItems; // reduce number of open list items by 1

                        // Delete the top item in binary heap and reorder the heap, with the lowest F cost item rising to the top.
                        this.openList[1] = this.openList[numberOfOpenListItems + 1]; // move the last item in the heap up to slot #1
                        v = 1;

                        // Repeat the following until the new item in slot #1 sinks to its proper spot in the heap.
                        do
                        {
                            u = v;
                            int u2 = 2 * u;
                            int u2plus1 = u2 + 1;

                            // If both children exist:
                            if( u2plus1 <= numberOfOpenListItems )
                            {
                                // Check if the F cost of the parent is greater than each child.
                                // Select the lowest of the two children.
                                if( this.costF[this.openList[u]] >= this.costF[this.openList[u2]] )
                                    v = u2;

                                if( this.costF[this.openList[v]] >= this.costF[this.openList[u2plus1]] )
                                    v = u2plus1;
                            }
                            else
                            {
                                // If only child #1 exists
                                if( u2 <= numberOfOpenListItems )
                                {
                                    // Check if the F cost of the parent is greater than child #1
                                    if( this.costF[this.openList[u]] >= this.costF[this.openList[u2]] )
                                        v = u2;
                                }
                            }

                            // if parent's F is > one of its children, swap them.
                            if( u != v )
                            {
                                temp = this.openList[u];
                                this.openList[u] = this.openList[v];
                                this.openList[v] = temp;
                            }
                            else
                            {
                                break; // otherwise, exit loop
                            }
                        }
                        while( true );

                        // 7.Check the adjacent squares. (Its "children" -- these path children
                        //   are similar, conceptually, to the binary heap children mentioned
                        //   above, but don't confuse them. They are different. Path children
                        //   are portrayed in Demo 1 with grey pointers pointing toward
                        //   their parents.) Add these adjacent child squares to the open list
                        //   for later consideration if appropriate (see various if statements
                        //   below).
                        for( b = parentYval - 1; b <= parentYval + 1; ++b )
                        {
                            for( a = parentXval - 1; a <= parentXval + 1; ++a )
                            {
                                // If not off the map (do this first to avoid array out-of-bounds errors)
                                if( a != -1 && b != -1 && a != this.mapWidth && b != this.mapHeight )
                                {
                                    // If not already on the closed list (items on the closed list have
                                    // already been considered and can now be ignored).
                                    if( whichList[a,b] != onClosedList )
                                    {
                                        // If not a wall/obstacle square.
                                        if( tileHandler.IsWalkable( this.dataLayer.GetTileAt( a, b ), caller ) )
                                        {
                                            // Don't cut across corners
                                            isCornerWalkable = true;

                                            if( a == parentXval - 1 )
                                            {
                                                if( b == parentYval - 1 )
                                                {
                                                    if( tileHandler.IsWalkable( this.dataLayer.GetTileAt( parentXval - 1, parentYval ), caller ) == false ||
                                                        tileHandler.IsWalkable( this.dataLayer.GetTileAt( parentXval, parentYval - 1 ), caller ) == false )
                                                        isCornerWalkable = false;
                                                }
                                                else if( b == parentYval + 1 )
                                                {
                                                    if( tileHandler.IsWalkable( this.dataLayer.GetTileAt( parentXval, parentYval + 1 ), caller ) == false ||
                                                        tileHandler.IsWalkable( this.dataLayer.GetTileAt( parentXval - 1, parentYval ), caller ) == false )
                                                        isCornerWalkable = false;
                                                }
                                            }
                                            else if( a == parentXval + 1 )
                                            {
                                                if( b == parentYval - 1 )
                                                {
                                                    if( tileHandler.IsWalkable( this.dataLayer.GetTileAt( parentXval, parentYval - 1 ), caller ) == false ||
                                                        tileHandler.IsWalkable( this.dataLayer.GetTileAt( parentXval + 1, parentYval ), caller ) == false )
                                                        isCornerWalkable = false;
                                                }
                                                else if( b == parentYval + 1 )
                                                {
                                                    if( tileHandler.IsWalkable( this.dataLayer.GetTileAt( parentXval + 1, parentYval ), caller ) == false ||
                                                        tileHandler.IsWalkable( this.dataLayer.GetTileAt( parentXval, parentYval + 1 ), caller ) == false )
                                                        isCornerWalkable = false;
                                                }
                                            }

                                            if( isCornerWalkable )
                                            {
                                                // If not already on the open list, add it to the open list.
                                                if( this.whichList[a, b] != onOpenList )
                                                {
                                                    // Create a new open list item in the binary heap.
                                                    newOpenListItemID = newOpenListItemID + 1; // each new item has a unique ID#

                                                    m = numberOfOpenListItems + 1;

                                                    // place the new open list item (actually, its ID#) at the bottom of the heap
                                                    this.openList[m] = newOpenListItemID;
                                                    this.openX[newOpenListItemID] = a;
                                                    this.openY[newOpenListItemID] = b; // record the x and y coordinates of the new item

                                                    // Figure out its G cost
                                                    if( System.Math.Abs( a - parentXval ) == 1 && System.Math.Abs( b - parentYval ) == 1 )
                                                        addedGCost = CostDiagonal; // cost of going to diagonal squares
                                                    else
                                                        addedGCost = CostNonDiagonal; // cost of going to non-diagonal squares
                                                    this.costG[a, b] = this.costG[parentXval, parentYval] + addedGCost;

                                                    // Figure out its H and F costs and parent
                                                    this.costH[this.openList[m]] = 10 * (System.Math.Abs( a - targetX ) + System.Math.Abs( b - targetY ));
                                                    this.costF[this.openList[m]] = this.costG[a, b] + this.costH[this.openList[m]];
                                                    this.parentX[a, b] = parentXval;
                                                    this.parentY[a, b] = parentYval;

                                                    // Move the new open list item to the proper place in the binary heap.
                                                    // Starting at the bottom, successively compare to parent items,
                                                    // swapping as needed until the item finds its place in the heap
                                                    // or bubbles all the way to the top (if it has the lowest F cost).
                                                    // Now...
                                                    // While item hasn't bubbled to the top (Rows=1).
                                                    while( m != 1 )
                                                    {
                                                        int mHalf = m / 2;
                                                        int openListM = this.openList[m];
                                                        int openListHalfM = this.openList[mHalf];
                                                    
                                                        // Check if child's F cost is < parent's F cost. If so, swap them.
                                                        if( this.costF[openListM] <= this.costF[openListHalfM] )
                                                        {
                                                            temp = openListHalfM;
                                                            this.openList[mHalf] = openListM;
                                                            this.openList[m] = temp;
                                                            m = mHalf;
                                                        }
                                                        else
                                                            break;
                                                    }

                                                    ++numberOfOpenListItems;

                                                    // Change whichList to show that the new item is on the open list.
                                                    this.whichList[a, b] = onOpenList;
                                                }
                                                else
                                                {
                                                    // 8. If adjacent cell is already on the open list, check to see if this
                                                    // path to that cell from the starting location is a better one.
                                                    // If so, change the parent of the cell and its G and F costs.

                                                    // Figure out the G cost of this possible new path
                                                    if( System.Math.Abs( a - parentXval ) == 1 && System.Math.Abs( b - parentYval ) == 1 )
                                                        addedGCost = CostDiagonal; // cost of going to diagonal tiles
                                                    else
                                                        addedGCost = CostNonDiagonal; // cost of going to non-diagonal tiles

                                                    tempGcost = this.costG[parentXval, parentYval] + addedGCost;

                                                    // If this path is shorter (G cost is lower) then change
                                                    // the parent cell, G cost and F cost.
                                                    if( tempGcost < this.costG[a, b] )
                                                    {
                                                        this.parentX[a, b] = parentXval; // change the square's parent
                                                        this.parentY[a, b] = parentYval;
                                                        this.costG[a, b]   = tempGcost;  // change the G cost

                                                        // Because changing the G cost also changes the F cost, if
                                                        // the item is on the open list we need to change the item's
                                                        // recorded F cost and its position on the open list to make
                                                        // sure that we maintain a properly ordered open list.
                                                        for( int x = 1; x <= numberOfOpenListItems; ++x )
                                                        {
                                                            int openListX = this.openList[x];

                                                            // If item found
                                                            if( this.openX[openListX] == a && this.openY[openListX] == b )
                                                            {
                                                                // change the F cost
                                                                this.costF[openListX] = this.costG[a, b] + this.costH[openListX];

                                                                // See if changing the F score bubbles the item up from
                                                                // it's current location in the heap
                                                                m = x;

                                                                // While item hasn't bubbled to the top (Rows=1)
                                                                while( m != 1 )
                                                                {
                                                                    int mHalf = m / 2;
                                                                    int openListM = this.openList[m];
                                                                    int openListHalfM = this.openList[mHalf];

                                                                    // Check if child is < parent. If so, swap them.
                                                                    if( this.costF[openListM] < this.costF[openListHalfM] )
                                                                    {
                                                                        temp = openListHalfM;
                                                                        openList[mHalf] = openListM;
                                                                        openList[m] = temp;
                                                                        m = mHalf;
                                                                    }
                                                                    else
                                                                        break;
                                                                }
                                                                break; // exit for x = loop                                           
                                                            } // If openX(openList(x)) = a
                                                        } // For x = 1 To numberOfOpenListItems
                                                    } // If tempGcost < Gcost(a,b)
                                                } // else If whichList(a,b) = onOpenList
                                            } // If not cutting a corner
                                        } // If not a wall/obstacle square.
                                    } // If not already on the closed list
                                } // If not off the map
                            } // for (a = parentXval-1; a <= parentXval+1; a++){
                        } // for (b = parentYval-1; b <= parentYval+1; b++){
                    } // if (numberOfOpenListItems != 0)

                    // 9.If open list is empty then there is no path.
                    else
                    {
                        return TilePath.CreateNotFound( dataLayer );
                    }

                    // If target is added to open list then path has been found.
                    if( this.whichList[targetX, targetY] == onOpenList )
                    {
                        break;
                    }
                }
                while( true ); // Do until path is found or deemed nonexistent

                // 10: save path:
                // a.Working backwards from the target to the starting location by checking
                //   each cell's parent, figure out the length of the path.
                pathX = targetX;
                pathY = targetY;

                int pathLength = 0;
                do
                {
                    // Look up the parent of the current cell.
                    tempx = this.parentX[pathX, pathY];
                    pathY = this.parentY[pathX, pathY];
                    pathX = tempx;

                    // Figure out the path length
                    ++pathLength;
                }
                while( pathX != startX || pathY != startY );

                // b. Now copy the path information over to the databank. Since we are
                //    working backwards from the target to the start location, we copy
                //    the information to the data bank in reverse order. The result is
                //    a properly ordered set of path data, from the first step to the
                //    last.
                pathX = targetX;
                pathY = targetY;

                Point2[] newPath = new Point2[pathLength + 1];
                int newPathIndex = 0;

                do
                {
                    newPath[newPathIndex] = new Point2( pathX, pathY );

                    // d.Look up the parent of the current cell.
                    tempx = this.parentX[pathX, pathY];
                    pathY = this.parentY[pathX, pathY];
                    pathX = tempx;
                    ++newPathIndex;

                    // e.If we have reached the starting square, exit the loop.
                }
                while( pathX != startX || pathY != startY );

                newPath[pathLength] = new Point2( startX, startY );

                Array.Reverse( newPath );
                return TilePath.CreateFound( dataLayer, newPath );
            }
            catch
            {
                return TilePath.CreateNotFound( dataLayer );
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The width of the map in tile-space.
        /// </summary>
        private int mapWidth;

        /// <summary>
        /// The height of the map in tile-space.
        /// </summary>
        private int mapHeight;

        /// <summary>
        /// The size of the tiles of the map.
        /// </summary>
        private int tileSize;

        /// <summary> 
        /// The number of items on the closed list (??).
        /// </summary>
        private int onClosedList = 10;

        /// <summary>
        /// The underlying map layer.
        /// </summary>
        private TileMapDataLayer dataLayer;

        #region Arrays

        /// <summary>
        /// Holds ID# of open list items.
        /// </summary>
        private int[] openList;

        /// <summary>
        /// Records whether a cell is on the open list or on the closed list.
        /// </summary>
        private int[,] whichList;

        /// <summary>
        /// Stores the location of an item on the open list.
        /// </summary>
        private int[] openX, openY;

        /// <summary>
        /// Stores parent location of each cell.
        /// </summary>
        private int[,] parentX, parentY;

        /// <summary>
        /// Stores the F cost of a cell on the open list.
        /// </summary>
        private int[] costF;

        /// <summary>
        /// Stores the H cost of a cell on the open list.
        /// </summary>
        private int[] costH;

        /// <summary>
        /// Stores the G cost of each cell.
        /// </summary>
        private int[,] costG;

        #endregion

        #endregion
    }
}
