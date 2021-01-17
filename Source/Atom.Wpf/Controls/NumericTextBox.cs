// <copyright file="NumericTextBox.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Controls.NumericTextBox class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Represents a <see cref="TextBox"/> that allows the user to enter only numeric values.
    /// </summary>
    public class NumericTextBox : TextBox
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NumericTextBox"/>
        /// allows the '-' sign to be entered.
        /// This is a dependency property.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool AllowsNegativeSign
        {
            get { return (bool)GetValue( AllowsNegativeSignProperty ); }
            set { SetValue( AllowsNegativeSignProperty, value ); }
        }

        /// <summary>
        /// Identifies the <see cref="AllowsNegativeSign"/> property.
        /// </summary>
        public static readonly DependencyProperty AllowsNegativeSignProperty = 
            DependencyProperty.Register(
                "AllowsNegativeSign",
                typeof( bool ),
                typeof( NumericTextBox ),
                new UIPropertyMetadata( true )
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NumericTextBox"/> allows 
        /// decimal numbers to be entered.
        /// This is a dependency property.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool AllowsDecimalNumbers
        {
            get { return (bool)GetValue( AllowsDecimalNumbersProperty ); }
            set { SetValue( AllowsDecimalNumbersProperty, value ); }
        }

        /// <summary>
        /// Identifies the <see cref="AllowsDecimalNumbers"/> property.
        /// </summary>
        public static readonly DependencyProperty AllowsDecimalNumbersProperty = 
            DependencyProperty.Register(
                "AllowsDecimalNumbers",
                typeof( bool ),
                typeof( NumericTextBox ),
                new UIPropertyMetadata( true )
            );

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericTextBox"/> class.
        /// </summary>
        public NumericTextBox()
        {
            this.AllowDrop = false;

            // Cancel the paste and cut command:
            CommandBindings.Add( new CommandBinding( ApplicationCommands.Paste, null, CancelCommand ) );
            CommandBindings.Add( new CommandBinding( ApplicationCommands.Cut, null, CancelCommand ) );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Helper method for the OnPreviewKeyDown method
        /// that returns whether the given Key is valid.
        /// </summary>
        /// <param name="key">The key to validate.</param>
        /// <returns>Returns true if the given Key is valid; otherwise false.</returns>
        private bool IsValidKey( Key key )
        {
            if( key >= Key.D0 && key <= Key.D9 )
                return true;

            switch( key )
            {
                case Key.OemMinus:
                    return this.AllowsNegativeSign;

                case Key.Separator:
                    return this.AllowsDecimalNumbers;
            
                case Key.Subtract:
                case Key.Delete:
                case Key.Tab:
                case Key.Enter:
                case Key.Back:
                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Helper method for the OnPreviewTextInput method
        /// that returns whether the given string is valid.
        /// </summary>
        /// <param name="str">The string to validate.</param>
        /// <returns>Returns true if the given string is valid; otherwise false.</returns>
        private bool IsValidString( string str )
        {
            for( int i = 0; i < str.Length; ++i )
            {
                char ch = str[i];

                switch( ch )
                {
                    case ' ':
                        return false;

                    case '-':
                        if( !(i == 0 && this.AllowsNegativeSign) )
                            return false;

                        break;

                    default:
                        if( !Char.IsDigit( ch ) )
                            return false;
                        break;
                }
            }
            
            var numberFormat = NumberFormatInfo.CurrentInfo;

            if( str == numberFormat.NegativeSign )
                return this.AllowsNegativeSign;

            if( str == numberFormat.NumberDecimalSeparator || 
                str == numberFormat.CurrencyDecimalSeparator ||                
                str == numberFormat.PercentDecimalSeparator )
            {
                return this.AllowsDecimalNumbers;
            }

            if( str == numberFormat.CurrencyGroupSeparator   |
                str == numberFormat.CurrencySymbol           |
                str == numberFormat.NegativeInfinitySymbol   |
                str == numberFormat.NumberGroupSeparator     |
                str == numberFormat.PercentGroupSeparator    |
                str == numberFormat.PercentSymbol            |
                str == numberFormat.PerMilleSymbol           |
                str == numberFormat.PositiveInfinitySymbol   |
                str == numberFormat.PositiveSign )
            {
                return true;
            }

            return true;
        }

        #region > Events <

        /// <summary>
        /// Overwritten to update the source Text binding when the user presses Enter. 
        /// </summary>
        /// <param name="e">The System.Windows.Input.KeyEventArgs that contains the event data.</param>
        protected override void OnKeyDown( KeyEventArgs e )
        {
            if( e.Key == Key.Enter )
            {
                var be = this.GetBindingExpression( TextBox.TextProperty );
                if( be != null )
                    be.UpdateSource();
            }

            base.OnKeyDown( e );
        }

        /// <summary>
        /// Overwritten to only allow valid keys to fire the KeyDown event.
        /// </summary>
        /// <param name="e">The System.Windows.Input.KeyEventArgs that contains the event data.</param>
        protected override void OnPreviewKeyDown( KeyEventArgs e )
        {
            e.Handled = !IsValidKey( e.Key );
            base.OnPreviewKeyDown( e );
        }

        /// <summary>
        /// Overwritten to only allow valid text to be inputed.
        /// </summary>
        /// <param name="e">The System.Windows.Input.TextCompositionEventArgs that contains the event data.</param>
        protected override void OnPreviewTextInput( System.Windows.Input.TextCompositionEventArgs e )
        {
            e.Handled = !IsValidString( e.Text );
            base.OnPreviewTextInput( e );
        }

        /// <summary>
        /// Cancels the event that fired this method.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The System.Windows.Input.CanExecuteRoutedEventArgs that contains the event data.</param>
        private static void CancelCommand( object sender, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        #endregion

        #endregion
    }
}
