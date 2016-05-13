// <copyright file="MD5FileHasher.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.AutoUpdate.MD5FileHasher class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.AutoUpdate
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Globalization;

    /// <summary>
    /// Implements a mechanism that is used to calculate
    /// the hash code of a file using the MD5 algorithm.
    /// </summary>
    public sealed class MD5FileHasher : IFileHasher
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
        public string GetHash( Stream stream )
        {
            byte[] buffer = new byte[(int)stream.Length];

            stream.Position = 0;
            stream.Read( buffer, 0, buffer.Length );

            return this.GetHash( buffer );
        }

        /// <summary>
        /// Calculates the hash-code for the specified bytes.
        /// </summary>
        /// <param name="bytes">
        /// The input to calculate the hash for.
        /// </param>
        /// <returns>
        /// The calculated hash.
        /// </returns>
        public string GetHash( byte[] bytes )
        {
            byte[] byteHash = this.md5.ComputeHash( bytes );
            return BitConverter.ToString( byteHash ).Replace( "-", "" ).ToLower( CultureInfo.InvariantCulture );
        }

        /// <summary>
        /// The MD5 hashing service provided by .net.
        /// </summary>
        private readonly MD5 md5 = new MD5CryptoServiceProvider();
    }
}
