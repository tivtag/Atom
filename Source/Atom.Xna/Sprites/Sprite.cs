// <copyright file="Sprite.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Sprite class.
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
    public sealed partial class Sprite : ISprite, ISpriteAsset
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the name that uniquely identifies this Sprite.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the size of this Sprite (in pixels).
        /// </summary>
        public Point2 Size
        {
            get
            {
                return new Point2( this.source.Width, this.source.Height );   
            }
        }

        /// <summary>
        /// Gets the width of this Sprite (in pixels).
        /// </summary>
        public int Width
        {
            get
            {
                return this.source.Width;
            }
        }

        /// <summary>
        /// Gets the height of this Sprite (in pixels).
        /// </summary>
        public int Height
        {
            get
            {
                return this.source.Height;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Rectangle"/> that defines the location 
        /// of this Sprite in the <see cref="Texture"/>.
        /// </summary>
        /// <value>The default value is <see cref="Rectangle.Empty"/>.</value>
        public Rectangle Source
        {
            get
            {
                return this.source.ToAtom();
            }

            set
            {
                this.source  = value.ToXna();
            }
        }

        /// <summary>
        /// Gets or sets the Texture2D of this Sprite.
        /// </summary>
        public Texture2D Texture
        {
            get 
            {
                return this.texture;
            }

            set
            {
                this.texture = value;
            }
        }

        /// <summary>
        /// Gets or sets the color this Sprite is tinted-in by default.
        /// </summary>
        /// <value>
        /// The default value is <see cref="Microsoft.Xna.Framework.Color.White"/>.
        /// </value>
        public Xna.Color Color
        {
            get 
            {
                return this.defaultColor;
            }

            set
            {
                this.defaultColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the scaling factor that is applied by default for this Sprite.
        /// </summary>
        /// <value>The default value is <see cref="Vector2.One"/>.</value>
        public Vector2 Scale
        {
            get
            {
                return this.defaultScale.ToAtom();
            }

            set
            {
                this.defaultScale = value.ToXna();
            }
        }

        /// <summary>
        /// Gets or sets the origin of rotation and scaling used by this Sprite.
        /// </summary>
        /// <value>The default value is <see cref="Vector2.Zero"/>.</value>
        public Vector2 Origin
        {
            get
            {
                return this.defaultOrigin.ToAtom();
            }

            set
            {
                this.defaultOrigin = value.ToXna();
            }
        }

        /// <summary>
        /// Gets or sets the rotation of this Sprite in radians.
        /// </summary>
        public float Rotation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="SpriteEffects"/> that are applied to this Sprite.
        /// </summary>
        /// <value>The default value is <see cref="SpriteEffects.None"/>.</value>
        public SpriteEffects Effects
        {
            get;
            set;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Draws this Sprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this Sprite.
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
            batch.Draw(
                this.texture,
                position,
                this.source,
                this.defaultColor,
                this.Rotation,
                this.defaultOrigin,
                this.defaultScale,
                this.Effects,
                0.0f
            );
        }

        /// <summary>
        /// Draws this Sprite at the given position
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this Sprite.
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
            batch.Draw(
                this.texture,
                new Vector2( x, y ),
                this.source,
                this.defaultColor,
                this.Rotation,
                this.defaultOrigin,
                this.defaultScale,
                this.Effects,
                0.0f
            );
        }

        /// <summary>
        /// Draws this Sprite at the given <paramref name="position"/>
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
            batch.Draw(
                this.texture,
                position,
                this.source,
                this.defaultColor,
                this.Rotation,
                this.defaultOrigin,
                this.defaultScale,
                this.Effects,
                depthLayer
            );
        }

        /// <summary>
        /// Draws this Sprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this Sprite.
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
            batch.Draw(
                this.texture,
                position,
                this.source,
                color,
                this.Rotation,
                this.defaultOrigin,
                this.defaultScale,
                this.Effects,
                0.0f
            );
        }

        /// <summary>
        /// Draws this Sprite at the given <paramref name="position"/>
        /// to the given <see cref="ISpriteBatch"/> using the default settings of this Sprite.
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
            batch.Draw(
                this.texture,
                position,
                this.source,
                color,
                this.Rotation,
                this.defaultOrigin,
                this.defaultScale,
                this.Effects,
                depthLayer
            );
        }

        /// <summary>
        /// Draws this Sprite into the given <paramref name="destination"/> rectangle.
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
            batch.Draw(
                this.texture,
                destination,
                this.source,
                color,
                this.Rotation,
                this.defaultOrigin,
                this.Effects,
                0.0f
            );
        }

        /// <summary>
        /// Draws this Sprite at the given <paramref name="position"/>
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
            batch.Draw(
                texture,
                position,
                this.source,
                this.defaultColor,
                rotation,
                origin.ToXna(),
                scale.ToXna(),
                this.Effects,
                0.0f
            );
        }

        /// <summary>
        /// Draws this Sprite at the given <paramref name="position"/>
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
            batch.Draw(
                texture,
                position,
                this.source,
                color,
                rotation,
                origin.ToXna(),
                scale.ToXna(),
                this.Effects,
                0.0f
            );
        }

        /// <summary>
        /// Draws this Sprite at the given <paramref name="position"/>
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
            batch.Draw(
                this.texture,
                position,
                this.source,
                color,
                rotation,
                origin.ToXna(),
                scale.ToXna(),
                effects,
                0.0f
            );
        }

        /// <summary>
        /// Draws this Sprite at the given <paramref name="position"/>
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
            batch.Draw(
                this.texture,
                position,
                this.source,
                color,
                rotation,
                origin.ToXna(),
                scale.ToXna(),
                effects,
                layerDepth
            );
        }     
        
        /// <summary>
        /// Overriden to return the <see cref="Name"/> of this Sprite.
        /// </summary>
        /// <returns>The <see cref="Name"/> of this Sprite.</returns>
        public override string ToString()
        {
            return this.Name ?? string.Empty;
        }

        /// <summary>
        /// Clones an instance of this Sprite.
        /// </summary>
        /// <remarks>
        /// There is no need to create a new instance
        /// because <see cref="Sprite"/>s are their own instances.
        /// </remarks>
        /// <returns>
        /// The clone of this instance.
        /// </returns>
        ISprite ISprite.CloneInstance()
        {
            return this;
        }

        /// <summary>
        /// Creates an instance of this ISpriteAsset.
        /// </summary>
        /// <returns>
        /// The instance that has been created.
        /// </returns>
        ISprite ISpriteAsset.CreateInstance()
        {
            return this;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Represents the storage field of the <see cref="Source"/> property.
        /// </summary>
        private Xna.Rectangle source;

        /// <summary>
        /// Represents the storage field of the <see cref="Texture"/> property.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Represents the storage field of the <see cref="Xna.Color"/> property.
        /// </summary>
        private Xna.Color defaultColor = Xna.Color.White;

        /// <summary>
        /// Represents the storage field of the <see cref="Scale"/> property.
        /// </summary>
        private Xna.Vector2 defaultScale = new Xna.Vector2( 1.0f, 1.0f );
        
        /// <summary>
        /// Represents the storage field of the <see cref="Origin"/> property.
        /// </summary>
        private Xna.Vector2 defaultOrigin;

        #endregion
    }
}