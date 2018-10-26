// <copyright file="NumericUpDown.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Controls.NumericUpDown class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Controls
{
    using System;
    using Atom.Diagnostics.Contracts;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Atom.Math;

    /// <summary>
    /// Represents a control that allows the user to enter a numeric value;
    /// and also control it using two up/down buttons.
    /// </summary>
    public class NumericUpDown : Control
    {
        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the NumericUpDown class.
        /// </summary>
        public NumericUpDown()
            : base()
        {
            this.UpdateValueString();
        }

        /// <summary>
        /// Initializes static members of the NumericUpDown class.
        /// </summary>
        static NumericUpDown()
        {
            InitializeCommands();

            // Listen to MouseLeftButtonDown event to determine 
            // if NumericUpDown should move focus to itself.
            EventManager.RegisterClassHandler(
                typeof( NumericUpDown ),
                Mouse.MouseDownEvent,
                new MouseButtonEventHandler( NumericUpDown.OnMouseLeftButtonDown ),
                true
            );

            DefaultStyleKeyProperty.OverrideMetadata(
                typeof( NumericUpDown ),
                new FrameworkPropertyMetadata( typeof( NumericUpDown ) )
            );
        }

        #endregion

        #region [ Events ]

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Bubble,
            typeof( RoutedPropertyChangedEventHandler<decimal> ),
            typeof( NumericUpDown )
        );

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
        {
            add 
            {
                AddHandler( ValueChangedEvent, value ); 
            }

            remove 
            {
                RemoveHandler( ValueChangedEvent, value );
            }
        }

        #endregion

        #region [ Properties ]

        #region Value

        /// <summary>
        /// Gets or sets the value displayed by this <see cref="NumericUpDown"/> control.
        /// This is a dependency property.
        /// </summary>
        /// <value>The value of the NumericUpDown control.</value>
        public decimal Value
        {
            get
            { 
                return (decimal)GetValue( ValueProperty );
            }

            set 
            {
                SetValue( ValueProperty, value ); 
            }
        }

        /// <summary>
        /// Identifies the <see cref="NumericUpDown.Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof( decimal ),
            typeof( NumericUpDown ),
            new FrameworkPropertyMetadata(
                DefaultValue,
                new PropertyChangedCallback( OnValueChanged ),
                new CoerceValueCallback( CoerceValue )
            )
        );

        /// <summary>
        /// Called when the Value value of a <see cref="NumericUpDown"/> has changed.
        /// </summary>
        /// <param name="obj">
        /// The NumericUpDown whose Value property has changed.
        /// </param>
        /// <param name="args">
        /// The DependencyPropertyChangedEventArgs that contains the event data.
        /// </param>
        private static void OnValueChanged( DependencyObject obj, DependencyPropertyChangedEventArgs args )
        {
            var control = (NumericUpDown)obj;
            decimal oldValue = (decimal)args.OldValue;
            decimal newValue = (decimal)args.NewValue;

            var peer = UIElementAutomationPeer.FromElement( control ) as NumericUpDownAutomationPeer;
            if( peer != null )
                peer.RaiseValueChangedEvent( oldValue, newValue );

            control.OnValueChanged( new RoutedPropertyChangedEventArgs<decimal>( oldValue, newValue, ValueChangedEvent ) );
            control.UpdateValueString();
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">
        /// Arguments associated with the ValueChanged event.
        /// </param>
        protected virtual void OnValueChanged( RoutedPropertyChangedEventArgs<decimal> args )
        {
            RaiseEvent( args );
        }

        /// <summary>
        /// Coerces the value of the Value property to be in valid range.
        /// </summary>
        /// <param name="element">The NumericUpDown whose Value property will be changed.</param>
        /// <param name="value">The new value of the Value property.</param>
        /// <returns>The coerced value that will be applied to the Value property.</returns>
        private static object CoerceValue( DependencyObject element, object value )
        {
            decimal newValue = (decimal)value;
            NumericUpDown control  = (NumericUpDown)element;

            newValue = newValue.Clamp( control.Minimum, control.Maximum );
            newValue = decimal.Round( newValue, control.DecimalPlaces );

            return newValue;
        }

        #endregion

        #region Minimum

        /// <summary>
        /// Gets or sets the minumum value this <see cref="NumericUpDown"/>
        /// control can have. This is a dependency property.
        /// </summary>
        /// <value>The default value is 0.</value>
        public decimal Minimum
        {
            get
            { 
                return (decimal)GetValue( MinimumProperty ); 
            }

            set
            {
                SetValue( MinimumProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="NumericUpDown.Minimum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum",
            typeof( decimal ),
            typeof( NumericUpDown ),
            new FrameworkPropertyMetadata(
                DefaultMinValue,
                new PropertyChangedCallback( OnMinimumChanged ),
                new CoerceValueCallback( CoerceMinimum )
            )
        );

        /// <summary>
        /// Called when the Minimum value of a <see cref="NumericUpDown"/> has changed.
        /// </summary>
        /// <param name="element">
        /// The NumericUpDown whose Minimum property has changed.
        /// </param>
        /// <param name="args">
        /// The DependencyPropertyChangedEventArgs that contains the event data.
        /// </param>
        private static void OnMinimumChanged( DependencyObject element, DependencyPropertyChangedEventArgs args )
        {
            element.CoerceValue( MaximumProperty );
            element.CoerceValue( ValueProperty );
        }

        /// <summary>
        /// Coerces the value of the Minimum property to be in valid range.
        /// </summary>
        /// <param name="element">The NumericUpDown whose Minimum property will be changed.</param>
        /// <param name="value">The new value of the Minimum property.</param>
        /// <returns>The coerced value that will be applied to the Minimum property.</returns>
        private static object CoerceMinimum( DependencyObject element, object value )
        {
            var control = (NumericUpDown)element;
            decimal minimum = (decimal)value;

            return Decimal.Round( minimum, control.DecimalPlaces );
        }

        #endregion

        #region Maximum

        /// <summary>
        /// Gets or sets the maximum value this <see cref="NumericUpDown"/>
        /// control can have. This is a dependency property.
        /// </summary>
        /// <value>The default value is 100.</value>
        public decimal Maximum
        {
            get
            { 
                return (decimal)GetValue( MaximumProperty ); 
            }

            set 
            { 
                SetValue( MaximumProperty, value ); 
            }
        }

        /// <summary>
        /// Identifies the <see cref="NumericUpDown.Maximum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum",
            typeof( decimal ),
            typeof( NumericUpDown ),
            new FrameworkPropertyMetadata(
                DefaultMaxValue,
                new PropertyChangedCallback( OnMaximumChanged ),
                new CoerceValueCallback( CoerceMaximum )
            )
        );

        /// <summary>
        /// Called when the Maximum value of a <see cref="NumericUpDown"/> instance has changed.
        /// </summary>
        /// <param name="element">
        /// The NumericUpDown whose Maximum property has changed.
        /// </param>
        /// <param name="args">
        /// The DependencyPropertyChangedEventArgs that contains the event data.
        /// </param>
        private static void OnMaximumChanged( DependencyObject element, DependencyPropertyChangedEventArgs args )
        {
            element.CoerceValue( ValueProperty );
        }

        /// <summary>
        /// Coerces the value of the Maximum property to be in valid range.
        /// </summary>
        /// <param name="element">The NumericUpDown whose Maximum property will be changed.</param>
        /// <param name="value">The new value of the Maximum property.</param>
        /// <returns>The coerced value that will be applied to the Maximum property.</returns>
        private static object CoerceMaximum( DependencyObject element, object value )
        {
            var control = (NumericUpDown)element;
            decimal maximum = (decimal)value;

            return Decimal.Round( System.Math.Max( maximum, control.Minimum ), control.DecimalPlaces );
        }

        #endregion

        #region Change

        /// <summary>
        /// Gets or sets the change in value that happens when the user 
        /// presses the up or down button.
        /// This is a dependency property.
        /// </summary>
        /// <value>
        /// The default value is 1.
        /// </value>
        public decimal Change
        {
            get 
            {
                return (decimal)GetValue( ChangeProperty );
            }

            set
            { 
                SetValue( ChangeProperty, value ); 
            }
        }

        /// <summary>
        /// Identifies the <see cref="NumericUpDown.Change"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChangeProperty = DependencyProperty.Register(
            "Change",
            typeof( decimal ),
            typeof( NumericUpDown ),
            new FrameworkPropertyMetadata( DefaultChange, null, new CoerceValueCallback( CoerceChange ) ),
            new ValidateValueCallback( ValidateChange )
        );

        /// <summary>
        /// Returns whether the specified value is an valid value
        /// for the Change property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>Returns true if the value is valid; otherwise false.</returns>
        private static bool ValidateChange( object value )
        {
            decimal change = (decimal)value;
            return change > 0;
        }

        /// <summary>
        /// Coerces the value of the Change property to be in valid range.
        /// </summary>
        /// <param name="element">The NumericUpDown whose Change property will be changed.</param>
        /// <param name="value">The new value of the Change property.</param>
        /// <returns>The coerced value that will be applied to the Change property.</returns>
        private static object CoerceChange( DependencyObject element, object value )
        {
            var control = (NumericUpDown)element;
            decimal newChange = (decimal)value;

            decimal coercedNewChange = Decimal.Round( newChange, control.DecimalPlaces );

            // If Change is .1 and DecimalPlaces is changed from 1 to 0, we want Change to go to 1, not 0.
            // Put another way, Change should always be rounded to DecimalPlaces, but never smaller than the 
            // previous Change
            if( coercedNewChange < newChange )
            {
                coercedNewChange = GetSmallestChangeForDecimalPlaces( control.DecimalPlaces );
            }

            return coercedNewChange;
        }

        /// <summary>
        /// Gets the smallest change given a number of decimal places.
        /// </summary>
        /// <param name="decimalPlaces">
        /// The number of decimal places.
        /// </param>
        /// <returns>
        /// The change to use.
        /// </returns>
        private static decimal GetSmallestChangeForDecimalPlaces( int decimalPlaces )
        {
            Contract.Requires<ArgumentException>( decimalPlaces >= 0 );

            decimal d = 1.0m;

            for( int i = 0; i < decimalPlaces; ++i )
            {
                d /= 10;
            }

            return d;
        }

        #endregion

        #region DecimalPlaces

        /// <summary>
        /// Gets or sets the number of decimal places
        /// displayed by this <see cref="NumericUpDown"/> control. 
        /// This is a dependency property.
        /// </summary>
        /// <value>
        /// The default value is 0.
        /// </value>
        public int DecimalPlaces
        {
            get
            { 
                return (int)GetValue( DecimalPlacesProperty );
            }

            set
            { 
                SetValue( DecimalPlacesProperty, value ); 
            }
        }

        /// <summary>
        /// Identifies the <see cref="NumericUpDown.DecimalPlaces"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(
            "DecimalPlaces",
            typeof( int ),
            typeof( NumericUpDown ),
            new FrameworkPropertyMetadata( DefaultDecimalPlaces, new PropertyChangedCallback( OnDecimalPlacesChanged ) ),
            new ValidateValueCallback( ValidateDecimalPlaces )
        );

        /// <summary>
        /// Called when the DecimalPlaces value of a <see cref="NumericUpDown"/> instance has changed.
        /// </summary>
        /// <param name="element">
        /// The NumericUpDown whose DecimalPlaces property has changed.
        /// </param>
        /// <param name="args">
        /// The DependencyPropertyChangedEventArgs that contains the event data.
        /// </param>
        private static void OnDecimalPlacesChanged( DependencyObject element, DependencyPropertyChangedEventArgs args )
        {
            var control = (NumericUpDown)element;

            control.CoerceValue( ChangeProperty );
            control.CoerceValue( MinimumProperty );
            control.CoerceValue( MaximumProperty );
            control.CoerceValue( ValueProperty );

            control.UpdateValueString();
        }

        /// <summary>
        /// Returns whether the specified value is a valid for the DecimalPlaces property.
        /// </summary>
        /// <param name="value">
        /// The value to validate.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if the given value is valid;
        /// otherwise <see langword="false"/>.
        /// </returns>
        private static bool ValidateDecimalPlaces( object value )
        {
            int decimalPlaces = (int)value;
            return decimalPlaces >= 0;
        }

        #endregion

        #region ValueString

        /// <summary>
        /// Gets the value of this <see cref="NumericUpDown"/> control as a string.
        /// This is a dependency property.
        /// </summary>
        public string ValueString
        {
            get
            {
                return (string)GetValue( ValueStringProperty );
            }
        }

        /// <summary>
        /// Identifies the <see cref="NumericUpDown.ValueString"/> dependency property key.
        /// </summary>
        private static readonly DependencyPropertyKey ValueStringPropertyKey = DependencyProperty.RegisterAttachedReadOnly( 
            "ValueString", 
            typeof( string ),
            typeof( NumericUpDown ), 
            new PropertyMetadata()
        );

        /// <summary>
        /// Identifies the <see cref="NumericUpDown.ValueString"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueStringProperty = ValueStringPropertyKey.DependencyProperty;

        /// <summary>
        /// Updates the string that is displayed by the <see cref="NumericUpDown"/> control.
        /// </summary>
        private void UpdateValueString()
        {
            this.numberFormatInfo.NumberDecimalDigits = this.DecimalPlaces;

            string newValueString = this.Value.ToString( "f", numberFormatInfo );
            this.SetValue( ValueStringPropertyKey, newValueString );
        }

        /// <summary>
        /// The number format that is used to convert the value into a string.
        /// </summary>
        private readonly NumberFormatInfo numberFormatInfo = new NumberFormatInfo();

        #endregion

        #endregion

        #region [ Commands ]

        /// <summary>
        /// Gets the Command that is used to increase the value 
        /// of a <see cref="NumericUpDown"/> instance by the value 
        /// specified by its <see cref="NumericUpDown.Change"/> property.
        /// </summary>
        public static RoutedCommand IncreaseCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Command that is used to decrease the value 
        /// of a <see cref="NumericUpDown"/> instance by the value 
        /// specified by its <see cref="NumericUpDown.Change"/> property.
        /// </summary>
        public static RoutedCommand DecreaseCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the commands used by the <see cref="NumericUpDown"/> class.
        /// </summary>
        private static void InitializeCommands()
        {
            IncreaseCommand = new RoutedCommand( "IncreaseCommand", typeof( NumericUpDown ) );
            CommandManager.RegisterClassCommandBinding( typeof( NumericUpDown ), new CommandBinding( IncreaseCommand, OnIncreaseCommand ) );
            CommandManager.RegisterClassInputBinding( typeof( NumericUpDown ), new InputBinding( IncreaseCommand, new KeyGesture( Key.Up ) ) );

            DecreaseCommand = new RoutedCommand( "DecreaseCommand", typeof( NumericUpDown ) );
            CommandManager.RegisterClassCommandBinding( typeof( NumericUpDown ), new CommandBinding( DecreaseCommand, OnDecreaseCommand ) );
            CommandManager.RegisterClassInputBinding( typeof( NumericUpDown ), new InputBinding( DecreaseCommand, new KeyGesture( Key.Down ) ) );
        }

        /// <summary>
        /// Called when the IncreaseCommand has been invoked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The ExecutedRoutedEventArgs that contains the event data.</param>
        private static void OnIncreaseCommand( object sender, ExecutedRoutedEventArgs e )
        {
            NumericUpDown control = sender as NumericUpDown;

            if( control != null )
                control.OnIncrease();
        }

        /// <summary>
        /// Called when the DecreaseCommand has been invoked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The ExecutedRoutedEventArgs that contains the event data.</param>
        private static void OnDecreaseCommand( object sender, ExecutedRoutedEventArgs e )
        {
            NumericUpDown control = sender as NumericUpDown;

            if( control != null )
                control.OnDecrease();
        }

        /// <summary>
        /// Called when the the value of this <see cref="NumericUpDown"/>
        /// is meant to be increased.
        /// </summary>
        protected virtual void OnIncrease()
        {
            this.Value += Change;
        }

        /// <summary>
        /// Called when the the value of this <see cref="NumericUpDown"/>
        /// is meant to be decreased.
        /// </summary>
        protected virtual void OnDecrease()
        {
            this.Value -= Change;
        }

        #endregion

        #region [ Automation ]

        /// <summary>
        /// Overriden to create the <see cref="AutomationPeer"/> to be used by this <see cref="NumericUpDown"/>.
        /// </summary>
        /// <returns>Alpha new instance of the <see cref="NumericUpDownAutomationPeer"/> class.</returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new NumericUpDownAutomationPeer( this );
        }

        #endregion

        #region [ Input Handling ]

        /// <summary>
        /// Called when MouseLeftButtonDown event has occured.
        /// The purpose of this handle is to move input focus to NumericUpDown when user pressed
        /// mouse left button on any part of slider that is not focusable.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The MouseButtonEventArgs that contains the event data.</param>
        private static void OnMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            NumericUpDown control = (NumericUpDown)sender;

            // When someone click on a part in the NumericUpDown and it's not focusable
            // NumericUpDown needs to take the focus in order to process keyboard correctly
            if( !control.IsKeyboardFocusWithin )
            {
                e.Handled = control.Focus() || e.Handled;
            }
        }

        #endregion

        #region [ Constants ]

        /// <summary>
        /// The default values of the NumericUpDown control.
        /// </summary>
        private const decimal DefaultMinValue = 0.0m,
                              DefaultValue    = DefaultMinValue,
                              DefaultMaxValue = 100.0m,
                              DefaultChange   = 1.0m;

        /// <summary>
        /// The default number of decimal places the NumericUpDown has.
        /// </summary>
        private const int DefaultDecimalPlaces = 0;

        #endregion
    }
}