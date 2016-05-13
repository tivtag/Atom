// <copyright file="TestHelper.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.TestHelper class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Contains test related utility methods.
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Gets whether the specified Action has thrown
        /// an exception of the specified type.
        /// </summary>
        /// <typeparam name="TException">
        /// The type of the exception to expect.
        /// </typeparam>
        /// <param name="action">
        /// The action to execute.
        /// </param>
        /// <returns>
        /// true if the exception was thrown;
        /// otherwise false.
        /// </returns>
        public static bool HasThrown<TException>( Action action )
            where TException : Exception
        {
            try
            {
                action();
                return false;
            }
            catch( TException )
            {
                return true;
            }
        }
    }
}
