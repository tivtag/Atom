// <copyright file="XnaDeserializationContextExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.XnaDeserializationContextExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using Atom.Storage;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines extension methods that allow deserialization of Xna types using the <see cref="ISerializationContext"/> class.
    /// </summary>
    public static class XnaDeserializationContextExtensions
    {
        /// <summary>
        /// Reads a <see cref="Color"/> value.
        /// </summary>
        /// <param name="context">
        /// The context to read with.
        /// </param>
        /// <returns>
        /// The Color value that has been read.
        /// </returns>
        public static Color ReadColor( this IDeserializationContext context )
        {
            byte red = context.ReadByte();
            byte green = context.ReadByte();
            byte blue = context.ReadByte();
            byte alpha = context.ReadByte();
            
            return new Color( red, green, blue, alpha );
        }
    }
}
