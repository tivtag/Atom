// <copyright file="ITransform2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Components.Transform.ITransform2 interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Components.Transform
{
    using System;
    using Atom.Math;

    /// <summary>
    /// Provides access to the transformation state of an <see cref="IEntity"/> in two-dimensional space.
    /// This includes <see cref="IPositionable2.Position"/>, <see cref="Scale"/>, <see cref="Rotation"/> and <see cref="Origin"/>,
    /// </summary>
    public interface ITransform2 : IComponent, IPositionable2
    {
        #region [ Events ]

        /// <summary>
        /// Fired when the transform this ITransform2 component represents has changed.
        /// </summary>
        event SimpleEventHandler<ITransform2> Changed;

        /// <summary>
        /// Fired when the <see cref="X"/> or <see cref="Y"/> position of this ITransform2 has changed.
        /// </summary>
        event RelaxedEventHandler<ChangedValue<Vector2>> PositionChanged;

        /// <summary>
        /// Fired when the <see cref="Scale"/> of this ITransform2 has changed.
        /// </summary>
        event RelaxedEventHandler<ChangedValue<Vector2>> ScaleChanged;

        /// <summary>
        /// Fired when the <see cref="Rotation"/> of this ITransform2 has changed.
        /// </summary>
        event RelaxedEventHandler<ChangedValue<float>> RotationChanged;

        /// <summary>
        /// Fired when the <see cref="Origin"/> of this ITransform2 has changed.
        /// </summary>
        event RelaxedEventHandler<ChangedValue<Vector2>> OriginChanged;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the position of this ITransform2 component on the x-axis.
        /// </summary>
        /// <value>The position on the x-axis of the transformable Entity.</value>
        float X
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position of this ITransform2 component on the y-axis.
        /// </summary>
        /// <value>The position on the y-axis of the transformable Entity.</value>
        float Y
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale of this ITransform2 component.
        /// </summary>
        /// <value>The scale of the transformable Entity.</value>
        Vector2 Scale
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rotation in radians of this ITransform2 component.
        /// </summary>
        /// <value>The rotation value of the transformable Entity.</value>
        float Rotation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the origin of this ITransform2 component.
        /// </summary>
        /// <value>The scale and rotation origin of the transformable Entity.</value>
        Vector2 Origin
        {
            get;
            set;
        }

        #endregion
    }
}
