// <copyright file="RigidBody.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Physics.Farseer.RigidBody class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Physics.Farseer
{
    using System;
    using Atom.Components;
    using Atom.Components.Transform;
    using Atom.Math;
    using Atom.Xna;
    using FarD = FarseerPhysics.Dynamics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a <see cref="Component"/> that encapsulates
    /// the functionality of a <see cref="FarseerPhysics.Dynamics.Body"/>.
    /// </summary>
    /// <remarks>
    /// This component depends on the <see cref="ITransform2"/> component.
    /// </remarks>
    public class RigidBody : Component, IRigidBody
    {
        #region [ Events ]

        /// <summary>
        /// Raised when the Farseer <see cref="Body"/> of this <see cref="RigidBody"/>
        /// component has changed.
        /// </summary>
        public event RelaxedEventHandler<ChangedValue<FarD.Body>> BodyChanged;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the Farseer Body object of this <see cref="RigidBody"/> component.
        /// </summary>
        /// <value>
        /// The <see cref="FarseerPhysics.Dynamics.Body"/> objects this RigidBody
        /// encapsulates.
        /// </value>
        public FarD.Body Body
        {
            get
            {
                return this.body;
            }

            set
            {
                if( this.body == value )
                    return;

                var oldValue = this.body;
                this.body = value;

                this.OnBodyChanged( oldValue, value );
            }
        }

        ///// <summary>
        ///// Gets or sets the linear drag coefficient of this <see cref="RigidBody"/>.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">
        ///// If the Farseer <see cref="Body"/> is null.
        ///// </exception>
        ///// <value>
        ///// The value of the <see cref="FarseerPhysics.Dynamics.Body.LinearDragCoefficient"/> property.
        ///// </value>
        //public float LinearDragCoefficient
        //{
        //    get
        //    {
        //        return this.body.LinearDragCoefficient;
        //    }

        //    set
        //    {
        //        this.body.LinearDragCoefficient = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the rotational drag coefficient of this <see cref="RigidBody"/>.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">
        ///// If the Farseer <see cref="Body"/> is null.
        ///// </exception>
        ///// <value>
        ///// The value of the <see cref="FarseerPhysics.Dynamics.Body.RotationalDragCoefficient"/> property.
        ///// </value>
        //public float RotationalDragCoefficient
        //{
        //    get
        //    {
        //        return this.body.RotationalDragCoefficient;
        //    }

        //    set
        //    {
        //        this.body. = value;
        //    }
        //}

        /// <summary>
        /// Gets or sets the mass of this <see cref="RigidBody"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the Farseer <see cref="Body"/> is null.
        /// </exception>
        /// <value>
        /// The value of the <see cref="FarseerPhysics.Dynamics.Body.Mass"/> property.
        /// </value>
        public float Mass
        {
            get
            {
                return this.body.Mass;
            }

            set
            {
                this.body.Mass = value;
            }
        }

        /// <summary>
        /// Gets or sets the moment of inertia of this <see cref="RigidBody"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the Farseer <see cref="Body"/> is null.
        /// </exception>
        /// <value>
        /// The value of the <see cref="FarseerPhysics.Dynamics.Body.MomentOfInertia"/> property.
        /// </value>
        public float Inertia
        {
            get
            {
                return this.body.Inertia;
            }

            set
            {
                this.body.Inertia = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RigidBody"/> is static,
        /// and as such doesn't react to physics input.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the Farseer <see cref="Body"/> is null.
        /// </exception>
        /// <value>
        /// If <see langword="true"/> the rigid body doesn't react to physics input;
        /// or otherwise <see langword="false"/>.
        /// </value>
        public bool IsStatic
        {
            get
            {
                return this.body.IsStatic;
            }

            set
            {
                this.body.IsStatic = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RigidBody"/>
        /// ignore gravity forces.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the Farseer <see cref="Body"/> is null.
        /// </exception>
        /// <value>
        /// If <see langword="true"/> the rigid body doesn't react to gravity forces;
        /// or otherwise <see langword="false"/>.
        /// </value>
        public bool IgnoresGravity
        {
            get
            {
                return this.body.IgnoreGravity;
            }

            set
            {
                this.body.IgnoreGravity = value;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="RigidBody"/> class.
        /// </summary>
        public RigidBody()
        {
        }

        /// <summary>
        /// Called when an IComponent has been removed or added
        /// to the <see cref="IEntity"/> that owns this Collision2 component.
        /// </summary>
        public override void InitializeBindings()
        {
            var acquiredTransform = this.Owner.Components.Find<ITransform2>();

            if( acquiredTransform != this.transform )
            {
                if( this.transform != null )
                    this.Unhook( this.transform );

                this.transform = acquiredTransform;

                if( this.transform != null )
                    this.Hook( this.transform );

                this.ApplyTransformToBody();
            }
        }

        /// <summary>
        /// Override to hook any events onto the given <see cref="ITransform2"/> component.
        /// </summary>
        /// <param name="transform">
        /// The related ITransform2 component. Is never null.
        /// </param>
        private void Hook( ITransform2 transform )
        {
            transform.PositionChanged += this.OnTransformPositionChanged;
            transform.RotationChanged += this.OnTransformRotationChanged;
        }

        /// <summary>
        /// Override to unhook any events from the given <see cref="ITransform2"/> component.
        /// </summary>
        /// <remarks>
        /// Unhook should remove all events that were added in <see cref="Hook"/>.
        /// </remarks>
        /// <param name="transform">
        /// The related ITransform2 component. Is never null.
        /// </param>
        private void Unhook( ITransform2 transform )
        {
            transform.PositionChanged -= this.OnTransformPositionChanged;
            transform.RotationChanged -= this.OnTransformRotationChanged;
        }

        /// <summary>
        /// Applies the current state of the ITransform to the Farseer body.
        /// </summary>
        private void ApplyTransformToBody()
        {
            if( this.body == null )
                return;

            if( this.transform != null )
            {
                this.body.Position = new Xna.Vector2( this.transform.X, this.transform.Y );
                this.body.Rotation = this.transform.Rotation;
            }
            else
            {
                this.body.Position = Xna.Vector2.Zero;
                this.body.Rotation = 0.0f;
            }
        }

        #endregion

        #region [ Methods ]

        #region > Apply <

        /// <summary>
        /// Applies the specified <paramref name="force"/> to this <see cref="RigidBody"/>.
        /// </summary>
        /// <param name="force">
        /// The force to apply.
        /// </param>
        public void ApplyForce( Vector2 force )
        {
            this.body.ApplyForce( force.ToXna() );
        }

        ///// <summary>
        ///// Applies the specified <paramref name="force"/> to this <see cref="RigidBody"/>.
        ///// </summary>
        ///// <param name="force">
        ///// The force to apply.
        ///// </param>
        ///// <param name="point">
        ///// The point at which the force should be applied.
        ///// </param>
        //public void ApplyForceAtLocalPoint( Vector2 force, Vector2 point )
        //{
        //    this.body.ApplyForceAtLocalPoint(
        //        new FarM.Vector2( force.X, force.Y ),
        //        new FarM.Vector2( point.X, point.Y )
        //    );
        //}

        ///// <summary>
        ///// Applies the specified <paramref name="force"/> to this <see cref="RigidBody"/>.
        ///// </summary>
        ///// <param name="force">
        ///// The force to apply.
        ///// </param>
        ///// <param name="point">
        ///// The point.
        ///// </param>
        //public void ApplyForceAtWorldPoint( Vector2 force, Vector2 point )
        //{
        //    this.body.ApplyForceAtWorldPoint(
        //        new FarM.Vector2( force.X, force.Y ),
        //        new FarM.Vector2( point.X, point.Y )
        //    );
        //}

        /// <summary>
        /// Applies the specified <paramref name="torque"/> to this <see cref="RigidBody"/>.
        /// </summary>
        /// <param name="torque">
        /// The torque to apply.
        /// </param>
        public void ApplyTorque( float torque )
        {
            this.body.ApplyTorque( torque );
        }

        /// <summary>
        /// Applies an angular impulse to this <see cref="RigidBody"/>.
        /// </summary>
        /// <param name="amount">
        /// The amount of impulse to apply.
        /// </param>
        public void ApplyAngularImpulse( float amount )
        {
            this.body.ApplyAngularImpulse( amount );
        }

        /// <summary>
        /// Applies an impulse to this <see cref="RigidBody"/>.
        /// </summary>
        /// <param name="amount">
        /// The amount of impulse to apply.
        /// </param>
        public void ApplyLinearImpulse( Vector2 amount, Vector2 point )
        {
            this.body.ApplyLinearImpulse( amount.ToXna(), point.ToXna() );
        }

        #endregion

        #region > Events <

        /// <summary>
        /// Called when the Farseer body's state has been updated.
        /// </summary>
        /// <param name="position">
        /// The new position of the farseer Body.
        /// </param>
        /// <param name="rotation">
        /// The new rotation of the farseer Body.
        /// </param>
        private void OnBodyUpdated(Xna.Vector2 position, float rotation)
        {
            this.transform.Position = new Vector2(position.X, position.Y);
            this.transform.Rotation = rotation;
        }

        /// <summary>
        /// Called when the <see cref="Body"/> of this <see cref="RigidBody"/> 
        /// component has changed.
        /// </summary>
        /// <param name="oldBody">The old body value.</param>
        /// <param name="newBody">The new body value.</param>
        protected virtual void OnBodyChanged(FarD.Body oldBody, FarD.Body newBody)
        {
            if (oldBody != null)
                oldBody.Updated -= this.OnBodyUpdated;

            if (newBody != null)
                newBody.Updated += this.OnBodyUpdated;

            if (this.BodyChanged != null)
                this.BodyChanged(this, new ChangedValue<FarD.Body>(oldBody, newBody));
        }

        /// <summary>
        /// Called when the <see cref="Transform2.Position"/> value of the <see cref="Entity"/> has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="changedValue">
        /// The ChangedValue{Vector2} that contains information about the property that has changed.
        /// </param>
        private void OnTransformPositionChanged( object sender, ChangedValue<Vector2> changedValue )
        {
            if( this.body != null )
            {
                var newValue = changedValue.NewValue.ToXna();

                if( this.body.Position != newValue )
                {
                    this.body.Position = newValue;
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="Transform2.Rotation"/> value of the <see cref="Entity"/> has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="changedValue">
        /// The ChangedValue{Single} that contains information about the property that has changed.
        /// </param>
        private void OnTransformRotationChanged( object sender, ChangedValue<float> changedValue )
        {
            if( this.body != null )
            {
                if( this.body.Rotation != changedValue.NewValue )
                {
                    this.body.Rotation = changedValue.NewValue;
                }
            }
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The transform of the Entity that owns this <see cref="RigidBody"/> component.
        /// </summary>
        private ITransform2 transform;

        /// <summary>
        /// The farseer rigid body object.
        /// </summary>
        private FarD.Body body;

        #endregion
    }
}