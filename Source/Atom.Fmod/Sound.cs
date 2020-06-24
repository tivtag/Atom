// <copyright file="Sound.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Fmod.Sound class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Fmod
{
    using global::System;
    using global::System.Threading;
    using Atom.Fmod.Native;

    /// <summary>
    /// Represents a sound resource which can be used to play on a <see cref="Atom.Fmod.Channel"/>.
    /// This class can't be inherited.
    /// </summary>
    public sealed class Sound
    {
        #region [ Events ]

        /// <summary>
        /// Called when a new <see cref="Atom.Fmod.Channel"/> for this Sound resource has been created.
        /// </summary>
        public event EventHandler<ChannelEventArgs> OnNewChannel;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the name of this Sound resource.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets the full path to this Sound Resource.
        /// </summary>
        public string FullPath
        {
            get
            {
                return this.fullPath;
            }
        }

        /// <summary>
        /// Gets or sets the mode of this Sound resource.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If the native Sound object is null, consider loading the Sound resource.
        /// </exception>
        /// <exception cref="AudioException">
        /// If an native FMOD error has occured.
        /// </exception>
        [CLSCompliant(false)]
        public MODE Mode
        {
            get
            {
                if( !nativeSound.hasHandle() )
                    throw new InvalidOperationException( Properties.Resources.Error_NativeSoundObjectIsNull );

                MODE mode;
                RESULT result = nativeSound.getMode( out mode );
                ThrowOnError( result );

                return mode;
            }

            set
            {
                if (!nativeSound.hasHandle())
                    throw new InvalidOperationException( Properties.Resources.Error_NativeSoundObjectIsNull );

                RESULT result = nativeSound.setMode( value );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets or sets how often the Sound will by default if
        /// it's Mode is FMOD_LOOP_NORMAL or FMOD_LOOP_BIDI.
        /// </summary>
        /// <value>
        /// Number of times to loop before stopping. 
        /// 0 = oneshot. 1 = loop once then stop. -1 = loop forever. Default = -1
        /// </value>
        /// <remarks>
        /// Native FMOD documentation remarks:
        /// This function does not affect FMOD_HARDWARE based sounds that are not streamable.
        /// FMOD_SOFTWARE based sounds or any type of sound created with System::CreateStream or 
        /// FMOD_CREATESTREAM will support this function.
        /// Issues with streamed audio. (Sounds created with with System::createStream or FMOD_CREATESTREAM). 
        /// When changing the loop count, sounds created with System::createStream or FMOD_CREATESTREAM 
        /// may already have been pre-buffered and executed their loop logic ahead of time, before this call was even made.
        /// This is dependant on the size of the sound versus the size of the stream decode buffer. 
        /// See FMOD_CREATESOUNDEXINFO.
        /// If this happens, you may need to reflush the stream buffer.
        /// To do this, you can call Channel::setPosition which forces a reflush of the stream buffer.
        /// Note this will usually only happen if you have sounds or 
        /// looppoints that are smaller than the stream decode buffer size. 
        /// Otherwise you will not normally encounter any problems.
        /// </remarks>
        /// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        public int LoopCount
        {
            set
            {
                if (!nativeSound.hasHandle())
                    throw new InvalidOperationException( Properties.Resources.Error_NativeSoundObjectIsNull );

                RESULT result = this.nativeSound.setLoopCount( value );
                ThrowOnError( result );
            }

            get
            {
                if (!nativeSound.hasHandle())
                    throw new InvalidOperationException( Properties.Resources.Error_NativeSoundObjectIsNull );

                int loopCount;
                RESULT result = this.nativeSound.getLoopCount( out loopCount );
                ThrowOnError( result );

                return loopCount;
            }
        }

        /// <summary>
        /// Gets the number of subsounds stored within the Sound.
        /// </summary>
        /// <remarks>
        /// A format that has subsounds is usually a container format,
        /// such as FSB, DLS, MOD, S3M, XM, IT.
        /// </remarks>
        public int SubSoundCount
        {
            get
            {
                if (!nativeSound.hasHandle())
                    throw new InvalidOperationException( Properties.Resources.Error_NativeSoundObjectIsNull );

                int count;
                RESULT result = nativeSound.getNumSubSounds( out count );
                ThrowOnError( result );

                return count;
            }
        }

        /// <summary>
        /// Gets the <see cref="AudioSystem"/> object that owns this Sound resource.
        /// </summary>
        public AudioSystem AudioSystem
        {
            get 
            {
                return this.audioSystem; 
            }
        }      

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        /// <param name="name"> The name of the sound file. </param>
        /// <param name="fullPath">The full path to the Sound Resource.</param>
        /// <param name="group">The optional group under which this sound is loaded.</param>
        /// <param name="audioSystem"> The sound manager that is used to find resource data. </param>
        internal Sound( string name, string fullPath, SoundGroup group, AudioSystem audioSystem )
        {
            this.name        = name;
            this.fullPath    = fullPath;
            this.soundGroup  = group;
            this.audioSystem = audioSystem;
        }
        
        /// <summary> 
        /// Loads the <see cref="Sound"/> resource.
        /// </summary>
        /// <param name="mode">
        /// The loading mode to use.
        /// </param>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        [CLSCompliant( false )]
        public void Load( MODE mode )
        {
            if( isLoaded )
                return;

            RESULT result = audioSystem.NativeSystem.createSound( this.fullPath, mode, out nativeSound );
            ThrowOnError( result );

            if( this.soundGroup != null )
            {
                result = this.nativeSound.setSoundGroup( this.soundGroup.NativeGroup );
                ThrowOnError( result );
            }

            isLoaded = true;
        }
        
        /// <summary> 
        /// Loads the <see cref="Sound"/> resource with the default settings a Sample would have:
        /// MODE.HARDWARE | MODE.CREATESAMPLE | MODE._2D | Looping Behaviour
        /// </summary>
        /// <param name="isLooping">
        /// States whether the Sound should be looping by default.
        /// </param>
        /// <exception cref="AudioException">
        /// If an native FMOD error has occured.
        /// </exception>
        /// <returns>
        /// This object for operation chaining.
        /// </returns>
        public Sound LoadAsSample( bool isLooping = false )
        {
            MODE mode = MODE.CREATESAMPLE | MODE._2D;
            if( isLooping )
                mode |= MODE.LOOP_NORMAL;
            else
                mode |= MODE.LOOP_OFF;

            this.LoadAsSample( mode );
            return this;
        }

        /// <summary> 
        /// Loads the <see cref="Sound"/> resource with the given settings in the Sample sound group.
        /// </summary>
        /// <exception cref="AudioException">
        /// If an native FMOD error has occured.
        /// </exception>
        /// <returns>
        /// This object for operation chaining.
        /// </returns>
        public Sound LoadAsSample( MODE mode )
        {
            this.soundGroup = this.audioSystem.SampleGroup;
            this.Load( mode );
            return this;
        }

        /// <summary> 
        /// Loads the <see cref="Sound"/> resource with the default settings a Music would have:
        /// MODE.HARDWARE | MODE.CREATESTREAM | MODE._2D | Looping Behaviour
        /// </summary>
        /// <param name="isLooping">
        /// States whether the Sound should be looping by default.
        /// </param>
        /// <exception cref="AudioException">
        /// If an native FMOD error has occured.
        /// </exception>
        public void LoadAsMusic( bool isLooping = true )
        {
            MODE mode = MODE.CREATESTREAM | MODE._2D;
            if( isLooping )
                mode |= MODE.LOOP_NORMAL;
            else
                mode |= MODE.LOOP_OFF;

            this.soundGroup = this.audioSystem.MusicGroup;
            this.Load( mode );
        }
        
        #endregion

        #region [ Methods ]

        #region - Play -
        
        /// <summary> Starts to play this <see cref="Sound"/> object. </summary>
        /// <exception cref="AudioException"> If an error has occured. </exception>
        /// <returns>
        /// A new managed <see cref="Channel"/> instance.
        /// </returns>
        public Channel Play()
        {
            return this.Play( false );
        }

        /// <summary>
        /// Starts to play this <see cref="Sound"/> object.
        /// </summary>
        /// <exception cref="AudioException">
        /// If an error has occured.
        /// </exception>
        /// <param name="startPaused">
        /// States whether the Sound starts paused or not.
        /// </param>
        /// <returns>
        /// A new managed <see cref="Channel"/> instance.
        /// </returns>
        public Channel Play( bool startPaused )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            Native.Channel nativeChannel;
            RESULT result = audioSystem.NativeSystem.playSound( nativeSound, nativeChannelGroup, startPaused, out nativeChannel );
            ThrowOnError( result );
               
            Channel channel = new Channel( this, nativeChannel );
            if( this.OnNewChannel != null )
                this.OnNewChannel( this, new ChannelEventArgs( channel ));

            return channel;
        }

        /// <summary>
        /// Starts to play this <see cref="Sound"/> object at the specified volumne.
        /// </summary>
        /// <exception cref="AudioException">
        /// If an error has occured.
        /// </exception>
        /// <param name="volume">
        /// The volume of the channel. Where 0 = silence and 1 = full volume.
        /// </param>
        /// <returns>
        /// A new managed <see cref="Channel"/> instance.
        /// </returns>
        public Channel Play( float volume )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            Native.Channel nativeChannel;
            RESULT result = audioSystem.NativeSystem.playSound( nativeSound, nativeChannelGroup, true, out nativeChannel );
            ThrowOnError( result );

            Channel channel = new Channel( this, nativeChannel );
            if( this.OnNewChannel != null )
                this.OnNewChannel( this, new ChannelEventArgs( channel ) );

            channel.Volume = volume;
            channel.Unpause();

            return channel;
        }

        #endregion

        #region - Release -

        /// <summary>
        /// Releases the <see cref="Sound"/> resource and its underlying native FMOD objects.
        /// </summary>
        /// <exception cref="AudioException"> If a native FMOD error has occured. </exception>
        public void Release()
        {
            if( isLoaded )
            {
                if( nativeSound.hasHandle() )
                {
                    Thread.BeginCriticalRegion(); 

                    RESULT result = nativeSound.release();
                    ThrowOnError( result );

                    Thread.EndCriticalRegion(); 
                }

                isLoaded = false;
            }
        }

        #endregion

        #region - Get/Set Defaults -

        /// <summary>
        /// Gets the default settings that are applied to new <see cref="Atom.Fmod.Channel"/>
        /// return by Play.
        /// </summary>
        /// <param name="frequency">
        /// Address of a variable that receives the default frequency for the sound.
        /// </param>
        /// <param name="volume">
        /// Address of a variable that receives the default volume for the sound. 
        /// Result will be from 0.0 to 1.0. 0.0 = Silent, 1.0 = full volume. Default = 1.0.
        /// </param>
        /// <param name="pan">
        /// Address of a variable that receives the default pan for the sound. 
        /// Result will be from -1.0 to +1.0. -1.0 = Full left, 0.0 = center, 1.0 = full right.
        /// Default = 0.0. 
        /// </param>
        /// <param name="priority">
        /// Address of a variable that receives the default priority for the sound when played on a channel.
        /// Result will be from 0 to 256. 0 = most important, 256 = least important. Default = 128.
        /// </param>
        /// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        public void GetDefaults(
              out float frequency,
              out float volume,
              out float pan,
              out int priority
            )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            frequency = 0.0f;
            volume    = 0.0f;
            pan       = 0.0f;
            priority  = 1;

            // ref volume, ref pan,
            RESULT result = nativeSound.getDefaults( out frequency, out priority );
            ThrowOnError( result );
        }

        /// <summary>
        /// Sets the default settings that are applied to new <see cref="Atom.Fmod.Channel"/>
        /// returned by Play. Doesn't change already existing Channels.
        /// </summary>
        /// <param name="frequency">
        /// The default frequency for the sound.
        /// </param>
        /// <param name="volume">
        /// The default volume for the sound. 
        /// Result will be from 0.0 to 1.0. 0.0 = Silent, 1.0 = full volume. Default = 1.0.
        /// </param>
        /// <param name="pan">
        /// The default pan for the sound. 
        /// Result will be from -1.0 to +1.0. -1.0 = Full left, 0.0 = center, 1.0 = full right.
        /// Default = 0.0. 
        /// </param>
        /// <param name="priority">
        /// The default priority for the sound when played on a channel.
        /// Result will be from 0 to 256. 0 = most important, 256 = least important. Default = 128.
        /// </param>
        /// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        public void SetDefaults(
              float frequency,
              float volume,
              float pan,
              int priority
            )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            /// volume, pan,
            RESULT result = nativeSound.setDefaults( frequency, priority );
            ThrowOnError( result );
        }

        #endregion

        #region - Get Format -

        /// <summary>
        /// Returns format information about the sound.  
        /// </summary>
        /// <param name="type">Address of a variable that receives the type of sound.</param>
        /// <param name="format">Address of a variable that receives the format of the sound. </param>
        /// <param name="channelCount">Address of a variable that receives the number of channels for the sound. </param>
        /// <param name="bits">
        /// Address of a variable that receives the number of bits per sample for the sound.
        /// This corresponds to FMOD_SOUND_FORMAT but is provided as an integer format for convenience.
        /// Hardware compressed formats such as VAG, XADPCM, GCADPCM that stay compressed in memory will return 0.
        /// </param>
        /// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        public void GetFormat( out SOUND_TYPE type, out SOUND_FORMAT format, out int channelCount, out int bits )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            RESULT result = nativeSound.getFormat( out type, out format, out channelCount, out bits );
            ThrowOnError( result );
        }

        #endregion

        #region - Get Length -

        /// <summary>
        /// Retrieves the length of thi Sound using the specified time unit.
        /// </summary>
        /// <param name="lengthType">Time unit retrieve into the length parameter.</param>
        /// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        /// <returns>
        /// The length of this Sound converted to the given TIMEUNIT.
        /// </returns>
        [CLSCompliant(false)]
        public uint GetLength( TIMEUNIT lengthType )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            uint length;
            RESULT result = nativeSound.getLength( out length, lengthType );
            ThrowOnError( result );

            return length;
        }

        /// <summary>
        /// Retrieves the length of the sound using the specified time unit.
        /// </summary>
        /// <param name="lengthType">Time unit retrieve into the length parameter.</param>
        /// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        /// <returns>
        /// The length of this Sound converted to the given TIMEUNIT.
        /// </returns>
        public int GetLengthInt( TIMEUNIT lengthType )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            uint length;
            RESULT result = nativeSound.getLength( out length, lengthType );
            ThrowOnError( result );

            return (int)length;
        }

        #endregion

        #region - Get TagCount -

        /// <summary>
        /// Retrieves the number of tags belonging to a sound.
        /// </summary>
        /// <param name="tagCount">
        /// Address of a variable that receives the number of tags in the sound.
        /// </param>
        /// <param name="updatedTagCount">
        /// Address of a variable that receives the number of tags updated since this function was last called.
        /// </param>
        /// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        public void GetTagCount( out int tagCount, out int updatedTagCount )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            RESULT result = nativeSound.getNumTags( out tagCount, out updatedTagCount );
            ThrowOnError( result );
        }

        #endregion        

        #region - Get/Set LoopPoints -

        /// <summary>
        /// Sets the loop points within this Sound resource.  
        /// </summary>
        /// <param name="start">The loop start point. This point in time is played, so it is inclusive. </param>
        /// <param name="startType">The time format used for the loop start point.</param>
        /// <param name="end">The loop end point. This point in time is played, so it is inclusive. </param>
        /// <param name="endType">The time format used for the loop end point.</param>
        /// <remarks>
        /// Native FMOD documentation:
        /// Not supported by static sounds created with FMOD_HARDWARE.
        /// Supported by sounds created with FMOD_SOFTWARE, or sounds of any type (hardware or software)
        /// created with System::createStream or FMOD_CREATESTREAM. 
        /// If a sound was 1000ms long and you wanted to loop the whole sound, loopstart would be 0, 
        /// and loopend would be 999,  not 1000.
        /// If loop end is smaller or equal to loop start, it will result in an error.
        /// If loop start or loop end is larger than the length of the sound, it will result in an error.
        /// </remarks>
        /// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        [CLSCompliant(false)]
        public void SetLoopPoints( uint start, TIMEUNIT startType, uint end, TIMEUNIT endType )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            RESULT result = nativeSound.setLoopPoints( start, startType, end, endType );
            ThrowOnError( result );
        }

        /// <summary>
        /// Sets the loop points within this Sound resource.  
        /// </summary>
        /// <param name="start">The loop start point. This point in time is played, so it is inclusive. </param>
        /// <param name="startType">The time format used for the loop start point.</param>
        /// <param name="end">The loop end point. This point in time is played, so it is inclusive. </param>
        /// <param name="endType">The time format used for the loop end point.</param>
        /// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        /// <remarks>
        /// Native FMOD documentation:
        /// Not supported by static sounds created with FMOD_HARDWARE.
        /// Supported by sounds created with FMOD_SOFTWARE, or sounds of any type (hardware or software)
        /// created with System::createStream or FMOD_CREATESTREAM. 
        /// If a sound was 1000ms long and you wanted to loop the whole sound, loopstart would be 0, 
        /// and loopend would be 999,  not 1000.
        /// If loop end is smaller or equal to loop start, it will result in an error.
        /// If loop start or loop end is larger than the length of the sound, it will result in an error.
        /// </remarks>
        public void SetLoopPointsInt( int start, TIMEUNIT startType, int end, TIMEUNIT endType )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            RESULT result = nativeSound.setLoopPoints( (uint)start, startType, (uint)end, endType );
            ThrowOnError( result );
        }

        /// <summary>
        /// Gets the loop points within this Sound resource.  
        /// </summary>
        /// <param name="start">The loop start point. This point in time is played, so it is inclusive.</param>
        /// <param name="startType">The time format used for the loop start point.</param>
        /// <param name="end">The loop end point. This point in time is played, so it is inclusive.</param>
        /// <param name="endType">The time format used for the loop end point.</param>
        /// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        /// <remarks>
        /// Native FMOD documentation:
        /// Not supported by static sounds created with FMOD_HARDWARE.
        /// Supported by sounds created with FMOD_SOFTWARE, or sounds of any type (hardware or software)
        /// created with System::createStream or FMOD_CREATESTREAM. 
        /// If a sound was 1000ms long and you wanted to loop the whole sound, loopstart would be 0, 
        /// and loopend would be 999,  not 1000.
        /// If loop end is smaller or equal to loop start, it will result in an error.
        /// If loop start or loop end is larger than the length of the sound, it will result in an error.
        /// </remarks>
        [CLSCompliant( false )]
        public void GetLoopPoints( out uint start, TIMEUNIT startType, out uint end, TIMEUNIT endType )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            RESULT result = nativeSound.getLoopPoints( out start, startType, out end, endType );
            ThrowOnError( result );
        }

        /// <summary>
        /// Gets the loop points within this Sound resource.  
        /// </summary>
        /// <param name="start">The loop start point. This point in time is played, so it is inclusive.</param>
        /// <param name="startType">The time format used for the loop start point.</param>
        /// <param name="end">The loop end point. This point in time is played, so it is inclusive.</param>
        /// <param name="endType">The time format used for the loop end point.</param>
        /// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        /// <remarks>
        /// Native FMOD documentation:
        /// Not supported by static sounds created with FMOD_HARDWARE.
        /// Supported by sounds created with FMOD_SOFTWARE, or sounds of any type (hardware or software)
        /// created with System::createStream or FMOD_CREATESTREAM. 
        /// If a sound was 1000ms long and you wanted to loop the whole sound, loopstart would be 0, 
        /// and loopend would be 999,  not 1000.
        /// If loop end is smaller or equal to loop start, it will result in an error.
        /// If loop start or loop end is larger than the length of the sound, it will result in an error.
        /// </remarks>
        public void GetLoopPointsInt( out int start, TIMEUNIT startType, out int end, TIMEUNIT endType )
        {
            if (!this.nativeSound.hasHandle())
            {
                throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
            }

            uint start2 = 0;
                 start  = 0;
            uint end2   = 0;
                 end    = 0; 

            RESULT result = nativeSound.getLoopPoints( out start2, startType, out end2, endType );
            ThrowOnError( result );

            start = (int)start2;
            end   = (int)end2;
        }

        #endregion

        ////#region - Get/Set Variations -

        /////// <summary>
        /////// Sets the current playback behaviour variations of this Sound resource.  
        /////// </summary>
        /////// <param name="frequencyVariation">
        /////// The frequency variation in hz. 
        /////// Frequency will play at its default frequency, plus or minus a random value within this range.
        /////// Default = 0.0.
        /////// </param>
        /////// <param name="volumeVariation">
        /////// Tthe volume variation. 0.0 to 1.0.
        /////// Sound will play at its default volume, plus or minus a random value within this range. 
        /////// Default = 0.0. 
        /////// </param>
        /////// <param name="panVariation">
        /////// The pan variation. 0.0 to 2.0. 
        /////// Sound will play at its default pan, plus or minus a random value within this range.
        /////// Pan is from -1.0 to +1.0 normally so the range can be a maximum of 2.0 in this case. 
        /////// Default = 0. Specify 0 or NULL to ignore.
        /////// </param>
        /////// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /////// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        ////public void SetVariations( float frequencyVariation, float volumeVariation, float panVariation )
        ////{
        ////    if (!this.nativeSound.hasHandle())
        ////    {
        ////        throw new InvalidOperationException(Properties.Resources.Error_NativeSoundObjectIsNull);
        ////    }

        ////    RESULT result = nativeSound.( frequencyVariation, volumeVariation, panVariation );
        ////    ThrowOnError( result );
        ////}

        /////// <summary>
        /////// Retrieves the current playback behaviour variations of this Sound resource.  
        /////// </summary>
        /////// <param name="frequencyVariation">
        /////// Address of a variable to receive the frequency variation in hz. 
        /////// Frequency will play at its default frequency, plus or minus a random value within this range.
        /////// Default = 0.0.
        /////// </param>
        /////// <param name="volumeVariation">
        /////// Address of a variable to receive the volume variation. 0.0 to 1.0.
        /////// Sound will play at its default volume, plus or minus a random value within this range. 
        /////// Default = 0.0. 
        /////// </param>
        /////// <param name="panVariation">
        /////// Address of a variable to receive the pan variation. 0.0 to 2.0. 
        /////// Sound will play at its default pan, plus or minus a random value within this range.
        /////// Pan is from -1.0 to +1.0 normally so the range can be a maximum of 2.0 in this case. 
        /////// Default = 0. Specify 0 or NULL to ignore.
        /////// </param>
        /////// <exception cref="InvalidOperationException">If the native Sound object is null, consider loading the Sound resource.</exception>
        /////// <exception cref="AudioException">If an native FMOD error has occured.</exception>
        ////public void GetVariations(
        ////      out float frequencyVariation,
        ////      out float volumeVariation,
        ////      out float panVariation
        ////)
        ////{
        ////    if( this.nativeSound == null )
        ////        throw new InvalidOperationException( Properties.Resources.Error_NativeSoundObjectIsNull );

        ////    frequencyVariation = 0.0f;
        ////    volumeVariation    = 0.0f;
        ////    panVariation       = 0.0f;

        ////    RESULT result = this.nativeSound.getVariations( ref frequencyVariation, ref volumeVariation, ref panVariation );
        ////    ThrowOnError( result );
        ////}

        ////#endregion

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

        #endregion

        #region [ Fields ]

        /// <summary>
        /// States whether the Sound resource has been loaded.
        /// </summary>
        private bool isLoaded;

        /// <summary>
        /// The group under which this sound is loaded.
        /// </summary>
        private SoundGroup soundGroup;

        /// <summary>
        /// The underlying sound object.
        /// </summary>
        private Native.Sound nativeSound;

        /// <summary>
        /// The channel group on which the sound plays.
        /// </summary>
        private readonly Native.ChannelGroup nativeChannelGroup = new Native.ChannelGroup();

        /// <summary>
        /// The name of the sound file. 
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The full path to the sound file.
        /// </summary>
        private readonly string fullPath;

        /// <summary>
        /// The underlying AudioSystem.
        /// </summary>
        private readonly AudioSystem audioSystem;

        #endregion
    }
}