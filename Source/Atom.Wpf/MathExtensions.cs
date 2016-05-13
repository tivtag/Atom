// <copyright file="MathExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.MathExtensions class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf
{
    using System.Collections.Generic;

    /// <summary>
    /// Contains extensions that are provide methods to make working with both Atom.Math types and WPF easier.
    /// </summary>
    public static class MathExtensions
    {
        #region ToWpf

        /// <summary>
        /// Converts the specified enumeration of <see cref="Atom.Math.Vector2"/> instances
        /// into a <see cref="System.Windows.Media.PointCollection"/> that is supported by XNA.
        /// </summary>
        /// <param name="points"> The points to convert.</param>
        /// <returns>
        /// The converted <see cref="System.Windows.Media.PointCollection"/>.
        /// </returns>
        public static System.Windows.Media.PointCollection ToWpfPointCollection( this IEnumerable<Atom.Math.Vector2> points )
        {
            var pointCollections = new System.Windows.Media.PointCollection();

            foreach( Atom.Math.Vector2 point in points )
            {
                pointCollections.Add( new System.Windows.Point( point.X, point.Y ) );
            }

            return pointCollections;
        }

        /// <summary>
        /// Converts the specified list of <see cref="Atom.Math.Vector2"/> instances
        /// into a <see cref="System.Windows.Media.PointCollection"/> that is supported by XNA.
        /// </summary>
        /// <param name="points"> The points to convert.</param>
        /// <returns>
        /// The converted <see cref="System.Windows.Media.PointCollection"/>.
        /// </returns>
        public static System.Windows.Media.PointCollection ToWpfPointCollection( this IList<Atom.Math.Vector2> points )
        {
            var pointCollections = new System.Windows.Media.PointCollection( points.Count );

            foreach( Atom.Math.Vector2 point in points )
            {
                pointCollections.Add( new System.Windows.Point( point.X, point.Y ) );
            }

            return pointCollections;
        }

        /// <summary>
        /// Converts the <see cref="Atom.Math.Point2"/> into a <see cref="System.Windows.Point"/>.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static System.Windows.Point ToWpf( this Atom.Math.Point2 point )
        {
            return new System.Windows.Point( point.X, point.Y );
        }

        /// <summary>
        /// Converts the <see cref="Atom.Math.Vector2"/> into a <see cref="System.Windows.Point"/>.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static System.Windows.Point ToWpf( this Atom.Math.Vector2 vector )
        {
            return new System.Windows.Point( vector.X, vector.Y );
        }

        /// <summary>
        /// Converts the <see cref="Atom.Math.Polygon2"/> into a <see cref="System.Windows.Shapes.Polygon"/> objct.
        /// </summary>
        /// <param name="polygon">The polygon to convert.</param>
        /// <returns>
        /// The converted object.
        /// </returns>
        public static System.Windows.Shapes.Polygon ToWpf( this Atom.Math.Polygon2 polygon )
        {
            var wpfPolygon = new System.Windows.Shapes.Polygon() {
                Points = new System.Windows.Media.PointCollection( polygon.VertexCount )
            };

            foreach( Atom.Math.Vector2 vertex in polygon.Vertices )
            {
                wpfPolygon.Points.Add( new System.Windows.Point( vertex.X, vertex.Y ) );
            }

            return wpfPolygon;
        }

        #endregion

        #region ToAtom

        /// <summary>
        /// Converts the <see cref="System.Windows.Point"/> into a <see cref="Atom.Math.Point2"/>.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Atom.Math.Point2 ToAtomPoint( this System.Windows.Point point )
        {
            return new Atom.Math.Point2( (int)point.X, (int)point.Y );
        }

        /// <summary>
        /// Converts the <see cref="Atom.Math.Vector2"/> into a <see cref="System.Windows.Point"/>.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static Atom.Math.Vector2 ToAtomVector( this System.Windows.Point point )
        {
            return new Atom.Math.Vector2( (float)point.X, (float)point.Y );
        }

        #endregion
    }
}
