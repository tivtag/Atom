// <copyright file="IQuadTreeItem2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.IQuadTreeItem2 interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene
{
    using System.Collections.Generic;
    using Atom.Components;
    using Atom.Math;

    /// <summary>
    /// Defines the interface that Objects must implement
    /// that want to be able to be stored in a <see cref="QuadTree2"/>.
    /// </summary>
    public interface IQuadTreeItem2 : IComponent
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IQuadTreeItem2"/> is visible to the <see cref="QuadTree2"/>.
        /// </summary>
        /// <value> 
        /// If this property returns <see langword="true"/> it is simply ignored
        /// when the QuadTree2 is searching for visible objects.
        /// </value>
        bool IsVisibleToQuadTree 
        {
            get;
        }

        /// <summary>
        /// Gets the collision rectangle of the <see cref="IQuadTreeItem2"/>.
        /// </summary>
        /// <value>
        /// The collision rectangle that is used to insert/re-insert this IQuadTree2Item.
        /// Should be up-to-date at all times.
        /// </value>
        RectangleF QuadTreeRectangle
        { 
            get;
        }

        /// <summary>
        /// Gets or sets the <see cref="QuadTreeNode2"/> the <see cref="IQuadTreeItem2"/> is attached to.
        /// This property is used internally. 
        /// </summary>
        /// <value>
        /// This property is used internally. 
        /// All that is required by the user is to provide a storage for the QuadTreeNode2 object.
        /// Don't mess around with it.
        /// </value>
        QuadTreeNode2 QuadTreeLeaf
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="LinkedListNode{IQuadTree2Item}"/> that identifies 
        /// the position of this IQuadTree2Item in the LinkedList of the <see cref="QuadTreeNode2"/>.
        /// This property is used internally. 
        /// </summary>
        /// <value>
        /// This property is used internally. 
        /// All that is required by the user is to provide a storage for the QuadTreeNode2 object.
        /// Don't mess around with it.
        /// </value>
        LinkedListNode<IQuadTreeItem2> QuadTreeListNode
        {
            get;
            set;
        }
    }
}
