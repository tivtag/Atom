// <copyright file="ICollision2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Components.Collision.ICollision2 interface.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Components.Collision
{
    using System;
    using Atom.Components.Transform;
    using Atom.Math;

    /// <summary>
    /// Provides access to the collision information of an <see cref="IEntity"/>
    /// in two-dimensional space.
    /// </summary>
    /// <remarks>
    /// The ICollision2 component depends on the <see cref="ITransform2"/> component
    /// for transformation information.
    /// </remarks>
    public interface ICollision2 : IComponent
    {
        /// <summary>
        /// Fired when the collision information of this 
        /// ICollision2 component has been changed/refreshed.
        /// </summary>
        event SimpleEventHandler<ICollision2> Changed;

        /// <summary>
        /// Gets the axis-aligned collision <see cref="RectangleF"/> of this ICollision2 component.
        /// </summary>
        /// <value>The collision RectangleF.</value>
        RectangleF Rectangle 
        {
            get;
        }

        /// <summary>
        /// Gets the collision <see cref="Circle"/> of this ICollision2 component.
        /// </summary>
        /// <value>The collision Circle.</value>
        Circle Circle 
        {
            get;
        }
        
        /// <summary>
        /// Gets the <see cref="ITransform2"/> component that exposes the
        /// transformation information of the <see cref="IEntity"/>.
        /// </summary>
        ITransform2 Transform 
        { 
            get;
        }
    }
}
