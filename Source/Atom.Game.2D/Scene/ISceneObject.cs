// <copyright file="ISceneObject.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.ISceneObject interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene
{
    using System;
    using Atom.Scene;

    /// <summary>
    /// Defines the interface that Objects must implement
    /// that wish to be part of a scene.
    /// </summary>
    public interface ISceneObject : ISceneProvider
    {
        /// <summary>
        /// Gets or sets the <see cref="IScene"/> that owns
        /// this <see cref="ISceneObject"/>.
        /// </summary>
        /// <value>
        /// Is null if the object is not part of a scene.
        /// </value>
        new IScene Scene 
        {
            get;
            set;
        }

        /// <summary>
        /// Adds this <see cref="ISceneObject"/> to the specified <see cref="IScene"/>.
        /// </summary>
        /// <param name="scene">
        /// The IScene to add this <see cref="ISceneObject"/> to.
        /// </param>
        void AddToScene( IScene scene );

        /// <summary>
        /// Removes this <see cref="ISceneObject"/> from its current <see cref="Scene"/>.
        /// </summary>
        void RemoveFromScene();
    }
}
