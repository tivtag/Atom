// <copyright file="SafeExecute.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.SafeExecute class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf
{
    using System;
    using System.Windows;

    /// <summary>
    /// Defines mechanism for executing <see cref="Action"/>s
    /// within a specific safe context.
    /// </summary>
    public static class SafeExecute
    {
        /// <summary>
        /// Executes the specified Action within a safe context
        /// that catches all exceptions and informs the user by displaying
        /// the error in a MessageBox.
        /// </summary>
        /// <param name="action">
        /// The action to apply.
        /// </param>
        public static void WithMsgBox( Action action )
        {
            try
            {
                action();
            }
            catch( Exception exc )
            {
                MessageBox.Show(
                    exc.ToString(),
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }
    }
}
