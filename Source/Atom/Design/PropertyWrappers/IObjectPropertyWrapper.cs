// <copyright file="IObjectPropertyWrapper.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Design.IObjectPropertyWrapper class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Design
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Exposes the properties of an <see cref="Object"/> by wrapping around it,
    /// adding additional behaviour and metadata.
    /// </summary>
    /// <remarks>
    /// An object property wrapper is used to extend the properties of an existing object.
    /// This is useful when an object is supposed to be bound to a PropertyGrid.
    /// <para>
    /// By binding the PropertyGrid to an IObjectPropertyWrapper instead of an actual Object
    /// one can easily add metadata (such as DisplayNameAttribute, DescriptionAttribute, EditorAttribute, ..),
    /// additional behaviour, and choose exactly what properties to expose to the PropertyGrid.
    /// </para>
    /// This also pulls all design-time related metadata, away from the object, to the IObjectPropertyWrapper.
    /// </remarks>
    // [Atom.Diagnostics.Contracts.ContractClass(typeof(IObjectPropertyWrapperContracts))]
    public interface IObjectPropertyWrapper : ICloneable, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the object this <see cref="IObjectPropertyWrapper"/> wraps around.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// If the type of the given value is not compatible with 
        /// the <see cref="WrappedType"/> of this IObjectPropertyWrapper.
        /// </exception>
        /// <value>The object this IObjectPropertyWrapper wraps around.</value>
        object WrappedObject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> this <see cref="IObjectPropertyWrapper"/> wraps around.
        /// </summary>
        /// <value>The Type of the object this IObjectPropertyWrapper wraps around.</value>
        Type WrappedType
        {
            get;
        }
    }
}
