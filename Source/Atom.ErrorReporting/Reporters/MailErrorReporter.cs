// <copyright file="MailErrorReporter.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.Reporters.MailErrorReporter class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting.Reporters
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Atom.Mail;

    /// <summary>
    /// Implements an <see cref="IErrorReporter"/> that opens the default mail-client
    /// with the error message.
    /// </summary>
    public class MailErrorReporter : BaseMailErrorReporter
    {
        /// <summary>
        /// Initializes a new instance of the MailErrorReporter class.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the application.
        /// </param>
        /// <param name="recipient">
        /// The e-mail address to which mail should be send to.
        /// </param>
        /// <param name="bodyFormatter">
        /// The IErrorMessageFormatter responsible for creating the body text the e-mail will contain.
        /// </param>
        /// <param name="mailSender">
        /// The IMailSender that should be used to actually send the e-mail.
        /// </param>
        public MailErrorReporter( string applicationName, string recipient, IErrorMessageFormatter bodyFormatter, IMailSender mailSender )
            : base( applicationName, recipient, mailSender )
        {
            Contract.Requires<ArgumentNullException>( bodyFormatter != null );

            this.bodyFormatter = bodyFormatter;
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
        protected override string GetBody( IError error )
        {
            return this.bodyFormatter.Format( error );
        }

        /// <summary>
        /// The IErrorMessageFormatter responsible for creating the body text the e-mail will contain.
        /// </summary>
        private readonly IErrorMessageFormatter bodyFormatter;
    }
}
