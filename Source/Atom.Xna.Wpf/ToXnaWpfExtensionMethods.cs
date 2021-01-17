// <copyright file="ToXnaWpfExtensionMethods.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Wpf.ToXnaWpfExtensionMethods class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Wpf
{
    /// <summary>
    /// Defines extension methods that convert between Xna and WPF types.
    /// </summary>
    public static class ToXnaWpfExtensionMethods
    {
        /// <summary>
        /// Converts a WPF <see cref="System.Windows.Media.Color"/>
        /// into a Xna <see cref="Microsoft.Xna.Framework.Color"/>.
        /// </summary>
        /// <param name="color">
        /// The color to convert.
        /// </param>
        /// <returns>
        /// The converted color.
        /// </returns>
        public static Microsoft.Xna.Framework.Color ToXna( this System.Windows.Media.Color color )
        {
            return new Microsoft.Xna.Framework.Color( color.R, color.G, color.B, color.A );
        }

        /// <summary>
        /// Converts a Xna <see cref="Microsoft.Xna.Framework.Color"/>
        /// into a WPF <see cref="System.Windows.Media.Color"/>.
        /// </summary>
        /// <param name="color">
        /// The color to convert.
        /// </param>
        /// <returns>
        /// The converted color.
        /// </returns>
        public static System.Windows.Media.Color ToWpf( this Microsoft.Xna.Framework.Color color )
        {
            return System.Windows.Media.Color.FromArgb( color.A, color.R, color.G, color.B );
        }
    }
}
