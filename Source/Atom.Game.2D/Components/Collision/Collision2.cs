// <copyright file="Collision2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Components.Collision.Collision2 class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Components.Collision
{
    using System;
    using System.Diagnostics;
    using Atom.Components.Transform;
    using Atom.Math;

    /// <summary>
    /// Defines an abstract base implemententation of the <see cref="ICollision2"/> component interface;
    /// providing access to the collision information of an <see cref="IEntity"/> in two-dimensional space.
    /// </summary>
    /// <remarks>
    /// The Collision2 component depends on the <see cref="ITransform2"/> component
    /// for transformation information.
    /// </remarks>
    public abstract class Collision2 : Component, ICollision2
    {
        #region [ Events ]

        /// <summary>
        /// Fired when the collision information of this 
        /// Collision2 component has been changed/refreshed.
        /// </summary>
        public event SimpleEventHandler<ICollision2> Changed;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the collision <see cref="RectangleF"/> of the <see cref="IEntity"/>
        /// that owns this Collision2 component.
        /// </summary>
        /// <value>The collision rectangle of the collideable IEntity.</value>
        public RectangleF Rectangle
        {
            get
            {
                return this.rectangle;
            }

            protected set
            {
                this.rectangle = value;            
            }
        }

        /// <summary>
        /// Gets or sets the collision <see cref="Circle"/> of the <see cref="IEntity"/>
        /// that owns this Collision2 component.
        /// </summary>
        /// <value>The collision circle of the collideable IEntity.</value>
        public Circle Circle
        {
            get
            {
                return circle;
            }

            protected set
            {
                this.circle = value;
            }
        }

        /// <summary>
        /// Gets the center of the collision rectangle/circle of the <see cref="IEntity"/>
        /// that owns this Collision2 component.
        /// </summary>
        /// <value>The center of the collision rectangle of the collideable IEntity.</value>
        public Vector2 Center
        {
            get
            {
                return circle.Center;
            }
        }

        /// <summary>
        /// Gets the position of the collision rectangle of the <see cref="IEntity"/>
        /// that owns this Collision2 component.
        /// </summary>
        /// <value>The position of the collision rectangle of the collideable IEntity.</value>
        public Vector2 Position
        {
            get
            {
                return rectangle.Position;
            }
        }

        /// <summary>
        /// Gets the <see cref="ITransform2"/> component of the <see cref="IEntity"/>
        /// that owns this Collision2 component.
        /// </summary>
        /// <remarks>
        /// This operation is very fast; as no look-up is required.
        /// </remarks>
        /// <value>
        /// The <see cref="ITransform2"/> component of the <see cref="IEntity"/>
        /// that owns this Collision2 component.
        /// </value>
        public ITransform2 Transform
        {
            get { return transform; }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Called when an IComponent has been removed or added
        /// to the <see cref="IEntity"/> that owns this Collision2 component.
        /// </summary>
        public override void InitializeBindings()
        {
            var acquiredTransform = this.Owner.Components.Find<ITransform2>();

            if( acquiredTransform != this.transform )
            {
                if( this.transform != null )
                    this.Unhook( this.transform );

                this.transform = acquiredTransform;

                if( this.transform != null )
                    this.Hook( this.transform );
            }
        }

        /// <summary>
        /// Override to hook any events onto the given <see cref="ITransform2"/> component.
        /// </summary>
        /// <param name="transform">
        /// The related ITransform2 component. Is never null.
        /// </param>
        protected abstract void Hook( ITransform2 transform );
        
        /// <summary>
        /// Override to unhook any events from the given <see cref="ITransform2"/> component.
        /// </summary>
        /// <remarks>
        /// Unhook should remove all events that were added in <see cref="Hook"/>.
        /// </remarks>
        /// <param name="transform">
        /// The related ITransform2 component. Is never null.
        /// </param>
        protected abstract void Unhook( ITransform2 transform );

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Refrehes the collision shapes of this Collision2 component.
        /// </summary>
        protected void Refresh()
        {
            this.ActuallyRefreshShapes();

            if( this.Changed != null )
            {
                this.Changed( this );
            }
        }

        /// <summary>
        /// Creates the collision shapes of this Collision2 component
        /// based on the current situation of the <see cref="IEntity"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="Circle"/> and <see cref="Rectangle"/> shapes should be updated.
        /// </remarks>
        protected abstract void ActuallyRefreshShapes();

        /// <summary>
        /// Returns a humen-readable string representation of this Collision2 component.
        /// </summary>
        /// <returns>
        /// Returns a humen-readable description of this Collision2 component.
        /// </returns>
        public override string ToString()
        {
            IFormatProvider formatProvider = System.Globalization.CultureInfo.CurrentCulture;

            return string.Format(
                formatProvider,
                "{0} [Owner='{1}' Rectangle='{2}' Circle='{3}']",
                this.GetType().Name,
                this.Owner,
                this.Rectangle.ToString( formatProvider ),
                this.Circle.ToString( formatProvider )
            );
        }

        #region > Intersection Tests <

        /// <summary>
        /// Returns whether the <see cref="Rectangle"/> of the given Collision2 component
        /// intersects with the <see cref="Rectangle"/> of this Collision2 component.
        /// </summary>
        /// <param name="collision">
        /// The collision component to test against.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the rectangles intersect;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Intersects( Collision2 collision )
        {
            Debug.Assert( collision != null, "The given Collision2 component is null." );

            bool result;
            this.rectangle.Intersects( ref collision.rectangle, out result );
            return result;
        }

        /// <summary>
        /// Returns whether the <see cref="Rectangle"/> of the given ICollision2 component
        /// intersects with the <see cref="Rectangle"/> of this Collision2 component.
        /// </summary>
        /// <param name="collision">
        /// The collision component to test against.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the rectangles intersect;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Intersects( ICollision2 collision )
        {
            Debug.Assert( collision != null, "The given ICollision2 component is null." );

            return this.rectangle.Intersects( collision.Rectangle );
        }

        /// <summary>
        /// Returns whether the given <see cref="Rectangle"/> intersects with
        /// the <see cref="Rectangle"/> of this Collision2 component.
        /// </summary>
        /// <param name="area">
        /// The area to check this Collision2 component against.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the rectangles intersect;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Intersects( Rectangle area )
        {
            bool result;
            this.rectangle.Intersects( ref area, out result );
            return result;
        }

        /// <summary>
        /// Returns whether the given <see cref="Rectangle"/> intersects with
        /// the <see cref="Rectangle"/> of this Collision2 component.
        /// </summary>
        /// <param name="area">
        /// The area to check this Collision2 component against.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the rectangles intersect;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [CLSCompliant( false )]
        public bool Intersects( ref Rectangle area )
        {
            bool result;
            this.rectangle.Intersects( ref area, out result );
            return result;
        }

        /// <summary>
        /// Returns whether the given <see cref="RectangleF"/> intersects with
        /// the <see cref="Rectangle"/> of this Collision2 component.
        /// </summary>
        /// <param name="area">
        /// The area to check this Collision2 component against.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the rectangles intersect;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool Intersects( RectangleF area )
        {
            bool result;
            this.rectangle.Intersects( ref area, out result );
            return result;
        }

        /// <summary>
        /// Returns whether the given <see cref="RectangleF"/> intersects with
        /// the <see cref="Rectangle"/> of this Collision2 component.
        /// </summary>
        /// <param name="area">
        /// The area to check this Collision2 component against.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the rectangles intersect;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [CLSCompliant( false )]
        public bool Intersects( ref RectangleF area )
        {
            bool result;
            this.rectangle.Intersects( ref area, out result );
            return result;
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The collision <see cref="Rectangle"/>.
        /// </summary>
        private RectangleF rectangle;

        /// <summary>
        /// The collision <see cref="Circle"/>.
        /// </summary>
        private Circle circle;

        /// <summary>
        /// The <see cref="ITransform2"/> component.
        /// </summary>
        private ITransform2 transform;

        #endregion
    }
}
