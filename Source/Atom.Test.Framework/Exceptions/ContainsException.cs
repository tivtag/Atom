// <copyright file="ContainsException.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ContainsException class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    /// <summary>
    /// Exception thrown when a collection unexpectedly does not contain the expected value.
    /// </summary>
    public class ContainsException : Microsoft.VisualStudio.TestTools.UnitTesting.UnitTestAssertException
    {
       /// <summary>
        /// Creates a new instance of the <see cref="ContainsException"/> class.
        /// </summary>
        /// <param name="expected">The expected object value</param>
        public ContainsException(object expected)
            : base(string.Format("CustomAssert.Contains() failure: Not found: {0}", expected)) 
        {
        }
    }
}
