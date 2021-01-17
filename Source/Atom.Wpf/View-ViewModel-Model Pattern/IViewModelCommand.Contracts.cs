//// <copyright file="IViewModelCommand.Contracts.cs" company="federrot Software">
////     Copyright (c) federrot Software. All rights reserved.
//// </copyright>
//// <summary>
////     Defines the Atom.Wpf.IViewModelCommandContracts{TModel} class.
//// </summary>
//// <author>
////     Paul Ennemoser
//// </author>

//namespace Atom.Wpf
//{
//    using System;
//    using Atom.Diagnostics.Contracts;

//    [System.Diagnostics.CodeAnalysis.SuppressMessage(
//        "Microsoft.StyleCop.CSharp.DocumentationRules",
//        "SA1600:ElementsMustBeDocumented",
//        Justification = "Contracts are not required to be documented. See the original class." )]
//    [ContractClassFor( typeof( IViewModelCommand<,> ) )]
//    internal abstract class IViewModelCommandContracts<TViewModel, TModel> : IViewModelCommand<TViewModel, TModel>
//        where TViewModel : IViewModel<TModel>
//    {
//        public TModel Model
//        {
//            get
//            {
//                Contract.Ensures( Contract.Result<TModel>() != null );
//                return default( TModel );
//            }
//        }

//        public TViewModel ViewModel
//        {
//            get
//            {
//                Contract.Ensures( Contract.Result<TViewModel>() != null );
//                return default( TViewModel );
//            }
//        }

//        bool System.Windows.Input.ICommand.CanExecute( object parameter )
//        {
//            return default( bool );
//        }

//        event EventHandler System.Windows.Input.ICommand.CanExecuteChanged
//        {
//            add { }
//            remove { }
//        }

//        void System.Windows.Input.ICommand.Execute( object parameter )
//        {
//        }
//    }
//}
