// <copyright file="TextWriterLog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.TextWriterLog class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.IO;

    /// <summary>
    /// Provides a mechanism to log string messages to a <see cref="TextWriter"/>.
    /// </summary>
    public class TextWriterLog : BaseLog, IDisposable
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the TextWriter into which the messages written to this TextWriterLog are re-directed.
        /// </summary>
        public TextWriter Writer
        {
            get
            {
                return this.writer;
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWriterLog"/> class.
        /// </summary>
        public TextWriterLog( TextWriter writer )
        {
            Contract.Requires<ArgumentNullException>( writer != null );

            this.writer = writer;
        }

        #endregion

        #region [ Deinitialization ]

        /// <summary>
        /// Finalizes an instance of the TextWriterLog class, releasing all unmanaged resources.
        /// </summary>
        ~TextWriterLog()
        {
            this.Dispose( false );
        }

        /// <summary>
        /// Releases all managed resources of this <see cref="TextWriterLog"/>. Further calls
        /// to methods of this object will result in an exception.
        /// </summary>
        public void Dispose()
        {
            this.Dispose( true );
            GC.SuppressFinalize( this );
        }
        
        /// <summary>
        /// Releases all resources this IDisposable object has aquired.
        /// </summary>
        /// <param name="releaseManaged">
        /// States whether managed resources should be disposed.
        /// </param>
        protected virtual void Dispose( bool releaseManaged )
        {
            try
            {
                if( this.writer != null )
                {
                    this.writer.Dispose();
                }
            }
            catch( ObjectDisposedException )
            {
                // Occurrs when the underlying stream has been
                // disposed by the GC. Just eat it.
            }
        }
        
        #endregion

        #region [ Methods ]

        /// <summary>
        /// Writes the given message to this TextWriterLog using
        /// the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        protected override void ActuallyWrite( LogSeverities severity, string message )
        {
            this.writer.Write( message );
        }

        /// <summary>
        /// Writes the given message, followed by a new line, to 
        /// this TextWriterLog using the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        protected override void ActuallyWriteLine( LogSeverities severity, string message )
        {
            this.writer.WriteLine( message );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The TextWriter into which the messages written to this TextWriterLog are re-directed.
        /// </summary>
        private readonly TextWriter writer;

        #endregion
    }
}
