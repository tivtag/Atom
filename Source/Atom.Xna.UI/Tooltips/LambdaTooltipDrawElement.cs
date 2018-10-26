// <copyright file="LambdaTooltipDrawElement.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Tooltips.LambdaTooltipDrawElement class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.UI.Tooltips
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents an ITooltipDrawElement that delegates the drawing logic into a lambda function / delegate.
    /// </summary>
    public sealed class LambdaTooltipDrawElement : BaseTooltipDrawElement, IDrawable
    {
        /// <summary>
        /// Initializes a new instance of the LambdaTooltipDrawElement class.
        /// </summary>
        /// <param name="drawAction">
        /// The action that is called when drawing the new LambdaTooltipDrawElement.
        /// </param>
        public LambdaTooltipDrawElement( Action<Tooltip, ISpriteDrawContext> drawAction )
        {
            Contract.Requires<ArgumentNullException>( drawAction != null );

            this.drawAction = drawAction;
        }
        
        /// <summary>
        /// Draws this LambdaTooltipDrawElement.
        /// </summary>
        /// <param name="drawContext">
        /// The current IDrawContext.
        /// </param>
        public void Draw( IDrawContext drawContext )
        {
            if( this.Tooltip != null )
            {
                this.drawAction( this.Tooltip, (ISpriteDrawContext)drawContext );
            }
        }

        /// <summary>
        /// The action that is called when drawing this LambdaTooltipDrawElement.
        /// </summary>
        private readonly Action<Tooltip, ISpriteDrawContext> drawAction;
    }
}
