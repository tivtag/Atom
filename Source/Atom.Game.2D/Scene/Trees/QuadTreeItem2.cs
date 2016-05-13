// <copyright file="QuadTreeItem2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.QuadTreeItem2 class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Atom.Components;
    using Atom.Components.Collision;
    using Atom.Components.Transform;
    using Atom.Math;

    /// <summary>
    /// Implements a <see cref="Component"/> that allows
    /// the owning <see cref="Entity"/> beeing added to a <see cref="QuadTree2"/>.
    /// </summary>
    /// <remarks>
    /// Depends on the <see cref="Collision2"/> and <see cref="Transform2"/> components.
    /// </remarks>
    public class QuadTreeItem2 : Component, IQuadTreeItem2
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IQuadTreeItem2"/> 
        /// is visible to the <see cref="QuadTree2"/>.
        /// </summary>
        /// <remarks>
        /// Non-visible objects are simply ignored.
        /// </remarks>
        /// <value>The default value is true.</value>
        public bool IsVisibleToQuadTree
        {
            get
            {
                return this._isVisibleToQuadTree;
            }

            set
            {
                this._isVisibleToQuadTree = value;
            }
        }

        /// <summary>
        /// Gets the collision rectangle of the <see cref="IQuadTreeItem2"/>.
        /// </summary>
        /// <value>
        /// The collision rectangle as defined by the <see cref="Collision2"/>
        /// component of the Entity that owns this QuadTree2Item component.
        /// </value>
        public RectangleF QuadTreeRectangle
        {
            get 
            {
                return this.collision.Rectangle; 
            }
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
        public QuadTreeNode2 QuadTreeLeaf
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
        public LinkedListNode<IQuadTreeItem2> QuadTreeListNode
        {
            get;
            set;
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Called when an IComponent has been removed or added to the <see cref="IEntity"/> that owns this IComponent.
        /// </summary>
        public override void InitializeBindings()
        {
            if( this.collision != null )
            {
                this.collision.Changed -= this.OnCollisionChanged;
            }

            this.collision = this.Owner.Components.Find<ICollision2>();
            Debug.Assert( collision != null, "Was unable to find the Collision2 component." );

            this.collision.Changed += this.OnCollisionChanged;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets called when the collision of this <see cref="QuadTreeItem2"/> has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        private void OnCollisionChanged( ICollision2 sender )
        {
            if( this.QuadTreeLeaf != null )
            {
                this.QuadTreeLeaf.CheckItem( this );
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Provides the collision information
        /// about the entity.
        /// </summary>
        private ICollision2 collision;

        /// <summary>
        /// Represents the storage field of the <see cref="IsVisibleToQuadTree"/> property.
        /// </summary>
        private bool _isVisibleToQuadTree = true;

        #endregion
    }
}
