// <copyright file="LightningSubdivisionOp.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Effects.Lightning.LightningSubdivisionOp class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Effects.Lightning
{
    /// <summary>
    /// Describes the operation that will be applied to the lightning segments
    /// </summary>
    public enum LightningSubdivisionOp
    {
        /// <summary>
        /// Take a point on the line and modify its position
        /// </summary>
        Jitter,

        /// <summary>
        /// Take a point on the line, modify it's position, and 
        /// generate a new segment starting in this point
        /// </summary>
        JitterAndFork
    }
}
