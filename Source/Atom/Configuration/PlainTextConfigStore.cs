// <copyright file="PlainTextConfigStore.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Configuration.PlainTextConfigStore class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Linq;

    /// <summary>
    /// Implements an <see cref="IConfigStore"/> that stores properties
    /// within a text file.
    /// </summary>
    /// <remarks>
    /// Format:
    /// -----------
    /// name : value
    /// one property per line.
    /// ------------
    /// #comment
    /// width: 1000
    /// height: 1200
    /// name: Gomez
    /// </remarks>
    public sealed class PlainTextConfigStore : IConfigStore
    {
        /// <summary>
        /// The character that identifies a line to contain a comment.
        /// </summary>
        private const char Comment = '\'';

        /// <summary>
        /// The character that seperates the name of a property and the value of a property: "name : value"
        /// </summary>
        private const char Seperator = ':';

        /// <summary>
        /// Initializes a new instance of the PlainTextConfigStore class.
        /// </summary>
        /// <param name="fileName">
        /// The full path of the text file to contains the configuration.
        /// </param>
        /// <param name="encoding">
        /// The encoding to use when reading or writing the store.
        /// </param>
        public PlainTextConfigStore( string fileName, Encoding encoding = null )
        {
            Contract.Requires<ArgumentNullException>( fileName != null );

            if( encoding == null )
            {
                encoding = Encoding.Default;
            }

            this.fileName = fileName;
            this.encoding = encoding;
        }

        /// <summary>
        /// Gets the properties that have been saved, by loading them from this IConfigStore.
        /// </summary>
        /// <returns>
        /// The properties that this IConfigStore contains; where the first string
        /// represents the name of the property and the second string the value of the property.
        /// </returns>
        public IEnumerable<Tuple<string, string>> Load()
        {
            foreach( string line in ReadLines() )
            {
                string trimmedLine = line.Trim();
                
                // ignore empty lines and comments
                if( trimmedLine.Length == 0 || trimmedLine[0] == Comment )
                {
                    continue;
                }

                // format
                // name: value
                string[] parts = trimmedLine.Split( Seperator );
                if( parts.Length != 2 )
                    continue;

                string name = parts[0].Trim();
                string value = parts[1].Trim();
                yield return Tuple.Create( name, value );
            }
        }

        /// <summary>
        /// Reads all configuration lines from the text file.
        /// </summary>
        /// <returns>
        /// The lines that have been read.
        /// </returns>
        private IEnumerable<string> ReadLines()
        {
            try
            {
                return File.ReadAllLines( this.fileName, this.encoding );
            }
            catch( FileNotFoundException )
            {
                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Saves the specified properties to this IConfigStore.
        /// </summary>
        /// <param name="properties">
        /// The properties to save in this IConfigStore; where the first string
        /// represents the name of the property and the second string the value of the property.
        /// </param>
        public void Save( IEnumerable<Tuple<string, string, IConfigPropertyAttribute>> properties )
        {
            File.WriteAllLines( 
                this.fileName,
                this.WriteLines( properties ), 
                this.encoding
            );
        }

        /// <summary>
        /// Converts the specified properties into a sequence of strings.
        /// </summary>
        /// <param name="properties">
        /// The properties to save in this IConfigStore; where the first string
        /// represents the name of the property and the second string the value of the property.
        /// </param>
        /// <returns>
        /// The properties that are written into the file.
        /// </returns>
        private IEnumerable<string> WriteLines( IEnumerable<Tuple<string, string, IConfigPropertyAttribute>> properties )
        {
            foreach( var property in properties )
            {
                string name = property.Item1;
                string value = property.Item2;
                IConfigPropertyAttribute config = property.Item3;

                bool hasComment = !string.IsNullOrWhiteSpace( config.Comment );

                if( hasComment  )
                {
                    yield return string.Empty;
                    yield return string.Format(
                        CultureInfo.InvariantCulture,
                        "{0} {1}",
                        Comment,
                        config.Comment
                    );
                }

                yield return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}{1} {2}",
                    name,
                    Seperator,
                    value
                );

                if( hasComment )
                {
                    yield return string.Empty;
                }
            }
        }

        /// <summary>
        /// The full path of the text file to contains the configuration.
        /// </summary>
        private readonly string fileName;

        /// <summary>
        /// The encoding to use when reading or writing the store.
        /// </summary>
        private readonly Encoding encoding;
    }
}
