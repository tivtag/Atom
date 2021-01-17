// <copyright file="IXnaUpdateContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.IXnaUpdateContext interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    /// <summary>
    /// Identifies an object that represents an update context used in an XNA application.
    /// </summary>
    /// <remarks>
    /// An update context is passed to the Update method of various objects.
    /// </remarks>
    public interface IXnaUpdateContext : IUpdateContext, IXnaContext
    {
    }
}
