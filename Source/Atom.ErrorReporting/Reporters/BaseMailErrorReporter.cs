// <copyright file="BaseMailErrorReporter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Reporters.BaseMailErrorReporter class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.ErrorReporting.Reporters
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.Globalization;
    using Atom.Mail;
    using Atom.Mail.Modifiers;

    /// <summary>
    /// Represents a base implementation of the <see cref="IErrorReporter"/> interface that
    /// reports errors by sending an e-mail.
    /// </summary>
    public abstract class BaseMailErrorReporter : IErrorReporter
    {
        /// <summary>
        /// Gets or sets the optional <see cref="IMailMessageModifier"/> that is applied to an
        /// <see cref="IMailMessage"/> before it is send.
        /// </summary>
        public IMailMessageModifier MailModifier
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the BaseMailErrorReporter class.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the application.
        /// </param>
        /// <param name="recipient">
        /// The e-mail address to which mail should be send to.
        /// </param>
        /// <param name="mailSender">
        /// The IMailSender that should be used to actually send the e-mail.
        /// </param>
        protected BaseMailErrorReporter( string applicationName, string recipient, IMailSender mailSender )
        {
            Contract.Requires<ArgumentNullException>( applicationName != null );
            Contract.Requires<ArgumentNullException>( recipient != null );
            Contract.Requires<ArgumentNullException>( mailSender != null );

            this.applicationName = applicationName;
            this.to = recipient;
            this.mailSender = mailSender;
        }

        /// <summary>
        /// Notifies this IErrorReporter that the specified IError has occurred.
        /// </summary>
        /// <param name="error">
        /// The error to report.
        /// </param>
        public void Report( IError error )
        {
            var mail = this.GetMailMessage( error );

            try
            {
                this.mailSender.Send( mail );
            }
            catch( MailException exc )
            {
                this.OnSendFailed( exc );
            }
        }

        /// <summary>
        /// Gets the <see cref="IMailMessage"/> this <see cref="BaseMailErrorReporter"/> will send
        /// in response to the specified <see cref="IError"/>.
        /// </summary>
        /// <param name="error">
        /// The error to report.
        /// </param>
        /// <returns>
        /// The ready-to-be send MailMessage.
        /// </returns>
        private IMailMessage GetMailMessage( IError error )
        {
            var mail = new MailMessage() {
                Subject = this.GetSubject( error ),
                Body = this.GetBody( error )
            };

            mail.Recipients.Add( this.GetTo( error ) );

            if( this.MailModifier != null )
            {
                this.MailModifier.Apply( mail );
            }

            return mail;
        }

        /// <summary>
        /// Gets the address to which the e-mail is send.
        /// </summary>
        /// <param name="error">
        /// The error that was reported.
        /// </param>
        /// <returns>
        /// The address to which the e-mail is send.
        /// </returns>
        protected virtual string GetTo( IError error )
        {
            return this.to;
        }

        /// <summary>
        /// Gets the subject of the e-mail.
        /// </summary>
        /// <param name="error">
        /// The error that was reported.
        /// </param>
        /// <returns>
        /// The subject of the e-mail.
        /// </returns>
        protected virtual string GetSubject( IError error )
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}: {1}",
                this.applicationName,
                error.Name
            );
        }

        /// <summary>
        /// Gets the body of the e-mail.
        /// </summary>
        /// <param name="error">
        /// The error that was reported.
        /// </param>
        /// <returns>
        /// The body of the e-mail.
        /// </returns>
        protected virtual string GetBody( IError error )
        {
            return error.Description;
        }

        /// <summary>
        /// Called when an exception has occurred while the mail was sent.
        /// </summary>
        /// <param name="exc">
        /// The exception that has occurred.
        /// </param>
        /// <exception cref="MailException">
        /// By default the exception is rethrown.
        /// </exception>
        protected virtual void OnSendFailed( MailException exc )
        {
            exc.PreserveStackTrace();
            throw exc;
        }

        /// <summary>
        /// The name of the application. Is shown in the e-mail subject.
        /// </summary>
        private readonly string applicationName;

        /// <summary>
        /// The e-mail address to which mail should be send to.
        /// </summary>
        private readonly string to;

        /// <summary>
        /// The IMailSender that should be used to actually send the e-mail.
        /// </summary>
        private readonly IMailSender mailSender;
    }
}
