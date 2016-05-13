// <copyright file="Graph.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Graph{TVertexData, TEdgeData} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using Atom.Math.Graph.Algorithms;
    using Atom.Patterns.Visitor;

    /// <summary>
    /// An implementation of a Graph data structure.
    /// The graph can be either directed or undirected.
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of data stored within the vertices of the Graph.
    /// </typeparam>
    /// <typeparam name="TEdgeData">
    /// The type of data stored within the edges of the Graph.
    /// </typeparam>
    [Serializable]
    public partial class Graph<TVertexData, TEdgeData> : IEnumerable<TVertexData>
        where TVertexData : IEquatable<TVertexData>
    {
        #region [ Events ]

        /// <summary>
        /// Raised when a Vertex has been added to this Graph{TVertexData, TEdgeData}.
        /// </summary>
        public event RelaxedEventHandler<Graph<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>> VertexAdded;

        /// <summary>
        /// Raised when an Edge has been added to this Graph{TVertexData, TEdgeData}.
        /// </summary>
        public event RelaxedEventHandler<Graph<TVertexData, TEdgeData>, Edge<TVertexData, TEdgeData>> EdgeAdded;
        
        /// <summary>
        /// Raised when a Vertex has been removed from this Graph{TVertexData, TEdgeData}.
        /// </summary>
        public event RelaxedEventHandler<Graph<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>> VertexRemoved;

        /// <summary>
        /// Raised when an Edge has been removed from this Graph{TVertexData, TEdgeData}.
        /// </summary>
        public event RelaxedEventHandler<Graph<TVertexData, TEdgeData>, Edge<TVertexData, TEdgeData>> EdgeRemoved;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the vertices contained in this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <value>The vertices contained in this <see cref="Graph{TVertexData, TEdgeData}"/>.</value>
        public IEnumerable<Vertex<TVertexData, TEdgeData>> Vertices
        {
            get
            {
                return this.vertices;
            }
        }

        /// <summary>
        /// Gets the edges contained in this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <value>The edges contained in this <see cref="Graph{TVertexData, TEdgeData}"/>.</value>
        public IEnumerable<Edge<TVertexData, TEdgeData>> Edges
        {
            get
            {
                return this.edges;
            }
        }

        /// <summary>
        /// Gets the <see cref="IGraphDataFactory{TVertexData, TEdgeData}"/> responsible for creating TVertexData and TEdgeData objects.
        /// </summary>
        public IGraphDataFactory<TVertexData, TEdgeData> DataFactory
        {
            get
            {
                return this.dataFactory;
            }
        }

        /// <summary>
        /// Gets the number of vertices in this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <value>
        /// The 'order' of a graph is the number of vertices in its vertex set.
        /// </value>
        public int VertexCount
        {
            get 
            {
                return this.vertices.Count;
            }
        }

        /// <summary> 
        /// Gets the number of edges in this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <value>The number of edges.</value>
        public int EdgeCount
        {
            get 
            {
                return this.edges.Count;
            }
        }

        /// <summary>
        /// Gets the number of regions an imbedding of this <see cref="Graph{TVertexData, TEdgeData}"/>
        /// into the Sphere would have.
        /// </summary>
        /// <remarks>
        /// Uses Eulers equation:
        /// #V - #E + #F = 2
        /// </remarks>
        /// <value>The number of faces.</value>
        public int FaceCount
        {
            get
            {
                int edgeCount = this.EdgeCount;
                if( edgeCount <= 1 )
                    return 1;

                return 2 - this.VertexCount + edgeCount;
            }
        }

        /// <summary>
        /// Gets or sets the number of vertices this Graph{TVertexData, TEdgeData} can contain
        /// without having to re-allocate memory.
        /// </summary>
        public int VertexCapacity
        {
            get 
            { 
                return this.vertices.Capacity;
            }

            set
            { 
                this.vertices.Capacity = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of edges this Graph{TVertexData, TEdgeData} can contain
        /// without having to re-allocate memory.
        /// </summary>
        public int EdgeCapacity
        {
            get
            {
                return this.edges.Capacity; 
            }

            set
            { 
                this.edges.Capacity = value; 
            }
        }

        /// <summary>
        /// Gets the girth of this <see cref="Graph{TVertexData, TEdgeData}"/>,
        /// which is the number of edges in its shortest cycle.
        /// </summary>
        /// <remarks>
        /// The girth of a graph is -1 if no cycles are found.
        /// </remarks>
        /// <value>The number of edges in the shortest cycle.</value>
        public int Girth
        {
            get
            {
                if( this.ContainsSelfLoops )
                    return 1;

                int girth = int.MaxValue;
                var currentWalk = new Stack<Vertex<TVertexData, TEdgeData>>();

                foreach( var vertex in this.vertices )
                {
                    currentWalk.Clear();
                    currentWalk.Push( vertex );
 
                    Cycles.FindMinimumLength( vertex, vertex, null, currentWalk, ref girth );
                    if( girth == 3 )
                        return 3;
                }

                if( girth == int.MaxValue )
                    girth = -1;

                return girth;
            }
        }
        
        #region > Settings <

        /// <summary>
        /// Gets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/> is a directed graph.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> is this Graph is directed;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsDirected
        {
            get 
            {
                return this.isDirected;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/>
        /// allows Edges that start and end at the same Vertex.
        /// </summary>
        /// <value>The default value is false.</value>
        public bool AllowsSelfLoops
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/>
        /// allows multiple edges to start and end at the same vertices.
        /// </summary>
        /// <value>The default value is false.</value>
        public bool AllowsMultipleEdges
        {
            get;
            set;
        }

        #endregion

        #region > State <

        /// <summary>
        /// Gets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/> is simplistic.
        /// </summary>
        /// <remarks>
        /// A "simple" Graph contains no double edges or edges that start and end at the same vertex.
        /// </remarks>
        /// <value>
        /// Returns <see langword="true"/> is this Graph is simplistic;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsSimplistic
        {
            get
            {
                return !this.ContainsSelfLoops || !this.ContainsMultipleEdges;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/>
        /// contains any edges that start and end at the same Vertex.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if this Graph contains self loops;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool ContainsSelfLoops
        {
            get
            {
                return this.selfLoopCount > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/>
        /// contains any multiple edges.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> is this Graph contains multiple edges;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool ContainsMultipleEdges
        {
            get 
            { 
                return this.multipleEdgeCount > 0; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/> is regular.
        /// </summary>
        /// <remarks>
        /// In a regular Graph every vertex has the same number of neighbours.
        /// </remarks>
        /// <value>
        /// Returns <see langword="true"/> if this Graph is regular;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsRegular
        {
            get
            {
                if( this.VertexCount <= 1 )
                    return true;

                int degree = this.vertices[0].Degree;

                for( int i = 1; i < this.vertices.Count; ++i )
                {
                    if( vertices[i].Degree != degree )
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/> is weakly connected.
        /// </summary>
        /// <exception cref="InvalidOperationException"> 
        /// If the Graph is empty.
        /// </exception>
        /// <value>
        /// Returns <see langword="true"/> if this Graph is weakly connected;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsWeaklyConnected
        {
            get
            {
                Contract.Requires<InvalidOperationException>( this.VertexCount > 0, GraphErrorStrings.GraphIsEmpty );

                var visitor = new CountingVisitor<Vertex<TVertexData, TEdgeData>>();
                Traversal.BreadthFirst( GetAnyVertex(), visitor );

                return visitor.Count == this.vertices.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/> is strongly connected.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// This operation is only valid on a directed graph. For undirected graphs, rather test for weak connectedness.
        /// or if the graph is empty.
        /// </exception>
        /// <value>
        /// Returns <see langword="true"/> if this Graph is strongly connected;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsStronglyConnected
        {
            get
            {
                Contract.Requires<InvalidOperationException>( this.IsDirected, GraphErrorStrings.OperationOnlyValidonDirectedGraph );
                Contract.Requires<InvalidOperationException>( this.VertexCount > 0, GraphErrorStrings.GraphIsEmpty );
                
                var vistor = new CountingVisitor<Vertex<TVertexData, TEdgeData>>();

                foreach( var vertex in this.vertices )
                {
                    Traversal.BreadthFirst( vertex, vistor );

                    if( vistor.Count != vertices.Count )
                        return false;

                    vistor.Reset();
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/> is cyclic (contains cycles).
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if this Graph contains cycles;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsCyclic
        {
            get
            {
                int count = Traversal.Topological( this, EmptyVisitor<Vertex<TVertexData, TEdgeData>>.Instance );

                // If the visitor has not visited each and every vertex in the 
                // graph, it has cycles in it.
                return count < this.VertexCount;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/> is a tree.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> is this Graph is a tree;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsTree
        {
            get
            {
                return this.FaceCount == 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Graph{TVertexData, TEdgeData}"/>
        /// has an imbedding in the Sphere/Plane.
        /// </summary>
        /// <remarks>
        /// This operation only works for simple graphs.
        /// </remarks>
        /// <value>
        /// Returns <see langword="true"/> if this Graph is planar;
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsPlanar
        {
            get
            {
                int edgeCount = this.EdgeCount /*- selfLoopCount - multipleEdgeCount + multipleSelfLoopCount*/;
                int faceCount = 2 - this.VertexCount + edgeCount;
                if( edgeCount <= 1 )
                    faceCount = 1;

                if( faceCount == 1 )
                    return true;

                if( (2 * edgeCount) < (3 * faceCount) )
                    return false;

                return (2 * edgeCount) >= (this.Girth * faceCount);
            }
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph{TVertexData, TEdgeData}"/> class, using an instance
        /// of the <see cref="DefaultGraphDataFactory{VertexData, TEdgeData}"/>.
        /// </summary>
        /// <param name="isDirected">
        /// Specifies whether the new Graph is going to be a directed graph, or not.
        /// </param>
        public Graph( bool isDirected = true )
            : this( DefaultGraphDataFactory<TVertexData, TEdgeData>.Instance, isDirected )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph{TVertexData, TEdgeData}"/> class.
        /// </summary>
        /// <param name="dataFactory">
        /// The factory that is used to create TVertexData and TEdgeData.
        /// </param>
        /// <param name="isDirected">
        /// Specifies whether the new Graph is going to be a directed graph, or not.
        /// </param>
        public Graph( IGraphDataFactory<TVertexData, TEdgeData> dataFactory, bool isDirected = true )
        {
            Contract.Requires<ArgumentNullException>( dataFactory != null );

            this.dataFactory = dataFactory;
            this.isDirected = isDirected;
        }

        #endregion

        #region [ Methods ]

        #region > Sorting <

        /// <summary>
        /// Computes the topological sort of this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <remarks>
        /// The topological sort algorithm is only valid for a directed, acyclic (cycle free) graph.
        /// </remarks>
        /// <returns>A list of vertices in topological order.</returns>
        /// <exception cref="ArgumentException">
        /// The graph is not directed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The graph contains cycles.
        /// </exception>
        public IList<Vertex<TVertexData, TEdgeData>> TopologicalSort()
        {
            var visitor = new TrackingVisitor<Vertex<TVertexData, TEdgeData>>();
            
            int count = Traversal.Topological( this, visitor );
            if( count < this.VertexCount )
                throw new InvalidOperationException( GraphErrorStrings.GraphHasCycles );

            return visitor.TrackingList;
        }

        #endregion

        #region > Organisation <

        /// <summary>
        /// Searches for the specified Vertex{TVertexData, TEdgeData} and returns the zero-based index of the vertex
        /// in the vertex list of this Graph{TVertexData, TEdgeData}.
        /// </summary>
        /// <param name="vertex">
        /// The vertex to search.
        /// </param>
        /// <returns>
        /// The zero-based index of the vertex in the vertex list of this Graph{TVertexData, TEdgeData};
        /// or -1.
        /// </returns>
        [Pure]
        public int IndexOfVertex( Vertex<TVertexData, TEdgeData> vertex )
        {
            return this.vertices.IndexOf( vertex );
        }

        /// <summary>
        /// Finds all vertices that match the supplied <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">
        /// The predicate (condition) to use.
        /// </param>
        /// <returns>
        /// All vertices that match the specified <paramref name="predicate"/>.
        /// </returns>
        [Pure]
        public IList<Vertex<TVertexData, TEdgeData>> FindVertices( Predicate<TVertexData> predicate )
        {
            Contract.Requires<ArgumentNullException>( predicate != null );

            var matches = new List<Vertex<TVertexData, TEdgeData>>();

            foreach( var vertex in this.vertices )
            {
                if( predicate( vertex.Data ) )
                    matches.Add( vertex );
            }

            return matches;
        }

        /// <summary>
        /// Removes all vertices and edges from this Graph{TVertexData, TEdgeData}.
        /// </summary>
        public void Clear()
        {
            this.selfLoopCount = 0;
            this.multipleEdgeCount = 0;
            this.multipleSelfLoopCount = 0;

            this.RemoveAllEdges();
            this.RemoveAllVertices();

            this.vertices.Clear();
        }

        /// <summary>
        /// Removes all edges from this Graph{TVertexData, TEdgeData}.
        /// </summary>
        private void RemoveAllEdges()
        {
            var oldEdges = this.edges.ToArray();
            this.edges.Clear();

            foreach( var edge in oldEdges )
            {
                this.EdgeRemoved.Raise( this, edge );
            }
        }

        /// <summary>
        /// Removes all vertices from this Graph{TVertexData, TEdgeData}.
        /// </summary>
        private void RemoveAllVertices()
        {
            var oldVertices = this.vertices.ToArray();
            this.vertices.Clear();

            foreach( var vertex in vertices )
            {
                this.VertexRemoved.Raise( this, vertex );
            }
        }

        #region - Add -

        #region AddVertex

        /// <summary>
        /// Adds a vertex to the graph with the specified data item.
        /// </summary>
        /// <returns>The vertex created and added to the graph.</returns>
        public Vertex<TVertexData, TEdgeData> AddVertex()
        {
            return this.AddVertex( this.dataFactory.BuildVertexData() );
        }

        /// <summary>
        /// Adds a vertex to the graph with the specified data item.
        /// </summary>
        /// <param name="data">
        /// The data to store in the vertex.
        /// </param>
        /// <returns>The vertex created and added to the graph.</returns>
        public Vertex<TVertexData, TEdgeData> AddVertex( TVertexData data )
        {
            var vertex = new Vertex<TVertexData, TEdgeData>( data );

            this.vertices.Add( vertex );
            this.VertexAdded.Raise( this, vertex );
            return vertex;
        }

        /// <summary>
        /// Adds the vertex specified to the graph.
        /// </summary>
        /// <param name="vertex">
        /// The vertex to add.
        /// </param>
        public void AddVertex( Vertex<TVertexData, TEdgeData> vertex )
        {
            Contract.Requires<ArgumentException>( !this.ContainsVertex( vertex ), GraphErrorStrings.VertexAlreadyExistsInGraph );

            this.vertices.Add( vertex );
            this.VertexAdded.Raise( this, vertex );
        }

        #endregion

        #region AddEdge

        /// <summary>
        /// Adds a new <see cref="Edge{TVertexData, TEdgeData}"/> to this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <param name="from">The from vertex.</param>
        /// <param name="to">The to vertex.</param>
        /// <returns>The newly created edge.</returns>
        public Edge<TVertexData, TEdgeData> AddEdge( Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to )
        {
            var edge = new Edge<TVertexData, TEdgeData>( 
                from,
                to, 
                this.isDirected,
                this.dataFactory.BuildEdgeData()
            );
            
            this.AddEdge( edge );            
            return edge;
        }

        /// <summary>
        /// Adds a new <see cref="Edge{TVertexData, TEdgeData}"/> to this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <param name="from">The from vertex.</param>
        /// <param name="to">The to vertex.</param>
        /// <param name="edgeData">The data stored in the new Edge.</param>
        /// <returns>The newly created edge.</returns>
        public Edge<TVertexData, TEdgeData> AddEdge(
            Vertex<TVertexData, TEdgeData> from,
            Vertex<TVertexData, TEdgeData> to,
            TEdgeData edgeData )
        {
            var edge = new Edge<TVertexData, TEdgeData>( from, to, this.isDirected, edgeData );
            
            this.AddEdge( edge );            
            return edge;
        }

        /// <summary>
        /// Adds a new <see cref="Edge{TVertexData, TEdgeData}"/> to this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <param name="fromData">
        /// The from vertex.
        /// </param>
        /// <param name="toData">
        /// The to vertex.
        /// </param>
        /// <returns>The newly created edge.</returns> 
        /// <exception cref="ArgumentException">
        /// If no Vertex has been found that matches the given TVertexData.
        /// </exception>
        public Edge<TVertexData, TEdgeData> AddEdge( TVertexData fromData, TVertexData toData )
        {
            return this.AddEdge( fromData, toData, this.dataFactory.BuildEdgeData() );
        }

        /// <summary>
        /// Adds a new <see cref="Edge{TVertexData, TEdgeData}"/> to this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <param name="fromData">
        /// The from vertex.
        /// </param>
        /// <param name="toData"
        /// >The to vertex.
        /// </param>
        /// <param name="edgeData">
        /// The data to store in the edge.
        /// </param>
        /// <returns>The newly created edge.</returns> 
        /// <exception cref="ArgumentException">
        /// If no Vertex has been found that matches the given TVertexData.
        /// </exception>
        public Edge<TVertexData, TEdgeData> AddEdge( TVertexData fromData, TVertexData toData, TEdgeData edgeData )
        {
            var from = this.GetVertex( fromData );
            if( from == null )
                throw new ArgumentException( GraphErrorStrings.NoMatchingVertexFoundInGraph, "fromData" );

            var to = this.GetVertex( toData );
            if( to == null )
                throw new ArgumentException( GraphErrorStrings.NoMatchingVertexFoundInGraph, "toData" );

            var edge = new Edge<TVertexData, TEdgeData>( from, to, this.isDirected, edgeData );            
            this.AddEdge( edge );            
            return edge;
        }

        /// <summary>
        /// Adds the specified <see cref="Edge{TVertexData, TEdgeData}"/> to this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        private void AddEdge( Edge<TVertexData, TEdgeData> edge )
        {
            Contract.Requires<ArgumentNullException>( edge != null );
            Contract.Requires<ArgumentException>( edge.IsDirected == this.isDirected, GraphErrorStrings.EdgeGraphDirectedMismatch );
            Contract.Requires<ArgumentException>( this.vertices.Contains( edge.From ), GraphErrorStrings.VertexCouldNotBeFound );
            Contract.Requires<ArgumentException>( this.vertices.Contains( edge.To ), GraphErrorStrings.VertexCouldNotBeFound );
                  
            bool isSelfLoop = false;

            if( edge.From == edge.To )
            {
                if( !this.AllowsSelfLoops )
                    throw new InvalidOperationException( GraphErrorStrings.GraphDoesntAllowSelfLoopAdded );

                ++this.selfLoopCount;
                isSelfLoop = true;
            }

            if( edge.From.HasEmanatingEdgeTo( edge.To ) )
            {
                if( !this.AllowsMultipleEdges )
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            GraphErrorStrings.EdgeAlreadyExistsFromXToY,
                            edge.From.Data.ToString(),
                            edge.To.Data.ToString()
                        )
                    );
                }

                ++this.multipleEdgeCount;

                if( isSelfLoop )
                {
                    ++this.multipleSelfLoopCount;
                }
            }

            this.edges.Add( edge );
            AddEdgeToVertices( edge );
            this.EdgeAdded.Raise( this, edge );
        }

        #endregion

        #endregion

        #region - Remove -

        #region RemoveVertex

        /// <summary>
        /// Removes the specified vertex from the graph.
        /// </summary>
        /// <param name="vertex">The vertex to be removed.</param>
        /// <returns>
        /// A value indicating whether the vertex was found (and removed) in the graph.
        /// </returns>
        /// <exception cref="ArgumentNullException"> If <paramref name="vertex"/> is null. </exception>
        public bool RemoveVertex( Vertex<TVertexData, TEdgeData> vertex )
        {
            Contract.Requires<ArgumentNullException>( vertex != null );

            if( this.vertices.Remove( vertex ) )
            {
                // Delete all the edges in which this vertex forms part of
                foreach( var edge in vertex.IncidentEdges.ToArray() )
                {
                    this.RemoveEdge( edge );
                }

                this.VertexRemoved.Raise( this, vertex );
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the vertex with the specified data from the graph.
        /// </summary>
        /// <param name="vertexData">
        /// The data of the vertex to remove.
        /// </param>
        /// <returns>
        /// A value indicating whether a vertex with the value specified was found (and removed) in the graph.
        /// </returns>
        public bool RemoveVertex( TVertexData vertexData )
        {
            for( int i = 0; i < vertices.Count; ++i )
            {
                var vertex = vertices[i];

                if( vertex.Data != null && vertex.Data.Equals( vertexData ) )
                {
                    return this.RemoveVertex( vertex );
                }
            }

            return false;
        }

        #endregion

        #region RemoveEdge

        /// <summary>
        /// Removes the edge specified from the graph.
        /// </summary>
        /// <exception cref="ArgumentNullException"> If <paramref name="edge"/> is null. </exception>
        /// <param name="edge">The edge to be removed.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified <paramref name="edge"/> was removed;
        /// or otherwise <see langword="false"/> if the <paramref name="edge"/> could not be found (and removed).
        /// </returns>
        public bool RemoveEdge( Edge<TVertexData, TEdgeData> edge )
        {
            Contract.Requires<ArgumentNullException>( edge != null );

            if( this.edges.Remove( edge ) )
            {
                edge.From.RemoveEdge( edge );
                edge.To.RemoveEdge( edge );

                this.EdgeRemoved.Raise( this, edge );
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes an edge that starts and ends at the specified vertices from this Graph.
        /// </summary>
        /// <exception cref="ArgumentNullException"> If <paramref name="from"/> or <paramref name="to"/> is null. </exception>
        /// <param name="from">
        /// The from vertex.
        /// </param>
        /// <param name="to">
        /// The to vertex.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified edge was removed;
        /// or otherwise <see langword="false"/> if the edge could not be found (and removed).
        /// </returns>
        public bool RemoveEdge( Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to )
        {
            Contract.Requires<ArgumentNullException>( from != null );
            Contract.Requires<ArgumentNullException>( to != null );

            if( this.isDirected )
            {
                for( int i = 0; i < edges.Count; ++i )
                {
                    var edge = edges[i];

                    if( (edge.From == from) && (edge.To == to) )
                    {
                        return this.RemoveEdge( edge );
                    }
                }
            }
            else
            {
                for( int i = 0; i < edges.Count; ++i )
                {
                    var edge = edges[i];

                    if( ((edge.From == from) && (edge.To == to)) ||
                        ((edge.From == to) && (edge.To == from)) )
                    {
                        return RemoveEdge( edge );
                    }
                }
            }

            return false;
        }

        #endregion

        #endregion

        #region - Contains -

        #region ContainsVertex

        /// <summary>
        /// Determines whether this graph contains the specified vertex.
        /// </summary>
        /// <param name="vertex">
        /// The vertex to look for.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if this Graph contains the given vertex;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool ContainsVertex( Vertex<TVertexData, TEdgeData> vertex )
        {
            return this.vertices.Contains( vertex );
        }

        /// <summary>
        /// Determines whether the specified item is contained in the Fraph.
        /// </summary>
        /// <param name="vertexData">
        /// The data of the Vertex to look for.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if this Graph contains a vertex that
        /// has the given <paramref name="vertexData"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool ContainsVertex( TVertexData vertexData )
        {
            foreach( Vertex<TVertexData, TEdgeData> vertex in vertices )
            {
                if( vertex.Data != null && vertex.Data.Equals( vertexData ) )
                    return true;
            }

            return false;
        }

        #endregion

        #region ContainsEdge

        /// <summary>
        /// Determines whether the vertex with the specified from value has an edge to a vertex with the specified to value.
        /// </summary>
        /// <param name="fromValue">The from vertex value.</param>
        /// <param name="toValue">The to vertex value.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the vertex with the specified from value has
        /// an edge to a vertex with the specified to value; otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool ContainsEdge( TVertexData fromValue, TVertexData toValue )
        {
            if( this.isDirected )
            {
                foreach( var edge in this.edges )
                {
                    if( edge.From.Data.Equals( fromValue ) && edge.To.Data.Equals( toValue ) )
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach( var edge in this.edges )
                {
                    if( (edge.From.Data.Equals( fromValue ) && edge.To.Data.Equals( toValue )) ||
                        (edge.From.Data.Equals( toValue ) && edge.To.Data.Equals( fromValue )) )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified vertex has a edge to the to vertex.
        /// </summary>
        /// <param name="from">The from vertex.</param>
        /// <param name="to">The to vertex.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified from vertex has an edge to the to vertex;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool ContainsEdge( Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to )
        {
            if( this.isDirected )
            {
                return from.HasEmanatingEdgeTo( to );
            }
            else
            {
                return from.HasIncidentEdgeWith( to );
            }
        }

        /// <summary>
        /// Determines whether the specified edge is contained in this graph.
        /// </summary>
        /// <param name="edge">The edge to look for.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the specified edge is contained in the graph;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool ContainsEdge( Edge<TVertexData, TEdgeData> edge )
        {
            return this.edges.Contains( edge );
        }

        #endregion

        #endregion

        #region - Get -

        /// <summary>
        /// Returns to get the first occurence of a Vertex
        /// that has the specified TVertexData.
        /// </summary>
        /// <param name="vertexData">
        /// The data of the vertex to search for.
        /// </param>
        /// <returns>
        /// The vertex; or null if not found.
        /// </returns>
        [Pure]
        public Vertex<TVertexData, TEdgeData> GetVertex( TVertexData vertexData )
        {
            if( vertexData == null )
            {
                foreach( Vertex<TVertexData, TEdgeData> vertex in this.Vertices )
                {
                    if( vertex.Data == null )
                    {
                        return vertex;
                    }
                }
            }
            else
            {
                foreach( Vertex<TVertexData, TEdgeData> vertex in this.Vertices )
                {
                    if( vertexData.Equals( vertex.Data ) )
                    {
                        return vertex;
                    }
                }
            }

            return null;
        }
        
        /// <summary>
        /// Gets the Vertex{TVertexData, TEdgeData} at the given zero-based <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the Vertex{TVertexData, TEdgeData} to get.
        /// </param>
        /// <returns>
        /// The respective Vertex{TVertexData, TEdgeData}.
        /// </returns>
        [Pure]
        public Vertex<TVertexData, TEdgeData> GetVertexAt( int index )
        {
            return this.vertices[index];
        }

        /// <summary>
        /// Gets the edge specified by the two vertices.
        /// </summary>
        /// <param name="fromData">The data contained by the from vertex.</param>
        /// <param name="toData">The data contained by the to vertex.</param>
        /// <returns>The edge between the two specified vertices if found.</returns>
        [Pure]
        public Edge<TVertexData, TEdgeData> GetEdge( TVertexData fromData, TVertexData toData )
        {
            var from = this.GetVertex( fromData );
            var to = this.GetVertex( toData );

            if( from == null || to == null )
                return null;

            return this.GetEdge( from, to );
        }

        /// <summary>
        /// Gets the edge specified by the two vertices.
        /// </summary>
        /// <param name="from">The from vertex.</param>
        /// <param name="to">The to vertex.</param>
        /// <returns>The edge between the two specified vertices if found.</returns>
        [Pure]
        public Edge<TVertexData, TEdgeData> GetEdge( Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to )
        {
            return from.GetEmanatingEdgeTo( to );
        }

        /// <summary>
        /// Gets the Edge{TVertexData, TEdgeData} at the given zero-based <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the Edge{TVertexData, TEdgeData} to get.
        /// </param>
        /// <returns>
        /// The respective Edge{TVertexData, TEdgeData}.
        /// </returns>
        [Pure]
        public Edge<TVertexData, TEdgeData> GetEdgeAt( int index )
        {
            return this.edges[index];
        }

        #endregion

        #region CopyTo

        /// <summary>
        /// Copies the elements of this <see cref="Graph{TVertexData, TEdgeData}"/> to the specified array,
        /// starting at a particular array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from <see cref="Graph{TVertexData, TEdgeData}"/>. The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        /// <exception cref="System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than 
        /// the length of array.-or-The number of elements in the source <see cref="Graph{TVertexData, TEdgeData}"/> is greater than the available space from arrayIndex to the end of the destination array.-or-TriangleType TVertexData cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo( TVertexData[] array, int arrayIndex )
        {
            Contract.Requires<ArgumentNullException>( array != null );
            Contract.Requires<ArgumentException>( (array.Length - arrayIndex) >= this.VertexCount );

            int counter = arrayIndex;

            foreach( Vertex<TVertexData, TEdgeData> v in vertices )
            {
                array.SetValue( v.Data, counter );
                ++counter;
            }
        }

        #endregion

        #endregion

        #region > Utility <

        #region - Cloning -

        /// <summary>
        /// Creates a new Graph{TVertexData, TEdgeData} that contains the vertices of this Graph,
        /// and has the same isDirected setting.
        /// </summary>
        /// <returns>
        /// A newly cloned Graph.
        /// </returns>
        public Graph<TVertexData, TEdgeData> CloneWithVertices()
        {
            var graph = new Graph<TVertexData, TEdgeData>( this.isDirected );

            this.CloneVertices( graph );

            return graph;
        }

        /// <summary>
        /// Clones the vertices of this Graph and inserts them into the given <paramref name="outputGraph"/>.
        /// </summary>
        /// <param name="outputGraph">
        /// The Graph into which the vertices should be inserted into.
        /// </param>
        private void CloneVertices( Graph<TVertexData, TEdgeData> outputGraph )
        {
            if( typeof( ICloneable ).IsAssignableFrom( typeof( TVertexData ) ) )
            {
                foreach( var vertex in this.vertices )
                {
                    var data = (TVertexData)((ICloneable)vertex.Data).Clone();
                    outputGraph.AddVertex( data );
                }
            }
            else
            {
                foreach( var vertex in this.vertices )
                {
                    outputGraph.AddVertex( vertex.Data );
                }
            }
        }

        #endregion

        /// <summary>
        /// Utility method that adds the edge to the vertices in the edge.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        private static void AddEdgeToVertices( Edge<TVertexData, TEdgeData> edge )
        {
            Debug.Assert( edge != null, "The given Edge is null." );
            Debug.Assert( edge.From != null, "The From vertex of given Edge is null." );
            Debug.Assert( edge.To != null, "The To vertex of the given Edge is null." );

            if( edge.To == edge.From )
            {
                edge.From.AddEdge( edge );
            }
            else
            {
                edge.From.AddEdge( edge );
                edge.To.AddEdge( edge );
            }
        }

        /// <summary>
        /// Utlity method that gets any vertex of the Graph.
        /// </summary>
        /// <returns>The first Vertex in the Graph.</returns>
        private Vertex<TVertexData, TEdgeData> GetAnyVertex()
        {
            System.Diagnostics.Debug.Assert( vertices.Count > 0, "The Graph contains no vertices." );
            return vertices[0];
        }

        #endregion

        #region > Impls/Overrides <

        /// <summary>
        /// Returns a human-readable string representation of this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <returns>
        /// A human-readable string representation of this <see cref="Graph{TVertexData, TEdgeData}"/>.
        /// </returns>
        public override string ToString()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            var sb = new System.Text.StringBuilder();

            sb.AppendFormat(
                culture,
                "{0} Graph  {1} Vertices, {2} Edges, {3} Faces \n\n",
                this.isDirected ? "directed" : "indirected",
                this.VertexCount.ToString( culture ),
                this.EdgeCount.ToString( culture ),
                this.FaceCount.ToString( culture )
            );

            foreach( Vertex<TVertexData, TEdgeData> vertex in this.Vertices )
            {
                sb.AppendLine( vertex.ToString() );
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the data
        /// stored by the vertices of the Grapth.
        /// </summary>
        /// <returns>A new enumerator that iterates over the data in this Graph.</returns>
        public IEnumerator<TVertexData> GetEnumerator()
        {
            foreach( Vertex<TVertexData, TEdgeData> vertex in this.Vertices )
            {
                yield return vertex.Data;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the data
        /// stored by the vertices of the Grapth.
        /// </summary>
        /// <returns>A new enumerator that iterates over the data in this Graph.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #endregion

        #region [ Fields ]
        
        /// <summary>
        /// Stores the number of edges that start and end at the same vertex in this Graph.
        /// </summary>
        private int selfLoopCount;

        /// <summary>
        /// Stores the number of multiple edges in this Graph.
        /// </summary>
        private int multipleEdgeCount;

        /// <summary>
        /// Stores the number of multiple self loops in this Graph.
        /// </summary>
        private int multipleSelfLoopCount;

        /// <summary>
        /// Indicates whether this Graph is a directed Graph or not.
        /// </summary>
        private readonly bool isDirected;

        /// <summary>
        /// The factory that is used to create TVertexData and TEdgeData.
        /// </summary>
        private readonly IGraphDataFactory<TVertexData, TEdgeData> dataFactory;

        /// <summary>
        /// Represents the VertexSet of this Graph.
        /// </summary>
        private readonly List<Vertex<TVertexData, TEdgeData>> vertices = new List<Vertex<TVertexData,TEdgeData>>();

        /// <summary>
        /// Represents the EdgeSet of this Graph.
        /// </summary>
        private readonly List<Edge<TVertexData, TEdgeData>> edges = new List<Edge<TVertexData,TEdgeData>>();

        #endregion
    }
}