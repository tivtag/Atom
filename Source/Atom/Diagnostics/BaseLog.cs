// <copyright file="BaseLog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Diagnostics.BaseLog class.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.Diagnostics
{
    using Atom.Diagnostics.Filters;

    /// <summary>
    /// Defines an abstract base implementation of the <see cref="ILog"/> interface.
    /// </summary>
    public abstract class BaseLog : ILog
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the ILogFilter that is used to filter the messages
        /// that are actually logged.
        /// </summary>
        public ILogFilter MessageFilter
        {
            get;
            set;
        }
    
        /// <summary>
        /// Gets the default <see cref="LogSeverities"/> messages are logged as.
        /// </summary>
        /// <value>
        /// The default value is <see cref="LogSeverities.Info"/>.
        /// </value>
        public virtual LogSeverities DefaultSeverity
        {
            get 
            {
                return LogSeverities.Info; 
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Initializes a new instance of the BaseLog class.
        /// </summary>
        protected BaseLog()
        {
        }

        /// <summary>
        /// Writes the given message to this BaseLog using the <see cref="DefaultSeverity"/>.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        public virtual void Write( string message )
        {
            this.Write( this.DefaultSeverity, message );
        }

        /// <summary>
        /// Writes the given message, followed by a new line, to 
        /// this BaseLog using the <see cref="DefaultSeverity"/>.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        public virtual void WriteLine( string message )
        {
            this.WriteLine( this.DefaultSeverity, message );
        }

        /// <summary>
        /// Writes a new empty line to this ILog with the <see cref="DefaultSeverity"/>.
        /// </summary>
        public virtual void WriteLine()
        {
            this.WriteLine( string.Empty );
        }

        /// <summary>
        /// Writes the given message to this BaseLog using
        /// the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        public virtual void Write( LogSeverities severity, string message )
        {
            if( this.IsNotLogged( severity, message ) )
                return;

            this.ActuallyWrite( severity, message );
        }

        /// <summary>
        /// Writes the given message, followed by a new line, to 
        /// this BaseLog using the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        public virtual void WriteLine( LogSeverities severity, string message )
        {
            if( this.IsNotLogged( severity, message ) )
                return;

            this.ActuallyWriteLine( severity, message );
        }

        /// <summary>
        /// Writes the given message to this BaseLog using
        /// the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <remarks>
        /// Messages that should not be logged have already been filtered by <see cref="IsNotLogged"/>
        /// when this method is called.
        /// </remarks>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        protected abstract void ActuallyWrite( LogSeverities severity, string message );

        /// <summary>
        /// Writes the given message, followed by a new line, to 
        /// this BaseLog using the given <see cref="LogSeverities"/>.
        /// </summary>
        /// <remarks>
        /// Messages that should not be logged have already been filtered by <see cref="IsNotLogged"/>
        /// when this method is called.
        /// </remarks>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the <paramref name="message"/>.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        protected abstract void ActuallyWriteLine( LogSeverities severity, string message );
        
        /// <summary>
        /// Gets a value indicating whether the message of the given <see cref="LogSeverities"/>
        /// is not logged by this BaseLog.
        /// </summary>
        /// <param name="severity">
        /// The <see cref="LogSeverities"/> of the message.
        /// </param>
        /// <param name="message">
        /// The message to filter.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the message is not supposed to be logged;
        /// or otherwise <see langword="false"/>.
        /// </returns>
        protected virtual bool IsNotLogged( LogSeverities severity, string message )
        {
            if( this.MessageFilter == null )
                return false;

            return this.MessageFilter.Filters( severity, message );
        }

        #endregion
    }
}
