// <copyright file="Vertex.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Math.Graph.Vertex{TVertexData, TEdgeData} class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Math.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Atom.Diagnostics.Contracts;

    /// <summary> 
    /// Represents a vertex in a <see cref="Graph{TVertexData, TEdgeData}"/>.
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of data stored in the Vertex.
    /// </typeparam>
    /// <typeparam name="TEdgeData">
    /// The type of data stored within the edges of the Graph.
    /// </typeparam>
    [Serializable]
    public sealed class Vertex<TVertexData, TEdgeData> : ICloneable
        where TVertexData : IEquatable<TVertexData>
    {
        #region [ Properties ]

        /// <summary> 
        /// Gets the edges incident(incoming) on this Vertex.
        /// </summary>
        /// <value>An enumeration that contains the edges that come in to this Vertex.</value>
        public IEnumerable<Edge<TVertexData, TEdgeData>> IncidentEdges
        {
            get
            {
                return incidentEdges;
            }
        }

        /// <summary>
        /// Gets the emanating(outgoing) edges on this Vertex. 
        /// </summary>
        /// <value>An enumeration that contains the edges that come out of this Vertex.</value>
        public IEnumerable<Edge<TVertexData, TEdgeData>> EmanatingEdges
        {
            get
            {
                return emanatingEdges;
            }
        }

        /// <summary>
        /// Gets or sets the data contained in this <see cref="Vertex{TVertexData, TEdgeData}"/>. 
        /// </summary>
        /// <remarks>
        /// If the specified TVertexData object implements <see cref="IOwnedBy"/>{Vertex{TVertexData, TEdgeData}} then the owner
        /// is set to this Vertex.
        /// </remarks>
        /// <value>The additonal data that has been associated with this Vertex.</value>
        public TVertexData Data
        {
            get
            {
                return this._data;
            }

            set
            {
                var owned = value as IOwnedBy<Vertex<TVertexData, TEdgeData>>;

                if( owned != null )
                {
                    owned.Owner = this;
                }

                this._data = value;
            }
        }

        /// <summary> 
        /// Gets a value that represents the number of edges that are incident on this Vertex. 
        /// </summary>
        /// <remarks>
        /// Loops count for two edges.
        /// </remarks>
        /// <value>The number of edges that are incident on this Vertex.</value>
        public int Degree
        {
            get
            {
                int degree = 0;

                foreach( var edge in emanatingEdges )
                {
                    if( edge.To == edge.From )
                        degree += 2;
                    else
                        degree += 1;
                }

                return degree;
            }
        }

        /// <summary>
        /// Gets the number of the incoming <see cref="Edge{TVertexData, TEdgeData}"/>s on this Vertex.
        /// </summary>
        /// <value>The number of incoming edges resident on the Vertex.</value>
        public int IncomingEdgeCount
        {
            get
            {
                return incidentEdges.Count - emanatingEdges.Count;
            }
        }

        /// <summary>
        /// Gets the number of outgoing <see cref="Edge{TVertexData, TEdgeData}"/>s on this Vertex.
        /// </summary>
        /// <value>The number of edges that are going out of this Vertex.</value>
        public int OutgoingEdgeCount
        {
            get
            {
                return emanatingEdges.Count;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex{TVertexData, TEdgeData}"/> class.
        /// </summary>
        public Vertex()
        {
            this.incidentEdges  = new List<Edge<TVertexData, TEdgeData>>();
            this.emanatingEdges = new List<Edge<TVertexData, TEdgeData>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex{TVertexData, TEdgeData}"/> class.
        /// </summary>
        /// <param name="data">
        /// The data contained in the new Vertex{TVertexData, TEdgeData}.
        /// </param>
        public Vertex( TVertexData data )
            : this()
        {
            this.Data = data;
        }

        #endregion

        #region [ Methods ]

        #region - Has -

        /// <summary>
        /// Determines whether this vertex has an emanating(outgoing) edge to the specified vertex.
        /// </summary>
        /// <param name="vertex"> The vertex to test connectivity with. </param>
        /// <returns>
        /// <c>true</c> if this vertex has an emanating edge to the specified vertex; otherwise, <c>false</c>.
        /// </returns>
        [Pure]
        public bool HasEmanatingEdgeTo( Vertex<TVertexData, TEdgeData> vertex )
        {
            Contract.Requires<ArgumentNullException>( vertex != null );

            if( vertex == this )
            {
                foreach( var edge in this.emanatingEdges )
                {
                    if( edge.To == this && edge.From == this )
                        return true;
                }
            }
            else
            {
                foreach( var edge in this.emanatingEdges )
                {
                    if( edge.IsDirected )
                    {
                        if( edge.To == vertex )
                            return true;
                    }
                    else
                    {
                        if( edge.To == vertex || edge.From == vertex )
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether this vertex has an incident(incoming) edge 
        /// that connects the specified vertex with this vertex.
        /// </summary>
        /// <param name="vertex">The vertex to test connectivity.</param>
        /// <returns>
        /// <c>true</c> if [has incident edge with] [the specified from vertex]; otherwise, <c>false</c>.
        /// </returns>
        [Pure]
        public bool HasIncidentEdgeWith( Vertex<TVertexData, TEdgeData> vertex )
        {
            Contract.Requires<ArgumentNullException>( vertex != null );

            if( vertex == this )
            {
                foreach( var edge in this.emanatingEdges )
                {
                    if( edge.To == this && edge.From == this )
                        return true;
                }
            }
            else
            {
                foreach( var edge in this.incidentEdges )
                {
                    if( edge.From == vertex || edge.To == vertex )
                        return true;
                }
            }

            return false;
        }

        #endregion

        #region - Get -

        /// <summary>
        /// Gets the emanating(outgoing) edge to the specified <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// The emanating edge to the vertex specified if found; otherwise null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <paramref name="vertex"/> is null.
        /// </exception>
        [Pure]
        public Edge<TVertexData, TEdgeData> GetEmanatingEdgeTo( Vertex<TVertexData, TEdgeData> vertex )
        {
            Contract.Requires<ArgumentNullException>( vertex != null );

            foreach( var edge in this.emanatingEdges )
            {
                if( edge.IsDirected )
                {
                    if( edge.To == vertex )
                    {
                        return edge;
                    }
                }
                else
                {
                    if( edge.From == vertex || edge.To == vertex )
                    {
                        return edge;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the incident(incoming) edge to the specified vertex.
        /// </summary>
        /// <param name="vertex">The to vertex.</param>
        /// <returns>
        /// The incident edge to the vertex specified if found; otherwise null.
        /// </returns>     
        ///  <exception cref="ArgumentNullException">
        /// If the specified <paramref name="vertex"/> is null.
        /// </exception>
        [Pure]
        public Edge<TVertexData, TEdgeData> GetIncidentEdgeWith( Vertex<TVertexData, TEdgeData> vertex )
        {
            Contract.Requires<ArgumentNullException>( vertex != null );

            foreach( Edge<TVertexData, TEdgeData> edge in this.incidentEdges )
            {
                if( edge.To == vertex || edge.From == vertex )
                {
                    return edge;
                }
            }

            return null;
        }

        /// <summary>
        /// Receives the emanating edge at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The index of the edge to receive.
        /// </param>
        /// <returns>The edge to receive.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If the given <paramref name="index"/> is invalid.
        /// </exception>
        [Pure]
        public Edge<TVertexData, TEdgeData> GetEmanatingEdge( int index )
        {
            return this.emanatingEdges[index];
        }

        /// <summary>
        /// Receives the incident edge at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The index of the edge to receive.
        /// </param>
        /// <returns>The edge to receive.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If the given <paramref name="index"/> is invalid.
        /// </exception>
        [Pure]
        public Edge<TVertexData, TEdgeData> GetIncidentEdge( int index )
        {
            return this.incidentEdges[index];
        }

        #endregion

        #region - Remove -

        /// <summary>
        /// Removes the edge specified from the vertex.
        /// </summary>
        /// <param name="edge">The edge to be removed.</param>
        internal void RemoveEdge( Edge<TVertexData, TEdgeData> edge )
        {
            Debug.Assert( edge != null, "Edge is null." );

            bool wasRemoved = this.incidentEdges.Remove( edge );
            Debug.Assert( wasRemoved, "Edge not found on vertex in RemoveEdgeFromVertex." );

            if( edge.IsDirected )
            {
                if( edge.From == this )
                {
                    emanatingEdges.Remove( edge );
                }
            }
            else
            {
                emanatingEdges.Remove( edge );
            }
        }

        #endregion

        #region - Add -

        /// <summary>
        /// Adds the edge to this <see cref="Vertex{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        internal void AddEdge( Edge<TVertexData, TEdgeData> edge )
        {
            Debug.Assert( edge != null, "Edge is null." );

            if( edge.IsDirected )
            {
                if( edge.From == this )
                {
                    emanatingEdges.Add( edge );
                }
            }
            else
            {
                emanatingEdges.Add( edge );
            }

            incidentEdges.Add( edge );
        }

        #endregion

        #region > Impls/Overrides <

        /// <summary>
        /// Returns a string representation of this <see cref="Vertex{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <returns>
        /// A humen readable description of the Vertex.
        /// </returns>
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            sb.Append( "Vertex " );
            if( this.Data == null )
            {
                sb.Append( "(no data" );
            }
            else
            {
                sb.Append( '(' );
                sb.Append( this.Data.ToString() );
            }

            sb.Append( "):" );

            sb.AppendLine();
            sb.AppendLine( "  Emanating Edges:" );
            for( int i = 0; i < emanatingEdges.Count; ++i )
            {
                sb.Append( "     " );
                sb.AppendLine( emanatingEdges[i].ToString() );
            }

            sb.AppendLine( "  Incident Edges:" );
            for( int i = 0; i < incidentEdges.Count; ++i )
            {
                sb.Append( "     " );
                sb.AppendLine( incidentEdges[i].ToString() );
            }

            return sb.ToString();
        }

        #region - Clone -

        /// <summary>
        /// Returns a clone of this <see cref="Vertex{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <returns>
        /// The cloned <see cref="Vertex{TVertexData, TEdgeData}"/>.
        /// </returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Returns a clone of this <see cref="Vertex{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="Data"/> of the Vertex is cloned if it implements <see cref="ICloneable"/>.
        /// </remarks>
        /// <returns>
        /// The cloned <see cref="Vertex{TVertexData, TEdgeData}"/>.
        /// </returns>
        public Vertex<TVertexData, TEdgeData> Clone()
        {
            if( this.Data == null || !typeof( ICloneable ).IsAssignableFrom( this.Data.GetType() ) )
                return new Vertex<TVertexData, TEdgeData>( this.Data );
            else
                return new Vertex<TVertexData, TEdgeData>( (TVertexData)((ICloneable)this.Data).Clone() );
        }

        #endregion

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Represents the storage field of the <see cref="Data"/> property.
        /// </summary>
        private TVertexData _data;

        /// <summary>
        /// The incoming edges.
        /// </summary>
        private readonly List<Edge<TVertexData, TEdgeData>> incidentEdges;

        /// <summary>
        /// The outgoing edges.
        /// </summary>
        private readonly List<Edge<TVertexData, TEdgeData>> emanatingEdges;

        #endregion
    }
}
