// <copyright file="AudioException.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Fmod.AudioException class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Fmod
{
    using System;

    /// <summary> 
    /// The exception that is thrown when an error occurs within the FMOD object framework.
    /// </summary>
    [Serializable]
    public class AudioException : System.Exception
    {
        /// <summary> Initializes a new instance of the <see cref="AudioException"/> class. </summary>
        public AudioException()
            : base()
        {         
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="AudioException"/> class and sets
        /// the error message to the specified <see cref="System.String"/>.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        public AudioException( string message )
            : base( message )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioException"/> class 
        /// with a specified error message and a reference 
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        /// <param name="innerException"> The exception that is cause of the new <see cref="AudioException"/>. </param>
        public AudioException( string message, System.Exception innerException )
            : base( message, innerException )
        {
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="AudioException"/> class; and
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
        protected AudioException( 
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
            : base( info, context )
        {
        }
    }
}
