// <copyright file="LambdaButton.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.UI.Controls.LambdaButton class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.UI.Controls
{
    using System;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a button that redirects the update and drawing operations
    /// to external lambdas / delegates.
    /// </summary>
    public class LambdaButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the LambdaButton class.
        /// </summary>
        /// <param name="onUpdate">
        /// The action that should be executed when the new LabdaButton is updating itself.
        /// </param>
        /// <param name="onDraw">
        /// The action that should be executed when the new LabdaButton is drawing itself.
        /// </param>
        public LambdaButton( Action<LambdaButton, IUpdateContext> onUpdate, Action<LambdaButton, ISpriteDrawContext> onDraw )
        {
            Contract.Requires<ArgumentNullException>( onUpdate != null );
            Contract.Requires<ArgumentNullException>( onDraw != null );
            
            this.onUpdate = onUpdate;
            this.onDraw = onDraw;
        }

        /// <summary>
        /// Called when this <see cref="UIElement"/> is updating.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        protected override void OnUpdate( IUpdateContext updateContext )
        {
            this.onUpdate( this, updateContext );
        }

        /// <summary>
        /// Called when this <see cref="UIElement"/> is drawing.
        /// </summary>
        /// <param name="drawContext">
        /// The current ISpriteDrawContext.
        /// </param>
        protected override void OnDraw( ISpriteDrawContext drawContext )
        {
            this.onDraw( this, drawContext );
        }

        /// <summary>
        /// The action that is executed when this LabdaButton is updating itself.
        /// </summary>
        private readonly Action<LambdaButton, IUpdateContext> onUpdate;

        /// <summary>
        /// The action that is executed when this LabdaButton is drawing itself.
        /// </summary>
        private readonly Action<LambdaButton, ISpriteDrawContext> onDraw;
    }
}
