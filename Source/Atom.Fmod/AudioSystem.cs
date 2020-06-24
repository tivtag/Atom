// <copyright file="AudioSystem.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Fmod.AudioSystem class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Fmod
{
    // use #pragma warning disable 1591
    // in the native wrappers.

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Globalization;
    using global::System.Text;
    using Atom.Diagnostics;
    using Atom.Fmod.Native;

    /// <summary>
    /// Represents the core object of the Fmod audio-libary.
    /// </summary>
    public class AudioSystem
    {
        #region [ Properties ]

        /// <summary>
        /// Gets a value indicating whether this AudioSystem has been initialized.
        /// </summary>
        public bool IsInitialized
        {
            get { return isInitialized; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Atom.Diagnostics.ILog"/> this AudioSystem uses
        /// to log error messages.
        /// </summary>
        /// <value>The default value is null.</value>
        public Atom.Diagnostics.ILog ErrorLog
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the master <see cref="ChannelGroup"/>; which is the channel group
        /// all new <see cref="Channel"/>s play under.
        /// </summary>
        public ChannelGroup MasterChannelGroup
        {
            get
            {
                return this.masterChannelGroup;
            }
        }

        /// <summary>
        /// Gets the master <see cref="SoundGroup"/>; which is the sound group
        /// all new <see cref="Sound"/>s play under.
        /// </summary>
        public SoundGroup MasterGroup
        {
            get
            {
                return this.masterGroup;
            }
        }

        /// <summary>
        /// Gets the <see cref="SoundGroup"/>; which is the channel group
        /// all new music <see cref="Sound"/>s play under by default.
        /// </summary>
        public SoundGroup MusicGroup
        {
            get
            {
                return this.musicGroup;
            }
        }

        /// <summary>
        /// Gets the <see cref="SoundGroup"/>; which is the channel group
        /// all new sample <see cref="Sound"/>s play under by default.
        /// </summary>
        public SoundGroup SampleGroup
        {
            get
            {
                return this.sampleGroup;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether exceptions 
        /// should be thrown when a <see cref="Sound"/> resource couldn't be found.
        /// </summary>
        /// <remarks>
        /// If this value is true exceptions are thrown,
        /// If this value is false the corresponding methods return null.
        /// </remarks>
        /// <value>The default value is true.</value>
        public bool ThrowExceptionOnResourceNotFound
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the version of the the loaden native FMODex dll.
        /// </summary>
        /// <exception cref="AudioException">
        /// If an internal FMODex error has occured.
        /// </exception>
        public string NativeVersion
        {
            get
            {
                uint version;
                RESULT result = system.getVersion( out version );
                ThrowOnError( result );

                return version.ToString( "X", CultureInfo.CurrentCulture );
            }
        }

        /// <summary>
        /// Gets the name of the Audio Driver that is in use.
        /// </summary>
        /// <exception cref="AudioException">
        /// If an internal FMODex error has occured.
        /// </exception>
        public string AudioDriverName
        {
            get
            {
                string name;
                global::System.Guid driverGuid;

                int driver;
                RESULT result = system.getDriver( out driver );
                ThrowOnError( result );

                result = system.getDriverInfo( 
                    driver, 
                    out name,
                    512, 
                    out driverGuid, 
                    out int systemrate, 
                    out SPEAKERMODE speakerMode,
                    out int speakermodechannels
                );
                ThrowOnError( result );

                return name.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the current <see cref="SpeakerMode"/>. 
        /// </summary>
        /// <exception cref="AudioException"> 
        /// If a native FMOD error has occured. 
        /// </exception>
        public SPEAKERMODE SpeakerMode
        {
            get
            {
                string name;
                global::System.Guid driverGuid;

                int driver;
                RESULT result = system.getDriver(out driver);
                ThrowOnError(result);

                result = system.getDriverInfo(
                    driver,
                    out name,
                    512,
                    out driverGuid,
                    out int systemrate,
                    out SPEAKERMODE speakerMode,
                    out int speakermodechannels
                );
                ThrowOnError(result);

                return speakerMode;
            }

            ////set
            ////{
            ////    RESULT result = system.setSpeakerMode( value );
            ////    ThrowOnError( result );
            ////}
        }

        /// <summary>
        /// Gets or sets the default path to Music resources. The default value is ' @"Content/Music";'.
        /// </summary>
        public string MusicDirectory
        {
            get
            {
                return this.defaultMusicDirectory;
            }

            set
            {
                this.defaultMusicDirectory = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the default path to Sample resources. The default value is ' @"Content/Samples";'.
        /// </summary>
        public string SampleDirectory
        {
            get
            {
                return this.defaultSampleDirectory;
            }

            set
            {
                this.defaultSampleDirectory = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the default <see cref="SoundManager"/> of the <see cref="AudioSystem"/>.
        /// </summary>
        public SoundManager DefaultSoundManager
        {
            get { return defaultSoundManager; }
        }

        /// <summary>
        /// Gets how many <see cref="SoundManager"/>s the AudioSystem has attached to.
        /// </summary>
        public int SoundManagerCount
        {
            get { return soundManagers.Count; }
        }

        /// <summary>
        /// Gets the underlying <see cref="Atom.Fmod.Native.System"/> object.
        /// </summary>
        internal Atom.Fmod.Native.System NativeSystem
        {
            get { return system; }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSystem"/> class.
        /// </summary>
        public AudioSystem()
        {
            this.ThrowExceptionOnResourceNotFound = true;
            this.defaultSoundManager = CreateNewSoundManager();
        }

        /// <summary> 
        /// Initializes the <see cref="AudioSystem"/> and the underlying FMOD system. 
        /// </summary>
        /// <param name="maxChannels">
        /// The maximum number of channels to use.
        /// </param>
        /// <exception cref="AudioException"> If an initialization error has occured. </exception> 
        /// <exception cref="InvalidOperationException">
        /// If the <see cref="AudioSystem"/> is already in an initialized state.
        /// </exception>
        public void Initialize( int maxChannels )
        {
            this.Initialize( INITFLAGS.NORMAL, maxChannels );
        }

        /// <summary> 
        /// Initializes the <see cref="AudioSystem"/> and the underlying FMOD system. 
        /// </summary>
        /// <param name="flags">
        /// The initialization flags.
        /// </param>
        /// <param name="maxChannels">
        /// The maximum number of channels to use.
        /// </param>
        /// <param name="speakerMode">
        /// If null: System Mode is used.
        /// If not null: Speaker Mode is used.
        /// </param>
        /// <exception cref="AudioException">
        /// If an initialization error has occured.
        /// </exception> 
        /// <exception cref="InvalidOperationException">
        /// If the <see cref="AudioSystem"/> is already in an initialized state.
        /// </exception>
        public void Initialize( INITFLAGS flags, int maxChannels, SPEAKERMODE? speakerMode = null )
        {
            if( this.isInitialized )
            {
                throw new InvalidOperationException(
                    Properties.Resources.Error_TheAudioSystemIsAlreadyInitialized
                );
            }

            uint version = 0;
            RESULT result;

            // 1.) Create FmodSystem
            result = Factory.System_Create( out system );
            ThrowOnError( result );

            // 2.) Check whether the version of the native dll is supported:
            result = system.getVersion(out version );
            ThrowOnError( result );

            if( version < VERSION.number )
            {
                OnInvalidDllVersion( version, VERSION.number );
            }

            int driverCount;
            result = system.getNumDrivers( out driverCount );
            ThrowOnError( result );

            if( driverCount == 0 )
            {
                this.LogError( "No sound driver could be detected. Disabling sound." );

                result = system.setOutput( OUTPUTTYPE.NOSOUND );
                ThrowOnError( result );
            }
            ////else
            ////{
            ////    SPEAKERMODE systemSpeakerMode = SPEAKERMODE.STEREO;
            ////    CAPS caps = CAPS.NONE;
            ////    int controlpaneloutputrate = 0;

            ////    system.getDriverCaps( 0, ref caps, ref controlpaneloutputrate, ref systemSpeakerMode );

            ////    //result = system.getDriverCaps( 0, ref caps, ref controlpaneloutputrate, ref systemSpeakerMode );
            ////    ThrowOnError( result );

            ////    var choosenSpeakerMode = speakerMode.HasValue ? speakerMode.Value : systemSpeakerMode;
            ////    result = system.setSpeakerMode( choosenSpeakerMode );  /* Set the user selected speaker mode. */
            ////    ThrowOnError( result );

            ////    if( (caps & CAPS.HARDWARE_EMULATED) != 0 )
            ////    {
            ////        /* The user has the 'Acceleration' slider set to off!  This is really bad for latency!. */
            ////        /* You might want to warn the user about this. */
            ////        /* At 48khz, the latency between issuing an fmod command and hearing it will now be about 213ms. */
            ////        result = system.setDSPBufferSize( 1024, 10 );
            ////        ThrowOnError( result );
            ////    }

            ////    var nameBuilder = new StringBuilder( 512 );
            ////    var driverGuid = new Atom.Fmod.Native.GUID();

            ////    result = system.getDriverInfo( 0, nameBuilder, 512, ref driverGuid );
            ////    ThrowOnError( result );

            ////    string driverName = nameBuilder.ToString();
            ////    if( driverName.Equals( "SigmaTel", StringComparison.OrdinalIgnoreCase ) )
            ////    {
            ////        /* Sigmatel sound devices crackle for some reason if the format is PCM 16bit. 
            ////         * PCM floating point output seems to solve it. */
            ////        result = system.setSoftwareFormat(
            ////            48000,
            ////            SOUND_FORMAT.PCMFLOAT,
            ////            0,
            ////            0,
            ////            DSP_RESAMPLER.LINEAR
            ////        );

            ////        ThrowOnError( result );
            ////    }
            ////}

            result = system.init( maxChannels, flags, IntPtr.Zero );

            if( result == RESULT.ERR_OUTPUT_CREATEBUFFER )
            {
                /////* Ok, the speaker mode selected isn't supported by this soundcard.
                //// * Switch it back to stereo... */
                ////result = system.setSpeakerMode( SPEAKERMODE.STEREO );
                ////ThrowOnError( result );

                // Try again
                result = system.init( maxChannels, INITFLAGS.NORMAL, IntPtr.Zero );
                ThrowOnError( result );
            }
            else
            {
                ThrowOnError( result );
            }

            CreateGroups();
            this.isInitialized = true;
        }

        private void CreateGroups()
        {
            Native.ChannelGroup nativeMasterChannelGroup;
            system.getMasterChannelGroup( out nativeMasterChannelGroup );

            this.masterChannelGroup = new ChannelGroup( nativeMasterChannelGroup, this );

            Native.SoundGroup nativeMasterSoundGroup;
            system.getMasterSoundGroup(out nativeMasterSoundGroup );

            this.masterGroup = new SoundGroup( nativeMasterSoundGroup, this );
            this.musicGroup = new SoundGroup( "Music", this );
            this.sampleGroup = new SoundGroup( "Sample", this );
        }

        /// <summary>
        /// Called when the expected version of the native fmod dll was invalid.
        /// </summary>
        /// <param name="version">
        /// The actual version of the dll.
        /// </param>
        /// <param name="expectedVersion">
        /// The expected version of the dll.
        /// </param>
        protected virtual void OnInvalidDllVersion( uint version, uint expectedVersion )
        {
            throw new AudioException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.Error_NativeDllVersionXNotSupportedYRequired,
                    version.ToString( "X", CultureInfo.CurrentCulture ),
                    expectedVersion.ToString( "X", CultureInfo.CurrentCulture )
                )
            );
        }

        /// <summary> 
        /// Shutdowns this <see cref="AudioSystem"/>. 
        /// </summary>
        /// <exception cref="AudioException"> 
        /// If an internal FMOD exception has occured. 
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If the <see cref="AudioSystem"/> is not in an initialized state.
        /// </exception>
        public void Shutdown()
        {
            if( !this.isInitialized )
            {
                throw new InvalidOperationException( Properties.Resources.Error_TheAudioSystemIsNotInitialized );
            }

            // Release resources.
            foreach( SoundManager manager in this.soundManagers )
            {
                manager.ReleaseAndRemoveAll();
            }

            this.soundManagers.Clear();

            // Close & release FMOD.
            RESULT result = this.system.close();
            ThrowOnError( result );

            result = this.system.release();
            ThrowOnError( result );

            this.isInitialized = false;
        }

        #endregion

        #region [ Methods ]

        /// <summary> 
        /// Updates the <see cref="AudioSystem"/>.
        /// </summary>
        /// <exception cref="AudioException"> 
        /// If a native FMOD error has occured. 
        /// </exception>
        public void Update()
        {
            RESULT result = system.update();

            if( result != RESULT.OK )
            {
                this.LogOnError( result );
            }

            ProcessEndedChannels();
        }

        /// <summary>
        /// Creates a new <see cref="SoundManager"/> object.
        /// </summary>
        /// <remarks>
        /// Most applications will only need one <see cref="SoundManager"/> at a time.
        /// </remarks>
        /// <returns>A new <see cref="SoundManager"/> instance.</returns>
        public SoundManager CreateNewSoundManager()
        {
            SoundManager manager = new SoundManager( this );
            soundManagers.Add( manager );

            return manager;
        }

        internal void AddChannelEnd( Channel channel )
        {
            endedChannels.Add( channel );
        }

        /// <summary>
        /// The "Ended"-events are triggered on the main thread and not in the native callback
        /// to avoid getting the native AudioSystem
        /// </summary>
        private void ProcessEndedChannels()
        {
            if( endedChannels.Count > 0 )
            {
                Channel[] channels = this.endedChannels.ToArray();
                this.endedChannels.Clear();

                foreach( Channel channel in channels )
                {
                    channel.OnEnded();
                }
            }
        }

        #region > Get <

        /// <summary>
        /// Tries to get the music <see cref="Sound"/> resource that
        /// has the given <paramref name="fullName"/> using the DefaultSoundManager.
        /// </summary>
        /// <param name="fullName">
        /// The full name of the resource, including the directory path.
        /// </param>
        /// <returns>
        /// The requested Sound resource; or null.
        /// </returns>
        public Sound Get( string fullName )
        {
            if( isInitialized )
            {
                return defaultSoundManager.Get( fullName );
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Tries to get the music <see cref="Sound"/> resource that
        /// has the given <paramref name="name"/> using the DefaultSoundManager.
        /// </summary>
        /// <param name="name">
        /// The name of the resource.
        /// </param>
        /// <param name="directoryPath">
        /// The path to the directory which contains the resource to get.
        /// </param>
        /// <param name="tag">
        /// The (optional) tag that is attached to the sound resource.
        /// Using different tags one can store the same sound multiple times
        /// in the same SoundManager.
        /// </param>
        /// <returns>
        /// The requested Sound resource; or null.
        /// </returns>
        public Sound Get( string name, string directoryPath, string tag = "" )
        {
            if( isInitialized )
            {
                return defaultSoundManager.Get( name, directoryPath, tag );
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Tries to get the music <see cref="Sound"/> resource that
        /// has the given <paramref name="name"/> using the DefaultSoundManager.
        /// </summary>
        /// <param name="name">
        /// The name of the resource.
        /// </param>
        /// <returns>
        /// The requested Sound resource; or null.
        /// </returns>
        public Sound GetMusic( string name )
        {
            if( isInitialized )
            {
                return defaultSoundManager.GetMusic( name );
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Tries to get the music <see cref="Sound"/> resource that
        /// has the given <paramref name="name"/> using the DefaultSoundManager.
        /// </summary>
        /// <param name="name">
        /// The name of the resource.
        /// </param>
        /// <returns>
        /// The requested Sound resource; or null.
        /// </returns>
        public Sound GetSample( string name )
        {
            if( isInitialized )
            {
                return defaultSoundManager.GetSample( name );
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Tries to get the music <see cref="Sound"/> resource that
        /// has the given <paramref name="name"/> using the DefaultSoundManager; then loads is a sample.
        /// </summary>
        /// <param name="name">
        /// The name of the resource.
        /// </param>
        /// <param name="isLooping">
        /// States whether the sample should loop.
        /// </param>
        /// <returns>
        /// The requested Sound resource; or null.
        /// </returns>
        public Sound LoadSample( string name, bool isLooping = false )
        {
            Sound sample = GetSample( name );

            if( sample == null )
            {
                LogError( "Error loading sample " + name );
                return null;
            }
            else
            {
                sample.LoadAsSample( isLooping );
                return sample;
            }
        }

        /// <summary>
        /// Gets the <see cref="SoundManager"/> at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the SoundManager to get.</param>
        /// <returns>The SoundManager instance.</returns>
        public SoundManager GetSoundManager( int index )
        {
            return soundManagers[index];
        }

        #endregion

        #region > Remove <

        /// <summary>
        /// Tries to remove the Sound Resource with the given <paramref name="soundName"/>
        /// from the DefaultSoundManager.
        /// </summary>
        /// <param name="soundName">
        /// The name of the Sound to remove.
        /// </param>
        /// <returns>
        /// True if the Sound with the given name has been removed;
        /// otherwise false.
        /// </returns>
        public bool Remove( string soundName )
        {
            return defaultSoundManager.Remove( soundName );
        }

        #endregion

        #region - 3D/2D -

        /// <summary>
        /// Sets the global doppler scale, distance factor and log rolloff scale for all 3D sound in FMOD
        /// </summary>
        /// <param name="dopplerScale">
        /// The scaling factor for doppler shift. Default = 1.0. 
        /// </param>
        /// <param name="distanceFactor">
        /// Relative distance factor to FMOD's units. Default = 1.0. (1.0 = 1 metre). 
        /// </param>
        /// <param name="rolloffScale">
        /// Scaling factor for 3D sound rolloff or attenuation for FMOD_3D_LOGROLLOFF 
        /// based sounds only (which is the default type). Default = 1.0.
        /// </param>
        /// <remarks>
        /// The doppler scale is a general scaling factor for how much the pitch varies due to doppler shifting in 3D sound.
        /// Doppler is the pitch bending effect when a sound comes towards the listener or moves away from it, much like the effect you hear when a train goes past you with its horn sounding. With dopplerscale you can exaggerate or diminish the effect.
        /// FMOD's effective speed of sound at a doppler factor of 1.0 is 340 m/s.
        /// <para>
        /// The distance factor is the FMOD 3D engine relative distance factor, compared to 1.0 meters.
        /// Another way to put it is that it equates to "how many units per meter' does your engine have". 
        /// For example. If you are using feet then scale would equal 3.28.
        /// Note! This only affects doppler! If you keep your min/max distance, custom rolloff curves and positions in scale relative to each other the volume rolloff will not change.
        /// If you set this, the mindistance of a sound will automatically set itself to this value when it is created in case the user forgets to set the mindistance to match the new distancefactor.
        /// </para>
        /// The rolloff scale sets the global attenuation rolloff factor for FMOD_3D_LOGROLLOFF based sounds only (which is the default).
        /// Normally volume for a sound will scale at mindistance / distance. This gives a logarithmic attenuation of volume as the source gets further away (or closer).
        /// Setting this value makes the sound drop off faster or slower. The higher the value, the faster volume will attenuate, and conversely the lower the value, the slower it will attenuate.
        /// For example a rolloff factor of 1 will simulate the real world, where as a value of 2 will make sounds attenuate 2 times quicker.
        /// rolloffscale has no effect for FMOD_3D_LINEARROLLOFF or FMOD_3D_CUSTOMROLLOFF.
        /// </remarks>
        public void Set3DSettings( float dopplerScale, float distanceFactor, float rolloffScale )
        {
            RESULT result = this.system.set3DSettings( dopplerScale, distanceFactor, rolloffScale );
            ThrowOnError( result );
        }

        /// <summary>
        /// Retrieves the global doppler scale, distance factor and rolloff scale for all 3D sound in FMOD.  
        /// </summary>        
        /// <param name="dopplerScale">
        /// The scaling factor for doppler shift. Default = 1.0. 
        /// </param>
        /// <param name="distanceFactor">
        /// Relative distance factor to FMOD's units. Default = 1.0. (1.0 = 1 metre). 
        /// </param>
        /// <param name="rolloffScale">
        /// Scaling factor for 3D sound rolloff or attenuation for FMOD_3D_LOGROLLOFF 
        /// based sounds only (which is the default type). Default = 1.0.
        /// </param>
        public void Get3DSettings( out float dopplerScale, out float distanceFactor, out float rolloffScale )
        {
            RESULT result = this.system.get3DSettings( out dopplerScale, out distanceFactor, out rolloffScale );
            ThrowOnError( result );
        }

        /// <summary>
        /// Updates the position of the specified 3D sound listener.
        /// </summary>
        /// <remarks>
        /// The forward and up MUST be of unit length and perpendicular,
        /// or the audiosystem may crash on some systems.
        /// </remarks>
        /// <param name="listenerId">
        /// The listener ID in a multi-listener environment. Specify 0 if there is only 1 listener. 
        /// </param>
        /// <param name="position">
        /// The position of the listener in world space, measured in distance units.
        /// You can specify 0 to not update the position. 
        /// </param>
        /// <param name="velocity">
        /// The velocity of the listener measured in distance units per second.
        /// You can specify 0 or NULL to not update the velocity of the listener.
        /// </param>
        /// <param name="forward">
        /// The forwards orientation of the listener. 
        /// This vector must be of unit length and perpendicular to the up vector.
        /// </param>
        /// <param name="up">
        /// The upwards orientation of the listener.
        /// This vector must be of unit length and perpendicular to the forwards vector. 
        /// </param>
        public void Set3DListenerAttributes(
            int listenerId,
            Atom.Math.Vector3 position,
            Atom.Math.Vector3 velocity,
            Atom.Math.Vector3 forward,
            Atom.Math.Vector3 up )
        {
            VECTOR position2 = new VECTOR();
            position2.x = position.X;
            position2.y = position.Y;
            position2.z = position.Z;

            VECTOR velocity2 = new VECTOR();
            velocity2.x = velocity.X;
            velocity2.y = velocity.Y;
            velocity2.z = velocity.Z;

            VECTOR forward2 = new VECTOR();
            forward2.x = forward.X;
            forward2.y = forward.Y;
            forward2.z = forward.Z;

            VECTOR up2 = new VECTOR();
            up2.x = up.X;
            up2.y = up.Y;
            up2.z = up.Z;

            RESULT result = this.system.set3DListenerAttributes(
                listenerId,
                ref position2,
                ref velocity2,
                ref forward2,
                ref up2
            );

            ThrowOnError( result );
        }

        /// <summary>
        /// Getsor sets the position of the specified 2D sound listener.
        /// Must have been set using Set2DListenerAttributes.
        /// </summary>
        /// <value>
        /// The position of the listener in world space, measured in distance units.
        /// </value>
        public Atom.Math.Vector2 ListenerPosition2D
        {
            get
            {
                return this.last2DListenerPosition;
            }

            set
            {
                VECTOR position2 = new VECTOR();
                position2.x = value.X;
                position2.y = value.Y;

                VECTOR velocity2 = new VECTOR();
                VECTOR forward2 = new VECTOR();
                forward2.y = -1.0f;

                VECTOR up2 = new VECTOR();
                up2.z = 1.0f;

                RESULT result = this.system.set3DListenerAttributes(
                    0,
                    ref position2,
                    ref velocity2,
                    ref forward2,
                    ref up2
                );

                ThrowOnError( result );
                this.last2DListenerPosition = value;
            }
        }

        #endregion

        #region > Helpers <

        /// <summary>
        /// Helper function that throws an <see cref="AudioException"/>
        /// if the given native <see cref="RESULT"/> is not OK.
        /// </summary>
        /// <param name="result">The result to test for.</param>
        private static void ThrowOnError( RESULT result )
        {
            if( result != RESULT.OK )
            {
                throw new AudioException( Error.String( result ) );
            }
        }

        /// <summary>
        /// Helper function that logs the error
        /// if the given native <see cref="RESULT"/> is not OK.
        /// </summary>
        /// <param name="result">The result to test for.</param>
        private void LogOnError( RESULT result )
        {
            if( result != RESULT.OK )
            {
                if( this.lastError == result )
                    return;

                if( this.ErrorLog != null )
                {
                    this.ErrorLog.WriteLine( "FMOD_Error: " + Error.String( result ) );
                }

                this.lastError = result;
            }
        }

        /// <summary>
        /// Writes the given <paramref name="message"/> as an error into the <see cref="ErrorLog"/>.
        /// </summary>
        /// <param name="message">
        /// The message to write.
        /// </param>
        private void LogError( string message )
        {
            if( this.ErrorLog != null )
            {
                this.ErrorLog.WriteLine( LogSeverities.Error, message );
            }
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The underlying fmod system.
        /// </summary>
        private Atom.Fmod.Native.System system;

        /// <summary>
        /// The ChannelGroup all music play under by default.
        /// </summary>
        private ChannelGroup masterChannelGroup;

        /// <summary>
        /// The SoundGroup all music/samples plays under by default.
        /// </summary>
        private SoundGroup masterGroup, musicGroup, sampleGroup;

        /// <summary>
        /// The default <see cref="SoundManager"/> of the <see cref="AudioSystem"/>.
        /// </summary>
        private readonly SoundManager defaultSoundManager;

        /// <summary>
        /// The list of <see cref="SoundManager"/> that have been created with the <see cref="AudioSystem"/>.
        /// </summary>
        private readonly List<SoundManager> soundManagers = new List<SoundManager>();

        /// <summary>
        /// 
        /// </summary>
        private readonly List<Channel> endedChannels = new List<Channel>();

        /// <summary>
        /// States whether the <see cref="AudioSystem"/> has been initialized.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// The default path to Music resources.
        /// </summary>
        private string defaultMusicDirectory = @"Content\Music";

        /// <summary>
        /// The default path to Sample resources.
        /// </summary>
        private string defaultSampleDirectory = @"Content\Samples";

        /// <summary>
        /// The last error, cached to not log the same error multiple times.
        /// </summary>
        private RESULT lastError;

        /// <summary>
        /// Stores the last value set using Set2DListenerAttributes.
        /// </summary>
        private Atom.Math.Vector2 last2DListenerPosition = new Atom.Math.Vector2( float.NaN, float.NaN );

        #endregion
    }
}
