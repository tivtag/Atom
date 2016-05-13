// <copyright file="IErrorHook.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.IErrorHook interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting
{
    /// <summary>
    /// Represents a mechanism that hooks into some other mechanism
    /// that listens to IErrors.
    /// </summary>
    public interface IErrorHook
    {
        /// <summary>
        /// Hooks this IErrorHook.
        /// </summary>
        void Hook();

        /// <summary>
        /// Unhooks this IErrorHook.
        /// </summary>
        void Unhook();
    }
}
