// <copyright file="HashCodeBuilder.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.HashCodeBuilder{T} structure.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    /// <summary>
    /// Provides an mechanism that allows the creation of
    /// hashcodes that exist of multiple independent objects
    /// of the same type.
    /// </summary>
    public struct HashCodeBuilder
    {
        /// <summary>
        /// Appends the hashcode of the specified Object to this HashCodeBuilder.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to append.
        /// </typeparam>
        /// <param name="obj">
        /// The object to append. Can be null.
        /// </param>
        public void Append<T>( T obj )
        {            
            this.AppendHashCode( (obj == null ? 0 : obj.GetHashCode()) );
        }

        /// <summary>
        /// Appends the hashcode of the specified structure to this HashCodeBuilder.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the structure to append.
        /// </typeparam>
        /// <param name="obj">
        /// The structure to append.
        /// </param>
        public void AppendStruct<T>( T obj )
            where T : struct
        {
            this.AppendHashCode( obj.GetHashCode() );
        }

        /// <summary>
        /// Appends the specified hashcode to this HashCodeBuilder.
        /// </summary>
        /// <param name="hashCode">
        /// The hashcode to append.
        /// </param>
        private void AppendHashCode( int hashCode )
        {     
            unchecked 
            {
                this.hash = 23 + (this.hash * 37) + hashCode;
            }
        }

        /// <summary>
        /// Gets the hashcode that has been calculated by this HashCodeBuilder{T}.
        /// </summary>
        /// <returns>
        /// The current hashcode.
        /// </returns>
        public override int GetHashCode()
        {
            return this.hash;
        }

        /// <summary>
        /// Stores the hashcode that has been build up
        /// so far using this HashCodeBuilder{T}. 
        /// </summary>
        private int hash;
    }
}
