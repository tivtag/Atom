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

    /// <summary>
    /// Represents an <see cref="ICommand"/> that is exposed by a <see cref="IViewModel&lt;TModel&gt;"/>.
    /// </summary>
    /// <typeparam name="TViewModel">
    /// The concrete type of ViewModel the <see cref="ICommand"/> uses.
    /// </typeparam>
    /// <typeparam name="TModel">
    /// The type of the Model stored in the <see cref="IViewModel&lt;TModel&gt;"/>.
    /// </typeparam>
    [System.Diagnostics.Contracts.ContractClass(typeof(IViewModelCommandContracts<,>))]
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
