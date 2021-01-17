// <copyright file="FileLog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.FileLog class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Diagnostics
{
    using System.IO;

    /// <summary>
    /// Provides a mechanism to log string messages into a text file.
    /// This class can't be inherited.
    /// </summary>
    public sealed class FileLog : TextWriterLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileLog"/> class,
        /// using <see cref="System.Text.Encoding.Default"/>. 
        /// </summary>
        /// <remarks>
        /// If there is already a file at the specified <paramref name="name"/> the file will be 
        /// overriden if it is not readonly. Other processes are allowed to open the file of this
        /// <see cref="FileLog"/> to read its content at any time. The file uses <see cref="System.Text.Encoding.Default"/>.
        /// </remarks>
        /// <param name="name"> 
        /// The name of the file to write. Can be an actual path such as
        /// @"Misc/MyLog.log" or just a file-name: "MyLog.log".
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// If the specified <paramref name="name"/> is null.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// The specified path is invalid, (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.NotSupportedException"> <paramref name="name"/> is in an invalid format. </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="name"/> is a zero-length string, contains only white space,
        /// or contains one or more invalid characters as defined by <see cref="System.IO.Path.InvalidPathChars"/>.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, 
        /// and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// path specified a file that is read-only.-or- This operation is not supported on the current platform.
        /// -or- path specified a directory.-or- The caller does not have the required permission. 
        /// </exception>
        public FileLog( string name )
            : this( name, System.Text.Encoding.Default )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLog"/> class.
        /// </summary>
        /// <remarks>
        /// If there is already a file at the specified <paramref name="name"/> the file will be 
        /// overriden if it is not readonly. Other processes are allowed to open the file of this
        /// <see cref="FileLog"/> to read its content at any time. The file uses <see cref="System.Text.Encoding.Default"/>.
        /// </remarks>
        /// <param name="name"> 
        /// The name of the file to write. Can be an actual path such as
        /// @"Misc/MyLog.log" or just a file-name: "MyLog.log".
        /// </param>
        /// <param name="encoding">
        /// The encoding to use for the underlying file-stream.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// If the specified <paramref name="name"/> is null.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// The specified path is invalid, (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="System.NotSupportedException"> <paramref name="name"/> is in an invalid format. </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="name"/> is a zero-length string, contains only white space,
        /// or contains one or more invalid characters as defined by <see cref="System.IO.Path.InvalidPathChars"/>.
        /// </exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, 
        /// and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// path specified a file that is read-only.-or- This operation is not supported on the current platform.
        /// -or- path specified a directory.-or- The caller does not have the required permission. 
        /// </exception>
        public FileLog( string name, System.Text.Encoding encoding )
            : base( CreateTextWriter( name, encoding ?? System.Text.Encoding.Default ) )
        {
        }

        /// <summary>
        /// Creates a new <see cref="TextWriter"/> that is used to write to the underlying file.
        /// </summary>
        /// <param name="name"> 
        /// The name of the file to write to.
        /// </param>
        /// <param name="encoding">
        /// The encoding to use for the underlying file-stream.
        /// </param>
        /// <returns>
        /// The newly created TextWriter.
        /// </returns>
        private static TextWriter CreateTextWriter( string name, System.Text.Encoding encoding )
        {
            var stream = File.Open( name, FileMode.Create, FileAccess.Write, FileShare.Read );

            try
            {
                return new StreamWriter( stream, encoding ) {
                    AutoFlush = true
                };
            }
            catch
            {
                if( stream != null )
                {
                    stream.Dispose();
                }
                 
                throw;
            }
        }
    }
}