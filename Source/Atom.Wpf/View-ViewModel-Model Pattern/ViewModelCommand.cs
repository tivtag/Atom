// <copyright file="ViewModelCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.ViewModelCommand{TViewModel, TModel} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.Windows.Input;

    /// <summary>
    /// Represents the base class of all <see cref="ICommand"/>s that are exposed
    /// by a <see cref="IViewModel&lt;TModel&gt;"/>.
    /// </summary>
    /// <remarks>
    /// Commands are usually exposed from the <see cref="IViewModel&lt;TModel&gt;"/>
    /// using <see cref="ICommand"/> and not this base class.
    /// This class is meant to reduce the amount of work needed 
    /// to implement new <see cref="ICommand"/>s for <see cref="IViewModel&lt;TModel&gt;"/>s.
    /// </remarks>
    /// <typeparam name="TViewModel">
    /// The concrete type of ViewModel the <see cref="ICommand"/> uses.
    /// </typeparam>
    /// <typeparam name="TModel">
    /// The type of the Model stored in the <see cref="IViewModel&lt;TModel&gt;"/>.
    /// </typeparam>
    public abstract class ViewModelCommand<TViewModel, TModel> : BaseCommand, IViewModelCommand<TViewModel, TModel>
        where TViewModel : IViewModel<TModel>
    {  
        /// <summary>
        /// Gets the model the <see cref="ViewModel"/> wraps around.
        /// </summary>
        public TModel Model
        {
            get
            {
                return this.viewModel.Model;
            }
        }

        /// <summary>
        /// Gets the <see cref="Atom.Wpf.ViewModel&lt;TModel&gt;"/> that owns this ViewModelCommand{TViewModel, TModel}.
        /// </summary>
        public TViewModel ViewModel
        {
            get
            {
                return this.viewModel;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelCommand{TViewModel, TModel}"/> class.
        /// </summary>
        /// <param name="viewModel">
        /// The <see cref="IViewModel{TModel}"/> that executes the new <see cref="ViewModelCommand{TViewModel, TModel}"/>.
        /// </param>
        protected ViewModelCommand( TViewModel viewModel )
        {
            // Contract.Requires<ArgumentNullException>( viewModel != null );

            this.viewModel = viewModel;
        }

        /// <summary>
        /// The <see cref="Atom.Wpf.IViewModel&lt;TModel&gt;"/> that owns this <see cref="ViewModelCommand{TViewModel, TModel}"/>.
        /// </summary>
        private readonly TViewModel viewModel;
    }
}
