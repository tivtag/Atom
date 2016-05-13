// <copyright file="PhysicsSimulator.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Physics.Farseer.PhysicsSimulator class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Physics.Farseer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Atom.Math;
    using Atom.Xna;
    using Far = FarseerPhysics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Runs the physics simulation, driven by Farseer.
    /// </summary>
    public class PhysicsSimulator : IUpdateable
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the gravity which is applied to all objects of the <see cref="PhysicsSimulator"/>.
        /// </summary>
        /// <value>
        /// The gravity vector which contains the strength and direction 
        /// of the global gravity effect.
        /// </value>
        public Vector2 Gravity
        {
            get
            {
                var gravity = world.Gravity;
                return new Vector2( gravity.X, gravity.Y );
            }

            set
            {
                world.Gravity = new Xna.Vector2( value.X, value.Y );
            }
        }

        /// <summary>
        /// Provides access to timing values that descripe how long each step
        /// of the Physics Simulation took to execute.
        /// </summary>
        public PhysicsSimulatorTimingProperties Timing
        {
            get
            {
                Contract.Ensures( Contract.Result<PhysicsSimulatorTimingProperties>() != null );

                return this.timing;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicsSimulator"/> class.
        /// </summary>
        public PhysicsSimulator()
        {
            this.timing = new PhysicsSimulatorTimingProperties( this.world );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Updates this PhysicsSimulator.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public virtual void Update( IUpdateContext updateContext )
        {
            this.world.Step( updateContext.FrameTime );
        }

        /// <summary>
        /// Finds the first IRigidGeometry that is at the given point.
        /// </summary>
        /// <param name="point">
        /// The point at which for IRigidGeometries should be looked for.
        /// </param>
        /// <returns>
        /// The IRigidGeometry that has been found first;
        /// or null if there is no IRigidGeometry at the given point.
        /// </returns>
        public IRigidFixture FindAt( Vector2 point )
        {
            var geom = this.world.TestPoint( point.ToXna() );
            
            if( geom != null )
            {
                return (IRigidFixture)geom.UserData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all IRigidGeometries that is at the given point.
        /// </summary>
        /// <param name="point">
        /// The point at which for IRigidGeometries should be looked for.
        /// </param>
        /// <returns>
        /// The IRigidGeometries that have been found;
        /// or null if there is no IRigidGeometry at the given point.
        /// </returns>
        public IEnumerable<IRigidFixture> FindAllAt( Vector2 point )
        {
            var geoms = this.world.TestPointAll( point.ToXna() );
            var list = new List<IRigidFixture>( geoms.Count );

            foreach( var geom in geoms )
            {
                list.Add( (IRigidFixture)geom.UserData );
            }

            return list;
        }

        #region > Organisation <
        
        /// <summary>
        /// Adds the specified <see cref="IRigidBody"/> to this <see cref="PhysicsSimulator"/>.
        /// </summary>
        /// <param name="body">
        /// The body object to add.
        /// </param>
        public void Add( IRigidBody body )
        {
            Contract.Requires<ArgumentNullException>( body != null );

            this.world.AddBody( body.Body );
        }
        /// <summary>
        /// Removes the specified <see cref="IRigidBody"/> from this <see cref="PhysicsSimulator"/>.
        /// </summary>
        /// <param name="body">
        /// The body object to remove.
        /// </param>
        public void Remove( IRigidBody body )
        {
            Contract.Requires<ArgumentNullException>( body != null );

            this.world.RemoveBody( body.Body );
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// 
        /// </summary>
        private readonly Far.Dynamics.World world = new Far.Dynamics.World( Microsoft.Xna.Framework.Vector2.Zero );

        /// <summary>
        /// Provides access to the timing properties of the simulator this PhysicsSimulator
        /// wraps around.
        /// </summary>
        private readonly PhysicsSimulatorTimingProperties timing;

        #endregion
    }
}
