// <copyright file="QuestionMessageBox.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.QuestionMessageBox class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf
{
    using System.Windows;
    using Atom.Wpf.Properties;

    /// <summary>
    /// Defines static utility methods that provide an easy
    /// way to show a MessageBox that asks the user a Yes/No question.
    /// </summary>
    public static class QuestionMessageBox
    {
        /// <summary>
        /// Shows a modal <see cref="MessageBox"/> that asks the
        /// user the given <paramref name="question"/>.
        /// </summary>
        /// <param name="question">
        /// The question to ask the user.
        /// </param>
        /// <returns>
        /// True if the user has answered to the question with Yes;
        /// otherwise false.
        /// </returns>
        public static bool Show( string question )
        {
            return MessageBox.Show( 
                question, 
                Resources.Question, 
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question
            ) == MessageBoxResult.Yes;
        }
    }
}
