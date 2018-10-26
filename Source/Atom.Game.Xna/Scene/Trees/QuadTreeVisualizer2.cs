// <copyright file="QuadTreeVisualizer2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.Xna.QuadTreeVisualizer2 class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene.Xna
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Math;
    using Atom.Xna;
    using Atom.Xna.Batches;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements a mechanism that visualizes the structure of a <see cref="QuadTree2"/>
    /// using simple lines.
    /// </summary>
    public sealed class QuadTreeVisualizer2
    {
        /// <summary>
        /// Initializes a new instance of the QuadTreeVisualizer2 class.
        /// </summary>
        /// <param name="drawBatch">
        /// The ITextureDrawBatch that is used to draw the lines that visualize a <see cref="QuadTree2"/>.
        /// </param>
        public QuadTreeVisualizer2( ITextureDrawBatch drawBatch )
        {
            Contract.Requires<ArgumentNullException>( drawBatch != null );

            this.drawBatch = drawBatch;
        }

        /// <summary>
        /// Visualizes the specified QuadTree2.
        /// </summary>
        /// <param name="tree">
        /// The QuadTree2 to visualize.
        /// </param>
        /// <param name="drawContext">
        /// The current IXnaDrawContext.
        /// </param>
        public void Draw( QuadTree2 tree, IXnaDrawContext drawContext )
        {
            if( tree == null )
                return;

            this.tree = tree;

            try
            {
                this.DrawNode( tree.UpperLeft, 0 );
                this.DrawNode( tree.UpperRight, 0 );
                this.DrawNode( tree.BottomLeft, 0 );
                this.DrawNode( tree.BottomRight, 0 );
            }
            finally
            {
                this.tree = null;
            }
        }

        /// <summary>
        /// Draws the specified QuadTreeNode2.
        /// </summary>
        /// <param name="node">
        /// The QuadTreeNode2 to draw.
        /// </param>
        /// <param name="depth">
        /// The current depth of the QuadTreeNode2.
        /// </param>
        private void DrawNode( QuadTreeNode2 node, int depth )
        {
            float factor = this.tree.SubdivisionCount / (float)depth;
   
            this.drawBatch.DrawLineRect(
                (Rectangle)node.Area,
                new Xna.Color( factor, factor, factor, 0.25f ),
                thickness: 1
            );

            if( !node.IsLeaf )
            {
                ++depth;

                this.DrawNode( node.UpperLeft, depth );
                this.DrawNode( node.UpperRight, depth );
                this.DrawNode( node.BottomLeft, depth );
                this.DrawNode( node.BottomRight, depth );
            }
        }

        /// <summary>
        /// The QuadTree2 that is currently beeing drawing by this QuadTreeVisualizer2.
        /// </summary>
        private QuadTree2 tree;

        /// <summary>
        /// The ITextureDrawBatch that is used to draw the lines that visualize a <see cref="QuadTree2"/>.
        /// </summary>
        private readonly ITextureDrawBatch drawBatch;
    }
}
