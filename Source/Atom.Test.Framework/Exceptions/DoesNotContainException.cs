// <copyright file="DoesNotContainException.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.DoesNotContainException class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    /// <summary>
    /// Exception thrown when a collection unexpectedly contains the expected value.
    /// </summary>
    public class DoesNotContainException : Microsoft.VisualStudio.TestTools.UnitTesting.UnitTestAssertException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DoesNotContainException"/> class.
        /// </summary>
        /// <param name="expected">The expected object value</param>
        public DoesNotContainException( object expected )
            : base( string.Format( "Assert.DoesNotContain() failure: Found: {0}", expected ) ) { }
    }
}
