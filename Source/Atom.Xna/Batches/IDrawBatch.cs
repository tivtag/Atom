// <copyright file="IDrawBatch.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Batches.IDrawBatch interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Batches
{
    /// <summary>
    /// Provides a mechanism that allows queueing of objects to draw
    /// and then later to output them.
    /// </summary>
    public interface IDrawBatch
    {
        /// <summary>
        /// Begins drawing to this IDrawBatch.
        /// </summary>
        /// <param name="drawContext">
        /// The current <see cref="IXnaDrawContext"/>.
        /// </param>
        void Begin( IXnaDrawContext drawContext );

        /// <summary>
        /// Ends drawing to this IDrawBatch, outputing the result.
        /// </summary>
        void End();
    }
}
