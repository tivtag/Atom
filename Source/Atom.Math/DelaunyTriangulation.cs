// <copyright file="DelaunyTriangulation.cs" company="None">
//      Credit to Paul Bourke (pbourke@swin.edu.au) for the original Fortran 77 Program :))
//      Converted to a standalone C# 2.0 library by Morten Nielsen (www.iter.dk)
//      Check out: http://astronomy.swin.edu.au/~pbourke/terrain/triangulate/
// </copyright>
// <summary>
//      Defines the DelaunyTriangulation class.
// </summary>
// <author>
//      Paul Bourke (pbourke@swin.edu.au), 
//      Morten Nielsen (www.iter.dk), 
//      Paul Ennemoser (Tick)
// </author>

namespace Atom.Math
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Performs the Delauney triangulation on a set of vertices.
    /// Only returns expected results with non-complex, convex polygons.
    /// </summary>
    /// <remarks>
    /// Based on Paul Bourke's "An Algorithm for Interpolating Irregularly-Spaced Data
    /// with Applications in Terrain Modelling" (http://astronomy.swin.edu.au/~pbourke/modelling/triangulate/).
    /// </remarks>
    public static class DelaunyTriangulation
    {
        /// <summary>
        /// Performs Delauney triangulation on a set of points.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The triangulation doesn't support multiple points with the same planar location.
        /// Vertex-lists with duplicate points may result in strange triangulation with intersecting edges.
        /// To avoid adding multiple points to your vertex-list you can use the following anonymous predicate method:
        /// <code>
        /// if(!vertices.Exists(delegate(Vector2 p) { return pNew == p; }))
        ///     vertices.Add(pNew);
        /// </code>
        /// </para>
        /// <para>The triangulation algorithm may be described in pseudo-code as follows:
        /// <code>
        /// subroutine Triangulate
        /// input : vertex list
        /// output : triangle list
        ///    initialize the triangle list
        ///    determine the supertriangle
        ///    add supertriangle vertices to the end of the vertex list
        ///    add the supertriangle to the triangle list
        ///    for each sample point in the vertex list
        ///       initialize the edge buffer
        ///       for each triangle currently in the triangle list
        ///          calculate the triangle circumcircle center and radius
        ///          if the point lies in the triangle circumcircle then
        ///             add the three triangle edges to the edge buffer
        ///             remove the triangle from the triangle list
        ///          endif
        ///       endfor
        ///       delete all doubly specified edges from the edge buffer
        ///          this leaves the edges of the enclosing polygon only
        ///       add to the triangle list all triangles formed between the point
        ///          and the edges of the enclosing polygon
        ///    endfor
        ///    remove any triangles from the triangle list that use the supertriangle vertices
        ///    remove the supertriangle vertices from the vertex list
        /// end
        /// </code>
        /// </para>
        /// </remarks>
        /// <param name="vertices">The list of vertices to triangulate.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="vertices"/> is null.</exception>
        /// <exception cref="ArgumentException">If <paramref name="vertices"/> contains less than 3 vertices.</exception>
        /// <returns>Triangles referencing vertex indices arranged in clockwise order.</returns>
        public static IList<IndexedTriangle> Triangulate( IList<Vector2> vertices )
        {
            if( vertices == null )
                throw new ArgumentNullException( "vertices" );

            int vertexCount = vertices.Count;
            if( vertexCount < 3 )
                throw new ArgumentException( MathErrorStrings.TriangulationRequiresAtLeastThreeVertices );

            int triMax = 4 * vertexCount;

            // Find the maximum and minimum vertex bounds.
            // This is to allow calculation of the bounding supertriangle.
            float minX = vertices[0].X;
            float minY = vertices[0].Y;
            float maxX = minX;
            float maxY = minY;

            for( int i = 1; i < vertexCount; ++i )
            {
                if( vertices[i].X < minX ) minX = vertices[i].X;
                if( vertices[i].X > maxX ) maxX = vertices[i].X;
                if( vertices[i].Y < minY ) minY = vertices[i].Y;
                if( vertices[i].Y > maxY ) maxY = vertices[i].Y;
            }

            float deltaX   = maxX - minX;
            float deltaY   = maxY - minY;
            float deltaMax = (deltaX > deltaY) ? deltaX : deltaY;

            float midX = (maxX + minX) * 0.5f;
            float midY = (maxY + minY) * 0.5f;

            // Set up the supertriangle
            // This is a triangle which encompasses all the sample points.
            // The supertriangle coordinates are added to the end of the
            // vertex list. The supertriangle is the first triangle in
            // the triangle list.
            vertices.Add( new Vector2( (midX - 2 * deltaMax), (midY - deltaMax) ) );
            vertices.Add( new Vector2( midX, (midY + 2 * deltaMax) ) );
            vertices.Add( new Vector2( (midX + 2 * deltaMax), (midY - deltaMax) ) );

            List<IndexedTriangle> triangles = new List<IndexedTriangle>();

            // SuperTriangle placed at index 0.
            triangles.Add( new IndexedTriangle( vertexCount, vertexCount + 1, vertexCount + 2 ) );

            // Include each point one at a time into the existing mesh.
            for( int i = 0; i < vertexCount; ++i )
            {
                List<IndexedEdge> edges = new List<IndexedEdge>();

                // Set up the edge buffer.
                // If the point (Vertex(i).x,Vertex(i).y) lies inside the circumcircle then the
                // three edges of that triangle are added to the edge buffer and the triangle is removed from list.
                for( int j = 0; j < triangles.Count; ++j )
                {
                    IndexedTriangle tri = triangles[j];

                    if( Circle.IsInside( vertices[i], vertices[tri.IndexA], vertices[tri.IndexB], vertices[tri.IndexC] ) )
                    {
                        edges.Add( new IndexedEdge( tri.IndexA, tri.IndexB ) );
                        edges.Add( new IndexedEdge( tri.IndexB, tri.IndexC ) );
                        edges.Add( new IndexedEdge( tri.IndexC, tri.IndexA ) );

                        triangles.RemoveAt( j );
                        --j;
                    }
                }

                // In case we the last duplicate point we removed was the last in the array.
                if( i >= vertexCount )
                    continue;

                // Remove duplicate edges
                // Note: if all triangles are specified anticlockwise then all
                // interior edges are opposite pointing in direction.
                for( int j = edges.Count - 2; j >= 0; --j )
                {
                    for( int k = edges.Count - 1; k >= j + 1; --k )
                    {
                        if( edges[j].Equals( edges[k] ) )
                        {
                            edges.RemoveAt( k );
                            edges.RemoveAt( j );
                            --k;
                            continue;
                        }
                    }
                }

                // Form new triangles for the current point
                // Skipping over any tagged edges.
                // All edges are arranged in clockwise order.
                for( int j = 0; j < edges.Count; ++j )
                {
                    if( triangles.Count >= triMax )
                        throw new OverflowException( "Exceeded maximum edges." );

                    triangles.Add( new IndexedTriangle( edges[j].IndexA, edges[j].IndexB, i ) );
                }

                edges.Clear();
                edges = null;
            }

            // Remove triangles with supertriangle vertices
            // These are triangles which have a vertex number greater than nv.
            for( int i = triangles.Count - 1; i >= 0; --i )
            {
                if( triangles[i].IndexA >= vertexCount || 
                    triangles[i].IndexB >= vertexCount ||
                    triangles[i].IndexC >= vertexCount )
                {
                    triangles.RemoveAt( i );
                }
            }

            // Remove SuperTriangle vertices
            vertices.RemoveAt( vertices.Count - 1 );
            vertices.RemoveAt( vertices.Count - 1 );
            vertices.RemoveAt( vertices.Count - 1 );

            triangles.TrimExcess();
            return triangles;
        }
    }
}