// <copyright file="Timeout.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the  Atom.Threading.Timeout class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Threading
{
    using System;
    using System.Threading;

    /// <summary>
    /// Provides utility mechanism for working with timeoutable actions.
    /// </summary>
    public static class Timeout
    {
        /// <summary>
        /// Executes the specified action, giving it the specified time to complete
        /// before it is aborted.
        /// </summary>
        /// <exception cref="TimeoutException">
        /// Thrown when the timeout has been reached.
        /// </exception>
        /// <param name="timeout">
        /// The TimeSpan the action is allowed to take at max.
        /// </param>
        /// <param name="action">
        /// The action to execute.
        /// </param>
        /// <param name="abort">
        /// The (optional) action that should be executed when the timeout has been reached.
        /// </param>
        public static void Invoke( TimeSpan timeout, Action action, Action abort = null )
        {
            Thread threadToKill = null;
            Action wrappedAction = () => {
                threadToKill = Thread.CurrentThread;
                action();
            };

            IAsyncResult result = wrappedAction.BeginInvoke( null, null );
            if( result.AsyncWaitHandle.WaitOne( timeout, true ) )
            {
                wrappedAction.EndInvoke( result );
            }
            else
            {
                if( threadToKill != null )
                {
                    try { threadToKill.Abort(); }
                    catch { /* Ignore */ }
                }

                if( abort != null )
                    abort();

                throw new TimeoutException();
            }
        }
    }
}
