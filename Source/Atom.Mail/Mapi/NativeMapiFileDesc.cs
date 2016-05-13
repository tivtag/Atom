// <copyright file="NativeMapiFileDesc.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Mail.Mapi.NativeMapiFileDesc class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Mail.Mapi
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout( LayoutKind.Sequential, CharSet = CharSet.Ansi )]
    internal sealed class NativeMapiFileDesc
    {
        public int reserved;
        public int flags;
        public int position;
        public string path;
        public string name;
        public IntPtr type;
    }
}
