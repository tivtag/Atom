// <copyright file="FileLog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.Contracts.Contract class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Allows checking of class contracts.
    /// </summary>
    /// <remarks>
    /// Quick implementation since I've had to remove the unsupported Microsoft Code Contracts.
    /// </remarks>
    public static class Contract
    {
        /// <summary>
        /// Checks whether the given obejct is not null.
        /// </summary>
        /// <param name="obj">
        /// The object that should not be null.
        /// </param>
        /// <param name="argumentName">
        /// The anme of the argument.
        /// </param>
        public static void NotNull<T>( T obj, string argumentName )
            where T : class
        {
            Requires<ArgumentNullException>( obj != null, $"{argumentName} must not be null." );
        }

        /// <summary>
        /// Checks whether the given condition is met.
        /// </summary>
        /// <param name="condition">
        /// The condition that must be true.
        /// </param>
        /// <param name="message">
        /// The message to output if the condition is not met. (optional)
        /// </param>
        public static void Requires( bool condition, string message = null )
        {
            Requires<ArgumentException>( condition, message );
        }

        /// <summary>
        /// Checks whether the given condition is met.
        /// </summary>
        /// <typeparam name="TException">
        /// The exception type to throw if the condition is not met.
        /// </typeparam>
        /// <param name="condition">
        /// The condition that must be true.
        /// </param>
        /// <param name="message">
        /// The message to output if the condition is not met. (optional)
        /// </param>
        public static void Requires<TException>( bool condition, string message = null )
            where TException : Exception, new()
        {
            if( !condition )
            {
                //https://stackoverflow.com/questions/41397/asking-a-generic-method-to-throw-specific-exception-type-on-fail/41450#41450
                var e = default( TException );
                try
                {
                    message = message ?? "Unexpected Condition"; // TODO consider to pass condition as lambda expression
                    e = Activator.CreateInstance( typeof( TException ), message ) as TException;
                }
                catch( MissingMethodException )
                {
                    e = new TException();
                }
                throw e;
            }
        }

        /// <summary>
        /// Checks whether the given condition is met for all item.
        /// </summary>
        public static bool ForAll<T>( IEnumerable<T> sequence, Func<T, bool> predicate )
        {
            return sequence.All( predicate );
        }

        /// <summary>
        /// Checks whether the given invariant condition is met.
        /// </summary>
        /// <param name="invariant">
        /// The invariant that must be true.
        /// </param>
        public static void Invariant( bool invariant )
        {
            Requires<Exception>( invariant );
        }
    }
}
