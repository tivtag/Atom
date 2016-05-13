// <copyright file="IOUtilities.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.IOUtilities class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;

    /// <summary>
    /// Defines static I/O-related utility methods.
    /// </summary>
    public static class IOUtilities
    {    
        /// <summary>
        /// Copies the complete content of the given <paramref name="input"/> <see cref="Stream"/>
        /// into the given <paramref name="output"/> <see cref="Stream"/>.
        /// </summary>
        /// <param name="input">
        /// The source stream.
        /// </param>
        /// <param name="output">
        /// The target stream.
        /// </param>
        public static void CopyStream( Stream input, Stream output )
        {
            Contract.Requires<ArgumentNullException>( input != null );
            Contract.Requires<ArgumentNullException>( output != null );
            
            long initialPosition = input.Position;
            input.Position = 0;

            input.CopyTo( output );
            input.Position = initialPosition;
        }
    }
}
