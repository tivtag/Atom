// <copyright file="XnaSerializationContextExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.XnaSerializationContextExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using Atom.Storage;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines extension methods that allow serialization of Xna types using the <see cref="ISerializationContext"/> class.
    /// </summary>
    public static class XnaSerializationContextExtensions
    {
        /// <summary>
        /// Writes the given Color value.
        /// </summary>
        /// <param name="context">
        /// The context to write with.
        /// </param>
        /// <param name="color">
        /// The value to write.
        /// </param>
        public static void Write( this ISerializationContext context, Color color )
        {
            context.Write( color.R );
            context.Write( color.G );
            context.Write( color.B );
            context.Write( color.A );
        }
    }
}
