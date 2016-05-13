// <copyright file="ManagedDisposable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ManagedDisposable class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System;
    using System.Threading;

    /// <summary>
    /// Represents an abstract base class that implements the IDisposable pattern.
    /// </summary>
    /// <seealso cref="UnmanagedDisposable"/>
    public abstract class ManagedDisposable : IIsDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IDisposable"/> object
        /// has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return this.disposeState == 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ManagedDisposable class.
        /// </summary>
        protected ManagedDisposable()
        {
        }

        /// <summary>
        /// Immediately releases all managed and unmanaged resources this IDisposable object
        /// has aquired.
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
            if( Interlocked.CompareExchange( ref this.disposeState, 1, 0 ) == 0 )
            {
                this.DisposeUnmanagedResources();

                if( releaseManaged )
                {
                    this.DisposeManagedResources();
                }
            }
        }

        /// <summary>
        /// Releases all managed resources.
        /// </summary>
        protected virtual void DisposeManagedResources()
        {
        }

        /// <summary>
        /// Releases all unmanaged resources. Umanaged resources are disposed before managed resources.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/> if this ManagedDisposable object has
        /// been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if( this.IsDisposed == true )
            {
                throw new ObjectDisposedException( this.GetType().FullName );
            }
        }

        /// <summary>
        /// Stores whether the resources of IDisposable object have been released.
        /// </summary>
        private int disposeState;
    }
}
