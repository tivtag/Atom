// <copyright file="IFileHasher.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.IFileHasher interface.</summary>
// <author>Paul Ennemoser</author>

namespace Atom.AutoUpdate
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// Provides a mechanism that is used to calculate
    /// the hash code of a file.
    /// </summary>
    public interface IFileHasher
    {        
        /// <summary>
        /// Calculates the hash-code for the specified Stream of bytes.
        /// </summary>
        /// <param name="stream">
        /// The input to calculate the hash for.
        /// </param>
        /// <returns>
        /// The calculated hash.
        /// </returns>
        string GetHash( Stream stream );

        /// <summary>
        /// Calculates the hash-code for the specified bytes.
        /// </summary>
        /// <param name="bytes">
        /// The input to calculate the hash for.
        /// </param>
        /// <returns>
        /// The calculated hash.
        /// </returns>
        string GetHash( byte[] bytes );
    }
}
