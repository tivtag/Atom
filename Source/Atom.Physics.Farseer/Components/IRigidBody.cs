// <copyright file="IRigidBody.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Physics.Farseer.IRigidBody interface.
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
    /// Represents an <see cref="IComponent"/> that encapsulates the
    /// functionality of a <see cref="FarseerPhysics.Dynamics.Body"/>.
    /// </summary>
    /// <remarks>
    /// This component depends on the <see cref="Atom.Components.Transform.ITransform2"/> component.
    /// </remarks>
    [ContractClass( typeof( IRigidBodyContracts ) )]
    public interface IRigidBody : IComponent
    {
        /// <summary>
        /// Raised when the Farseer <see cref="Body"/> of this <see cref="IRigidBody"/>
        /// component has changed.
        /// </summary>
        event Atom.RelaxedEventHandler<ChangedValue<Body>> BodyChanged;

        /// <summary>
        /// Gets or sets the Farseer Body object of this <see cref="IRigidBody"/> component.
        /// </summary>
        /// <value>
        /// The <see cref="FarseerPhysics.Dynamics.Body"/> objects this RigidBody
        /// encapsulates.
        /// </value>
        Body Body
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IRigidBody"/>
        /// ignore gravity forces.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the Farseer <see cref="Body"/> is null.
        /// </exception>
        /// <value>
        /// If <see langword="true"/> the rigid body doesn't react to gravity forces;
        /// or otherwise <see langword="false"/>.
        /// </value>
        bool IgnoresGravity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IRigidBody"/> is static,
        /// and as such doesn't react to physics input.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the Farseer <see cref="Body"/> is null.
        /// </exception>
        /// <value>
        /// If <see langword="true"/> the rigid body doesn't react to physics input;
        /// or otherwise <see langword="false"/>.
        /// </value>
        bool IsStatic
        {
            get;
            set;
        }

        ///// <summary>
        ///// Gets or sets the linear drag coefficient of this <see cref="IRigidBody"/>.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">
        ///// If the Farseer <see cref="Body"/> is null.
        ///// </exception>
        ///// <value>
        ///// The value of the <see cref="FarseerPhysics.Dynamics.Body.LinearDragCoefficient"/> property.
        ///// </value>
        //float LinearDragCoefficient
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Gets or sets the mass of this <see cref="IRigidBody"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the Farseer <see cref="Body"/> is null.
        /// </exception>
        /// <value>
        /// The value of the <see cref="FarseerPhysics.Dynamics.Body.Mass"/> property.
        /// </value>
        float Mass
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the moment of inertia of this <see cref="IRigidBody"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the Farseer <see cref="Body"/> is null.
        /// </exception>
        /// <value>
        /// The value of the <see cref="FarseerPhysics.Dynamics.Body.MomentOfInertia"/> property.
        /// </value>
        float Inertia
        {
            get;
            set;
        }

        ///// <summary>
        ///// Gets or sets the rotational drag coefficient of this <see cref="IRigidBody"/>.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">
        ///// If the Farseer <see cref="Body"/> is null.
        ///// </exception>
        ///// <value>
        ///// The value of the <see cref="FarseerPhysics.Dynamics.Body.RotationalDragCoefficient"/> property.
        ///// </value>
        //float RotationalDragCoefficient
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Applies the specified <paramref name="force"/> to this <see cref="IRigidBody"/>.
        /// </summary>
        /// <param name="force">
        /// The force to apply.
        /// </param>
        void ApplyForce( Vector2 force );

        /// <summary>
        /// Applies the specified <paramref name="torque"/> to this <see cref="IRigidBody"/>.
        /// </summary>
        /// <param name="torque">
        /// The torque to apply.
        /// </param>
        void ApplyTorque( float torque );

        /// <summary>
        /// Applies an angular impulse to this <see cref="IRigidBody"/>.
        /// </summary>
        /// <param name="amount">
        /// The amount of impulse to apply.
        /// </param>
        void ApplyAngularImpulse( float amount );
    }
}
