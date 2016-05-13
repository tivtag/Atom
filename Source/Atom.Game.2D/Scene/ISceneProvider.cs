// <copyright file="ISceneProvider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Scene.ISceneProvider interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Scene
{
    /// <summary>
    /// Provides access to an <see cref="IScene"/> object.
    /// </summary>
    public interface ISceneProvider
    {
        /// <summary>
        /// Gets an <see cref="IScene"/> object.
        /// </summary>
        /// <value>The <see cref="IScene"/> object provided by this ISceneProvider.</value>
        IScene Scene 
        {
            get; 
        }
    }
}
