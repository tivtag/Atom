// <copyright file="ThrowsException.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ThrowsException class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Exception thrown when code unexpectedly fails to throw an exception.
    /// </summary>
    public class ThrowsException : Microsoft.VisualStudio.TestTools.UnitTesting.UnitTestAssertException
    {
        /// <summary>
        /// Initializes a new  instance of the ThrowsException class.
        /// </summary>
        internal ThrowsException()
            : base( "Expected an exception to be thrown. But no exception was thrown!" )
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ThrowsException"/> class. Call this constructor
        /// when no exception was thrown.
        /// </summary>
        /// <param name="expectedType">The type of the exception that was expected</param>
        internal ThrowsException( Type expectedType )
            : base( string.Format( "Expected an exception of type {0} to be thrown. But no exception was thrown!", expectedType ) )
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ThrowsException"/> class. Call this constructor
        /// when an exception of the wrong type was thrown.
        /// </summary>
        /// <param name="expectedType">The type of the exception that was expected</param>
        /// <param name="actual">The actual exception that was thrown</param>
        internal ThrowsException( Type expectedType, Exception actual )
            : base( GetMessage( expectedType, actual.GetType(), actual.Message, actual.StackTrace ) )
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expectedType"></param>
        /// <param name="actualType"></param>
        /// <param name="actualMessage"></param>
        /// <param name="stackTrace"></param>
        /// <returns></returns>
        private static string GetMessage( Type expectedType, Type actualType, string actualMessage, string stackTrace )
        {
            return string.Format(
                @"Expected an exception of type '{0}' but actually one of type '{1}' was thrown.
                 \n\nException Message:\n{2}\n\n\nException Stack Trace:\n{3}",
                expectedType.ToString(),
                actualType.ToString(),
                actualMessage ?? string.Empty,
                stackTrace ?? string.Empty
            );
        }
    }
}
