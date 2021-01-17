// <copyright file="IEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Components.IEntity interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Components
{
    /// <summary>
    /// By aggregating <see cref="IComponent"/>s an IEntity can loosely dynamic behaviour.
    /// </summary>
    /// <remarks>
    /// By using composition over inheritance one can archive more flexible object models.
    /// </remarks>
    public interface IEntity : INameable, IUpdateable, IPreUpdateable
    {
        /// <summary>
        /// Gets the <see cref="IEntityComponentCollection"/> that contains the <see cref="IComponent"/>s
        /// this IEntity is composed of.
        /// </summary>
        IEntityComponentCollection Components
        {
            get;
        }
    }
}
