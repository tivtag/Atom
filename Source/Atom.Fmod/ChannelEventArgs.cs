// <copyright file="ChannelEventArgs.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Fmod.ChannelEventArgs class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Fmod
{
    using System;

    /// <summary>
    /// Defines specialized <see cref="EventArgs"/> for <see cref="Atom.Fmod.Channel"/> related events.
    /// This class can't be inherited.
    /// </summary>
    public sealed class ChannelEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="Atom.Fmod.Channel"/> object that is related to the event.
        /// </summary>
        public Channel Channel
        {
            get
            {
                return this.channel;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelEventArgs"/> clas.
        /// </summary>
        /// <param name="channel">
        /// The <see cref="Atom.Fmod.Channel"/> object the new ChannelEventArgs relates to.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="channel"/> is null.
        /// </exception>
        public ChannelEventArgs( Channel channel )
        {
            if( channel == null )
                throw new ArgumentNullException( "channel" );
            
            this.channel = channel;
        }

        /// <summary>
        /// The <see cref="Atom.Fmod.Channel"/> object this ChannelEventArgs relates to.
        /// </summary>
        private readonly Channel channel;
    }
}
