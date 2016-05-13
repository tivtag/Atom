// <copyright file="CollisionEventArgs.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Physics.Farseer.CollisionEventArgs structure.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Physics.Farseer
{
    using System.Collections.Generic;
    using FarseerPhysics.Dynamics.Contacts;

    /// <summary>
    /// Defines the event arguments that contain information about the collision
    /// event of two <see cref="IRigidFixture"/> objects.
    /// </summary>
    public struct CollisionEventArgs
    {
        /// <summary>
        /// Gets the first <see cref="IRigidFixture"/> that was part of the collision event.
        /// </summary>
        public IRigidFixture GeometryA
        {
            get
            {
                return this.geometryA;
            }
        }

        /// <summary>
        /// Gets the second <see cref="IRigidFixture"/> that was part of the collision event.
        /// </summary>
        public IRigidFixture GeometryB
        {
            get
            {
                return this.geometryB;
            }
        }

        /// <summary>
        /// Gets the list of points at which the <see cref="IRigidFixture"/> objects collided.
        /// </summary>
        public Contact Contact
        {
            get
            {
                return this.contact;
            }
        }

        /// <summary>
        /// Initializes a new instance of the CollisionEventArgs structure.
        /// </summary>
        /// <param name="fixtureA">
        /// The first <see cref="IRigidFixture"/> that was part of the collision event.
        /// </param>
        /// <param name="fixtureB">
        /// The second <see cref="IRigidFixture"/> that was part of the collision event.
        /// </param>
        /// <param name="contacts">
        /// The list of points at which the <see cref="IRigidFixture"/> objects collided.
        /// </param>
        public CollisionEventArgs( IRigidFixture fixtureA, IRigidFixture fixtureB, Contact contacts )
        {
            this.geometryA = fixtureA;
            this.geometryB = fixtureB;
            this.contact = contacts;
        }

        /// <summary>
        /// The first <see cref="IRigidFixture"/> that was part of the collision event.
        /// </summary>
        private readonly IRigidFixture geometryA;

        /// <summary>
        /// The second <see cref="IRigidFixture"/> that was part of the collision event.
        /// </summary>
        private readonly IRigidFixture geometryB;

        /// <summary>
        /// The list of points at which the <see cref="IRigidFixture"/> objects collided.
        /// </summary>
        private readonly Contact contact;
    }
}
