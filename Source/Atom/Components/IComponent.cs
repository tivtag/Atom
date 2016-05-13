// <copyright file="IComponent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Components.IComponent interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Components
{
    using System;

    /// <summary>
    /// An IComponent represents an abstraction of specific functionality
    /// that is owned by an <see cref="IEntity"/>.
    /// </summary>
    public interface IComponent : IUpdateable, IPreUpdateable
    {
        /// <summary>
        /// Gets or sets the <see cref="IEntity"/> that owns this IComponent.
        /// </summary>
        /// <remarks>
        /// IComponents are supposed to be added or removed using
        /// the <see cref="IEntityComponentCollection"/>, not this property.
        /// </remarks>
        /// <value>
        /// The <see cref="IEntity"/> that owns this IComponent.
        /// </value>
        IEntity Owner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this IComponent is enabled or disabled.
        /// </summary>
        /// <value>The default value is true.</value>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Called when this IComponent has been successfully attached to an <see cref="IEntity"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="InitializeBindings"/> should be used to get any IComponents
        /// this IComponent depends on.
        /// </remarks>
        void Initialize();

        /// <summary>
        /// Called when an IComponent has been removed or added to the <see cref="IEntity"/> that owns this IComponent.
        /// Override this method to receive IComponents this IComponent depends on.
        /// </summary>
        void InitializeBindings();
    }
}
