
namespace Atom.Xna
{
    /// <summary>
    /// Used to calcualte the frames per second of a XNA game.
    /// </summary>
    public sealed class FramesPerSecond : IUpdateable
    {
        /// <summary>
        /// Gets the current frames per seconds.
        /// </summary>
        public int Value
        {
            get
            {
                return fps;
            }
        }

        /// <summary>
        /// Updates this FramesPerSecond calculation. Must be called once per frame.
        /// </summary>
        /// <param name="updateContext">
        /// The current update context.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            elapsedTime += updateContext.FrameTime;
            ++totalFrames;

            if( elapsedTime >= 1.0f )
            {
                fps = totalFrames;
                totalFrames = 0;
                elapsedTime = 0;
            }
        }

        private int fps;
        private int totalFrames;
        private float elapsedTime;
    }
}
