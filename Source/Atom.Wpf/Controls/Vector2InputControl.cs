// <copyright file="Vector2InputControl.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Controls.Vector2InputControl class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using Atom.Math;

    /// <summary>
    /// Represents a control that allows the editing of a <see cref="Vector2"/> value.
    /// </summary>
    public class Vector2InputControl : Control
    {
        /// <summary>
        /// Initializes static members of the <see cref="Vector2InputControl"/> class.
        /// </summary>
        static Vector2InputControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof( Vector2InputControl ),
                new FrameworkPropertyMetadata( typeof( Vector2InputControl ) )
            );
        }

        #region - Value -

        /// <summary>
        /// Gets or sets the <see cref="Vector2"/> value displayed by this <see cref="Vector2InputControl"/>.
        /// This is a dependency property.
        /// </summary>
        /// <value>The Vector2 value being edited in this Vector2InputControl.</value>
        public Vector2 Value
        {
            get 
            {
                return (Vector2)GetValue( ValueProperty );
            }

            set 
            {
                SetValue( ValueProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="Vector2InputControl.Value"/> property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof( Vector2 ),
            typeof( Vector2InputControl ),
            new FrameworkPropertyMetadata( Vector2.Zero, new PropertyChangedCallback( OnValueChanged ) )
        );

        /// <summary>
        /// Gets called when the <see cref="Vector2InputControl.Value"/> property has changed.
        /// </summary>
        /// <param name="d">The RectangleInputControl that has been changed.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var control = (Vector2InputControl)d;
            Vector2 newValue = (Vector2)e.NewValue;

            control.X = newValue.X;
            control.Y = newValue.Y;
        }

        #endregion

        #region - X -

        /// <summary>
        /// Gets or sets the x-component of the <see cref="Vector2"/> value displayed by this <see cref="Vector2InputControl"/>.
        /// This is a dependency property.
        /// </summary>
        /// <value>The value on the x-axis of the Vector2 value being edited in this Vector2InputControl.</value>
        public float X
        {
            get
            {
                return (float)GetValue( XProperty );
            }

            set
            {
                SetValue( XProperty, value ); 
            }
        }

        /// <summary>
        /// Identifies the <see cref="Vector2InputControl.X"/> property.
        /// </summary>
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X",
            typeof( float ),
            typeof( Vector2InputControl ),
            new FrameworkPropertyMetadata( 0.0f, new PropertyChangedCallback( OnXChanged ) )
        );

        /// <summary>
        /// Gets called when the <see cref="Vector2InputControl.Value"/> property has changed.
        /// </summary>
        /// <param name="d">The RectangleInputControl that has been changed.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnXChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var control = (Vector2InputControl)d;
            float newX = (float)e.NewValue;

            if( control.Value.X != newX )
                control.Value = new Vector2( newX, control.Y );
        }

        #endregion

        #region - Y -

        /// <summary>
        /// Gets or sets the y-component of the <see cref="Vector2"/> value displayed by this <see cref="Vector2InputControl"/>.
        /// This is a dependency property.
        /// </summary>
        /// <value>The value on the y-axis of the Vector2 value being edited in this Vector2InputControl.</value>
        public float Y
        {
            get
            { 
                return (float)GetValue( YProperty );
            }

            set
            { 
                SetValue( YProperty, value ); 
            }
        }

        /// <summary>
        /// Identifies the <see cref="Vector2InputControl.Y"/> property.
        /// </summary>
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y",
            typeof( float ),
            typeof( Vector2InputControl ),
            new FrameworkPropertyMetadata( 0.0f, new PropertyChangedCallback( OnYChanged ) )
        );

        /// <summary>
        /// Gets called when the <see cref="Vector2InputControl.Value"/> property has changed.
        /// </summary>
        /// <param name="d">The RectangleInputControl that has been changed.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnYChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var control = (Vector2InputControl)d;
            float newY = (float)e.NewValue;

            if( control.Value.Y != newY )
                control.Value = new Vector2( control.X, newY );
        }

        #endregion
    }
}
