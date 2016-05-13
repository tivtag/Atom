// <copyright file="BaseTooltipDrawElement.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Tooltips.BaseTooltipDrawElement class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI.Tooltips
{
    using System;

    /// <summary>
    /// Represents an abstract base implementation of the ITooltipDrawElement interface.
    /// </summary>
    public abstract class BaseTooltipDrawElement : ITooltipDrawElement
    {
        /// <summary>
        /// Invoked when the <see cref="Tooltip"/> drawn
        /// by this ITooltipDrawElement has changed.
        /// </summary>
        public event EventHandler TooltipChanged;

        /// <summary>
        /// Gets or sets the Tooltip this BaseTooltipDrawElement is visualizing.
        /// </summary>
        public Tooltip Tooltip
        {
            get
            {
                return this.activeTooltip;
            }

            set
            {
                if( value == this.activeTooltip )
                    return;

                this.activeTooltip = value;
                this.OnTooltipChangedCore();
            }
        }

        /// <summary>
        /// Called when the <see cref="Tooltip"/> of this BaseTooltipDrawElement has changed.
        /// </summary>
        private void OnTooltipChangedCore()
        {
            this.TooltipChanged.Raise( this );
            this.OnTooltipChanged();
        }

        /// <summary>
        /// Called when the <see cref="Tooltip"/> of this BaseTooltipDrawElement has changed.
        /// </summary>
        protected virtual void OnTooltipChanged()
        {
        }
        
        /// <summary>
        /// The Tooltip this BaseTooltipDrawElement is visualizing.
        /// </summary>
        private Tooltip activeTooltip;
    }
}
