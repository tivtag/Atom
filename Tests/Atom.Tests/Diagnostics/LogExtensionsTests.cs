// <copyright file="LogExtensionsTests.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Diagnostics.Tests.LogExtensionsTests class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Diagnostics.Tests
{
    using System;
    using System.Collections.Generic;
    using Atom.Diagnostics.Moles;
    using Microsoft.Pex.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the usage of the <see cref="LogExtensions"/> class.
    /// </summary>
    [TestClass]
    public sealed partial class LogExtensionsTests
    {
        [TestMethod]
        public void WriteFormat_CorrectlyWrites_FormattedString()
        {
            // Arrange
            object[] args = { "Cat", 12 };
            const string Format = "Hello, {0}. You are {1} years old?";
            const string ExpectedMessage = "Hello, Cat. You are 12 years old?";

            string writtenMessage = null;

            ILog log = new SILog() {
                DefaultSeverityGet = () => LogSeverities.Info,
                WriteLogSeveritiesString = ( severity, message ) => {
                    Assert.IsNull( writtenMessage );
                    writtenMessage = message;
                }
            };

            // Act
            log.Write( Format, args );

            // Assert
            Assert.AreEqual( ExpectedMessage, writtenMessage );
        }
        
        [TestMethod]
        public void WriteLineFormat_CorrectlyWrites_FormattedString()
        {
            // Arrange
            object[] args = { "Cat", 12 };
            const string Format = "Hello, {0}. You are {1} years old?";
            const string ExpectedMessage = "Hello, Cat. You are 12 years old?";

            string writtenMessage = null;

            ILog log = new SILog() {
                DefaultSeverityGet = () => LogSeverities.Info,
                WriteLineLogSeveritiesString = (severity, message) => {
                    Assert.IsNull( writtenMessage );
                    writtenMessage = message;
                }
            };

            // Act
            log.WriteLine( Format, args );

            // Assert
            Assert.AreEqual( ExpectedMessage, writtenMessage );
        }

        [TestMethod]
        public void WriteLineEnumerable_WithNullEnumerable_WritesNothing()
        {
            // Arrange
            ILog log = new SILog() {
                DefaultSeverityGet = () => LogSeverities.Info
            };

            // Act & Assert
            CustomAssert.DoesNotThrow( () => log.WriteLine( (IEnumerable<int>)null ) );
        }

        [TestMethod]
        public void WriteLineEnumerable_WithEnumerableWithNullEntries_WritesEmptyMessages()
        {
            var enumerable = new string[] { "Hey", null, null, "You" };
            var writtenMessages = new List<string>();

            // Arrange
            ILog log = new SILog() {
                DefaultSeverityGet = () => LogSeverities.Info,
                WriteLineLogSeveritiesString = (severity, message) => {
                    writtenMessages.Add( message );
                }
            };

            // Act
            log.WriteLine( enumerable );

            // Assert
            Assert.AreEqual( enumerable.Length, writtenMessages.Count );
            
            for( int i = 0; i < writtenMessages.Count; ++i )
            {
                Assert.AreEqual( enumerable[i] ?? string.Empty, writtenMessages[i] );
            }
        }

        [TestMethod]
        public void WriteEnumerable_WithNullEnumerable_WritesNothing()
        {
            // Arrange
            ILog log = new SILog()
            {
                DefaultSeverityGet = () => LogSeverities.Info
            };

            // Act & Assert
            CustomAssert.DoesNotThrow( () => log.Write( (IEnumerable<int>)null ) );
        }

        [TestMethod]
        public void WriteEnumerable_WithEnumerableWithNullEntries_WritesEmptyMessages()
        {
            var enumerable = new string[] { "Hey", null, null, "You" };
            var writtenMessages = new List<string>();

            // Arrange
            ILog log = new SILog()
            {
                DefaultSeverityGet = () => LogSeverities.Info,
                WriteLogSeveritiesString = ( severity, message ) =>
                {
                    writtenMessages.Add( message );
                }
            };

            // Act
            log.Write( enumerable );

            // Assert
            Assert.AreEqual( enumerable.Length, writtenMessages.Count );

            for( int i = 0; i < writtenMessages.Count; ++i )
            {
                Assert.AreEqual( enumerable[i] ?? string.Empty, writtenMessages[i] );
            }
        }

        [PexMethod]
        public void WriteLineEnumerable_WithDefaultSeverity_WritesEachElementInNewLine(
            [PexAssumeNotNull]int[] valuesToWrite, LogSeverities defaultLogSeverity )
        {
            // Assume
            PexAssume.EnumIsDefined( defaultLogSeverity );
            PexAssume.AreDistinctValues( valuesToWrite );

            // Arrange
            var writtenLines = new List<Tuple<LogSeverities,string>>();

            ILog log = new SILog() { 
                WriteLineLogSeveritiesString = (sev, message) => writtenLines.Add(Tuple.Create(sev, message)),
                DefaultSeverityGet = () => defaultLogSeverity
            };

            // Act
            log.WriteLine( valuesToWrite );

            // Assert
            Assert.AreEqual( valuesToWrite.Length, writtenLines.Count );

            for( int i = 0; i < writtenLines.Count; i++ )
            {
                Assert.AreEqual( defaultLogSeverity, writtenLines[i].Item1 );
                Assert.AreEqual( valuesToWrite[i].ToString(), writtenLines[i].Item2 );                
            }
        }
        
        [PexMethod]
        public void WriteLineEnumerable_WitSpecifiedSeverity_WritesEachElementInNewLine(
            [PexAssumeNotNull]int[] valuesToWrite, LogSeverities severity )
        {
            // Assume
            PexAssume.EnumIsDefined( severity );
            PexAssume.AreDistinctValues( valuesToWrite );

            // Arrange
            var writtenLines = new List<Tuple<LogSeverities, string>>();

            ILog log = new SILog() {
                WriteLineLogSeveritiesString = ( sev, message ) => writtenLines.Add( Tuple.Create( sev, message ) )
            };

            // Act
            log.WriteLine( severity, valuesToWrite );

            // Assert
            Assert.AreEqual( valuesToWrite.Length, writtenLines.Count );

            for( int i = 0; i < writtenLines.Count; i++ )
            {
                Assert.AreEqual( severity, writtenLines[i].Item1 );
                Assert.AreEqual( valuesToWrite[i].ToString(), writtenLines[i].Item2 );
            }
        }
        
        [PexMethod]
        public void WriteEnumerable_WithDefaultSeverity_WritesEachElementAfterAnother(
            [PexAssumeNotNull]int[] valuesWrite, LogSeverities defaultLogSeverity )
        {
            // Assume
            PexAssume.EnumIsDefined( defaultLogSeverity );
            PexAssume.AreDistinctValues( valuesWrite );

            // Arrange
            var writtenLine = "";

            ILog log = new SILog()
            {
                WriteLineLogSeveritiesString = (sev, message) => {
                    Assert.AreEqual( defaultLogSeverity, sev );
                    writtenLine += message;
                },
                DefaultSeverityGet = () => defaultLogSeverity
            };

            // Act
            log.WriteLine( valuesWrite );

            // Assert
            for( int i = 0; i < valuesWrite.Length; i++ )
            {
                CustomAssert.Contains( valuesWrite[i].ToString(), writtenLine );
            }
        }

        [PexMethod]
        public void WriteEnumerable_WithSpecifiedSeverity_WritesEachElementAfterAnother(
            [PexAssumeNotNull]int[] valuesWrite, LogSeverities logSeverity )
        {
            // Assume
            PexAssume.EnumIsDefined( logSeverity );
            PexAssume.AreDistinctValues( valuesWrite );

            // Arrange
            var writtenLine = "";

            ILog log = new SILog()
            {
                WriteLineLogSeveritiesString = ( sev, message ) =>
                {
                    Assert.AreEqual( logSeverity, sev );
                    writtenLine += message;
                }
            };

            // Act
            log.WriteLine( logSeverity, valuesWrite );

            // Assert
            for( int i = 0; i < valuesWrite.Length; i++ )
            {
                CustomAssert.Contains( valuesWrite[i].ToString(), writtenLine );
            }
        }
    }
}
