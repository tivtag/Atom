// <copyright file="PathSegment.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Waypoints.PathSegment class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Waypoints
{
    using System;
    using Atom.Components.Transform;
    using Atom.Math;
    using Atom.Math.Graph;
    using Atom.Math.Graph.Data;

    /// <summary>
    /// Represents a direct path between two <see cref="Waypoint"/>s.
    /// </summary>
    public class PathSegment : IOwnedBy<Edge<Waypoint, PathSegment>>, IReadOnlyWeightData, IReadOnlyDistanceData
    {
        /// <summary>
        /// Gets or sets the final weight of this PathSegment.
        /// </summary>
        public float Weight
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the IValueModifier that is applied on the Distance to create the Weight.
        /// </summary>
        public IValueModifier<float, float> WeightModifier
        {
            get
            {
                return this._weightModifier;
            }

            set
            {
                if( this.WeightModifier != null )
                {
                    this.WeightModifier.Changed -= this.OnWeightModifierChanged;
                }

                this._weightModifier = value;
                
                if( this.WeightModifier != null )
                {
                    this.WeightModifier.Changed += this.OnWeightModifierChanged;
                }

                this.UpdateWeight();
            }
        }

        /// <summary>
        /// Gets the distance this <see cref="PathSegment"/> takes up.
        /// </summary>
        public float Distance
        {
            get
            {
                return Vector2.Distance( this.From.Position, this.To.Position );
            }
        }

        /// <summary>
        /// Gets the <see cref="Waypoint"/> at which this PathSegment begins.
        /// </summary>
        public Waypoint From
        {
            get
            {
                return this.edge.From.Data;
            }
        }

        /// <summary>
        /// Gets the <see cref="Waypoint"/> at which this PathSegment ends.
        /// </summary>
        public Waypoint To
        {
            get
            {
                return this.edge.To.Data;
            }
        }

        /// <summary>
        /// Gets the Edge{Waypoint, PathSegment} that owns this PathSegment.
        /// </summary>
        public Edge<Waypoint, PathSegment> Edge
        {
            get
            {
                return this.edge;
            }
        }

        /// <summary>
        /// Gets the <see cref="IReadOnlyLineSegment2"/> that represents this PathSegment.
        /// </summary>
        /// <returns>
        /// The line from the <see cref="From"/> WayPoint to the <see cref="To"/> Waypoint.
        /// </returns>
        public IReadOnlyLineSegment2 Line
        {
            get
            {
                return this.line;
            }
        }

        /// <summary>
        /// Gets or sets the Edge{Waypoint, PathSegment} that owns this PathSegment.
        /// </summary>
        Edge<Waypoint, PathSegment> IOwnedBy<Edge<Waypoint, PathSegment>>.Owner
        {
            get
            {
                return this.edge;
            }

            set
            {
                if( this.edge != null )
                {
                    this.To.Transform.Changed -= this.OnWaypointTransformChangedCore;
                    this.From.Transform.Changed -= this.OnWaypointTransformChangedCore;
                }

                this.edge = value;

                if( this.edge != null )
                {
                    this.To.Transform.Changed += this.OnWaypointTransformChangedCore;
                    this.From.Transform.Changed += this.OnWaypointTransformChangedCore;
                    this.UpdateLine();
                }
            }
        }

        /// <summary>
        /// Called when the transformation of the starting or ending Waypoint has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        private void OnWaypointTransformChangedCore( ITransform2 sender )
        {
            this.UpdateLine();

            this.OnWaypointTransformChanged( sender );
            this.UpdateWeight();
        }
        
        /// <summary>
        /// Updates the LineSegment2 to the newest position values.
        /// </summary>
        private void UpdateLine()
        {
            this.line.Set( this.From.Position, this.To.Position );
        }

        /// <summary>
        /// Called when the transformation of the starting or ending Waypoint has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        protected virtual void OnWaypointTransformChanged( ITransform2 sender )
        {
        }
        
        /// <summary>
        /// Called when the internal state of the current WeightModifier has changed.
        /// </summary>
        /// <param name="modifier">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The EventArgs that contain the event data.
        /// </param>
        private void OnWeightModifierChanged( object modifier, EventArgs e )
        {
            this.UpdateWeight();
        }

        /// <summary>
        /// Updates the Weight of this PathSegment.
        /// </summary>
        protected virtual void UpdateWeight()
        {
            if( this.WeightModifier != null )
            {
                this.Weight = this.WeightModifier.Apply( this.Distance );
            }
            else
            {
                this.Weight = this.Distance;
            }
        }

        /// <summary>
        /// Overriden to return a description of this PathSegment.
        /// </summary>
        /// <returns>
        /// A human-readable description of this PathSegment.
        /// </returns>
        public override string ToString()
        {
            return (this.From.Name ?? string.Empty) + " -> " + (this.To.Name ?? string.Empty);
        }

        /// <summary>
        /// Represents the actual LineSegment2 this PathSegment covers.
        /// </summary>
        private readonly LineSegment2 line = new LineSegment2();

        /// <summary>
        /// Represents the edge that owns this PathSegment.
        /// </summary>
        private Edge<Waypoint, PathSegment> edge;

        /// <summary>
        /// The IValueModifier that is applied on the Distance to create the Weight.
        /// </summary>
        private IValueModifier<float, float> _weightModifier;
    }
}
