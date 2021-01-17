// <copyright file="IMailMessage.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Mail.IMailMessage interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Mail
{
    using  System.Collections.Generic;

    /// <summary>
    /// Represents a message that can be send using an <see cref="IMailSender"/>.
    /// </summary>
    public interface IMailMessage
    {
        /// <summary>
        /// Gets the list of recipients (To) of this IMailMessage.
        /// </summary>
        IList<string> Recipients
        {
            get;
        }

        /// <summary>
        /// Gets the list of carbon copy recipients (CC) of this IMailMessage.
        /// </summary>
        IList<string> CarbonCopyRecipients
        {
            get;
        }

        /// <summary>
        /// Gets the list of blind carbon copy recipients (BCC) of this IMailMessage.
        /// </summary>
        IList<string> BlindCarbonCopyRecipients
        {
            get;
        }

        /// <summary>
        /// Gets the list that contains the full paths of the files that are attached to this IMailMessage.
        /// </summary>
        IList<string> Attachments
        {
            get;
        }

        /// <summary>
        /// Gets or sets the subject of this IMailMessage.
        /// </summary>
        string Subject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the actual message this IMailMessage contains.
        /// </summary>
        string Body
        {
            get;
            set;
        }
    }
}
