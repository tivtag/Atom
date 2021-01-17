// <copyright file="ChannelGroup.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Fmod.ChannelGroup class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Fmod
{
    using global::System;
    using global::System.Collections.Generic;
    using Atom.Fmod.Native;
    using Atom.Math;

    /// <summary>
    /// A channel group allows to set the properties
    /// of multiple <see cref="Channel"/>s at a time.
    /// This class can't be inherited.
    /// </summary>
    public sealed class ChannelGroup
    {
        /// <summary>
        /// Gets the name of this ChannelGroup.
        /// </summary>
        public string Name
        {
            get
            {
                string name;

                RESULT result = nativeGroup.getName( out name, 255 );
                ThrowOnError( result );

                return name;
            }
        }

        /// <summary>
        /// Gets or sets the master volume level for the channel group.
        /// </summary>
        /// <remarks>
        /// This function does not go through and overwrite the channel volumes. 
        /// It scales them by the channel group's volume.
        /// That way when Channel::setVolume / Channel::getVolume is called the respective individual channel volumes 
        /// will still be preserved. 
        /// </remarks>
        /// <value>
        /// A linear volume level, from 0.0 to 1.0 inclusive. 
        /// 0.0 = silent, 1.0 = full volume. Default = 1.0. 
        /// </value>
        public float Volume
        {
            get
            {
                float volume;

                RESULT result = nativeGroup.getVolume( out volume );
                ThrowOnError( result );

                return volume;
            }

            set
            {
                RESULT result = nativeGroup.setVolume( MathUtilities.Clamp( value, 0.0f, 1.0f ) );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets or sets the master pitch level of this ChannelGroup.  
        /// </summary>
        /// <remarks>
        /// This function does not go through and overwrite the channel frequencies.
        /// It scales them by the channel group's pitch.
        /// That way when Channel::setFrequency / Channel::getFrequency is called the respective individual channel frequencies 
        /// will still be preserved.
        /// </remarks>
        /// <value>
        /// The channel group pitch value, from 0.0 to 10.0 inclusive.
        /// 0.0 = silent, 1.0 = full volume. 
        /// Default = 1.0. 
        /// </value>
        public float Pitch
        {
            get
            {
                float pitch;

                RESULT result = nativeGroup.getPitch( out pitch );
                ThrowOnError( result );

                return pitch;
            }

            set
            {
                RESULT result = nativeGroup.setPitch( value );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ChannelGroup is muted.
        /// </summary>
        public bool IsMuted
        {
            get
            {
                bool isMuted;

                RESULT result = nativeGroup.getMute( out isMuted );
                ThrowOnError( result );

                return isMuted;
            }

            set
            {
                RESULT result = nativeGroup.setMute( value );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ChannelGroup is paused.
        /// </summary>
        public bool IsPaused
        {
            get
            {
                bool isPaused;

                RESULT result = nativeGroup.getPaused( out isPaused );
                ThrowOnError( result );

                return isPaused;
            }

            set
            {
                RESULT result = nativeGroup.setPaused( value );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets the current number of assigned channels to this channel group.
        /// </summary>
        public int ChannelCount
        {
            get
            {
                int channelCount;

                RESULT result = nativeGroup.getNumChannels( out channelCount );
                ThrowOnError( result );

                return channelCount;
            }
        }

        /// <summary>
        /// Gets the number of child channel groups this ChannelGroup has.
        /// </summary>
        public int GroupChildCount
        {
            get
            {
                return this.childGroups == null ? 0 : this.childGroups.Count;
            }
        }

        /// <summary>
        /// Gets or sets the pointer to optional user data.
        /// </summary>
        /// <value>The default value is InpPtr.Zero.</value>
        public IntPtr UserData
        {
            get
            {
                IntPtr ptr;
                RESULT result = nativeGroup.getUserData( out ptr );

                return ptr;
            }

            set
            {
                RESULT result = nativeGroup.setUserData( value );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets the <see cref="AudioSystem"/> that owns this ChannelGroup.
        /// </summary>
        public AudioSystem AudioSystem
        {
            get
            {
                return this.audioSystem;
            }
        }

        /// <summary>
        /// Gets the <see cref="Atom.Fmod.Native.ChannelGroup"/> object.
        /// </summary>
        internal Native.ChannelGroup NativeGroup
        {
            get
            {
                return this.nativeGroup;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelGroup"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the new ChannelGroup.
        /// </param>
        /// <param name="audioSystem">
        /// The AudioSystem that owns the new ChannelGroup.
        /// </param>
        public ChannelGroup( string name, AudioSystem audioSystem )
        {
            if( name == null )
            {
                throw new ArgumentNullException( nameof( name ) );
            }

            if( audioSystem == null )
            {
                throw new ArgumentNullException( nameof( audioSystem ) );
            }

            this.audioSystem = audioSystem;

            RESULT result = audioSystem.NativeSystem.createChannelGroup( name, out nativeGroup );
            ThrowOnError( result );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelGroup"/> class.
        /// </summary>
        /// <param name="nativeGroup">
        /// The native ChannelGroup object.
        /// </param>
        /// <param name="audioSystem">
        /// The AudioSystem that owns the new ChannelGroup.
        /// </param>
        internal ChannelGroup( Native.ChannelGroup nativeGroup, AudioSystem audioSystem )
        {
            this.nativeGroup = nativeGroup;
            this.audioSystem = audioSystem;
        }

        /// <summary>
        /// Warning: This function should in theory set the ChannelGroup
        /// of all associated Channels to the MasterChannelGroup. But it does not. Not implemented.
        /// ---------------------
        /// Frees this ChannelGroup.
        /// </summary>
        /// <remarks>
        /// All channels assigned to this group are returned back to the master channel group owned by the System object. 
        /// See System::getMasterChannelGroup.
        /// All child groups assigned to this group are returned back to the master channel group owned by the System object. 
        /// See System::getMasterChannelGroup.
        /// </remarks>
        public void Release()
        {
            RESULT result = nativeGroup.release();
            ThrowOnError( result );
        }

        /// <summary>
        /// Overrides the volume of all channels within this channel group and those of any sub channelgroups.
        /// </summary>
        /// <remarks>
        /// Panning only works on sounds created with FMOD_2D. 3D sounds are not pannable.
        /// Only sounds that are mono or stereo can be panned. Multichannel sounds (ie >2 channels) cannot be panned.
        /// <para>
        /// Mono sounds are panned from left to right using constant power panning.
        /// This means when pan = 0.0, the balance for the sound in each speaker is 71% left and 71% right, not 50% left and 50% right.
        /// This gives (audibly) smoother pans.
        /// Stereo sounds heave each left/right value faded up and down according to the specified pan position.
        /// This means when pan = 0.0, the balance for the sound in each speaker is 100% left and 100% right.
        /// When pan = -1.0, only the left channel of the stereo sound is audible, when pan = 1.0, only the right channel of the stereo sound is audible.
        /// </para>
        /// </remarks>
        /// <param name="volume">
        /// A linear volume level, from 0.0 to 1.0 inclusive. 0.0 = silent, 1.0 = full volume. Default = 1.0. 
        /// </param>
        public void SetVolume( float volume )
        {
            RESULT result = nativeGroup.setVolume( volume );
            ThrowOnError( result );
        }

        /// <summary>
        /// Sets pan position linearly of all channels within this channel group and those of any sub channelgroups.  
        /// </summary>
        /// <remarks>
        /// This is not to be used as a master volume for the group, as it will modify the volumes of the channels themselves.
        /// </remarks>
        /// <param name="pan">
        /// A pan position, from 0.0 to 1.0 inclusive. 0.0 = left, 1.0 = right. Default = 0.5. 
        /// </param>
        public void SetPan( float pan )
        {
            RESULT result = nativeGroup.setPan( pan );
            ThrowOnError( result );
        }

        /// <summary>
        /// Sets the pitch value.
        /// </summary>
        /// <param name="pan">
        /// Pitch value, 0.5 = half pitch, 2.0 = double pitch, etc default = 1.0.
        /// </param>
        public void SetPitch( float pan )
        {
            RESULT result = nativeGroup.setPitch( pan );
            ThrowOnError( result );
        }

        /// <summary>
        /// Sets whether the channel automatically ramps when setting volumes.
        /// </summary>
        /// <remarks>
        /// When changing volumes on a non-paused channel, FMOD normally adds a small ramp to avoid a pop sound.
        /// This function allows that setting to be overriden and volume changes to be applied immediately.
        /// </remarks>
        /// <param name="ramp">
        /// Whether to enable volume ramping.
        /// </param>
        public void SetVolumeRamp( bool ramp )
        {
            RESULT result = nativeGroup.setVolumeRamp( ramp );
            ThrowOnError( result );
        }

        /// <summary>
        /// Adds a channel group as a child of the current channel group.  
        /// </summary>
        /// <param name="group">
        /// The channel group to add as a child.
        /// </param>
        public void AddChildGroup( ChannelGroup group )
        {
            if( group == null )
            {
                throw new ArgumentNullException( nameof( group ) );
            }

            RESULT result = this.nativeGroup.addGroup( group.nativeGroup );
            ThrowOnError( result );

            if( this.childGroups == null )
            {
                this.childGroups = new List<ChannelGroup>();
            }

            this.childGroups.Add( group );
        }

        /// <summary>
        /// Gets the child ChannelGroup at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the child ChannelGroup to receive.
        /// </param>
        /// <returns>
        /// The requested ChannelGroup.
        /// </returns>
        public ChannelGroup GetChildGroup( int index )
        {
            if( this.childGroups == null )
            {
                throw new InvalidOperationException( Properties.Resources.Error_TheChannelGroupHasNoChildGroups );
            }

            return this.childGroups[index];
        }

        /// <summary>
        /// Helper function that throws an <see cref="AudioException"/>
        /// if the given native <see cref="RESULT"/> is not OK.
        /// </summary>
        /// <param name="result">The result to test for.</param>
        private static void ThrowOnError( Native.RESULT result )
        {
            if( result != Native.RESULT.OK )
                throw new AudioException( Native.Error.String( result ) );
        }

        /// <summary>
        /// The list that stores the child groups of this ChannelGroup.
        /// </summary>
        private List<ChannelGroup> childGroups;

        /// <summary>
        /// The native FmodO.Native.ChannelGroup object.
        /// </summary>
        private readonly Native.ChannelGroup nativeGroup;

        /// <summary>
        /// The AudioSystem that owns this ChannelGroup.
        /// </summary>
        private readonly AudioSystem audioSystem;
    }
}
