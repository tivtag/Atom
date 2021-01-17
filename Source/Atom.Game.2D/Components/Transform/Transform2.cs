// <copyright file="Transform2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Components.Transform.Transform2 class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Components.Transform
{
    using System;
    using Atom.Math;

    /// <summary>
    /// Defines the transformation state of an <see cref="IEntity"/> in two dimensional space.
    /// This includes <see cref="Position"/>, <see cref="Scale"/>, <see cref="Rotation"/> and <see cref="Origin"/>,
    /// </summary>
    public class Transform2 : Component, ITransform2
    {
        #region [ Events ]

        /// <summary>
        /// Fired when the transform this <see cref="Transform2"/> component represents has changed.
        /// </summary>
        public event SimpleEventHandler<ITransform2> Changed;

        /// <summary>
        /// Fired when the <see cref="Position"/> of this <see cref="Transform2"/> has changed.
        /// </summary>
        public event RelaxedEventHandler<ChangedValue<Vector2>> PositionChanged;

        /// <summary>
        /// Fired when the <see cref="Scale"/> of this <see cref="Transform2"/> has changed.
        /// </summary>
        public event RelaxedEventHandler<ChangedValue<Vector2>> ScaleChanged;

        /// <summary>
        /// Fired when the <see cref="Rotation"/> of this <see cref="Transform2"/> has changed.
        /// </summary>
        public event RelaxedEventHandler<ChangedValue<float>> RotationChanged;

        /// <summary>
        /// Fired when the <see cref="Origin"/> of this <see cref="Transform2"/> has changed.
        /// </summary>
        public event RelaxedEventHandler<ChangedValue<Vector2>> OriginChanged;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the position of this <see cref="Transform2"/> component on the x-axis.
        /// </summary>
        /// <value>The position on the x-axis of the transformable Entity.</value>
        public float X
        {
            get
            {
                return this.position.X;
            }

            set
            {
                this.Position = new Vector2( value, this.position.Y );
            }
        }

        /// <summary>
        /// Gets or sets the position of this <see cref="Transform2"/> component on the y-axis.
        /// </summary>
        /// <value>The position on the y-axis of the transformable Entity.</value>
        public float Y
        {
            get
            {
                return this.position.Y;
            }

            set
            {
                this.Position = new Vector2( this.position.X, value );
            }
        }

        /// <summary>
        /// Gets or sets the position of this <see cref="Transform2"/> component.
        /// </summary>
        /// <value>The position of the transformable Entity.</value>
        public virtual Vector2 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                if( value.X == this.position.X && value.Y == this.position.Y )
                    return;

                Vector2 oldValue = this.position;
                this.position    = value;

                this.OnPositionChanged( oldValue, value );
            }
        }

        /// <summary>
        /// Gets or sets the scale of this <see cref="Transform2"/> component.
        /// </summary>
        /// <value>The scale of the transformable Entity.</value>
        public virtual Vector2 Scale
        {
            get
            {
                return this.scale;
            }

            set
            {
                if( value.X == this.scale.X && value.Y == this.scale.Y )
                    return;

                Vector2 oldValue = this.scale;
                this.scale = value;

                this.OnScaleChanged( oldValue, value );
            }
        }

        /// <summary>
        /// Gets or sets the rotation in radians of this <see cref="Transform2"/> component.
        /// </summary>
        /// <value>The rotation value of the transformable Entity.</value>
        public virtual float Rotation
        {
            get
            {
                return this.rotation;
            }

            set
            {
                if( value == this.rotation )
                    return;

                float oldValue = this.rotation;
                this.rotation = value;

                this.OnRotationChanged( oldValue, value );
            }
        }

        /// <summary>
        /// Gets or sets the origin of this <see cref="Transform2"/> component.
        /// </summary>
        /// <value>The scale and rotation origin of the transformable Entity.</value>
        public virtual Vector2 Origin
        {
            get
            {
                return this.origin;
            }

            set
            {
                if( value.X == this.origin.X && value.Y == this.origin.Y )
                    return;

                Vector2 oldValue = this.origin;
                this.origin = value;

                this.OnOriginChanged( oldValue, value );
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Called when the <see cref="Position"/> of this <see cref="Transform2"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnPositionChanged( Vector2 oldValue, Vector2 newValue )
        {
            if( this.PositionChanged != null )
                this.PositionChanged( this, new ChangedValue<Vector2>( oldValue, newValue ) );
            
            this.OnTransformChanged();
        }

        /// <summary>
        /// Called when the <see cref="Scale"/> of this <see cref="Transform2"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnScaleChanged( Vector2 oldValue, Vector2 newValue )
        {
            if( this.ScaleChanged != null )
                this.ScaleChanged( this, new ChangedValue<Vector2>( oldValue, newValue ) );

            this.OnTransformChanged();
        }

        /// <summary>
        /// Called when the <see cref="Rotation"/> of this <see cref="Transform2"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnRotationChanged( float oldValue, float newValue )
        {
            if( this.RotationChanged != null )
                this.RotationChanged( this, new ChangedValue<float>( oldValue, newValue ) );

            this.OnTransformChanged();
        }

        /// <summary>
        /// Called when the <see cref="Origin"/> of this <see cref="Transform2"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnOriginChanged( Vector2 oldValue, Vector2 newValue )
        {
            if( this.OriginChanged != null )
                this.OriginChanged( this, new ChangedValue<Vector2>( oldValue, newValue ) );

            this.OnTransformChanged();
        }
        
        /// <summary>
        /// Called when any of transformation properties of this <see cref="Transform2"/> has changed.
        /// </summary>
        protected virtual void OnTransformChanged()
        {
            if( this.Changed != null )
            {
                this.Changed( this );
            }
        }

        /// <summary>
        /// Returns a humen-readable string representation of this <see cref="Transform2"/>.
        /// </summary>
        /// <returns>
        /// Returns a humen-readable description of this <see cref="Transform2"/>.
        /// </returns>
        public override string ToString()
        {
            IFormatProvider formatProvider = System.Globalization.CultureInfo.CurrentCulture;

            return string.Format(
                formatProvider,
                "{0} [Owner='{1}' Position='{2}' Scale='{3}' Rotation='{4}']",
                this.GetType().Name,
                this.Owner,
                this.Position.ToString( formatProvider ),
                this.Scale.ToString( formatProvider ),
                this.Rotation.ToString( formatProvider )
            );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Stores the position of this <see cref="Transform2"/> component.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Stores the scale of this <see cref="Transform2"/> component.
        /// </summary>
        private Vector2 scale = Vector2.One;

        /// <summary>
        /// Stores the rotation in radians of this <see cref="Transform2"/> component.
        /// </summary>
        private float rotation;

        /// <summary>
        /// Stores the orgin of this <see cref="Transform2"/> component.
        /// </summary>
        private Vector2 origin;

        #endregion
    }
}
