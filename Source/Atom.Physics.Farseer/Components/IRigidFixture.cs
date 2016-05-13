// <copyright file="IRigidGeometry.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Physics.Farseer.IRigidGeometry interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Physics.Farseer
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.Components.Collision;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Dynamics;

    /// <summary>
    /// Represents an <see cref="Atom.Components.IComponent"/> that provides an <see cref="IRigidBody"/>
    /// with a geometry shape used for collision.
    /// </summary>
    /// <remarks>
    /// This component depends on the <see cref="IRigidBody"/> component.
    /// </remarks>
    [ContractClass( typeof( IRigidFixtureContracts ) )]
    public interface IRigidFixture : ICollision2
    {
        /// <summary>
        /// Raised when this IRigidGeometry has collided with another IRigidGeometry.
        /// </summary>
        event Atom.RelaxedEventHandler<CollisionEventArgs> Collided;

        /// <summary>
        /// Raised when this IRigidGeometry has seperated from another IRigidGeometry.
        /// </summary>
        event RelaxedEventHandler<SeperationEventArgs> Seperated;

        /// <summary>
        /// Raised when the farseer <see cref="Fixture"/> of this IRigidGeometry component has changed.
        /// </summary>
        event Atom.RelaxedEventHandler<ChangedValue<Fixture>> FixtureChanged;
       
        /// <summary>
        /// Gets or sets the Farseer Fixture object of this RigidGeometry component.
        /// </summary>
        /// <value>
        /// The Fixture objects this IRigidGeometry
        /// encapsulates.
        /// </value>
        Fixture Fixture 
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="IRigidBody"/> this IRigidGeometry depends on.
        /// </summary>
        IRigidBody RigidBody 
        {
            get;
        }

        /// <summary>
        /// Gets or sets an optional <see cref="ICollisionFilter"/>; that provides a mechanism
        /// for deciding whether this IRigidGeometry should collide with another IRigidGeometry.
        /// </summary>
        ICollisionFilter CollisionFilter 
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
        float Friction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the restitution coefficient of this IRigidGeometry.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the Farseer <see cref="Fixture"/> is null.
        /// </exception>
        /// <value>The <see cref="FarseerPhysics.Collision.Geom.RestitutionCoefficient"/> property.</value>
        float Restitution
        { 
            get;
            set; 
        }
    }
}
