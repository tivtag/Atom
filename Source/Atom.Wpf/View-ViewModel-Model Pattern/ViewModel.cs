// <copyright file="ViewModel.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.ViewModel{TModel} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A ViewModel represents the layer between the View (XAML) and a Model (any kind of data/buisiness object/etc).
    /// </summary>
    /// <remarks>
    /// This layer provides the View with the commands and bindings it needs to modify the Model.
    /// Usually Models only contain valid data,
    /// where it is possible for the ViewModel to store invalid values.
    /// Also the ViewModel is responsible to convert data of the Model 
    /// into data the UI can understand. (reduces the amount of IValueConverters needed in XAML code)
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the Model stored in the <see cref="ViewModel{TModel}"/>.
    /// </typeparam>
    public abstract class ViewModel<TModel> : IViewModel<TModel>
    {
        /// <summary>
        /// Raised when a property of this <see cref="ViewModel{TModel}"/> has changed.
        /// This is required for binding.
        /// </summary>
        /// <remarks>
        /// Not every property is required to notify by raising this event.
        /// </remarks>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the model this <see cref="ViewModel{TModel}"/> wraps around.
        /// </summary>
        [Browsable(false)]
        public TModel Model
        {
            get
            {
                return this.model;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel{TModel}"/> class.
        /// </summary>
        /// <param name="model">
        /// The model the new <see cref="ViewModel{TModel}"/> wraps around.
        /// </param>
        protected ViewModel( TModel model )
        {
            Contract.Requires<ArgumentNullException>( model != null );

            this.model = model;
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event of this <see cref="ViewModel{TModel}"/>.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property whose value has changed.
        /// </param>
        protected virtual void OnPropertyChanged( string propertyName )
        {
            if( this.PropertyChanged != null )
            {
                this.PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        /// <summary>
        /// Overriden return the string return by the ToString method of the <see cref="Model"/>.
        /// </summary>
        /// <returns>The string return by the ToString method of the <see cref="Model"/>.</returns>
        public override string ToString()
        {
            return this.Model.ToString();
        }

        /// <summary>
        /// The model this <see cref="ViewModel{TModel}"/> wraps around.
        /// </summary>
        private readonly TModel model;
    }
}
