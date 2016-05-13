// <copyright file="SafeMailSender.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines Atom.Mail.SafeMailSender class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Mail
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Implements a safe <see cref="IMailSender"/> that tries to send the e-mail using various different techniques.
    /// </summary>
    /// <remarks>
    /// By default if sending the mail failed using <see cref="Mapi.MapiMailSender"/>, the <see cref="MailTo.MailToMailSender"/> is used.
    /// </remarks>
    public sealed class SafeMailSender : IMailSender
    {
        /// <summary>
        /// Gets or sets a value indicating whether the SafeMailSender throws an MailException if an IMailMessage could
        /// not be send using any of techniques.
        /// </summary>
        /// <value>
        /// The default value is false.
        /// </value>
        public bool ThrowsOnError
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the SafeMailSender class;
        /// which uses the default IMailSenders <see cref="Mapi.MapiMailSender"/> and <see cref="MailTo.MailToMailSender"/>.
        /// </summary>
        public SafeMailSender()
        {
            this.senders = new IMailSender[] { 
                new Mapi.MapiMailSender(),
                new MailTo.MailToMailSender()
            };
        }
        
        /// <summary>
        /// Initializes a new instance of the SafeMailSender class.
        /// </summary>
        /// <param name="senders">
        /// The ordered list of IMailSenders the new SafeMailSender is going try to use.
        /// </param>
        public SafeMailSender( IEnumerable<IMailSender> senders )
        {
            Contract.Requires<ArgumentNullException>( senders != null );

            this.senders = senders.ToArray();
        }

        /// <summary>
        /// Attempts to send the specified IMailMessage.
        /// </summary>
        /// <exception cref="MailException">
        /// Thrown when an error has occurred during the mail sending operation.
        /// </exception>
        /// <param name="mail">
        /// The mail message to send.
        /// </param>
        public void Send( IMailMessage mail )
        {
            IList<Exception> exceptions = new List<Exception>();

            foreach( IMailSender sender in this.senders )
            {
                try
                {
                    sender.Send( mail );
                    return;
                }
                catch( StackOverflowException )
                {
                    throw;
                }
                catch( OutOfMemoryException )
                {
                    throw;
                }
                catch( Exception exc )
                {
                    exceptions.Add( exc );
                }                
            }

            if( this.ThrowsOnError )
            {
                throw new MailException(
                    "The mail message could not be send.",
                    new AggregateException( exceptions ) 
                );
            }
        }

        /// <summary>
        /// The MAPI mail sender that is prefered.
        /// </summary>
        private readonly IEnumerable<IMailSender> senders;
    }
}
