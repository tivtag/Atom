// <copyright file="Tooltip.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Tooltips.Tooltip class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI.Tooltips
{
    using System;
    using Atom.Diagnostics.Contracts;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// A TextTooltip (by default) draws some information using a <see cref="ITooltipDrawElement"/>
    /// when the mouse is hovering over the <see cref="UIElement.ClientArea"/> of the Tooltip or the UIElement it is attached to.
    /// </summary>
    /// <remarks>
    /// The <see cref="ITooltipDrawElement"/> is drawn inline if it implements <see cref="IDrawable"/> and
    /// is not an <see cref="UIElement"/>.
    /// (<see cref="LambdaTooltipDrawElement"/> for example)
    /// </remarks>
    public class Tooltip : UIElement
    {   
        /// <summary>
        /// Gets the <see cref="ITooltipDrawElement"/> this Tooltip uses to visualize its <see cref="Text"/>.
        /// </summary>
        /// <value>The <see cref="ITooltipDrawElement"/> this Tooltip uses.</value>
        public ITooltipDrawElement DrawElement
        {
            get
            {
                return this.drawElement;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="UIElement"/> this Tooltip is attached to.
        /// </summary>
        public UIElement AttachedTo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the Tooltip is currently visible.
        /// </summary>
        public bool IsTooltipVisible
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the Tooltip class.
        /// </summary>
        /// <param name="tooltipDrawElement">
        /// The UIElement that is responsible for drawing the tooltip.
        /// </param>
        public Tooltip( ITooltipDrawElement tooltipDrawElement )
            : base()
        {
            Contract.Requires<ArgumentNullException>( tooltipDrawElement != null );

            this.drawElement = tooltipDrawElement;
        }

        /// <summary>
        /// Called once every frame, allows this <see cref="UIElement"/> to
        /// react to mouse input.
        /// </summary>
        /// <param name="mouseState">
        /// The state of the <see cref="Mouse"/>.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        /// <param name="oldMouseState">
        /// The state of the <see cref="Mouse"/> one frame ago.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        protected override void HandleMouseInput( ref MouseState mouseState, ref MouseState oldMouseState )
        {
            this.IsTooltipVisible = this.GetTooltipVisibility( ref mouseState, ref oldMouseState );

            if( this.IsTooltipVisible )
            {
                this.drawElement.Tooltip = this;
            }
            else
            {
                if( this.drawElement.Tooltip == this )
                {
                    this.drawElement.Tooltip = null;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tooltip is currently visible.
        /// </summary>
        /// <param name="mouseState">
        /// The state of the <see cref="Mouse"/>.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        /// <param name="oldMouseState">
        /// The state of the <see cref="Mouse"/> one frame ago.
        /// Do NOT modify this value, unless you know what you do.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the Tooltip is currently visible;
        /// otherwise <see langword="false"/>.
        /// </returns>
        protected virtual bool GetTooltipVisibility( ref MouseState mouseState, ref MouseState oldMouseState )
        {
            if( this.AttachedTo != null )
                return this.AttachedTo.ClientArea.Contains( mouseState.X, mouseState.Y );

            return this.ClientArea.Contains( mouseState.X, mouseState.Y );
        }

        /// <summary>
        /// Called when this UIElement is drawing itself.
        /// </summary>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        protected override void OnDraw( ISpriteDrawContext drawContext )
        {
            if( this.IsTooltipVisible )
            {
                var drawable = this.drawElement as IDrawable;

                if( drawable != null && !(this.drawElement is UIElement) )
                {
                    drawable.Draw( drawContext );
                }
                else
                {
                    // The actual drawing logic is handled by the ITooltipDrawElement.
                }
            }
        }

        /// <summary>
        /// Called when this UIElement is drawing itself.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        protected override void OnUpdate( IUpdateContext updateContext )
        {
        }

        /// <summary>
        /// The UIElement that is actually responsible for drawing the tooltip.
        /// </summary>
        private readonly ITooltipDrawElement drawElement;
    }
}
