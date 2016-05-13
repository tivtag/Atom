// <copyright file="IAction.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.IAction interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;
    using System.ComponentModel;
    using Atom.Storage;

    /// <summary>
    /// Represents an undoable and storable action.
    /// </summary>
    [TypeConverter( typeof( ExpandableObjectConverter ) )]
    public interface IAction : IStorable
    {
        /// <summary>
        /// Executes this IAction.
        /// </summary>
        void Execute();

        /// <summary>
        /// Immediately undoes this IAction.
        /// </summary>
        void Dexecute();

        /// <summary>
        /// Gets a value indicating whether this IAction can be executed.
        /// </summary>
        /// <returns>
        /// true if this IAction can be executed;
        /// otherwise false.
        /// </returns>
        bool CanExecute();

        /// <summary>
        /// Gets a localized description text of this IAction.
        /// </summary>
        /// <returns>
        /// The localized description of this IAction.
        /// </returns>
        string GetDescription();
    }
}
