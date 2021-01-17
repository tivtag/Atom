// <copyright file="DrawBatchBase.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Batches.DrawBatchBase class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Batches
{
    using System;

    /// <summary>
    /// Represents an abstract base implementation of the <see cref="IDrawBatch"/> interface.
    /// </summary>
    public abstract class DrawBatchBase : IDrawBatch
    {
        /// <summary>
        /// Gets a value indicating stating whether this IDrawBatch is currently in a <see cref="Begin"/> <see cref="End"/> block.
        /// </summary>
        /// <value>
        /// true if this IDrawBatch is currently in a <see cref="Begin"/> <see cref="End"/> block;
        /// -or- otherwise false.
        /// </value>
        public bool IsInBlock
        {
            get;
            private set;
        }

        /// <summary>
        /// Begins drawing to this IDrawBatch.
        /// </summary>
        /// <param name="drawContext">
        /// The current <see cref="IXnaDrawContext"/>.
        /// </param>
        public void Begin( IXnaDrawContext drawContext )
        {
            if( this.IsInBlock )
            {
                throw new InvalidOperationException( "The draw batch is already with-in a Begin block." );
            }
            
            this.IsInBlock = true;

            try
            {
                this.BeginCore();
            }
            catch
            {
                this.IsInBlock = false;
                throw;
            }
        }

        /// <summary>
        /// Begins drawing to this IDrawBatch.
        /// </summary>
        protected abstract void BeginCore();

        /// <summary>
        /// Ends drawing to this IDrawBatch, outputing the result.
        /// </summary>
        public void End()
        {
            try
            {
                this.EndCore();
            }
            finally
            {
                this.IsInBlock = false;
            }
        }

        /// <summary>
        /// Ends drawing to this IDrawBatch, outputing the result.
        /// </summary>
        protected abstract void EndCore();
    }
}
