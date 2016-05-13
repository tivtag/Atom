
namespace Atom.Fmod
{
    using Atom.Math;
    
    /// <summary>
    /// Defines extension methods for the Sound class.
    /// </summary>
    public static class SoundExtensions
    {
        /// <summary>
        /// Plays the given 3D-Sounmd at the given position and distance.
        /// </summary>
        /// <param name="sound">
        /// The sound to play. Must be loaded as a 3D sound.
        /// </param>
        /// <param name="center">
        /// The position at which to play the sound at.
        /// </param>
        /// <param name="range">
        /// The min-max distance of the sound.
        /// </param>
        /// <returns>
        /// The channel in which the sound is playing.
        /// </returns>
        public static Channel PlayAt( this Sound sound, Vector2 center, FloatRange range )
        {
            return PlayAt( sound, center, range, volume: 1.0f );
        }

        /// <summary>
        /// Plays the given 3D-Sounmd at the given position and distance.
        /// </summary>
        /// <param name="sound">
        /// The sound to play. Must be loaded as a 3D sound.
        /// </param>
        /// <param name="center">
        /// The position at which to play the sound at.
        /// </param>
        /// <param name="range">
        /// The min-max distance of the sound.
        /// </param>
        /// <param name="volume">
        /// The volume to play the sound at.
        /// </param>
        /// <returns>
        /// The channel in which the sound is playing.
        /// </returns>
        public static Channel PlayAt( this Sound sound, Vector2 center, FloatRange range, float volume )
        {
            if( sound != null )
            {
                var channel = sound.Play( true );

                channel.Volume = volume;
                channel.Set3DAttributes( center.X, center.Y );
                channel.Set3DMinMaxDistance( range.Minimum, range.Maximum );

                channel.Unpause();
                return channel;
            }
            else
            {
                return null;
            }
        }
    }
}
