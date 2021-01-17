// <copyright file="XnaWpfGame.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Xna.Wpf.XnaWpfGame class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Xna.Wpf
{
    using System;
    using Atom.Math;
    using Microsoft.Xna.Framework;
    using WinForms = System.Windows.Forms;

    /// <summary>
    /// Extends the Xna <see cref="Game"/> class to provide support for running
    /// side-by-side with a WPF application.
    /// </summary>
    public abstract class XnaWpfGame : Game
    {
        /// <summary>
        /// Gets the size of the window/control xna is drawing.
        /// </summary>
        /// <remarks>
        /// This value can't change because of limitation of the Xna framework.
        /// </remarks>
        public Point2 WindowSize { get; }

        /// <summary>
        /// Gets the <see cref="GraphicsDeviceManager"/> of this WpfGame.
        /// </summary>
        public GraphicsDeviceManager Graphics { get; }

        /// <summary>
        /// Initializes a new instance of the XnaWpfGame class.
        /// </summary>
        /// <param name="windowSize">
        /// The static size of the window/control that Xna draws into.
        /// </param>
        /// <param name="controlHandle">
        /// The handle of the Control into which Xna is drawing.
        /// </param>
        protected XnaWpfGame( Point2 windowSize, IntPtr controlHandle )
        {
            this.WindowSize = windowSize;
            this.controlHandle = controlHandle;

            this.Graphics = new GraphicsDeviceManager( this );
            this.Graphics.PreparingDeviceSettings += this.OnGraphicsPreparingDeviceSettings;
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
            Microsoft.Xna.Framework.Graphics.PresentationParameters presentParams = e.GraphicsDeviceInformation.PresentationParameters;
            presentParams.BackBufferWidth = this.WindowSize.X;
            presentParams.BackBufferHeight = this.WindowSize.Y;
            presentParams.DeviceWindowHandle = this.controlHandle;

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
        /// Initializes this <see cref="XnaWpfGame"/>.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            var gameForm = (WinForms.Form)WinForms.Form.FromHandle( this.Window.Handle );

            gameForm.VisibleChanged += (sender, e) => {
                var xnaForm = (WinForms.Control)sender;
                xnaForm.Visible = false;
            };
        }

        /// <summary>
        /// The handle of the Control into which Xna is drawing.
        /// </summary>
        private readonly IntPtr controlHandle;
    }
}
