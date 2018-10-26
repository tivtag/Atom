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

    //    [System.Diagnostics.CodeAnalysis.SuppressMessage(
    //        "Microsoft.StyleCop.CSharp.DocumentationRules",
    //        "SA1600:ElementsMustBeDocumented",
    //        Justification = "Contracts are not required to be documented. See the original class.")]
    //    // [Atom.Diagnostics.Contracts.ContractClassFor(typeof(IViewModel<>))]
    //    internal abstract class IViewModelContracts<TModel> : IViewModel<TModel>
    //    {
    //        public TModel Model
    //        {
    //            get
    //            {
    //                // Contract.Ensures(Contract.Result<TModel>() != null);

    //                return default(TModel);
    //            }
    //        }

    //        event System.ComponentModel.PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    //        {
    //            add { }
    //            remove { }
    //        }
    //    }
    //}

    // [Atom.Diagnostics.Contracts.ContractClass(typeof(IViewModelCommandContracts<,>))]
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

