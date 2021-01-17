// <copyright file="MailMessage.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Mail.MailMessage class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Mail
{
    using System.Collections.Generic;
    using Atom.Collections;

    /// <summary>
    /// Represents a message that can be send using an <see cref="IMailSender"/>.
    /// This class can't be inherited.
    /// </summary>
    public sealed class MailMessage : IMailMessage
    {
        /// <summary>
        /// Gets the list of recipients of this MailMessage.
        /// </summary>
        public IList<string> Recipients
        {
            get
            {
                return this.to; 
            }
        }

        /// <summary>
        /// Gets the list of carbon copy recipients of this MailMessage.
        /// </summary>
        public IList<string> CarbonCopyRecipients
        {
            get
            {
                return this.cc;
            }
        }

        /// <summary>
        /// Gets the list of blind carbon copy recipients of this MailMessage.
        /// </summary>
        public IList<string> BlindCarbonCopyRecipients
        {
            get
            {
                return this.bcc;
            }
        }

        /// <summary>
        /// Gets the list that contains the full paths of the files that are attached to this MailMessage.
        /// </summary>
        public IList<string> Attachments
        {
            get
            {
                return this.attachments;
            }
        }

        /// <summary>
        /// Gets or sets the subject of this MailMessage.
        /// </summary>
        public string Subject
        {
            get
            {
                return this.subject;
            }

            set
            {
                if( value == null )
                {
                    value = string.Empty;
                }

                this.subject = value;
            }
        }

        /// <summary>
        /// Gets or sets the actual message this MailMessage contains.
        /// </summary>
        public string Body
        {
            get
            {
                return this.body;
            }

            set
            {
                if( value == null )
                {
                    value = string.Empty;
                }

                this.body = value;
            }
        }

        /// <summary>
        /// The subject of this MailMessage.
        /// </summary>
        private string subject;

        /// <summary>
        /// The body of this MailMessage.
        /// </summary>
        private string body;

        /// <summary>
        /// The list of recipients of this MailMessage.
        /// </summary>
        private readonly NonNullList<string> to = new NonNullList<string>();

        /// <summary>
        /// The list of carbon copy recipients of this MailMessage.
        /// </summary>
        private readonly NonNullList<string> cc = new NonNullList<string>();

        /// <summary>
        /// The list of blind carbon copy recipients of this MailMessage.
        /// </summary>
        private readonly NonNullList<string> bcc = new NonNullList<string>();

        /// <summary>
        /// The full paths of the files that are attached to this MailMessage.
        /// </summary>
        private readonly NonNullList<string> attachments = new NonNullList<string>();
    }
}
