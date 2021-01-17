// <copyright file="Config.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Configuration.Config class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Configuration
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using ConfigProperty = System.Tuple<System.Reflection.PropertyInfo, IConfigPropertyAttribute>;

    /// <summary>
    /// Represents an abstract base class of a configuration class.
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyConfig : Config
    /// {
    ///     [ConfigProperty(DefaultValue = 1024)]
    ///     public int Width
    ///     {
    ///         get;
    ///         set;
    ///     }
    ///     
    ///     [ConfigProperty(DefaultValue = "", StorageName = "name")]
    ///     public string FirstName
    ///     {
    ///         get;
    ///         set;
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class Config : IConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class,
        /// which uses a <see cref="ConfigPropertySearcher"/> and a <see cref="TypeStringConverter"/>.
        /// </summary>
        /// <param name="store">
        /// The store that manages loading and saving the configuration data.
        /// </param>
        protected Config( IConfigStore store )
            : this( store, new ConfigPropertySearcher(), new TypeStringConverter() )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        /// <param name="store">
        /// The store that manages loading and saving the configuration data.
        /// </param>
        /// <param name="propertySearcher">
        /// Provides a mechanism for searching the configuration properties of a Type.
        /// </param>
        /// <param name="converter">
        /// Provides a mechanism that converts between configuration properties and string values that
        /// are stored in the IConfigStore.
        /// </param>
        protected Config( IConfigStore store, IConfigPropertySearcher propertySearcher, IStringConverter converter )
        {
            Contract.Requires<ArgumentNullException>( store != null );
            Contract.Requires<ArgumentNullException>( propertySearcher != null );
            Contract.Requires<ArgumentNullException>( converter != null );

            this.store = store;
            this.propertySearcher = propertySearcher;
            this.converter = converter;
        }

        /// <summary>
        /// Loads the data stored in the <see cref="IConfigStore"/> into this <see cref="Config"/>.
        /// </summary>
        public void Load()
        {
            var properties = this.FindConfigProperties().ToArray();
            this.LoadDefaults( properties );
            this.LoadFromStore( properties );
        }

        /// <summary>
        /// Finds the configuration properties that have been defined on the sub-(classes) of this Config.
        /// </summary>
        /// <returns>
        /// The configuration properties that have been found.
        /// </returns>
        private IEnumerable<ConfigProperty> FindConfigProperties()
        {
            return this.propertySearcher.Search( this.GetType() );
        }

        /// <summary>
        /// Loads the default values of the config properties.
        /// </summary>
        public void LoadDefaults()
        {
            var properties = this.FindConfigProperties();
            this.LoadDefaults( properties );
        }

        /// <summary>
        /// Loads the default values of the config properties.
        /// </summary>
        /// <param name="properties">
        /// The configuration properties this type has.
        /// </param>
        private void LoadDefaults( IEnumerable<ConfigProperty> properties )
        {
            foreach( var property in properties )
            {
                var info = property.Item1;
                var config = property.Item2;

                this.SetPropertyValue( info, config.DefaultValue );
            }
        }

        /// <summary>
        /// Loads the data stored in the <see cref="IConfigStore"/> into this <see cref="Config"/>.
        /// </summary>
        /// <param name="properties">
        /// The configuration properties this type has.
        /// </param>
        private void LoadFromStore( IEnumerable<ConfigProperty> properties )
        {
            foreach( var storedProperty in this.store.Load() )
            {
                string storedPropertyName = storedProperty.Item1;
                string storedPropertyValue = storedProperty.Item2;

                var property = properties.FirstOrDefault(
                    p => GetStorageName( p ).Equals( storedPropertyName, StringComparison.OrdinalIgnoreCase )
                );

                if( property != null )
                {
                    var info = property.Item1;
                    try
                    {
                        object value = this.ParseValue( storedPropertyValue, info.PropertyType );
                        this.SetPropertyValue( info, value );
                    }
                    catch( OutOfMemoryException )
                    {
                        throw;
                    }
                    catch( StackOverflowException )
                    {
                        throw;
                    }
                    catch( Exception )
                    {
                        // ToDo: add logging.
                        continue;
                    }
                }                
            }
        }

        /// <summary>
        /// Sets the values of the specified property to the specified value.
        /// </summary>
        /// <param name="property">
        /// The PropertyInfo that descripes the property to set.
        /// </param>
        /// <param name="value">
        /// The value to set the property to.
        /// </param>
        private void SetPropertyValue( PropertyInfo property, object value )
        {
            property.SetValue( this, value, null );
        }

        /// <summary>
        /// Converts the specified string into an object of the specified type.
        /// </summary>
        /// <param name="valueString">
        /// The input string to convert.
        /// </param>
        /// <param name="propertyType">
        /// The output type.
        /// </param>
        /// <returns>
        /// The converted output value.
        /// </returns>
        private object ParseValue( string valueString, Type propertyType )
        {
            return this.converter.ConvertFromString( valueString, propertyType );
        }

        /// <summary>
        /// Gets the name under which the specified configuration property is stored.
        /// </summary>
        /// <param name="property">
        /// The configuration property to query.
        /// </param>
        /// <returns>
        /// The string that identifies the specified configuration property in the IConfigStore.
        /// </returns>
        private static string GetStorageName( ConfigProperty property )
        {
            var info = property.Item1;
            var config = property.Item2;

            return config.StorageName != null ? config.StorageName : info.Name;
        }

        /// <summary>
        /// Saves this Config to the <see cref="IConfigStore"/>.
        /// </summary>
        public void Save()
        {
            var properties = this.SerializeProperties();
            this.store.Save( properties );
        }

        /// <summary>
        /// Serializes the properties stored in this Config instance
        /// into a storeable format.
        /// </summary>
        /// <returns>
        /// The properties that have been serialized.
        /// </returns>
        private IEnumerable<Tuple<string, string, IConfigPropertyAttribute>> SerializeProperties()
        {
            foreach( var property in this.FindConfigProperties() )
            {
                string storageName;
                string encodedValue;

                try
                {
                    var info = property.Item1;
                    var value = info.GetValue( this, null );

                    storageName = GetStorageName( property );
                    encodedValue = this.converter.ConvertToString( value );
                }
                catch( OutOfMemoryException )
                {
                    throw;
                }
                catch( StackOverflowException )
                {
                    throw;
                }
                catch( Exception ex )
                {
                    Console.WriteLine(ex);
                    continue;
                }

                yield return Tuple.Create( storageName, encodedValue, property.Item2 );
            }
        }
        
        /// <summary>
        /// The store that manages loading and saving the configuration data.
        /// </summary>
        private readonly IConfigStore store;

        /// <summary>
        /// Provides a mechanism for searching the configuration properties of a Type.
        /// </summary>
        private readonly IConfigPropertySearcher propertySearcher;

        /// <summary>
        /// Provides a mechanism that converts between configuration properties and string values that
        /// are stored in the IConfigStore.
        /// </summary>
        private readonly IStringConverter converter;
    }
}