// <copyright file="NumericUpDownAutomationPeer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Controls.NumericUpDownAutomationPeer class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf.Controls
{
    using System;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;

    /// <summary>
    /// Exposes the <see cref="NumericUpDown"/> control to UI Automation.
    /// </summary>
    public class NumericUpDownAutomationPeer : FrameworkElementAutomationPeer, IRangeValueProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDownAutomationPeer"/> class.
        /// </summary>
        /// <param name="control">
        /// The <see cref="NumericUpDown"/> associated with this <see cref="NumericUpDownAutomationPeer"/>.
        /// </param>
        public NumericUpDownAutomationPeer( NumericUpDown control )
            : base( control )
        {
        }

        /// <summary>
        /// Gets the control pattern for the <see cref="System.Windows.UIElement"/> that is associated
        /// with this <see cref="System.Windows.Automation.Peers.UIElementAutomationPeer"/>.
        /// </summary>
        /// <param name="patternInterface">
        /// Alpha value from the enumeration.
        /// </param>
        /// <returns> Returns this -or- null. </returns>
        public override object GetPattern( PatternInterface patternInterface )
        {
            if( patternInterface == PatternInterface.RangeValue )
            {
                return this;
            }

            return base.GetPattern( patternInterface );
        }

        /// <summary>
        /// Gets the name of the <see cref="System.Windows.UIElement"/> that is associated with this
        /// <see cref="System.Windows.Automation.Peers.UIElementAutomationPeer"/>.
        /// This method is called by System.Windows.Automation.Peers.AutomationPeer.GetClassName().
        /// </summary>
        /// <returns>
        /// Returns the string "NumericUpDown".
        /// </returns>
        protected override string GetClassNameCore()
        {
            return "NumericUpDown";
        }

        /// <summary>
        /// Gets the control type for the <see cref="System.Windows.UIElement"/> that is associated
        /// with this <see cref="System.Windows.Automation.Peers.UIElementAutomationPeer"/>. This method
        /// is called by System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType().
        /// </summary>
        /// <returns>
        /// Returns <see cref="AutomationControlType.Spinner"/>.
        /// </returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Spinner;
        }

        /// <summary>
        /// Raises the value changed event.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        internal void RaiseValueChangedEvent( decimal oldValue, decimal newValue )
        {
            this.RaisePropertyChangedEvent(
                RangeValuePatternIdentifiers.ValueProperty,
                (double)oldValue,
                (double)newValue
            );
        }

        /// <summary>
        /// Gets the owner of this <see cref="NumericUpDownAutomationPeer"/> instance.
        /// </summary>
        protected new NumericUpDown Owner
        {
            get
            {
                return (NumericUpDown)base.Owner;
            }
        }

        #region [ IRangeValueProvider Members ]

        /// <summary>
        /// Gets a value indicating whether the value of a control is read-only.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if the value is read-only; 
        /// otherwise <see langword="false"/> if it can be modified.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return !this.IsEnabled();
            }
        }

        /// <summary>
        /// Gets the value that is added to or subtracted from the System.Windows.Automation.Provider.IRangeValueProvider.Value
        /// property when a large change is made, such as with the PAGE DOWN key.
        /// </summary>
        public double LargeChange
        {
            get
            {
                return (double)this.Owner.Change;
            }
        }

        /// <summary>
        /// Gets the maximum range value supported by the control.
        /// </summary>
        public double Maximum
        {
            get
            {
                return (double)this.Owner.Maximum;
            }
        }

        /// <summary>
        /// Gets the minimum range value supported by the control.
        /// </summary>
        public double Minimum
        {
            get
            {
                return (double)this.Owner.Minimum;
            }
        }

        /// <summary>
        /// Gets the value that is added to or subtracted from the System.Windows.Automation.Provider.IRangeValueProvider.Value
        /// property when a small change is made, such as with an arrow key.
        /// </summary>
        public double SmallChange
        {
            get
            {
                return (double)this.Owner.Change;
            }
        }

        /// <summary>
        /// Gets the value of the control.
        /// </summary>
        public double Value
        {
            get
            {
                return (double)this.Owner.Value;
            }
        }

        /// <summary>
        /// Sets the value of the control.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <exception cref="ElementNotEnabledException">If the control is not enabled.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// When value is less than the minimum or greater than the maximum value of the control.
        /// </exception>
        public void SetValue( double value )
        {
            if( !this.IsEnabled() )
            {
                throw new ElementNotEnabledException();
            }

            decimal val = (decimal)value;
            if( val < Owner.Minimum || val > Owner.Maximum )
                throw new ArgumentOutOfRangeException( "value" );

            Owner.Value = val;
        }

        #endregion
    }
}
