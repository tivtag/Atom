// <copyright file="TextFieldToolTipDrawElement.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Tooltips.TextFieldToolTipDrawElement class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI.Tooltips
{
    using System;
    using Atom.Xna.UI.Controls;

    /// <summary>
    /// Implements an <see cref="ITooltipDrawElement"/> that draws <see cref="TextTooltip"/>s using a <see cref="TextField"/>.
    /// </summary>
    public class TextFieldToolTipDrawElement : TextField, ITooltipDrawElement
    {
        #region [ Events ]

        /// <summary>
        /// Invoked when the <see cref="TextFieldToolTipDrawElement"/> drawn
        /// by this ITooltipDrawElement has changed.
        /// </summary>
        public event EventHandler TooltipChanged;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the TextTooltip this TextFieldToolTipDrawElement is visualizing.
        /// </summary>
        public TextTooltip Tooltip
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
        /// Gets or sets the TextTooltip this TextFieldToolTipDrawElement is visualizing.
        /// </summary>
        Tooltip ITooltipDrawElement.Tooltip
        {
            get
            {
                return this.Tooltip;
            }

            set
            {
                this.Tooltip = (TextTooltip)value;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Called when the <see cref="Tooltip"/> of this TextFieldToolTipDrawElement has changed.
        /// </summary>
        private void OnTooltipChangedCore()
        {
            // Inform user.
            if( this.TooltipChanged != null )
                this.TooltipChanged( this, EventArgs.Empty );
            this.OnTooltipChanged();

            // Update displayed Text.
            if( this.activeTooltip != null )
                this.Text = this.activeTooltip.Text;
            else
                this.Text = null;
        }

        /// <summary>
        /// Called when the <see cref="Tooltip"/> of this TextFieldToolTipDrawElement has changed.
        /// </summary>
        protected virtual void OnTooltipChanged()
        {
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The Tooltip this TextFieldToolTipDrawElement is visualizing.
        /// </summary>
        private TextTooltip activeTooltip;

        #endregion
    }
}
