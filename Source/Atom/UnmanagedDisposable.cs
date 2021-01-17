// <copyright file="UnmanagedDisposable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.UnmanagedDisposable class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    /// <summary>
    /// Represents an abstract base class that implements the IDisposable pattern;
    /// resources are disposed automatically incase the user has not called Dipose manually.
    /// </summary>
    /// <remarks>
    /// Warning: Implementing this class over ManagedDisposable brings large perfomance penalities
    /// because of finalization.
    /// </remarks>
    public abstract class UnmanagedDisposable : ManagedDisposable
    {
        /// <summary>
        /// Finalizes an instance of the UnmanagedDisposable class.
        /// </summary>
        ~UnmanagedDisposable()
        {
            this.Dispose( false );
        }
    }
}
