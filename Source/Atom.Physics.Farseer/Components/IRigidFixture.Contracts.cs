// <copyright file="IRigidFixture.Contracts.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Physics.Farseer.IRigidFixtureContracts class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Physics.Farseer
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.Components;
    using Atom.Math;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Dynamics;

    /// <summary>
    /// Defines the code contracts for the <see cref="IRigidFixture"/> interface.
    /// </summary>
    [ContractClassFor( typeof( IRigidFixture ) )]
    internal abstract class IRigidFixtureContracts : IRigidFixture
    {
        #region IRigidGeometry Members
        
        public Fixture Fixture
        {
            get
            {
                return default(Fixture);
            }

            set
            {
                Contract.Ensures(this.Fixture == value);
            }
        }

        public IRigidBody RigidBody
        {
            get
            {
                return default( IRigidBody ); 
            }
        }

        public ICollisionFilter CollisionFilter
        {
            get
            {
                return default( ICollisionFilter ); 
            }

            set
            {
                Contract.Ensures( this.CollisionFilter == value );
            }
        }

        public float Friction
        {
            get
            {
                Contract.Requires<InvalidOperationException>( this.Fixture != null );
                return default( float ); 
            }

            set
            {
                Contract.Requires<InvalidOperationException>( this.Fixture != null );
            }
        }

        public float Restitution
        {
            get
            {
                Contract.Requires<InvalidOperationException>( this.Fixture != null );
                return default( float );
            }

            set
            {
                Contract.Requires<InvalidOperationException>( this.Fixture != null );
            }
        }

        public  event RelaxedEventHandler<CollisionEventArgs> Collided
        {
            add { }
            remove { }
        }

        public event RelaxedEventHandler<ChangedValue<Fixture>> FixtureChanged
        {
            add { }
            remove { }
        }

        public  event RelaxedEventHandler<SeperationEventArgs> Seperated
        {
            add { }
            remove { }
        }

        #endregion

        #region ICollision2 Members

        event SimpleEventHandler<Atom.Components.Collision.ICollision2> Atom.Components.Collision.ICollision2.Changed
        {
            add { }
            remove {  }
        }

        RectangleF Atom.Components.Collision.ICollision2.Rectangle
        {
            get { return default( RectangleF ); }
        }

        Circle Atom.Components.Collision.ICollision2.Circle
        {
            get { return default( Circle ); }
        }

        Atom.Components.Transform.ITransform2 Atom.Components.Collision.ICollision2.Transform
        {
            get { return null; }
        }

        #endregion

        #region IComponent Members

        IEntity IComponent.Owner
        {
            get
            {
                return null;
            }

            set
            {

            }
        }

        bool IComponent.IsEnabled
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        void IComponent.Initialize()
        {
            throw new NotImplementedException();
        }

        void IComponent.InitializeBindings()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUpdateable Members

        void IUpdateable.Update( IUpdateContext updateContext )
        {
        }

        #endregion

        #region IPreUpdateable Members

        void IPreUpdateable.PreUpdate( IUpdateContext updateContext )
        {
        }

        #endregion
    }
}
