// <copyright file="NotFoundException.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.NotFoundException class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    using System;

    /// <summary> 
    /// The exception that is thrown when something couldn't be found.
    /// </summary>
    [Serializable]
    public class NotFoundException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        public NotFoundException()
            : base()
        {
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="NotFoundException"/> class and sets
        /// the error message to the specified <see cref="System.String"/>.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        public NotFoundException( string message )
            : base( message )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class 
        /// with a specified error message and a reference 
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        /// <param name="innerException"> The exception that is cause of the new <see cref="NotFoundException"/>. </param>
        public NotFoundException( string message, System.Exception innerException )
            : base( message, innerException )
        {
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="NotFoundException"/> class, and
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
        protected NotFoundException(
                System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context
            )
            : base( info, context )
        {
        }
    }
}
