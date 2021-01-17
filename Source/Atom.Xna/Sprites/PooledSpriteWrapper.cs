// <copyright file="PooledSpriteWrapper.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.PooledSpriteWrapper class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna
{
    using Atom.Collections.Pooling;
    using Atom.Math;
    using Atom.Xna.Batches;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;
    
    /// <summary>
    /// Represents a wrapper around an existing <see cref="ISprite"/> that adds
    /// pooling support. <seealso cref="PooledObjectWrapper&lt;ISprite&gt;"/>
    /// </summary>
    public sealed class PooledSpriteWrapper : PooledObjectWrapper<ISprite>, ISprite, IUpdateable
    {
        /// <summary>
        /// Gets the name that uniquely identifies the ISprite.
        /// </summary>
        public string Name
        {
            get
            {
                return this.PooledObject.Name;               
            }
        }
        
        /// <summary>
        /// Gets the size of this ISprite (in pixels).
        /// </summary>
        public Point2 Size
        {
            get
            {
                return this.PooledObject.Size;
            }
        }

        /// <summary>
        /// Gets the width of this ISprite (in pixels).
        /// </summary>
        public int Width
        {
            get
            {
                return this.PooledObject.Width;
            }
        }

        /// <summary>
        /// Gets the height of this ISprite (in pixels).
        /// </summary>
        public int Height
        {
            get
            {
                return this.PooledObject.Height;
            }
        }

        /// <summary>
        /// Gets the <see cref="Rectangle"/> that defines the location 
        /// of this ISprite in the <see cref="Texture"/>.
        /// </summary>
        /// <value>The default value is <see cref="Rectangle.Empty"/>.</value>
        public Rectangle Source
        {
            get
            {
                return this.PooledObject.Source;
            }
        }

        /// <summary>
        /// Gets the Texture2D of this ISprite.
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return this.PooledObject.Texture;
            }
        }

        /// <summary>
        /// Gets the color this ISprite is tinted-in by default.
        /// </summary>
        /// <value>
        /// The default value is <see cref="Xna.Color.White"/>.
        /// </value>
        public Xna.Color Color
        {
            get
            {
                return this.PooledObject.Color;
            }
        }

        /// <summary>
        /// Gets the scaling factor that is applied by default for this ISprite.
        /// </summary>
        /// <value>The default value is <see cref="Vector2.One"/>.</value>
        public Vector2 Scale
        {
            get
            {
                return this.PooledObject.Scale;
            }
        }

        /// <summary>
        /// Gets the origin of rotation and scaling used by this ISprite.
        /// </summary>
        /// <value>The default value is <see cref="Vector2.Zero"/>.</value>
        public Vector2 Origin
        {
            get
            {
                return this.PooledObject.Origin;
            }
        }

        /// <summary>
        /// Gets the rotation of this ISprite in radians.
        /// </summary>
        public float Rotation
        {
            get
            {
                return this.PooledObject.Rotation;
            }
        }

        /// <summary>
        /// Initializes a new instance of the PooledSpriteWrapper class.
        /// </summary>
        /// <param name="pooledSprite">
        /// The sprite that is intended to be pooled.
        /// </param>
        public PooledSpriteWrapper( ISprite pooledSprite )
            : base( pooledSprite )
        {
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
        public void Draw( Vector2 position, ISpriteBatch batch )
        {
            this.PooledObject.Draw( position, batch );
        }

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
        public void Draw( float x, float y, ISpriteBatch batch )
        {
            this.PooledObject.Draw( x, y, batch );
        }

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
        public void Draw( Vector2 position, float depthLayer, ISpriteBatch batch )
        {
            this.PooledObject.Draw( position, depthLayer, batch );
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
        /// <param name="color">
        /// The color channel modulation to use.
        /// </param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw( Vector2 position, Xna.Color color, ISpriteBatch batch )
        {
            this.PooledObject.Draw( position, color, batch );
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
        /// <param name="color">The color channel modulation to use.</param>
        /// <param name="depthLayer">The layer to draw at.</param>
        /// <param name="batch">
        /// The <see cref="ISpriteBatch"/> to draw to.
        /// </param>
        public void Draw( Vector2 position, Xna.Color color, float depthLayer, ISpriteBatch batch )
        {
            this.PooledObject.Draw( position, color, depthLayer, batch );
        }

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
        public void Draw(
            Rectangle destination,
            Xna.Color color,
            ISpriteBatch batch )
        {
            this.PooledObject.Draw( destination, color, batch );
        }

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
        public void Draw(
            Vector2 position,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            ISpriteBatch batch )
        {
            this.PooledObject.Draw( position, rotation, origin, scale, batch );
        }

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
        public void Draw(
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            ISpriteBatch batch )
        {
            this.PooledObject.Draw( position, color, rotation, origin, scale, batch );
        }

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
        public void Draw(
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            ISpriteBatch batch )
        {
            this.PooledObject.Draw( position, color, rotation, origin, scale, effects, batch );
        }

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
        public void Draw(
            Vector2 position,
            Xna.Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth,
            ISpriteBatch batch )
        {
            this.PooledObject.Draw( position, color, rotation, origin, scale, effects, layerDepth, batch );
        }

        /// <summary>
        /// Updates the ISprite this PooledSpriteWrapper wraps around;
        /// if required.
        /// </summary>
        /// <param name="updateContext">
        /// The current IUpdateContext.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            var updateable = this.PooledObject as IUpdateable;

            if( updateable != null )
            {
                updateable.Update( updateContext );
            }
        }

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
        public ISprite CloneInstance()
        {
            return new PooledSpriteWrapper( this.PooledObject.CloneInstance() );
        }
    }
}
