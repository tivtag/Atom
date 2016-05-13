// <copyright file="ColorPicker.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Controls.ColorPicker class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Implements an HSB (hue, saturation, brightness) based color picker.
    /// </summary>
    public class ColorPicker : Control
    {
        #region [ Events ]

        /// <summary>
        /// Fired when the <see cref="Color"/> of the <see cref="ColorPicker"/> has changed.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<Color> SelectedColorChanged
        {
            add
            {
                AddHandler( SelectedColorChangedEvent, value );
            }

            remove
            {
                RemoveHandler( SelectedColorChangedEvent, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="RoutedEvent"/> for the <see cref="SelectedColorChanged"/> event.
        /// </summary>
        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedColorChanged",
            RoutingStrategy.Bubble,
            typeof( RoutedPropertyChangedEventHandler<Color> ),
            typeof( ColorPicker )
        );

        #endregion

        #region [ Properties ]

        #region > SelectedColor <

        /// <summary>
        /// Gets or sets the selected Color. This is a dependency property.
        /// </summary>
        public System.Windows.Media.Color SelectedColor
        {
            get
            {
                return (System.Windows.Media.Color)GetValue( SelectedColorProperty );
            }

            set
            {
                SetValue( SelectedColorProperty, color );
                SetColor( (Color)value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            "SelectedColor",
            typeof( System.Windows.Media.Color ),
            typeof( ColorPicker ),
            new PropertyMetadata( System.Windows.Media.Colors.Transparent, new PropertyChangedCallback( OnSelectedColorPropertyChanged ) )
        );

        /// <summary>
        /// Gets called when <see cref="SelectedColor"/> dependency property of the specified 
        /// <see cref="ColorPicker"/> <see cref="DependencyObject"/> has changed.
        /// </summary>
        /// <param name="d">The <see cref="ColorPicker"/> whose <see cref="Alpha"/> value has changed.</param>
        /// <param name="e">Stores the change.</param>
        private static void OnSelectedColorPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ColorPicker colorPicker = (ColorPicker)d;
            colorPicker.OnSelectedColorChanged( (Color)e.OldValue, (Color)e.NewValue );
        }

        #endregion

        #region > RGP <

        #region - Alpha -

        /// <summary>
        /// Gets or sets the ARGB alpha value of the selected color. This is a dependency property.
        /// </summary>
        public byte Alpha
        {
            get
            {
                return (byte)GetValue( AlphaProperty );
            }

            set
            {
                SetValue( AlphaProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="Alpha"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(
            "Alpha",
            typeof( byte ),
            typeof( ColorPicker ),
            new PropertyMetadata( (byte)255, new PropertyChangedCallback( AlphaChanged ) )
        );

        /// <summary>
        /// Gets called when <see cref="Alpha"/> dependency property of the specified 
        /// <see cref="ColorPicker"/> <see cref="DependencyObject"/> has changed.
        /// </summary>
        /// <param name="d">The <see cref="ColorPicker"/> whose <see cref="Alpha"/> value has changed.</param>
        /// <param name="e">Stores the change.</param>
        private static void AlphaChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ColorPicker colorPicker = (ColorPicker)d;
            colorPicker.OnAlphaChanged( (byte)e.NewValue );
        }

        #endregion

        #region - Red -

        /// <summary>
        /// Gets or sets the ARGB red value of the selected Color. This is a dependency property.
        /// </summary>
        public byte Red
        {
            get
            {
                return (byte)GetValue( RedProperty );
            }

            set
            {
                SetValue( RedProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="Red"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedProperty = DependencyProperty.Register(
            "Red",
            typeof( byte ),
            typeof( ColorPicker ),
            new PropertyMetadata( (byte)255, new PropertyChangedCallback( RedChanged ) )
        );

        /// <summary>
        /// Gets called when <see cref="Red"/> dependency property of the specified 
        /// <see cref="ColorPicker"/> <see cref="DependencyObject"/> has changed.
        /// </summary>
        /// <param name="d">The <see cref="ColorPicker"/> whose <see cref="Red"/> value has changed.</param>
        /// <param name="e">Stores the change.</param>
        private static void RedChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ColorPicker colorPicker = (ColorPicker)d;
            colorPicker.OnRedChanged( (byte)e.NewValue );
        }

        #endregion

        #region - Green -

        /// <summary>
        /// Gets or sets the ARGB green value of the selected Color. This is a dependency property.
        /// </summary>
        public byte Green
        {
            get
            {
                return (byte)GetValue( GreenProperty );
            }

            set
            {
                SetValue( GreenProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="Green"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register(
            "Green",
            typeof( byte ),
            typeof( ColorPicker ),
            new PropertyMetadata( (byte)255, new PropertyChangedCallback( GreenChanged ) )
        );

        /// <summary>
        /// Gets called when <see cref="Green"/> dependency property of the specified 
        /// <see cref="ColorPicker"/> <see cref="DependencyObject"/> has changed.
        /// </summary>
        /// <param name="d">The <see cref="ColorPicker"/> whose <see cref="Green"/> value has changed.</param>
        /// <param name="e">Stores the change.</param>
        private static void GreenChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ColorPicker colorPicker = (ColorPicker)d;
            colorPicker.OnGreenChanged( (byte)e.NewValue );
        }

        #endregion

        #region - Blue -

        /// <summary>
        /// Gets or sets the ARGB blue value of the selected Color. This is a dependency property.
        /// </summary>
        public byte Blue
        {
            get
            {
                return (byte)GetValue( BlueProperty );
            }

            set
            {
                SetValue( BlueProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="Blue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register(
            "Blue",
            typeof( byte ),
            typeof( ColorPicker ),
            new PropertyMetadata( (byte)255, new PropertyChangedCallback( BlueChanged ) )
        );

        /// <summary>
        /// Gets called when <see cref="Blue"/> dependency property of the specified 
        /// <see cref="ColorPicker"/> <see cref="DependencyObject"/> has changed.
        /// </summary>
        /// <param name="d">The <see cref="ColorPicker"/> whose <see cref="Blue"/> value has changed.</param>
        /// <param name="e">Stores the change.</param>
        private static void BlueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ColorPicker colorPicker = (ColorPicker)d;
            colorPicker.OnBlueChanged( (byte)e.NewValue );
        }

        #endregion

        #endregion

        #region > ScRGB <

        #region - ScAlpha -

        /// <summary>
        /// Gets or sets the ScRGB alpha value of the selected Color. This is a dependency property.
        /// </summary>
        public float ScAlpha
        {
            get
            {
                return (float)GetValue( ScAlphaProperty );
            }

            set
            {
                SetValue( ScAlphaProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="ScAlpha"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScAlphaProperty = DependencyProperty.Register(
            "ScAlpha",
            typeof( float ),
            typeof( ColorPicker ),
            new PropertyMetadata( 1.0f, new PropertyChangedCallback( ScAlphaChanged ) )
        );

        /// <summary>
        /// Gets called when <see cref="ScAlpha"/> dependency property of the specified 
        /// <see cref="ColorPicker"/> <see cref="DependencyObject"/> has changed.
        /// </summary>
        /// <param name="d">The <see cref="ColorPicker"/> whose <see cref="ScAlpha"/> value has changed.</param>
        /// <param name="e">Stores the change.</param>
        private static void ScAlphaChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ColorPicker colorPicker = (ColorPicker)d;
            colorPicker.OnScAlphaChanged( (float)e.NewValue );
        }

        #endregion

        #region - ScRed -

        /// <summary>
        /// Gets or sets the ScRGB red value of the selected Color. This is a dependency property.
        /// </summary>
        public float ScRed
        {
            get
            {
                return (float)GetValue( ScRedProperty );
            }

            set
            {
                SetValue( ScRedProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="ScRed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScRedProperty = DependencyProperty.Register(
            "ScRed",
            typeof( float ),
            typeof( ColorPicker ),
            new PropertyMetadata( 1.0f, new PropertyChangedCallback( ScRedChanged ) )
        );

        /// <summary>
        /// Gets called when <see cref="ScRed"/> dependency property of the specified 
        /// <see cref="ColorPicker"/> <see cref="DependencyObject"/> has changed.
        /// </summary>
        /// <param name="d">The <see cref="ColorPicker"/> whose <see cref="ScRed"/> value has changed.</param>
        /// <param name="e">Stores the change.</param>
        private static void ScRedChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ColorPicker colorPicker = (ColorPicker)d;
            colorPicker.OnScRedChanged( (float)e.NewValue );
        }

        #endregion

        #region - ScGreen -

        /// <summary>
        /// Gets or sets the ScRGB green value of the selected Color. This is a dependency property.
        /// </summary>
        public float ScGreen
        {
            get
            {
                return (float)GetValue( ScGreenProperty );
            }

            set
            {
                SetValue( ScGreenProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="ScGreen"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScGreenProperty = DependencyProperty.Register(
            "ScGreen",
            typeof( float ),
            typeof( ColorPicker ),
            new PropertyMetadata( 1.0f, new PropertyChangedCallback( ScGreenChanged ) )
        );

        /// <summary>
        /// Gets called when <see cref="ScGreen"/> dependency property of the specified 
        /// <see cref="ColorPicker"/> <see cref="DependencyObject"/> has changed.
        /// </summary>
        /// <param name="d">The <see cref="ColorPicker"/> whose <see cref="ScGreen"/> value has changed.</param>
        /// <param name="e">Stores the change.</param>
        private static void ScGreenChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ColorPicker colorPicker = (ColorPicker)d;
            colorPicker.OnScGreenChanged( (float)e.NewValue );
        }

        #endregion

        #region - ScBlue -

        /// <summary>
        /// Gets or sets the ScRGB blue value of the selected Color. This is a dependency property.
        /// </summary>
        public float ScBlue
        {
            get
            {
                return (float)GetValue( ScBlueProperty );
            }

            set
            {
                SetValue( ScBlueProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="ScBlue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScBlueProperty = DependencyProperty.Register(
            "ScBlue",
            typeof( float ),
            typeof( ColorPicker ),
            new PropertyMetadata( 1.0f, new PropertyChangedCallback( ScBlueChanged ) )
        );

        /// <summary>
        /// Gets called when <see cref="ScBlue"/> dependency property of the specified 
        /// <see cref="ColorPicker"/> <see cref="DependencyObject"/> has changed.
        /// </summary>
        /// <param name="d">The <see cref="ColorPicker"/> whose <see cref="ScBlue"/> value has changed.</param>
        /// <param name="e">Stores the change.</param>
        private static void ScBlueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ColorPicker colorPicker = (ColorPicker)d;
            colorPicker.OnScBlueChanged( (float)e.NewValue );
        }

        #endregion

        #endregion

        #region > String <

        /// <summary>
        /// Gets or sets the the selected Color in hexadecimal notation. This is a dependency property.
        /// </summary>
        public string HexadecimalString
        {
            get
            {
                return (string)GetValue( HexadecimalStringProperty );
            }

            set
            {
                SetValue( HexadecimalStringProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="HexadecimalString"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HexadecimalStringProperty = DependencyProperty.Register(
            "HexadecimalString",
            typeof( string ),
            typeof( ColorPicker ),
            new PropertyMetadata( "#FFFFFFFF", new PropertyChangedCallback( HexadecimalStringChanged ) )
        );

        /// <summary>
        /// Gets called when <see cref="HexadecimalString"/> dependency property of the specified 
        /// <see cref="ColorPicker"/> <see cref="DependencyObject"/> has changed.
        /// </summary>
        /// <param name="d">The <see cref="ColorPicker"/> whose <see cref="HexadecimalString"/> value has changed.</param>
        /// <param name="e">Stores the change.</param>
        private static void HexadecimalStringChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ColorPicker colorPicker = (ColorPicker)d;
            colorPicker.OnHexadecimalStringChanged( (string)e.OldValue, (string)e.NewValue );
        }

        #endregion

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        public ColorPicker()
        {
            SetValue( AlphaProperty, color.A );
            SetValue( RedProperty, color.R );
            SetValue( GreenProperty, color.G );
            SetValue( BlueProperty, color.B );
            SetValue( SelectedColorProperty, color );
        }

        /// <summary>
        /// Initializes static members of the <see cref="ColorPicker"/> class.
        /// </summary>
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof( ColorPicker ),
                new FrameworkPropertyMetadata( typeof( ColorPicker ) )
            );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Is invoked whenever application code or internal processes call <see cref="System.Windows.FrameworkElement.ApplyTemplate()"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Receive the child elements of the ControlTemplate
            colorDetail = GetTemplateChild( ColorDetailName ) as FrameworkElement;
            colorMarker = GetTemplateChild( ColorMarkerName ) as Path;
            colorSlider = GetTemplateChild( ColorSliderName ) as ColorSpectrumSlider;

            // Setup the child elements of the ControlTemplate
            colorDetail.MouseLeftButtonDown += this.OnMouseLeftButtonDown;
            colorDetail.PreviewMouseMove    += this.OnMouseMove;
            colorDetail.SizeChanged         += this.ColorDetailSizeChanged;

            colorMarker.RenderTransform = markerTransform;
            colorMarker.RenderTransformOrigin = new Point( 0.5, 0.5 );

            colorSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>( BaseColorChanged );

            // Set default value:
            this.templateApplied = true;
            this.shouldFindPoint = true;
            this.isAlphaChange = false;
            this.SelectedColor = color;
        }

        #region > Events <

        #region > SelectedColor <

        /// <summary>
        /// Gets called when the <see cref="SelectedColor"/> value of this <see cref="ColorPicker"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnSelectedColorChanged( Color oldValue, Color newValue )
        {
            var e = new RoutedPropertyChangedEventArgs<Color>( oldValue, newValue, ColorPicker.SelectedColorChangedEvent );
            RaiseEvent( e );
        }

        #endregion

        #region > RGB <

        /// <summary>
        /// Gets called when the <see cref="Alpha"/> value of this <see cref="ColorPicker"/> has changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnAlphaChanged( byte newValue )
        {
            color.A = newValue;

            SetValue( ScAlphaProperty, color.ScA );
            SetValue( SelectedColorProperty, color );
        }

        /// <summary>
        /// Gets called when the <see cref="Red"/> value of this <see cref="ColorPicker"/> has changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnRedChanged( byte newValue )
        {
            color.R = newValue;

            SetValue( ScRedProperty, color.ScR );
            SetValue( SelectedColorProperty, color );
        }

        /// <summary>
        /// Gets called when the <see cref="Green"/> value of this <see cref="ColorPicker"/> has changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnGreenChanged( byte newValue )
        {
            color.G = newValue;

            SetValue( ScGreenProperty, color.ScG );
            SetValue( SelectedColorProperty, color );
        }

        /// <summary>
        /// Gets called when the <see cref="Blue"/> value of this <see cref="ColorPicker"/> has changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnBlueChanged( byte newValue )
        {
            color.B = newValue;

            SetValue( ScBlueProperty, color.ScB );
            SetValue( SelectedColorProperty, color );
        }

        #endregion

        #region > ScRGB <

        /// <summary>
        /// Gets called when the <see cref="ScAlpha"/> value of this <see cref="ColorPicker"/> has changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnScAlphaChanged( float newValue )
        {
            isAlphaChange = true;

            if( shouldFindPoint )
            {
                color.ScA = newValue;

                SetValue( AlphaProperty, color.A );
                SetValue( SelectedColorProperty, color );
                SetValue( HexadecimalStringProperty, color.ToString() );
            }

            isAlphaChange = false;
        }

        /// <summary>
        /// Gets called when the <see cref="ScRed"/> value of this <see cref="ColorPicker"/> has changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnScRedChanged( float newValue )
        {
            if( shouldFindPoint )
            {
                color.ScR = newValue;

                SetValue( RedProperty, color.R );
                SetValue( SelectedColorProperty, color );
                SetValue( HexadecimalStringProperty, color.ToString() );
            }
        }

        /// <summary>
        /// Gets called when the <see cref="ScGreen"/> value of this <see cref="ColorPicker"/> has changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnScGreenChanged( float newValue )
        {
            if( shouldFindPoint )
            {
                color.ScG = newValue;

                SetValue( GreenProperty, color.G );
                SetValue( SelectedColorProperty, color );
                SetValue( HexadecimalStringProperty, color.ToString() );
            }
        }

        /// <summary>
        /// Gets called when the <see cref="ScBlue"/> value of this <see cref="ColorPicker"/> has changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnScBlueChanged( float newValue )
        {
            if( shouldFindPoint )
            {
                color.ScB = newValue;

                SetValue( BlueProperty, color.B );
                SetValue( SelectedColorProperty, color );
                SetValue( HexadecimalStringProperty, color.ToString() );
            }
        }

        #endregion

        #region > String <

        /// <summary>
        /// Gets called when the <see cref="HexadecimalString"/> value of this <see cref="ColorPicker"/> has changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnHexadecimalStringChanged( string oldValue, string newValue )
        {
            try
            {
                if( shouldFindPoint )
                {
                    color = (Color)ColorConverter.ConvertFromString( newValue );
                }

                SetValue( AlphaProperty, color.A );
                SetValue( RedProperty, color.R );
                SetValue( GreenProperty, color.G );
                SetValue( BlueProperty, color.B );

                if( shouldFindPoint && !isAlphaChange && templateApplied )
                {
                    UpdateMarkerPosition( color );
                }
            }
            catch( FormatException )
            {
                SetValue( HexadecimalStringProperty, oldValue );
            }
        }

        #endregion

        #region > Template Related <

        /// <summary>
        /// Called whenever the control's template changes.
        /// </summary>
        /// <param name="oldTemplate">The old template.</param>
        /// <param name="newTemplate">The new template.</param>
        protected override void OnTemplateChanged( ControlTemplate oldTemplate, ControlTemplate newTemplate )
        {
            templateApplied = false;

            if( oldTemplate != null )
            {
                colorSlider.ValueChanged        -= new RoutedPropertyChangedEventHandler<double>( BaseColorChanged );
                colorDetail.MouseLeftButtonDown -= new MouseButtonEventHandler( OnMouseLeftButtonDown );
                colorDetail.PreviewMouseMove    -= new MouseEventHandler( OnMouseMove );
                colorDetail.SizeChanged         -= new SizeChangedEventHandler( ColorDetailSizeChanged );

                colorDetail = null;
                colorMarker = null;
                colorSlider = null;
            }

            base.OnTemplateChanged( oldTemplate, newTemplate );
        }

        #endregion

        #region > Input Related <

        /// <summary>
        /// Gets called when this <see cref="ColorPicker"/> was clicked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The MouseButtonEventArgs that contains the event data.</param>
        private void OnMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            Point position = e.GetPosition( colorDetail );
            UpdateMarkerPosition( position );
        }

        /// <summary>
        /// Gets called when the mouse is over this <see cref="ColorPicker"/> and got moved.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The MouseEventArgs that contains the event data.</param>
        private void OnMouseMove( object sender, MouseEventArgs e )
        {
            if( e.LeftButton == MouseButtonState.Pressed )
            {
                Point position = e.GetPosition( colorDetail );
                UpdateMarkerPosition( position );

                Mouse.Synchronize();
            }
        }

        #endregion

        #region > Color Related <

        /// <summary>
        /// Gets called when the base color of this <see cref="ColorPicker"/> has changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The RoutedPropertyChangedEventArgs{double} that contains the event data.</param>
        private void BaseColorChanged( object sender, RoutedPropertyChangedEventArgs<double> e )
        {
            if( colorPosition != null )
            {
                DetermineColor( (Point)colorPosition );
            }
        }

        /// <summary>
        /// Gets called when the color detail element of the control has changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The SizeChangedEventArgs that contains the event data.</param>
        private void ColorDetailSizeChanged( object sender, SizeChangedEventArgs e )
        {
            if( e.PreviousSize != Size.Empty && e.PreviousSize.Width != 0 && e.PreviousSize.Height != 0 )
            {
                double widthDifference  = e.NewSize.Width / e.PreviousSize.Width;
                double heightDifference = e.NewSize.Height / e.PreviousSize.Height;

                markerTransform.X       = markerTransform.X * widthDifference;
                markerTransform.Y       = markerTransform.Y * heightDifference;
            }
            else if( colorPosition != null )
            {
                markerTransform.X = ((Point)colorPosition).X * e.NewSize.Width;
                markerTransform.Y = ((Point)colorPosition).Y * e.NewSize.Height;
            }
        }

        #endregion

        #endregion

        #region > Helpers <

        /// <summary>
        /// Private helper method that sets
        /// the color stored by the <see cref="ColorPicker"/>.
        /// </summary>
        /// <param name="newColor">
        /// The new color.
        /// </param>
        private void SetColor( Color newColor )
        {
            this.color = newColor;

            if( templateApplied )
            {
                SetValue( AlphaProperty, color.A );
                SetValue( RedProperty, color.R );
                SetValue( GreenProperty, color.G );
                SetValue( BlueProperty, color.B );

                UpdateMarkerPosition( newColor );
            }
        }

        /// <summary>
        /// Private helper method that updates the position of the marker
        /// given a <paramref name="position"/>.
        /// </summary>
        /// <param name="position">
        /// The new position.
        /// </param>
        private void UpdateMarkerPosition( Point position )
        {
            // Limit to allowed range:
            if( position.X < 0.0 )
                position.X = 0.0;
            else if( position.X > colorDetail.ActualWidth )
                position.X = colorDetail.ActualWidth;

            if( position.Y < 0.0 )
                position.Y = 0.0;
            else if( position.Y > colorDetail.ActualHeight )
                position.Y = colorDetail.ActualHeight;

            this.markerTransform.X = position.X;
            this.markerTransform.Y = position.Y;

            position.X = position.X / colorDetail.ActualWidth;
            position.Y = position.Y / colorDetail.ActualHeight;

            this.colorPosition = position;

            DetermineColor( position );
        }

        /// <summary>
        /// Private helper method that updates the position of the marker
        /// given a <see cref="Color"/>.
        /// </summary>
        /// <param name="newColor">
        /// The new color.
        /// </param>
        private void UpdateMarkerPosition( Color newColor )
        {
            this.colorPosition = null;

            HsvColor hsv   = HsvColor.FromRGB( newColor.R, newColor.G, newColor.B );
            Point position = new Point( hsv.Saturation, 1 - hsv.Value );

            this.colorPosition     = position;
            this.markerTransform.X = position.X * colorDetail.ActualWidth;
            this.markerTransform.Y = position.Y * colorDetail.ActualHeight;
        }

        /// <summary>
        /// Determines the color to set given an input <paramref name="position"/>
        /// on the color picker.
        /// </summary>
        /// <param name="position">The position.</param>
        private void DetermineColor( Point position )
        {
            HsvColor hsv;

            hsv.Hue        = 360.0 - colorSlider.Value;
            hsv.Saturation = position.X;
            hsv.Value      = 1 - position.Y;

            this.color = HsvColor.ToRGB( hsv.Hue, hsv.Saturation, hsv.Value );

            // Update fields:
            shouldFindPoint = false;
            color.ScA = (float)GetValue( ScAlphaProperty );
            SetValue( HexadecimalStringProperty, color.ToString() );
            shouldFindPoint = true;
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Stores the color stored by this <see cref="ColorPicker"/>.
        /// </summary>
        private Color color = Colors.White;

        /// <summary>
        /// Refernce to the spectrum slider of the Control Template.
        /// </summary>
        private ColorSpectrumSlider colorSlider;

        /// <summary>
        /// Refernce to the color detail view of the Control Template.
        /// </summary>
        private FrameworkElement colorDetail;

        /// <summary>
        /// Refernce to the element ofthe Control Template that marks the current color.
        /// </summary>
        private Path colorMarker;

        /// <summary>
        /// Stores the translation transformation fo the color marker.
        /// </summary>
        private TranslateTransform markerTransform = new TranslateTransform();

        /// <summary>
        /// Stores the position on the color detail.
        /// </summary>
        private Point? colorPosition;

        /// <summary>
        /// States whether this <see cref="ColorPicker"/> should find the current color given the color position.
        /// </summary>
        private bool shouldFindPoint = true;

        /// <summary>
        /// States whether a control tempalte has been applied to this <see cref="ColorPicker"/>.
        /// </summary>
        private bool templateApplied;

        /// <summary>
        /// States whether the alpha value of this <see cref="ColorPicker"/> is currently changing.
        /// </summary>
        private bool isAlphaChange;

        /// <summary>
        /// Identifies the name of the color slider control template element.
        /// </summary>
        private const string ColorSliderName = "PART_ColorSlider";

        /// <summary>
        /// Identifies the name of the color detail control template element.
        /// </summary>
        private const string ColorDetailName = "PART_ColorDetail";

        /// <summary>
        /// Identifies the name of the color marker control template element.
        /// </summary>
        private const string ColorMarkerName = "PART_ColorMarker";

        #endregion
    }
}