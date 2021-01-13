// <copyright file="IViewModelCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.IViewModelCommand{TViewModel, TModel} interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf
{
    using System.Windows.Input;

    public interface IViewModelCommand<TViewModel, TModel> : ICommand
        where TViewModel : IViewModel<TModel>
    {
        /// <summary>
        /// Gets the model the <see cref="ViewModel"/> wraps around.
        /// </summary>
        TModel Model
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IViewModel&lt;TModel&gt;"/> that owns this IViewModelCommand{TViewModel, TModel}.
        /// </summary>
        TViewModel ViewModel
        {
            get;
        }
    }
}

