// <copyright file="SoundManager.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Fmod.SoundManager class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Fmod
{
    using System.Collections.Generic;
    using System.IO;
    using Atom.Diagnostics;

    /// <summary>
    /// Manages the creation and loading of Sound Resources.
    /// This class can't be inherited.
    /// </summary>
    public sealed class SoundManager
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the <see cref="AudioSystem"/> that owns the <see cref="SoundManager"/>.
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
        /// Initializes a new instance of the <see cref="SoundManager"/> class.
        /// </summary>
        /// <param name="audioSystem">
        /// Reference to the <see cref="AudioSystem"/> that owns the <see cref="SoundManager"/>.
        /// </param>
        internal SoundManager( AudioSystem audioSystem )
        {
            this.audioSystem = audioSystem;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Tries to get the music <see cref="Sound"/>-Resource that
        /// has the given <paramref name="name"/>.
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
        /// <param name="group">
        /// The optional group under which the sound is loaded.
        /// </param>
        /// <returns>
        /// The requested Sound resource; or null.
        /// </returns>
        public Sound Get( string name, string directoryPath, string tag = "", SoundGroup group = null )
        {
            Sound sound;
            string resourceId = GetId( name, tag );
            if( resources.TryGetValue( resourceId, out sound ) )
                return sound;

            string fullPath = Path.Combine( directoryPath, name );

            if( !File.Exists( fullPath ) )
            {
                string message = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.Error_ResourceXAtPathYDoesntExist,
                    name,
                    fullPath
                );

                if( audioSystem.ThrowExceptionOnResourceNotFound )
                {
                    throw new FileNotFoundException( message );
                }
                else
                {
                    if( audioSystem.ErrorLog != null )
                        audioSystem.ErrorLog.WriteLine( message );
                    return null;
                }
            }

            sound = new Sound( name, fullPath, group, audioSystem );
            resources.Add( resourceId, sound );

            return sound;
        }

        private static string GetId( string name, string tag )
        {
            return name + tag;
        }

        /// <summary>
        /// Tries to get the music <see cref="Sound"/>-Resource that
        /// has the given <paramref name="fullName"/>.
        /// </summary>
        /// <param name="fullName">
        /// The full name of the resource, including the directory path.
        /// </param>
        /// <returns>The requested Sound resource; or null.</returns>
        public Sound Get( string fullName )
        {
            var name = System.IO.Path.GetFileName( fullName );
            var directory = System.IO.Path.GetDirectoryName( fullName );

            return this.Get( name, directory );
        }

        /// <summary>
        /// Tries to get the music <see cref="Sound"/>-Resource that
        /// has the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="tag">
        /// The (optional) tag that is attached to the sound resource.
        /// Using different tags one can store the same sound multiple times
        /// in the same SoundManager.
        /// </param>
        /// <returns>The requested Sound resource; or null.</returns>
        public Sound GetMusic( string name, string tag = "" )
        {
            return Get( name, audioSystem.MusicDirectory, tag, audioSystem.MusicGroup );
        }

        /// <summary>
        /// Tries to get the music <see cref="Sound"/>-Resource that
        /// has the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="tag">
        /// The (optional) tag that is attached to the sound resource.
        /// Using different tags one can store the same sound multiple times
        /// in the same SoundManager.
        /// </param>
        /// <returns>The requested Sound resource; or null.</returns>
        public Sound GetSample( string name, string tag = "" )
        {
            return Get( name, audioSystem.SampleDirectory, tag, audioSystem.SampleGroup );
        }

        /// <summary>
        /// Tries to remove the Sound Resource with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the Sound to remove.</param>
        /// <param name="tag">
        /// The (optional) tag that is attached to the sound resource.
        /// Using different tags one can store the same sound multiple times
        /// in the same SoundManager.
        /// </param>
        /// <returns>
        /// Returns true if the resource was removed; otherwise false.
        /// </returns>
        public bool Remove( string name, string tag = "" )
        {
            return resources.Remove( GetId( name, tag ) );
        }

        /// <summary>
        /// Releases and removes all Sound resources that have been created
        /// by the <see cref="SoundManager"/>.
        /// </summary>
        public void ReleaseAndRemoveAll()
        {
            foreach( Sound sound in resources.Values )
                sound.Release();

            resources.Clear();
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Reference to the <see cref="AudioSystem"/> that owns the <see cref="SoundManager"/>.
        /// </summary>
        private readonly AudioSystem audioSystem;

        /// <summary>
        /// The dictionary of sound resources.
        /// </summary>
        private readonly Dictionary<string, Sound> resources = new Dictionary<string, Sound>();

        #endregion
    }
}