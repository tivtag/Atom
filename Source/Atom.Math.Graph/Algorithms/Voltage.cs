// <copyright file="Voltage.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Graph.Algorithms.Voltage class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math.Graph.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Atom.Math.Graph.Data;

    /// <summary>
    /// Provides a mechanism to receive the (right) derivation of a Voltage Graph.
    /// </summary>
    public static class Voltage
    {
        /// <summary>
        /// Returns the (right) derivation of the given voltage graph.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored in the vertices. 
        /// Must implement <see cref="INameable"/> and <see cref="IEquatable{TVertexData}"/>.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored in the edges. 
        /// Must implement <see cref="IVoltageData"/>.
        /// </typeparam>
        /// <param name="voltageGraph">
        /// The input voltage graph.
        /// </param>
        /// <param name="voltageOrder">
        /// The order of the input voltage graph.
        /// </param>
        /// <returns>
        /// The derived voltage graph.
        /// </returns>
        public static Graph<TVertexData, TEdgeData> Derive<TVertexData, TEdgeData>(
            Graph<TVertexData, TEdgeData> voltageGraph,
            int voltageOrder )
            where TVertexData : IEquatable<TVertexData>, INameable, new()
            where TEdgeData : IVoltageData
        {
            // Verify the input.
            Verify( voltageGraph, voltageOrder );

            // Lets start the work :).
            var graph = new Graph<TVertexData, TEdgeData>( true ) {
                AllowsSelfLoops = true
            };

            // V = VertexSet of input graph
            // E = EdgeSet of input graph
            // L = Voltage Group

            // The VertexMap maps old vertices to new vertices.
            var vertexMap = new Dictionary<Vertex<TVertexData, TEdgeData>, Vertex<TVertexData, TEdgeData>[]>( voltageGraph.VertexCount );

            // VertexSet V' = V x L:
            foreach( var vertex in voltageGraph.Vertices )
            {
                var derivedVertices = new Vertex<TVertexData, TEdgeData>[voltageOrder];

                for( int i = 0; i < voltageOrder; ++i )
                {
                    TVertexData data = new TVertexData() {
                        Name = vertex.Data.Name + "_" + i.ToString( CultureInfo.InvariantCulture )
                    };

                    var derivedVertex = new Vertex<TVertexData, TEdgeData>( data );

                    derivedVertices[i] = derivedVertex;
                    graph.AddVertex( derivedVertex );
                }

                vertexMap.Add( vertex, derivedVertices );
            }

            // EdgeSet E' = E x L
            foreach( var edge in voltageGraph.Edges )
            {
                int voltage = edge.Data.Voltage;

                var derivedStartVertices = vertexMap[edge.From];
                var derivedEndVertices   = vertexMap[edge.To];

                for( int i = 0; i < voltageOrder; ++i )
                {
                    var derivedStartVertex = derivedStartVertices[i];
                    var derivedEndVertex   = derivedEndVertices[(i + voltage) % voltageOrder];

                    graph.AddEdge( derivedStartVertex, derivedEndVertex );
                }
            }

            return graph;
        }

        /// <summary>
        /// Verifies that the given input voltage data is correct.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored in the vertices. 
        /// Must implement <see cref="INameable"/> and <see cref="IEquatable{TVertexData}"/>.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored in the edges. 
        /// Must implement <see cref="IVoltageData"/>.
        /// </typeparam>
        /// <param name="voltageGraph">
        /// The input voltage graph.
        /// </param>
        /// <param name="voltageOrder">
        /// The order of the input voltage graph.
        /// </param>
        private static void Verify<TVertexData, TEdgeData>( Graph<TVertexData, TEdgeData> voltageGraph, int voltageOrder )
            where TVertexData : IEquatable<TVertexData>, INameable, new()
            where TEdgeData : IVoltageData
        {
            Contract.Requires<ArgumentNullException>( voltageGraph != null );
            Contract.Requires( voltageOrder >= 1 );
            Contract.Requires( voltageGraph.IsDirected );

            foreach( var edge in voltageGraph.Edges )
            {
                VerifyEdge( edge, voltageOrder );
            }
        }

        /// <summary>
        /// Verifies that the given edge of a voltage graph has valid data.
        /// </summary>
        /// <typeparam name="TVertexData">
        /// The type of data stored in the vertices. 
        /// Must implement <see cref="INameable"/> and <see cref="IEquatable{TVertexData}"/>.
        /// </typeparam>
        /// <typeparam name="TEdgeData">
        /// The type of data stored in the edges. 
        /// Must implement <see cref="IVoltageData"/>.
        /// </typeparam>
        /// <param name="edge">
        /// The voltage edge to verify.
        /// </param>
        /// <param name="voltageOrder">
        /// The order of the input voltage graph.
        /// </param>
        private static void VerifyEdge<TVertexData, TEdgeData>( Edge<TVertexData, TEdgeData> edge, int voltageOrder )
            where TVertexData : IEquatable<TVertexData>
            where TEdgeData : IVoltageData
        {
            Contract.Requires<ArgumentException>( edge.Data != null, GraphErrorStrings.EdgeDataIsNull );

            int voltage = edge.Data.Voltage;

            if( voltage < 0 || voltage >= voltageOrder )
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        GraphErrorStrings.VoltageXOfVoltageGraphNotPartOfSpecifiedVoltageGroupOrderY,
                        voltage.ToString( CultureInfo.CurrentCulture ),
                        voltageOrder.ToString( CultureInfo.CurrentCulture )
                    ),
                    "voltageGraph"
                );
            }
        }
    }
}
