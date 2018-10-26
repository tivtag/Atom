// <copyright file="Polygon2.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Math.Polygon2 class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a polygon in two dimensional space.
    /// </summary>
    public class Polygon2 : IEnumerable<Vector2>, ICultureSensitiveToStringProvider
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the reference of the List
        /// that contains the vertices of the <see cref="Polygon2"/>.
        /// </summary>
        /// <value>The internal list of vertices.</value>
        public List<Vector2> Vertices
        {
            get
            {
                return this.vertices;
            }
        }

        /// <summary>
        /// Gets the number of vertices the <see cref="Polygon2"/> consists of.
        /// </summary>
        /// <value>The total number of vertices.</value>
        public int VertexCount
        {
            get 
            {
                return this.vertices.Count;
            }
        }

        /// <summary>
        /// Gets or sets the Vertex of this <see cref="Polygon2"/>
        /// at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based vertex index.</param>
        /// <returns>
        /// The vertex at the specified <paramref name="index"/>.
        /// </returns>
        public Vector2 this[int index]
        {
            get 
            {
                return this.vertices[index]; 
            }

            set 
            {
                this.vertices[index] = value; 
            }
        }

        #region Area

        /// <summary>
        /// Gets the (signed) area of the <see cref="Polygon2"/>.
        /// </summary>
        /// <value>The (signed) area of the <see cref="Polygon2"/>.</value>
        public float SignedArea
        {
            get
            {
                float area        = 0.0f;
                int   vertexCount = this.vertices.Count;

                for( int i = 0; i < vertexCount; ++i )
                {
                    int j = (i + 1) % vertexCount;

                    Vector2 vertexI = vertices[i];
                    Vector2 vertexJ = vertices[j];

                    area += vertexI.X * vertexJ.Y;
                    area -= vertexI.Y * vertexJ.X;
                }

                return area * 0.5f;
            }
        }

        /// <summary>
        /// Gets the area of the <see cref="Polygon2"/>.
        /// </summary>
        /// <value>The (unsigned) area of the <see cref="Polygon2"/>.</value>
        public float Area
        {
            get
            {
                float area = this.SignedArea;
                return area < 0.0f ? -area : area;
            }
        }

        #endregion

        #region Centroid

        /// <summary>
        /// Gets the centroid of this <see cref="Polygon2"/>.
        /// </summary>
        /// <remarks>
        /// This operation may be slow. Caching this value is preffered.
        /// </remarks>
        /// <value>
        /// The centroid or barycenter of an object X in n-dimensional space
        /// is the intersection of all hyperplanes that divide X into two parts of equal moment about the hyperplane.
        /// Informally, it is the "average" of all points of X.
        /// </value>
        public Vector2 Centroid
        {
            get
            {
                float signedArea = this.SignedArea;
                float area       = signedArea < 0.0f ? -signedArea : signedArea;

                TurnDirection vertexOrder =
                    (signedArea < 0.0f) ? TurnDirection.AntiClockwise :
                    (signedArea > 0.0f) ? TurnDirection.Clockwise :
                                          TurnDirection.None;

                if( vertexOrder == TurnDirection.None )
                    throw new InvalidOperationException( MathErrorStrings.CantReceiveValueVertexOrderIsNone );

                // The following algorithm only works with polygons
                // that have an AntiClockwise vertex order.
                if( vertexOrder == TurnDirection.Clockwise )
                {
                    this.VertexOrder = TurnDirection.AntiClockwise;
                }

                Vector2 centroid = new Vector2();
                int vertexCount  = this.vertices.Count;                

                for( int i = 0; i < vertexCount; ++i )
                {
                    int j = (i + 1) % vertexCount;

                    Vector2 vertexI = vertices[i];
                    Vector2 vertexJ = vertices[j];

                    float factor = -((vertexI.X * vertexJ.Y) - (vertexJ.X * vertexI.Y));

                    centroid.X += (vertexI.X + vertexJ.X) * factor;
                    centroid.Y += (vertexI.Y + vertexJ.Y) * factor;
                }

                float areaFactor = 1.0f / ( area * 6.0f );
                centroid.X *= areaFactor;
                centroid.Y *= areaFactor;

                // Reset the vertex order
                if( vertexOrder == TurnDirection.Clockwise )
                {
                    this.VertexOrder = TurnDirection.Clockwise;
                }

                return centroid;
            }
        }

        #endregion

        #region VertexOrder

        /// <summary>
        /// Gets or sets the Vertex Order of the <see cref="Polygon2"/>.
        /// </summary>
        /// <value>The direction the vertices turn; calculated using the <see cref="SignedArea"/>.</value>
        public TurnDirection VertexOrder
        {
            get
            {
                float area = this.SignedArea;

                if( area < 0.0f )
                    return TurnDirection.AntiClockwise;

                if( area > 0.0f )
                    return TurnDirection.Clockwise;

                return TurnDirection.None;
            }

            set
            {
                float area = this.SignedArea;

                switch( value )
                {
                    case TurnDirection.Clockwise:
                        if( area < 0.0f )
                            this.vertices.Reverse();

                        break;

                    case TurnDirection.AntiClockwise:
                        if( area > 0.0f )
                            this.vertices.Reverse();

                        break;

                    case TurnDirection.None:
                        throw new NotSupportedException( MathErrorStrings.CantSetVertexOrderToNone );

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        #endregion

        #region MomentOfInertia

        /// <summary>
        /// Gets the Moment of Inertia of this <see cref="Polygon2"/>.
        /// </summary> 
        /// <remarks>
        /// This operation is slow. Caching this value is preffered.
        /// </remarks>
        /// <value>
        /// Moment of inertia, also called mass moment of inertia or 
        /// the angular mass, is the rotational analog of mass.
        /// </value>
        public float MomentOfInertia
        {
            get
            {
                if( this.VertexCount == 1 )
                {
                    return 0.0f;
                }

                TurnDirection vertexOrder = this.VertexOrder;

                if( vertexOrder == TurnDirection.None )
                    throw new InvalidOperationException( MathErrorStrings.CantReceiveValueVertexOrderIsNone );
                
                // The following algorithm only works with polygons
                // that have an AntiClockwise vertex order.
                if( vertexOrder == TurnDirection.Clockwise )
                {
                    this.VertexOrder = TurnDirection.AntiClockwise;
                }

                // Translate center of polygon to the orgin.
                Vector2 centroid = this.Centroid;
                this.Translate( -centroid );

                float denom = 0.0f;
                float numer = 0.0f;
                float a, b, c, d;
                Vector2 vertexB;
                Vector2 vertexA = vertices[vertices.Count - 1];

                for( int index = 0; index < vertices.Count; ++index, vertexA = vertexB )
                {
                    vertexB = vertices[index];

                    Vector2.Dot(   ref vertexB, ref vertexB, out a );
                    Vector2.Dot(   ref vertexB, ref vertexA, out b );
                    Vector2.Dot(   ref vertexA, ref vertexA, out c );
                    Vector2.Cross( ref vertexA, ref vertexB, out d );

                    d = System.Math.Abs( d );

                    numer += d;
                    denom += (a + b + c) * d;
                }

                // Reset translation
                this.Translate( centroid );

                // Reset the vertex order
                if( vertexOrder == TurnDirection.Clockwise )
                {
                    this.VertexOrder = TurnDirection.Clockwise;
                }

                return denom / (numer * 6);
            }
        }

        #endregion

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon2"/> class.
        /// </summary>
        /// <param name="initialVertexCapacity">
        /// The initial number of vertices the Polygon can contain without reallocating memory.
        /// </param>
        public Polygon2( int initialVertexCapacity )
        {
            this.vertices = new List<Vector2>( initialVertexCapacity );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon2"/> class.
        /// </summary>
        /// <param name="vertices">
        /// The vertices of the polygon to create.
        /// </param>
        public Polygon2( IEnumerable<Vector2> vertices )
        {
            Contract.Requires<ArgumentNullException>( vertices != null );

            this.vertices = new List<Vector2>();
            this.vertices.AddRange( vertices );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon2"/> class.
        /// </summary>
        /// <param name="vertices">
        /// The vertices of the polygon to create.
        /// </param>
        public Polygon2( ICollection<Vector2> vertices )
        {
            Contract.Requires<ArgumentNullException>( vertices != null );
            // Contract.Ensures( this.VertexCount == vertices.Count );

            this.vertices = new List<Vector2>( vertices.Count );
            this.vertices.AddRange( vertices );
        }

        #endregion

        #region [ Methods ]

        #region InvertVertexOrder

        /// <summary>
        /// Inverts the <see cref="VertexOrder"/> of the <see cref="Polygon2"/>.
        /// </summary>
        public void InvertVertexOrder()
        {
            switch( this.VertexOrder )
            {
                case TurnDirection.AntiClockwise:
                    this.VertexOrder = TurnDirection.Clockwise;
                    break;

                case TurnDirection.Clockwise:
                    this.VertexOrder = TurnDirection.AntiClockwise;
                    break;

                case TurnDirection.None:
                default:
                    break;
            }
        }

        #endregion

        #region SubdivideEdges

        /// <summary>
        /// Subdives the edges of the <see cref="Polygon2"/>.
        /// </summary>
        /// <param name="maximumAllowedEdgeLength">
        /// The maximum allowed length of an edge
        /// before it gets sub-devided.
        /// </param>
        public void SubdivideEdges( float maximumAllowedEdgeLength )
        {
            Contract.Requires( maximumAllowedEdgeLength > 0.0f );

            List<Vector2> newVertices = new List<Vector2>( this.vertices.Count );

            for( int i = 0; i < this.vertices.Count; ++i )
            {
                Vector2 currentVertex = this.vertices[i];
                Vector2 nextVertex    = this.vertices[this.GetNextIndex( i )];
                
                Vector2 edge;
                edge.X = currentVertex.X - nextVertex.X;
                edge.Y = currentVertex.Y - nextVertex.Y;

                float edgeLength = edge.Length;                                
                newVertices.Add( currentVertex );

                if( edgeLength > maximumAllowedEdgeLength )
                {
                    float edgeCount = (float)System.Math.Ceiling( (double)edgeLength / (double)maximumAllowedEdgeLength );

                    for( int j = 0; j < edgeCount - 1; ++j )
                    {
                        Vector2 newVertex;

                        Vector2.Lerp( ref currentVertex, ref nextVertex, (float)(j + 1) / edgeCount, out newVertex );
                        newVertices.Add( newVertex );
                    }
                }
            }

            // Copy the elements in the existing list.
            // This is needed instead of this.vertices = newVertices;
            // because this.vertices is public accessable.
            // and as such may have also a different owner.
            this.vertices.Clear();

            this.vertices.Capacity = newVertices.Capacity;
            this.vertices.AddRange( newVertices );
        }

        #endregion

        #region Translate

        /// <summary>
        /// Translates all vertices of the <see cref="Polygon2"/>
        /// by the specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="offset">
        /// The offset to translate.
        /// </param>
        public void Translate( Vector2 offset )
        {
            for( int i = 0; i < this.vertices.Count; ++i )
            {
                this.vertices[i] = Vector2.Add( this.vertices[i], offset );
            }
        }

        #endregion

        #region ProjectToAxis

        /// <summary>
        /// Projects the <see cref="Polygon2"/> onto the
        /// specified <paramref name="axis"/>.
        /// </summary>
        /// <param name="axis">
        /// The axis to project the Polygon onto.
        /// </param>
        /// <param name="minimum">
        /// Will contain the projected minumum point of the polygon.
        /// </param>
        /// <param name="maximum">
        /// Will contain the projected maximum point of the polygon.
        /// </param>
        public void ProjectToAxis( Vector2 axis, out float minimum, out float maximum )
        {
            Contract.Requires<InvalidOperationException>( this.VertexCount > 0 );

            minimum = maximum = Vector2.Dot( axis, vertices[0] );
      
            for( int i = 0; i < this.vertices.Count; ++i )
            {
                float dot = Vector2.Dot( this.vertices[i], axis );

                if( dot < minimum )
                {
                    minimum = dot;
                }
                else
                {
                    if( dot > maximum )
                    {
                        maximum = dot;
                    }
                }
            }
        }

        #endregion

        #region - Get -

        #region GetIndex

        /// <summary>
        /// Gets the vertex index that follows the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The current vertex index.
        /// </param>
        /// <returns>
        /// The next vertex index.
        /// </returns>
        [Pure]
        public int GetNextIndex( int index )
        {
            // Contract.Ensures( Contract.Result<int>() >= 0 );
            // Contract.Ensures( Contract.Result<int>() < this.VertexCount );

            if ( index >= this.VertexCount - 1 )
            {
                return 0;
            }
            else
            {
                return index + 1;
            }
        }

        /// <summary>
        /// Gets the vertex index that is before the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The current vertex index.
        /// </param>
        /// <returns>
        /// The previous vertex index.
        /// </returns>
        [Pure]
        public int GetPreviousIndex( int index )
        {
            // Contract.Ensures( Contract.Result<int>() >= 0 );
            // Contract.Ensures( Contract.Result<int>() < this.VertexCount );

            if ( index <= 0 )
            {
                return this.VertexCount - 1;
            }
            else
            {
                return index - 1;
            }
        }

        #endregion

        #region GetVertexNormal

        /// <summary>
        /// Gets the normal of the vertex at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the vertex
        /// to get the normal of.
        /// </param>
        /// <returns>
        /// The normal of the vertex at the specified <paramref name="index"/>.
        /// </returns>
        [Pure]
        public Vector2 GetVertexNormal( int index )
        {
            Contract.Requires<ArgumentException>( index >= 0 && index < this.VertexCount );

            Vector2 currEdge = this.GetEdgeNormal( index );
            Vector2 prevEdge = this.GetEdgeNormal( this.GetPreviousIndex( index ) );

            Vector2 vertexNormal;
            vertexNormal.X = currEdge.X + prevEdge.X;
            vertexNormal.Y = currEdge.Y + prevEdge.Y;

            vertexNormal.Normalize();

            return vertexNormal;
        }

        #endregion

        #region GetEdge

        /// <summary>
        /// Gets the edge at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the edge to get.
        /// This is also the index of the starting vertex of the edge.
        /// </param>
        /// <returns>
        /// The edge at the specified <paramref name="index"/>.
        /// </returns>
        [Pure]
        public Vector2 GetEdge( int index )
        {
            Contract.Requires<ArgumentException>( index >= 0 && index < this.VertexCount );

            Vector2 next = this.vertices[this.GetNextIndex( index )];
            Vector2 curr = this.vertices[index];

            Vector2 edge;

            edge.X = next.X - curr.X;
            edge.Y = next.Y - curr.Y;

            return edge;
        }

        #endregion

        #region GetEdgeNormal

        /// <summary>
        /// Gets the the normal of the edge at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the edge to get.
        /// This is also the index of the starting vertex of the edge.
        /// </param>
        /// <returns>
        /// The normal of the edge at the specified <paramref name="index"/>.
        /// </returns>
        [Pure]
        public Vector2 GetEdgeNormal( int index )
        {
            Contract.Requires<ArgumentException>( index >= 0 && index < this.VertexCount );

            Vector2 edge = this.GetEdge( index );

            // normalize
            edge.Normalize();

            // get right-hand normal
            edge.X = -edge.Y;
            edge.Y =  edge.X;

            return edge;
        }

        #endregion

        #region GetEdgeMidPoint

        /// <summary>
        /// Gets the middle point of the edge at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the edge to get.
        /// This is also the index of the starting vertex of the edge.
        /// </param>
        /// <returns>
        /// The mid point of the edge at the specified <paramref name="index"/>.
        /// </returns>
        [Pure]
        public Vector2 GetEdgeMidPoint( int index )
        {
            Contract.Requires<ArgumentException>( index >= 0 && index < this.VertexCount );

            Vector2 next = this.vertices[this.GetNextIndex( index )];
            Vector2 curr = this.vertices[index];

            Vector2 edge;
            edge.X = (next.X - curr.X) * 0.5f;
            edge.Y = (next.Y - curr.Y) * 0.5f;

            Vector2 midPoint;
            midPoint.X = curr.X + edge.X;
            midPoint.Y = curr.Y + edge.Y;

            return midPoint;
        }

        #endregion

        #region GetShortestEdge

        /// <summary>
        /// Gets the length of the shortest edge of the <see cref="Polygon2"/>.
        /// </summary>
        /// <remarks>
        /// This method has a complexitity of O(N), where N = VertexCount.
        /// </remarks>
        /// <returns>
        /// The shortest edge of the <see cref="Polygon2"/>.
        /// </returns>
        [Pure]
        public float GetShortestEdge()
        {
            int vetexCount = this.vertices.Count;
            if( vetexCount == 0 )
                return 0.0f;

            float shortestEdge = float.MaxValue;

            for( int i = 0; i < vetexCount; ++i )
            {
                Vector2 edge = this.GetEdge( i );
                float edgeLength = edge.Length;

                if( edgeLength < shortestEdge )
                {
                    shortestEdge = edgeLength;
                }
            }

            return shortestEdge;
        }

        #endregion

        #endregion

        #region - Creation -

        /// <summary>
        /// Utility method that creates the shape of a regular axis aligned rectangle.
        /// </summary>
        /// <param name="offset">
        /// The offset that is applied to each coordinate.
        /// </param>
        /// <param name="dimensions">
        /// The dimensions of the rectangle.
        /// </param>
        /// <returns>The newly created Polygon2.</returns>
        public static Polygon2 CreateRectangle( Vector2 offset, Vector2 dimensions )
        {
            Vector2[] vertices = new Vector2[] {
                new Vector2( offset.X,                offset.Y ),
                new Vector2( offset.X + dimensions.X, offset.Y ),
                new Vector2( offset.X + dimensions.X, offset.Y + dimensions.Y ),
                new Vector2( offset.X,                offset.Y + dimensions.Y ),
            };

            return new Polygon2( vertices );
        }

        /// <summary>
        /// Utility method that creates the shape of a regular circle.
        /// </summary>
        /// <param name="offset">
        /// The offset that is applied to each coordinate.
        /// </param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="spikeCount">The number of spikes.</param>
        /// <returns>The vertices that make up the star.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="spikeCount"/> is less than or equal zero.</exception>
        public static Polygon2 CreateCircle( Vector2 offset, float radius, int spikeCount )
        {
            if( spikeCount <= 0 )
            {
                throw new ArgumentOutOfRangeException( 
                    "spikeCount",
                    spikeCount,
                    Atom.ErrorStrings.SpecifiedValueIsZeroOrNegative
                );
            }

            Vector2[] vertices = new Vector2[spikeCount];

            double angleStep = (double)Constants.TwoPi / (double)vertices.Length;
            double angle     = 0.0f;

            for( int i = 0; i < vertices.Length; ++i, angle += angleStep )
            {
                float sin = (float)System.Math.Sin( angle );
                float cos = (float)System.Math.Cos( angle );

                // Create Spike
                vertices[i] = new Vector2(
                    offset.X + (sin * radius),
                    offset.Y + (cos * radius)
                );
            }

            return new Polygon2( vertices );
        }

        /// <summary>
        /// Utility method that creates the shape of a regular star.
        /// </summary>
        /// <param name="offset">
        /// The offset that is applied to each coordinate.
        /// </param>
        /// <param name="innerRadius">The inner radius.</param>
        /// <param name="outerRadius">The outer radius; the spikes.</param>
        /// <param name="spikeCount">The number of spikes.</param>
        /// <returns>The vertices that make up the star.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="spikeCount"/> is less than or equal zero.</exception>
        public static Polygon2 CreateStar( 
            Vector2 offset, 
            float   innerRadius, 
            float   outerRadius,
            int     spikeCount )
        {
            if( spikeCount <= 0 )
            {
                throw new ArgumentOutOfRangeException(
                    "spikeCount",
                    spikeCount,
                    Atom.ErrorStrings.SpecifiedValueIsZeroOrNegative
                );
            }

            Vector2[] vertices = new Vector2[spikeCount * 2];

            double angleStep = (double)Constants.TwoPi / (double)vertices.Length;
            double angle     = 0.0f;

            for( int i = 0; i < vertices.Length; ++i, angle += angleStep )
            {
                float sin = (float)System.Math.Sin( angle );
                float cos = (float)System.Math.Cos( angle );

                if( i % 2 == 0 )
                {
                    // Create Spike
                    vertices[i] = new Vector2(
                        offset.X + (sin * outerRadius),
                        offset.Y + (cos * outerRadius)
                    );
                }
                else
                {
                    vertices[i] = new Vector2(
                       offset.X + (sin * innerRadius),
                       offset.Y + (cos * innerRadius) 
                    );
                }
            }

            return new Polygon2( vertices );
        }

        #endregion

        #region - Impls/Overrides -

        #region GetEnumerator

        /// <summary>
        /// Returns an enumerator that iterates through the
        /// vertices of the <see cref="Polygon2"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="IEnumerator{Vector2}"/> instance.
        /// </returns>
        public IEnumerator<Vector2> GetEnumerator()
        {
            return this.vertices.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the
        /// vertices of the <see cref="Polygon2"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="IEnumerator{Vector2}"/> instance.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.vertices.GetEnumerator();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a human-readable representation
        /// of the <see cref="Polygon2"/>.
        /// </summary>
        /// <returns>
        /// A string representation of the <see cref="Polygon2"/>. 
        /// </returns>
        public override string ToString()
        {
            return ToString( System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns a human-readable representation
        /// of the <see cref="Polygon2"/>.
        /// </summary>
        /// <param name="formatProvider">
        /// Provides culture sensitive formatting information.
        /// </param>
        /// <returns>
        /// A string representation of the <see cref="Polygon2"/>. 
        /// </returns>
        public string ToString( IFormatProvider formatProvider )
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder( this.vertices.Count * 5 );

            for( int i = 0; i < this.vertices.Count; ++i )
            {
                sb.AppendLine( vertices[i].ToString( formatProvider ) );
            }

            return sb.ToString();
        }

        #endregion

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The vertices that make-up the polygon.
        /// </summary>
        private readonly List<Vector2> vertices;

        #endregion
    }
}
