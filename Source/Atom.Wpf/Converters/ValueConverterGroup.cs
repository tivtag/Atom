// <copyright file="ValueConverterGroup.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.Converters.ValueConverterGroup class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Wpf.Converters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// A value converter which contains a list of <see cref="IValueConverter"/>s
    /// and invokes their Convert or ConvertBack methods in the order that they exist in the list.
    /// </summary>
    /// <remarks>
    /// The output of one converter is piped into the next converter,
    /// allowing for modular value converters to be chained together.
    /// If the ConvertBack method is invoked,
    /// the value converters are executed in reverse order (highest to lowest index).
    /// Do not leave an element in the Converters property collection null,
    /// every element must reference a valid <see cref="IValueConverter"/> instance.
    /// If a value converter's type is not decorated with the <see cref="ValueConversionAttribute"/>,
    /// an <see cref="InvalidOperationException"/> will be thrown when the converter
    /// is added to the <see cref="Converters"/> collection.
    /// </remarks>
    [System.Windows.Markup.ContentProperty( "Converters" )]
    public class ValueConverterGroup : IValueConverter
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the list of IValueConverters contained in this <see cref="ValueConverterGroup"/>.
        /// </summary>
        public ObservableCollection<IValueConverter> Converters
        {
            get { return this.converters; }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueConverterGroup"/> class.
        /// </summary>
        public ValueConverterGroup()
        {
            this.converters.CollectionChanged += this.OnConvertersCollectionChanged;
        }

        #endregion

        #region [ Methods ]

        #region > IValueConverter <

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture"> The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            int converterCount = this.converters.Count;
            object output = value;

            for( int i = 0; i < converterCount; ++i )
            {
                var converter         = this.Converters[i];
                var currentTargetType = this.GetTargetType( i, targetType, true );

                output = converter.Convert( output, currentTargetType, parameter, culture );

                // If the converter returns 'DoNothing' 
                // then the binding operation should terminate.
                if( output == Binding.DoNothing )
                    break;
            }

            return output;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> into radians.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            object output = value;

            for( int i = this.Converters.Count - 1; i >= 0; --i )
            {
                var converter         = this.Converters[i];
                var currentTargetType = this.GetTargetType( i, targetType, false );

                output = converter.ConvertBack( output, currentTargetType, parameter, culture );

                // When a converter returns 'DoNothing' 
                // then the binding operation should terminate.
                if( output == Binding.DoNothing )
                    break;
            }

            return output;
        }

        #endregion

        #region GetTargetType

        /// <summary>
        /// Returns the target type for a conversion operation.
        /// </summary>
        /// <param name="converterIndex">The index of the current converter about to be executed.</param>
        /// <param name="finalTargetType">The 'targetType' argument passed into the conversion method.</param>
        /// <param name="convert">Pass true if calling from the Convert method, or false if calling from ConvertBack.</param>
        /// <returns>The type to target for conversation.</returns>
        protected virtual Type GetTargetType( int converterIndex, Type finalTargetType, bool convert )
        {
            // If the current converter is not the last/first in the list, 
            // get a reference to the next/previous converter.
            IValueConverter nextConverter = null;

            if( convert )
            {
                if( converterIndex < this.Converters.Count - 1 )
                {
                    nextConverter = this.Converters[converterIndex + 1];

                    if( nextConverter == null )
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.CurrentUICulture,
                                 Atom.Wpf.Properties.Resources.Error_ValueConverterGroup_NullIndexX,
                                (converterIndex + 1).ToString( CultureInfo.CurrentCulture )
                            )
                        );
                    }
                }
            }
            else
            {
                if( converterIndex > 0 )
                {
                    nextConverter = this.Converters[converterIndex - 1];

                    if( nextConverter == null )
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.CurrentUICulture,
                                Atom.Wpf.Properties.Resources.Error_ValueConverterGroup_NullIndexX,
                                (converterIndex - 1).ToString( CultureInfo.CurrentCulture )
                            )
                        );
                    }
                }
            }

            if( nextConverter != null )
            {
                var conversionAttribute = cachedAttributes[nextConverter];

                // If the Convert method is going to be called, we need to use the SourceType of the next 
                // converter in the list.  If ConvertBack is called, use the TargetType.
                return convert ? conversionAttribute.SourceType : conversionAttribute.TargetType;
            }

            // If the current converter is the last one to be executed
            // return the target type passed into the conversion method.
            return finalTargetType;
        }

        #endregion

        #region OnConvertersCollectionChanged

        /// <summary>
        /// Gets called when the user has added or removed <see cref="IValueConverter"/> s
        /// to/from this <see cref="ValueConverterGroup"/>.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="NotifyCollectionChangedEventArgs"/> that contain the event data.
        /// </param>
        private void OnConvertersCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            // The 'Converters' collection has been modified, so validate that each value converter it now
            // contains is decorated with ValueConversionAttribute and then cache the attribute value.
            IList convertersToProcess = null;

            if( e.Action == NotifyCollectionChangedAction.Add ||
                e.Action == NotifyCollectionChangedAction.Replace )
            {
                convertersToProcess = e.NewItems;
            }
            else if( e.Action == NotifyCollectionChangedAction.Remove )
            {
                foreach( IValueConverter converter in e.OldItems )
                    this.cachedAttributes.Remove( converter );
            }
            else if( e.Action == NotifyCollectionChangedAction.Reset )
            {
                this.cachedAttributes.Clear();
                convertersToProcess = this.converters;
            }

            if( convertersToProcess != null && convertersToProcess.Count > 0 )
            {
                foreach( IValueConverter converter in convertersToProcess )
                {
                    object[] attributes = converter.GetType().GetCustomAttributes( typeof( ValueConversionAttribute ), false );
                    if( attributes.Length != 1 )
                        throw new InvalidOperationException( Atom.Wpf.Properties.Resources.Error_ValueConverterGroup_ConverterAttributeNeededOnce );

                    this.cachedAttributes.Add( converter, attributes[0] as ValueConversionAttribute );
                }
            }
        }

        #endregion

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The collection of <see cref="IValueConverter"/>s that have been added to this <see cref="ValueConverterGroup"/>.
        /// </summary>
        private readonly ObservableCollection<IValueConverter> converters =
            new ObservableCollection<IValueConverter>();

        /// <summary>
        /// The <see cref="ValueConversionAttribute"/> of the <see cref="IValueConverter"/>s
        /// that have been added of this <see cref="ValueConverterGroup"/>
        /// cached to reduce reflection-related lock-up time.
        /// </summary>
        private readonly Dictionary<IValueConverter, ValueConversionAttribute> cachedAttributes =
            new Dictionary<IValueConverter, ValueConversionAttribute>();

        #endregion
    }
}