// <copyright file="ColorSpectrumSlider.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Controls.ColorSpectrumSlider class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    
    /// <summary>
    /// Represents a <see cref="Slider"/> that can be used to set a <see cref="Color"/>.
    /// </summary>
    public class ColorSpectrumSlider : Slider
    {
        #region [ Properties ]

        /// <summary>
        /// Gets or sets the color selected by this <see cref="ColorSpectrumSlider"/>. 
        /// This is a dependency property.
        /// </summary>
        /// <value>The value that is selected in this ColorSpectrumSlider.</value>
        public Color SelectedColor
        {
            get
            {
                return (Color)GetValue( SelectedColorProperty );
            }

            set
            {
                SetValue( SelectedColorProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                "SelectedColor",
                typeof( Color ),
                typeof( ColorSpectrumSlider ),
                new PropertyMetadata( Colors.Transparent )
            );

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes static members of the <see cref="ColorSpectrumSlider"/> class.
        /// </summary>
        static ColorSpectrumSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof( ColorSpectrumSlider ),
                new FrameworkPropertyMetadata( typeof( ColorSpectrumSlider ) )
            );
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Builds the visual tree for the <see cref="ColorSpectrumSlider"/> control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.spectrumDisplay = GetTemplateChild( SpectrumDisplayName ) as Rectangle;
            this.UpdateColorSpectrum();
            this.OnValueChanged( Double.NaN, Value );
        }

        /// <summary>
        /// Updates the current position of the <see cref="ColorSpectrumSlider"/> when the
        /// <see cref="System.Windows.Controls.Primitives.RangeBase.Value"/> property changes.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnValueChanged( double oldValue, double newValue )
        {
            base.OnValueChanged( oldValue, newValue );

            SetValue( SelectedColorProperty, HsvColor.ToRGB( 360.0 - newValue, 1.0, 1.0 ) );
        }

        /// <summary>
        /// Updates the color spectrum of this <see cref="ColorSpectrumSlider"/>.
        /// </summary>
        private void UpdateColorSpectrum()
        {
            if( spectrumDisplay != null )
                CreateSpectrum();
        }

        /// <summary>
        /// Creates the spectrum this <see cref="ColorSpectrumSlider"/> can represent.
        /// </summary>
        private void CreateSpectrum()
        {
            pickerBrush            = new LinearGradientBrush();
            pickerBrush.StartPoint = new Point( 0.5, 0 );
            pickerBrush.EndPoint   = new Point( 0.5, 1 );
            pickerBrush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation;

            List<Color> colorsList = HsvColor.GenerateSpectrum();
            double stopIncrement   = 1.0 / (double)colorsList.Count;

            int i;
            for( i = 0; i < colorsList.Count; ++i )
                pickerBrush.GradientStops.Add( new GradientStop( colorsList[i], i * stopIncrement ) );
            pickerBrush.GradientStops[i - 1].Offset = 1.0;

            spectrumDisplay.Fill = pickerBrush;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The shape object that represents the spectrum.
        /// </summary>
        private Rectangle spectrumDisplay;

        /// <summary>
        /// The brush that is used to draw the picker.
        /// </summary>
        private LinearGradientBrush pickerBrush;

        /// <summary>
        /// Identifies the spectrum display of the control template.
        /// </summary>
        private const string SpectrumDisplayName = "PART_SpectrumDisplay";

        #endregion
    }
}
