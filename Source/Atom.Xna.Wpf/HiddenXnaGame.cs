// <copyright file="HiddenXnaGame.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Wpf.HiddenXnaGame class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Xna.Wpf
{
    using Microsoft.Xna.Framework;
    using WinForms = System.Windows.Forms;

    /// <summary>
    /// Extends the Xna <see cref="Game"/> class to provide support for running
    /// Xna silently, without a window, side-by-side with a WPF application.
    /// </summary>
    public abstract class HiddenXnaGame : Game
    {
        /// <summary>
        /// Gets the <see cref="GraphicsDeviceManager"/> this HiddenXnaGame uses.
        /// </summary>
        public GraphicsDeviceManager Graphics
        {
            get
            {
                return this.graphics;
            }
        }

        /// <summary>
        /// Initializes a new instance of the HiddenXnaGame class.
        /// </summary>
        protected HiddenXnaGame()
        {
            this.graphics = new GraphicsDeviceManager( this );
            this.graphics.PreparingDeviceSettings += this.OnGraphicsPreparingDeviceSettings;
        }

        /// <summary>
        /// Called when the Xna framework is preparing the GraphicsDevice.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="PreparingDeviceSettingsEventArgs"/> that contain the event data.
        /// </param>
        private void OnGraphicsPreparingDeviceSettings( object sender, PreparingDeviceSettingsEventArgs e )
        {
            var presentParams = e.GraphicsDeviceInformation.PresentationParameters;
            presentParams.BackBufferWidth = 1;
            presentParams.BackBufferHeight = 1;

            this.OnPreparingDeviceSettings( e );
        }

        /// <summary>
        /// Called when the Xna framework is preparing the GraphicsDevice.
        /// </summary>
        /// <param name="e">
        /// The <see cref="PreparingDeviceSettingsEventArgs"/> that contain the event data.
        /// </param>
        protected virtual void OnPreparingDeviceSettings( PreparingDeviceSettingsEventArgs e )
        {
        }

        /// <summary>
        /// Initializes this <see cref="HiddenXnaGame"/>.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            var gameForm = (WinForms.Form)WinForms.Form.FromHandle( this.Window.Handle );

            gameForm.VisibleChanged += (sender, e) =>  {
                var xnaForm = (WinForms.Control)sender;
                xnaForm.Visible = false;
            };
        }

        /// <summary>
        /// The Xna GraphicsDeviceManager object.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;
    }
}
