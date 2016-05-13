// <copyright file="RectangleInputControl.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Controls.RectangleInputControl class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Atom.Math;

    /// <summary>
    /// Defines an control that allows the editing of
    /// a <see cref="Rectangle"/> object.
    /// </summary>
    public class RectangleInputControl : Control
    {
        /// <summary>
        /// Initializes static members of the <see cref="RectangleInputControl"/> class.
        /// </summary>
        static RectangleInputControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof( RectangleInputControl ),
                new FrameworkPropertyMetadata( typeof( RectangleInputControl ) )
            );
        }

        #region - Value -

        /// <summary>
        /// Gets or sets the <see cref="Rectangle"/> value displayed by this <see cref="RectangleInputControl"/>.
        /// This is a dependency property.
        /// </summary>
        /// <value>The Rectangle value being edited in this RectangleInputControl.</value>
        public Rectangle Value
        {
            get 
            { 
                return (Rectangle)GetValue( ValueProperty );
            }

            set
            { 
                SetValue( ValueProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="RectangleInputControl.Value"/> property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof( Rectangle ),
            typeof( RectangleInputControl ),
            new FrameworkPropertyMetadata( new Rectangle(), new PropertyChangedCallback( OnValueChanged ) )
        );

        /// <summary>
        /// Gets called when the <see cref="RectangleInputControl.Value"/> property has changed.
        /// </summary>
        /// <param name="d">The RectangleInputControl that has been changed.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var control = (RectangleInputControl)d;
            Rectangle newValue = (Rectangle)e.NewValue;

            control.RectangleX      = newValue.X;
            control.RectangleY      = newValue.Y;
            control.RectangleWidth  = newValue.Width;
            control.RectangleHeight = newValue.Height;
        }

        #endregion

        #region - RectangleX -

        /// <summary>
        /// Gets or sets the x-component of the <see cref="Rectangle"/> value displayed by this <see cref="RectangleInputControl"/>.
        /// This is a dependency property.
        /// </summary>
        /// <value>The position on the x-axis of the Rectangle value being edited in this RectangleInputControl.</value>
        public int RectangleX
        {
            get 
            { 
                return (int)GetValue( RectangleXProperty );
            }

            set
            { 
                SetValue( RectangleXProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="RectangleInputControl.RectangleX"/> property.
        /// </summary>
        public static readonly DependencyProperty RectangleXProperty = DependencyProperty.Register(
            "RectangleX",
            typeof( int ),
            typeof( RectangleInputControl ),
            new FrameworkPropertyMetadata( 0, new PropertyChangedCallback( OnRectangleXChanged ) )
        );

        /// <summary>
        /// Gets called when the <see cref="RectangleInputControl.RectangleX"/> property has changed.
        /// </summary>
        /// <param name="d">The RectangleInputControl that has been changed.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnRectangleXChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var control = (RectangleInputControl)d;
            Rectangle oldValue = control.Value;
            int newRectangleX = (int)e.NewValue;

            if( oldValue.X != newRectangleX )
                control.Value = new Rectangle( newRectangleX, oldValue.Y, oldValue.Width, oldValue.Height );
        }

        #endregion

        #region - RectangleY -

        /// <summary>
        /// Gets or sets the y-component of the <see cref="Rectangle"/> value displayed by this <see cref="RectangleInputControl"/>.
        /// This is a dependency property.
        /// </summary>
        /// <value>The position on the y-axis of the Rectangle value being edited in this RectangleInputControl.</value>
        public int RectangleY
        {
            get 
            {
                return (int)GetValue( RectangleYProperty );
            }

            set 
            {
                SetValue( RectangleYProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="RectangleY"/> property.
        /// </summary>
        public static readonly DependencyProperty RectangleYProperty = DependencyProperty.Register(
            "RectangleY",
            typeof( int ),
            typeof( RectangleInputControl ),
            new FrameworkPropertyMetadata( 0, new PropertyChangedCallback( OnRectangleYChanged ) )
        );

        /// <summary>
        /// Gets called when the <see cref="RectangleInputControl.RectangleY"/> property has changed.
        /// </summary>
        /// <param name="d">The RectangleInputControl that has been changed.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnRectangleYChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var control = (RectangleInputControl)d;
            Rectangle oldValue = control.Value;
            int newRectangleY = (int)e.NewValue;

            if( oldValue.Y != newRectangleY )
                control.Value = new Rectangle( oldValue.X, newRectangleY, oldValue.Width, oldValue.Height );
        }

        #endregion

        #region - RectangleWidth -

        /// <summary>
        /// Gets or sets the width-component of the <see cref="Rectangle"/> value displayed by this <see cref="RectangleInputControl"/>.
        /// This is a dependency property.
        /// </summary>
        /// <value>The width of the Rectangle value being edited in this RectangleInputControl.</value>
        public int RectangleWidth
        {
            get 
            { 
                return (int)GetValue( RectangleWidthProperty );
            }

            set 
            {
                SetValue( RectangleWidthProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="RectangleInputControl.RectangleWidth"/> property.
        /// </summary>
        public static readonly DependencyProperty RectangleWidthProperty = DependencyProperty.Register(
            "RectangleWidth",
            typeof( int ),
            typeof( RectangleInputControl ),
            new FrameworkPropertyMetadata( 0, new PropertyChangedCallback( OnRectangleWidthChanged ) )
        );

        /// <summary>
        /// Gets called when the <see cref="RectangleInputControl.RectangleWidth"/> property has changed.
        /// </summary>
        /// <param name="d">The RectangleInputControl that has been changed.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnRectangleWidthChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var control = (RectangleInputControl)d;
            Rectangle oldValue = control.Value;
            int newRectangleWidth = (int)e.NewValue;

            if( oldValue.Width != newRectangleWidth )
                control.Value = new Rectangle( oldValue.X, oldValue.Y, newRectangleWidth, oldValue.Height );
        }

        #endregion

        #region - RectangleHeight -

        /// <summary>
        /// Gets or sets the y-component of the <see cref="Rectangle"/> value displayed by this <see cref="RectangleInputControl"/>.
        /// This is a dependency property.
        /// </summary>
        /// <value>The height of the Rectangle value being edited in this RectangleInputControl.</value>
        public int RectangleHeight
        {
            get
            {
                return (int)GetValue( RectangleHeightProperty ); 
            }

            set
            { 
                SetValue( RectangleHeightProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="RectangleInputControl.RectangleHeight"/> property.
        /// </summary>
        public static readonly DependencyProperty RectangleHeightProperty = DependencyProperty.Register(
            "RectangleHeight",
            typeof( int ),
            typeof( RectangleInputControl ),
            new FrameworkPropertyMetadata( 0, new PropertyChangedCallback( OnRectangleHeightChanged ) )
        );

        /// <summary>
        /// Gets called when the <see cref="RectangleInputControl.RectangleHeight"/> property has changed.
        /// </summary>
        /// <param name="d">The RectangleInputControl that has been changed.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnRectangleHeightChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var control = (RectangleInputControl)d;
            Rectangle oldValue = control.Value;
            int newRectangleHeight = (int)e.NewValue;

            if( oldValue.Height != newRectangleHeight )
                control.Value = new Rectangle( oldValue.X, oldValue.Y, oldValue.Width, newRectangleHeight );
        }

        #endregion
    }
}
