// <copyright file="BaseCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.BaseCommand class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Represents a base implementation of the <see cref="ICommand"/> interface.
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        /// <summary>
        /// Raised when the CanExecute state of this <see cref="ICommand"/> has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Executes this ICommand.
        /// </summary>
        /// <param name="parameter">
        /// The parameter passed to the command.
        /// </param>
        public abstract void Execute( object parameter );

        /// <summary>
        /// Defines the method that determines whether this ICommand can execute in its current state.
        /// </summary>
        /// <remarks>
        /// The default value is true. Overwrite this method to provide a custom implementation.
        /// </remarks>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if this command can be executed; otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool CanExecute( object parameter )
        {
            return true;
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            this.CanExecuteChanged.Raise( this );
        }
    }
}
