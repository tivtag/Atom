// <copyright file="AStar.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Algorithms.AStar{TVertexData, TEdgeData} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Math.Graph.Algorithms
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using Atom.Collections;
    using Atom.Math.Graph.Data;

    /// <summary>
    /// Implements the A* algorithm for <see cref="Graph{TVertexData, TEdgeData}"/>s
    /// that store <see cref="IPositionable2"/> data in their vertices.
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of data stored by the vertices of the <see cref="Graph{TVertexData, TEdgeData}"/>.
    /// Must implement <see cref="IPositionable2"/>.
    /// </typeparam>
    /// <typeparam name="TEdgeData">
    /// The type of data stored within the edges of the Graph.
    /// </typeparam>
    public class AStar<TVertexData, TEdgeData>
        where TVertexData : IPositionable2, IEquatable<TVertexData>
        where TEdgeData : IReadOnlyWeightData
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the source <see cref="Vertex{TVertexData, TEdgeData}"/> of the last <see cref="Search"/>.
        /// </summary>
        /// <value>The source vertex of the last AStar search.</value>
        public Vertex<TVertexData, TEdgeData> Source
        {
            get 
            {
                return this.source;
            }
        }

        /// <summary>
        /// Gets the target <see cref="Vertex{TVertexData, TEdgeData}"/> of the last <see cref="Search"/>.
        /// </summary>
        /// <value>The target vertex of the last AStar search.</value>
        public Vertex<TVertexData, TEdgeData> Target
        {
            get 
            {
                return this.target;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a path has been found for the last <see cref="Search"/>.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if a path could be found; otherwise <see langword="false"/>.
        /// </value>
        public bool IsPathFound
        {
            get 
            {
                return this.leafToGoBackUp != null;
            }
        }

        /// <summary>
        /// Gets the array of <see cref="Edge{TVertexData, TEdgeData}"/>s representing
        /// the path found in the last <see cref="Search"/>.
        /// </summary>
        /// <returns>
        /// The found path; or null if no path has been found.
        /// </returns>
        public Edge<TVertexData, TEdgeData>[] GetFoundPath()
        {
            if( !this.IsPathFound )
                return null;

            int edgeCount = leafToGoBackUp.NumberOfEdgesVisited;

            var path  = new Edge<TVertexData, TEdgeData>[edgeCount];
            var track = leafToGoBackUp;

            for( int i = edgeCount - 1; i >= 0; --i, track = track.PreviousTrack )
            {
                path[i] = track.PreviousTrack.End.GetEmanatingEdgeTo( track.End );
            }

            return path;
        }

        #endregion

        #region [ Constructors ]

        /// <summary> 
        /// Initializes a new instance of the <see cref="AStar{TVertexData, TEdgeData}"/> class.
        /// </summary>
        /// <param name="graph">
        /// The graph on which the A* algorithm will perform the search.
        /// </param>
        public AStar( Graph<TVertexData, TEdgeData> graph )
        {
            Contract.Requires<ArgumentNullException>( graph != null );

            this.graph = graph;
            this.open = new SortableList<Track<TVertexData, TEdgeData>>();
            this.closed = new SortableList<Track<TVertexData, TEdgeData>>();
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Initializes AStar before performing a search.
        /// </summary>
        /// <exception cref="ArgumentNullException">StartNode and EndNode cannot be null.</exception>
        /// <param name="source">The node from which the path must start.</param>
        /// <param name="target">The node to which the path must end.</param>
        private void Initialize( Vertex<TVertexData, TEdgeData> source, Vertex<TVertexData, TEdgeData> target )
        {
            this.source = source;
            this.target = target;

            this.closed.Clear();
            this.open.Clear();

            this.open.Add( new Track<TVertexData, TEdgeData>( source, target, heuristic ) );
            this.leafToGoBackUp = null;
        }

        /// <summary>
        /// Searches the best path from the <paramref name="source"/> vertex to the <paramref name="target"/> vertex.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="source"/> or <paramref name="target"/> is null.
        /// </exception>
        /// <param name="source">The vertex from which the path must start.</param>
        /// <param name="target">The vertex to which the path must end.</param>
        /// <returns>
        /// Returns <see langword="true"/> if a path could be found; or
        /// otherwise, if the search has failed, <see langword="false"/>.
        /// </returns>
        public bool Search( Vertex<TVertexData, TEdgeData> source, Vertex<TVertexData, TEdgeData> target )
        {
            Contract.Requires<ArgumentNullException>( source != null );
            Contract.Requires<ArgumentNullException>( target != null );

            lock( this.graph )
            {
                this.Initialize( source, target );

                while( this.NextStep() )
                {
                }

                return this.IsPathFound;
            }
        }

        /// <summary>
        /// Does the next step in the A* search.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if the search should continue;
        /// or otherwise <see langword="false"/> if the search has failed or completed.
        /// </returns>
        private bool NextStep()
        {
            if( this.open.Count == 0 )
                return false;

            int minimumIndex = this.open.IndexOfMinimum;
            var bestTrack = this.open[minimumIndex];

            this.open.RemoveAt( minimumIndex );

            if( bestTrack.Succeed )
            {
                this.leafToGoBackUp = bestTrack;
                this.open.Clear();
            }
            else
            {
                this.Propagate( bestTrack );
                this.closed.Add( bestTrack );
            }

            return this.open.Count > 0;
        }

        /// <summary>
        /// Continues the search.
        /// </summary>
        /// <param name="bestTrack">
        /// The currently best track.
        /// </param>
        private void Propagate( Track<TVertexData, TEdgeData> bestTrack )
        {
            foreach( Edge<TVertexData, TEdgeData> edge in bestTrack.End.EmanatingEdges )
            {
                var successor = new Track<TVertexData, TEdgeData>( bestTrack, edge, target, heuristic );
                int positionClosed = closed.IndexOf( successor, trackComparer );
                int positionOpen   = open.IndexOf( successor, trackComparer );

                if( positionClosed > 0 && successor.Weight >= closed[positionClosed].Weight )
                    continue;
                if( positionOpen > 0 && successor.Weight >= open[positionOpen].Weight )
                    continue;

                if( positionClosed > 0 )
                    closed.RemoveAt( positionClosed );
                if( positionOpen > 0 )
                    open.RemoveAt( positionOpen );

                open.Add( successor );
            }
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The source vertex at which the search has started.
        /// </summary>
        private Vertex<TVertexData, TEdgeData> source;

        /// <summary>
        /// The target vertex at which the search should end.
        /// </summary>
        private Vertex<TVertexData, TEdgeData> target;

        /// <summary>
        /// The heuristic used to find the the path.
        /// </summary>
        private Heuristic<TVertexData, TEdgeData> heuristic = Heuristics.Positionable2.EuclidianDistance;

        /// <summary>
        /// The currently processed track.
        /// </summary>
        private Track<TVertexData, TEdgeData> leafToGoBackUp;

        /// <summary>
        /// The open and closed lists of nodes used by the A* algorithm.
        /// </summary>
        private readonly SortableList<Track<TVertexData, TEdgeData>> open, closed;

        /// <summary>
        /// The graph this A* algorithm searches in. 
        /// </summary>
        private readonly Graph<TVertexData, TEdgeData> graph;

        /// <summary>
        /// The comparer which is used to identify the better of two tracks.
        /// </summary>
        private readonly IEqualityComparer<Track<TVertexData, TEdgeData>> trackComparer 
            = new Track<TVertexData, TEdgeData>.SameEndNodeComparer();

        #endregion
    }
}
