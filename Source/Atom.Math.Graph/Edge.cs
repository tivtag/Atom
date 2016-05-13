// <copyright file="Edge.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Edge{TVertexData, TEdgeData} class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents an weighted edge in a graph. 
    /// An edge connects two vertices (<see cref="Vertex{TVertexData, TEdgeData}"/>).
    /// This class can't be inherited.
    /// </summary>
    /// <typeparam name="TVertexData">
    /// The type of data contained within the vertices of the Edge. 
    /// </typeparam>
    /// <typeparam name="TEdgeData">
    /// The type of data stored within the Edge.
    /// </typeparam>
    public sealed class Edge<TVertexData, TEdgeData>
        where TVertexData : IEquatable<TVertexData>
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the vertex this <see cref="Edge{TVertexData, TEdgeData}"/> starts from.
        /// </summary>
        /// <value>The vertex this Edge goes away from.</value>
        public Vertex<TVertexData, TEdgeData> From
        {
            get
            {
                Contract.Ensures( Contract.Result<Vertex<TVertexData, TEdgeData>>() != null );

                return this.from;
            }
        }

        /// <summary> 
        /// Gets the vertex this <see cref="Edge{TVertexData, TEdgeData}"/> ends at.
        /// </summary>
        /// <value>The vertex this Edge goes to.</value>
        public Vertex<TVertexData, TEdgeData> To
        {
            get
            {
                Contract.Ensures( Contract.Result<Vertex<TVertexData, TEdgeData>>() != null );

                return this.to;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Edge{TVertexData, TEdgeData}"/> is directed.
        /// </summary>
        /// <remarks>
        /// An edge which is directed only goes from his <see cref="From"/> to his <see cref="To"/>.
        /// An undirected vertex goes in both directions.
        /// </remarks>
        /// <value>States whether this Edge is a directed Edge.</value>
        public bool IsDirected
        {
            get
            {
                return this.isDirected;
            }
        }

        /// <summary>
        /// Gets or sets the custom data associated with this <see cref="Edge{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <remarks>
        /// If the specified TEdgeData object implements <see cref="IOwnedBy"/>{Edge{TVertexData, TEdgeData}} then the owner
        /// is set to this Edge.
        /// </remarks>
        /// <value>The additional data associated with this Edge.</value>
        public TEdgeData Data
        {
            get
            {
                return this._data;
            }

            set
            {
                var owned = value as IOwnedBy<Edge<TVertexData, TEdgeData>>;

                if( owned != null )
                {
                    owned.Owner = this;
                }

                this._data = value;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge{TVertexData, TEdgeData}"/> class.
        /// </summary>
        /// <param name="fromVertex"> 
        /// The vertex the new Edge starts from.
        /// </param>
        /// <param name="toVertex"> 
        /// The vertex the new Edge ends at. 
        /// </param>
        /// <param name="data">
        /// The data associated with the new Edge.
        /// </param>
        /// <param name="isDirected"> 
        /// Specifies whether the new Edge is a directed edge.
        /// </param>
        /// <exception cref="ArgumentNullException"> 
        /// If <paramref name="fromVertex"/> or <paramref name="toVertex"/> is null. 
        /// </exception>
        internal Edge(
            Vertex<TVertexData, TEdgeData> fromVertex,
            Vertex<TVertexData, TEdgeData> toVertex,
            bool isDirected,
            TEdgeData data )
        {
            Contract.Requires<ArgumentNullException>( fromVertex != null );
            Contract.Requires<ArgumentNullException>( toVertex != null );

            this.isDirected = isDirected;
            this.from = fromVertex;
            this.to = toVertex;
            this.Data = data;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the partner vertex in this Edge relationship.
        /// </summary>
        /// <exception cref="ArgumentException"> 
        /// If the specified <paramref name="vertex"/> is not part of this Edge. 
        /// </exception>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// The partner of the vertex specified in this edge relationship.
        /// </returns>
        [Pure]
        public Vertex<TVertexData, TEdgeData> GetPartnerVertex( Vertex<TVertexData, TEdgeData> vertex )
        {
            if( this.from == vertex )
            {
                return this.to;
            }
            else if( this.to == vertex )
            {
                return this.from;
            }
            else
            {
                throw new ArgumentException( GraphErrorStrings.VertexNotPartOfEdge );
            }
        }

        /// <summary>
        /// Returns a string representation of this <see cref="Edge{TVertexData, TEdgeData}"/>.
        /// </summary>
        /// <returns>A string representation of this <see cref="Edge{TVertexData, TEdgeData}"/>.</returns>
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append( '[' );

            if( this.IsDirected )
                sb.Append( "Edge from Vertex" );
            else
                sb.Append( "Edge conntecting Vertex" );

            if( this.from.Data == null )
            {
                sb.Append( "(no data)" );
            }
            else
            {
                sb.Append( '(' );
                sb.Append( this.from.Data.ToString() );
                sb.Append( ')' );
            }

            if( this.IsDirected )
                sb.Append( " to Vertex" );
            else
                sb.Append( " with Vertex" );

            if( this.to.Data == null )
            {
                sb.Append( "(no data)" );
            }
            else
            {
                sb.Append( '{' );
                sb.Append( this.to.Data.ToString() );
                sb.Append( '}' );
            }

            sb.Append( ']' );

            return sb.ToString();
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Represents the storage field of the <see cref="Data"/> property.
        /// </summary>
        private TEdgeData _data;

        /// <summary>
        /// Stores the vertex this <see cref="Edge{TVertexData, TEdgeData}"/> starts from.
        /// </summary>
        private readonly Vertex<TVertexData, TEdgeData> from;

        /// <summary>
        /// Stores the vertex this <see cref="Edge{TVertexData, TEdgeData}"/> ends at.
        /// </summary>
        private readonly Vertex<TVertexData, TEdgeData> to;

        /// <summary>
        /// States whether this Edge{TVertexData, TEdgeData} is directed.
        /// </summary>
        private readonly bool isDirected;

        #endregion
    }
}
