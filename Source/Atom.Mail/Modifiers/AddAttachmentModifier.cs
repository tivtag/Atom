// <copyright file="AddAttachmentModifier.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Mail.Modifiers.AddAttachmentModifier class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Mail.Modifiers
{
    using System.IO;

    /// <summary>
    /// Implements an <see cref="IMailMessageModifier"/> that attaches a file to the <see cref="IMailMessage"/>.
    /// </summary>
    public sealed class AddAttachmentModifier : IMailMessageModifier
    {
        /// <summary>
        /// Gets or sets the full path of the file to attach to the <see cref="IMailMessage"/>.
        /// </summary>
        /// <value>
        /// The default value is null.
        /// </value>
        public string FilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Modifies the specified IMailMessage.
        /// </summary>
        /// <param name="mail">
        /// The mail to modify.
        /// </param>
        public void Apply( IMailMessage mail )
        {
            if( File.Exists( this.FilePath ) )
            {
                mail.Attachments.Add( this.FilePath );
            }
        }
    }
}
