// <copyright file="IViewModel.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.IViewModel{TModel} interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf
{
    using System.ComponentModel;

    /// <summary>
    /// Implement this interface if you want your specific <see cref="ViewModel{TModel}"/> 
    /// implemenentation class to allow type-safe access to the Model.
    /// </summary>
    /// <remarks>
    /// See the <see cref="ViewModel{TModel}"/> class for a more detailed description.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of model.
    /// </typeparam>
    // [Atom.Diagnostics.Contracts.ContractClass(typeof(IViewModelContracts<>))]
    public interface IViewModel<TModel> : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the model this <see cref="IViewModel{TModel}"/> wraps around.
        /// </summary>
        TModel Model { get; }
    }
}
