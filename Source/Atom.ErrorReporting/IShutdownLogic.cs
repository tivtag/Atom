// <copyright file="IShutdownLogic.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.IShutdownLogic interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting
{
    /// <summary>
    /// Provides a mechanism for shutting down the application after
    /// a fatal error.
    /// </summary>
    public interface IShutdownLogic
    {
        /// <summary>
        /// Executes this IShutdownLogic.
        /// </summary>
        void DoShutdown();
    }
}
