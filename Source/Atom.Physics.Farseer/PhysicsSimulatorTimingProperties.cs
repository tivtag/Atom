// <copyright file="PhysicsSimulatorTimingProperties.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Physics.Farseer.PhysicsSimulatorTimingProperties class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Physics.Farseer
{
    using System;
    using System.Diagnostics.Contracts;
    
    /// <summary>
    /// Provides access timing values of the various indivual steps that are
    /// taken during the Physics Simulation.
    /// </summary>
    public sealed class PhysicsSimulatorTimingProperties
    {
        /// <summary>
        /// Gets the time it took the PhysicsSimulator to add/remove objects.
        /// </summary>
        public float AddRemove
        {
            get
            {
                return this.world.AddRemoveTime;
            }
        }

        /// <summary>
        /// Gets the time it took the PhysicsSimulator to update contact data.
        /// </summary>
        public float ContactsUpdate
        {
            get
            {
                return this.world.ContactsUpdateTime;
            }
        }

        /// <summary>
        /// Gets the time it took the PhysicsSimulator to apply continuous physics simulation.
        /// </summary>
        public float ContinuousPhysics
        {
            get
            {
                return this.world.ContinuousPhysicsTime;
            }
        }

        /// <summary>
        /// Gets the time it took the PhysicsSimulator to update the controller.
        /// </summary>
        public float ControllersUpdate
        {
            get
            {
                return this.world.ControllersUpdateTime;
            }
        }

        /// <summary>
        /// Gets the time it took the PhysicsSimulator to solve the update step.
        /// </summary>
        public float SolveUpdate
        {
            get
            {
                return this.world.SolveUpdateTime;
            }
        }

        /// <summary>
        /// Gets the time it took the PhysicsSimulator to fully
        /// update.
        /// </summary>
        public float Update
        {
            get
            {
                return this.world.UpdateTime;
            }
        }

        /// <summary>
        /// Initializes a new instance of the PhysicsSimulatorTimingProperties class.
        /// </summary>
        /// <param name="world">
        /// The Farseer PhysicsSimulator object whose timing properties are
        /// exposed by the new PhysicsSimulatorTimingProperties object.
        /// </param>
        internal PhysicsSimulatorTimingProperties( FarseerPhysics.Dynamics.World world )
        {
            Contract.Requires<ArgumentNullException>( world != null );

            this.world = null;
        }

        /// <summary>
        /// The Farseer PhysicsSimulator object whose timing properties are
        /// exposed by this PhysicsSimulatorTimingProperties object.
        /// </summary>
        private readonly FarseerPhysics.Dynamics.World world;
    }
}
