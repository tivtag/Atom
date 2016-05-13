// <copyright file="Component.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Components.Component class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Components
{
    /// <summary>
    /// Defines an abstract base implement of the <see cref="IComponent"/> interface,
    /// representing an abstraction of specific functionality that is owned by an <see cref="IEntity"/>.
    /// </summary>
    public abstract class Component : IComponent
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the <see cref="IEntity"/> that owns this <see cref="Component"/>.
        /// </summary>
        /// <remarks>
        /// Components are supposed to be added or removed using
        /// the <see cref="IEntityComponentCollection"/>, not this property.
        /// </remarks>
        /// <value>
        /// The Entity that owns this Component.
        /// </value>
        public IEntity Owner
        {
            get
            {
                return this.owner;
            }

            set
            {
                this.owner = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Component"/> is enabled or disabled.
        /// </summary>
        /// <remarks>
        /// Override this property to handle toggling manually.
        /// </remarks>
        /// <value>The default value is true.</value>
        public virtual bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                this.isEnabled = value;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the Component class.
        /// </summary>
        protected Component()
        {
        }

        /// <summary>
        /// Called when this IComponent has been successfully attached to an <see cref="IEntity"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="InitializeBindings"/> should be used to get any IComponents
        /// this IComponent depends on.
        /// </remarks>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Called when an IComponent has been removed or added to the <see cref="IEntity"/> that owns this IComponent.
        /// Override this method to receive IComponents this IComponent depends on.
        /// </summary>
        public virtual void InitializeBindings()
        {
        }
        
        #endregion

        #region [ Methods ]

        /// <summary>
        /// Called when the game has determined that logic needs to be processed.
        /// Override this method to add logic specific code.
        /// This is a no-op and must not be called by the sub-class.
        /// </summary>
        /// <param name="updateContext">
        /// The current update context.
        /// </param>
        public virtual void Update( IUpdateContext updateContext )
        {
            // no-op
        }

        /// <summary>
        /// Gets called before <see cref="Update"/> is called.
        /// This is a no-op and must not be called by the sub-class.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public virtual void PreUpdate( IUpdateContext updateContext )
        {
            // no-op
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The <see cref="IEntity"/> that owns this <see cref="Component"/>.
        /// </summary>
        private IEntity owner;

        /// <summary>
        /// States whether this <see cref="Component"/> is currently enabled.
        /// </summary>
        private bool isEnabled = true;

        #endregion
    }
}
