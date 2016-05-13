// <copyright file="BaseObjectPropertyWrapper.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Design.BaseObjectPropertyWrapper class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Design
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Represents an abstract base implementation of the <see cref="IObjectPropertyWrapper"/> interface.
    /// </summary>
    public abstract class BaseObjectPropertyWrapper : IObjectPropertyWrapper
    {
        #region [ Events ]

        /// <summary>
        /// Fired when any property of this IObjectPropertyWrapper has changed.
        /// </summary>
        /// <remarks>
        /// Properties that wish to support binding, such as in Windows Presentation Foundation,
        /// must notify the user that they have change.
        /// </remarks>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the object this <see cref="IObjectPropertyWrapper"/> wraps around.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// If the type of the given value is not compatible with 
        /// the <see cref="WrappedType"/> of this IObjectPropertyWrapper.
        /// </exception>
        /// <value>
        /// The actual data object this IObjectPropertyWrapper wraps around.
        /// </value>
        [Browsable( false )]
        public object WrappedObject
        {
            get
            {
                return this.wrappedObject;
            }

            set
            {
                if( value == this.wrappedObject )
                    return;

                this.wrappedObject = value;
                this.OnPropertyChanged( "WrappedObject" );
            }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> this <see cref="IObjectPropertyWrapper"/> wraps around.
        /// </summary>
        [Browsable( false )]
        public abstract Type WrappedType
        {
            get;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns a clone of this <see cref="IObjectPropertyWrapper"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="WrappedObject"/> is not cloned, only the IObjectPropertyWrapper.
        /// </remarks>
        /// <returns>
        /// The cloned IObjectPropertyWrapper.
        /// </returns>
        public abstract IObjectPropertyWrapper Clone();

        /// <summary>
        /// Returns a clone of this <see cref="IObjectPropertyWrapper"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="WrappedObject"/> is not cloned, only the IObjectPropertyWrapper.
        /// </remarks>
        /// <returns>
        /// The cloned IObjectPropertyWrapper.
        /// </returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property whose value has changed.
        /// </param>
        protected void OnPropertyChanged( string propertyName )
        {
            if( this.PropertyChanged != null )
            {
                this.PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The object this <see cref="IObjectPropertyWrapper"/> wraps around.
        /// </summary>
        private object wrappedObject;

        #endregion
    }
}