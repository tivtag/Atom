//// <copyright file="BodyFactory.cs" company="federrot Software">
////     Copyright (c) federrot Software. All rights reserved.
//// </copyright>
//// <summary>Defines the Atom.Physics.Farseer.BodyFactory class.</summary>
//// <author>Paul Ennemoser (Tick)</author>

//namespace Atom.Physics.Farseer
//{
//    using System.Collections.Generic;
//    using Atom.Math;
//    using FarC=FarseerPhysics.Collision;
//    using FarD=FarseerPhysics.Dynamics;
//    using FarF=FarseerPhysics.Factories;

//    /// <summary>
//    /// Provides static utility methods for creating farseer Body objects.
//    /// </summary>
//    public static class BodyFactory
//    {
//        /// <summary>
//        /// Creates a new rectangular farseer Body with the given width, height and mass.
//        /// </summary>
//        /// <param name="size">The size of the rectangle.</param>
//        /// <param name="mass">The mass of the rectangle.</param>
//        /// <returns>
//        /// The newly created rectangular body.
//        /// </returns>
//        public static FarD.Body CreateRectangle( Vector2 size, float mass )
//        {
//            return FarF.BodyFactory.Instance.CreateRectangleBody( size.X, size.Y, mass );
//        }

//        /// <summary>
//        /// Creates a new rectangular farseer Body with the given width, height and mass.
//        /// </summary>
//        /// <param name="width">The width of the rectangle.</param>
//        /// <param name="height">The height of the rectangle.</param>
//        /// <param name="mass">The mass of the rectangle.</param>
//        /// <returns>
//        /// The newly created rectangular body.
//        /// </returns>
//        public static FarD.Body CreateRectangle( float width, float height, float mass )
//        {
//            return FarF.BodyFactory.Instance.CreateRectangleBody( width, height, mass );
//        }

//        /// <summary>
//        /// Creates a new circle farseer Body with the given radius and mass.
//        /// </summary>
//        /// <param name="radius">The radius of the circular body.</param>
//        /// <param name="mass">The mass of the circular body.</param>
//        /// <returns>
//        /// The newly created circular body.
//        /// </returns>
//        public static FarD.Body CreateCircle( float radius, float mass )
//        {
//            return FarF.BodyFactory.Instance.CreateCircleBody( radius, mass );
//        }

//        /// <summary>
//        /// Creates a new ellipse farseer body with the given radius and mass.
//        /// </summary>
//        /// <param name="radiusX">
//        /// The radius of the ellipse body on the x-axis.
//        /// </param>
//        /// <param name="radiusY">
//        /// The radius of the ellipse body on the y-axis.
//        /// </param>
//        /// <param name="mass">
//        /// The mass of the circular body.
//        /// </param>
//        /// <returns>
//        /// The newly created ellipse body.
//        /// </returns>
//        public static FarD.Body CreateEllipse( float radiusX, float radiusY, float mass )
//        {
//            return FarF.BodyFactory.Instance.CreateEllipseBody( radiusX, radiusY, mass );
//        }

//        /// <summary>
//        /// Creates a new ellipse farseer body with the given radius and mass.
//        /// </summary>
//        /// <param name="vertices">
//        /// The individual vertices that descripe the shape of the Polygon.
//        /// </param>
//        /// <param name="mass">
//        /// The mass of the polygon body.
//        /// </param>
//        /// <returns>
//        /// The newly created polygon body.
//        /// </returns>
//        public static FarD.Body CreatePolygon( IList<Vector2> vertices, float mass )
//        {
//            var farVertices = new FarC.Vertices();
//            farVertices.Capacity = vertices.Count;

//            for( int i = 0; i < vertices.Count; ++i )
//            {
//                var vertex = vertices[i];
//                farVertices.Add( new FarM.Vector2( vertex.X, vertex.Y ) );
//            }

//            return FarF.BodyFactory.Instance.CreatePolygonBody( farVertices, mass );
//        }
//    }
//}
