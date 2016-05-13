// <copyright file="ThrowHelper.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ThrowHelper class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Defines extension methods for the <see cref="Exception"/> class.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Preserves the stack trace of the given Exception;
        /// even if it is rethrown.
        /// </summary>
        /// <param name="ex">
        /// The exception which stack trace should be preserved.
        /// </param>
        public static void PreserveStackTrace( this Exception ex )
        {
            InternalPreserveStackTrace( ex );
        }

        /// <summary>
        /// Uses reflection to return the internal InternalPreserveStackTrace method of the Exception class.
        /// </summary>
        /// <returns>
        /// The reflected method.
        /// </returns>
        private static Action<Exception> GetInternalPreserveStackTraceMethod()
        {
            MethodInfo preserveStackTrace = typeof( Exception ).GetMethod(
                "InternalPreserveStackTrace",
                BindingFlags.Instance | BindingFlags.NonPublic
            );

            return (Action<Exception>)Delegate.CreateDelegate( typeof( Action<Exception> ), preserveStackTrace );
        }

        /// <summary>
        /// An action delegate that caches the internal InternalPreserveStackTrace method of the Exception class.
        /// </summary>
        private static readonly Action<Exception> InternalPreserveStackTrace = GetInternalPreserveStackTraceMethod();
    }
}