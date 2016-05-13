// <copyright file="RigidFixture.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Physics.Farseer.RigidFixture class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Physics.Farseer
{
    using System;
    using Atom.Components;
    using Atom.Components.Collision;
    using Atom.Components.Transform;
    using Atom.Math;
    using FarC = FarseerPhysics.Collision;
    using FarD = FarseerPhysics.Dynamics;
    using FarseerPhysics.Dynamics;
    using Xna = Microsoft.Xna.Framework;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Dynamics.Contacts;

    /// <summary>
    /// Implements a <see cref="Component"/> that provides an <see cref="IRigidBody"/>
    /// with a geometry shape used for collision.
    /// </summary>
    /// <remarks>
    /// This component depends on the <see cref="IRigidBody"/> component.
    /// </remarks>
    public class RigidFixture : Collision2, IRigidFixture
    {
        #region [ Events ]
        
        /// <summary>
        /// Raised when this RigidGeometry has collided with another <see cref="IRigidFixture"/>.
        /// </summary>
        public event RelaxedEventHandler<CollisionEventArgs> Collided;

        /// <summary>
        /// Raised when this RigidGeometry has seperated from another <see cref="IRigidFixture"/>.
        /// </summary>
        public event RelaxedEventHandler<SeperationEventArgs> Seperated;

        /// <summary>
        /// Raised when the Farseer Fixture of this <see cref="RigidFixture"/> component has changed.
        /// </summary>
        public event RelaxedEventHandler<ChangedValue<Fixture>> FixtureChanged;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the Farseer Fixture object of this RigidGeometry component.
        /// </summary>
        public Fixture Fixture
        {
            get
            {
                return this.fixture;
            }

            set
            {
                if( this.fixture == value )
                    return;

                var oldValue  = this.fixture;
                this.fixture = value;
                
                this.OnGeometryChanged( oldValue, value );
            }
        }

        /// <summary>
        /// Gets the <see cref="IRigidBody"/> this RigidGeometry depends on.
        /// </summary>
        public IRigidBody RigidBody
        {
            get
            {
                return this.rigidBody;
            }
        }

        /// <summary>
        /// Gets or sets an optional <see cref="ICollisionFilter"/>; that provides a mechanism
        /// for deciding whether this RigidGeometry should collide with another RigidGeometry.
        /// </summary>
        public ICollisionFilter CollisionFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the friction coefficient of this IRigidGeometry.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the Farseer <see cref="Fixture"/> is null.
        /// </exception>
        /// <value>The <see cref="FarseerPhysics.Collision.Geom.FrictionCoefficient"/> property.</value>
        public float Friction
        {
            get
            {
                return this.fixture.Friction;
            }

            set
            {
                this.fixture.Friction = value;
            }
        }

        /// <summary>
        /// Gets or sets the restitution coefficient of this IRigidGeometry.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the Farseer <see cref="Fixture"/> is null.
        /// </exception>
        /// <value>The <see cref="FarseerPhysics.Collision.Geom.RestitutionCoefficient"/> property.</value>
        public float Restitution
        {
            get
            {
                return this.fixture.Restitution;
            }

            set
            {
                this.fixture.Restitution = value;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Called when an IComponent has been removed or added
        /// to the <see cref="IEntity"/> that owns this Collision2 component.
        /// </summary>
        public override void InitializeBindings()
        {
            var acquiredRigidBody = this.Owner.Components.Find<IRigidBody>();

            if( acquiredRigidBody != this.rigidBody )
            {
                if( this.rigidBody != null )
                    this.Unhook( this.rigidBody );

                this.rigidBody = acquiredRigidBody;

                if( this.rigidBody != null )
                    this.Hook( this.rigidBody );
            }
        }

        #region > Component Hooks <

        /// <summary>
        /// Override to hook any events onto the given <see cref="IRigidBody"/> component.
        /// </summary>
        /// <param name="rigidBody">
        /// The related RigidBody component. Is never null.
        /// </param>
        private void Hook( IRigidBody rigidBody )
        {
            rigidBody.BodyChanged += OnBodyChanged;
        }

        /// <summary>
        /// Override to unhook any events from the given <see cref="IRigidBody"/> component.
        /// </summary>
        /// <remarks>
        /// Unhook should remove all events that were added in <see cref="Hook( IRigidBody )"/>.
        /// </remarks>
        /// <param name="rigidBody">
        /// The related RigidBody component. Is never null.
        /// </param>
        private void Unhook( IRigidBody rigidBody )
        {
            rigidBody.BodyChanged -= OnBodyChanged;
        }

        /// <summary>
        /// Override to hook any events onto the given <see cref="ITransform2"/> component.
        /// </summary>
        /// <param name="transform">
        /// The related ITransform2 component. Is never null.
        /// </param>
        protected override void Hook( ITransform2 transform )
        {
            // do nothing.
        }

        /// <summary>
        /// Override to unhook any events from the given <see cref="ITransform2"/> component.
        /// </summary>
        /// <remarks>
        /// Unhook should remove all events that were added in <see cref="Hook( ITransform2 )"/>.
        /// </remarks>
        /// <param name="transform">
        /// The related ITransform2 component. Is never null.
        /// </param>
        protected override void Unhook( ITransform2 transform )
        {
            // do nothing.
        }

        #endregion

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Creates the collision shapes of this RigidGeometry component
        /// based on the current situation of the <see cref="IEntity"/>.
        /// </summary>
        protected override void ActuallyRefreshShapes()
        {
            if( this.fixture == null )
            {
                this.Rectangle = new RectangleF();
                this.Circle    = new Circle();
            }
            else
            {
                FarC.AABB aabb;
                fixture.GetAABB(out aabb, 0);
                var rectangle = new RectangleF(aabb.LowerBound.X, aabb.LowerBound.Y, aabb.Extents.X * 2, aabb.Extents.Y * 2);

                this.Rectangle = rectangle;
                this.Circle    = Circle.FromRectangle( rectangle );
            }
        }

        /// <summary>
        /// Called when the <see cref="FarseerPhysics.Dynamics.Body"/> of this <see cref="RigidBody"/> 
        /// component has changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="changedValue">
        /// The ChangedValue{FarseerPhysics.Dynamics.Body} that contains data
        /// about the property that has been changed.
        /// </param>
        private void OnBodyChanged( object sender, ChangedValue<FarD.Body> changedValue )
        {
            if( changedValue.OldValue != null )
                changedValue.OldValue.Updated -= this.OnBodyUpdated;

            if( changedValue.NewValue != null )
                changedValue.NewValue.Updated += this.OnBodyUpdated;
        }

        /// <summary>
        /// Called when the Farseer body's state has been updated.
        /// </summary>
        /// <param name="position">The new position of the farseer Body.</param>
        /// <param name="rotation">The new rotation of the farseer Body.</param>
        private void OnBodyUpdated( Xna.Vector2 position, float rotation )
        {
            this.Refresh();
        }

        /// <summary>
        /// Called when the <see cref="Fixture"/> of this <see cref="RigidFixture"/> 
        /// component has changed.
        /// </summary>
        /// <param name="oldFixture">
        /// The old geometry value.
        /// </param>
        /// <param name="newFixture">
        /// The new geometry value.
        /// </param>
        protected virtual void OnGeometryChanged(Fixture oldFixture, Fixture newFixture)
        {
            if( oldFixture != null )
            {
                oldFixture.UserData = null;
                oldFixture.OnCollision = null;
                oldFixture.OnSeparation = null;
            }

            if( newFixture != null )
            {
                newFixture.UserData = this;
                newFixture.OnCollision = this.OnCollision;
                newFixture.OnSeparation = this.OnSeperation;
            }

            if( this.FixtureChanged != null )
                this.FixtureChanged(this, new ChangedValue<Fixture>(oldFixture, newFixture));

            this.Refresh();
        }

        /// <summary>
        /// Called when this farseer geometry object is colliding with another.
        /// </summary>
        /// <param name="fixtureA">
        /// The first farseer geometry object.
        /// </param>
        /// <param name="fixtureB">
        /// The second farseer geometry object.
        /// </param>
        /// <param name="contact">
        /// The list of contact points.
        /// </param>
        /// <returns>
        /// true if the collision should be responded to;
        /// otherwise false if the collision should be ignored.
        /// </returns>
        private bool OnCollision( Fixture fixtureA, Fixture fixtureB, Contact contact )
        {
            var fixA = fixtureA.UserData as IRigidFixture;
            var fixB = fixtureB.UserData as IRigidFixture;

            if( this.CollisionFilter != null )
            {
                if( !this.CollisionFilter.ShouldCollide( fixA, fixB, contact ) )
                {
                    return false;
                }
            }

            if( this.Collided != null )
            {
                this.Collided( this, new CollisionEventArgs( fixA, fixB, contact ) );
            }

            return true;
        }
        
        /// <summary>
        /// Called when this farseer Fixture object is seperating from another Fixture.
        /// </summary>
        /// <param name="fixtureA">
        /// The first farseer Fixture object.
        /// </param>
        /// <param name="fixtureB">
        /// The second farseer Fixture object.
        /// </param>
        private void OnSeperation(Fixture fixtureA, Fixture fixtureB)
        {
            if( this.Seperated != null )
            {
                var fixA = fixtureA.UserData as IRigidFixture;
                var fixB = fixtureB.UserData as IRigidFixture;

                this.Seperated(this, new SeperationEventArgs(fixA, fixB));
            }
        }        

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The <see cref="IRigidBody"/> component of the <see cref="Entity"/> that owns this <see cref="Component"/>.
        /// </summary>
        private IRigidBody rigidBody;

        /// <summary>
        /// The Farseer fixture object.
        /// </summary>
        private Fixture fixture;

        #endregion
    }
}