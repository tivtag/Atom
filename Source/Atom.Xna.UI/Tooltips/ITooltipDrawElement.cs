// <copyright file="ITooltipDrawElement.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Tooltips.ITooltipDrawElement interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI.Tooltips
{
    using System;

    /// <summary>
    /// Provides a mechanism to draw a tooltip string.
    /// </summary>
    public interface ITooltipDrawElement
    {
        /// <summary>
        /// Invoked when the <see cref="Tooltip"/> drawn
        /// by this ITooltipDrawElement has changed.
        /// </summary>
        event EventHandler TooltipChanged;

        /// <summary>
        /// Gets or sets the <see cref="Tooltip"/> this ITooltipDrawElement 
        /// is visualizing.
        /// </summary>
        /// <value>The Tooltip to visualize. Set to null to not visualize any Tooltip.</value>
        Tooltip Tooltip
        {
            get;
            set;
        }
    }
}
