// <copyright file="Path.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Waypoints.Path class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Waypoints
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;

    /// <summary>
    /// Represents a path of connected <see cref="Waypoint"/>s.
    /// </summary>
    public class Path : IEnumerable<Waypoint>, INameable
    {
        /// <summary>
        /// Gets or sets the name that uniquely identifies this Path.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="Waypoint"/> this Path starts at.
        /// </summary>
        public Waypoint Start
        {
            get
            {
                if( this.Length > 0 )
                {
                    return this.waypoints[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Waypoint"/> this Path ends at.
        /// </summary>
        public Waypoint End
        {            
            get
            {
                if( this.Length > 0 )
                {
                    return this.waypoints[this.waypoints.Count - 1];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this Path starts and ends at the same Waypoint.
        /// </summary>
        public bool IsCircular
        {
            get
            {
                return this.Start == this.End;
            }
        }

        /// <summary>
        /// Gets the number of <see cref="Waypoint"/>s that lie on this Path.
        /// </summary>
        public int Length
        {
            get
            {
                return this.waypoints.Count;
            }
        }

        /// <summary>
        /// Gets the <see cref="Waypoint"/> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the Waypoint.
        /// </param>
        /// <returns>
        /// The requested Waypoint.
        /// </returns>
        public Waypoint this[int index]
        {
            get
            {
                return this.waypoints[index];
            }
        }

        /// <summary>
        /// Gets or sets the number of Waypoints that can be added to this Path
        /// without having to re-allocate memory.
        /// </summary>
        public int Capacity 
        {
            get
            {
                return this.waypoints.Capacity;
            }

            set
            {
                this.waypoints.Capacity = value;
            }
        }

        /// <summary>
        /// Adds a new Waypoint to the end of this Path.
        /// </summary>
        /// <param name="waypoint">
        /// The Waypoint to add to this Path.
        /// </param>
        public void Add( Waypoint waypoint )
        {
            Contract.Requires<ArgumentNullException>( waypoint != null );
            Contract.Requires<ArgumentException>( this.Length == 0 || waypoint.HasPathSegmentTo( this.End ) );

            this.waypoints.Add( waypoint );
        }

        /// <summary>
        /// Gets an enumerator that iterates over the <see cref="Waypoint"/>s that lie on this Path.
        /// </summary>
        /// <returns>
        /// A new enumerator.
        /// </returns>
        public IEnumerator<Waypoint> GetEnumerator()
        {
            return this.waypoints.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that iterates over the <see cref="Waypoint"/>s that lie on this Path.
        /// </summary>
        /// <returns>
        /// A new enumerator.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Gets the zero-based index of the specified Waypoint in this Path.
        /// </summary>
        /// <param name="waypoint">
        /// The Waypoint to locate.
        /// </param>
        /// <returns>
        /// The zero-based index of the specified Waypoint
        /// -or- -1 if this Path doesn't contain the specified Waypoint.
        /// </returns>
        public int IndexOf( Waypoint waypoint )
        {
            return this.waypoints.IndexOf( waypoint );
        }

        /// <summary>
        /// Attempts to isnert the specified Waypoint at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which the Waypoint should be inserted.
        /// </param>
        /// <param name="waypoint">
        /// The Waypoint to insert.
        /// </param>
        public void Insert( int index, Waypoint waypoint )
        {
            Contract.Requires<ArgumentNullException>( waypoint != null );
            Contract.Requires<ArgumentOutOfRangeException>( index >= 0 && index < this.Length );

            Waypoint before = null;
            Waypoint after = null;

            if( index == 0 )
            {
                before = this.waypoints[0];
            }
            else if( index == (this.Length - 1) )
            {
                before = this.waypoints[this.Length-1];
            }
            else
            {
                before = this.waypoints[index];
                after = this.waypoints[index + 1];
                ++index;

                if( waypoint == after || waypoint == before )
                    return;
            }

            var backup = this.CreateBackup();
            backup.Insert( index, waypoint );

            if( AreDirectlyConnected( backup ) )
            {
                this.waypoints.Insert( index, waypoint );
            }
            else
            {
                throw new ArgumentException( "The specified waypoint can't be inserted into this Path.", "waypoint" );
            }
        }

        /// <summary>
        /// Attempts to remove the Waypoint at the specified index.
        /// </summary>
        /// <param name="index">
        /// THe zero-based index of the Waypoint to remove.
        /// </param>
        /// <returns>
        /// true if it was removed;
        /// -or- otherwise false if removing it would result in a not directly connected Path.
        /// </returns>
        public bool RemoveAt( int index )
        {
            Contract.Requires<ArgumentOutOfRangeException>( index >= 0 && index < this.Length );

            if( index == 0 )
            {
                return this.RemoveStart();
            }
            else if( index == this.Length - 1 )
            {
                return this.RemoveEnd();
            }
            else
            {
                Waypoint before = this.waypoints[index - 1];
                Waypoint after = this.waypoints[index + 1];

                if( before.HasPathSegmentTo( after ) )
                {
                    this.waypoints.RemoveAt( index );
                    return true;
                }
                else
                {
                    foreach( var segment in before.Segments )
                    {
                        Waypoint other = segment.From == before ? segment.To : segment.From;

                        if( other.HasPathSegmentTo( after ) )
                        {
                            this.waypoints[index] = other;
                            return true;
                        }
                    }

                    return false;
                }
            }            
        }

        /// <summary>
        /// Removes all Waypoints after the specified zero-based index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index to start removing at.
        /// </param>
        public void RemoveAllAfter( int index )
        {
            Contract.Requires<ArgumentOutOfRangeException>( index >= 0 && index < this.Length );

            this.waypoints.RemoveRange( index, this.waypoints.Count - index );
        }

        /// <summary>
        /// Removes all Waypoints from this Path.
        /// </summary>
        public void Clear()
        {
            this.waypoints.Clear();
        }

        /// <summary>
        /// Creates a backup of the Waypoint list this Path contains.
        /// </summary>
        /// <returns>
        /// The newly created Waypoint list that mirrors
        /// the waypoints of this Path.
        /// </returns>
        private IList<Waypoint> CreateBackup()
        {
            return new List<Waypoint>( this.waypoints );
        }

        /// <summary>
        /// Gets a value indicating whether the specified Waypoint are directly
        /// connected to their neighbours.
        /// </summary>
        /// <param name="waypoints">
        /// The Waypoints to check.
        /// </param>
        /// <returns>
        /// True if the specified Waypoints are directly connected;
        /// otherwise false.
        /// </returns>
        [Pure]
        public static bool AreDirectlyConnected( IEnumerable<Waypoint> waypoints )
        {
            Contract.Requires<ArgumentNullException>( waypoints != null );

            Waypoint previous = null;

            foreach( var waypoint in waypoints )
            {
                if( previous != null )
                {
                    if( !previous.HasPathSegmentTo( waypoint ) )
                    {
                        return false;
                    }
                }

                previous = waypoint;
            }

            return true;
        }

        /// <summary>
        /// Removes the Waypoint at the start of this Path.
        /// </summary>
        /// <returns>
        /// true if it was removed;
        /// otherwise false.
        /// </returns>
        public bool RemoveStart()
        {
            if( this.Length == 0 )
                return false;

            this.waypoints.RemoveAt( 0 );
            return true;
        }

        /// <summary>
        /// Removes the Waypoint at the end of this Path.
        /// </summary>
        /// <returns>
        /// true if it was removed;
        /// otherwise false.
        /// </returns>
        public bool RemoveEnd()
        {
            if( this.Length == 0 )
                return false;

            this.waypoints.RemoveAt( this.Length - 1 );
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether this Path uses the specified PathSegment.
        /// </summary>
        /// <param name="segment">
        /// The segment to locate.
        /// </param>
        /// <returns>
        /// true if the specified PathSegment was found;
        /// otherwise false.
        /// </returns>
        public bool Contains( PathSegment segment )
        {
            for( int index = 0; index < this.waypoints.Count; ++index )
            {
                if( this.waypoints[index].HasPathSegment( segment ) )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Overriden to return that name that uniquely identifies this Path.
        /// </summary>
        /// <returns>
        /// The name of this Path.
        /// </returns>
        public override string ToString()
        {
            return this.Name ?? string.Empty;
        }

        /// <summary>
        /// The Waypoints this Path consists of.
        /// </summary>
        private readonly List<Waypoint> waypoints = new List<Waypoint>();
    }
}
