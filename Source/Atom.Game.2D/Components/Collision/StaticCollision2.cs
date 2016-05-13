// <copyright file="StaticCollision2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Components.Collision.StaticCollision2 class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Components.Collision
{
    using System;
    using Atom.Math;

    /// <summary>
    /// Implements the <see cref="Collision2"/> component
    /// by using the static <see cref="Offset"/> and <see cref="Size"/> data
    /// provided by the <see cref="StaticCollision2"/> to create the Collision Shapes.
    /// </summary>
    public class StaticCollision2 : Collision2
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the static collision offset that is applied 
        /// from the <see cref="Atom.Components.Transform.Transform2.Position"/> of the owning <see cref="Entity"/> 
        /// to the actual start of the <see cref="Collision2.Rectangle"/>.
        /// </summary>
        /// <value>The offset from the position of the static-collideable Entity to the collision area.</value>
        public Vector2 Offset
        {
            get
            {
                return this.collisionOffset;
            }

            set
            {
                this.collisionOffset = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the static size of the <see cref="Collision2.Rectangle"/>.
        /// </summary>
        /// <value>The collision size of the static-collideable Entity.</value>
        public Vector2 Size
        {
            get
            {
                return this.collisionSize;
            }

            set
            {
                this.collisionSize = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets the static width of the <see cref="Collision2.Rectangle"/>.
        /// </summary>
        /// <value>The collision width of the static-collideable Entity.</value>
        public float Width
        {
            get 
            { 
                return this.collisionSize.X;
            }
        }

        /// <summary>
        /// Gets the static height of the <see cref="Collision2.Rectangle"/>.
        /// </summary>
        /// <value>The collision height of the static-collideable Entity.</value>
        public float Height
        {
            get 
            { 
                return this.collisionSize.Y; 
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Sets the properties of the <see cref="StaticCollision2"/> in one go.
        /// </summary>
        /// <remarks>
        /// Reduces the number of CollisionShape refreshes.
        /// </remarks>
        /// <param name="collisionOffset"> 
        /// Thestatic collision offset that is applied 
        /// from the <see cref="Atom.Components.Transform.Transform2.Position"/> of the owning <see cref="Entity"/> 
        /// to the actual start of the <see cref="Collision2.Rectangle"/>.
        /// </param>
        /// <param name="collisionSize">
        /// The static size of the <see cref="Collision2.Rectangle"/>.
        /// </param>
        public void Set( Vector2 collisionOffset, Vector2 collisionSize )
        {
            this.collisionOffset = collisionOffset;
            this.collisionSize = collisionSize;

            this.Refresh();
        }

        /// <summary>
        /// Override to hook any events onto the given <see cref="Atom.Components.Transform.ITransform2"/> component.
        /// </summary>
        /// <param name="transform">
        /// The related ITransform2 component. Is never null.
        /// </param>
        protected override void Hook( Atom.Components.Transform.ITransform2 transform )
        {
            transform.Changed += this.OnTransformChanged;
        }

        /// <summary>
        /// Override to unhook any events from the given <see cref="Atom.Components.Transform.ITransform2"/> component.
        /// </summary>
        /// <remarks>
        /// Unhook should remove all events that were added in <see cref="Hook"/>.
        /// </remarks>
        /// <param name="transform">
        /// The related ITransform2 component. Is never null.
        /// </param>
        protected override void Unhook( Atom.Components.Transform.ITransform2 transform )
        {
            transform.Changed -= this.OnTransformChanged;
        }

        /// <summary>
        /// Creates the collision shapes of this Collision2 component
        /// based on the current situation of the <see cref="IEntity"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="Circle"/> and <see cref="Rectangle"/> shapes should be updated.
        /// </remarks>
        protected override void ActuallyRefreshShapes()
        {
            var transform        = this.Transform;
            Vector2 scale        = transform.Scale;
            Vector2 scaledOrigin = transform.Origin * scale;

            // Create Collision Rectangle:
            this.Rectangle = new RectangleF(
                transform.Position + (collisionOffset * scale) - scaledOrigin,
                collisionSize * scale
            );

            // Incoporate rotation:
            float rotation = transform.Rotation;
            if( rotation != 0.0f )
                this.Rectangle = RectangleF.FromOrientedRectangle( this.Rectangle, rotation, scaledOrigin );

            // Create Collision Circle:
            this.Circle = Circle.FromRectangle( this.Rectangle );
        }

        /// <summary>
        /// Gets called when the transformation of the Entity that owns 
        /// this <see cref="StaticCollision2"/> object has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        private void OnTransformChanged( Atom.Components.Transform.ITransform2 sender )
        {
            this.Refresh();
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The static collision offset that is applied
        /// from the <see cref="Atom.Components.Transform.Transform2.Position"/> of the owning <see cref="Entity"/>
        /// to the actual start of the <see cref="Collision2.Rectangle"/>.
        /// </summary>
        private Vector2 collisionOffset;

        /// <summary>
        /// The static size of the <see cref="Collision2.Rectangle"/>.
        /// </summary>
        private Vector2 collisionSize;

        #endregion
    }
}
