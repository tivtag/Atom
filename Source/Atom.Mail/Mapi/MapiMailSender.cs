// <copyright file="MapiMailSender.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Mail.Mapi.MapiMailSender class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Mail.Mapi
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Implements an <see cref="IMailSender"/> that uses the windows Messaging Application Programming Interface (MAPI)
    /// to send <see cref="IMailMessage"/>s.
    /// This class can't be inherited.
    /// </summary>
    /// <remarks>
    /// See also: http://de.wikipedia.org/wiki/Messaging_Application_Programming_Interface
    /// </remarks>
    public sealed class MapiMailSender : IMailSender
    {
        /// <summary>
        /// Attempts to send the specified IMailMessage using MAPI.
        /// </summary>
        /// <exception cref="MailException">
        /// Thrown when an error has occurred during the mail sending operation.
        /// </exception>
        /// <param name="mail">
        /// The mail message to send.
        /// </param>
        public void Send( IMailMessage mail )
        {
            int result = SendNative( mail, MAPI_DIALOG );

            if( result != 0 )
            {
                throw new MailException( GetErrorMessage( result ) );
            }
        }

        /// <summary>
        /// Attempts to send the specified IMailMessage using MAPI,
        /// not allowing the user to manipulate or view the message.
        /// </summary>
        /// <exception cref="MailException">
        /// Thrown when an error has occurred during the mail sending operation.
        /// </exception>
        /// <param name="mail">
        /// The mail message to send.
        /// </param>
        public void SendDirect( IMailMessage mail )
        {
            int result = SendNative( mail, MAPI_LOGON_UI );

            if( result != 0 )
            {
                throw new MailException( GetErrorMessage( result ) );
            }
        }

        /// <summary>
        /// Implements the native mail sending logic.
        /// </summary>
        /// <param name="mail">
        /// The mail message to send.
        /// </param>
        /// <param name="how">
        /// Specifies how the IMailMessage should be send.
        /// </param>
        /// <returns>
        /// The native errror code.
        /// </returns>
        private static int SendNative( IMailMessage mail, int how )
        {
            var message = new NativeMapiMessage() {
                subject = mail.Subject,
                noteText = mail.Body,
                files = GetNativeAttachments( mail ),
                fileCount = mail.Attachments.Count
            };

            var recipients = BuildNativeRecipients( mail );
            message.recips = GetNativeRecipientsPointer( recipients );
            message.recipCount = recipients.Count;

            int result = NativeMethods.MAPISendMail( new IntPtr( 0 ), new IntPtr( 0 ), message, how, 0 );
            Cleanup( ref message );
            return result;
        }
        
        /// <summary>
        /// Builds the list of <see cref="NativeMapiRecipDesc"/> for the specified IMailMessage.
        /// </summary>
        /// <param name="mail">
        /// The input mail message.
        /// </param>
        /// <returns>
        /// The corresponding native MAPI list of recipients of the specified IMailMessage.
        /// </returns>
        private static IList<NativeMapiRecipDesc> BuildNativeRecipients( IMailMessage mail )
        {
            var recipients = new List<NativeMapiRecipDesc>();

            foreach( var email in mail.Recipients )
            {
                var nativeRecipient = new NativeMapiRecipDesc()
                {
                    recipClass = (int)MapiRecipientType.MAPI_TO,
                    name = email
                };

                recipients.Add( nativeRecipient );
            }

            foreach( var email in mail.CarbonCopyRecipients )
            {
                var nativeRecipient = new NativeMapiRecipDesc()
                {
                    recipClass = (int)MapiRecipientType.MAPI_CC,
                    name = email
                };

                recipients.Add( nativeRecipient );
            }

            foreach( var email in mail.BlindCarbonCopyRecipients )
            {
                var nativeRecipient = new NativeMapiRecipDesc()
                {
                    recipClass = (int)MapiRecipientType.MAPI_BCC,
                    name = email
                };

                recipients.Add( nativeRecipient );
            }

            return recipients;
        }

        /// <summary>
        /// Gets the pointer send to MAPI that contains the specified NativeMapiRecipDescs.
        /// </summary>
        /// <param name="recipients">
        /// The recipients the mail is send to.
        /// </param>
        /// <returns>
        /// A pointer used by MAPI.
        /// </returns>
        private static IntPtr GetNativeRecipientsPointer( IList<NativeMapiRecipDesc> recipients )
        {
            if( recipients.Count == 0 )
                return IntPtr.Zero;

            int size = Marshal.SizeOf( typeof( NativeMapiRecipDesc ) );
            IntPtr intPtr = Marshal.AllocHGlobal( recipients.Count * size );

            int ptr = (int)intPtr;
            foreach( NativeMapiRecipDesc mapiDesc in recipients )
            {
                Marshal.StructureToPtr( mapiDesc, (IntPtr)ptr, false );
                ptr += size;
            }

            return intPtr;
        }

        /// <summary>
        /// Gets the pointer to a structure containing information about the attachments of this IMailMessage.
        /// </summary>
        /// <param name="mail">
        /// The mail message to send.
        /// </param>
        /// <returns>
        /// A pointer used by MAPI.
        /// </returns>
        private static IntPtr GetNativeAttachments( IMailMessage mail )
        {
            const int MaximumAttachments = 20;
            if( mail.Attachments.Count == 0 || mail.Attachments.Count >  MaximumAttachments )
                return IntPtr.Zero;

            int size = Marshal.SizeOf( typeof( NativeMapiFileDesc ) );
            IntPtr intPtr = Marshal.AllocHGlobal( mail.Attachments.Count * size );

            NativeMapiFileDesc mapiFileDesc = new NativeMapiFileDesc();
            mapiFileDesc.position = -1;
            int ptr = (int)intPtr;

            foreach( string strAttachment in mail.Attachments )
            {
                mapiFileDesc.name = Path.GetFileName( strAttachment );
                mapiFileDesc.path = strAttachment;

                Marshal.StructureToPtr( mapiFileDesc, (IntPtr)ptr, false );
                ptr += size;
            }

            return intPtr;
        }

        /// <summary>
        /// Cleans up the specified NativeMapiMessage by de-allocating memory.
        /// </summary>
        /// <param name="nativeMessage">
        /// The native message to dispose.
        /// </param>
        private static void Cleanup( ref NativeMapiMessage nativeMessage )
        {
            int size = Marshal.SizeOf( typeof( NativeMapiRecipDesc ) );
            int ptr = 0;

            if( nativeMessage.recips != IntPtr.Zero )
            {
                ptr = (int)nativeMessage.recips;

                for( int i = 0; i < nativeMessage.recipCount; ++i )
                {
                    Marshal.DestroyStructure( (IntPtr)ptr, typeof( NativeMapiRecipDesc ) );
                    ptr += size;
                }

                Marshal.FreeHGlobal( nativeMessage.recips );
            }

            if( nativeMessage.files != IntPtr.Zero )
            {
                size = Marshal.SizeOf( typeof( NativeMapiFileDesc ) );
                ptr = (int)nativeMessage.files;

                for( int i = 0; i < nativeMessage.fileCount; ++i )
                {
                    Marshal.DestroyStructure( (IntPtr)ptr, typeof( NativeMapiFileDesc ) );
                    ptr += size;
                }

                Marshal.FreeHGlobal( nativeMessage.files );
            }
        }
        
        /// <summary>
        /// Gets the error message that responds to the specified errorNumber.
        /// </summary>
        /// <param name="errorNumber">
        /// The native error number returned by MAPI.
        /// </param>
        /// <returns>
        /// The corresponding error message.
        /// </returns>
        private static string GetErrorMessage( int errorNumber )
        {
            if( errorNumber >= 0 && errorNumber < ErrorMessages.Length )
            {
                return ErrorMessages[errorNumber];
            }

            return "MAPI error [" + errorNumber.ToString( CultureInfo.InvariantCulture ) + "]";
        }

        /// <summary>
        /// Enumerates the various MAPI error messages.
        /// </summary>
        private static readonly string[] ErrorMessages = new string[] {
            "OK [0]",
            "User abort [1]",
            "General MAPI failure [2]", 
            "MAPI login failure [3]", 
            "Disk full [4]", 
            "Insufficient memory [5]", 
            "Access denied [6]", 
            "-unknown- [7]", 
            "Too many sessions [8]", 
            "Too many files were specified [9]", 
            "Too many recipients were specified [10]", 
            "A specified attachment was not found [11]",
            "Attachment open failure [12]", 
            "Attachment write failure [13]",
            "Unknown recipient [14]", 
            "Bad recipient type [15]", 
            "No messages [16]", 
            "Invalid message [17]",
            "Text too large [18]", 
            "Invalid session [19]",
            "Type not supported [20]", 
            "A recipient was specified ambiguously [21]", 
            "Message in use [22]", 
            "Network failure [23]",
            "Invalid edit fields [24]", 
            "Invalid recipients [25]", 
            "Not supported [26]" 
        };

        /// <summary> </summary>
        private const int MAPI_LOGON_UI = 0x00000001;

        /// <summary> </summary>
        private const int MAPI_DIALOG = 0x00000008;

        /// <summary>
        /// Enumerates the various recipient types MAPI supports.
        /// </summary>
        private enum MapiRecipientType 
        {
            MAPI_ORIG = 0, 
            MAPI_TO,
            MAPI_CC, 
            MAPI_BCC 
        }

        /// <summary>
        /// Contains the native MAPI bindings.
        /// </summary>
        private static class NativeMethods
        {
            [DllImport( "MAPI32.DLL", ThrowOnUnmappableChar=true, BestFitMapping=false )]
            public static extern int MAPISendMail( IntPtr sess, IntPtr hwnd, NativeMapiMessage message, int flg, int rsv );
        }
    }
}
