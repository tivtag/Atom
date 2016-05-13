// <copyright file="HsvColor.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.HsvColor structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf
{
    using System.Collections.Generic;
    using System.Windows.Media;

    /// <summary>
    /// Describes a color in terms of Hue, Saturation
    /// and Value (brightness).
    /// </summary>
    internal struct HsvColor
    {
        /// <summary>
        /// The hue of the color.
        /// </summary>
        public double Hue;

        /// <summary>
        /// The saturation of the color.
        /// </summary>
        public double Saturation;

        /// <summary>
        /// The value (brightness) of the color.
        /// </summary>
        public double Value;

        /// <summary>
        /// Generates a list of colors with hues ranging from 0 360
        /// and a saturation and value of 1. 
        /// </summary>
        /// <returns>
        /// The generated color spectrum.
        /// </returns> 
        public static List<Color> GenerateSpectrum()
        {
            List<Color> colorsList = new List<Color>( 30 );

            for( int i = 0; i < 29; ++i )
            {
                colorsList.Add(
                    ToRGB( i * 12, 1, 1 )
                );
            }

            colorsList.Add( ToRGB( 0, 1, 1 ) );

            return colorsList;
        }

        /// <summary>
        /// Converts the given RGB color into a <see cref="HsvColor"/>.
        /// </summary>
        /// <param name="red">The red value of the input RGB color.</param>
        /// <param name="green">The green value of the input RGB color.</param>
        /// <param name="blue">The blue value of the input RGB color.</param>
        /// <returns>
        /// The converted <see cref="HsvColor"/> color.
        /// </returns>
        public static HsvColor FromRGB( int red, int green, int blue )
        {
            double min   = System.Math.Min( System.Math.Min( red, green ), blue );
            double value = System.Math.Max( System.Math.Max( red, green ), blue );
            double delta = value - min;

            double hue = 0, saturation;

            if( value == 0.0 )
                saturation = 0.0;
            else
                saturation = delta / value;

            if( saturation == 0 )
            {
                hue = 0.0;
            }
            else
            {
                if( red == value )
                    hue = (blue - green) / delta;
                else if( blue == value )
                    hue = 2.0 + ((green - red) / delta);
                else if( green == value )
                    hue = 4.0 + ((red - blue) / delta);

                hue *= 60.0;
                if( hue < 0.0 )
                    hue = hue + 360.0;
            }

            HsvColor hsvColor;

            hsvColor.Hue        = hue;
            hsvColor.Saturation = saturation;
            hsvColor.Value      = value / 255;

            return hsvColor;
        }

        /// <summary>
        /// Converts the given HSV color into a RGB <see cref="Color"/>.
        /// </summary>
        /// <param name="hue">The hue value of the input HSV color.</param>
        /// <param name="saturation">The saturation value of the input HSV color.</param>
        /// <param name="value">The brightness value of the input HSV color.</param>
        /// <returns>
        /// The converted RGB <see cref="Color"/> color.
        /// </returns>
        public static Color ToRGB( double hue, double saturation, double value )
        {
            double red = 0.0, green = 0.0, blue = 0.0;

            if( saturation == 0 )
            {
                red   = value;
                green = value;
                blue  = value;
            }
            else
            {
                if( hue == 360 )
                    hue = 0.0;
                else
                    hue = hue / 60;

                int i = (int)System.Math.Truncate( hue );

                double f = hue - i;
                double p = value * (1.0 - saturation);
                double q = value * (1.0 - (saturation * f));
                double t = value * (1.0 - (saturation * (1.0 - f)));

                switch( i )
                {
                    case 0:
                        red = value;
                        green = t;
                        blue = p;
                        break;

                    case 1:
                        red = q;
                        green = value;
                        blue = p;
                        break;

                    case 2:
                        red = p;
                        green = value;
                        blue = t;
                        break;

                    case 3:
                        red = p;
                        green = q;
                        blue = value;
                        break;

                    case 4:
                        red = t;
                        green = p;
                        blue = value;
                        break;

                    default:
                        red = value;
                        green = p;
                        blue = q;
                        break;
                }
            }

            return Color.FromArgb( 255, (byte)(red * 255), (byte)(green * 255), (byte)(blue * 255) );
        }
    }
}
