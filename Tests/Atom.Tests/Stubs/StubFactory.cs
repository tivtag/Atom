// <copyright file="StubFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.StubFactory class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom
{
    using System.IO;
    using System.IO.Moles;

    /// <summary>
    /// Defines helper functions that create stub objects.
    /// </summary>
    public static class StubFactory
    {
        /// <summary>
        /// Creates a SFileStream that re-directs calls to a MemoryStream.
        /// </summary>
        /// <param name="fileName">
        /// The file name to pass to the SFileStream.
        /// </param>
        /// <param name="fileMode">
        /// The file mode to pass to the SFileStream.
        /// </param>
        /// <param name="fileAccess">
        /// The file access to pass to the SFileStream.
        /// </param>
        /// <param name="memoryStream">
        /// The MemoryStream all calls should be re-directed to.
        /// </param>
        /// <returns>
        /// The newly created SFileStream instance.
        /// </returns>
        public static SFileStream CreateFileStream( string fileName, FileMode fileMode, FileAccess fileAccess, MemoryStream memoryStream )
        {
            return new SFileStream( fileName, fileMode, fileAccess ) {
                LengthGet = () => memoryStream.Length,
                SetLengthInt64 = length => memoryStream.SetLength( length ),
                ReadTimeoutGet = () => memoryStream.ReadTimeout,
                ReadByteArrayInt32Int32 = ( buffer, offset, count ) => memoryStream.Read( buffer, offset, count ),
                ReadTimeoutSetInt32 = value => memoryStream.ReadTimeout = value,
                SeekInt64SeekOrigin = ( offset, loc ) => memoryStream.Seek( offset, loc ),
                WriteTimeoutGet = () => memoryStream.WriteTimeout,
                WriteTimeoutSetInt32 = timeout => memoryStream.WriteTimeout = timeout,
                FlushBoolean = boolean => memoryStream.Flush(),
                IsAsyncGet = () => false,
                CanReadGet = () => memoryStream.CanRead,
                CanSeekGet = () => memoryStream.CanSeek,
                CanWriteGet = () => memoryStream.CanWrite,
                CanTimeoutGet = () => memoryStream.CanTimeout,
                PositionGet = () => memoryStream.Position,
                PositionSetInt64 = value => memoryStream.Position = value,
                WriteByteByte = value => memoryStream.WriteByte( value ),
                WriteByteArrayInt32Int32 = ( buffer, offset, count ) => memoryStream.Write( buffer, offset, count ),
                ReadByte01 = () => memoryStream.ReadByte(),
                Close01 = () => { },
                Flush01 = () => memoryStream.Flush(),
                DisposeBoolean = disposeManaged => { }
            };
        }
        /// <summary>
        /// Creates a SFileStream that re-directs calls to a MemoryStream.
        /// </summary>
        /// <param name="fileName">
        /// The file name to pass to the SFileStream.
        /// </param>
        public static SFileStream CreateFileStream( string fileName )
        {
            return CreateFileStream( fileName, FileMode.Create, FileAccess.Write, new MemoryStream() );
        }
    }
}
