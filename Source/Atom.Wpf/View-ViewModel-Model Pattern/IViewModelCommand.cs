// <copyright file="IViewModelCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.IViewModelCommand{TViewModel, TModel} interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf
{
    using System.Windows.Input;

    /// <summary>
    /// Definues a command that is executed within a View-ViewModel-Model context.
    /// </summary>
    /// <typeparam name="TViewModel">
    /// The type of the view model.
    /// </typeparam>
    /// <typeparam name="TModel">
    /// The type of the model.
    /// </typeparam>
    public interface IViewModelCommand<TViewModel, TModel> : ICommand
        where TViewModel : IViewModel<TModel>
    {
        /// <summary>
        /// Gets the model the <see cref="ViewModel"/> wraps around.
        /// </summary>
        TModel Model { get; }

        /// <summary>
        /// Gets the <see cref="IViewModel&lt;TModel&gt;"/> that owns this IViewModelCommand{TViewModel, TModel}.
        /// </summary>
        TViewModel ViewModel { get; }
    }
}

