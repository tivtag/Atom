// <copyright file="SpriteSheet.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.SpriteSheet class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>
 
namespace Atom.Xna
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements an <see cref="ISpriteSheet"/> that
    /// represents an ordered sheet of <see cref="ISprite"/>s.
    /// </summary>
    public sealed partial class SpriteSheet : ISpriteSheet
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the name of this SpriteSheet.
        /// </summary>
        /// <value>
        /// The name that (uniquely) identifies this SpriteSheet.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="ISprite"/> at the given zero-based <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The index of the sprie to get or set.
        /// </param>
        /// <returns>
        /// The <see cref="ISprite"/> at the given zero-based <paramref name="index"/>.
        /// </returns>
        public ISprite this[int index]
        {
            get
            {
                return this.sprites[index];
            }

            set
            {
                this.sprites[index] = value;
            }
        }

        /// <summary>
        /// Gets the number of <see cref="ISprite"/>s this SpriteSheet contains.
        /// </summary>
        /// <value>The number of sprites this SpriteSheet contains.</value>
        public int Count
        {
            get 
            {
                return this.sprites.Count;
            }
        }

        /// <summary>
        /// Gets the number of <see cref="ISprite"/>s this SpriteSheet contains that implement 
        /// the <see cref="IUpdateable"/> interface.
        /// </summary>
        public int UpdatableCount
        {
            get
            {
                return this.spritesToUpdate.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this ICollection is read-only.
        /// </summary>
        bool ICollection<ISprite>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Updates this SpriteSheet.
        /// </summary>
        /// <param name="updateContext">
        /// The current <see cref="IUpdateContext"/>.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            for( int i = 0; i < this.spritesToUpdate.Count; ++i )
            {
                this.spritesToUpdate[i].Update( updateContext );
            }
        }

        /// <summary>
        /// Adds the given ISprite to the list of sprites that
        /// need to be updated each frame if it implements IUpdateable.
        /// </summary>
        /// <param name="sprite">
        /// The sprite to check.
        /// </param>
        private void AddToUpdateList( ISprite sprite )
        {
            var updateable = sprite as IUpdateable;

            if( updateable != null )
            {
                this.spritesToUpdate.Add( updateable );
            }
        }
        
        /// <summary>
        /// Removes the given ISprite from the list of sprites
        /// that need to be updated each frame.
        /// </summary>
        /// <param name="sprite">
        /// The sprite to check.
        /// </param>
        private void RemoveFromUpdateList( ISprite sprite )
        {
            var updateable = sprite as IUpdateable;

            if( updateable != null )
            {
                this.spritesToUpdate.Remove( updateable );
            }
        }

        #region > IList <

        /// <summary>
        /// Determines the index of a specific item in this SpriteSheet.
        /// </summary>
        /// <param name="item">
        /// The object to locate in this SpriteSheet.
        /// </param>
        /// <returns>
        /// The index of item if found in the list;
        /// otherwise, -1.
        /// </returns>
        public int IndexOf( ISprite item )
        {
            return this.sprites.IndexOf( item );
        }

        /// <summary>
        /// Inserts an item into this SpriteSheet at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which item should be inserted.
        /// </param>
        /// <param name="item">
        /// The ISprite to insert into this SpriteSheet.
        /// </param>
        public void Insert( int index, ISprite item )
        {
            this.sprites.Insert( index, item );
            this.AddToUpdateList( item );            
        }

        /// <summary>
        /// Removes the <see cref="ISprite"/> item at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        public void RemoveAt( int index )
        {
            if( index < 0 || index >= this.Count )
                throw new ArgumentOutOfRangeException( "index" );

            var sprite = this.sprites[index];
            this.sprites.RemoveAt( index );
            this.RemoveFromUpdateList( sprite );
        }

        #endregion

        #region > ICollection <

        /// <summary>
        /// Adds the specified ISprite to the end of this SpriteSheet.
        /// </summary>
        /// <param name="item">
        /// The sprite to insert.
        /// </param>
        public void Add( ISprite item )
        {
            this.sprites.Add( item );
            this.AddToUpdateList( item );
        }

        /// <summary>
        /// Removes all ISprites from this SpriteSheet.
        /// </summary>
        public void Clear()
        {
            this.sprites.Clear();
            this.spritesToUpdate.Clear();
        }

        /// <summary>
        /// Determines whether this SpriteSheet contains the
        /// specified ISprite.
        /// </summary>
        /// <param name="item">
        /// The ISprite to locate.
        /// </param>
        /// <returns>
        /// true if item is found in this SpriteSheet;
        /// otherwise, false.
        /// </returns>
        public bool Contains( ISprite item )
        {
            return this.sprites.Contains( item );
        }
        
        /// <summary>
        /// Copies the ISprites of this SpriteSheet to an System.Array,
        /// starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">
        /// The destination array.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in array at which copying begins.
        /// </param>
        public void CopyTo( ISprite[] array, int arrayIndex )
        {
            this.sprites.CopyTo( array, arrayIndex );
        }
        
        /// <summary>
        /// Attempts to remove the specified ISprite from this SpriteSheet.
        /// </summary>
        /// <param name="item">
        /// The sprite to remove.
        /// </param>
        /// <returns>
        /// true if item was successfully removed from this SpriteSheet;
        /// otherwise false.
        /// </returns>
        public bool Remove( ISprite item )
        {
            if( this.sprites.Remove( item ) )
            {
                this.RemoveFromUpdateList( item );
                return true;
            }

            return false;
        }

        #endregion

        #region > IEnumerable <

        /// <summary>
        /// Returns an IEnumerator{ISprite} that iterates through the ISprites
        /// in this SpriteSheet.
        /// </summary>
        /// <returns>
        /// A new IEnumerator{ISprite}
        /// </returns>
        public IEnumerator<ISprite> GetEnumerator()
        {
            return this.sprites.GetEnumerator();
        }

        /// <summary>
        /// Returns an IEnumerator that iterates through the ISprites
        /// in this SpriteSheet.
        /// </summary>
        /// <returns>
        /// A new IEnumerator.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The list of sprites in the <see cref="SpriteSheet"/>, sorted by index.
        /// </summary>
        private readonly List<ISprite> sprites = new List<ISprite>();

        /// <summary>
        /// The <see cref="ISprite"/> that are required to be updated each frame.
        /// </summary>
        private readonly List<IUpdateable> spritesToUpdate = new List<IUpdateable>();

        #endregion
    }
}
