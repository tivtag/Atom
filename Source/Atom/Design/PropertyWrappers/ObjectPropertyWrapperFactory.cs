// <copyright file="ObjectPropertyWrapperFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Design.ObjectPropertyWrapperFactory class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Design
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implements a factory that is responsible of creating <see cref="IObjectPropertyWrapper"/>s.
    /// </summary>
    public class ObjectPropertyWrapperFactory : IObjectPropertyWrapperFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPropertyWrapperFactory"/> class.
        /// </summary>
        public ObjectPropertyWrapperFactory()
        {
            this.wrappers = new Dictionary<Type, IObjectPropertyWrapper>();
        }

        /// <summary>
        /// Receives an <see cref="IObjectPropertyWrapper"/> for the given <see cref="Object"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the given <paramref name="obj"/> is null.
        /// </exception>
        /// <param name="obj">
        /// The object to receive an IObjectPropertyWrapper for.
        /// </param>
        /// <returns>
        /// The initialized IObjectPropertyWrapper for the given Object,
        /// or null if there exists no IObjectPropertyWrapper for the requested type.
        /// </returns>
        public IObjectPropertyWrapper ReceiveWrapper( object obj )
        {
            IObjectPropertyWrapper wrapperTemplate;

            if( this.wrappers.TryGetValue( obj.GetType(), out wrapperTemplate ) )
            {
                var wrapper = (IObjectPropertyWrapper)wrapperTemplate.Clone();
                wrapper.WrappedObject = obj;
                return wrapper;
            }

            return null;
        }

        /// <summary>
        /// Receives an <see cref="IObjectPropertyWrapper"/> for the given <see cref="Object"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the given <paramref name="obj"/> is null.
        /// </exception>
        /// <param name="obj">
        /// The object to receive an IObjectPropertyWrapper for.
        /// </param>
        /// <returns>
        /// The initialized IObjectPropertyWrapper for the given Object,
        /// or the original object if there exists no IObjectPropertyWrapper for the given type.
        /// </returns>
        public object ReceiveWrapperOrObject( object obj )
        {
            var wrapper = this.ReceiveWrapper( obj );
            return wrapper != null ? wrapper : obj;
        }

        /// <summary>
        /// Gets the types of the objects this <see cref="ObjectPropertyWrapperFactory"/>
        /// provides an <see cref="IObjectPropertyWrapper"/> for.
        /// </summary>
        /// <returns>A new array that contains the types.</returns>
        public Type[] GetObjectTypes()
        {
            return this.wrappers.Keys.ToArray();
        }

        /// <summary>
        /// Registers the given <see cref="IObjectPropertyWrapper"/> at this <see cref="ObjectPropertyWrapperFactory"/>.
        /// </summary>
        /// <param name="wrapper">
        /// The wrapper to register.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the given <paramref name="wrapper"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the <paramref name="wrapper"/> already has been registered.
        /// </exception>
        public void RegisterWrapper( IObjectPropertyWrapper wrapper )
        {
            this.wrappers.Add( wrapper.WrappedType, wrapper );
        }

        /// <summary>
        /// Unregisters the <see cref="IObjectPropertyWrapper"/> for the given <see cref="Type"/>
        /// from this <see cref="ObjectPropertyWrapperFactory"/>.
        /// </summary>
        /// <param name="type">
        /// The type of the object the wrapper to unregister wraps around.
        /// </param>
        /// <returns>
        /// Returns true when the <see cref="IObjectPropertyWrapper"/> for the given <see cref="Type"/> has been removed;
        /// otherwise false.
        /// </returns>
        public bool UnregisterWrapper( Type type )
        {
            return this.wrappers.Remove( type );
        }

        /// <summary>
        /// Stores the IObjectPropertyWrappers, sorted by the type they wrap.
        /// </summary>
        private readonly Dictionary<Type, IObjectPropertyWrapper> wrappers;
    }
}

