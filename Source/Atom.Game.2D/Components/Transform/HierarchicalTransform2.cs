// <copyright file="HierarchicalTransform2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Components.Transform.HierarchicalTransform2 interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Components.Transform
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Atom.Math;

    /// <summary>
    /// Defines a <see cref="Transform2"/> that is hirachically organized.
    /// Child transforms inherit the transformation of their parents.
    /// </summary>
    public class HierarchicalTransform2 : Transform2
    {
        #region [ Events ]

        /// <summary>
        /// Fired when the <see cref="RelativePosition"/> of this <see cref="HierarchicalTransform2"/> has changed.
        /// </summary>
        public event RelaxedEventHandler<ChangedValue<Vector2>> RelativePositionChanged;

        /// <summary>
        /// Fired when the <see cref="RelativeScale"/> of this <see cref="HierarchicalTransform2"/> has changed.
        /// </summary>
        public event RelaxedEventHandler<ChangedValue<Vector2>> RelativeScaleChanged;

        /// <summary>
        /// Fired when the <see cref="RelativeRotation"/> of this <see cref="HierarchicalTransform2"/> has changed.
        /// </summary>
        public event RelaxedEventHandler<ChangedValue<float>> RelativeRotationChanged;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the parent <see cref="HierarchicalTransform2"/>
        /// this <see cref="HierarchicalTransform2"/> inherits
        /// its properties from. Can be null.
        /// </summary>
        /// <value>
        /// The <see cref="HierarchicalTransform2"/> component that is the parent of this HierarchicalTransform2.
        /// The default value is null.
        /// </value>
        public HierarchicalTransform2 Parent
        {
            get
            {
                return this.parent;
            }

            set
            {
                if( this.parent == value )
                    return;

                this.parent = value;
                this.InformTransformUpdateNeeded();
            }
        }

        /// <summary>
        /// Gets the <see cref="IEntity"/> that owns the <see cref="Parent"/> <see cref="HierarchicalTransform2"/> 
        /// of this <see cref="HierarchicalTransform2"/>.
        /// </summary>
        /// <value>The parent Entity of the Entity that owns this Component.</value>
        public IEntity ParentOwner
        {
            get
            {
                if( parent == null )
                    return null;

                return parent.Owner;
            }
        }

        /// <summary>
        /// Gets the list of child <see cref="HierarchicalTransform2"/>s
        /// of this <see cref="HierarchicalTransform2"/>.
        /// </summary>
        /// <value>The list of children.</value>
        public IEnumerable<HierarchicalTransform2> Children
        {
            get
            {
                return this.children;
            }
        }

        /// <summary>
        /// Gets or sets the relative position of this <see cref="HierarchicalTransform2"/> component
        /// to its <see cref="Parent"/>.
        /// </summary>
        /// <value>
        /// The position of the hierarchical-transformable Entity relative to its <see cref="ParentOwner"/>.
        /// The default value is <see cref="Vector2.Zero"/>.
        /// </value>
        public virtual Vector2 RelativePosition
        {
            get
            {
                return this.relativePosition;
            }

            set
            {
                if( value.X == this.relativePosition.X && value.Y == this.relativePosition.Y )
                    return;

                Vector2 oldValue = this.relativePosition;
                this.relativePosition = value;
                this.OnRelativePositionChanged( oldValue, value );
            }
        }

        /// <summary>
        /// Gets or sets the relative scale of this <see cref="HierarchicalTransform2"/> component
        /// to its <see cref="Parent"/>.
        /// </summary>
        /// <value>
        /// The scale of the hierarchical-transformable Entity relative to its <see cref="ParentOwner"/>.
        /// The default value is <see cref="Vector2.One"/>.
        /// </value>
        public virtual Vector2 RelativeScale
        {
            get
            {
                return this.relativeScale;
            }

            set
            {
                if( value.X == this.relativeScale.X && value.Y == this.relativeScale.Y )
                    return;

                Vector2 oldValue = this.relativeScale;
                this.relativeScale = value;

                this.OnRelativeScaleChanged( oldValue, value );
            }
        }

        /// <summary>
        /// Gets or sets the relative scale of this <see cref="HierarchicalTransform2"/> component
        /// to its <see cref="Parent"/>.
        /// </summary>
        /// <value>
        /// The rotation of the hierarchical-transformable Entity relative to its <see cref="ParentOwner"/>.
        /// The default value is 0.
        /// </value>
        public virtual float RelativeRotation
        {
            get
            {
                return this.relativeRotation;
            }

            set
            {
                if( value == this.relativeRotation )
                    return;

                float oldValue = this.relativeRotation;
                this.relativeRotation = value;

                this.OnRelativeRotationChanged( oldValue, value );
            }
        }

        /// <summary>
        /// Gets or sets the position of this <see cref="HierarchicalTransform2"/> component.
        /// </summary>
        /// <value>The position of the hierarchical-transformable Entity.</value>
        public override Vector2 Position
        {
            get
            {
                if( this.isTransformUpdateNeeded )
                    this.UpdateTransformNoCheck();

                return base.Position;
            }

            set
            {
                base.Position = value;
            }
        }

        /// <summary>
        /// Gets or sets the scale of this <see cref="HierarchicalTransform2"/> component.
        /// </summary>
        /// <value>The scale of the hierarchical-transformable Entity.</value>
        public override Vector2 Scale
        {
            get
            {
                if( this.isTransformUpdateNeeded )
                    this.UpdateTransformNoCheck();

                return base.Scale;
            }
            set
            {
                base.Scale = value;
            }
        }

        /// <summary>
        /// Gets or sets the rotation of this <see cref="HierarchicalTransform2"/> component.
        /// </summary>
        /// <value>The rotation value of the hierarchical-transformable Entity.</value>
        public override float Rotation
        {
            get
            {
                if( this.isTransformUpdateNeeded )
                    this.UpdateTransformNoCheck();

                return base.Rotation;
            }
            set
            {
                base.Rotation = value;
            }
        }

        #region Bequeaths

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HierarchicalTransform2"/> bequeaths
        /// its <see cref="Transform2.Position"/> to its <see cref="Children"/>.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool BequeathsPosition
        {
            get
            {
                return this.bequeathsPosition;
            }

            set
            {
                if( this.bequeathsPosition == value )
                    return;

                this.bequeathsPosition = value;
                this.InformChildrenTransformUpdateNeeded();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HierarchicalTransform2"/> bequeaths
        /// its <see cref="Transform2.Scale"/> to its <see cref="Children"/>.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool BequeathsScale
        {
            get
            {
                return this.bequeathsScale;
            }

            set
            {
                if( this.bequeathsScale == value )
                    return;

                this.bequeathsScale = value;
                this.InformChildrenTransformUpdateNeeded();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HierarchicalTransform2"/> bequeaths
        /// its <see cref="Transform2.Rotation"/> to its <see cref="Children"/>.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool BequeathsRotation
        {
            get
            {
                return this.bequeathsRotation;
            }

            set
            {
                if( this.bequeathsRotation == value )
                    return;

                this.bequeathsRotation = value;
                this.InformChildrenTransformUpdateNeeded();
            }
        }

        #endregion

        #region Inherits

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HierarchicalTransform2"/> inherits
        /// its <see cref="Transform2.Position"/> from its <see cref="Parent"/>.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool InheritsPosition
        {
            get
            {
                return this.inheritsPosition;
            }

            set
            {
                if( this.inheritsPosition == value )
                    return;

                this.inheritsPosition = value;
                this.InformTransformUpdateNeeded();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HierarchicalTransform2"/> inherits
        /// its <see cref="Transform2.Scale"/> from its <see cref="Parent"/>.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool InheritsScale
        {
            get
            {
                return this.inheritsScale;
            }

            set
            {
                if( this.inheritsScale == value )
                    return;

                this.inheritsScale = value;
                this.InformTransformUpdateNeeded();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HierarchicalTransform2"/> inherits
        /// its <see cref="Transform2.Rotation"/> from its <see cref="Parent"/>.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool InheritsRotation
        {
            get
            {
                return this.inheritsRotation;
            }

            set
            {
                if( this.inheritsRotation == value )
                    return;

                this.inheritsRotation = value;
                this.InformTransformUpdateNeeded();
            }
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalTransform2"/> class.
        /// </summary>
        public HierarchicalTransform2()
        {
            this.children = new List<HierarchicalTransform2>();
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Informs this node that its position needs to be updated.
        /// </summary>
        private void InformTransformUpdateNeeded()
        {
            this.isTransformUpdateNeeded = true;
            this.InformChildrenTransformUpdateNeeded();
        }

        /// <summary>
        /// Informs the children of this <see cref="HierarchicalTransform2"/> that 
        /// an transformation update is needed.
        /// </summary>
        private void InformChildrenTransformUpdateNeeded()
        {
            foreach( HierarchicalTransform2 child in this.children )
            {
                child.InformTransformUpdateNeeded();
            }
        }

        /// <summary>
        /// Updates the transformation information
        /// of this <see cref="HierarchicalTransform2"/>.
        /// </summary>
        public void UpdateTransform()
        {
            if( this.isTransformUpdateNeeded )
            {
                this.UpdateTransformNoCheck();
            }
        }

        /// <summary>
        /// Updates the transformation information
        /// of this <see cref="HierarchicalTransform2"/>.
        /// </summary>
        private void UpdateTransformNoCheck()
        {
            if( this.parent != null )
            {
                // Position
                if( this.inheritsPosition && parent.bequeathsPosition )
                    base.Position = parent.Position + this.RelativePosition;

                // Scale TODO: Implement correctly
                if( this.inheritsScale && parent.bequeathsScale )
                    base.Scale = (parent.Scale + this.RelativeScale) / 2.0f;

                // Rotation TODO: Implement correctly
                if( this.inheritsRotation && parent.bequeathsRotation )
                    base.Rotation = parent.Rotation + this.RelativeRotation;
            }

            this.isTransformUpdateNeeded = false;
        }

        /// <summary>
        /// Returns a humen-readable string representation of this <see cref="HierarchicalTransform2"/>.
        /// </summary>
        /// <returns>
        /// Returns a humen-readable description of this <see cref="HierarchicalTransform2"/>.
        /// </returns>
        public override string ToString()
        {
            IFormatProvider formatProvider = System.Globalization.CultureInfo.CurrentCulture;

            return string.Format(
                formatProvider,
                "{0} [Owner='{1}' ParentOwner='{2}' Position='{3}' RelativePosition='{4}'  Scale='{5}' RelativeScale='{6} Rotation='{7}' RelativeRotation='{8}']",
                this.GetType().Name,
                this.Owner,
                this.ParentOwner,
                this.Position.ToString( formatProvider ),
                this.RelativePosition.ToString( formatProvider ),
                this.Scale.ToString( formatProvider ),
                this.RelativeScale.ToString( formatProvider ),
                this.Rotation.ToString( formatProvider ),
                this.RelativeRotation.ToString( formatProvider )
            );
        }

        #region > Child Organisation <

        /// <summary>
        /// Adds the specified <see cref="HierarchicalTransform2"/> to
        /// this <see cref="HierarchicalTransform2"/> as a child.
        /// </summary>
        /// <param name="transform">
        /// The <see cref="HierarchicalTransform2"/> to add.
        /// </param>
        public void AddChild( HierarchicalTransform2 transform )
        {
            Contract.Requires<ArgumentNullException>( transform != null );

            this.children.Add( transform );
            transform.Parent = this;
        }

        #endregion

        #region > Events <

        /// <summary>
        /// Called when the <see cref="Transform2.Position"/> of this <see cref="HierarchicalTransform2"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnPositionChanged( Vector2 oldValue, Vector2 newValue )
        {
            this.InformChildrenTransformUpdateNeeded();
            base.OnPositionChanged( oldValue, newValue );
        }

        /// <summary>
        /// Called when the <see cref="Transform2.Scale"/> of this <see cref="HierarchicalTransform2"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnScaleChanged( Vector2 oldValue, Vector2 newValue )
        {
            this.InformChildrenTransformUpdateNeeded();
            base.OnScaleChanged( oldValue, newValue );
        }

        /// <summary>
        /// Called when the <see cref="Transform2.Rotation"/> of this <see cref="HierarchicalTransform2"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnRotationChanged( float oldValue, float newValue )
        {
            this.InformChildrenTransformUpdateNeeded();
            base.OnRotationChanged( oldValue, newValue );
        }

        /// <summary>
        /// Called when the <see cref="RelativePosition"/> of this <see cref="HierarchicalTransform2"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnRelativePositionChanged( Vector2 oldValue, Vector2 newValue )
        {
            this.InformTransformUpdateNeeded();
            if( this.RelativePositionChanged != null )
                this.RelativePositionChanged( this, new ChangedValue<Vector2>( oldValue, newValue ) );
        }

        /// <summary>
        /// Called when the <see cref="RelativeScale"/> of this <see cref="HierarchicalTransform2"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnRelativeScaleChanged( Vector2 oldValue, Vector2 newValue )
        {
            this.InformTransformUpdateNeeded();
            if( this.RelativeScaleChanged != null )
                this.RelativeScaleChanged( this, new ChangedValue<Vector2>( oldValue, newValue ) );
        }

        /// <summary>
        /// Called when the <see cref="RelativeRotation"/> of this <see cref="HierarchicalTransform2"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnRelativeRotationChanged( float oldValue, float newValue )
        {
            this.InformTransformUpdateNeeded();

            if( this.RelativeRotationChanged != null )
                this.RelativeRotationChanged( this, new ChangedValue<float>( oldValue, newValue ) );
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The parent <see cref="HierarchicalTransform2"/>
        /// this <see cref="HierarchicalTransform2"/> inherits
        /// its properties from. Can be null.
        /// </summary>
        private HierarchicalTransform2 parent;

        /// <summary>
        /// The child <see cref="HierarchicalTransform2"/>s
        /// of this <see cref="HierarchicalTransform2"/>.
        /// </summary>
        private readonly List<HierarchicalTransform2> children;

        /// <summary>
        /// Stores the relative position of this <see cref="HierarchicalTransform2"/> component
        /// to its <see cref="Parent"/>.
        /// </summary>
        private Vector2 relativePosition;

        /// <summary>
        /// Stores the relative scale of this <see cref="HierarchicalTransform2"/> component
        /// to its <see cref="Parent"/>.
        /// </summary>
        private Vector2 relativeScale = new Vector2( 1.0f, 1.0f );

        /// <summary>
        /// Stores the relative rotation of this <see cref="HierarchicalTransform2"/> component
        /// to its <see cref="Parent"/>.
        /// </summary>
        private float relativeRotation;

        /// <summary>
        /// Specifies whether this <see cref="HierarchicalTransform2"/> is out of date
        /// and needs to be updated.
        /// </summary>
        private bool isTransformUpdateNeeded;

        #region Bequeaths

        /// <summary>
        /// Specifies whether this <see cref="HierarchicalTransform2"/> bequeaths
        /// its <see cref="Transform2.Position"/> to its <see cref="Children"/>.
        /// </summary>
        private bool bequeathsPosition = true;

        /// <summary>
        /// Specifies whether this <see cref="HierarchicalTransform2"/> bequeaths
        /// its <see cref="Transform2.Scale"/> to its <see cref="Children"/>.
        /// </summary>
        private bool bequeathsScale = true;

        /// <summary>
        /// Specifies whether this <see cref="HierarchicalTransform2"/> bequeaths
        /// its <see cref="Transform2.Rotation"/> to its <see cref="Children"/>.
        /// </summary>
        private bool bequeathsRotation = true;

        #endregion

        #region Inherits

        /// <summary>
        /// Specifies whether this <see cref="HierarchicalTransform2"/> inherits
        /// its <see cref="Transform2.Position"/> from its <see cref="Parent"/>.
        /// </summary>
        private bool inheritsPosition = true;

        /// <summary>
        /// Specifies whether this <see cref="HierarchicalTransform2"/> inherits
        /// its <see cref="Transform2.Scale"/> from its <see cref="Parent"/>.
        /// </summary>
        private bool inheritsScale = true;

        /// <summary>
        /// Specifies whether this <see cref="HierarchicalTransform2"/> inherits
        /// its <see cref="Transform2.Rotation"/> from its <see cref="Parent"/>.
        /// </summary>
        private bool inheritsRotation = true;

        #endregion

        #endregion
    }
}
