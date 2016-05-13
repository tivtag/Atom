// <copyright file="WinFormsShutdownLogic.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ErrorReporting.WinFormsShutdownLogic class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.ErrorReporting
{
    using System.Windows.Forms;

    /// <summary>
    /// Implements a mechanism for shutting down a Win Forms application after
    /// a fatal error. This class can't be inherited.
    /// </summary>
    public sealed class WinFormsShutdownLogic : IShutdownLogic
    {
        /// <summary>
        /// Executes this WinFormsShutdownLogic.
        /// </summary>
        public void DoShutdown()
        {
            Application.Exit();
        }
    }
}
