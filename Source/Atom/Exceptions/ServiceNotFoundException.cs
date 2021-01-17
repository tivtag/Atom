// <copyright file="ServiceNotFoundException.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//      Defines the Atom.ServiceNotFoundException class.
// </summary>
// <author>
//      Paul Ennemoser
// </author>

namespace Atom
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary> 
    /// The exception that is thrown when a Service could not be found.
    /// </summary>
    [Serializable]
    public class ServiceNotFoundException : NotFoundException
    {
        /// <summary>
        /// Gets the type of the service which could not be found - if set.
        /// </summary>
        /// <value>The type of the service which could not be found - if set.</value>
        public Type ServiceType 
        {
            get
            {
                // Contract.Ensures( Contract.Result<Type>() != null );

                return this.serviceType;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceNotFoundException"/> class.
        /// </summary>
        public ServiceNotFoundException()
            : base()
        {
            this.serviceType = typeof( System.Reflection.Missing );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceNotFoundException"/> class.
        /// </summary>
        /// <param name="serviceType">
        /// The type of the service which could not be found.
        /// </param>
        public ServiceNotFoundException( Type serviceType )
            : this( GetMessage( serviceType ), serviceType )
        {
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="ServiceNotFoundException"/> class and sets
        /// the error message to the specified <see cref="System.String"/>.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        /// <param name="serviceType">
        /// The type of the service which could not be found.
        /// </param>
        public ServiceNotFoundException( string message, Type serviceType )
            : base( message )
        {
            Contract.Requires<ArgumentNullException>( serviceType != null );

            this.serviceType = serviceType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceNotFoundException"/> class 
        /// with a specified error message and a reference 
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        /// <param name="serviceType">
        /// The type of the service which could not be found.
        /// </param>
        /// <param name="innerException">
        /// The exception that is cause of the new <see cref="ServiceNotFoundException"/>.
        /// </param>
        public ServiceNotFoundException( string message, Type serviceType, System.Exception innerException )
            : base( message, innerException )
        {
            Contract.Requires<ArgumentNullException>( serviceType != null );

            this.serviceType = serviceType;
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="ServiceNotFoundException"/> class and sets
        /// the error message to the specified <see cref="System.String"/>.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        public ServiceNotFoundException( string message )
            : base( message )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceNotFoundException"/> class 
        /// with a specified error message and a reference 
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        /// <param name="innerException"> The exception that is cause of the new <see cref="ServiceNotFoundException"/>. </param>
        public ServiceNotFoundException( string message, System.Exception innerException )
            : base( message, innerException )
        {
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="ServiceNotFoundException"/> class, and
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
        protected ServiceNotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
            : base( info, context )
        {
            this.serviceType = info.GetValue( "ServiceType", typeof( Type ) ) as Type;
        }

        /// <summary>
        /// When overridden in a derived class, sets the System.Runtime.Serialization.SerializationInfo
        /// with information about the exception.
        /// </summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized
        /// object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual
        /// information about the source or destination.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// The info parameter is a null reference (Nothing in Visual Basic).
        /// </exception>
        [System.Security.SecurityCritical]
        public override void GetObjectData(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        {
            base.GetObjectData( info, context );

            info.AddValue( "ServiceType", this.serviceType );
        }

        /// <summary>
        /// Gets the message to show for the given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">
        /// The type of the service that was not found.
        /// </param>
        /// <returns>
        /// A human-readable error message string.
        /// </returns>
        private static string GetMessage( Type serviceType )
        {
            return string.Format(
                System.Globalization.CultureInfo.CurrentCulture,
                ErrorStrings.ServiceXNotFound,
                serviceType != null ? serviceType.ToString() : string.Empty
            );               
        }

        /// <summary>
        /// Represents the storage field of the <see cref="ServiceType"/> property.
        /// </summary>
        private readonly Type serviceType;
    }
}
