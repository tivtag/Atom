// <copyright file="ColorThumb.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Controls.ColorThumb class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;

    /// <summary>
    /// Represents a color control that can be dragged by the user.
    /// </summary>
    public class ColorThumb : Thumb
    {
        /// <summary>
        /// Initializes static members of the <see cref="ColorThumb"/> class.
        /// </summary>
        static ColorThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof( ColorThumb ),
                new FrameworkPropertyMetadata( typeof( ColorThumb ) )
            );
        }

        #region - ThumbColor -

        /// <summary>
        /// Gets or sets the color of this <see cref="ColorThumb"/>. This is a dependency property.
        /// </summary>
        public Color ThumbColor
        {
            get
            {
                return (Color)GetValue( ThumbColorProperty );
            }

            set
            {
                SetValue( ThumbColorProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="ThumbColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ThumbColorProperty = DependencyProperty.Register(
            "ThumbColor",
            typeof( Color ),
            typeof( ColorThumb ),
            new FrameworkPropertyMetadata( Colors.Transparent )
        );

        #endregion

        #region - PointerOutlineThickness -

        /// <summary>
        /// Gets or sets the thickness of the pointer outline of this <see cref="ColorThumb"/>.
        /// </summary>
        public double PointerOutlineThickness
        {
            get
            {
                return (double)GetValue( PointerOutlineThicknessProperty );
            }

            set
            {
                SetValue( PointerOutlineThicknessProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="PointerOutlineThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PointerOutlineThicknessProperty = DependencyProperty.Register(
            "PointerOutlineThickness",
            typeof( double ),
            typeof( ColorThumb ),
            new FrameworkPropertyMetadata( 1.0 )
        );

        #endregion

        #region - PointerOutlineBrush -

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> of the pointer outline.
        /// This is a dependency property.
        /// </summary>
        public Brush PointerOutlineBrush
        {
            get
            {
                return (Brush)GetValue( PointerOutlineBrushProperty );
            }

            set
            {
                SetValue( PointerOutlineBrushProperty, value );
            }
        }

        /// <summary>
        /// Identifies the <see cref="PointerOutlineBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PointerOutlineBrushProperty = DependencyProperty.Register(
            "PointerOutlineBrush",
            typeof( Brush ),
            typeof( ColorThumb ),
            new FrameworkPropertyMetadata( null )
        );

        #endregion
    }
}
