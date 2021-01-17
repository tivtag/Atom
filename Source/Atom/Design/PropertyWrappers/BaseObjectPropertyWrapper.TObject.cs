// <copyright file="BaseObjectPropertyWrapper.TObject.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.BaseObjectPropertyWrapper{TObject} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Design
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Represents an abstract base implementation of the <see cref="IObjectPropertyWrapper"/> interface.
    /// </summary>
    /// <typeparam name="TObject">
    /// The type of the object that is beeing wrapped by this IObjectPropertyWrapper.
    /// </typeparam>
    public abstract class BaseObjectPropertyWrapper<TObject> : BaseObjectPropertyWrapper
    {
        /// <summary>
        /// Gets or sets the object this <see cref="IObjectPropertyWrapper"/> wraps around.
        /// </summary>
        /// <value>
        /// The actual data object this IObjectPropertyWrapper wraps around.
        /// </value>
        [Browsable( false )]
        public new TObject WrappedObject
        {
            get
            {
                return (TObject)base.WrappedObject;
            }

            set
            {
                base.WrappedObject = (TObject)value;
            }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> this <see cref="IObjectPropertyWrapper"/> wraps around.
        /// </summary>
        /// <value>
        /// The type of the object this IObjectPropertyWrapper wraps around; <typeparamref name="TObject"/>.
        /// </value>
        [Browsable( false )]
        public override Type WrappedType
        {
            get
            {
                return typeof( TObject );
            }
        }
    }
}
