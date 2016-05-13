// <copyright file="GeometryFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.Physics.Farseer.GeometryFactory class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.Physics.Farseer
{
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using FarC = FarseerPhysics.Collision;
    using FarD = FarseerPhysics.Dynamics;
    using FarFac = FarseerPhysics.Factories;

    /// <summary>
    /// Provides static utility methods for creating farseer fixture objects.
    /// </summary>
    public static class FixtureFactory
    {
        public static Fixture CreateRectangle( FarD.Body body, float width, float height, float density )
        {
            return FarFac.FixtureFactory.CreateRectangle(width, height, density, Vector2.Zero, body);
        }

        public static Fixture CreateCircle( FarD.Body body, float radius, float density )
        {
            return FarFac.FixtureFactory.CreateCircle(radius, density, body);
        }

        public static Fixture CreateEllipse(FarD.Body body, float radiusX, float radiusY, int edgeCount, float density )
        {
            return FarFac.FixtureFactory.CreateEllipse( radiusX, radiusY, edgeCount, density, body );
        }
    }
}
