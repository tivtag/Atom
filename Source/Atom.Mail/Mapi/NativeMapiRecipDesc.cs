// <copyright file="NativeMapiRecipDesc.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Mail.Mapi.NativeMapiRecipDesc class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Mail.Mapi
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout( LayoutKind.Sequential, CharSet=CharSet.Ansi )]
    internal sealed class NativeMapiRecipDesc
    {
        public int reserved;
        public int recipClass;
        public string name;
        public string address;
        public int eIDSize;
        public IntPtr entryID;
    }
}
