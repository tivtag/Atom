// <copyright file="IRigidBody.Contracts.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Physics.Farseer.IRigidBodyContracts class.
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
    using FarseerPhysics.Dynamics;

    /// <summary>
    /// Defines the code contracts for the <see cref="IRigidBody"/> interface.
    /// </summary>
    [ContractClassFor( typeof( IRigidBody ) )]
    internal abstract class IRigidBodyContracts : IRigidBody
    {
        #region IRigidBody Members

        event RelaxedEventHandler<ChangedValue<Body>> IRigidBody.BodyChanged
        {
            add { }
            remove { }
        }

        Body IRigidBody.Body
        {
            get
            {
                return default( Body );
            }

            set
            {
                var @this = (IRigidBody)this;
                Contract.Ensures( @this.Body == value );
            }
        }

        bool IRigidBody.IgnoresGravity
        {
            get
            {
                var @this = (IRigidBody)this;
                Contract.Requires<InvalidOperationException>( @this.Body != null );
                return default( bool );
            }

            set
            {
                var @this = (IRigidBody)this; 
                Contract.Requires<InvalidOperationException>( @this.Body != null );
            }
        }

        bool IRigidBody.IsStatic
        {
            get
            {
                var @this = (IRigidBody)this; 
                Contract.Requires<InvalidOperationException>( @this.Body != null );
                return default( bool );
            }

            set
            {
                var @this = (IRigidBody)this; 
                Contract.Requires<InvalidOperationException>( @this.Body != null );
            }
        }

        float IRigidBody.Mass
        {
            get
            {
                var @this = (IRigidBody)this; 
                Contract.Requires<InvalidOperationException>( @this.Body != null );
                return default( float );
            }

            set
            {
                var @this = (IRigidBody)this;
                Contract.Requires<InvalidOperationException>( @this.Body != null );
            }
        }

        float IRigidBody.Inertia
        {
            get
            {
                var @this = (IRigidBody)this; 
                Contract.Requires<InvalidOperationException>( @this.Body != null );
                return default( float );
            }

            set
            {
                var @this = (IRigidBody)this;
                Contract.Requires<InvalidOperationException>( @this.Body != null );
            }
        }            

        void IRigidBody.ApplyForce( Vector2 force )
        {
            var @this = (IRigidBody)this; 
            Contract.Requires<InvalidOperationException>( @this.Body != null );
        }
        void IRigidBody.ApplyTorque( float torque )
        {
            var @this = (IRigidBody)this; 
            Contract.Requires<InvalidOperationException>( @this.Body != null );
        }

        void IRigidBody.ApplyAngularImpulse( float amount )
        {
            var @this = (IRigidBody)this;
            Contract.Requires<InvalidOperationException>( @this.Body != null );
        }

        #endregion

        #region IComponent Members

        IEntity IComponent.Owner
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
            throw new NotImplementedException();
        }

        #endregion

        #region IPreUpdateable Members

        void IPreUpdateable.PreUpdate( IUpdateContext updateContext )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
