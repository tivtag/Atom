// <copyright file="ComponentNotFoundException.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Components.ComponentNotFoundException class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Components
{
    using System;

    /// <summary> 
    /// The exception that is thrown when an <see cref="IComponent"/> could not be found.
    /// </summary>
    [Serializable]
    public class ComponentNotFoundException : NotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentNotFoundException"/> class.
        /// </summary>
        public ComponentNotFoundException()
            : base()
        {
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="ComponentNotFoundException"/> class and sets
        /// the error message automatically.
        /// </summary>
        /// <param name="componentType">
        /// The type of the Component.
        /// </param>
        public ComponentNotFoundException( Type componentType )
            : base(
                string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    Atom.ErrorStrings.ComponentWasNotFound,
                    componentType != null ? componentType.ToString() : string.Empty
                )
             )
        {
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="ComponentNotFoundException"/> class and sets
        /// the error message to the specified <see cref="System.String"/>.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        public ComponentNotFoundException( string message )
            : base( message )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentNotFoundException"/> class 
        /// with a specified error message and a reference 
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        /// <param name="innerException"> The exception that is cause of the new <see cref="ComponentNotFoundException"/>. </param>
        public ComponentNotFoundException( string message, System.Exception innerException )
            : base( message, innerException )
        {
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="ComponentNotFoundException"/> class, and
        /// passes the specified <see cref="System.Runtime.Serialization.SerializationInfo"/> and
        /// <see cref="System.Runtime.Serialization.StreamingContext"/> to the <see cref="System.Exception"/> constructor.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext "/> that 
        /// contains contextual information about the source or destination.
        /// </param>
        protected ComponentNotFoundException(
                System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context
            )
            : base( info, context )
        {
        }
    }
}
