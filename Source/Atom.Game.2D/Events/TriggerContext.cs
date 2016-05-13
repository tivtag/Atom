// <copyright file="TriggerContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Events.TriggerContext structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Events
{
    /// <summary>
    /// Represents the context of trigger search and execution.
    /// </summary>
    public sealed class TriggerContext
    {
        /// <summary>
        /// The source of the trigger execution. Can be used to execute some triggers only in specific execution contexts.
        /// </summary>
        public readonly object Source;

        /// <summary>
        /// The object that triggered the trigger execution.
        /// </summary>
        public readonly object Object;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerContext"/> class.
        /// </summary>
        /// <param name="source">
        /// The source of the trigger execution.
        /// </param>
        /// <param name="object">
        /// The object that triggered the trigger execution.
        /// </param>
        public TriggerContext( object source, object @object )
        {
            this.Source = source;
            this.Object = @object;
        }
    }
}
