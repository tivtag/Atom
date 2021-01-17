// <copyright file="SoundGroup.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Fmod.SoundGroup class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Fmod
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Text;
    using Atom.Fmod.Native;
    using Atom.Math;

    /// <summary>
    /// A Sound group allows to set the properties
    /// of multiple <see cref="Sound"/>s at a time.
    /// This class can't be inherited.
    /// </summary>
    public sealed class SoundGroup
    {
        /// <summary>
        /// Gets the name of this SoundGroup.
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
        /// Gets or sets the master volume level for the Sound group.
        /// </summary>
        /// <remarks>
        /// This function does not go through and overwrite the Sound volumes. 
        /// It scales them by the Sound group's volume.
        /// That way when Sound::setVolume / Sound::getVolume is called the respective individual Sound volumes 
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
                float volume = 0.0f;

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
        /// Gets the current number of assigned Sounds to this Sound group.
        /// </summary>
        public int SoundCount
        {
            get
            {
                int SoundCount;

                RESULT result = nativeGroup.getNumSounds( out SoundCount );
                ThrowOnError( result );

                return SoundCount;
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
        /// Gets the <see cref="AudioSystem"/> that owns this SoundGroup.
        /// </summary>
        public AudioSystem AudioSystem
        {
            get
            {
                return this.audioSystem;
            }
        }

        /// <summary>
        /// Gets the <see cref="Atom.Fmod.Native.SoundGroup"/> object.
        /// </summary>
        internal Native.SoundGroup NativeGroup
        {
            get
            {
                return this.nativeGroup;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundGroup"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the new SoundGroup.
        /// </param>
        /// <param name="audioSystem">
        /// The AudioSystem that owns the new SoundGroup.
        /// </param>
        public SoundGroup( string name, AudioSystem audioSystem )
        {
            if( name == null )
                throw new ArgumentNullException( "name" );

            if( audioSystem == null )
                throw new ArgumentNullException( "audioSystem" );

            this.audioSystem = audioSystem;

            RESULT result = audioSystem.NativeSystem.createSoundGroup( name, out nativeGroup );
            ThrowOnError( result );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundGroup"/> class.
        /// </summary>
        /// <param name="nativeGroup">
        /// The native SoundGroup object.
        /// </param>
        /// <param name="audioSystem">
        /// The AudioSystem that owns the new SoundGroup.
        /// </param>
        internal SoundGroup( Native.SoundGroup nativeGroup, AudioSystem audioSystem )
        {
            this.nativeGroup = nativeGroup;
            this.audioSystem = audioSystem;
        }
        
        /// <summary>
        /// Warning: This function should in theory set the SoundGroup
        /// of all associated Sounds to the MasterSoundGroup. But it does not. Not implemented.
        /// ---------------------
        /// Frees this SoundGroup.
        /// </summary>
        /// <remarks>
        /// All Sounds assigned to this group are returned back to the master Sound group owned by the System object. 
        /// See System::getMasterSoundGroup.
        /// All child groups assigned to this group are returned back to the master Sound group owned by the System object. 
        /// See System::getMasterSoundGroup.
        /// </remarks>
        public void Release()
        {
            RESULT result = nativeGroup.release();
            ThrowOnError( result );
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
        /// The native FmodO.Native.SoundGroup object.
        /// </summary>
        private readonly Native.SoundGroup nativeGroup;

        /// <summary>
        /// The AudioSystem that owns this SoundGroup.
        /// </summary>
        private readonly AudioSystem audioSystem;
    }
}
