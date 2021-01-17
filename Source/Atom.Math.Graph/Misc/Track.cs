// <copyright file="Track.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Track{TVertexData, TEdgeData} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using Atom.Math.Graph.Data;

    /// <summary>
    /// A track is a succession of vertices which have been visited.
    /// Thus when it leads to the target node, it is easy to return the result path.
    /// These objects are contained in Open and Closed lists.
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of data stored within the vertices of the Graph.
    /// </typeparam>
    /// <typeparam name="TEdgeData">
    /// The type of data stored within the edges of the Graph.
    /// </typeparam>
    internal sealed class Track<TVertexData, TEdgeData> : IComparable, IComparable<Track<TVertexData, TEdgeData>>
        where TVertexData : IEquatable<TVertexData>
        where TEdgeData : IReadOnlyWeightData
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the Vertex at which this Track ends.
        /// </summary>
        public Vertex<TVertexData, TEdgeData> End
        {
            get 
            {
                return this.end;
            }

            set
            { 
                this.end = value; 
            }
        }

        /// <summary>
        /// Gets or sets the Track prior of this Track.
        /// </summary>
        public Track<TVertexData, TEdgeData> PreviousTrack
        {
            get 
            {
                return this.previousTrack;
            }

            set
            {
                this.previousTrack = value;
            }
        }

        /// <summary>
        /// Gets or sets the targeted track.
        /// </summary>
        public Vertex<TVertexData, TEdgeData> Target
        {
            get 
            {
                return this.target;
            }

            set
            { 
                this.target = value;
            }
        }

        /// <summary>
        /// Gets or sets the heuristic that is used to calcualte the <see cref="Evaluation"/> value.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="value"/> is null.
        /// </exception>
        public Heuristic<TVertexData, TEdgeData> Heuristic
        {
            get
            {
                // Contract.Ensures( Contract.Result<Heuristic<TVertexData, TEdgeData>>() != null );

                return this.heuristic;
            }

            set
            {
                Contract.Requires<ArgumentNullException>( value != null );

                this.heuristic = value;
            }
        }

        /// <summary>
        /// Gets the weight of this Track.
        /// </summary>
        public float Weight
        {
            get 
            {
                return this.weight; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether this Track has reached its end.
        /// </summary>
        public bool Succeed
        {
            get 
            {
                return this.end == this.target; 
            }
        }

        /// <summary>
        /// Gets the number of edges that have been visited
        /// after traversing the Track queue until this Track.
        /// </summary>
        public int NumberOfEdgesVisited
        {
            get
            {
                // Contract.Ensures( Contract.Result<int>() >= 0 );

                return this.numberOfEdgesVisited;
            }
        }

        /// <summary>
        /// Gets or sets the coefficient which balances the respective influences of Dijkstra and the Heuristic must belong to [0; 1].
        /// -&gt; 0 will minimize the number of nodes explored but will not take the real cost into account.
        /// -&gt; 0.5 will minimize the cost without developing more nodes than necessary.
        /// -&gt; 1 will only consider the real cost without estimating the remaining cost.
        /// </summary>
        public float DijkstraHeuristicBalance
        {
            get
            {
                // Contract.Ensures( Contract.Result<float>() >= 0.0f && Contract.Result<float>() <= 1.0f );

                return this.coefficient;
            }

            set
            {
                Contract.Requires<ArgumentException>( value >= 0.0f && value <= 1.0f );

                this.coefficient = value;
            }
        }

        /// <summary>
        /// Gets the evaluation value.
        /// </summary>
        public float Evaluation
        {
            get
            {
                return (this.coefficient * this.weight) + ((1.0f - this.coefficient) * this.heuristic( this.end, this.target ));
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Track{TVertexData, TEdgeData}"/> class.
        /// </summary>
        /// <param name="source">
        /// The source vertex of this Track.
        /// </param>
        /// <param name="target">
        /// The target of the Search.
        /// </param>
        /// <param name="heuristic">
        /// The heuristic that is used to calcualte the <see cref="Evaluation"/> value.
        /// </param>
        internal Track(
            Vertex<TVertexData, TEdgeData> source,
            Vertex<TVertexData, TEdgeData> target,
            Heuristic<TVertexData, TEdgeData> heuristic )
        {
            Contract.Requires<ArgumentNullException>( target != null );
            Contract.Requires<ArgumentNullException>( heuristic != null );

            this.target = target;
            this.end = source;
            this.heuristic = heuristic;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Track{TVertexData, TEdgeData}"/> class.
        /// </summary>
        /// <param name="previousTrack">
        /// The track prior of the new Track.</param>
        /// <param name="transition">
        /// The edge from the previous Track to the new Track.
        /// </param>
        /// <param name="target">
        /// The target of the Search.
        /// </param>
        /// <param name="heuristic">
        /// The heuristic that is used to calcualte the <see cref="Evaluation"/> value.
        /// </param>
        internal Track(
            Track<TVertexData, TEdgeData> previousTrack,
            Edge<TVertexData, TEdgeData> transition,
            Vertex<TVertexData, TEdgeData> target,
            Heuristic<TVertexData, TEdgeData> heuristic )
        {
            Contract.Requires<ArgumentNullException>( previousTrack != null );
            Contract.Requires<ArgumentNullException>( transition != null );
            Contract.Requires<ArgumentNullException>( target != null );
            Contract.Requires<ArgumentNullException>( heuristic != null );

            this.heuristic     = heuristic;
            this.target        = target;
            this.previousTrack = previousTrack;
            this.end           = transition.To;

            this.weight = previousTrack.Weight + transition.Data.Weight;
            this.numberOfEdgesVisited = previousTrack.numberOfEdgesVisited + 1;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Compares this Track to the given Object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>The comparisation result.</returns>
        public int CompareTo( object obj )
        {
            return this.CompareTo( obj as Track<TVertexData, TEdgeData> );
        }

        /// <summary>
        /// Compares this Track to the given Track.
        /// </summary>
        /// <param name="other">The Track to compare to.</param>
        /// <returns>The comparisation result.</returns>
        public int CompareTo( Track<TVertexData, TEdgeData> other )
        {
            if( other == null )
                return -1;

            return this.Evaluation.CompareTo( other.Evaluation );
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The target and end of this Track.
        /// </summary>
        private Vertex<TVertexData, TEdgeData> target, end;

        /// <summary>
        /// The heurestic used to calcualte the weight of this Track.
        /// </summary>
        private Heuristic<TVertexData, TEdgeData> heuristic;

        /// <summary>
        /// The Track prior of this Track.
        /// </summary>
        private Track<TVertexData, TEdgeData> previousTrack;

        /// <summary>
        /// The coefficient of this Track.
        /// </summary>
        private float coefficient;

        /// <summary>
        /// The number of edges that have been visited so far.
        /// </summary>
        private readonly int numberOfEdgesVisited;

        /// <summary>
        /// The weight of this Track, calculate using the Heuristic.
        /// </summary>
        private readonly float weight;

        #endregion

        #region > class SameEndNodeComparer <

        /// <summary>
        /// Compares the end nodes of two tracks.
        /// </summary>
        internal sealed class SameEndNodeComparer : IEqualityComparer<Track<TVertexData, TEdgeData>>
        {
            /// <summary>
            /// Determines whether the given Tracks are equal.
            /// </summary>
            /// <param name="x">The first Track.</param>
            /// <param name="y">The second Track.</param>
            /// <returns>
            /// Returns true if the given Tracks are equal;
            /// otherwise false.
            /// </returns>
            public bool Equals( Track<TVertexData, TEdgeData> x, Track<TVertexData, TEdgeData> y )
            {
                return x.end == y.end;
            }

            /// <summary>
            /// Gets the hash code of the given Track.
            /// </summary>
            /// <param name="obj">The given Track.</param>
            /// <returns>The hash code of the given Track.</returns>
            public int GetHashCode( Track<TVertexData, TEdgeData> obj )
            {
                return obj.GetHashCode();
            }
        }

        #endregion
    }
}
