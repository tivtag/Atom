// <copyright file="MailToMailSender.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Mail.MailTo.MailToMailSender class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Mail.MailTo
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Implements an <see cref="IMailSender"/> that uses the mailto: command to send the e-mail.
    /// </summary>
    /// <remarks>
    /// The MailToMailSender can't send attachments or messages over 1000 characters.
    /// Only one person can receive the e-mail.
    /// </remarks>
    public sealed class MailToMailSender : IMailSender
    {
        /// <summary>
        /// The string that is used to represent a new-line.
        /// </summary>
        private const string NewLineString = "%0A";

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
            if( mail.Recipients.Count == 0 )
            {
                throw new ArgumentException( "Atleast one Recipients recipient must be specified.", "mail" );
            }

            string command = string.Format(
                CultureInfo.InvariantCulture,
                "mailto:{0}?subject={1}&body={2}",
                mail.Recipients[0],
                mail.Subject,
                mail.Body.Replace( "\n", NewLineString )
            );

            Process.Start( command );
        }
    }
}
