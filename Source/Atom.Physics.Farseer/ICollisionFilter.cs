// <copyright file="ICollisionFilter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Physics.Farseer.ICollisionFilter interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Physics.Farseer
{
    using System.Collections.Generic;
    using FarseerPhysics.Dynamics.Contacts;

    /// <summary>
    /// Provides a mechanism that decides whether two <see cref="IRigidFixture"/> objects should collide.
    /// </summary>
    public interface ICollisionFilter
    {
        /// <summary>
        /// Determines whether collision of two IRigidGeometry instances should be handled;
        /// or ignored.
        /// </summary>
        /// <param name="geometryA">
        /// The first IRigidGeometry that has collided.
        /// </param>
        /// <param name="geometryB">
        /// The seconds IRigidGeometry that has collided.
        /// </param>
        /// <param name="contact">
        /// The contact points of the collision.
        /// </param>
        /// <returns>
        /// true if the collision should be handled;
        /// otherwise false if it should be ignored.
        /// </returns>
        bool ShouldCollide( IRigidFixture geometryA, IRigidFixture geometryB, Contact contact );
    }
}
