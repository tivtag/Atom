// <copyright file="SeperationEventArgs.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Physics.Farseer.SeperationEventArgs structure.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Physics.Farseer
{
    using FarseerPhysics.Collision;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the event arguments that contain information about the seperation
    /// event of two <see cref="IRigidFixture"/> objects.
    /// </summary>
    public struct SeperationEventArgs
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
        /// Initializes a new instance of the SeperationEventArgs structure.
        /// </summary>
        /// <param name="geometryA">
        /// The first <see cref="IRigidFixture"/> that was part of the seperation event.
        /// </param>
        /// <param name="geometryB">
        /// The second <see cref="IRigidFixture"/> that was part of the seperation event.
        /// </param>
        public SeperationEventArgs( IRigidFixture geometryA, IRigidFixture geometryB )
        {
            this.geometryA = geometryA;
            this.geometryB = geometryB;
        }

        /// <summary>
        /// The first <see cref="IRigidFixture"/> that was part of the seperation event.
        /// </summary>
        private readonly IRigidFixture geometryA;

        /// <summary>
        /// The second <see cref="IRigidFixture"/> that was part of the seperation event.
        /// </summary>
        private readonly IRigidFixture geometryB;
    }
}
