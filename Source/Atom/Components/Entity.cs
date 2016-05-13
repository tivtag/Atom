// <copyright file="Entity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Components.Entity class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Components
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// By aggregating <see cref="IComponent"/>s an Entity can loosely expose behaviour.
    /// </summary>
    /// <remarks>
    /// By using composition over inheritance one can archive more flexible object models.
    /// </remarks>
    public class Entity : IEntity
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the name that (usually) uniquely identifies this Entity.
        /// </summary>
        /// <value>
        /// The default value is null.
        /// </value>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="IEntityComponentCollection"/> that contains the <see cref="IComponent"/>s
        /// this Entity is composed of.
        /// </summary>
        public IEntityComponentCollection Components
        {
            get 
            {
                return this.components;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the Entity class, that 
        /// uses the <see cref="EntityComponentCollection"/> with a default capacity of 5.
        /// </summary>
        public Entity()
            : this( 5 )
        {
        }

        /// <summary>
        /// Initializes a new instance of the Entity class, that 
        /// uses the <see cref="EntityComponentCollection"/> with the given <paramref name="capacity"/>.
        /// </summary>
        /// <param name="capacity">
        /// The initial number of <see cref="IComponent"/>s this Entity may contain
        /// without reallocating memory.
        /// </param>
        public Entity( int capacity )
        {
            this.components = new EntityComponentCollection( this, capacity );
        }

        /// <summary>
        /// Initializes a new instance of the Entity class.
        /// </summary>
        /// <param name="components">
        /// The IEntityComponentCollection that is injected into the new Entity.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="components"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the given <see cref="IEntityComponentCollection"/> is already owned by another <see cref="Entity"/>.
        /// </exception>
        public Entity( IEntityComponentCollection components )
        { 
            Contract.Requires<ArgumentNullException>( components != null );

            // Contract.Requires<ArgumentException>(
            //     components.Owner == this,
            //     ErrorStrings.EntityComponentCollectionIsOwnedByDifferentEntity
            // );
            this.components = components;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Updates this Entity and all the <see cref="IComponent"/>s it is composed of.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public virtual void Update( IUpdateContext updateContext )
        {
            this.components.Update( updateContext );
        }

        /// <summary>
        /// Pre-updates this Entity and all the <see cref="IComponent"/>s it is composed of.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public virtual void PreUpdate( IUpdateContext updateContext )
        {
            this.components.PreUpdate( updateContext );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The <see cref="IEntityComponentCollection"/> that contains the
        /// <see cref="IComponent"/>s this Entity is composed of.
        /// </summary>
        private readonly IEntityComponentCollection components;

        #endregion
    }
}
