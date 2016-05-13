// <copyright file="IMailSender.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Mail.IMailSender interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Mail
{
    /// <summary>
    /// Provides a mechanism for sending <see cref="IMailMessage"/>s.
    /// </summary>
    public interface IMailSender
    {
        /// <summary>
        /// Attempts to send the specified IMailMessage.
        /// </summary>
        /// <exception cref="MailException">
        /// Thrown when an error has occurred during the mail sending operation.
        /// </exception>
        /// <param name="mail">
        /// The mail message to send.
        /// </param>
        void Send( IMailMessage mail );
    }
}
