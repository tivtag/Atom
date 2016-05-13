// <copyright file="ISprite.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.ISprite interface.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna
{
    using Atom.Math;
    using Atom.Xna.Batches;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents a two dimensional image that can easily be
    /// drawn on the screen.
    /// </summary>
    public interface ISprite : IAsset, ISizeable2
    {
        /// <summary>
        /// Gets the <see cref="Rectangle"/> that defines the location 
        /// of this ISprite in the <see cref="Texture"/>.
        /// </summary>
        /// <value>The default value is <see cref="Rectangle.Empty"/>.</value>
        Rectangle Source
        {
            get;
        }

        /// <summary>
        /// Gets the Texture2D of this ISprite.
        /// </summary>
        Texture2D Texture
        {
            get;
        }

        /// <summary>
        /// Gets the color this ISprite is tinted-in by default.
        /// </summary>
        /// <value>
        /// The default value is <see cref="Microsoft.Xna.Framework.Color.White"/>.
        /// </value>
        Xna.Color Color
        {
            get;
        }

        /// <summary>
        /// Gets the scaling factor that is applied by default for this ISprite.
        /// </summary>
        /// <value>The default value is <see cref="Vector2.One"/>.</value>
        Vector2 Scale
        {
            get;
        }

        /// <summary>
        /// Gets the origin of rotation and scaling used by this ISprite.
        /// </summary>
        /// <value>The default value is <see cref="Vector2.Zero"/>.</value>
        Vector2 Origin
        {
            get;
        }

        /// <summary>
        /// Gets the rotation of this ISprite in radians.
        /// </summary>
        float Rotation
        {
            get;
        }

        /// <summary>
        /// Draws this ISprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this ISprite.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        void Draw( Vector2 position, ISpriteBatch batch );

        /// <summary>
        /// Draws this ISprite at the given position
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this ISprite.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="x">
        /// The position to render the sprite at on the x-axis.
        /// </param>
        /// <param name="y">
        /// The position to render the sprite at on the y-axis.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        void Draw( float x, float y, ISpriteBatch batch );

        /// <summary>
        /// Draws this ISprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this <see cref="ISprite"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="depthLayer">
        /// The depth layer to draw the sprite at; used for sorting.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        void Draw( Vector2 position, float depthLayer, ISpriteBatch batch );

        /// <summary>
        /// Draws this ISprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this ISprite.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        void Draw( Vector2 position, Xna.Color color, ISpriteBatch batch );

        /// <summary>
        /// Draws this ISprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this ISprite.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="color">The color channel modulation to use.</param>
        /// <param name="depthLayer">The layer to draw at.</param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        void Draw( Vector2 position, Xna.Color color, float depthLayer, ISpriteBatch batch );

        /// <summary>
        /// Draws this ISprite into the given <paramref name="destination"/> rectangle.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="destination">
        /// The rectangle to draw the sprite in.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        void Draw(
            Rectangle destination,
            Xna.Color color,
            ISpriteBatch batch );

        /// <summary>
        /// Draws this ISprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using a mix of the specified and the default settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="rotation">
        /// The rotation in radians.
        /// </param>
        /// <param name="origin">
        /// The origin of rotation and scaling.
        /// </param>
        /// <param name="scale">
        /// The scaling factor to apply.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        void Draw(
            Vector2 position,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            ISpriteBatch batch );

        /// <summary>
        /// Draws this ISprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using a mix of the specified and the default settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="color">The color channel modulation to use.</param>
        /// <param name="rotation">The rotation in radians.</param>
        /// <param name="origin">The origin of rotation and scaling.</param>
        /// <param name="scale">The scaling factor to apply.</param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        void Draw(
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            ISpriteBatch batch );

        /// <summary>
        /// Draws this ISprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the specified settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use.
        /// </param>
        /// <param name="rotation">
        /// The rotation in radians.
        /// </param>
        /// <param name="origin">
        /// The origin of rotation and scaling.
        /// </param>
        /// <param name="scale">
        /// The scaling factor to apply.
        /// </param>
        /// <param name="effects">
        /// The SpriteEffect to apply.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        void Draw(
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            ISpriteBatch batch );

        /// <summary>
        /// Draws this ISprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the specified settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This call must be within the ISpriteBatch.Begin/ISpriteBatch.End
        /// block of the given <paramref name="batch"/>.
        /// </para>
        /// <para>
        /// It is NOT checked wheter the given <see cref="ISpriteBatch"/> is null.
        /// </para>
        /// </remarks>
        /// <param name="position">
        /// The position to render the sprite at.
        /// </param>
        /// <param name="color">
        /// The color channel modulation to use.
        /// </param>
        /// <param name="rotation">
        /// The rotation in radians.
        /// </param>
        /// <param name="origin">
        /// The origin of rotation and scaling.
        /// </param>
        /// <param name="scale">
        /// The scaling factor to apply.
        /// </param>
        /// <param name="effects">
        /// The SpriteEffect to apply.
        /// </param>
        /// <param name="layerDepth">
        /// The depth layer to draw the Sprite at.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        void Draw(
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth,
            ISpriteBatch batch );

        /// <summary>
        /// Returns an instance of this ISprite. 
        /// This may be a new object or the original object.
        /// </summary>
        /// <remarks>
        /// The cloned object may be the same the original object
        /// if there is no need to have independent instances.
        /// </remarks>
        /// <returns>
        /// The clone of this instance.
        /// </returns>
        ISprite CloneInstance();
    }
}
