//// <copyright file="IViewModel.Contracts.cs" company="federrot Software">
////     Copyright (c) federrot Software. All rights reserved.
//// </copyright>
//// <summary>
////     Defines the Atom.Wpf.IViewModelContracts{TModel} class.
//// </summary>
//// <author>
////     Paul Ennemoser
//// </author>

//namespace Atom.Wpf
//{
//    using Atom.Diagnostics.Contracts;

//    [System.Diagnostics.CodeAnalysis.SuppressMessage(
//        "Microsoft.StyleCop.CSharp.DocumentationRules",
//        "SA1600:ElementsMustBeDocumented",
//        Justification = "Contracts are not required to be documented. See the original class." )]
//    [Atom.Diagnostics.Contracts.ContractClassFor( typeof( IViewModel<> ) )]
//    internal abstract class IViewModelContracts<TModel> : IViewModel<TModel>
//    {
//        public TModel Model
//        {
//            get
//            {
//                Contract.Ensures( Contract.Result<TModel>() != null );

//                return default( TModel );
//            }
//        }

//        event System.ComponentModel.PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged
//        {
//            add { }
//            remove { }
//        }
//    }
//}
