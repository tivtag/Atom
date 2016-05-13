namespace Atom.Xna
{
    using Atom.Math;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Defines extension methods for the <see cref="MouseState"/> class.
    /// </summary>
    public static class MouseInputExtensions
    {
        /// <summary>
        /// Gets the position of the mouse cursor as a <see cref="Point2"/>.
        /// </summary>
        /// <param name="mouseState">
        /// The state to query.
        /// </param>
        /// <returns>
        /// The position of the mouse.
        /// </returns>
        public static Point2 Position( this MouseState mouseState )
        {
            return new Point2( mouseState.X, mouseState.Y );
        }
    }
}
