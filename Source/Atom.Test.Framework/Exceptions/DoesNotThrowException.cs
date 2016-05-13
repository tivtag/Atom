// <copyright file="DoesNotThrowException.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.DoesNotThrowException class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Exception thrown when code unexpectedly throws an exception.
    /// </summary>
    public class DoesNotThrowException : Microsoft.VisualStudio.TestTools.UnitTesting.UnitTestAssertException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DoesNotThrowException"/> class.
        /// </summary>
        /// <param name="actual">Actual exception</param>
        internal DoesNotThrowException( Exception actual )
            : base( string.Format( "An exception was unexpectedly thrown.\n\n{0}", actual.ToString() ) )
        {
        }
    }
}
