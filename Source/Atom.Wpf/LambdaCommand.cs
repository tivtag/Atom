// <copyright file="LambdaCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.LambdaCommand class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.Windows.Input;

    /// <summary>
    /// Represents an <see cref="ICommand"/> that delegates its actions to lambdas / delegates.
    /// </summary>
    public sealed class LambdaCommand : ICommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the LambdaCommand class.
        /// </summary>
        /// <param name="action">
        /// The action that is executed when the new LambdaCommand is executed.
        /// </param>
        /// <param name="canExecute">
        /// The function that determines whether the new LambdaCommand can be executed in its current state.
        /// If null then this LambdaCommand can always be executed.
        /// </param>
        public LambdaCommand( Action<object> action, Func<object, bool> canExecute = null )
        {
            Contract.Requires<ArgumentNullException>( action != null );

            this.action = action;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.
        /// If the command does not require data to be passed, this object can be set to null.
        /// </param>
        /// <returns>
        /// true if this command can be executed;
        /// otherwise, false.
        /// </returns>
        public bool CanExecute( object parameter )
        {
            return this.canExecute != null ? this.canExecute( parameter ) : true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.
        /// If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public void Execute( object parameter )
        {
            if( !this.CanExecute( parameter ) )
                return;

            this.action( parameter );
        }

        /// <summary>
        /// Notifies this LambdaCommand that its current <see cref="CanExecute"/>
        /// change might have changed.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            this.CanExecuteChanged.Raise( this );
        }

        /// <summary>
        /// The action that is executed when the new LambdaCommand is executed.
        /// </summary>
        private readonly Action<object> action;

        /// <summary>
        /// The function that determines whether this LambdaCommand can be executed in its current state.
        /// If null then this LambdaCommand can always be executed.
        /// </summary>
        private readonly Func<object, bool> canExecute;
    }
}
