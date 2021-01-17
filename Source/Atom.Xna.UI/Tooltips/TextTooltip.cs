// <copyright file="TextTooltip.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Tooltips.TextTooltip class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI.Tooltips
{
    /// <summary>
    /// A TextTooltip (by default) shows some <see cref="Text"/> using a <see cref="ITooltipDrawElement"/>
    /// when the mouse is hovering over the Tooltip's <see cref="UIElement.ClientArea"/>.
    /// </summary>
    public class TextTooltip : Tooltip
    {
        /// <summary>
        /// Gets or sets the <see cref="Text"/> shown by this Tooltip.
        /// </summary>
        /// <value>The default value is null.</value>
        public Text Text
        {
            get;
            set;
        }     
        
        /// <summary>
        /// Initializes a new instance of the TextTooltip class.
        /// </summary>
        /// <param name="tooltipDrawElement">
        /// The UIElement that is responsible for drawing the tooltip.
        /// </param>
        public TextTooltip( ITooltipDrawElement tooltipDrawElement )
            : base( tooltipDrawElement )
        {
        }
    }
}
