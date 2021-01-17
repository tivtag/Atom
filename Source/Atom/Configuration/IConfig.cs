// <copyright file="IConfig.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Configuration.IConfig interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Configuration
{
    /// <summary>
    /// Provides a mechanism for loading and saving configurations.
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// Loads the data stored in the <see cref="IConfigStore"/> into this <see cref="Config"/>.
        /// </summary>
        void Load();

        /// <summary>
        /// Loads the default values of the config properties.
        /// </summary>
        void LoadDefaults();

        /// <summary>
        /// Saves this Config to the <see cref="IConfigStore"/>.
        /// </summary>
        void Save();
    }
}
