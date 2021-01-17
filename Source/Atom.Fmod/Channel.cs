// <copyright file="Channel.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Fmod.Channel class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Fmod
{
    using System;
    using Atom.Fmod.Native;

    /// <summary>
    /// A 'channel' is an instance of a sound. You can play a sound many times at once,
    /// and each time you play a sound you will get a new channel handle. 
    /// This class can't be inherited.
    /// </summary>
    /// <remarks>
    /// Note that this is only if it is not a stream. Streams can only be played once at a time,
    /// and if you attempt to play it multiple times, it will simply restart the existing stream 
    /// and return the same handle that it was using before. 
    /// This is because streams only have 1 stream buffer, and 1 file handle. 
    /// To play a stream twice at once, open and play it twice. 
    /// </remarks>
    public sealed class Channel
    {
        /// <summary>
        /// Gets called when the Channel reaches the end.
        /// </summary>
        public event SimpleEventHandler<Channel> Ended
        {
            add
            {
                this.ended += value;
                HookCallback();
            }

            remove
            {
                this.ended -= value;

                if( ended == null || ended.GetInvocationList().Length == 0 )
                {
                    UnhookCallback();
                }
            }
        }
        
        #region [ Properties ]

        /// <summary>
        /// Gets the <see cref="Atom.Fmod.Sound"/> the <see cref="Atom.Fmod.Channel"/> was initialy
        /// created with. Be aware that Channels may get reused anytime.
        /// </summary>
        public Atom.Fmod.Sound Sound
        {
            get
            {
                return this.sound;
            }
        }

        /// <summary>
        /// Gets or sets the ChannelGroup this Channel is associated to.
        /// </summary>
        public ChannelGroup ChannelGroup
        {
            get
            {
                return this.channelGroup;
            }

            set
            {
                if( value == null )
                {
                    throw new ArgumentNullException( nameof( value ) );
                }

                RESULT result = nativeChannel.setChannelGroup( value.NativeGroup );
                ThrowOnError( result );
                
                this.channelGroup = value;

                // The following could be used to implement the 
                // functionality of accessing all Channels of a ChannelGroup.
                // For now this is not supported, as there is not much use for it.

                // this.channelGroup.RemoveChannel( this );
                // this.channelGroup = value;
                // this.channelGroup.AddChannel( this );
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Atom.Fmod.Channel"/> is currently playing.
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                if( !this.nativeChannel.hasHandle() )
                {
                    return false;
                }

                bool state = false;
                RESULT result = this.nativeChannel.isPlaying( out state );
                if( result == RESULT.ERR_INVALID_HANDLE || result == RESULT.ERR_CHANNEL_STOLEN )
                {
                    return false;
                }

                ThrowOnError( result );
                return state;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Atom.Fmod.Channel"/> has been paused.
        /// </summary>
        /// <remarks>
        /// If a channel belongs to a paused channelgroup, it will stay paused regardless of the channel pause state. 
        /// The channel pause state will still be reflected internally though, ie Channel::Paused will still return 
        /// the value you set. If the channelgroup has paused set to false, this function will become effective again.
        /// </remarks>
        public bool IsPaused
        {
            get
            {
                bool state;
                RESULT result = nativeChannel.getPaused( out state );
                ThrowOnError( result );

                return state;
            }

            set
            {
                RESULT result = nativeChannel.setPaused( value );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Atom.Fmod.Channel"/> has been muted.
        /// </summary>
        /// <value>true = channel is muted (silent), false = channel is set its to normal volume.</value>
        /// <remarks>
        /// If a channel belongs to a muted channelgroup, it will stay muted regardless of the channel mute state.
        /// The channel mute state will still be reflected internally though, ie Channel::getMute will still 
        /// return the value you set. If the channelgroup has mute set to false, this function will become effective again. 
        /// </remarks>
        public bool IsMuted
        {
            get
            {
                bool state;
                RESULT result = nativeChannel.getMute(out state );
                ThrowOnError( result );

                return state;
            }

            set
            {
                RESULT result = nativeChannel.setMute( value );
                ThrowOnError( result );
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether the <see cref="Atom.Fmod.Channel"/> is virtual (emulated) 
        /// or not due to FMOD Ex's virtual channel management system.
        /// </summary>
        /// <remarks>
        /// Native documentation:
        /// Virtual channels are not audible, because there are no more real hardware or software channels available.
        /// If you are plotting virtual voices vs real voices graphically, and wondering why FMOD sometimes chooses seemingly
        /// random channels to be virtual that are usually far away, that is because they are probably silent. 
        /// It doesn't matter which are virtual and which are not if they are silent. 
        /// Virtual voices are not calculation on 'closest to listener' calculation, they are based on audibility.
        /// See the tutorial in the FMOD Ex documentation for more information on virtual channels. 
        /// </remarks>
        public bool IsVirtual
        {
            get
            {
                bool state;
                RESULT result = nativeChannel.isVirtual( out state );
                ThrowOnError( result );

                return state;
            }
        }

        /// <summary>
        /// Gets or sets how often the <see cref="Atom.Fmod.Channel"/> should loop before it stops.
        /// </summary>
        /// <remarks>
        /// Native documentation:
        /// This function does not affect FMOD_HARDWARE
        /// based sounds that are not streamable.
        /// FMOD_SOFTWARE based sounds or any type of sound created with
        /// System::CreateStream or FMOD_CREATESTREAM will support this function.
        /// </remarks>
        public int LoopCount
        {
            get
            {
                int count;
                RESULT result = nativeChannel.getLoopCount( out count );
                ThrowOnError( result );

                return count;
            }

            set
            {
                RESULT result = nativeChannel.setLoopCount( value );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets or sets the volume level of the <see cref="Atom.Fmod.Channel"/>.
        /// Where 0.0 = silent, 1.0 = full volume. Default = 1.0.
        /// </summary>
        /// <remarks>
        /// Set: When a sound is played, it plays at the default volume of the sound 
        /// which can be set by Sound::SetDefaults.
        /// For most file formats, the volume is determined by the audio format. 
        /// </remarks>
        public float Volume
        {
            get
            {
                float volume;
                RESULT result = nativeChannel.getVolume( out volume );
                ThrowOnError( result );
                
                return volume;
            }

            set
            {
                RESULT result = nativeChannel.setVolume( value );
                ThrowOnError( result );
            }
        }

        /////// <summary>
        /////// Gets or sets the <see cref="Atom.Fmod.Channel"/>s pan position.
        /////// Where -1.0 = Full left, 1.0 = full right. Default = 0.0.
        /////// </summary>
        /////// <remarks>
        /////// Native remarks:
        /////// This function only works on sounds created with FMOD_2D. 3D sounds are not pannable 
        /////// and will return FMOD_ERR_NEEDS2D.
        /////// Only sounds that are mono or stereo can be panned. Multichannel sounds (ie >2 channels) cannot be panned.
        /////// Mono sounds are panned from left to right using constant power panning (non linear fade). 
        /////// This means when pan = 0.0, the balance for the sound in each speaker is 71% left and 71% right,
        /////// not 50% left and 50% right. This gives (audibly) smoother pans.
        /////// Stereo sounds heave each left/right value faded up and down according to the specified pan position. 
        /////// This means when pan = 0.0, the balance for the sound in each speaker is 100% left and 100% right. 
        /////// When pan = -1.0, only the left channel of the stereo sound is audible, 
        /////// when pan = 1.0, only the right channel of the stereo sound is audible.
        /////// Panning does not work if the speaker mode is FMOD_SPEAKERMODE_RAW.
        /////// </remarks>
        ////public float Pan
        ////{
        ////    get
        ////    {
        ////        float pan = 0.0f;
        ////        RESULT result = nativeChannel.getpa( ref pan );
        ////        ThrowOnError( result );

        ////        return pan;
        ////    }

        ////    set
        ////    {
        ////        RESULT result = nativeChannel.setPan( value );
        ////        ThrowOnError( result );
        ////    }
        ////}

        /// <summary>
        /// Gets or sets the frequency or playback rate of the <see cref="Atom.Fmod.Channel"/> in, in HZ.  
        /// </summary>
        /// <value>
        /// A frequency value in HZ. This value can also be negative to play the sound backwards
        /// (negative frequencies allowed with FmodMode.Software based non-stream sounds only).
        /// DirectSound hardware voices have limited frequency range on some soundcards.
        /// Please see remarks for more on this. 
        /// </value>
        /// <remarks>
        /// When a sound is played, it plays at the default frequency of the sound which can be set by Sound::SetDefaults.
        /// For most file formats, the volume is determined by the audio format.
        /// Frequency limitations for sounds created with FmodMode.Hardware in DirectSound.
        /// Every hardwa re device has a minimum and maximum frequency. 
        /// This means setting the frequency above the maximum and below the minimum will have no effect.
        /// FMOD clamps frequencies to these values when playing back on hardware, 
        /// so if you are setting the frequency outside of this range, 
        /// the frequency will stay at either the minimum or maximum.
        /// Note that FMOD_SOFTWARE based sounds do not have this limitation.
        /// To find out the minimum and maximum value before initializing FMOD
        /// (maybe to decide whether to use a different soundcard, output mode, or drop back fully to software mixing),
        /// you can use the AudioSystem::GetDriverCaps function. 
        /// </remarks>
        public float Frequency
        {
            get
            {
                float frequency;
                RESULT result = nativeChannel.getFrequency( out frequency );
                ThrowOnError( result );

                return frequency;
            }

            set
            {
                RESULT result = nativeChannel.setFrequency( value );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets the combined volume of the channel after 3d sound, volume, channel group volume 
        /// and geometry occlusion calculations have been performed on it.  
        /// </summary>
        /// <remarks>
        /// This does not represent the waveform, just the calculated volume based on 3d distance, occlusion,
        /// volume and channel group volume. This value is used by the FMOD Ex virtual channel system to order
        /// its channels between real and virtual.
        /// </remarks>
        public float Audibility
        {
            get
            {
                float audibility;
                RESULT result = nativeChannel.getAudibility(out audibility );
                ThrowOnError( result );

                return audibility;
            }
        }

        /// <summary>
        /// Gets or sets the priority of the <see cref="Atom.Fmod.Channel"/>.
        /// This is a value from 0 to 256 inclusive. 0 = most important. 256 = least important. Default = 128. 
        /// </summary>
        /// <remarks>
        /// Priority will make a channel more important or less important than its counterparts. 
        /// When virtual channels are in place, by default the importance of the sound
        /// (whether it is audible or not when more channels are playing than exist) is based on the volume,
        /// or audiblity of the sound. This is determined by distance from the listener in 3d,
        /// the volume set with Channel::setVolume, channel group volume, and geometry occlusion factors.
        /// To make a quiet sound more important, so that it isn't made virtual by louder sounds, 
        /// you can use this function to increase its importance, and keep it audible.
        /// </remarks>
        public int Priority
        {
            get
            {
                int priority;
                RESULT result = nativeChannel.getPriority(out priority );
                ThrowOnError( result );

                return priority;
            }

            set
            {
                RESULT result = nativeChannel.setPriority( value );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets or sets the spread of a 3d stereo or multichannel sound in speaker space.
        /// </summary>
        /// <value>
        /// The spread angle for subchannels. 0 = all subchannels are located at the same position.
        /// 360 = all subchannels are located at the opposite position. 
        /// </value>
        /// <remarks>
        /// !! Only affects sounds created with FMOD_SOFTWARE. !!
        /// Normally a 3d sound is aimed at one position in a speaker array depending on the 3d position, 
        /// to give it direction. Left and right parts of a stereo sound for example are consequently
        /// summed together and become 'mono'.
        /// When increasing the 'spread' of a sound, the left and right parts of a stereo sound rotate 
        /// away from their original position, to give it more 'stereoness'.
        /// The rotation of the sound channels are done in 'speaker space'.
        /// </remarks>
        public float Spread3D
        {
            get
            {
                float spread;
                RESULT result = nativeChannel.get3DSpread(out spread );
                ThrowOnError( result );

                return spread;
            }

            set
            {
                RESULT result = nativeChannel.set3DSpread( value );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets or sets how much the 3d engine has an effect on the channel, versus that set by Channel::setPan, Channel::setSpeakerMix, Channel::setSpeakerLevels
        /// </summary>
        /// <value>
        /// 1 = Sound pans and attenuates according to 3d position. 0 = Attenuation is ignored and pan/speaker levels are defined by Channel::setPan, Channel::setSpeakerMix, Channel::setSpeakerLevels.
        /// Default = 1 (all by 3D position). 
        /// </value>
        public float Level3D
        {
            get
            {
                float level;
                RESULT result = nativeChannel.get3DLevel( out level );
                ThrowOnError( result );

                return level;
            }

            set
            {
                RESULT result = nativeChannel.set3DLevel( value );
                ThrowOnError( result );
            }
        }

        /// <summary>
        /// Gets the <see cref="AudioSystem"/> that owns this Channel.
        /// </summary>
        public AudioSystem AudioSystem
        {
            get { return sound.AudioSystem; }
        }

        /// <summary>
        /// Gets or sets the pointer to optional user data.
        /// </summary>
        /// <value>The default value is InpPtr.Zero.</value>
        public IntPtr UserData
        {
            get 
            {
                IntPtr ptr = IntPtr.Zero;
                RESULT result = nativeChannel.getUserData( out ptr );

                return ptr;
            }

            set 
            {
                RESULT result = nativeChannel.setUserData( value );
                ThrowOnError( result );
            }
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="Atom.Fmod.Channel"/> class.
        /// </summary>
        /// <param name="sound">
        /// The <see cref="Atom.Fmod.Sound"/> the <see cref="Atom.Fmod.Channel"/> was initialy
        /// created with.
        /// </param>
        /// <param name="nativeChannel">
        /// The <see cref="Atom.Fmod.Native.Channel"/> object that is wrapped by the <see cref="Atom.Fmod.Channel"/>.
        /// </param>
        internal Channel( Atom.Fmod.Sound sound, Atom.Fmod.Native.Channel nativeChannel )
        {
            this.sound         = sound;
            this.nativeChannel = nativeChannel;

            // Setup ChannelGroup.
            this.ChannelGroup = sound.AudioSystem.MasterChannelGroup;

        }

        /// <summary>
        /// Finalizes an instance of the Channel class.
        /// </summary>
        ~Channel()
        {
            UnhookCallback();
        }

        private void HookCallback()
        {
            if( this.endCallback == null )
            {
                this.endCallback = new CHANNELCONTROL_CALLBACK( CHANNEL_CALLBACK_END );
                RESULT result = nativeChannel.setCallback( endCallback );
                ThrowOnError( result );
            }
        }

        private void UnhookCallback()
        {
            // Reset native callbacks; we need to do this or we get
            // CallbackOnCollectedDelegate exceptions.
            nativeChannel.setCallback( null );
            endCallback = null;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Stops to play this <see cref="Atom.Fmod.Channel"/>.
        /// </summary>
        public void Stop()
        {
            RESULT result = nativeChannel.stop();
            if( result == RESULT.ERR_INVALID_HANDLE || result == RESULT.ERR_CHANNEL_STOLEN )
            {
                return;
            }

            ThrowOnError( result );
        }

        /// <summary>
        /// Pauses this Channel.
        /// </summary>
        public void Pause()
        {
            this.IsPaused = true;
        }

        /// <summary>
        /// Unpauses this Channel.
        /// </summary>
        public void Unpause()
        {
            this.IsPaused = false;
        }

        #region - Get/Set Position -

        /// <summary>
        /// Gets the current PCM offset or playback position of this <see cref="Atom.Fmod.Channel"/>.
        /// </summary>
        /// <param name="positionType">Time unit to retrieve into the position parameter.</param>
        /// <remarks>
        /// Certain timeunits do not work depending on the file format. 
        /// For example FMOD_TIMEUNIT_MODORDER will not work with an mp3 file.
        /// A PCM sample is a unit of measurement in audio that contains the data for one audible element of sound.
        /// 1 sample might be 16bit stereo, so 1 sample contains 4 bytes. 
        /// 44,100 samples of a 44khz sound would represent 1 second of data.
        /// </remarks>
        /// <returns>
        /// The current PCM offset or playback position of this Channel, converted to the given TIMEUNIT.
        /// </returns>
        [CLSCompliant( false )]
        public uint GetPosition( TIMEUNIT positionType )
        {
            uint position;
            RESULT result = nativeChannel.getPosition( out position, positionType );
            ThrowOnError( result );
            return position;
        }

        /// <summary>
        /// Gets the current PCM offset or playback position of the <see cref="Atom.Fmod.Channel"/>,
        /// may be less accurate than GetPosition !
        /// </summary>
        /// <param name="positionType">Time unit to retrieve into the position parameter.</param>
        /// <remarks>
        /// Certain timeunits do not work depending on the file format. 
        /// For example FMOD_TIMEUNIT_MODORDER will not work with an mp3 file.
        /// A PCM sample is a unit of measurement in audio that contains the data for one audible element of sound.
        /// 1 sample might be 16bit stereo, so 1 sample contains 4 bytes. 
        /// 44,100 samples of a 44khz sound would represent 1 second of data.
        /// </remarks>
        /// <returns>
        /// The current PCM offset or playback position of this Channel, converted to the given TIMEUNIT.
        /// </returns>
        public int GetPositionInt( TIMEUNIT positionType )
        {
            uint position;
            RESULT result = nativeChannel.getPosition(out position, positionType );
            ThrowOnError( result );
            return (int)position;
        }

        /// <summary>
        /// Sets the current playback position for the currently playing sound to the specified PCM offset.  
        /// </summary>
        /// <param name="position">Position of the channel to set in units specified in the postype parameter. </param>
        /// <param name="positionType">Time unit to retrieve into the position parameter.</param>
        /// <remarks>
        /// Certain timeunits do not work depending on the file format. 
        /// For example FMOD_TIMEUNIT_MODORDER will not work with an mp3 file.
        /// A PCM sample is a unit of measurement in audio that contains the data for one audible element of sound.
        /// 1 sample might be 16bit stereo, so 1 sample contains 4 bytes. 
        /// 44,100 samples of a 44khz sound would represent 1 second of data.
        /// <para>
        /// Note that if you are calling this function on a stream, it has to possibly reflush its buffer
        /// to get zero latency playback when it resumes playing, therefore it could potentially cause
        /// a stall or take a small amount of time to do this. 
        /// Warning! Using a VBR source that does not have an associated seek table or seek information 
        /// (such as MP3 or MOD/S3M/XM/IT) may cause inaccurate seeking if you specify FMOD_TIMEUNIT_MS or FMOD_TIMEUNIT_PCM.
        /// If you want FMOD to create a pcm vs bytes seek table so that seeking is accurate, 
        /// you will have to specify FMOD_ACCURATETIME when loading or opening the sound. 
        /// This means there is a slight delay as FMOD scans the whole file when loading the sound to create this table.
        /// </para>
        /// </remarks>
        [CLSCompliant( false )]
        public void SetPosition( uint position, TIMEUNIT positionType )
        {
            RESULT result = nativeChannel.setPosition( position, positionType );
            ThrowOnError( result );
        }

        /// <summary>
        /// Sets the current playback position for the currently playing sound to the specified PCM offset.  
        /// </summary>
        /// <param name="position">Position of the channel to set in units specified in the postype parameter. </param>
        /// <param name="positionType">Time unit to retrieve into the position parameter.</param>
        /// <remarks>
        /// Certain timeunits do not work depending on the file format. 
        /// For example FMOD_TIMEUNIT_MODORDER will not work with an mp3 file.
        /// A PCM sample is a unit of measurement in audio that contains the data for one audible element of sound.
        /// 1 sample might be 16bit stereo, so 1 sample contains 4 bytes. 
        /// 44,100 samples of a 44khz sound would represent 1 second of data.
        /// <para>
        /// Note that if you are calling this function on a stream, it has to possibly reflush its buffer
        /// to get zero latency playback when it resumes playing, therefore it could potentially cause
        /// a stall or take a small amount of time to do this. 
        /// Warning! Using a VBR source that does not have an associated seek table or seek information 
        /// (such as MP3 or MOD/S3M/XM/IT) may cause inaccurate seeking if you specify FMOD_TIMEUNIT_MS or FMOD_TIMEUNIT_PCM.
        /// If you want FMOD to create a pcm vs bytes seek table so that seeking is accurate, 
        /// you will have to specify FMOD_ACCURATETIME when loading or opening the sound. 
        /// This means there is a slight delay as FMOD scans the whole file when loading the sound to create this table.
        /// </para>
        /// </remarks>
        public void SetPositionInt( int position, TIMEUNIT positionType )
        {
            RESULT result = nativeChannel.setPosition( (uint)position, positionType );
            ThrowOnError( result );
        }

        #endregion

        #region - Get/Set Delay -

        /// <summary>
        /// Sets a start (and/or stop) time relative to the parent channel group DSP clock, with sample accuracy.
        /// </summary>
        /// <param name="dspclockStart">
        /// DSP clock of the parent channel group to audibly start playing sound at, a value of 0 indicates no delay.
        /// </param>
        /// <param name="dspclockEnd">
        /// DSP clock of the parent channel group to audibly stop playing sound at, a value of 0 indicates no delay.
        /// </param>
        /// <param name="stopChannels">
        /// TRUE = stop according to ChannelControl::isPlaying. FALSE = remain 'active' and a new start delay could start playback again at a later time.
        /// </param>
        public void SetDelay( ulong dspclockStart, ulong dspclockEnd, bool stopChannels)
        {
            RESULT result = nativeChannel.setDelay(dspclockStart, dspclockEnd, stopChannels);
            ThrowOnError( result );
        }

        /// <summary>
        /// Gets the currently set delay values. 
        /// </summary>
        /// <remarks>
        /// Note! Only works with sounds created with FMOD_SOFTWARE.
        /// If FMOD_DELAYTYPE_DSPCLOCK_START is used, this will be the value of the DSP clock time at the time System::playSound was called,
        /// if the user has not called Channel::setDelay.
        /// What is the 'dsp clock'
        /// The DSP clock represents the output stream to the soundcard, and is incremented by the output rate every second 
        /// (though of course with much finer granularity than this).
        /// So if your output rate is 48khz, the DSP clock will increment by 48000 per second.
        /// The hi and lo values represent this 64bit number,
        /// with the delaylo representing the least significant 32bits and the delayhi value representing the most significant 32bits.
        /// </remarks>
        /// <param name="dspclockStart">
        /// Address of a variable to receive the top (most significant) 32 bits of a 64bit number representing the time.
        /// </param>
        /// <param name="dspclockEnd">
        /// Address of a variable to receive the bottom (least significant) 32 bits of a 64bit number representing the time.
        /// </param>
        public void GetDelay( out ulong dspclockStart, out ulong dspclockEnd )
        {
            RESULT result = nativeChannel.getDelay( out dspclockStart, out dspclockEnd);
            ThrowOnError( result );
        }

        #endregion

        #region - Get/Set 3DAttributes -

        /// <summary>
        /// Sets the position and velocity of this 3d channel. 
        /// </summary>
        /// <param name="positionX">
        /// The position on the x-axis in 3D space of the channel. Specifying 0 will ignore this parameter. 
        /// </param>
        /// <param name="positionY">
        /// The position on the y-axis in 3D space of the channel. Specifying 0 will ignore this parameter.
        /// </param>
        /// <param name="positionZ">
        /// The position on the z-axis in 3D space of the channel. Specifying 0 will ignore this parameter.
        /// </param>
        /// <param name="velocityX">
        /// Velocity in 'distance units per second' on the x-axis in 3D space of the channel. See remarks. Specifying 0 will ignore this parameter.
        /// </param>
        /// <param name="velocityY">
        /// Velocity in 'distance units per second' on the y-axis in 3D space of the channel. See remarks. Specifying 0 will ignore this parameter.
        /// </param>
        /// <param name="velocityZ">
        /// Velocity in 'distance units per second' on the z-axis in 3D space of the channel. See remarks. Specifying 0 will ignore this parameter.
        /// </param>
        /// <remarks>
        /// A 'distance unit' is specified by System::set3DSettings. By default this is set to meters which is a distance scale of 1.0.
        /// For a stereo 3d sound, you can set the spread of the left/right parts in speaker space by using Channel::set3DSpread. 
        /// </remarks>
        public void Set3DAttributes( 
            float positionX,
            float positionY, 
            float positionZ,
            float velocityX,
            float velocityY, 
            float velocityZ )
        {
            VECTOR position = new VECTOR();
            position.x = positionX;
            position.y = positionY;
            position.z = positionZ;

            VECTOR velocity = new VECTOR();
            velocity.x = velocityX;
            velocity.y = velocityY;
            velocity.z = velocityZ;

            RESULT result = nativeChannel.set3DAttributes( ref position, ref velocity );
            ThrowOnError( result );
        }

        /// <summary>
        /// Sets the position and velocity of this 3d channel. 
        /// </summary>
        /// <param name="positionX">
        /// The position on the x-axis in 3D space of the channel. Specifying 0 will ignore this parameter. 
        /// </param>
        /// <param name="positionY">
        /// The position on the y-axis in 3D space of the channel. Specifying 0 will ignore this parameter.
        /// </param>
        /// <param name="positionZ">
        /// The position on the z-axis in 3D space of the channel. Specifying 0 will ignore this parameter.
        /// </param>
        /// <remarks>
        /// A 'distance unit' is specified by System::set3DSettings. By default this is set to meters which is a distance scale of 1.0.
        /// For a stereo 3d sound, you can set the spread of the left/right parts in speaker space by using Channel::set3DSpread. 
        /// </remarks>
        public void Set3DAttributes( float positionX, float positionY, float positionZ )
        {
            VECTOR position = new VECTOR();
            position.x = positionX;
            position.y = positionY;
            position.z = positionZ;

            VECTOR velocity = new VECTOR();

            RESULT result = nativeChannel.set3DAttributes( ref position, ref velocity );
            ThrowOnError( result );
        }

        /// <summary>
        /// Sets the position of this 3d channel. 
        /// </summary>
        /// <param name="positionX">
        /// The position on the x-axis in 3D space of the channel. Specifying 0 will ignore this parameter. 
        /// </param>
        /// <param name="positionY">
        /// The position on the y-axis in 3D space of the channel. Specifying 0 will ignore this parameter.
        /// </param>
        /// <remarks>
        /// A 'distance unit' is specified by System::set3DSettings. By default this is set to meters which is a distance scale of 1.0.
        /// For a stereo 3d sound, you can set the spread of the left/right parts in speaker space by using Channel::set3DSpread. 
        /// </remarks>
        public void Set3DAttributes( float positionX, float positionY )
        {
            VECTOR position = new VECTOR();
            position.x = positionX;
            position.y = positionY;

            VECTOR velocity = new VECTOR();

            RESULT result = nativeChannel.set3DAttributes( ref position, ref velocity );
            ThrowOnError( result );
        }

        /// <summary>
        /// Gets the position and velocity of this 3d channel. 
        /// </summary>
        /// <param name="positionX">
        /// The position on the x-axis in 3D space of the channel.
        /// </param>
        /// <param name="positionY">
        /// The position on the y-axis in 3D space of the channel.
        /// </param>
        /// <param name="positionZ">
        /// The position on the z-axis in 3D space of the channel.
        /// </param>
        /// <param name="velocityX">
        /// Velocity in 'distance units per second' on the x-axis in 3D space of the channel.
        /// </param>
        /// <param name="velocityY">
        /// Velocity in 'distance units per second' on the y-axis in 3D space of the channel.
        /// </param>
        /// <param name="velocityZ">
        /// Velocity in 'distance units per second' on the z-axis in 3D space of the channel.
        /// </param>
        /// <remarks>
        /// A 'distance unit' is specified by System::set3DSettings. By default this is set to meters which is a distance scale of 1.0.
        /// For a stereo 3d sound, you can set the spread of the left/right parts in speaker space by using Channel::set3DSpread. 
        /// </remarks>
        public void Get3DAttributes( 
            out float positionX,
            out float positionY, 
            out float positionZ,
            out float velocityX, 
            out float velocityY, 
            out float velocityZ )
        {
            VECTOR position;
            VECTOR velocity;

            RESULT result = nativeChannel.get3DAttributes( out position, out velocity );
            ThrowOnError( result );

            positionX = position.x;
            positionY = position.y;
            positionZ = position.z;
            velocityX = velocity.x;
            velocityY = velocity.y;
            velocityZ = velocity.z;
        }

        /// <summary>
        /// Gets the position of this 3d channel. 
        /// </summary>
        /// <param name="positionX">
        /// The position on the x-axis in 3D space of the channel.
        /// </param>
        /// <param name="positionY">
        /// The position on the y-axis in 3D space of the channel.
        /// </param>
        /// <param name="positionZ">
        /// The position on the z-axis in 3D space of the channel.
        /// </param>
        /// <remarks>
        /// A 'distance unit' is specified by System::set3DSettings. By default this is set to meters which is a distance scale of 1.0.
        /// For a stereo 3d sound, you can set the spread of the left/right parts in speaker space by using Channel::set3DSpread. 
        /// </remarks>
        public void Get3DAttributes( out float positionX, out float positionY, out float positionZ )
        {
            VECTOR position;
            VECTOR velocity;

            RESULT result = nativeChannel.get3DAttributes(out position, out velocity );
            ThrowOnError( result );

            positionX = position.x;
            positionY = position.y;
            positionZ = position.z;
        }

        /// <summary>
        /// Gets the position of this 3d channel. 
        /// </summary>
        /// <param name="positionX">
        /// The position on the x-axis in 3D space of the channel.
        /// </param>
        /// <param name="positionY">
        /// The position on the y-axis in 3D space of the channel.
        /// </param>
        /// <remarks>
        /// A 'distance unit' is specified by System::set3DSettings. By default this is set to meters which is a distance scale of 1.0.
        /// For a stereo 3d sound, you can set the spread of the left/right parts in speaker space by using Channel::set3DSpread. 
        /// </remarks>
        public void Get3DAttributes( out float positionX, out float positionY )
        {
            VECTOR position = new VECTOR();
            VECTOR velocity = new VECTOR();

            RESULT result = nativeChannel.get3DAttributes(out position, out velocity );
            ThrowOnError( result );

            positionX = position.x;
            positionY = position.y;
        }

        #endregion

        #region - Get/Set 3DMinMaxDistance -

        /// <summary>
        /// Sets the minimum and maximum audible distance for this channel.
        /// </summary>
        /// <param name="minimumDistance">
        /// The channel's minimum volume distance in "units". The default value is 1.0f. See remarks for more on units. 
        /// </param>
        /// <param name="maximumDistance">
        /// The channel's maximum volume distance in "units". The default value is 10000.0f. See remarks for more on units.
        /// </param>
        /// <remarks>
        /// MinDistance is the minimum distance that the sound emitter will cease to continue growing louder at (as it approaches the listener).
        /// Within the mindistance it stays at the constant loudest volume possible. Outside of this mindistance it begins to attenuate.
        /// MaxDistance is the distance a sound stops attenuating at. Beyond this point it will stay at the volume it would be at maxdistance units from the listener and will not attenuate any more.
        /// MinDistance is useful to give the impression that the sound is loud or soft in 3d space. An example of this is a small quiet object, such as a bumblebee, which you could set a mindistance of to 0.1 for example, which would cause it to attenuate quickly and dissapear when only a few meters away from the listener.
        /// Another example is a jumbo jet, which you could set to a mindistance of 100.0, which would keep the sound volume at max until the listener was 100 meters away, then it would be hundreds of meters more before it would fade out.
        /// <para>
        /// In summary, increase the mindistance of a sound to make it 'louder' in a 3d world, and decrease it to make it 'quieter' in a 3d world.
        /// maxdistance is effectively obsolete unless you need the sound to stop fading out at a certain point. Do not adjust this from the default if you dont need to.
        /// Some people have the confusion that maxdistance is the point the sound will fade out to, this is not the case.
        /// </para>
        /// <para>
        /// A 'distance unit' is specified by System::set3DSettings. By default this is set to meters which is a distance scale of 1.0.
        /// The default units for minimum and maximum distances are 1.0 and 10000.0f.
        /// Volume drops off at mindistance / distance.
        /// To define the min and max distance per sound and not per channel use Sound::set3DMinMaxDistance.
        /// </para>
        /// If FMOD_3D_CUSTOMROLLOFF is used, then these values are stored, but ignored in 3d processing.
        /// </remarks>
        public void Set3DMinMaxDistance( float minimumDistance, float maximumDistance )
        {
            RESULT result = nativeChannel.set3DMinMaxDistance( minimumDistance, maximumDistance );
            ThrowOnError( result );
        }

        /// <summary>
        /// Sets the minimum and maximum audible distance for this channel.
        /// </summary>
        /// <param name="minimumDistance">
        /// The channel's minimum volume distance in "units". The default value is 1.0f. See remarks for more on units. 
        /// </param>
        /// <param name="maximumDistance">
        /// The channel's maximum volume distance in "units". The default value is 10000.0f. See remarks for more on units.
        /// </param>
        /// <remarks>
        /// MinDistance is the minimum distance that the sound emitter will cease to continue growing louder at (as it approaches the listener).
        /// Within the mindistance it stays at the constant loudest volume possible. Outside of this mindistance it begins to attenuate.
        /// MaxDistance is the distance a sound stops attenuating at. Beyond this point it will stay at the volume it would be at maxdistance units from the listener and will not attenuate any more.
        /// MinDistance is useful to give the impression that the sound is loud or soft in 3d space. An example of this is a small quiet object, such as a bumblebee, which you could set a mindistance of to 0.1 for example, which would cause it to attenuate quickly and dissapear when only a few meters away from the listener.
        /// Another example is a jumbo jet, which you could set to a mindistance of 100.0, which would keep the sound volume at max until the listener was 100 meters away, then it would be hundreds of meters more before it would fade out.
        /// <para>
        /// In summary, increase the mindistance of a sound to make it 'louder' in a 3d world, and decrease it to make it 'quieter' in a 3d world.
        /// maxdistance is effectively obsolete unless you need the sound to stop fading out at a certain point. Do not adjust this from the default if you dont need to.
        /// Some people have the confusion that maxdistance is the point the sound will fade out to, this is not the case.
        /// </para>
        /// <para>
        /// A 'distance unit' is specified by System::set3DSettings. By default this is set to meters which is a distance scale of 1.0.
        /// The default units for minimum and maximum distances are 1.0 and 10000.0f.
        /// Volume drops off at mindistance / distance.
        /// To define the min and max distance per sound and not per channel use Sound::set3DMinMaxDistance.
        /// </para>
        /// If FMOD_3D_CUSTOMROLLOFF is used, then these values are stored, but ignored in 3d processing.
        /// </remarks>
        public void Get3DMinMaxDistance( out float minimumDistance, out float maximumDistance )
        {
            RESULT result = nativeChannel.get3DMinMaxDistance(out minimumDistance, out maximumDistance );
            ThrowOnError( result );
        }

        #endregion

        /// <summary>
        /// Called by native FMOD when the native Channel reaches the end.
        /// </summary>
        /// <param name="channelControl">
        /// A pointer to the raw FMOD channel the callback is related to.
        /// </param>
        /// <param name="controlType">
        /// Identifies the type of control pointer.
        /// </param>
        /// <param name="callbackType">
        /// Identifies the type of the callback.
        /// </param>
        /// <param name="commandData1">
        /// Contains additional command data.
        /// </param>
        /// <param name="commandData2">
        /// Contains additional secondary command data.
        /// </param>
        /// <returns>
        /// The result of the callback.
        /// </returns>
        private RESULT CHANNEL_CALLBACK_END(
            IntPtr channelControl, 
            CHANNELCONTROL_TYPE controlType, 
            CHANNELCONTROL_CALLBACK_TYPE callbackType,
            IntPtr commandData1,
            IntPtr commandData2 )
        {
            if(callbackType == CHANNELCONTROL_CALLBACK_TYPE.END )
            {
                this.sound.AudioSystem.AddChannelEnd( this );
            }

            return RESULT.OK;
        }

        internal void OnEnded()
        {
            if( this.ended != null )
            {
                this.ended( this );
            }
        }

        /// <summary>
        /// Helper function that throws an <see cref="AudioException"/>
        /// if the given native <see cref="RESULT"/> is not OK.
        /// </summary>
        /// <param name="result">The result to test for.</param>
        private static void ThrowOnError( Native.RESULT result )
        {
            if( result != Native.RESULT.OK )
            {
                throw new AudioException( Native.Error.String( result ) );
            }
        }

        #endregion

        /// <summary>
        /// Holds the event handlers that will be informed once this Channel stops.
        /// </summary>
        private SimpleEventHandler<Channel> ended;

        /// <summary>
        /// The ChannelGroup this Channel is part of.
        /// </summary>
        private ChannelGroup channelGroup;

        /// <summary>
        /// The <see cref="Atom.Fmod.Native.Channel"/> object. 
        /// </summary>
        private readonly Atom.Fmod.Native.Channel nativeChannel;

        /// <summary>
        /// The <see cref="Atom.Fmod.Sound"/> the <see cref="Atom.Fmod.Channel"/> was initialy
        /// created with.
        /// </summary>
        private readonly Atom.Fmod.Sound sound;

        /// <summary>
        /// The native callback that is called when the channel stops to play.
        /// </summary>
        private CHANNELCONTROL_CALLBACK endCallback;
    }
}
