// <copyright file="ISceneEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.ISceneEntity interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Scene
{
    using Atom.Components;

    /// <summary>
    /// Represents an <see cref="IEntity"/> that is part of an <see cref="IScene"/>.
    /// </summary>
    public interface ISceneEntity : IEntity, ISceneObject
    {
    }
}
